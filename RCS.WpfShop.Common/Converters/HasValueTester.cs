﻿using RCS.AdventureWorks.Common.Interfaces;
using System;
using System.Collections;
using System.Globalization;

namespace RCS.WpfShop.Common.Converters
{
    public class HasValueTester : SingleDirectionConverter
    {
        public override object Convert(object testObject, Type targetType, object parameter, CultureInfo culture)
        {
            var hasValue = HasValue(testObject);

            // Invert if any parameter is passed.
            var result = parameter != null
                ? !hasValue
                : hasValue;

            return result;
        }

        public static bool HasValue(object testObject)
        {
            // Note the tests sometimes are a matter of arbitrary definition.
            return !(
                testObject == null ||
                testObject is string && testObject as string == string.Empty ||
                testObject is bool && (bool)testObject == false ||
                testObject is int && (int)testObject == 0 ||
                testObject is IEmptyAble && (testObject as IEmptyAble).IsEmpty ||
                testObject is IList && (testObject as IList).Count == 0 ||
                testObject is ICollection && (testObject as ICollection).Count == 0
                );
        }
    }
}
