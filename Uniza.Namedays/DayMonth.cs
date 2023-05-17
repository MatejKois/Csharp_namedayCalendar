namespace Uniza.Namedays
{
    public record struct DayMonth
    {
        /// <summary>
        /// Day on which the person's nameday is
        /// </summary>
        public int Day { get; init; }

        /// <summary>
        /// Month on which the person's nameday is
        /// </summary>
        public int Month { get; init; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="day">Day on which the person's nameday is</param>
        /// <param name="month">Month on which the person's nameday is</param>
        public DayMonth(int day, int month)
        {
            Day = day;
            Month = month;
        }

        /// <summary>
        /// Converts the DayMonth to DateTime with current year
        /// </summary>
        /// <returns>DateTime with current year</returns>
        public DateTime ToDateTime()
        {
            return new DateTime(DateTime.Now.Year, Month, Day);
        }

        /// <summary>
        /// String typecast operator
        /// </summary>
        /// <param name="dayMonth">String format of DayMonth</param>
        public static explicit operator string(DayMonth dayMonth)
        {
            return $"{dayMonth.Day}.{dayMonth.Month}.";
        }

        /// <summary>
        /// Overridden ToString to handle binding in EditorGui FilterOutputListBox DataTemplate
        /// </summary>
        /// <returns>String format of DayMonth</returns>
        public override string ToString()
        {
            return $"{Day}.{Month}.";
        }
    }
}
