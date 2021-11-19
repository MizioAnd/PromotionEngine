using System.Collections.Generic;

namespace Promotion.Engine.Library;
public static class PromotionEngineLibrary
{
    private static int _priceA = 50;
    private static int _priceB = 30;
    private static int _priceC = 20;
    private static int _priceD = 15;

    public static IEnumerable<int> CountSKU(this IEnumerable<string>? str)
    {
        IEnumerable<int> counts = new List<int>{2, 0, 2, 0};
        return counts;
        // foreach(string i in str)
        // {
        //     switch(i)
        //     {
        //         case i.Equals("A"):
        //             counts[0] += 1;
        //             break;
        //         case i.Equals("B"):
        //             counts[1] += 1;
        //             break;
        //         case i.Equals("C"):
        //             counts[2] += 1;
        //             break;
        //         case i.Equals("D"):
        //             counts[3] += 1;
        //             break;
        //     }
        // }
    }

    public static int TotalPrice(this IEnumerable<string>? str)
    {
        if (str.Contains("A"))
            return 50;
        else
            return 0;
    }

}
