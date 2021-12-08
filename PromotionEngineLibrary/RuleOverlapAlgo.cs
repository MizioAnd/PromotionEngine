
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
        foreach (var rule in promotionRules)
        {
            // Todo: implement logic that prevents overlap and optimizes the total savings
            // Simple check would be that if rule has overlap with earlier added rule, then skip.
            if (promotionRulesApplied.OverLappingRulesIndices(rule).Count() == 0)
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
        // Either do a check on PromotionRule class members Item_i and Item_j which is similar to checking instead if any of idx_i or idx_j matches
        // Only check overlaps for rules with an applied count > 0
        int overlappingPromotionRulesCount = 0;
        IEnumerable<int> overlappingRulesIndices;
        var appliedPromotionRulesQuery = promotionRules.Where(x => rulesAppliedCount.ToList<int>().ElementAt(x.Idx_i) > 0);

        // Debug
        var isPromotionRulesDimSame = promotionRules.Count() == appliedPromotionRulesQuery.Count();

        foreach (var rule in appliedPromotionRulesQuery)
        {
            if (appliedPromotionRulesQuery.Where(x => x != rule).Count() > 0)
            {
                // var overlappingRulesIndicesTest = promotionRules.Where(
                //     x => x.Item_i == rule.Item_i & rulesAppliedCount.ToList<int>().ElementAt(x.Idx_i) > 0 
                //     & promotionRules.ToList<PromotionRule>().IndexOf(rule) != promotionRules.ToList<PromotionRule>().IndexOf(x)).Select(
                //         x => promotionRules.ToList<PromotionRule>().IndexOf(x));

                overlappingRulesIndices = appliedPromotionRulesQuery.OverLappingRulesIndices(rule);
                // var isOverlappingRulesIndicesEqual = overlappingRulesIndices.SequenceEqual(overlappingRulesIndicesTest);
                overlappingPromotionRulesCount += overlappingRulesIndices.Count();
            }
        }
        // Divide by 2 since same overlap gets counted twice
        return overlappingPromotionRulesCount/2;
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