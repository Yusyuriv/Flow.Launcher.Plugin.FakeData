using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Bogus;
using Flow.Launcher.Plugin.FakeData.Data;
using JetBrains.Annotations;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.FakeData;

internal record Command {
    private const string CommonArgs = "repeat:1 lang:en";
    public string Name { get; }
    public string Description { get; }
    public string Arguments { get; }

    public Func<string> GetOutput { get; }

    public Command(string name, Func<string> getOutput) : this(name, string.Empty, string.Empty, getOutput) {
    }

    public Command(string name, string description, Func<string> getOutput) : this(
        name, description, string.Empty, getOutput
    ) {
    }

    public Command(string name, string description, string arguments, Func<string> getOutput) {
        Name = name;
        Description = description;
        Arguments = "Allowed arguments: " + $"{arguments} {CommonArgs}".Trim();
        GetOutput = getOutput;
    }
}

[UsedImplicitly]
public partial class FakeData : IPlugin, IContextMenu {
    private const string Icon = "app.png";
    private PluginInitContext _context;
    private static readonly ParsedArgs ParsedArgs = new();
    private const string DateFormat = "yyyy-MM-dd";

    private static Faker F { get; set; } = new();

    private static readonly Dictionary<string, Command[]> Commands = new(StringComparer.OrdinalIgnoreCase) {
        {
            "Address", new Command[] {
                new("ZipCode", "0000A", "?#?#?", () => F.Address.ZipCode(ParsedArgs.Address.ZipCodeFormat)),
                new("City", "Rosenbaumport", () => F.Address.City()), new(
                    "StreetAddress", "2949 Very Fake", "full",
                    () => F.Address.StreetAddress(ParsedArgs.Address.UseFullAddress)
                ),
                new("CityPrefix", "New", () => F.Address.CityPrefix()),
                new("CitySuffix", "chester", () => F.Address.CitySuffix()),
                new("StreetName", "Very Fake", () => F.Address.StreetName()),
                new("BuildingNumber", "71778", () => F.Address.BuildingNumber()),
                new("StreetSuffix", "Alley", () => F.Address.StreetSuffix()),
                new("SecondaryAddress", "Apt. 9876", () => F.Address.SecondaryAddress()),
                new("County", "Bedfordshire", () => F.Address.County()),
                new("Country", "Finland", () => F.Address.Country()),
                new("FullAddress", "208 Fake Street, City, Country", () => F.Address.FullAddress()),
                new("CountryCode", "US", "2|3", () => F.Address.CountryCode(ParsedArgs.Address.CountryCodeFormat)),
                new("State", "New Jersey", () => F.Address.State()),
                new("StateAbbr", "IL", () => F.Address.StateAbbr()),
                new(
                    "Latitude", "33.7723004", "min:-90 max:90",
                    () => F.Address.Latitude(ParsedArgs.Address.LatitudeMin, ParsedArgs.Address.LatitudeMax)
                        .ToString(CultureInfo.InvariantCulture)
                ),
                new(
                    "Longitude", "-42.0504102", "min:-180 max:180",
                    () => F.Address.Longitude(ParsedArgs.Address.LongitudeMin, ParsedArgs.Address.LongitudeMax)
                        .ToString(CultureInfo.InvariantCulture)
                ),
                new("Direction", "North", "abbr", () => F.Address.Direction(ParsedArgs.Address.DirectionAbbreviation)),
                new(
                    "CardinalDirection", "North", "abbr",
                    () => F.Address.CardinalDirection(ParsedArgs.Address.DirectionAbbreviation)
                ),
                new(
                    "OrdinalDirection", "Northwest", "abbr",
                    () => F.Address.OrdinalDirection(ParsedArgs.Address.DirectionAbbreviation)
                ),
            }
        }, {
            "Commerce", new Command[] {
                new("Department", "Computers", () => F.Commerce.Department(1)), new(
                    "Price", "$19.99", "$ min:1 max:1000 decimals:2",
                    () => F.Commerce.Price(
                        ParsedArgs.Commerce.MinPrice, ParsedArgs.Commerce.MaxPrice, ParsedArgs.Commerce.Decimals,
                        ParsedArgs.Commerce.Symbol
                    )
                ),
                new("Categories", "Electronics", () => F.Commerce.Categories(1)[0]),
                new("ProductName", "Handmade Soft Shirt", () => F.Commerce.ProductName()),
                new("Color", "magenta", () => F.Commerce.Color()),
                new("Product", "Keyboard", () => F.Commerce.Product()),
                new("ProductAdjective", "Fantastic", () => F.Commerce.ProductAdjective()),
                new("ProductMaterial", "Wooden", () => F.Commerce.ProductMaterial()),
                new("Ean8", "00000000", () => F.Commerce.Ean8()),
                new("Ean13", "0000000000000", () => F.Commerce.Ean13()),
            }
        }, {
            "Company", new Command[] {
                new("Suffix", "LLC", () => F.Company.CompanySuffix()), new(
                    "Name", "Fake Non-Existent Company LLC", "0|1|2",
                    () => F.Company.CompanyName(ParsedArgs.Company.Format)
                ),
                new("CatchPhrase", "Focused static algorithm", () => F.Company.CatchPhrase()),
                new("BS", "integrate distributed models", () => F.Company.Bs()),
            }
        }, {
            "Database",
            new Command[] {
                new("Column", "updatedAt", () => F.Database.Column()), new("Type", "text", () => F.Database.Type()),
                new("Collation", "utf8_bin", () => F.Database.Collation()),
                new("Engine", "MEMORY", () => F.Database.Engine()),
            }
        }, {
            "Date", new Command[] {
                new(
                    "Past", "", "years:1", () => F.Date.Past(ParsedArgs.Date.Years).ToString(CultureInfo.CurrentCulture)
                ),
                new(
                    "PastOffset", "", "years:1",
                    () => F.Date.PastOffset(ParsedArgs.Date.Years).ToString(CultureInfo.CurrentCulture)
                ),
                new("Soon", "", "days:1", () => F.Date.Soon(ParsedArgs.Date.Days).ToString(CultureInfo.CurrentCulture)),
                new(
                    "SoonOffset", "", "days:1",
                    () => F.Date.SoonOffset(ParsedArgs.Date.Days).ToString(CultureInfo.CurrentCulture)
                ),
                new(
                    "Future", "", "years:1",
                    () => F.Date.Future(ParsedArgs.Date.Years).ToString(CultureInfo.CurrentCulture)
                ),
                new(
                    "FutureOffset", "", "years:1",
                    () => F.Date.FutureOffset(ParsedArgs.Date.Years).ToString(CultureInfo.CurrentCulture)
                ),
                new(
                    "Between", "", "from:2020-01-01 to:2021-01-01",
                    () => F.Date.Between(ParsedArgs.Date.From, ParsedArgs.Date.To).ToString(DateFormat)
                ),
                new(
                    "BetweenOffset", "", "from:2020-01-01 to:2021-01-01",
                    () => F.Date.BetweenOffset(ParsedArgs.Date.From, ParsedArgs.Date.To).ToString(DateFormat)
                ),
                new(
                    "Recent", "", "days:1",
                    () => F.Date.Recent(ParsedArgs.Date.Days).ToString(CultureInfo.CurrentCulture)
                ),
                new(
                    "RecentOffset", "", "days:1",
                    () => F.Date.RecentOffset(ParsedArgs.Date.Days).ToString(CultureInfo.CurrentCulture)
                ),
                new("Timespan", "2.02:28:02.0860690", () => F.Date.Timespan().ToString()),
                new("Month", "May", "abbr:true", () => F.Date.Month(ParsedArgs.Date.Abbreviate)),
                new("Weekday", "Tuesday", "abbr:true", () => F.Date.Weekday(ParsedArgs.Date.Abbreviate)),
            }
        }, {
            "Finance", new Command[] {
                new("Account", "00000000", "8", () => F.Finance.Account(ParsedArgs.Finance.Length)),
                new("AccountName", "Savings Account", () => F.Finance.AccountName()), new(
                    "Amount", "525.20", "min:0 max:1000 decimals:2",
                    () => F.Finance.Amount(ParsedArgs.Finance.Min, ParsedArgs.Finance.Max, ParsedArgs.Finance.Decimals)
                        .ToString(CultureInfo.InvariantCulture)
                ),
                new("TransactionType", "invoice", () => F.Finance.TransactionType()),
                new("Currency", "USD", "fund", () => F.Finance.Currency(ParsedArgs.Finance.IncludeFundCodes).Code), new(
                    "CreditCardNumber", "0000-0000-0000-0000",
                    "visa|mastercard|discover|amex|diners|jcb|instapayment|laser|solo|maestro|switch",
                    () => F.Finance.CreditCardNumber(ParsedArgs.Finance.CardType)
                ),
                new("CreditCardCvv", "000", () => F.Finance.CreditCardCvv()),
                new("BitcoinAddress", "0000000000000000000000000000000", () => F.Finance.BitcoinAddress()),
                new("EthereumAddress", "0x0000000000000000000000000000000000000000", () => F.Finance.EthereumAddress()),
                new("RoutingNumber", "000000000", () => F.Finance.RoutingNumber()),
                new("BIC", "AAAAAAAA", () => F.Finance.Bic()),
                new("IBAN", "AAAAAAAAAAAAAAAAAAAAAAAA", () => F.Finance.Iban()),
            }
        }, {
            "Hacker",
            new Command[] {
                new("Abbreviation", "XML", () => F.Hacker.Abbreviation()),
                new("Adjective", "wireless", () => F.Hacker.Adjective()),
                new("Noun", "interface", () => F.Hacker.Noun()),
                new("Verb", "override", () => F.Hacker.Verb()), new("IngVerb", "indexing", () => F.Hacker.IngVerb()),
                new("Phrase", "We need to calculate the neural XML array!", () => F.Hacker.Phrase()),
            }
        }, {
            "Images", new Command[] {
                new(
                    "DataUri", "data:image/svg+xml;charset=UTF-8,...", "w:640 h:480 color:grey",
                    () => F.Image.DataUri(ParsedArgs.Images.Width, ParsedArgs.Images.Height, ParsedArgs.Images.Color)
                ),
                new(
                    "PicsumUrl", "https://picsum.photos/640/480/?image=1", "w:640 h:480 gray:true blur:true",
                    () => F.Image.PicsumUrl(
                        ParsedArgs.Images.Width, ParsedArgs.Images.Height, ParsedArgs.Images.Greyscale,
                        ParsedArgs.Images.Blur
                    )
                ),
                new(
                    "LoremFlickrUrl", "https://loremflickr.com/320/240?lock=1", "w:640 h:480",
                    () => F.Image.LoremFlickrUrl(
                        ParsedArgs.Images.Width, ParsedArgs.Images.Height, ParsedArgs.Images.Keywords,
                        ParsedArgs.Images.Greyscale, ParsedArgs.Images.Blur
                    )
                ),
            }
        }, {
            "Internet", new Command[] {
                new("Avatar", () => F.Internet.Avatar()), new(
                    "Email", "john.doe38@example.com", "firstName:john lastName:doe provider:example.com suffix:unique",
                    () => F.Internet.Email(
                        ParsedArgs.Internet.FirstName, ParsedArgs.Internet.LastName, ParsedArgs.Internet.Provider,
                        ParsedArgs.Internet.Suffix
                    )
                ),
                new(
                    "ExampleEmail", "john.doe38@example.com", "firstName:john lastName:doe",
                    () => F.Internet.ExampleEmail(ParsedArgs.Internet.FirstName, ParsedArgs.Internet.LastName)
                ),
                new(
                    "Username", "john.doe38", "firstName:john lastName:doe",
                    () => F.Internet.UserName(ParsedArgs.Internet.FirstName, ParsedArgs.Internet.LastName)
                ),
                new(
                    "UsernameUnicode", "john.doe38", "firstName:john lastName:doe",
                    () => F.Internet.UserNameUnicode(ParsedArgs.Internet.FirstName, ParsedArgs.Internet.LastName)
                ),
                new("DomainName", "example.com", () => F.Internet.DomainName()),
                new("DomainWord", "example", () => F.Internet.DomainWord()),
                new("DomainSuffix", "com", () => F.Internet.DomainSuffix()),
                new("IP", "127.0.0.1", () => F.Internet.Ip()),
                new("Port", "80", () => F.Internet.Port().ToString()),
                new("IPEndpoint", "127.0.0.1:80", () => F.Internet.IpEndPoint().ToString()),
                new("IPv6", "0000:0000:0000:0000:0000:0000:0000:0001", () => F.Internet.Ipv6()), new(
                    "IPv6Endpoint", "[0000:0000:0000:0000:0000:0000:0000:0001]:80",
                    () => F.Internet.Ipv6EndPoint().ToString()
                ),
                new(
                    "UserAgent", "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:15.9) Gecko/20100101 Firefox/15.9.1",
                    () => F.Internet.UserAgent()
                ),
                new("Mac", "00:00:00:00:00:00", "sep::", () => F.Internet.Mac(ParsedArgs.Internet.Separator)), new(
                    "Password", "xB0ursmeZK", "length:10 memorable:true",
                    () => F.Internet.Password(ParsedArgs.Internet.Length, ParsedArgs.Internet.Memorable)
                ),
                new(
                    "Color", "#712a73", "hex|rgb red:0 green:0 blue:0 gray:true",
                    () => F.Internet.Color(
                        ParsedArgs.Internet.Red, ParsedArgs.Internet.Green, ParsedArgs.Internet.Blue,
                        ParsedArgs.Internet.Grayscale, ParsedArgs.Internet.Format
                    )
                ),
                new("Protocol", "https", () => F.Internet.Protocol()),
                new("Url", "https://example.com", () => F.Internet.Url()), new(
                    "UrlWithPath", "https://example.com/file/path", "protocol:https domain:example.com ext:png",
                    () => F.Internet.UrlWithPath(
                        ParsedArgs.Internet.Protocol, ParsedArgs.Internet.Domain, ParsedArgs.Internet.FileExtension
                    )
                ),
                new(
                    "UrlRootedPath", "/file/path", "ext:png",
                    () => F.Internet.UrlRootedPath(ParsedArgs.Internet.FileExtension)
                ),
            }
        }, {
            "Lorem", new Command[] {
                new("Word", "lorem", () => F.Lorem.Word()), new(
                    "Sentence",
                    "Beatae est et earum magnam voluptatem id aperiam velit laboriosam mollitia possimus voluptatem.",
                    "15 range:10", () => F.Lorem.Sentence(ParsedArgs.Lorem.Words, ParsedArgs.Lorem.Range)
                ),
                new(
                    "Paragraph",
                    "Nobis quos debitis unde ut quae facere. Consequatur sit ipsam ex enim ex in commodi quisquam ut. Fugit suscipit et quo et.",
                    "5", () => F.Lorem.Paragraph(ParsedArgs.Lorem.Sentences)
                ),
                new(
                    "Text",
                    "Quam aut sint autem maiores est minus sequi incidunt et. Et dolore molestiae eos porro et dolor in et. Inventore sint veniam numquam quasi consequuntur laboriosam iure iure accusantium. Nisi quis ratione sit sint.",
                    () => F.Lorem.Text()
                ),
                new(
                    "Lines", "Sit commodi enim aliquid a eveniet vitae ipsum assumenda.", "3",
                    () => F.Lorem.Lines(ParsedArgs.Lorem.Lines)
                ),
                new("Slug", "tempora-quaerat-quia", "3", () => F.Lorem.Slug(ParsedArgs.Lorem.Words)),
            }
        }, {
            "Name",
            new Command[] {
                new("Full", "John Doe", "m|f", () => F.Name.FullName(ParsedArgs.Name.Gender)),
                new("First", "John", "m|f", () => F.Name.FirstName(ParsedArgs.Name.Gender)),
                new("Last", "Doe", "m|f", () => F.Name.LastName(ParsedArgs.Name.Gender)),
                new("Prefix", "Mr.", "m|f", () => F.Name.Prefix(ParsedArgs.Name.Gender)),
                new("Suffix", "Jr.", () => F.Name.Suffix()),
            }
        }, {
            "Job",
            new Command[] {
                new("Title", "Legacy Accountability Planner", () => F.Name.JobTitle()),
                new("Descriptor", "Legacy", () => F.Name.JobDescriptor()),
                new("Area", "Research", () => F.Name.JobArea()),
                new("Type", "Assistant", () => F.Name.JobType()),
            }
        }, {
            "Phone",
            new Command[] {
                new("Number", "+1 555 0100", "+1 555 01##", () => F.Phone.PhoneNumber(ParsedArgs.Phone.Format)),
                new("NumberFormat", "+1 555 0100", "0-19", () => F.Phone.PhoneNumberFormat(ParsedArgs.Phone.Index)),
            }
        }, {
            "Rant", new Command[] {
                new(
                    "Review", "This product works quite well. It pointedly improves my golf by a lot.", "product name",
                    () => F.Rant.Review(ParsedArgs.Common.Search)
                ),
            }
        }, {
            "System", new Command[] {
                new("FileName", "fresh_multimedia.vtt", "extension", () => F.System.FileName(ParsedArgs.Common.Search)),
                new("DirectoryPath", "/etc", () => F.System.DirectoryPath()),
                new("FilePath", "/home/user/image.png", () => F.System.FilePath()),
                new("CommonFileName", "rand.gif", "extension", () => F.System.CommonFileName()),
                new("MimeType", "image/svg+xml", () => F.System.MimeType()),
                new("CommonFileType", "text", () => F.System.CommonFileType()),
                new("CommonFileExt", "jpg", () => F.System.CommonFileExt()),
                new("FileType", "image", () => F.System.FileType()),
                new("FileExt", "html", "mime", () => F.System.FileExt(ParsedArgs.Common.Search)),
                new("Semver", "1.0.0", () => F.System.Semver()),
                new("Version", "6.0.7.3", () => F.System.Version().ToString()), new(
                    "Exception",
                    "System.ArithmeticException: Frozen clear-thinking Ergonomic Granite Gloves\n   at Bogus.DataSets.System.Exception()",
                    () => F.System.Exception().ToString()
                ),
                new("AndroidId", () => F.System.AndroidId()), new("ApplePushToken", () => F.System.ApplePushToken()),
                new("BlackberryPint", () => F.System.BlackBerryPin()),
            }
        }, {
            "Vehicle",
            new Command[] {
                new("Vin", "AAAAA0AAAAA000000", "strict", () => F.Vehicle.Vin(ParsedArgs.Vehicle.Strict)),
                new("Manufacturer", "Audi", () => F.Vehicle.Manufacturer()),
                new("Model", "A4", () => F.Vehicle.Model()),
                new("Type", "SUV", () => F.Vehicle.Type()), new("Fuel", "Electric", () => F.Vehicle.Fuel()),
            }
        }, {
            "Random", new Command[] {
                new(
                    "Number", "15", "min:0 max:100",
                    () => F.Random.Number(ParsedArgs.Random.MinInt, ParsedArgs.Random.MaxInt).ToString()
                ),
                new(
                    "Even", "12", "min:0 max:100",
                    () => F.Random.Even(ParsedArgs.Random.MinInt, ParsedArgs.Random.MaxInt).ToString()
                ),
                new(
                    "Odd", "15", "min:0 max:100",
                    () => F.Random.Odd(ParsedArgs.Random.MinInt, ParsedArgs.Random.MaxInt).ToString()
                ),
                new(
                    "Double", "0.3113012942344838", "min:0 max:100",
                    () => F.Random.Double(ParsedArgs.Random.MinDouble, ParsedArgs.Random.MaxDouble)
                        .ToString(CultureInfo.InvariantCulture)
                ),
                new(
                    "Decimal", "0.9432828563923850", "min:0 max:100",
                    () => F.Random.Decimal(ParsedArgs.Random.MinDecimal, ParsedArgs.Random.MaxDecimal)
                        .ToString(CultureInfo.InvariantCulture)
                ),
                new(
                    "Float", "0.8372593", "min:0 max:100",
                    () => F.Random.Float(ParsedArgs.Random.MinFloat, ParsedArgs.Random.MaxFloat)
                        .ToString(CultureInfo.InvariantCulture)
                ),
                new(
                    "Byte", "221", "min:0 max:255",
                    () => F.Random.Byte(ParsedArgs.Random.MinByte, ParsedArgs.Random.MaxByte).ToString()
                ),
                new(
                    "SByte", "43", "min:-128 max:127",
                    () => F.Random.SByte(ParsedArgs.Random.MinSByte, ParsedArgs.Random.MaxSByte).ToString()
                ),
                new(
                    "Int", "417442315", "min:0 max:100",
                    () => F.Random.Int(ParsedArgs.Random.MinInt, ParsedArgs.Random.MaxInt).ToString()
                ),
                new(
                    "UInt", "1220010018", "min:0 max:100",
                    () => F.Random.UInt(ParsedArgs.Random.MinUInt, ParsedArgs.Random.MaxUInt).ToString()
                ),
                new(
                    "ULong", "11167613944573237248", "min:0 max:100",
                    () => F.Random.ULong(ParsedArgs.Random.MinULong, ParsedArgs.Random.MaxULong).ToString()
                ),
                new(
                    "Long", "-6393864994783639112", "min:0 max:100",
                    () => F.Random.Long(ParsedArgs.Random.MinLong, ParsedArgs.Random.MaxLong).ToString()
                ),
                new(
                    "Short", "-2572", "min:0 max:100",
                    () => F.Random.Short(ParsedArgs.Random.MinShort, ParsedArgs.Random.MaxShort).ToString()
                ),
                new(
                    "UShort", "3497", "min:0 max:100",
                    () => F.Random.UShort(ParsedArgs.Random.MinUShort, ParsedArgs.Random.MaxUShort).ToString()
                ),
                new("Char", "잭", () => F.Random.Char().ToString()), new(
                    "String2", "ahjq", "abcdefghijklmnopqrstuvwxyz length:4",
                    () => F.Random.String2(ParsedArgs.Random.Length, ParsedArgs.Common.Search)
                ),
                new(
                    "Hash", "3b85cc3035828c6d5ffedcf8eeccce71040c99e7", "upper length:40",
                    () => F.Random.Hash(ParsedArgs.Random.Length, ParsedArgs.Random.Uppercase)
                ),
                new("Bool", "False", () => F.Random.Bool().ToString()), new(
                    "ReplaceNumbers", "a1b2c3", "string symbol:#",
                    () => F.Random.ReplaceNumbers(ParsedArgs.Common.Search, ParsedArgs.Random.Symbol)
                ),
                new("Replace", "283QED4", "###???***", () => F.Random.Replace(ParsedArgs.Common.Search)), new(
                    "ClampString", "hello nhgfdg", "string min:1 max:24",
                    () => F.Random.ClampString(
                        ParsedArgs.Common.Search, ParsedArgs.Random.MinInt, ParsedArgs.Random.MaxInt
                    )
                ),
                new("Word", "Incredible", () => F.Random.Word()),
                new("GUID", "126e3ffb-432e-2205-b832-7fbfb4d0db23", () => F.Random.Guid().ToString()),
                new("UUID", "65179c30-d336-207f-202d-e8891ccbb4f0", () => F.Random.Uuid().ToString()),
                new("Locale", "en_US", () => F.Random.RandomLocale()),
                new("AlphaNumeric", "l1jn5", "5", () => F.Random.AlphaNumeric(ParsedArgs.Random.Length)), new(
                    "Hexadecimal", "0xff", "0x length:2",
                    () => F.Random.Hexadecimal(ParsedArgs.Random.Length, ParsedArgs.Random.Prefix)
                ),
            }
        },
        { "Custom", new Command[] { new("Custom", () => F.Parse(ParsedArgs.Common.Search)) } },
    };

    public void Init(PluginInitContext context) {
        _context = context;
    }

    public List<Result> Query(Query query) {
        // When search is empty
        if (!query.SearchTerms.Any())
            return Commands.Select(
                v => new Result {
                    Title = v.Key,
                    SubTitle = string.Join(", ", v.Value.Select(command => command.Name)),
                    IcoPath = Icon,
                }
            ).ToList();

        // When search has exactly one word, and it doesn't match the category name
        if (
            query.SearchTerms.Length == 1 &&
            !Commands.Any(
                v =>
                    string.Equals(v.Key, query.SearchTerms[0], StringComparison.CurrentCultureIgnoreCase)
            )
        )
            return Commands
                .Where(v => _context.API.FuzzySearch(query.SearchTerms[0], v.Key).Success)
                .Select(
                    v => new Result {
                        Title = v.Key,
                        SubTitle = string.Join(", ", v.Value.Select(command => command.Name)),
                        IcoPath = Icon,
                    }
                )
                .ToList();

        // When search has exactly one word, and it matches the category name
        if (query.SearchTerms.Length == 1)
            return Commands[query.SearchTerms[0]]
                .Select(
                    v => new Result {
                        Title = $"{query.SearchTerms[0]} {v.Name}", SubTitle = v.Description, IcoPath = Icon,
                    }
                )
                .ToList();

        // When search has exactly two words, and the second word doesn't match the command name
        if (
            query.SearchTerms.Length == 2 &&
            !Commands.Any(
                kv =>
                    kv.Value.Any(
                        v =>
                            string.Equals(v.Name, query.SearchTerms[1], StringComparison.CurrentCultureIgnoreCase)
                    )
            )
        )
            return Commands[query.SearchTerms[0]]
                .Where(v => _context.API.FuzzySearch(query.SearchTerms[1], v.Name).Success)
                .Select(
                    v => new Result {
                        Title = $"{query.SearchTerms[0]} {v.Name}", SubTitle = v.Description, IcoPath = Icon,
                    }
                )
                .ToList();

        var argsAfterCommands = string.Join(" ", query.SearchTerms.Skip(2).ToArray());

        ParsedArgs.Search = argsAfterCommands;

        if (F.Locale != ParsedArgs.Common.Locale)
            F = new Faker(ParsedArgs.Common.Locale);

        var section = query.SearchTerms[0];
        var subsection = query.SearchTerms[1];

        var cb = Commands[section].FirstOrDefault(v => v.Name.Equals(subsection, StringComparison.OrdinalIgnoreCase))
            ?.GetOutput;

        var results = cb is not null ? GetResults(cb, ParsedArgs.Common.Repeat) : new List<Result>();
        if (results.Count == 0) return results;

        var command = Commands[section].First(v => v.Name.Equals(subsection, StringComparison.OrdinalIgnoreCase));
        results[0].SubTitle = command.Arguments;
        return results;
    }

    private List<Result> GetResults(Func<string> getResult, int repeat = 1, int max = 10) {
        var results = new List<Result>(max);
        var regex = ImageDimensionsRegex();
        for (var i = 0; i < max; i++) {
            var list = new List<string>(repeat);
            for (var j = 0; j < repeat; j++) list.Add(getResult());

            string icoPath = null;
            if (list.Count > 0)
                if (
                    list[0].StartsWith("https://picsum.photos/") ||
                    list[0].StartsWith("https://loremflickr.com/") ||
                    (list[0].StartsWith("https://") && (list[0].EndsWith(".jpg") || list[0].EndsWith(".png")))
                )
                    icoPath = list[0];
            results.Add(
                new Result {
                    Title = string.Join(", ", list),
                    IcoPath = icoPath is null ? Icon : regex.Replace(icoPath, "/32/32/"),
                    Preview = icoPath is null
                        ? new Result.PreviewInfo()
                        : new Result.PreviewInfo { PreviewImagePath = icoPath, IsMedia = true },
                    ContextData = list,
                    Action = _ => {
                        _context.API.CopyToClipboard(string.Join(", ", list), showDefaultNotification: false);
                        return true;
                    },
                }
            );
        }

        return results;
    }

    public List<Result> LoadContextMenus(Result selectedResult) {
        if (selectedResult is not { ContextData: List<string> list }) return new List<Result>();

        if (list.Count == 1)
            return new List<Result> {
                new() {
                    Title = "Copy to clipboard",
                    IcoPath = Icon,
                    Action = _ => {
                        _context.API.CopyToClipboard(list[0], showDefaultNotification: false);
                        return false;
                    },
                },
            };

        return new List<Result> {
            new() {
                Title = "Copy to clipboard, comma-separated",
                IcoPath = Icon,
                Action = _ => {
                    _context.API.CopyToClipboard(string.Join(", ", list), showDefaultNotification: false);
                    return true;
                },
            },
            new() {
                Title = "Copy to clipboard, newline-separated",
                IcoPath = Icon,
                Action = _ => {
                    _context.API.CopyToClipboard(string.Join("\n", list), showDefaultNotification: false);
                    return true;
                },
            },
            new() {
                Title = "Copy to clipboard, space-separated",
                IcoPath = Icon,
                Action = _ => {
                    _context.API.CopyToClipboard(string.Join(" ", list), showDefaultNotification: false);
                    return true;
                },
            },
            new() {
                Title = "Copy to clipboard as JSON",
                IcoPath = Icon,
                Action = _ => {
                    var json = System.Text.Json.JsonSerializer.Serialize(list);
                    _context.API.CopyToClipboard(json, showDefaultNotification: false);
                    return true;
                },
            },
        };
    }

    [GeneratedRegex(@"/\d+/\d+/")]
    private static partial Regex ImageDimensionsRegex();
}
