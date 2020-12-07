using System;
using System.IO;
using System.Linq;

namespace TollFeeCalculator
{
    internal class Program
    {
        private static void Main()
        {
            TollFeeCalculator.Run(Environment.CurrentDirectory + "../../../../testData.txt");
        }
    }
}