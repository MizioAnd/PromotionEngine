using NUnit.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using StackExchange.Profiling;
using Promotion.Engine.Library;

namespace Promotion.Engine.UnitTests.Library;
[TestFixture]
public class UnitTestPromotionEngineLibrary
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TotalPrice_SmallInputCartWithTwoUniqueSKUAndOnePromotionApplied_PriceSubtractedSavingsOfOnePromotion()
    {
        // Arrange
        // SKUs {"A", "B", "A", "B"}
        IEnumerable<int> counts = new List<int>{2, 2, 0, 0};

        // Act
        int totalPrice = counts.TotalPrice();

        // Assert
        int expectedTotal = 2*PromotionEngineLibrary.PriceA + 2*PromotionEngineLibrary.PriceB - PromotionEngineLibrary.Promotion2BsSaving;
        bool result = expectedTotal == totalPrice;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [Test]
    public void CountSKU_SmallInputCartWithTwoUniqueSKU_CountOfEachUniqueSKUFromInputCartArrangedInAList()
    {
        // Arrange
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "B", "A", "B"};

        // Act
        var counts = stockKeepingUnits.CountSKU();

        // Assert
        IEnumerable<int> expected = (new List<int>{2, 2, 0, 0, 0}).Concat(new List<int>(new int[PromotionEngineLibrary.ProductList.Count() - 5]));
        bool result = counts.SequenceEqual(expected);
        Assert.IsTrue(result, String.Format("Expected counts '{0}': true, but actual counts '{1}': {2}", String.Join(",", expected), String.Join(",", counts), result));
    }

    [Test]
    public void CountSKU_SmallInputCartWithThreeUniqueSKU_CountOfEachUniqueSKUFromInputCartArrangedInAList()
    {
        // Arrange
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "B", "A", "B", "E"};

        // Act
        var counts = stockKeepingUnits.CountSKU();

        // Assert
        IEnumerable<int> expected = (new List<int>{2, 2, 0, 0, 1}).Concat(new List<int>(new int[PromotionEngineLibrary.ProductList.Count() - 5]));
        bool result = counts.SequenceEqual(expected);
        Assert.IsTrue(result, String.Format("Expected counts '{0}': true, but actual counts '{1}': {2}", String.Join(",", expected), String.Join(",", counts), result));
    }

    [Test]
    public void TotalPrice_InputCartWith3UniqueSKU_PriceWithoutAnyPromotion()
    {
        // Test Scenario A

        // Arrange
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "B", "C"};
        var counts = stockKeepingUnits.CountSKU();

        // Act
        int totalPrice = counts.TotalPrice();

        // Assert
        int expectedTotal = PromotionEngineLibrary.PriceA + PromotionEngineLibrary.PriceB + PromotionEngineLibrary.PriceC;
        bool result = expectedTotal == totalPrice & expectedTotal == 100;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [Test]
    public void TotalPrice_InputCartWith3UniqueSKU_PriceSubtractedSavingFromTwoPromotionRules()
    {
        // Test Scenario B

        // Arrange
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "A", "A", "A", "B", "B", "B", "B", "B", "C"};
        var counts = stockKeepingUnits.CountSKU();

        // Act
        int totalPrice = counts.TotalPrice();

        // Assert
        int expectedTotal = 5*PromotionEngineLibrary.PriceA + 5*PromotionEngineLibrary.PriceB + PromotionEngineLibrary.PriceC 
        - PromotionEngineLibrary.Promotion3AsSaving - 2*PromotionEngineLibrary.Promotion2BsSaving;
        bool result = expectedTotal == totalPrice & expectedTotal == 370;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [Test]
    public void TotalPrice_InputCartWith3UniqueSKU_PriceSubtractedSavingFromThreePromotionRules()
    {
        // Test Scenario C

        // Arrange
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "A", "B", "B", "B", "B", "B", "C", "D"};
        var counts = stockKeepingUnits.CountSKU();
        
        // Act
        int totalPrice = counts.TotalPrice();

        // Assert
        int expectedTotal = 3*PromotionEngineLibrary.PriceA + 5*PromotionEngineLibrary.PriceB + PromotionEngineLibrary.PriceC 
        + PromotionEngineLibrary.PriceD - PromotionEngineLibrary.Promotion3AsSaving - 2*PromotionEngineLibrary.Promotion2BsSaving 
        - PromotionEngineLibrary.PromotionCandDSaving;
        bool result = expectedTotal == totalPrice & expectedTotal == 280;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [Test]
    public void CreatePromotion2ItemsForFixedPrice_TwoUniqueSKUAndPriceOfPromotionRule_PopulatedVariableValuesOnNewlyCreatedPromotionRuleMatchesInputVariables()
    {
        // Arrange
        int price = 30;
        string item_i = "C";
        string item_j = "D";
        List<string> expected = new List<string>{item_i, item_j, price.ToString()};
        List<PromotionRule> PromotionRules = new List<PromotionRule>();

        // Act
        PromotionRules.CreatePromotion2ItemsForFixedPrice(item_i, item_j, price);
        var promotionRule = PromotionRules.ElementAt(0);
        
        // Assert
        bool result = false;
        if (!string.IsNullOrEmpty(promotionRule.Item_j))
            result = promotionRule.Item_j.Equals(item_j) & promotionRule.Item_j.Equals(item_j) & promotionRule.Price.Equals(price);
        Assert.IsTrue(result, String.Format("Expected item '{0}': true, but actual item '{1}': {2}", String.Join(",", expected), promotionRule.PrintRule, result));
    }

    [Test]
    public void TotalPriceUsingPromotionRules_PromotionRulesWith2ItemsForFixedPrice_PriceMatchesReturnValueOfMethodTotalPrice()
    {
        // Arrange
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "B", "C", "D"};
        var counts = stockKeepingUnits.CountSKU();

        // Create Promotion rule
        int price = 30;
        string item_i = "C";
        string item_j = "D";
        List<PromotionRule> PromotionRules = new List<PromotionRule>();
        PromotionRules.CreatePromotion2ItemsForFixedPrice(item_i, item_j, price);

        // Act
        var totalPrice = counts.TotalPriceUsingPromotionRules(PromotionRules);
        
        // Assert
        int expectedTotal = counts.TotalPrice();
        bool result = expectedTotal == totalPrice;        
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [Test]
    public void TotalPriceUsingPromotionRules_PromotionRuleCreatedWithMethodCreatePromotionNItemsForFixedPrice_PricesIsSubtractedSavingsOfPromotionRule()
    {
        // Arrange
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "A", "B", "C"};
        var counts = stockKeepingUnits.CountSKU();
        
        // Create Promotion rule
        int price = 130;
        int nItems = 3;
        string item_i = "A";
        List<PromotionRule> PromotionRules = new List<PromotionRule>();
        PromotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        // Act
        var totalPrice = counts.TotalPriceUsingPromotionRules(PromotionRules);

        // Assert
        int expectedTotal = counts.TotalPrice();
        bool result = expectedTotal == totalPrice;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [Test]
    public void TotalPriceUsingPromotionRules_PromotionRuleCreatedWithMethodsCreatePromotionNItemsForFixedPriceAndMethodAndCreatePromotion2ItemsForFixedPrice_PriceIsSubtractedSavingsOfThreePromotionRules()
    {
        // Arrange
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "A", "B", "B", "B", "B", "B", "C", "D"};
        var counts = stockKeepingUnits.CountSKU();
        List<PromotionRule> PromotionRules = new List<PromotionRule>();
        Create3PromotionRules(PromotionRules);

        // Act
        var totalPrice = counts.TotalPriceUsingPromotionRules(PromotionRules);

        // Assert
        int expectedTotal = counts.TotalPrice();
        bool result = expectedTotal == totalPrice;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    private static void Create3PromotionRules(List<PromotionRule> PromotionRules)
    {
        // Create Promotion rule
        int price = 130;
        int nItems = 3;
        string item_i = "A";
        PromotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        // Create Promotion rule
        price = 45;
        nItems = 2;
        item_i = "B";
        PromotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        // Create Promotion rule
        price = 30;
        item_i = "C";
        string item_j = "D";
        PromotionRules.CreatePromotion2ItemsForFixedPrice(item_i, item_j, price);
    }    

    [Test]
    public void TotalPriceUsingPromotionRules_InputCartWith100RandomSKUs_PriceIsSubtractedSavingsOfThreePromotionRules()
    {
        // Arrange     
        IEnumerable<string> randomSKU = new List<string>(new string[100]);
        Random random = new Random();
        randomSKU = randomSKU.Select(x => PromotionEngineLibrary.ProductList.ToList<string>()[random.Next(100)]);
        var counts = randomSKU.CountSKU();
        List<PromotionRule> PromotionRules = new List<PromotionRule>();
        Create3PromotionRules(PromotionRules);

        // Act
        var totalPrice = counts.TotalPriceUsingPromotionRules(PromotionRules);

        // Assert
        int expectedTotal = counts.TotalPrice();
        bool result = expectedTotal == totalPrice;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [TestCase(500)]
    [TestCase(50)]
    [TestCase(5)]
    public void CustomTiming_PrerequisiteCodePartsForMethodTotalPriceUsingPromotionRules_MethodCurrentOfMiniProfilerShowsExecutionTimeOfCodePartsThatLeadsToPriceComputation(int value)
    {
        // Arrange
        var profiler = MiniProfiler.StartNew("Promotion Engine Profiler");

        // Act
        using (profiler.Step("Outer Scope"))
        {
            // Todo: Check that in case of a big cart with many SKU ids how the code manages and clears memory
            IEnumerable<string> randomSKU;
            using (profiler.Step($"Create cart with {value} random items"))
            {
                randomSKU = new List<string>(new string[value]);
                Random random = new Random();
                randomSKU = randomSKU.Select(x => PromotionEngineLibrary.ProductList.ToList<string>()[random.Next(PromotionEngineLibrary.ProductList.Count())]);
            }

            IEnumerable<int> counts;
            using (profiler.CustomTiming("CountSKU", "test method"))
            {
                counts = randomSKU.CountSKU();
            }

            List<PromotionRule> PromotionRules = new List<PromotionRule>();

            using (profiler.Step("Add Promotion rules"))
            {
                Create3PromotionRules(PromotionRules);
            }

            using (profiler.CustomTiming("TotalPriceUsingPromotionRules", "test method"))
            {
                var totalPrice = counts.TotalPriceUsingPromotionRules(PromotionRules);
            }
        }

        // Assert

        // Console.WriteLine(profiler.RenderPlainText());
        // Todo: use execution time data in a system independent Mock. Ex. set a limit on percentage of time spent in code scopes relative to the system.
        Console.WriteLine(MiniProfiler.Current.RenderPlainText());
        // Todo: Assert.Fail() will display output lines written to console.
        // Assert.Fail();
    }

    [Test]
    public void TestOverlappingPromotionRules()
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

        List<int> rulesAppliedCount = counts.OptimizeRulesApplied();
        var overlaps = OverlappingPromotionRules(rulesAppliedCount);
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