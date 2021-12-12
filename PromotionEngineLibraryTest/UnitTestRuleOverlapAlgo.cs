using NUnit.Framework;
using System;
using System.Collections.Generic;
using Promotion.Engine.Library;
using System.Linq;

namespace Promotion.Engine.UnitTests.Library;
[TestFixture]
public class UnitTestRuleOverlapAlgo
{
    [SetUp]
    public void Setup()
    {
    }
  
    private static void Create2OverlappingPromotionRules(List<PromotionRule> promotionRules)
    {
        // Create Promotion rule
        int nItems = 3;
        int price = nItems*PromotionEngineLibrary.PriceA - PromotionEngineLibrary.Promotion3AsSaving;
        string item_i = "A";
        promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        // Create Promotion rule
        nItems = 2;
        price = nItems*PromotionEngineLibrary.PriceA - PromotionEngineLibrary.Promotion3AsSaving/2;
        item_i = "A";
        promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);
    }

    private static void Create2OverlappingButDifferentTypePromotionRules(List<PromotionRule> promotionRules)
    {
        // Create Promotion rule
        var nItems = 2;
        var priceRule2Bs = nItems*PromotionEngineLibrary.PriceB - PromotionEngineLibrary.Promotion2BsSaving;
        var item_i = "B";
        promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, priceRule2Bs);

        // Create Promotion rule
        // Todo: derive price that Create promotion such that the price for B and C is such that the rule must be applied 2 times in BBBBCC
        // and that 2B rule is applied once. Edge cases BC rule could be such that it was always cheaper to apply that one since the single B price 
        // is less than single B price in 2B rule or BC rule could be applied always if C unit has lower price than without promotion.
        var price = PromotionEngineLibrary.PriceB + PromotionEngineLibrary.PriceC - Math.Max(PromotionEngineLibrary.PriceB, PromotionEngineLibrary.PriceC)/2;
        item_i = "B";
        string item_j = "C";
        promotionRules.CreatePromotion2ItemsForFixedPrice(item_i, item_j, price);
    }

    private static void Create2NonOverlappingPromotionRules(List<PromotionRule> promotionRules)
    {
        // Create Promotion rule
        var nItems = 2;
        var priceRule2Bs = nItems*PromotionEngineLibrary.PriceB - PromotionEngineLibrary.Promotion2BsSaving;
        var item_i = "B";
        promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, priceRule2Bs);

        // Create Promotion rule
        var price = PromotionEngineLibrary.PromotionCandD;
        item_i = "C";
        string item_j = "D";
        promotionRules.CreatePromotion2ItemsForFixedPrice(item_i, item_j, price);
    }

    [Test]
    public void NonOptimizeRulesApplied_TwoOverlappingPromotionRules_OneOverlap()
    {
        // Find f(x)=1 where x = {times_rule_1_applied, times_rule_2_applied, .., times_rule_n_applied}
        // f: x --> number of times multiple rules overlapped

        // Arrange
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "A", "B", "B", "B", "B", "B", "C", "D"};
        var counts = stockKeepingUnits.CountSKU();
        List<PromotionRule> promotionRules = new List<PromotionRule>();
        Create2OverlappingPromotionRules(promotionRules);

        // Act
        IEnumerable<int> rulesAppliedCount = counts.NonOptimizeRulesApplied(promotionRules);
        var overlaps = rulesAppliedCount.OverlappingPromotionRules(promotionRules);

        // Assert
        var expectedOverlaps = 1;
        var result = overlaps == expectedOverlaps;
        Assert.True(result, String.Format("Expected number of times multiple rules overlapped '{0}': true, and actual overlap count '{1}': '{2}'", expectedOverlaps, overlaps, result));
    }

    [Test]
    public void OptimizeRulesApplied_TwoOverlappingPromotionRules_ZeroOverlap()
    {
        // Find f(x)=0 where x = {times_rule_1_applied, times_rule_2_applied, .., times_rule_n_applied}
        // f: x --> number of times multiple rules overlapped

        // Arrange
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "A", "B", "B", "B", "B", "B", "C", "D"};
        var counts = stockKeepingUnits.CountSKU();
        List<PromotionRule> promotionRules = new List<PromotionRule>();
        Create2OverlappingPromotionRules(promotionRules);
        Create2NonOverlappingPromotionRules(promotionRules);

        // Act
        IEnumerable<int> rulesAppliedCount = counts.OptimizeRulesApplied(promotionRules);
        var overlaps = rulesAppliedCount.OverlappingPromotionRules(promotionRules);

        // Assert
        var expectedOverlaps = 0;
        var result = overlaps == expectedOverlaps;
        Assert.True(result, String.Format("Expected number of times multiple rules overlapped '{0}': true, and actual overlap count '{1}': '{2}'", expectedOverlaps, overlaps, result));
    }

    [Test]
    public void SKUConsumptionInRulesSum_TwoOverlappingPromotionRules_ZeroOverlappingSKUAndBothOverlappingRulesApplied()
    {
        // Arrange
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "A", "A", "A"};
        var counts = stockKeepingUnits.CountSKU();
        List<PromotionRule> promotionRules = new List<PromotionRule>();
        Create2OverlappingPromotionRules(promotionRules);

        // Act
        IEnumerable<int> rulesAppliedCount = counts.OptimizeRulesApplied(promotionRules);
        var overlapsRules = rulesAppliedCount.OverlappingPromotionRules(promotionRules);
        var overlapsSKU = rulesAppliedCount.SKUConsumptionInRulesSum(promotionRules, counts);

        // Assert
        var expectedOverlapsRules = 1;
        var expectedOverlapsSKU = 0;
        var result = overlapsSKU == expectedOverlapsSKU & overlapsRules == expectedOverlapsRules;
        Assert.True(result, String.Format("Expected number of overlapping SKUs after applied rules '{0}': true, and actual overlap count '{1}': '{2}'", expectedOverlapsSKU, overlapsSKU, result));
    }

    [Test]
    public void MaxSavings_ThreeOverlappingPromotionRules_HigherSavingsThanForRandomSelectionOfAppliedOverlappingPromotionRules()
    {
        // Todo: maximize savings and find x_0 which satifies f(x_0)=0 and g(x_0)=max(g(x)) where g maps to total amount saved

        // Arrange
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "A", "A", "A", "A"};
        var counts = stockKeepingUnits.CountSKU();
        List<PromotionRule> promotionRules = new List<PromotionRule>();
        Create2OverlappingPromotionRules(promotionRules);
        // Create Cheapest Promotion rule
        var nItems = 3;
        var price = nItems*PromotionEngineLibrary.PriceA - PromotionEngineLibrary.Promotion3AsSaving/2;
        var item_i = "A";
        promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        // Act
        IEnumerable<int> maxSavingsRulesAppliedCount = counts.MaxSavings(promotionRules);

        // Assert
        IEnumerable<int> expectedRulesAppliedCount = new List<int>{0,0,2};
        var result = maxSavingsRulesAppliedCount.SequenceEqual(expectedRulesAppliedCount);
        Assert.True(result, String.Format("Expected rules applied indices'{0}': true, and actual indices '{1}': '{2}'"
            , String.Join(",", expectedRulesAppliedCount), String.Join(",", maxSavingsRulesAppliedCount), result));
    }

    [Test]
    public void MaxSavings_TwoOverlappingDifferentTypePromotionRules_HigherSavingsThanForRandomSelectionOfAppliedOverlappingPromotionRules()
    {
        // Arrange
        IEnumerable<string> stockKeepingUnits = new List<string>{"B", "B", "B", "B", "C", "C"};
        var counts = stockKeepingUnits.CountSKU();
        List<PromotionRule> promotionRules = new List<PromotionRule>();
        Create2OverlappingButDifferentTypePromotionRules(promotionRules);

        // Act
        IEnumerable<int> maxSavingsRulesAppliedCount = counts.MaxSavings(promotionRules);

        // Assert
        IEnumerable<int> expectedRulesAppliedCount = new List<int>{1,1};
        var result = maxSavingsRulesAppliedCount.SequenceEqual(expectedRulesAppliedCount);
        Assert.True(result, String.Format("Expected rules applied indices'{0}': true, and actual indices '{1}': '{2}'"
            , String.Join(",", expectedRulesAppliedCount), String.Join(",", maxSavingsRulesAppliedCount), result));
    }

    [Test]
    public void OptimizeRulesAppliedAndMaxSavings_TwoOverlappingPromotionRules_HigherSavingsThanForMaxSavingsWithAppliedOverlappingPromotionRules()
    {
        // Todo: Create algo that both satifies 0 overlapping promotion rules applied and max savings corresponding to accumulated
        // savings from applied promotion rules

        // Arrange
        // Act
        // Assert

    }
}