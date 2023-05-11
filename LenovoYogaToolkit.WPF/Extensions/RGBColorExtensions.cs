using System.Windows.Media;
using LenovoYogaToolkit.Lib;

namespace LenovoYogaToolkit.WPF.Extensions;

public static class RGBColorExtensions
{
    public static Color ToColor(this RGBColor color) => Color.FromRgb(color.R, color.G, color.B);
}