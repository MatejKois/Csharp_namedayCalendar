using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

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

        private NamedayCalendar _namedayCalendar;

        public ConsoleViewer()
        {
            _namedayCalendar = new NamedayCalendar();
        }

        public ConsoleViewer(FileInfo file)
        {
            _namedayCalendar = new NamedayCalendar();
            _namedayCalendar.Load(file);
        }

        public void MainLoop()
        {
            while (true)
            {
                DisplayCurrentState();
                ShowMenu();
                TakeInput();
            }
        }

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

        private void OnSelectionStatistics()
        {
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
            for (int i = 65; i <= 90; i++) // ASCII values A-Z
            {
                char letter = Convert.ToChar(i);
                Console.WriteLine($"{letter}: {_namedayCalendar.GetNamedays($"^{letter}\\w*").Length}");
            }

            Console.WriteLine("Pocet mien podla dlzky znakov:");
            var nameLengthGroups = _namedayCalendar.GetNamedays().GroupBy(nd => nd.Name.Length).OrderBy(g => g.Key);
            foreach (var nameLengthGroup in nameLengthGroups)
            {
                Console.WriteLine($"  {nameLengthGroup.Key}: {nameLengthGroup.Count()}");
            }

            Console.WriteLine("Pre ukoncenie stlacte Enter alebo inu klavesu");
            Console.ReadKey(true);
        }

        private void OnSelectionSearchNames()
        {
            throw new NotImplementedException();
        }

        private void OnSelectionSearchNamesByDate()
        {
            throw new NotImplementedException();
        }

        private void OnSelectionDisplayCalendar()
        {
            throw new NotImplementedException();
        }

        private void OnSelectionExit()
        {
            Console.Clear();
            Environment.Exit(0);
        }
    }
}
