﻿using RCS.WpfShop.Common.ViewModels;
using System.Windows.Controls;

namespace RCS.WpfShop.Common.Views
{
    public abstract class View : UserControl
    {
        // This cannot be uses instead of DataContext from XAML.
        public ViewModel ViewModel
        {
            get => DataContext as ViewModel;
            init => DataContext = value;
        }
    }
}
