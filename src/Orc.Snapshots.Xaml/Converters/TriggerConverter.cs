﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TriggerConverter.cs" company="WildGums">
//   Copyright (c) 2008 - 2014 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.Snapshots.Converters
{
    using System;
    using System.Windows.Data;
    using Catel.Collections;
    using Catel.MVVM.Converters;

    /// <summary>
    /// Workaround class for bug with non-evaluating commands with command parameters:
    /// http://stackoverflow.com/questions/335849/wpf-commandparameter-is-null-first-time-canexecute-is-called
    /// </summary>
    public class TriggerConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // First value is target value.
            // All others are update triggers only.
            if (values.Length < 1)
            {
                return ConverterHelper.UnsetValue;
            }

            return values[0];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return ArrayShim.Empty<object>();
        }
        #endregion
    }
}
