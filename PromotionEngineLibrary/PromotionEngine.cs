using System.Collections.Generic;

namespace Promotion.Engine.Library;
public static class PromotionEngineLibrary
{
    private static int _priceA = 50;
    private static int _priceB = 30;
    private static int _priceC = 20;
    private static int _priceD = 15;

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

    public static int TotalPrice(this IEnumerable<string>? str)
    {
        if (str.Contains("A"))
            return 50;
        else
            return 0;
    }

}
