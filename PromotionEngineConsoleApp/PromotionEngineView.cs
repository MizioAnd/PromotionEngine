namespace Promotion.Engine.ConsoleApp;
public class PromotionEngineView
{
    int rowsWrittenToConsolesWindow;

    PromotionEngineViewModel promotionEngineViewModel;

    private string? Input
    {
        get { return promotionEngineViewModel.Input; }
        set { promotionEngineViewModel.Input = value; }
    }

    public PromotionEngineView()
    {
        rowsWrittenToConsolesWindow = 0;
        promotionEngineViewModel = new PromotionEngineViewModel();
    }

    public void CreateView()
    {
        do
        {
            if (rowsWrittenToConsolesWindow == 0 | rowsWrittenToConsolesWindow >= 44)
            {
                ResetConsoleWindow();
            }
            Console.WriteLine(String.Format("Example of an input cart: {0}", String.Join("", String.Join(",", PromotionEngineViewModel.StockKeepingUnitsExample))));
            Console.WriteLine("Enter your input cart:");
            try {
                Input = Console.ReadLine();

                if (string.IsNullOrEmpty(Input))
                    break;
                
                Console.WriteLine($"Your input cart: {Input}");
                Console.WriteLine();
                
                Console.WriteLine("Your total price: {0}", promotionEngineViewModel.TotalPrice);
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
    }

    public void ResetConsoleWindow()
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