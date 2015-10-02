using Demo.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Demo.Test.ConvertersTest
{
    [TestClass]
    public class WeightFormatterTest
    {
        [TestMethod]
        public void Convert()
        {
            var target = new WeightFormatter();
            var weight = (decimal)1;
            var unit = "Some Unit";

            var result = target.Convert(new object[] {weight, unit }, null, null, null);
            var expected = string.Format("{0} {1}", weight, unit);

            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void ConvertBack()
        {
            var target = new WeightFormatter();

            var result = target.ConvertBack("Some string", null, null, null);
        }
    }
}
