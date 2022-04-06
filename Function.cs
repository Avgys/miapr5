using System.Windows;

namespace PotentialMethod
{
    public class Function
    {
        private double XKoeff { get; set; }
        private double YKoeff { get; set; }
        private double XyKoeff { get; set; }
        private double FreeKoeff { get; set; }
        public Function(double xKoeff, double yKoeff, double xyKoeff, double freeKoeff)
        {
            XKoeff = xKoeff;
            YKoeff = yKoeff;
            XyKoeff = xyKoeff;
            FreeKoeff = freeKoeff;
        }

        public double GetValue(Point point)
        {
            return FreeKoeff + XKoeff * point.X + YKoeff * point.Y + XyKoeff * point.X * point.Y;
        }

        public double GetY(double x)
        {
            return -(XKoeff * x + FreeKoeff) / (XyKoeff * x + YKoeff);
        }

        public override string ToString()
        {
            if (XyKoeff != 0)
            {
                return
                    $"y=({-XKoeff}*x{(-FreeKoeff < 0 ? "" : "+")}{-FreeKoeff})/({XyKoeff}*x{(YKoeff < 0 ? "" : "+")}{YKoeff})";
            }
            if (YKoeff != 0)
            {
                return
                    $"y={-(double)XKoeff / YKoeff}*x{(-(double)FreeKoeff / YKoeff < 0 ? "" : "+")}{-(double)FreeKoeff / YKoeff}";
            }
            return $"x={-(double)FreeKoeff / XKoeff}";
        }

        public static Function operator +(Function first, Function second)
        {
            return new Function(first.XKoeff + second.XKoeff, first.YKoeff + second.YKoeff,
                first.XyKoeff + second.XyKoeff, first.FreeKoeff + second.FreeKoeff);
        }

        public static Function operator *(int koeff, Function function)
        {
            return new Function(koeff * function.XKoeff, koeff * function.YKoeff, koeff * function.XyKoeff,
                koeff * function.FreeKoeff);
        }
    }
}