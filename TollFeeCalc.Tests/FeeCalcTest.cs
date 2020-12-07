using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TollFeeCalc.Tests
{
    [TestClass]
    public class FeeCalcTest
    {
        private readonly string _filePath = Environment.CurrentDirectory + "../../../../testData.txt";

        [TestMethod]
        public void TestOutPutString()
        {
            var expected = "The total fee for the inputfile is ";
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                TollFeeCalculator.TollFeeCalculator.Run(_filePath);
                Assert.IsTrue(sw.ToString().Contains(expected));
            }
        }

        [TestMethod]
        public void TestInputFile()
        {
            bool fileExists = File.Exists(_filePath);
            Assert.IsTrue(fileExists);
        }

        [TestMethod]
        public void TestTollFeeDailyPrice()
        {
            var dates = new[]
            {
                //tisdag
                DateTime.Parse("1994-05-24 08:00"), DateTime.Parse("1994-05-24 09:00"),
                DateTime.Parse("1994-05-24 10:00"), DateTime.Parse("1994-05-24 11:00"),
                DateTime.Parse("1994-05-24 12:00"), DateTime.Parse("1994-05-24 13:00"),
                DateTime.Parse("1994-05-24 14:00"), DateTime.Parse("1994-05-24 15:00"),
                DateTime.Parse("1994-05-24 16:00"), DateTime.Parse("1994-05-24 17:00"),
                DateTime.Parse("1994-05-24 18:00"), DateTime.Parse("1994-05-24 19:00"),
                DateTime.Parse("1994-05-24 20:00"), DateTime.Parse("1994-05-24 21:00"),
                //onsdag
                DateTime.Parse("1994-05-25 08:00"), DateTime.Parse("1994-05-25 09:00"),
                DateTime.Parse("1994-05-25 10:00"), DateTime.Parse("1994-05-25 11:00"),
                DateTime.Parse("1994-05-25 12:00"), DateTime.Parse("1994-05-25 13:00"),
                DateTime.Parse("1994-05-25 14:00"), DateTime.Parse("1994-05-25 15:00"),
                DateTime.Parse("1994-05-25 16:00"), DateTime.Parse("1994-05-25 17:00"),
                DateTime.Parse("1994-05-25 18:00"), DateTime.Parse("1994-05-25 19:00"),
                DateTime.Parse("1994-05-25 20:00"), DateTime.Parse("1994-05-25 21:00"),
                //lördag
                DateTime.Parse("1994-05-28 08:00"), DateTime.Parse("1994-05-28 09:00"),
                DateTime.Parse("1994-05-28 10:00"), DateTime.Parse("1994-05-28 11:00"),
                DateTime.Parse("1994-05-28 12:00"), DateTime.Parse("1994-05-28 13:00"),
                DateTime.Parse("1994-05-28 14:00"), DateTime.Parse("1994-05-28 15:00"),
                DateTime.Parse("1994-05-28 16:00"), DateTime.Parse("1994-05-28 17:00"),
                DateTime.Parse("1994-05-28 18:00"), DateTime.Parse("1994-05-28 19:00"),
                DateTime.Parse("1994-05-28 20:00"), DateTime.Parse("1994-05-28 21:00"),
            };

            //En dag
            var returnedPrice = TollFeeCalculator.TollFeeCalculator.TotalFeeCost(dates.Take(14).ToArray());
            Assert.IsTrue(returnedPrice == 60);

            //3 dagar, fast lördag inc.
            returnedPrice = TollFeeCalculator.TollFeeCalculator.TotalFeeCost(dates);
            Assert.IsTrue(returnedPrice == 120);
        }

        [TestMethod]
        public void TestFreeDate()
        {
            //Free date
            var testDate = DateTime.Parse("2020-07-01 00:00");
            Assert.AreEqual(true, TollFeeCalculator.TollFeeCalculator.IsFree(testDate));
            // Payed. 
            testDate = DateTime.Parse("11/30/2020 1:49:34");
            Assert.AreEqual(false, TollFeeCalculator.TollFeeCalculator.IsFree(testDate));
        }
    }
}