using System.Globalization;
using System.Windows.Data;

namespace AuroraChatBot.CS
{
    public class HeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double totalHeight && totalHeight > 0)
            {
                // 减去底部输入区域预估高度（根据实际布局调整偏移量）
                return totalHeight - 250; // 250为底部区域+margin的估算值
            }
            return 600;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
