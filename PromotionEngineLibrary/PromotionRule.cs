using Promotion.Engine.Library;

namespace Promotion.Engine.Library;
public class PromotionRule
{
    public int Price {get;}
    public int Saving {get;}
    public string Item_i {get;}
    public string? Item_j {get;}
    public IEnumerable<string?> Items {get;}
    public int IdxProduct_i {get;}
    public int IdxProduct_j {get;}
    public string? PrintRule {get;}
    public Func<IEnumerable<int>?, int, int, int> OccurencesDelegate {get; set;}

    public PromotionRule(string item_i, string? item_j, int idx_i, int idx_j, int price, int saving, Func<IEnumerable<int>?, int, int, int> ruleMethod)
    {
        Item_i = item_i;
        Item_j = item_j;
        Items = new List<string?>{item_i, item_j};
        IdxProduct_i = idx_i;
        IdxProduct_j = idx_j;
        Price = price;
        Saving = saving;
        if (!String.IsNullOrEmpty(Item_j))
            PrintRule = String.Join(";", new List<string>{Item_i, Item_j, Price.ToString()});
        OccurencesDelegate = ruleMethod;
    }

    public int PromotionOccurences(IEnumerable<int>? counts)
    {
        var occurences = OccurencesDelegate(counts, IdxProduct_i, IdxProduct_j);
        return occurences;
    }

    public int TotalPromotionSaving(IEnumerable<int>? counts)
    {
        var totalSaving = PromotionOccurences(counts)*Saving;
        return totalSaving;
    }
}