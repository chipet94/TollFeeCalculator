using System;
using System.IO;
using System.Linq;
using static System.TimeSpan;

namespace TollFeeCalculator
{
    public class TollFeeCalculator
    {
        public static TimeFrame[] TollPrices =
        {
            new() {Price = 8, Start = Parse("06:00"), End = Parse("06:30")},
            new() {Price = 13, Start = Parse("06:30"), End = Parse("07:00")},
            new() {Price = 18, Start = Parse("07:00"), End = Parse("08:00")},
            new() {Price = 13, Start = Parse("08:00"), End = Parse("08:30")},
            new() {Price = 8, Start = Parse("08:30"), End = Parse("15:00")},
            new() {Price = 13, Start = Parse("15:00"), End = Parse("15:30")},
            new() {Price = 18, Start = Parse("15:30"), End = Parse("17:00")},
            new() {Price = 8, Start = Parse("17:00"), End = Parse("18:00")},
            new() {Price = 0, Start = Parse("18:30"), End = Parse("06:00")}
        };

        public static void Run(string filePath)
        {
            var dates = LoadFile(filePath);
            var sum = TotalFeeCost(dates);
            Console.Write("The total fee for the inputfile is " + sum);
        }

        public static int TotalFeeCost(DateTime[] tollPasses)
        {
            var sortedPasses = tollPasses.GroupBy(d => d.Date).OrderBy(m => m.Key.TimeOfDay).ToArray();
            var totalFee = 0;
            
            foreach (var day in sortedPasses)
            {
                var dailyFee = 0;
                var lastBilled = day.First(); //Starting interval
                foreach (var pass in day)
                {
                    long diffInMinutes = (pass - lastBilled).Minutes;
                    if (diffInMinutes > 60)
                    {
                        dailyFee += TollFeePass(pass);
                        lastBilled = pass;
                    }
                    else
                    {
                        dailyFee += Math.Max(TollFeePass(pass), TollFeePass(lastBilled));
                    }
                }

                totalFee += dailyFee >= 60 ? 60 : dailyFee;
            }

            return totalFee;
        }

        public static int TollFeePass(DateTime d)
        {
            return IsFree(d)
                ? 0
                : (from toll in TollPrices where toll.ContainsTime(d) select toll.Price).FirstOrDefault();
        }

        public static bool IsFree(DateTime date)
        {
            return date.Month == 7 || date.DayOfWeek == DayOfWeek.Saturday
                                   || date.DayOfWeek == DayOfWeek.Sunday;
        }

        public static DateTime[] LoadFile(string filePath)
        {
            if (!File.Exists(filePath)) throw new Exception($"File '{filePath}' does not exist.");

            return File.ReadAllText(filePath)
                .Split(',')
                .Select(d => DateTime.Parse(d))
                .ToArray();
        }
    }
}