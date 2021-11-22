﻿using System.Collections.Generic;

namespace Promotion.Engine.Library;
public static class PromotionEngineLibrary
{
    public static List<string> ProductListSmall => new List<string>{"A", "B", "C", "D", "E"};
    public static List<string> ProductList => ProductListSmall.Concat(new List<string>(Enumerable.Range(0,100).Select(x => String.Format("item_{0}", x)))).ToList<string>();

    public static int PriceA => 50;
    public static int PriceB => 30;
    public static int PriceC => 20;
    public static int PriceD => 15;
    public static int PriceE => 15;

    public static IEnumerable<int> PricesSmall => new List<int>{PriceA, PriceB, PriceC, PriceD, PriceE};
    public static IEnumerable<int> Prices => PricesSmall.Concat(new List<int>(new int[100]));

    public static int Promotion3As => 130;
    public static int Promotion3AsSaving => PriceA*3 - Promotion3As;
    
    public static int Promotion2Bs => 45;
    public static int Promotion2BsSaving => PriceB*2 - Promotion2Bs;
    
    public static int PromotionCandD => 30;
    public static int PromotionCandDSaving => PriceC + PriceD - PromotionCandD;

    public static IEnumerable<int> CountSKU(this IEnumerable<string>? sku)
    {
        IList<int> counts = new List<int>(new int[ProductList.Count()]);

        // In order to sort by products in ProductList add the elements in ProductList to sku and subtract 1 occurence afterwards
        List<string> skuWithAddedProductList = sku.ToList<string>();
        skuWithAddedProductList.AddRange(ProductList);

        var uniquesGroupedQuery = from occurence in skuWithAddedProductList group occurence by occurence;
        var orderGroupedQuery = uniquesGroupedQuery.Select(x => x.FirstOrDefault()).ToList();
        
        // Subtract 1 occurence as mentioned earlier
        var unorderedCounts = uniquesGroupedQuery.Select(x => x.Count() - 1);
        var indices = orderGroupedQuery.Select(x => ProductList.IndexOf(x)).ToList();
        var ite = 0;
        foreach (var unorderedCount in unorderedCounts)
        {
            counts[indices[ite]] = unorderedCount;
            ite++;
        }
        return counts;
    }

    public static int TotalPrice(this IEnumerable<int>? counts)
    {
        int priceWithoutPromotion = 0;
        foreach (var ite in Enumerable.Range(0, counts.Count()))
        {
            priceWithoutPromotion += counts.ElementAt(ite)*Prices.ElementAt(ite);
        }

        int Promotion3AsCount = counts.ElementAt(0)/3;
        int Promotion3AsTotalSaving = Promotion3AsCount*Promotion3AsSaving;
        
        int Promotion2BsCount = counts.ElementAt(1)/2;
        int Promotion2BsTotalSaving = Promotion2BsCount*Promotion2BsSaving;

        int PromotionCandDCount = Math.Min(counts.ElementAt(2), counts.ElementAt(3));
        int PromotionCandDTotalSaving = PromotionCandDCount*PromotionCandDSaving;

        int totalPromotionSaving = Promotion3AsTotalSaving + Promotion2BsTotalSaving + PromotionCandDTotalSaving;

        return priceWithoutPromotion - totalPromotionSaving;
    }

    public static void CreatePromotion2ItemsForFixedPrice(this List<PromotionRule>? PromotionRules, string item_i, string item_j, int price)
    {

        // Anonymous function for Occurences in case of 2ItemsForFixedPrice
        Func<IEnumerable<int>?, int, int, int> OccurencesDelegate = delegate(IEnumerable<int>? counts, int idx_i, int idx_j)
        {
            var occurences = Math.Min(counts.ElementAt(idx_i), counts.ElementAt(idx_j));
            return occurences;
        };

        var idx_i = ProductList.IndexOf(item_i);
        var idx_j = ProductList.IndexOf(item_j);
        var saving = Prices.ElementAt(idx_i) + Prices.ElementAt(idx_j) - price;
        PromotionRule promotionRule = new PromotionRule(item_i, item_j, idx_i, idx_j, price, saving, OccurencesDelegate);
        PromotionRules.Add(promotionRule);
    }

    public static void CreatePromotionNItemsForFixedPrice(this List<PromotionRule>? PromotionRules, int nItems, string item_i, int price)
    {
        // Anonymous function for Occurences in case of NItemsForFixedPrice
        Func<IEnumerable<int>?, int, int, int> OccurencesDelegate = delegate(IEnumerable<int>? counts, int idx_i, int nItems)
        {
            var occurences = counts.ElementAt(idx_i)/nItems;
            return occurences;
        };

        var idx_i = ProductList.IndexOf(item_i);
        var saving = nItems*Prices.ElementAt(idx_i) - price;
        PromotionRule promotionRule = new PromotionRule(item_i, null, idx_i, nItems, price, saving, OccurencesDelegate);
        PromotionRules.Add(promotionRule);
    }

    public static int TotalPriceUsingPromotionRules(this IEnumerable<int>? counts, IEnumerable<PromotionRule> promotionRules)
    {
        int priceWithoutPromotion = 0;
        foreach (var ite in Enumerable.Range(0, counts.Count()))
            priceWithoutPromotion += counts.ElementAt(ite)*Prices.ElementAt(ite);

        var totalPromotionSaving = 0;
        foreach (var rule in promotionRules)
        {
            totalPromotionSaving += rule.TotalPromotionSaving(counts);
        }

        return priceWithoutPromotion - totalPromotionSaving;
    }
}
