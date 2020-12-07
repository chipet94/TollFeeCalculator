using System;

namespace TollFeeCalculator
{
    public class TimeFrame
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }

        public int Price { get; set; }

        public bool ContainsTime(DateTime dateTime)
        {
            return dateTime.TimeOfDay >= Start && dateTime.TimeOfDay <= End;
        }
    }
}