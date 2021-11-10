using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RCS.WpfShop.Common.Converters.Test
{
    [TestClass()]
    public class WeightFormatterTest
    {
        [TestMethod()]
        public void ConvertTest()
        {
            var target = new WeightFormatter();
            const decimal weight = (decimal)1;
            const string unit = "Some Unit";

            var result = target.Convert(new object[] {weight, unit }, null, null, null);
            var expected = $"{weight} {unit}";

            Assert.AreEqual(result, expected);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void ConvertBackTest()
        {
            var target = new WeightFormatter();

            var result = target.ConvertBack("Some string", null, null, null);
        }
    }
}
