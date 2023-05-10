namespace Uniza.Namedays
{
    public record struct Nameday
    {
        /// <summary>
        /// Name of the person
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Represents day and month of the person's nameday
        /// </summary>
        public DayMonth DayMonth { get; init; }

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
    }
}
