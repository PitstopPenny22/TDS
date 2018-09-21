using System.Windows;
using System.Windows.Controls;

namespace WpfCustomControls
{
    /// <summary>
    /// A custom control that allows for a coloured background with a thin (BorderThickness = 1) black border.
    /// </summary>
    public class BorderedBackgroundTextBlock : Control
    {
        static BorderedBackgroundTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BorderedBackgroundTextBlock), new FrameworkPropertyMetadata(typeof(BorderedBackgroundTextBlock)));
        }

        /// <summary>
        /// This needs to be set and will bind to the inner TextBlock's <see cref="TextBlock.Text"/> property.
        /// </summary>
        public string TextValue
        {
            get { return (string)GetValue(TextValueProperty); }
            set { SetValue(TextValueProperty, value); }
        }
        public static readonly DependencyProperty TextValueProperty = DependencyProperty.Register("TextValue", typeof(string), typeof(BorderedBackgroundTextBlock), new PropertyMetadata(string.Empty));
    }
}