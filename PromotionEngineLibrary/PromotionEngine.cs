using System.Collections.Generic;

namespace Promotion.Engine.Library;
public static class PromotionEngineLibrary
{
    public static int PriceA => 50;
    public static int PriceB => 30;
    public static int PriceC => 20;
    public static int PriceD => 15;

    public static IEnumerable<int> Prices => new List<int>{PriceA, PriceB, PriceC, PriceD};

    public static int Promotion3As => 130;
    public static int Promotion3AsSaving => PriceA*3 - Promotion3As;
    
    public static int Promotion2Bs => 45;
    public static int Promotion2BsSaving => PriceB*2 - Promotion2Bs;
    
    public static int PromotionCandD => 30;
    public static int PromotionCandDSaving => PriceC + PriceD - PromotionCandD;

    public static IEnumerable<int> CountSKU(this IEnumerable<string>? sku)
    {
        IList<int> counts = new List<int>{0, 0, 0, 0};

        foreach(string i in sku)
        {
            switch(i)
            {
                case "A":
                    counts[0] += 1;
                    break;
                case "B":
                    counts[1] += 1;
                    break;
                case "C":
                    counts[2] += 1;
                    break;
                case "D":
                    counts[3] += 1;
                    break;
            }
        }
        return counts;
    }

    public static int TotalPrice(this IEnumerable<int>? counts)
    {
        // Todo: implement modular expression that uses promotion saving together with price without promotions.
        // int priceWithoutPromotion = counts.ElementAt(0)*PriceA + counts.ElementAt(1)*PriceB;
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
        PromotionRule promotionRule = new PromotionRule(item_i, item_j, price);
        PromotionRules.Add(promotionRule);
    }

    public static int TotalPriceUsingPromotionRules(this IEnumerable<int>? counts)
    {
        return 0;
    }
}
