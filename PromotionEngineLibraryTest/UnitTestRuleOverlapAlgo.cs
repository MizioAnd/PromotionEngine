using NUnit.Framework;
using System;
using System.Collections.Generic;
using Promotion.Engine.Library;

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
        int price = 130;
        int nItems = 3;
        string item_i = "B";
        promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        // Create Promotion rule
        price = 90;
        nItems = 2;
        item_i = "B";
        promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);
    }

    private static void Create2NonOverlappingPromotionRules(List<PromotionRule> promotionRules)
    {
        // Create Promotion rule
        int price = 130;
        int nItems = 3;
        string item_i = "A";
        promotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        // Create Promotion rule
        price = 30;
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
    public void OverlappingSKUConsumptionInRules_TwoOverlappingPromotionRules_ZeroOverlappingSKUAndBothOverlappingRulesApplied()
    {
        // Arrange
        IEnumerable<string> stockKeepingUnits = new List<string>{"B", "B", "B", "B", "B"};
        var counts = stockKeepingUnits.CountSKU();
        List<PromotionRule> promotionRules = new List<PromotionRule>();
        Create2OverlappingPromotionRules(promotionRules);

        // Act
        IEnumerable<int> rulesAppliedCount = counts.OptimizeRulesApplied(promotionRules);
        var overlapsRules = rulesAppliedCount.OverlappingPromotionRules(promotionRules);
        var overlapsSKU = rulesAppliedCount.OverlappingSKUConsumptionInRulesSum(promotionRules, counts);

        // Assert
        var expectedOverlapsRules = 1;
        var expectedOverlapsSKU = 0;
        var result = overlapsSKU == expectedOverlapsSKU & overlapsRules == expectedOverlapsRules;
        Assert.True(result, String.Format("Expected number of overlapping SKUs after applied rules '{0}': true, and actual overlap count '{1}': '{2}'", expectedOverlapsSKU, overlapsSKU, result));
    }

    [Test]
    public void MaxSavings_TwoOverlappingPromotionRules_HigherSavingsThanForRandomSelectionOfAppliedOverlappingPromotionRules()
    {
        // Todo: maximize savings and find x_0 which satifies f(x_0)=0 and g(x_0)=max(g(x)) where g maps to total amount saved

        // Arrange
        // Act
        // Assert
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