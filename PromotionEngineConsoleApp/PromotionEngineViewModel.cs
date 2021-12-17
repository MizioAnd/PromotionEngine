using Promotion.Engine.Library;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PromotionEngineAPI.Models;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http.Headers;

namespace Promotion.Engine.ConsoleApp;
public class PromotionEngineViewModel : INotifyPropertyChanged
{
    private List<PromotionRule> _promotionRules = new List<PromotionRule>();
    // private IEnumerable<string>? stockKeepingUnits;
    // private IEnumerable<int>? _counts;
    private int _totalPrice;
    private static string? _input;
    private HttpClient _client;
    private string _url;

    public PromotionEngineViewModel()
    {
        _totalPrice = 0;
        // PropertyChanged += new PropertyChangedEventHandler(this.ComputeTotalPriceFor3RulesAsync);
        PropertyChanged += async (s, e) => await ComputeTotalPriceFor3RulesAsync(s, e);
        PropertyChanged += new PropertyChangedEventHandler(this.UpdatePromotionRulesCount);
        PromotionEngineLibrary.Add3PromotionRules(_promotionRules);
        // using var client = new HttpClient();
        CreateHttpConnection();
    }

    public string? Input 
    { 
        get { return _input; }
        set 
        {
            if (value != this.Input)
            {
                _input = value;
                NotifyPropertyChanged();
            }
        } 
    }

    private int _promotionRulesCount;
    public int PromotionRulesCount
    {
        get { return _promotionRulesCount; }
        set 
        {
            if (value != _promotionRulesCount)
            {
                _promotionRulesCount = value;
            }
        }
    }

    public List<PromotionRule> PromotionRules
    {
        get { return _promotionRules; }
        set {;}
    }


    public int TotalPrice
    {
        get 
        {
            if (_promotionRules.Count() != PromotionRulesCount)
            {
                NotifyPropertyChanged();
            }
            return _totalPrice; 
        }
        set { _totalPrice = value; }
    }

    bool _hasPOSTFinished;
    public bool HasPOSTFinished
    {
        get { return _hasPOSTFinished; }
        set { _hasPOSTFinished = value; }
    }

    public static IEnumerable<string> StockKeepingUnitsExample { get; } = new List<string>{"A", "A", "A", "B", "B", "B", "B", "B", "C", "D"};

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private async Task ComputeTotalPriceFor3RulesAsync(object? sender, PropertyChangedEventArgs e)
    {
        // TODO: implement api POST request instead that takes serialized json input

        // POST api to compute TotalPrice
        // var url = "https://localhost:7218/api/promotionengineitems/";
        try {
            // Todo: implement that promotionrules gets passed as class member assignment in post ctor call PromotionEngineItem()
            var promotionEngineItem = new PromotionEngineItem(){TotalPrice = 0, InputSKU = _input, PromotionRules = "none"};
            var json = JsonConvert.SerializeObject(promotionEngineItem);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _client;
            var response = await client.PostAsync(_url, data);
            var result = response.Content.ReadAsStringAsync().Result;

            PromotionEngineItem promotionEngineItemRes = JsonConvert.DeserializeObject<PromotionEngineItem>(result);
            _totalPrice = promotionEngineItemRes.TotalPrice;

            _hasPOSTFinished = response.IsSuccessStatusCode;
        } catch (JsonReaderException) {
            _totalPrice = 0;
            _hasPOSTFinished = true;
        }

        // GET to retrieve computed TotalPrice
        // response = await client.GetAsync(_url);
        // response.EnsureSuccessStatusCode();
        // var resp = await response.Content.ReadAsStringAsync();
        // List<PromotionEngineItem> promotionEngineItems = JsonConvert.DeserializeObject<List<PromotionEngineItem>>(result);

        // Select the one with correct id corresponding to above POST request
        // TODO: we need to retrieve the record with correct ID.
        // _totalPrice = promotionEngineItems.LastOrDefault().TotalPrice;

        // _totalPrice = PromotionEngineLibrary.TotalPriceFromInput(_input, _promotionRules);
    }

    private void CreateHttpConnection()
    {
        _client = new HttpClient();
        _client.BaseAddress = new Uri("http://localhost:5278/");
        _client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue("application/json"));
        _url = "api/promotionengineitems";
    }

    private void UpdatePromotionRulesCount(object? sender, PropertyChangedEventArgs e)
    {
        PromotionRulesCount = _promotionRules.Count();
    }
}