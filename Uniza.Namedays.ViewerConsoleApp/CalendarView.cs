using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniza.Namedays.ViewerConsoleApp
{
    internal class CalendarView
    {
        private readonly NamedayCalendar _namedayCalendar;

        public CalendarView(NamedayCalendar namedayCalendar)
        {
            _namedayCalendar = namedayCalendar;
        }
    }
}
