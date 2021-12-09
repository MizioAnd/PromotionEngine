
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
        List<PromotionRule> promotionRulesApplied = new List<PromotionRule>(promotionRules.Count());
        bool noOverlapWithEarlierRules;
        foreach (var rule in promotionRules)
        {
            noOverlapWithEarlierRules = promotionRulesApplied.OverLappingRulesIndices(rule).Count() == 0;
            if (noOverlapWithEarlierRules)
            {
                promotionRulesAppliedCount[promotionRules.ToList<PromotionRule>().IndexOf(rule)] = rule.PromotionOccurences(countsSKU);
                if (rule.PromotionOccurences(countsSKU) > 0)
                    promotionRulesApplied.Add(rule);
            }
        }
        return promotionRulesAppliedCount;
    }

    public static int OverlappingPromotionRules(this IEnumerable<int> rulesAppliedCount, IEnumerable<PromotionRule> promotionRules)
    {
        int overlappingPromotionRulesCount = 0;
        IEnumerable<int> overlappingRulesIndices;
        var appliedPromotionRulesQuery = promotionRules.Where(x => rulesAppliedCount.ToList<int>().ElementAt(x.Idx_i) > 0);

        foreach (var rule in appliedPromotionRulesQuery)
        {
            if (appliedPromotionRulesQuery.Where(x => x != rule).Count() > 0)
            {
                overlappingRulesIndices = appliedPromotionRulesQuery.OverLappingRulesIndices(rule);
                overlappingPromotionRulesCount += overlappingRulesIndices.Count();
            }
        }
        // Divide by 2 since same overlap gets counted twice
        return overlappingPromotionRulesCount/2;
    }

    public static int OverlappingSKUConsumptionInRules()
    {
        throw new NotImplementedException("Please create a test first.");
    }

    private static IEnumerable<int> OverLappingRulesIndices(this IEnumerable<PromotionRule> appliedPromotionRulesQuery, PromotionRule rule)
    {
        var overlappingRulesIndices = appliedPromotionRulesQuery
        .Where(x => appliedPromotionRulesQuery.ToList<PromotionRule>().IndexOf(rule) != appliedPromotionRulesQuery.ToList<PromotionRule>().IndexOf(x))
        .Where(x => x.Items.Where(x => x != null).Intersect(rule.Items).Count() > 0)
        .Select(x => appliedPromotionRulesQuery.ToList<PromotionRule>().IndexOf(x));

        // var rulesIndicesExceptOneRuleQuery = appliedPromotionRulesQuery.Where(x => appliedPromotionRulesQuery.ToList<PromotionRule>().IndexOf(rule) != appliedPromotionRulesQuery.ToList<PromotionRule>().IndexOf(x));
        // var countExceptRule = rulesIndicesExceptOneRuleQuery.Count();
        // var overlappingRulesIndicesExceptOneRuleQuery = rulesIndicesExceptOneRuleQuery.Where(x => x.Items.Where(x => x != null).Intersect(rule.Items).Count() > 0);
        // var countOverlap = overlappingRulesIndicesExceptOneRuleQuery.Count();
        // var overlappingRulesIndicesQuery = overlappingRulesIndicesExceptOneRuleQuery.Select(x => appliedPromotionRulesQuery.ToList<PromotionRule>().IndexOf(x));
        // var doQueriesMatch = overlappingRulesIndicesQuery.SequenceEqual(overlappingRulesIndices);
        return overlappingRulesIndices;
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