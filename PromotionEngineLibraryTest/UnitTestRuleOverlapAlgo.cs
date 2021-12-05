using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Promotion.Engine.Library.Test;
public class UnitTestRuleOverlapAlgo
{
    [SetUp]
    public void Setup()
    {
    }
  
   [Test]
    public void TestNonOptimizedOverlappingPromotionRules()
    {
        // Todo: find f(x)=0 where x = {times_rule_1_applied, times_rule_2_applied, .., times_rule_n_applied}
        // f: x --> number of times multiple rules overlapped 

        // Outcommented since to general as first step
        // var cartSize = 10;
        // IEnumerable<string> randomSKU = new List<string>(new string[cartSize]);
        // Random random = new Random();
        // randomSKU = randomSKU.Select(x => PromotionEngineLibrary.ProductList.ToList<string>()[random.Next(cartSize)]);
        // var counts = randomSKU.CountSKU();

        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "A", "B", "B", "B", "B", "B", "C", "D"};
        var counts = stockKeepingUnits.CountSKU();

        List<PromotionRule> promotionRules = new List<PromotionRule>();

        // Create Promotion rule
        int price = 130;
        int nItems = 3;
        string item_i = "B";
        promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        // Create Promotion rule
        price = 45;
        nItems = 2;
        item_i = "B";
        promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        IEnumerable<int> rulesAppliedCount = counts.NonOptimizeRulesApplied(promotionRules);
        var overlaps = rulesAppliedCount.OverlappingPromotionRules(promotionRules);
        var expectedOverlaps = 1;
        var result = overlaps == expectedOverlaps;
        Assert.True(result, String.Format("Expected number of times multiple rules overlapped '{0}': true, and actual overlap count '{1}': '{2}'", expectedOverlaps, overlaps, result));
    }

    [Test]
    public void TestOptimizedOverlappingPromotionRules()
    {
        // Todo: find f(x)=0 where x = {times_rule_1_applied, times_rule_2_applied, .., times_rule_n_applied}
        // f: x --> number of times multiple rules overlapped 

        // Outcommented since to general as first step
        // var cartSize = 10;
        // IEnumerable<string> randomSKU = new List<string>(new string[cartSize]);
        // Random random = new Random();
        // randomSKU = randomSKU.Select(x => PromotionEngineLibrary.ProductList.ToList<string>()[random.Next(cartSize)]);
        // var counts = randomSKU.CountSKU();

        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "A", "B", "B", "B", "B", "B", "C", "D"};
        var counts = stockKeepingUnits.CountSKU();

        List<PromotionRule> promotionRules = new List<PromotionRule>();

        // Create Promotion rule
        int price = 130;
        int nItems = 3;
        string item_i = "B";
        promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        // Create Promotion rule
        price = 45;
        nItems = 2;
        item_i = "B";
        promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        IEnumerable<int> rulesAppliedCount = counts.OptimizeRulesApplied(promotionRules);
        var overlaps = rulesAppliedCount.OverlappingPromotionRules(promotionRules);
        var expectedOverlaps = 0;
        var result = overlaps == expectedOverlaps;
        Assert.True(result, String.Format("Expected number of times multiple rules overlapped '{0}': true, and actual overlap count '{1}': '{2}'", expectedOverlaps, overlaps, result));
    }


    [Test]
    public void TestMaxSavings()
    {
        // Todo: maximize savings and find x_0 which satifies f(x_0)=0 and g(x_0)=max(g(x)) where g maps to total amount saved
    }

    [Test]
    public void TestOverlappingPromotionRulesAndMaxSavings()
    {
        // Todo: Create algo that both satifies 0 overlapping promotion rules applied and max savings corresponding to accumulated
        // savings from applied promotion rules
    }
}