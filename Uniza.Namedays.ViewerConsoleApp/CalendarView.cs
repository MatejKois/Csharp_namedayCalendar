namespace Uniza.Namedays.ViewerConsoleApp
{
    internal class CalendarView
    {
        /// <summary>
        /// Nameday calendar
        /// </summary>
        private readonly NamedayCalendar _namedayCalendar;

        /// <summary>
        /// A reference DateTime to add / substract to / from, tells which month will be displayed
        /// </summary>
        private DateTime _displayDateTime;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="namedayCalendar">Nameday calendar to take data from</param>
        public CalendarView(NamedayCalendar namedayCalendar)
        {
            _namedayCalendar = namedayCalendar;
            _displayDateTime = DateTime.Now;
            Start();
        }

        /// <summary>
        /// Starts the displaying loop, exits on enter keypress
        /// </summary>
        private void Start()
        {
            while (true)
            {
                Console.Clear();
                Display();

                Console.WriteLine("Sipka dolava / doprava - mesiac dozadu / dopredu");
                Console.WriteLine("Sipka hore   / dole    - rok    dozadu / dopredu");
                Console.WriteLine("Klaves Home alebo D    - aktualny den");
                Console.WriteLine("Pre ukoncenie stlacte Enter");

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        _displayDateTime = _displayDateTime.AddMonths(-1);
                        continue;
                    case ConsoleKey.RightArrow:
                        _displayDateTime = _displayDateTime.AddMonths(1);
                        continue;
                    case ConsoleKey.UpArrow:
                        _displayDateTime = _displayDateTime.AddYears(-1);
                        continue;
                    case ConsoleKey.DownArrow:
                        _displayDateTime = _displayDateTime.AddYears(1);
                        continue;
                    case ConsoleKey.Home:
                    case ConsoleKey.D:
                        _displayDateTime = DateTime.Now;
                        continue;
                    case ConsoleKey.Enter:
                        break;
                }
                break;
            }
        }

        /// <summary>
        /// Displays all namedays in month
        /// </summary>
        private void Display()
        {
            Console.WriteLine("KALENDAR MENIN");
            Console.WriteLine(_displayDateTime.ToString("MMMM yyyy"));

            for (int i = 1; i <= DateTime.DaysInMonth(_displayDateTime.Year, _displayDateTime.Month); i++)
            {
                DateTime toPrint = new DateTime(_displayDateTime.Year, _displayDateTime.Month, i);
                var namesAtDate = _namedayCalendar[toPrint];

                if (toPrint.DayOfWeek == DayOfWeek.Sunday || toPrint.DayOfWeek == DayOfWeek.Saturday)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                if (toPrint == DateTime.Today)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }

                Console.Write(toPrint.ToString("dd.MM ddd "));

                if (namesAtDate.Length == 0)
                {
                    Console.WriteLine("nema nikto meniny");
                }
                else
                {
                    string namesToPrint = "";

                    foreach (var name in namesAtDate)
                    {
                        if (namesToPrint.Length > 0)
                        {
                            namesToPrint += ", ";
                        }
                        namesToPrint += name;
                    }

                    Console.WriteLine(namesToPrint);
                }

                Console.ResetColor();
            }
        }
    }
}
