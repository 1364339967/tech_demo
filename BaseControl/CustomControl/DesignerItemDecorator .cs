using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace BaseControl.CustomControl
{
    public class DesignerItemDecorator : Control
    {
        private Adorner adorner;

        public bool ShowDecorator
        {
            get { return (bool)GetValue(ShowDecoratorProperty); }
            set { SetValue(ShowDecoratorProperty, value); }
        }

        public static readonly DependencyProperty ShowDecoratorProperty =
            DependencyProperty.Register
                ("ShowDecorator", typeof(bool), typeof(DesignerItemDecorator),
            new FrameworkPropertyMetadata
                (false, new PropertyChangedCallback(ShowDecoratorProperty_Changed)));


        private void HideAdorner()
        {

        }

        private void ShowAdorner()
        {

        }

        private static void ShowDecoratorProperty_Changed
            (DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DesignerItemDecorator decorator = (DesignerItemDecorator)d;
            bool showDecorator = (bool)e.NewValue;

            if (showDecorator)
            {
                decorator.ShowAdorner();
            }
            else
            {
                decorator.HideAdorner();
            }
        }
    }
}
