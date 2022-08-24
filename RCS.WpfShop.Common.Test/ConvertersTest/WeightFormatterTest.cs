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
            const decimal weight = 1;
            const string unit = "Some Unit";

            var result = target.Convert(new object[] { weight, unit }, null, null, null);
            var expected = $"{weight} {unit}";

            Assert.AreEqual(result, expected);
        }

        [TestMethod()]
        public void ConvertBackTest()
        {
            var target = new WeightFormatter();

            //Note Despite this assertion, exception handling had to be disabled for testing.
            Assert.ThrowsException<NotImplementedException>(() => target.ConvertBack("Some string", null, null, null));
        }
    }
}
