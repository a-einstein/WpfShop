using Prism.Commands;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace RCS.WpfShop.Common.Controls
{
    public partial class ClearableTextBox : UserControl
    {
        #region Construction
        public ClearableTextBox()
        {
            InitializeComponent();
        }
        #endregion

        #region Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(ClearableTextBox), new PropertyMetadata(TextChanged));

        // TODO This is only reached on when leaving the TextBox, instead of while changing the Text.
        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Reevaluate dependencies.
            (d as ClearableTextBox).ClearCommand.RaiseCanExecuteChanged();
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        #endregion

        #region Command
        // Note that Control.IsEnabled depends on Control.Command.CanExecute if defined.
        public static readonly DependencyProperty ClearCommandProperty =
            DependencyProperty.Register(nameof(ClearCommand), typeof(DelegateCommand<ClearableTextBox>), typeof(ClearableTextBox), new PropertyMetadata(new DelegateCommand<ClearableTextBox>(Clear, CanClear)));

        public DelegateCommand<ClearableTextBox> ClearCommand
        {
            get => (DelegateCommand<ClearableTextBox>)GetValue(ClearCommandProperty);
            set => SetValue(ClearCommandProperty, value);
        }

        private static void Clear(ClearableTextBox clearableTextBox)
        {
            clearableTextBox.Text = string.Empty;
        }

        private static bool CanClear(ClearableTextBox clearableTextBox)
        {
            return !string.IsNullOrEmpty(clearableTextBox?.Text);
        }
        #endregion

        #region Behaviors
        public IList<Behavior> TextBoxBehaviors
        {
            get => Interaction.GetBehaviors(textBox);
            set
            {
                var behaviors = TextBoxBehaviors;

                // TODO This may have to be removed.
                behaviors.Clear();

                foreach (var behavior in value)
                {
                    behaviors.Add(behavior);
                }
            }
        }
        #endregion
    }
}
