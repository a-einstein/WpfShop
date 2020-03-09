using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace RCS.WpfShop.Common.Behaviors
{
    public class TextBoxRegexValidationBehavior : Behavior<TextBox>
    {
        #region Behavior
        protected override void OnAttached()
        {
            base.OnAttached();

            var textBox = AssociatedObject;
            textBox.TextChanged += HandleTextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            var textBox = AssociatedObject;
            textBox.TextChanged -= HandleTextChanged;
        }

        void HandleTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            var isValid = (Regex.IsMatch(textBox.Text, ValidExpression, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));

            // Use Background as it stands out more.
            // This could be replaced by entire styles.
            // An empty string is not signalled.
            textBox.Background = string.IsNullOrEmpty(textBox.Text) || isValid
                ? ValidBackground
                : InvalidBackground;
        }
        #endregion

        #region Parameters
        public static readonly DependencyProperty ValidExpressionProperty =
            DependencyProperty.Register(nameof(ValidExpression), typeof(string), typeof(TextBoxRegexValidationBehavior), new PropertyMetadata(".*"));

        public string ValidExpression
        {
            get { return (string)GetValue(ValidExpressionProperty); }
            set { SetValue(ValidExpressionProperty, value); }
        }

        public static readonly DependencyProperty ValidBackgroundProperty =
            DependencyProperty.Register(nameof(ValidBackground), typeof(Brush), typeof(TextBoxRegexValidationBehavior), new PropertyMetadata(Brushes.Black));

        public Brush ValidBackground
        {
            get { return (Brush)GetValue(ValidBackgroundProperty); }
            set { SetValue(ValidBackgroundProperty, value); }
        }

        public static readonly DependencyProperty InvalidBackgroundProperty =
            DependencyProperty.Register(nameof(InvalidBackground), typeof(Brush), typeof(TextBoxRegexValidationBehavior), new PropertyMetadata(Brushes.Black));

        public Brush InvalidBackground
        {
            get { return (Brush)GetValue(InvalidBackgroundProperty); }
            set { SetValue(InvalidBackgroundProperty, value); }
        }
        #endregion
    }
}
