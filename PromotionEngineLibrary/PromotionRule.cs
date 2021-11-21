using Promotion.Engine.Library;

namespace Promotion.Engine.Library;
public class PromotionRule
{
    public int Price {get;}
    public int Saving {get;}
    public string Item_i {get;}
    public string Item_j {get;}
    public int Idx_i {get;}
    public int Idx_j {get;}
    public string PrintRule {get;}
    // public delegate void Action<in T>(T obj);
    // public delegate TResult Func<in T,out TResult>(T arg);
    public Func<IEnumerable<int>?, int, int, int> OccurencesDelegate {get; set;}

    public PromotionRule(string item_i, string item_j, int idx_i, int idx_j, int price, int saving, Func<IEnumerable<int>?, int, int, int> ruleMethod)
    {
        Item_i = item_i;
        Item_j = item_j;
        Idx_i = idx_i;
        Idx_j = idx_j;
        Price = price;
        Saving = saving;
        PrintRule = String.Join(";", new List<string>{Item_i, Item_j, Price.ToString()});
        OccurencesDelegate = ruleMethod;
    }

    // public int Occurences(IEnumerable<int>? counts)
    // {
    //     var occurences = Math.Min(counts.ElementAt(Idx_i), counts.ElementAt(Idx_j));
    //     return occurences;
    // }

    public int PromotionOccurences(IEnumerable<int>? counts)
    {
        // Todo: Should be more general. Consider handler method such that same class can be used by other to promotions
        // var occurences = Math.Min(counts.ElementAt(Idx_i), counts.ElementAt(Idx_j));
        var occurences = OccurencesDelegate(counts, Idx_i, Idx_j);
        return occurences;
    }

    public int TotalPromotionSaving(IEnumerable<int>? counts)
    {
        var totalSaving = PromotionOccurences(counts)*Saving;
        return totalSaving;
    }
}