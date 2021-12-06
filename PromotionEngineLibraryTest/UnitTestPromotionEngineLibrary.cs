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
    public void TestTotalPrice()
    {
        // SKUs {"A", "B", "A", "B"}
        IEnumerable<int> counts = new List<int>{2, 2, 0, 0};
        // int expectedTotal = 45 + 2*50;
        int expectedTotal = 2*PromotionEngineLibrary.PriceA + 2*PromotionEngineLibrary.PriceB - PromotionEngineLibrary.Promotion2BsSaving;
        int totalPrice = counts.TotalPrice();
        bool result = expectedTotal == totalPrice;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
        // Assert.Pass();
    }

    [Test]
    public void TestCountSKU()
    {
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "B", "A", "B"};
        var counts = stockKeepingUnits.CountSKU();
        IEnumerable<int> expected = (new List<int>{2, 2, 0, 0, 0}).Concat(new List<int>(new int[PromotionEngineLibrary.ProductList.Count() - 5]));
        bool result = counts.SequenceEqual(expected);
        Assert.IsTrue(result, String.Format("Expected counts '{0}': true, but actual counts '{1}': {2}", String.Join(",", expected), String.Join(",", counts), result));
    }

    [Test]
    public void TestCountSKUGenerality()
    {
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "B", "A", "B", "E"};
        var counts = stockKeepingUnits.CountSKU();
        IEnumerable<int> expected = (new List<int>{2, 2, 0, 0, 1}).Concat(new List<int>(new int[PromotionEngineLibrary.ProductList.Count() - 5]));
        bool result = counts.SequenceEqual(expected);
        Assert.IsTrue(result, String.Format("Expected counts '{0}': true, but actual counts '{1}': {2}", String.Join(",", expected), String.Join(",", counts), result));
    }

    [Test]
    public void TestScenarioA()
    {
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "B", "C"};
        var counts = stockKeepingUnits.CountSKU();
        int expectedTotal = PromotionEngineLibrary.PriceA + PromotionEngineLibrary.PriceB + PromotionEngineLibrary.PriceC;
        int totalPrice = counts.TotalPrice();
        bool result = expectedTotal == totalPrice & expectedTotal == 100;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [Test]
    public void TestScenarioB()
    {
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "A", "A", "A", "B", "B", "B", "B", "B", "C"};
        var counts = stockKeepingUnits.CountSKU();
        int expectedTotal = 5*PromotionEngineLibrary.PriceA + 5*PromotionEngineLibrary.PriceB + PromotionEngineLibrary.PriceC - PromotionEngineLibrary.Promotion3AsSaving - 2*PromotionEngineLibrary.Promotion2BsSaving;
        int totalPrice = counts.TotalPrice();
        bool result = expectedTotal == totalPrice & expectedTotal == 370;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [Test]
    public void TestScenarioC()
    {
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "A", "B", "B", "B", "B", "B", "C", "D"};
        var counts = stockKeepingUnits.CountSKU();
        int expectedTotal = 3*PromotionEngineLibrary.PriceA + 5*PromotionEngineLibrary.PriceB + PromotionEngineLibrary.PriceC + PromotionEngineLibrary.PriceD - PromotionEngineLibrary.Promotion3AsSaving - 2*PromotionEngineLibrary.Promotion2BsSaving - PromotionEngineLibrary.PromotionCandDSaving;
        int totalPrice = counts.TotalPrice();
        bool result = expectedTotal == totalPrice & expectedTotal == 280;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [Test]
    public void TestCreatePromotion2ItemsForFixedPrice()
    {
        // Adds promotion rule to a collection of promotion rules
        int price = 30;
        string item_i = "C";
        string item_j = "D";
        List<string> expected = new List<string>{item_i, item_j, price.ToString()};
        List<PromotionRule> PromotionRules = new List<PromotionRule>();
        PromotionRules.CreatePromotion2ItemsForFixedPrice(item_i, item_j, price);
        var promotionRule = PromotionRules.ElementAt(0);
        bool result = false;
        if (!string.IsNullOrEmpty(promotionRule.Item_j))
            result = promotionRule.Item_j.Equals(item_j) & promotionRule.Item_j.Equals(item_j) & promotionRule.Price.Equals(price);
        Assert.IsTrue(result, String.Format("Expected item '{0}': true, but actual item '{1}': {2}", String.Join(",", expected), promotionRule.PrintRule, result));
    }

    [Test]
    public void TestTotalPriceUsingPromotionRulesWith2ItemsForFixedPrice()
    {
        // Compute total price using TotalPrice() method that has previously been tested
        // Use SKUs where only C and D promotion is effective in TotalPrice()
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "B", "C", "D"};
        var counts = stockKeepingUnits.CountSKU();
        int expectedTotal = counts.TotalPrice();

        // Create Promotion rule
        int price = 30;
        string item_i = "C";
        string item_j = "D";
        List<PromotionRule> PromotionRules = new List<PromotionRule>();
        PromotionRules.CreatePromotion2ItemsForFixedPrice(item_i, item_j, price);
        var totalPrice = counts.TotalPriceUsingPromotionRules(PromotionRules);
        bool result = expectedTotal == totalPrice;        
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [Test]
    public void TestTotalPriceUsingPromotionRulesNItemsForFixedPrice()
    {
        // Compute total price using TotalPrice() method that has previously been tested
        // Use SKUs where only 3 of A's promotion is effective in TotalPrice()
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "A", "B", "C"};
        var counts = stockKeepingUnits.CountSKU();
        int expectedTotal = counts.TotalPrice();

        // Create Promotion rule
        int price = 130;
        int nItems = 3;
        string item_i = "A";
        
        List<PromotionRule> PromotionRules = new List<PromotionRule>();
        PromotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);
        var totalPrice = counts.TotalPriceUsingPromotionRules(PromotionRules);
        bool result = expectedTotal == totalPrice;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [Test]
    public void TestTotalPriceUsingPromotionRulesNItemsForFixedPriceAnd2ItemsForFixedPrice()
    {
        // Compute total price using TotalPrice() method that has previously been tested
        // Use SKUs where 3 of A's promotion, 2 of B's promotion, and C and D promotion are effective in TotalPrice()
        IEnumerable<string> stockKeepingUnits = new List<string>{"A", "A", "A", "B", "B", "B", "B", "B", "C", "D"};
        var counts = stockKeepingUnits.CountSKU();
        int expectedTotal = counts.TotalPrice();
        
        List<PromotionRule> PromotionRules = new List<PromotionRule>();

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

        var totalPrice = counts.TotalPriceUsingPromotionRules(PromotionRules);
        bool result = expectedTotal == totalPrice;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [Test]
    public void TestBigCartWithRandomItems()
    {     
        IEnumerable<string> randomSKU = new List<string>(new string[100]);
        Random random = new Random();

        randomSKU = randomSKU.Select(x => PromotionEngineLibrary.ProductList.ToList<string>()[random.Next(100)]);
        
        var counts = randomSKU.CountSKU();
        int expectedTotal = counts.TotalPrice();

        List<PromotionRule> PromotionRules = new List<PromotionRule>();

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

        var totalPrice = counts.TotalPriceUsingPromotionRules(PromotionRules);
        bool result = expectedTotal == totalPrice;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [TestCase(500)]
    [TestCase(50)]
    [TestCase(5)]
    public void TestPromotionEngineProfiler(int value)
    {
        var profiler = MiniProfiler.StartNew("Promotion Engine Profiler");

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

            using (profiler.CustomTiming("TotalPriceUsingPromotionRules", "test method"))
            {
                var totalPrice = counts.TotalPriceUsingPromotionRules(PromotionRules);
            }
        }
        // Console.WriteLine(profiler.RenderPlainText());
        Console.WriteLine(MiniProfiler.Current.RenderPlainText());
        // Assert.Fail();
    }
}