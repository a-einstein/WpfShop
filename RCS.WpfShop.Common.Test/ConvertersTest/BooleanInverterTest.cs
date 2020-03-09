using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RCS.WpfShop.Common.Converters.Test
{
    [TestClass()]
    public class BooleanInverterTest
    {
        [TestMethod()]
        public void ConvertTest()
        {
            var target = new BooleanInverter();

            var result = (bool)target.Convert(true, null, null, null);

            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void ConvertBackTest()
        {
            var target = new BooleanInverter();

            var result = (bool)target.ConvertBack(true, null, null, null);

            Assert.IsFalse(result);
        }
    }
}
