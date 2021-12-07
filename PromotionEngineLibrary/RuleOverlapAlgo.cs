
namespace Promotion.Engine.Library;
public static class RuleOverlapAlgo
{
    public static IEnumerable<int> NonOptimizeRulesApplied(this IEnumerable<int> countsSKU, IEnumerable<PromotionRule> promotionRules)
    {
        List<int> promotionRulesAppliedCount = new List<int>(new int[promotionRules.Count()]);
        foreach (var rule in promotionRules)
        {
            promotionRulesAppliedCount[promotionRules.ToList<PromotionRule>().IndexOf(rule)] = rule.PromotionOccurences(countsSKU);
        }
        return promotionRulesAppliedCount;
    }

    public static IEnumerable<int> OptimizeRulesApplied(this IEnumerable<int> countsSKU, IEnumerable<PromotionRule> promotionRules)
    {
        List<int> promotionRulesAppliedCount = new List<int>(new int[promotionRules.Count()]);
        foreach (var rule in promotionRules)
        {
            // Todo: implement logic that prevents overlap and optimizes the total savings
            promotionRulesAppliedCount[promotionRules.ToList<PromotionRule>().IndexOf(rule)] = rule.PromotionOccurences(countsSKU);
        }
        return promotionRulesAppliedCount;
    }

    public static int OverlappingPromotionRules(this IEnumerable<int> rulesAppliedCount, IEnumerable<PromotionRule> promotionRules)
    {
        // Either do a check on PromotionRule class members Item_i and Item_j which is similar to checking instead if any of idx_i or idx_j matches
        // Only check overlaps for rules with an applied count > 0
        int overlappingPromotionRulesCount = 0;
        IEnumerable<int> overlappingRulesIndices;
        foreach (var rule in promotionRules)
        {
            if (promotionRules.Where(x => x.Item_i == rule.Item_i).Count() > 0)
            {
                overlappingRulesIndices = promotionRules.Where(
                    x => x.Item_i == rule.Item_i & rulesAppliedCount.ToList<int>().ElementAt(x.Idx_i) > 0 
                    & promotionRules.ToList<PromotionRule>().IndexOf(rule) != promotionRules.ToList<PromotionRule>().IndexOf(x)).Select(
                        x => promotionRules.ToList<PromotionRule>().IndexOf(x));
                overlappingPromotionRulesCount += overlappingRulesIndices.Count();
            }
        }
        // Divide by 2 since same overlap gets counted twice
        return overlappingPromotionRulesCount/2;
    }

    public static void MaxSavings()
    {
        throw new NotImplementedException("Please create a test first.");
    }

    public static void OptimizeRulesAppliedAndMaxSavings()
    {
        throw new NotImplementedException("Please create a test first.");
    }
}