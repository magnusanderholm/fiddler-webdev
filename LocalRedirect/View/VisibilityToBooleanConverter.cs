namespace Fiddler.LocalRedirect.View
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    [Localizability(LocalizationCategory.NeverLocalize)]
    public class VisibilityToBooleanConverter : MarkupExtension,  IValueConverter
    {
        public VisibilityToBooleanConverter()
        {
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;                        
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isVisible = (value is bool && (bool)value);
            
            if (value is bool?)
            {
                bool? tmp = (bool?)value;                                                   
                isVisible = tmp.HasValue && tmp.Value;                
            }
            return isVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
