using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Flow.Launcher.Plugin.FakeData.Data.Attributes;

namespace Flow.Launcher.Plugin.FakeData.Data.Parser;

/// <summary>
/// Represents a parser that parses a search string into key-value pairs.
/// </summary>
public static class Parser {
    /// <summary>
    /// Parses a search string and returns a dictionary of key-value pairs.
    /// </summary>
    /// <param name="searchString">The search string to parse.</param>
    /// <returns>A dictionary containing the parsed key-value pairs.</returns>
    private static Dictionary<string, string> ParseSearchString(string searchString) {
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        var search = new List<string>();

        var terms = searchString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i < terms.Length; i++) {
            var term = terms[i];
            if (term.Contains(':')) {
                var parts = term.Split(':', 2);
                if (parts.Length != 2) continue;
                if (!parts[1].StartsWith('"')) {
                    result[parts[0]] = parts[1];
                } else {
                    var value = parts[1][1..];
                    if (value.EndsWith('"')) {
                        result[parts[0]] = value[..^1];
                        continue;
                    }

                    for (var j = i + 1; j < terms.Length; j++) {
                        value += " " + terms[j];
                        if (terms[j].EndsWith('"')) {
                            value = value[..^1];
                            result[parts[0]] = value;
                            i = j;
                            break;
                        }

                        if (j != terms.Length - 1) continue;
                        result[parts[0]] = parts[1];
                        break;
                    }
                }
            } else {
                search.Add(term);
            }
        }

        result.Add("__search__", string.Join(" ", search));

        return result;
    }

    /// <summary>
    /// Parses a search string and returns an object of type T containing the parsed key-value pairs.
    /// </summary>
    /// <typeparam name="T">The type of object to parse the search string into.</typeparam>
    /// <param name="terms">The search string to parse.</param>
    /// <returns>An object of type T with the parsed key-value pairs.</returns>
    public static T Parse<T>(string terms) where T : new() {
        var data = new T();
        var args = ParseSearchString(terms);
        args.TryGetValue("__search__", out var search);
        search ??= "";
        var searchTerms = search.Split(" ");
        var properties = typeof(T).GetProperties();
        var methods = typeof(T).GetMethods();
        foreach (var property in properties) {
            string value = null;
            var renameAttributes = property.GetCustomAttributes(typeof(RenameAttribute), true)
                as RenameAttribute[] ?? Array.Empty<RenameAttribute>();
            var positionalAttribute = property.GetCustomAttributes(typeof(PositionalAttribute), true)
                as PositionalAttribute[] ?? Array.Empty<PositionalAttribute>();
            var fullSearchAttribute = property.GetCustomAttributes(typeof(FullSearchAttribute), true)
                as FullSearchAttribute[] ?? Array.Empty<FullSearchAttribute>();

            if (positionalAttribute.Length > 0)
                if (positionalAttribute[0].Position < searchTerms.Length)
                    value = searchTerms[positionalAttribute[0].Position];
            if (string.IsNullOrEmpty(value) && fullSearchAttribute.Length > 0) value = search;
            if (string.IsNullOrEmpty(value) && renameAttributes.Length > 0)
                foreach (var name in renameAttributes[0].NewNames)
                    if (args.TryGetValue(name, out value))
                        break;
            if (string.IsNullOrEmpty(value)) args.TryGetValue(property.Name, out value);
            if (string.IsNullOrEmpty(value)) continue;

            var method = methods.FirstOrDefault(v => v.Name == $"Set{property.Name}");

            if (method is not null && method.IsPublic) {
                var arguments = method.GetParameters();
                if (arguments.Length == 1 && arguments[0].ParameterType == typeof(string))
                    method.Invoke(data, new object[] { value });
            } else {
                if (property.PropertyType == typeof(bool)) {
                    property.SetValue(data, ParseBool(value));
                    continue;
                }

                var converter = TypeDescriptor.GetConverter(property.PropertyType);
                if (!converter.CanConvertFrom(typeof(string))) continue;
                try {
                    property.SetValue(data, converter.ConvertFrom(value));
                } catch {
                    // ignored
                }
            }
        }

        return data;
    }

    /// <summary>
    /// Parses a string value into a boolean.
    /// </summary>
    /// <param name="value">The string value to parse.</param>
    /// <returns>A boolean value representing the parsed result.</returns>
    public static bool ParseBool(string value) {
        return value.ToLower() switch {
            "true" or "t" or "1" or "yes" or "y" or "on" or "enable" or "e" or "+" => true,
            _ => false,
        };
    }
}
