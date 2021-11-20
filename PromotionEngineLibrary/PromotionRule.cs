using Promotion.Engine.Library;

namespace Promotion.Engine.Library;
public class PromotionRule
{
    public int Price {get;}
    public string Item_i {get;}
    public string Item_j {get;}
    public string PrintRule {get;}
    public PromotionRule(string item_i, string item_j, int price)
    {
        Item_i = item_i;
        Item_j = item_j;
        Price = price;
        PrintRule = String.Join(";", new List<string>{Item_i, Item_j, Price.ToString()});
    }
    
}