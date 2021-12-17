using Promotion.Engine.Library;

namespace PromotionEngineAPI.Models;
public class PromotionEngineItem
{
    public long Id { get; set; }
    public int TotalPrice { get; set; }
    public string? InputSKU { get; set; }
    public string? PromotionRules { get; set; }
}