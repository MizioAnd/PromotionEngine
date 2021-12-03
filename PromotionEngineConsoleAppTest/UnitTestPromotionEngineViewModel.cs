using NUnit.Framework;
using System;
using Promotion.Engine.ConsoleApp;
using Promotion.Engine.Library;
using System.Collections.Generic;

namespace Promotion.Engine.ConsoleApp.UnitTests;
public class UnitTestPromotionEngineViewModel
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestComputeTotalPriceFor3Rules()
    {   
        var input = "A,A,A,B,B,B,B,B,C,D";
        IEnumerable<string> stockKeepingUnits = new List<string>(input.Split(","));
        var counts = stockKeepingUnits.CountSKU();
        
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

        var expectedTotal = counts.TotalPriceUsingPromotionRules(PromotionRules);

        PromotionEngineViewModel promotionEngineViewModel = new PromotionEngineViewModel();
        promotionEngineViewModel.Input = input;
        var totalPrice = promotionEngineViewModel.TotalPrice;
        var result = expectedTotal == totalPrice;
        Assert.IsTrue(result, String.Format("Expected total price '{0}': true, but actual price '{1}': {2}", expectedTotal, totalPrice, result));
    }

    [Test]
    public void TestTotalPriceAfterManuallyAddingExtraPromotionRule()
    {
        var input = "A,A,A,B,B,B,B,B,C,D,E,E";
        PromotionEngineViewModel promotionEngineViewModel = new PromotionEngineViewModel();
        promotionEngineViewModel.Input = input;
        var totalPrice = promotionEngineViewModel.TotalPrice;

        // Create Promotion rule for "E"
        var price = 25;
        var nItems = 2;
        var item_i = "E";
        promotionEngineViewModel.PromotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        var totalPriceAfterAddingRule = promotionEngineViewModel.TotalPrice;
        var result = totalPriceAfterAddingRule != totalPrice;
        Assert.IsTrue(result, String.Format("totalPriceAfterAddingRule '{0}': true, and totalt price before '{1}' do not match: {2}", totalPriceAfterAddingRule, totalPrice, result));
    }
}