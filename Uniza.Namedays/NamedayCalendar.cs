using System.Collections;
using System.Text.RegularExpressions;

namespace Uniza.Namedays
{
    public class NamedayCalendar : IEnumerable<Nameday>
    {
        /// <summary>
        /// Contains all the namedays
        /// </summary>
        private List<Nameday> _namedays = new();

        /// <summary>
        /// Returns number of names in the calendar
        /// </summary>
        public int NameCount => _namedays.Count;

        /// <summary>
        /// Returns number of days in the calendar that somebody's nameday is on
        /// </summary>
        public int DayCount => _namedays.Select(nd => nd.DayMonth).Distinct().Count();

        /// <summary>
        /// Finds the day and month that the person has nameday on
        /// </summary>
        /// <param name="name">Name of the person to find nameday of</param>
        /// <returns>DayMonth of the person's nameday</returns>
        public DayMonth? this[string name] => _namedays.FirstOrDefault(nd => nd.Name == name).DayMonth;

        /// <summary>
        /// Finds all names that have nameday on the entered date
        /// </summary>
        /// <param name="dayMonth">DayMonth of the namedays to find</param>
        /// <returns>Names that have nameday on the entered day</returns>
        public string[] this[DayMonth dayMonth] =>
            _namedays
                .Where(nd => nd.DayMonth
                    .Equals(dayMonth))
                .Select(nd => nd.Name)
                .ToArray();

        /// <summary>
        /// Finds all names that have nameday on the entered date
        /// </summary>
        /// <param name="date">Date of the namedays to find</param>
        /// <returns>Names that have nameday on the entered day</returns>
        public string[] this[DateOnly date] =>
            _namedays
                .Where(nd => nd.DayMonth.Day == date.Day && nd.DayMonth.Month == date.Month)
                .Select(nd => nd.Name)
                .ToArray();

        /// <summary>
        /// Finds all names that have nameday on the entered date
        /// </summary>
        /// <param name="dateTime">DateTime of the namedays to find</param>
        /// <returns>Names that have nameday on the entered day</returns>
        public string[] this[DateTime dateTime] =>
            _namedays
                .Where(nd => nd.DayMonth.Day == dateTime.Day && nd.DayMonth.Month == dateTime.Month)
                .Select(nd => nd.Name)
                .ToArray();

        /// <summary>
        /// Finds all names that have nameday on the entered date
        /// </summary>
        /// <param name="day">Day of the namedays to find</param>
        /// <param name="month">Month of the namedays to find</param>
        /// <returns></returns>
        public string[] this[int day, int month] =>
            _namedays
                .Where(nd => nd.DayMonth.Day == day && nd.DayMonth.Month == month)
                .Select(nd => nd.Name)
                .ToArray();

        /// <summary>
        /// Returns namedays Enumerator
        /// </summary>
        /// <returns>Namedays Enumerator</returns>
        public IEnumerator<Nameday> GetEnumerator() => _namedays.GetEnumerator();

        /// <summary>
        /// Returns namedays Enumerator
        /// </summary>
        /// <returns>Namedays Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Returns array with all namedays
        /// </summary>
        /// <returns>Array with all namedays</returns>
        public Nameday[] GetNamedays() => _namedays.ToArray();

        /// <summary>
        /// Returns all namedays in month
        /// </summary>
        /// <param name="month">Month of the namedays</param>
        /// <returns>All namedays in month</returns>
        public Nameday[] GetNamedays(int month) => _namedays.Where(nd => nd.DayMonth.Month == month).ToArray();

        /// <summary>
        /// Returns all namedays with names matching the pattern
        /// </summary>
        /// <param name="pattern">Pattern to match names with</param>
        /// <returns>All namedays with names matching the pattern</returns>
        public Nameday[] GetNamedays(string pattern) => _namedays.Where(nd => Regex.IsMatch(nd.Name, pattern)).ToArray();

        /// <summary>
        /// Adds nameday to the calendar
        /// </summary>
        /// <param name="nameday">Nameday to add</param>
        public void Add(Nameday nameday) => _namedays.Add(nameday);

        /// <summary>
        /// Adds nameday to the calendar
        /// </summary>
        /// <param name="day">Day to add on</param>
        /// <param name="month">Month to add on</param>
        /// <param name="names">Names to add to the date</param>
        public void Add(int day, int month, params string[] names) => Add(new DayMonth(day, month), names);

        /// <summary>
        /// Adds nameday to the calendar
        /// </summary>
        /// <param name="dayMonth">Day and month to add on</param>
        /// <param name="names">Names to add to the date</param>
        public void Add(DayMonth dayMonth, params string[] names) => names.ToList().ForEach(name => Add(new Nameday(name, dayMonth)));

        /// <summary>
        /// Removes a name from calendar
        /// </summary>
        /// <param name="name">Name to remove</param>
        public void Remove(string name)
        {
            for (int i = 0; i < _namedays.Count; i++)
            {
                if (_namedays[i].Name == name)
                {
                    _namedays.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Finds if calendar contains the name
        /// </summary>
        /// <param name="name">Name to check</param>
        /// <returns>True if calendar contains the name</returns>
        public bool Contains(string name) => _namedays.Any(nd => nd.Name == name);

        /// <summary>
        /// Clears all the names from calendar
        /// </summary>
        public void Clear() => _namedays.Clear();

        /// <summary>
        /// Updates values of a nameday
        /// </summary>
        /// <param name="oldNameday">Nameday to update</param>
        /// <param name="newName">New name</param>
        /// <param name="newDate">New date</param>
        public void Update(Nameday oldNameday, string newName, DateTime newDate)
        {
            for (int i = 0; i < _namedays.Count; i++)
            {
                if (_namedays[i] == oldNameday)
                {
                    Nameday updatedNameday = new Nameday
                    {
                        Name = newName,
                        DayMonth = new DayMonth(newDate.Day, newDate.Month)
                    };

                    _namedays[i] = updatedNameday;
                }
            }
        }

        /// <summary>
        /// Sorts namedays by date
        /// </summary>
        public void Sort()
        {
            _namedays = _namedays.OrderBy(n => n).ToList();
        }

        /// <summary>
        /// Loads a calendar from a CSV file
        /// </summary>
        /// <param name="file">File to read from</param>
        /// <exception cref="Exception"></exception>
        public void Load(FileInfo file)
        {
            Clear();
            StreamReader reader;
            try
            {
                reader = new StreamReader(file.FullName);
            }
            catch (Exception)
            {
                throw new Exception("Error parsing file!");
            }

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var splittedLine = line.Split(';');
                if (splittedLine.Length >= 3)
                {
                    int day;
                    int month;
                    try
                    {
                        day = int.Parse(splittedLine[0].Split(" ")[0].TrimEnd('.'));
                        month = int.Parse(splittedLine[0].Split(" ")[1].TrimEnd('.'));
                    }
                    catch (Exception)
                    {
                        throw new Exception("Error parsing file!");
                    }

                    for (var i = 1; i < splittedLine.Length; i++)
                    {
                        if (!splittedLine[i].Contains("-") && splittedLine[i] != "")
                        {
                            Add(day, month, splittedLine[i]);
                        }
                    }
                }
            }
            Sort();
        }

        /// <summary>
        /// Saves the calendar to CSV a file
        /// </summary>
        /// <param name="file">File to save to</param>
        /// <exception cref="Exception"></exception>
        public void Save(FileInfo file)
        {
            StreamWriter writer;
            try
            {
                writer = new StreamWriter(file.FullName);
            }
            catch (Exception)
            {
                throw new Exception("Error opening file!");
            }

            for (var month = 1; month <= 12; month++)
            {
                for (var day = 1; day <= 31; day++)
                {
                    var names = this[day, month];
                    string line = "";
                    bool emptyLine = true; // if there are no names for the day, nothing will be written to file

                    line += $"{day}. {month};";

                    foreach (var name in names)
                    {
                        emptyLine = false;
                        line += $"{name};";
                    }

                    if (!emptyLine)
                    {
                        writer.WriteLine(line);
                    }
                }
            }
        }
    }
}
