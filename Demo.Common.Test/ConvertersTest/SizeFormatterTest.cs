using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Demo.Common.Converters.Test
{
    [TestClass()]
    public class SizeFormatterTest
    {
        [TestMethod()]
        public void ConvertTest()
        {
            var target = new SizeFormatter();
            var parameter1 = "Parameter1";
            var parameter2 = "Parameter2";

            var result = target.Convert(new object[] { parameter1, parameter2 }, null, null, null);
            var expected = string.Format("{0} {1}", parameter1, parameter2);

            Assert.AreEqual(result, expected);
        }

        [TestMethod()]
        [ExpectedException(typeof(NotImplementedException))]
        public void ConvertBackTest()
        {
            var target = new SizeFormatter();

            var result = target.ConvertBack("Some string", null, null, null);
        }
    }
}