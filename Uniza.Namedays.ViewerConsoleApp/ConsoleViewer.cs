namespace Uniza.Namedays.ViewerConsoleApp
{
    internal class ConsoleViewer
    {
        // consts -------------------------------
        private const ConsoleKey SelectionLoad = ConsoleKey.D1;
        private const ConsoleKey SelectionStatistics = ConsoleKey.D2;
        private const ConsoleKey SelectionSearchNames = ConsoleKey.D3;
        private const ConsoleKey SelectionSearchNamesByDate = ConsoleKey.D4;
        private const ConsoleKey SelectionDisplayCalendar = ConsoleKey.D5;
        private const ConsoleKey SelectionExitNumber = ConsoleKey.D6;
        private const ConsoleKey SelectionExitEsc = ConsoleKey.Escape;
        // --------------------------------------

        /// <summary>
        /// Nameday calendar which holds all data
        /// </summary>
        private readonly NamedayCalendar _namedayCalendar;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConsoleViewer()
        {
            _namedayCalendar = new NamedayCalendar();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="file">File to initialize with</param>
        public ConsoleViewer(string filepath)
        {
            _namedayCalendar = new NamedayCalendar();
            try
            {
                _namedayCalendar.Load(new FileInfo(VerifyFilename(filepath)));
                Console.WriteLine("Nacitanie prebehlo uspesne.");
                Console.WriteLine();
            }
            catch (Exception)
            {
                Console.WriteLine("Pri nacitani doslo k chybe...");
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Starts the main loop of the app
        /// </summary>
        public void MainLoop()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; // the console won't display diacritics without specifying this
            Console.InputEncoding = System.Text.Encoding.UTF8;
            while (true)
            {
                DisplayCurrentState();
                ShowMenu();
                TakeInput();
                Console.Clear();
            }
        }

        /// <summary>
        /// Displays current info about namedays
        /// </summary>
        private void DisplayCurrentState()
        {
            string[] namesToday = _namedayCalendar[DateTime.Today];
            string[] namesTomorrow = _namedayCalendar[DateTime.Today.AddDays(1)];

            Console.WriteLine("KALLENDAR MIEN");

            Console.Write($"Dnes {DateTime.Today.ToShortDateString()} ");
            if (namesToday.Length == 0)
            {
                Console.WriteLine("nema nikto meniny");
            }
            else
            {
                Console.Write("ma meniny ");
                string namesToDisplay = "";

                foreach (var name in namesToday)
                {
                    if (namesToDisplay.Length > 0)
                    {
                        namesToDisplay += ", ";
                    }
                    namesToDisplay += name;
                }

                Console.WriteLine(namesToDisplay);
            }

            Console.Write($"Zajtra {DateTime.Today.AddDays(1).ToShortDateString()} ");
            if (namesTomorrow.Length == 0)
            {
                Console.WriteLine("nema nikto meniny");
            }
            else
            {
                Console.Write("ma meniny ");
                string namesToDisplay = "";

                foreach (var name in namesTomorrow)
                {
                    if (namesToDisplay.Length > 0)
                    {
                        namesToDisplay += ", ";
                    }
                    namesToDisplay += name;
                }

                Console.WriteLine(namesToDisplay);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Shows menu options for the user
        /// </summary>
        private void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Menu");

            Console.WriteLine($"{SelectionLoad} - nacitat kalendar");
            Console.WriteLine($"{SelectionStatistics} - zobrazit statistiku");
            Console.WriteLine($"{SelectionSearchNames} - vyhladat mena");
            Console.WriteLine($"{SelectionSearchNamesByDate} - vyhladat mena podla datumu");
            Console.WriteLine($"{SelectionDisplayCalendar} - zobrazit kalendar mien v mesiaci");
            Console.WriteLine($"{SelectionExitNumber} | {SelectionExitEsc} - koniec");
        }

        /// <summary>
        /// Handles input from the user and calls relevant methods
        /// </summary>
        private void TakeInput()
        {
            while (true)
            {
                Console.WriteLine("Vasa volba:");
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case SelectionLoad:
                        OnSelectionLoad();
                        break;
                    case SelectionStatistics:
                        OnSelectionStatistics();
                        break;
                    case SelectionSearchNames:
                        OnSelectionSearchNames();
                        break;
                    case SelectionSearchNamesByDate:
                        OnSelectionSearchNamesByDate();
                        break;
                    case SelectionDisplayCalendar:
                        OnSelectionDisplayCalendar();
                        break;
                    case SelectionExitNumber:
                    case SelectionExitEsc:
                        OnSelectionExit();
                        break;
                    default:
                        Console.WriteLine("Nespravna volba!");
                        continue;
                }
                break;
            }
        }

        /// <summary>
        /// Is called when user selects load option
        /// </summary>
        private void OnSelectionLoad()
        {
            Console.Clear();
            Console.WriteLine("OTVORENIE");
            Console.WriteLine("Zadajte cestu k suboru kalendara mien alebo stlacte Enter pre ukoncenie.");
            while (true)
            {
                Console.Write("Zadajte cestu k .csv suboru: ");

                string? filename = Console.ReadLine();
                if (string.IsNullOrEmpty(filename))
                {
                    break;
                }

                try
                {
                    string verifiedFilename = VerifyFilename(filename);
                    FileInfo file = new FileInfo(verifiedFilename);
                    _namedayCalendar.Load(file);
                    Console.WriteLine("Nacitanie prebehlo uspesne.");
                    Console.WriteLine("Pre navrat do menu stlacte Enter.");
                    Console.ReadKey(true);
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Zadany subor {filename} neexistuje!");
                }
            }
        }

        /// <summary>
        /// Verifies filename extension and modifies it if needed
        /// </summary>
        /// <param name="filename">Filename to verify</param>
        /// <exception cref="Exception"></exception>
        private string VerifyFilename(string filename)
        {
            string[] splittedFilename = filename.Split(".");

            if (splittedFilename.Length > 2)
            {
                throw new Exception("Incorrect filename format!");
            }

            if (splittedFilename.Length == 2)
            {
                if (splittedFilename[1] == "csv")
                {
                    return filename;
                }

                Console.WriteLine($"Pripona opravena -> {splittedFilename[0] + ".csv"}");
                return splittedFilename[0] + ".csv";
            }

            if (splittedFilename.Length == 1)
            {
                Console.WriteLine($"Doplnena pripona -> {filename + ".csv"}");
                return filename + ".csv";
            }

            throw new Exception("Empty filename!");
        }

        /// <summary>
        /// Is called when user selects stats option
        /// </summary>
        private void OnSelectionStatistics()
        {
            // groups names by start letter
            var nameStartLetterGroups = _namedayCalendar.GetNamedays()
                .GroupBy(nd => nd.Name.Length > 0 ? nd.Name.Substring(0, 1) : "-")
                .OrderBy(g => g.Key);

            // groups names by their string length
            var nameLengthGroups = _namedayCalendar.GetNamedays()
                .GroupBy(nd => nd.Name.Length)
                .OrderBy(g => g.Key);

            Console.Clear();
            Console.WriteLine("STATISTIKA");

            Console.WriteLine($"Celkovy pocet mien v kalendari: {_namedayCalendar.NameCount}");

            Console.WriteLine($"Celkovy pocet dni obsahujucich mena v kalendari: {_namedayCalendar.DayCount}");

            Console.WriteLine("Celkovy pocet mien v jednotlivych mesiacoch:");
            Console.WriteLine($"  januar: {_namedayCalendar.GetNamedays(1).Length}");
            Console.WriteLine($"  februar: {_namedayCalendar.GetNamedays(2).Length}");
            Console.WriteLine($"  marec: {_namedayCalendar.GetNamedays(3).Length}");
            Console.WriteLine($"  april: {_namedayCalendar.GetNamedays(4).Length}");
            Console.WriteLine($"  maj: {_namedayCalendar.GetNamedays(5).Length}");
            Console.WriteLine($"  jun: {_namedayCalendar.GetNamedays(6).Length}");
            Console.WriteLine($"  jul: {_namedayCalendar.GetNamedays(7).Length}");
            Console.WriteLine($"  august: {_namedayCalendar.GetNamedays(8).Length}");
            Console.WriteLine($"  semtember: {_namedayCalendar.GetNamedays(9).Length}");
            Console.WriteLine($"  oktober: {_namedayCalendar.GetNamedays(10).Length}");
            Console.WriteLine($"  november: {_namedayCalendar.GetNamedays(11).Length}");
            Console.WriteLine($"  december: {_namedayCalendar.GetNamedays(12).Length}");

            Console.WriteLine("Pocet mien podla zaciatocnych pismen:");
            foreach (var nameStartLetterGroup in nameStartLetterGroups)
            {
                Console.WriteLine($"  {nameStartLetterGroup.Key}: {nameStartLetterGroup.Count()}");
            }

            Console.WriteLine("Pocet mien podla dlzky znakov:");
            foreach (var nameLengthGroup in nameLengthGroups)
            {
                Console.WriteLine($"  {nameLengthGroup.Key}: {nameLengthGroup.Count()}");
            }

            Console.WriteLine("Pre ukoncenie stlacte Enter.");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Is called when user selects option to search for names
        /// </summary>
        private void OnSelectionSearchNames()
        {
            Console.Clear();
            Console.WriteLine("VYHLADAVANIE MIEN");

            while (true)
            {
                Console.WriteLine("Pre ukoncenie stlacte Enter.");
                Console.Write("Zadajte meno (regularny vyraz): ");
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                Nameday[] fetched = _namedayCalendar.GetNamedays(input);

                for (int i = 0; i < fetched.Length; i++)
                {
                    Console.WriteLine($"  {i + 1}. {fetched[i].Name} ({fetched[i].DayMonth.Day}.{fetched[i].DayMonth.Month})");
                }
            }
        }

        /// <summary>
        /// Is called when user selects option to search by date
        /// </summary>
        private void OnSelectionSearchNamesByDate()
        {
            Console.Clear();
            Console.WriteLine("VYHLADAVANIE MIEN PODLA DATUMU");

            while (true)
            {
                Console.WriteLine("Pre ukoncenie stlacte Enter.");
                Console.Write("Zadajte den a mesiac: ");
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                string[] splittedInput = input.Split(".");

                try
                {
                    if (splittedInput.Length != 2)
                    {
                        throw new Exception();
                    }

                    string[] fetchedNames = _namedayCalendar[Int32.Parse(splittedInput[0]), Int32.Parse(splittedInput[1])];
                    if (fetchedNames.Length == 0)
                    {
                        Console.WriteLine("  Neboli najdene ziadne mena");
                        continue;
                    }
                    for (int i = 0; i < fetchedNames.Length; i++)
                    {
                        Console.WriteLine($"  {i + 1}. {fetchedNames[i]}");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Nespravny format!");
                }
            }
        }

        /// <summary>
        /// Is called when user selects option to display a calendar for current month
        /// </summary>
        private void OnSelectionDisplayCalendar()
        {
            new CalendarView(_namedayCalendar);
        }

        /// <summary>
        /// Is called on exit
        /// </summary>
        private void OnSelectionExit()
        {
            Console.Clear();
            Environment.Exit(0);
        }
    }
}
