using System;
using System.Drawing;
using System.Windows;

namespace PotentialMethod
{
    public class Potential
    {
        private const int ClassCount = 2;

        private const int IterationsCount = 50;

        private int _correction;

        public bool Warning { get; private set; }

        public Function GetFunction(Point[][] teachingPoints)
        {
            var result = new Function(0, 0, 0, 0);
            _correction = 1;
            Warning = false;
            var nextIteration = true;
            var iterationNumber = 0;

            result += _correction * PartFunction(teachingPoints[teachingPoints.Length - 1][teachingPoints[teachingPoints.Length - 1].Length-1]);
            while (nextIteration && iterationNumber < IterationsCount)
            {
                iterationNumber++;
                nextIteration = DoOneIteration(teachingPoints, ref result);
            }

            if (iterationNumber == IterationsCount) Warning = true;
            return result;
        }

        private bool DoOneIteration(Point[][] teachingPoints, ref Function result)
        {
            var nextIteration = false;

            if (teachingPoints.Length != ClassCount) throw new Exception();

            for (var classNumber = 0; classNumber < ClassCount; classNumber++)
            {
                for (var i = 0; i < teachingPoints[classNumber].Length; i++)
                {
                    var curPoint = teachingPoints[classNumber][i];
                    _correction = GetNewCorrection(curPoint, result, classNumber);
                    if (_correction != 0) nextIteration = true;
                    result += _correction * PartFunction(teachingPoints[classNumber][i]);
                }
            }

            return nextIteration;
        }

        private static int GetNewCorrection(Point point, Function result, int classNumber)
        {
            var functionValue = result.GetValue(point);
            if (functionValue <= 0 && classNumber == 0)
            {
                return 1;
            }

            if (functionValue > 0 && classNumber == 1)
            {
                return -1;
            }

            return 0;
        }


        private static Function PartFunction(Point point)
        {
            return new Function(4 * point.X, 4 * point.Y, 16 * point.X * point.Y, 1);
        }
    }
}