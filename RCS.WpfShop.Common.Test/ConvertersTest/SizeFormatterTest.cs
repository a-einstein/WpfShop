using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RCS.WpfShop.Common.Converters.Test
{
    [TestClass()]
    public class SizeFormatterTest
    {
        [TestMethod()]
        public void ConvertTest()
        {
            var target = new SizeFormatter();
            const string parameter1 = "Parameter1";
            const string parameter2 = "Parameter2";

            var result = target.Convert(new object[] { parameter1, parameter2 }, null, null, null);
            var expected = $"{parameter1} {parameter2}";

            Assert.AreEqual(result, expected);
        }

        [TestMethod()]
        public void ConvertBackTest()
        {
            var target = new SizeFormatter();


            //Note Despite this assertion, exception handling had to be disabled for testing.
            Assert.ThrowsException<NotImplementedException>(() => target.ConvertBack("Some string", null, null, null));
        }
    }
}