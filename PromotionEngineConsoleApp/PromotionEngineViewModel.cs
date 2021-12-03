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
        PropertyChanged += new PropertyChangedEventHandler(this.ComputeTotalPriceFor3Rules);
        Add3PromotionRules();
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

    // private int _promotionRulesCount;
    // public int PromotionRulesCount
    // {
    //     get { return _promotionRulesCount; }
    //     set 
    //     {
    //         if (value != this.PromotionRulesCount)
    //         {
    //             _promotionRulesCount = value;
    //             NotifyPropertyChanged();
    //         }
    //     }
    // }

    public List<PromotionRule> PromotionRules
    {
        get 
        {
            // if (_promotionRules.Count() != PromotionRulesCount)
            //     PromotionRulesCount = _promotionRules.Count();
            return _promotionRules; 
        }
        set {;}
    }


    public int TotalPrice
    {
        get 
        {
            NotifyPropertyChanged();
            return _totalPrice; 
        }
        set { _totalPrice = value; }
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
        PromotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        // Create Promotion rule
        price = 45;
        nItems = 2;
        item_i = "B";
        PromotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        // Create Promotion rule
        price = 30;
        item_i = "C";
        string item_j = "D";
        PromotionRules.CreatePromotion2ItemsForFixedPrice(item_i, item_j, price);
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