using Promotion.Engine.Library;

class Program
{
    static void Main(string[] args)
    {
        IEnumerable<string> stockKeepingUnitsExample = new List<string>{"A", "A", "A", "B", "B", "B", "B", "B", "C", "D"};
        List<PromotionRule> PromotionRules = new List<PromotionRule>();

        // Create Promotion rule
        int price = 130;
        int nItems = 3;
        string item_i = "A";
        PromotionRules.CreatePromotionNItemsForFixedPrice(nItems, item_i, price);

        // Create Promotion rule
        int priceBs = 45;
        int nItemsBs = 2;
        string item_i_Bs = "B";
        PromotionRules.CreatePromotionNItemsForFixedPrice(nItemsBs, item_i_Bs, priceBs);

        // Create Promotion rule
        price = 30;
        item_i = "C";
        string item_j = "D";
        PromotionRules.CreatePromotion2ItemsForFixedPrice(item_i, item_j, price);

        int rowsWrittenToConsolesWindow = 0;

        do
        {
            if (rowsWrittenToConsolesWindow == 0 | rowsWrittenToConsolesWindow >= 44)
            {
                ResetConsoleWindow();
            }
            Console.WriteLine(String.Format("Example of an input cart: {0}", String.Join("", String.Join(",", stockKeepingUnitsExample))));
            Console.WriteLine("Enter your input cart:");
            try {

                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    break;
                Console.WriteLine($"Your input cart: {input}");
                Console.WriteLine();

                // Todo: Check format of input. It should be cast to IEnumerable<string> like stockKeepingUnits
                IEnumerable<string> stockKeepingUnits = new List<string>(input.Split(","));

                var counts = stockKeepingUnits.CountSKU();

                var totalPrice = counts.TotalPriceUsingPromotionRules(PromotionRules);

                Console.WriteLine("Your total price: {0}", totalPrice);
                Console.WriteLine();
                Console.WriteLine(("").PadRight(20, '-'));
                Console.WriteLine();
                Console.WriteLine("Your next input cart...");

            } catch (ArgumentOutOfRangeException e) {
                Console.WriteLine("--> Your input format was not correct");
                Console.WriteLine("Try entering");
                Console.WriteLine("A,B");
                Console.WriteLine();
            }
            
            rowsWrittenToConsolesWindow += 9;
        } while (true);
        return;

        void ResetConsoleWindow()
        {
            if (rowsWrittenToConsolesWindow > 0)
            {
                Console.WriteLine("Press a key to continue");
                Console.ReadKey();
            }
            Console.Clear();
            Console.WriteLine($"{Environment.NewLine}Press <enter> only to exit; othwerwise enter your input cart and then <enter>:{Environment.NewLine}");
            rowsWrittenToConsolesWindow = 3;
        }
    }
}