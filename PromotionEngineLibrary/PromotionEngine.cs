using System.Collections.Generic;

namespace Promotion.Engine.Library;
public static class PromotionEngineLibrary
{
    public static int PriceA => 50;
    public static int PriceB => 30;
    public static int PriceC => 20;
    public static int PriceD => 15;

    public static int Promotion3As => 130;
    public static int Promotion3AsSaving => PriceA*3 - 130;
    
    public static int Promotion2Bs => 45;
    public static int Promotion2BsSaving => PriceB*2 - 45;
    
    public static int PromotionCandD => 30;
    public static int PromotionCandDSaving => PriceC + PriceD - 30;


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

    public static int TotalPrice(this IEnumerable<int>? str)
    {
        int priceWithoutPromotion = 0;

        return 0;
    }
}
