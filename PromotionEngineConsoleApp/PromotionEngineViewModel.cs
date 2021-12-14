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
        PropertyChanged += new PropertyChangedEventHandler(this.UpdatePromotionRulesCount);
        PromotionEngineLibrary.Add3PromotionRules(_promotionRules);
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

    public static IEnumerable<string> StockKeepingUnitsExample { get; } = new List<string>{"A", "A", "A", "B", "B", "B", "B", "B", "C", "D"};

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private void ComputeTotalPriceFor3Rules(object? sender, PropertyChangedEventArgs e)
    {
        // TODO: implement api POST request instead that takes serialized json input
        _totalPrice = PromotionEngineLibrary.TotalPriceFromInput(_input, _promotionRules);
    }

    private void UpdatePromotionRulesCount(object? sender, PropertyChangedEventArgs e)
    {
        PromotionRulesCount = _promotionRules.Count();
    }
}