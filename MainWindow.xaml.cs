using PotentialMethod;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace miapr5
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Graph _graph;
        private Function _desiciveFunction;
        private readonly List<Point>[] _points = new List<Point>[2];
        private const int Step = 21;

        public MainWindow()
        {
            InitializeComponent();
            ClearPoints();
        }

        private void ClearPoints()
        {
            _points[0] = new List<Point>();
            _points[1] = new List<Point>();
        }

        private void TextCheck(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("(^-?[0-9]+$)|(^-?$)");
            if (e.Text == " ") e.Handled = true;
            if (!regex.IsMatch($"{(sender as TextBox)?.Text}{e.Text}")) e.Handled = true;
        }

        private void ButtonTrain_OnClick(object sender, RoutedEventArgs e)
        {
            ClearPoints();
            var potential = new Potential();
            var teaching = new Point[2][];

            teaching[0] = new Point[2];
            teaching[1] = new Point[2];
            try
            {
                teaching[0][0] = new Point(int.Parse(TextBoxO1X.Text), int.Parse(TextBoxO1Y.Text));
                teaching[0][1] = new Point(int.Parse(TextBoxO2X.Text), int.Parse(TextBoxO2Y.Text));
                teaching[1][0] = new Point(int.Parse(TextBoxO3X.Text), int.Parse(TextBoxO3Y.Text));
                teaching[1][1] = new Point(int.Parse(TextBoxO4X.Text), int.Parse(TextBoxO4Y.Text));
            }
            catch (FormatException)
            {
                MessageBox.Show("Ошибка заполнения полей");
                return;
            }

            _points[0].Add(teaching[0][0]);
            _points[0].Add(teaching[0][1]);
            _points[1].Add(teaching[1][0]);
            _points[1].Add(teaching[1][1]);


            _desiciveFunction = potential.GetFunction(teaching);
            TextBoxFunction.Text = "";
            if (!potential.Warning)
            {
                TextBoxFunction.Text = _desiciveFunction.ToString();
                ButtonClassify.IsEnabled = true;
                _graph = new Graph(_desiciveFunction, Canvas.ActualWidth, Canvas.ActualHeight);
            }
            else
            {
                MessageBox.Show("Невозможно построить разделяющую функцию");
                _graph = new Graph(Canvas.ActualWidth, Canvas.ActualHeight);
                ButtonClassify.IsEnabled = false;
            }

            Canvas.Source = new DrawingImage(_graph.DrawingGroup);
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                {
                    _graph.AddPoint(teaching[i][j], i == 0);
                }
            }
        }

        private void ButtonClassify_OnClick(object sender, RoutedEventArgs e)
        {
            Point testPoint;
            try
            {
                testPoint = new Point(int.Parse(TextBoxOX.Text), int.Parse(TextBoxOY.Text));
            }
            catch (FormatException)
            {
                MessageBox.Show("Заполните X и Y точки для классификации");
                return;
            }

            var classNumber = _desiciveFunction.GetValue(testPoint) >= 0 ? 0 : 1;
            _points[classNumber].Add(testPoint);
            MessageBox.Show($"Класс {classNumber + 1}");
            _graph.AddPoint(testPoint, classNumber == 0);
        }

        private void Move_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (DrawToolTip(e, _points[0], 1)) return;
            if (DrawToolTip(e, _points[1], 2)) return;
        }

        private bool DrawToolTip(MouseEventArgs e, IEnumerable<Point> list, int classNumber)
        {
            foreach (var point in list)
            {
                var position = e.GetPosition(Canvas);
                if (!(Math.Abs(point.X * Step + Canvas.ActualWidth / 2 - position.X) < 10) ||
                    !(Math.Abs(Canvas.ActualHeight / 2 - point.Y * Step - position.Y) < 10)) continue;
                Canvas.ToolTip = $"Класс:{classNumber}{Environment.NewLine}({point.X};{point.Y})";
                return true;
            }

            return false;
        }
    }
}
