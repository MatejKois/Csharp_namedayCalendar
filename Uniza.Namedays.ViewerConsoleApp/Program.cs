using Uniza.Namedays;

FileInfo file =
    new FileInfo(
        "D:\\Files\\__UNIZA\\S6\\JCN\\Zadanie2\\Namedays\\Uniza.Namedays\\Uniza.Namedays.ViewerConsoleApp\\namedays-sk.csv");

NamedayCalendar calendar = new NamedayCalendar();
calendar.Load(file);

Console.WriteLine();
