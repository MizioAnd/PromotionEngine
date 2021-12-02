using Promotion.Engine.Library;
using System.ComponentModel;
using System.Runtime.CompilerServices;  


namespace Promotion.Engine.ConsoleApp;
public class PromotionEngineViewModel : INotifyPropertyChanged
{
    private List<PromotionRule> _promotionRules = new List<PromotionRule>();
    private IEnumerable<string>? stockKeepingUnits;
    private IEnumerable<int>? _counts;
    private int _totalPrice;
    private static string? _input;

    public PromotionEngineViewModel()
    {
        _totalPrice = 0;
        Add3PromotionRules();
        // computeTotalPriceFor3RulesDel += this.ComputeTotalPriceFor3Rules;
        // PropertyChanged += new PropertyChangedEventHandler(computeTotalPriceFor3RulesDel);
        PropertyChanged += new PropertyChangedEventHandler(this.ComputeTotalPriceFor3Rules);
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
            // _input = value;
            // NotifyPropertyChanged();
            // computeTotalPriceFor3RulesDel += this.ComputeTotalPriceFor3Rules;
            // computeTotalPriceFor3RulesDel();
            // computeTotalPriceFor3RulesDel -= this.ComputeTotalPriceFor3Rules;
        } 
    }

    // public delegate void ComputeTotalPriceFor3RulesDelegate(object? sender, PropertyChangedEventArgs e);
    // ComputeTotalPriceFor3RulesDelegate? computeTotalPriceFor3RulesDel;

    public int TotalPrice
    {
        get { return _totalPrice; }
        set 
        { 
            _totalPrice = value; 
            // NotifyPropertyChanged();
        }
    }

    public static IEnumerable<string> StockKeepingUnitsExample { get; } = new List<string>{"A", "A", "A", "B", "B", "B", "B", "B", "C", "D"};

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void Add3PromotionRules()
    {
        // Create Promotion rule
        int price = 130;
        int nItems = 3;
        string item_i = "A";
        _promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        // Create Promotion rule
        price = 45;
        nItems = 2;
        item_i = "B";
        _promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        // Create Promotion rule
        price = 30;
        item_i = "C";
        string item_j = "D";
        _promotionRules.CreatePromotion2ItemsForFixedPrice(item_i, item_j, price);
    }

    private void ComputeTotalPriceFor3Rules(object? sender, PropertyChangedEventArgs e)
    {
        try {
            if (string.IsNullOrEmpty(_input))
                throw new ArgumentNullException("Parameter needs to be set", nameof(_input));
            stockKeepingUnits = new List<string>(_input.Split(","));
            _counts = stockKeepingUnits.CountSKU();
            _totalPrice = _counts.TotalPriceUsingPromotionRules(_promotionRules);

        } catch (ArgumentNullException) {}
    }
}