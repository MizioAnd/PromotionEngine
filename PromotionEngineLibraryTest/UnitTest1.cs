using NUnit.Framework;
using Promotion.Engine.Library;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Promotion.Engine.Library.Test;
public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
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
        IEnumerable<int> expected = new List<int>{2, 2, 0, 0};
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
        // Todo: Rules should then be created that gets included in the engine.
        // adds promotion to a collection of promotion rules
        int price = 30;
        string item_i = "C";
        string item_j = "D";
        List<string> expected = new List<string>{item_i, item_j, price.ToString()};
        List<PromotionRule> PromotionRules = new List<PromotionRule>();
        PromotionRules.CreatePromotion2ItemsForFixedPrice(item_i, item_j, price);
        var promotionRule = PromotionRules.ElementAt(0);
        bool result = promotionRule.Item_j.Equals(item_j) & promotionRule.Item_j.Equals(item_j) & promotionRule.Price.Equals(price);
        Assert.IsTrue(result, String.Format("Expected item '{0}': true, but actual item '{1}': {2}", String.Join(",", expected), promotionRule.PrintRule, result));
    }
}