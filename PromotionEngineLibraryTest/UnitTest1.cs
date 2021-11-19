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


}