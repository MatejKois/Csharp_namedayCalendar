namespace Uniza.Namedays
{
    public record struct Nameday : IComparable<Nameday>
    {
        /// <summary>
        /// Name of the person
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents day and month of the person's nameday
        /// </summary>
        public DayMonth DayMonth { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of the person</param>
        /// <param name="dayMonth">Represents day and month of the person's nameday</param>
        public Nameday(string name, DayMonth dayMonth)
        {
            Name = name;
            DayMonth = dayMonth;
        }

        public int CompareTo(Nameday other)
        {
            if (DayMonth.Month == other.DayMonth.Month)
            {
                return DayMonth.Day == other.DayMonth.Day ? 0 : DayMonth.Day > other.DayMonth.Day ? 1 : -1;
            }
            return DayMonth.Month == other.DayMonth.Month ? 0 : DayMonth.Month > other.DayMonth.Month ? 1 : -1;
        }
    }
}
