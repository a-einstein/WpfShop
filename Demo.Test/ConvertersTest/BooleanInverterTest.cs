using Demo.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Demo.Test.ConvertersTest
{
    [TestClass]
    public class BooleanInverterTest
    {
        [TestMethod]
        public void Convert()
        {
            var target = new BooleanInverter();

            var result = (Boolean)target.Convert(true, null, null, null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ConvertBack()
        {
            var target = new BooleanInverter();

            var result = (Boolean)target.ConvertBack(true, null, null, null);

            Assert.IsFalse(result);
        }
    }
}
