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
        List<int> countsSKUState = new List<int>(new int[countsSKU.Count()]);
        UpdateCounts(ref countsSKUState, (List<int>)countsSKU);

        IEnumerable<int> countsSKUSubtractedConsumedSKU;
        var idxRule = 0;
        
        foreach (var rule in promotionRules)
        {
            promotionRulesAppliedCount[promotionRules.ToList<PromotionRule>().IndexOf(rule)] = rule.PromotionOccurences(countsSKUState);

            // Reset list with 0s
            var countsOnlyByAppliedRule = new List<int>(new int[promotionRules.Count()]);
            countsOnlyByAppliedRule[idxRule] = rule.PromotionOccurences(countsSKUState);
            var countsConsumed = countsOnlyByAppliedRule.SKUConsumptionInRules(promotionRules);

            countsSKUSubtractedConsumedSKU = SubtractConsumedCounts(countsSKUState, countsConsumed);

            UpdateCounts(ref countsSKUState, countsSKUSubtractedConsumedSKU);
            idxRule++;
        }
        return promotionRulesAppliedCount;
    }

    private static void UpdateCounts(ref List<int> countsOld, IEnumerable<int> countsNew)
    {
        foreach (var ite in Enumerable.Range(0, countsOld.Count()))
        {
            countsOld[ite] = countsNew.ToList<int>()[ite];
        }
    }

    private static IEnumerable<int> SubtractConsumedCounts(List<int> countsOld, IEnumerable<int> countsConsumed)
    {
        List<int> countsStateAfterConsumption = new List<int>(new int[countsOld.Count()]);
        foreach (var ite in Enumerable.Range(0, countsOld.Count()))
        {
            countsStateAfterConsumption[ite] = countsOld[ite] - countsConsumed.ToList<int>()[ite];
        }
        return countsStateAfterConsumption;
    }

    public static int OverlappingPromotionRules(this IEnumerable<int> rulesAppliedCount, IEnumerable<PromotionRule> promotionRules)
    {
        int overlappingPromotionRulesCount = 0;
        IEnumerable<int> overlappingRulesIndices;
        var appliedPromotionRulesQuery = promotionRules.Where(x => rulesAppliedCount.ToList<int>().ElementAt(promotionRules.ToList().IndexOf(x)) > 0);

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

    public static IEnumerable<int> SKUConsumptionInRules(this IEnumerable<int> rulesAppliedCount, IEnumerable<PromotionRule> promotionRules)
    {
        // Todo: for each applied promotion rule use number of times it was applied in rulesAppliedCount together 
        // with Items member of a promotion to compute sum of SKU consumption that should not be larger than actual countsSKU
        if (rulesAppliedCount.All(x => x == 0))
        {
            IList<int> counts = new List<int>(new int[PromotionEngineLibrary.ProductList.Count()]);
            return counts;
            // throw new ArgumentException("Parameter only has zeros", nameof(rulesAppliedCount));
        }

        int idxRule = 0;
        PromotionRule rule;
        string skuConsumed = "";
        
        string ruleItems;
        foreach (var ruleAppliedCount in rulesAppliedCount)
        {
            if (ruleAppliedCount > 0)
            {
                rule = promotionRules.ToList<PromotionRule>().ElementAt(idxRule);

                ruleItems = String.Join(",", rule.Items.Where(x => x != null));

                var multiplyFactor = ruleAppliedCount;
                if (rule.Items.Contains(null))
                    multiplyFactor *= rule.IdxProduct_j;

                string ruleSkuConsumed = "";
                foreach (var ite in Enumerable.Range(0, multiplyFactor))
                    ruleSkuConsumed = String.Format("{0},{1}", ruleSkuConsumed, ruleItems);
                skuConsumed = String.Format("{0},{1}", skuConsumed, ruleSkuConsumed);
            }
            idxRule++;
        }
        skuConsumed = String.Join(",", skuConsumed.Split(",").Where(x => x != ""));
        var stockKeepingUnits = new List<string>(skuConsumed.Split(","));
        var appliedRulesSKUcounts = stockKeepingUnits.CountSKU();
        return appliedRulesSKUcounts;
    }

    public static int SKUConsumptionInRulesSum(this IEnumerable<int> rulesAppliedCount, IEnumerable<PromotionRule> promotionRules, IEnumerable<int> countsSKU)
    {
        var appliedRulesSKUcounts = rulesAppliedCount.SKUConsumptionInRules(promotionRules);
        var diff = appliedRulesSKUcounts.Select(x => x - countsSKU.ToList().ElementAt(appliedRulesSKUcounts.ToList<int>().IndexOf(x)));
        return diff.Sum();
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

    public static IEnumerable<int> MaxSavings(this IEnumerable<int> countsSKU, IEnumerable<PromotionRule> promotionRules)
    {
        // Returns true if application of rule maximizes savings in total price
        // Builds on top of method OptimizeRulesApplied() so it solves also OptimizeRulesAppliedAndMaxSavings(), but MaxSavings() is a better name since OptimizeRulesApplied() 
        // is more an underlying optimization.
        // First simple solution: It should first try out all sequence combinations of rules which for n rules amounts to n! combinations
        // For each combination of rules in promotionRules it could call counts.OptimizeRulesApplied(promotionRules)
        // Then for each of the returned rulesAppliedCount the total savings get computed
        // finally the rulesAppliedCount leading to a max for total savings is selected
        
        throw new NotImplementedException("Please create a test first.");
    }

    public static void OptimizeRulesAppliedAndMaxSavings()
    {
        throw new NotImplementedException("Please create a test first.");
    }
}