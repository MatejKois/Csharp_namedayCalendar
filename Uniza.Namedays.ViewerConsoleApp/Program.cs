using Uniza.Namedays.ViewerConsoleApp;

if (args.Length > 0)
{
    try
    {
        FileInfo fileInfo = new FileInfo("namedays-sk.csv");
    }
    catch (Exception)
    {
        Console.WriteLine("Nespravna cesta k suboru!");
    }
    new ConsoleViewer(args[0]).MainLoop();
}
else
{
    new ConsoleViewer().MainLoop();
}
