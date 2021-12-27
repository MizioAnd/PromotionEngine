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
        
        if (rulesAppliedCount.All(x => x == 0))
        {
            IList<int> counts = new List<int>(new int[PromotionEngineLibrary.ProductList.Count()]);
            return counts;
            // throw new ArgumentException("Parameter only has zeros", nameof(rulesAppliedCount));
        }

        var skuConsumed = TranslateRulesAppliedCountToSKUs(rulesAppliedCount, promotionRules);
        var stockKeepingUnits = new List<string>(skuConsumed.Split(","));
        var appliedRulesSKUcounts = stockKeepingUnits.CountSKU();
        return appliedRulesSKUcounts;
    }

    private static string TranslateRulesAppliedCountToSKUs(IEnumerable<int> rulesAppliedCount, IEnumerable<PromotionRule> promotionRules)
    {
        int idxRule = 0;
        PromotionRule rule;
        string skuConsumed = "";
        
        string ruleItems;
        foreach (var ruleAppliedCount in rulesAppliedCount)
        {
            if (ruleAppliedCount > 0)
            {
                skuConsumed = CountToSKU(promotionRules, idxRule, out rule, skuConsumed, out ruleItems, ruleAppliedCount);
            }
            idxRule++;
        }
        skuConsumed = String.Join(",", skuConsumed.Split(",").Where(x => x != ""));
        return skuConsumed;
    }

    private static string CountToSKU(IEnumerable<PromotionRule> promotionRules, int idxRule, out PromotionRule rule, string skuConsumed, out string ruleItems, int ruleAppliedCount)
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
        return skuConsumed;
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
        var permutationsRules = GetPermutations<PromotionRule>(promotionRules, promotionRules.Count());

        List<int> maxSavings;
        SavingsForAllPermutationsOfRules(countsSKU, permutationsRules, out maxSavings);

        var maxIdx = maxSavings.IndexOf(maxSavings.Max());
        var rulePermMaxSaving = permutationsRules.ToList<IEnumerable<PromotionRule>>()[maxIdx];
        var rulesAppliedCountMaxSavings = countsSKU.OptimizeRulesApplied(rulePermMaxSaving);

        List<int> rulesAppliedCountMaxSavingsOrdered = RulesAppliedCountOrdered(promotionRules, rulePermMaxSaving, rulesAppliedCountMaxSavings);

        return rulesAppliedCountMaxSavingsOrdered;
    }

    private static List<int> RulesAppliedCountOrdered(IEnumerable<PromotionRule> promotionRulesOrdered, IEnumerable<PromotionRule> rulePermutationUnordered, IEnumerable<int> rulesAppliedCountMaxSavings)
    {
        var inverseIdxRulesQuery = rulePermutationUnordered.Select(rule => promotionRulesOrdered.ToList<PromotionRule>().IndexOf(rule)).ToList<int>();
        var rulesAppliedCountOrdered = new List<int>(new int[rulesAppliedCountMaxSavings.Count()]);
        var ite = 0;
        foreach (var count in rulesAppliedCountMaxSavings)
        {
            rulesAppliedCountOrdered[inverseIdxRulesQuery[ite]] = count;
            ite++;
        }
        return rulesAppliedCountOrdered;
    }

    private static void SavingsForAllPermutationsOfRules(IEnumerable<int> countsSKU, IEnumerable<IEnumerable<PromotionRule>> permutationsRules, out List<int> maxSavings)
    {
        maxSavings = new List<int>(new int[permutationsRules.Count()]);
        var ite = 0;
        int savings;
        foreach (var rulesPerm in permutationsRules)
        {
            savings = 0;
            SavingsOfRulePermuation(countsSKU, out savings, rulesPerm);
            maxSavings[ite] = savings;
            ite++;
        }
    }

    private static void SavingsOfRulePermuation(IEnumerable<int> countsSKU, out int savings, IEnumerable<PromotionRule> rulesPerm)
    {
        var rulesAppliedCount = countsSKU.OptimizeRulesApplied(rulesPerm);
        savings = 0;
        var jte = 0;
        foreach (var count in rulesAppliedCount)
        {
            savings += rulesPerm.ToList<PromotionRule>()[jte].Saving * count;
            jte++;
        }
    }

    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
    {
        if (length == 1)
            return list.Select(t => new T[]{ t });
        return GetPermutations(list, length - 1).SelectMany(t => list.Where(e => !t.Contains(e)), (t1, t2) => t1.Concat(new T[] { t2 }));
    }

    public static void OptimizeRulesAppliedAndMaxSavings()
    {
        throw new NotImplementedException("Please create a test first.");
    }
}