using Flow.Launcher.Plugin.FakeData.Data.DataClasses;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.FakeData.Data;

public class ParsedArgs {
    private CommonFakeData _commonFakeData;
    private AddressFakeData _addressFakeData;
    private CommerceFakeData _commerceFakeData;
    private CompanyFakeData _companyFakeData;
    private DateFakeData _dateFakeData;
    private FinanceFakeData _financeFakeData;
    private NameFakeData _nameFakeData;
    private PhoneFakeData _phoneFakeData;
    private ImagesFakeData _imagesFakeData;
    private InternetFakeData _internetFakeData;
    private LoremFakeData _loremFakeData;
    private VehicleFakeData _vehicleFakeData;
    private RandomFakeData _randomFakeData;
    private string _search = "";

    /// <summary>
    /// Gets or sets the search keyword. When this property is set, all the cached fake data is cleared.
    /// </summary>
    public string Search {
        get => _search;
        set {
            if (_search == value) return;
            _search = value;
            _commonFakeData = null;
            _addressFakeData = null;
            _commerceFakeData = null;
            _companyFakeData = null;
            _dateFakeData = null;
            _financeFakeData = null;
            _nameFakeData = null;
            _phoneFakeData = null;
            _imagesFakeData = null;
            _internetFakeData = null;
            _loremFakeData = null;
            _vehicleFakeData = null;
            _randomFakeData = null;
        }
    }

    public CommonFakeData Common => _commonFakeData ??= Parser.Parser.Parse<CommonFakeData>(Search);

    public AddressFakeData Address => _addressFakeData ??= Parser.Parser.Parse<AddressFakeData>(Search);
    public CommerceFakeData Commerce => _commerceFakeData ??= Parser.Parser.Parse<CommerceFakeData>(Search);
    public CompanyFakeData Company => _companyFakeData ??= Parser.Parser.Parse<CompanyFakeData>(Search);
    public DateFakeData Date => _dateFakeData ??= Parser.Parser.Parse<DateFakeData>(Search);
    public FinanceFakeData Finance => _financeFakeData ??= Parser.Parser.Parse<FinanceFakeData>(Search);
    public NameFakeData Name => _nameFakeData ??= Parser.Parser.Parse<NameFakeData>(Search);
    public PhoneFakeData Phone => _phoneFakeData ??= Parser.Parser.Parse<PhoneFakeData>(Search);
    public ImagesFakeData Images => _imagesFakeData ??= Parser.Parser.Parse<ImagesFakeData>(Search);
    public InternetFakeData Internet => _internetFakeData ??= Parser.Parser.Parse<InternetFakeData>(Search);
    public LoremFakeData Lorem => _loremFakeData ??= Parser.Parser.Parse<LoremFakeData>(Search);
    public VehicleFakeData Vehicle => _vehicleFakeData ??= Parser.Parser.Parse<VehicleFakeData>(Search);
    public RandomFakeData Random => _randomFakeData ??= Parser.Parser.Parse<RandomFakeData>(Search);
}