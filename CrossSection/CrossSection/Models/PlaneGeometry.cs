using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CrossSection.Models
{
    public class PlaneGeometry : Geometry
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public PlaneGeometry()
        {

        }

        #region Методы.

        /// <summary>
        /// Получит вершины геометрии плоскости.
        /// </summary>
        /// <param name="args">double? sideSize, double? coordinateY</param>
        /// <returns></returns>
        protected override Point3DCollection GetPointsGeometry(object[] args)
        {
            var result = new Point3DCollection();

            if (args.Length == 2)
            {

                double? sideSize = args[0] as double?;
                double? coordinateY = args[1] as double?;

                if (sideSize != null && coordinateY != null)
                {
                    var coordinate = sideSize / 2;

                    for (var i = 1; i > -2; i -= 2)
                    {
                        for (var j = 36; j < 180; j += 36)
                        {
                            var xMul = Math.Sin(j) * 10 / Math.Abs(Math.Sin(j) * 10) * -1;
                            var zMul = Math.Cos(j) * 10 / Math.Abs(Math.Cos(j) * 10);

                            result.Add(new Point3D()
                            {
                                X = (double)coordinate * xMul,
                                Y = (double)coordinateY + (0.025 * i),
                                Z = (double)coordinate * zMul
                            });
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Триангуляция вершин плоскости.
        /// </summary>
        /// <returns></returns>
        protected override Int32Collection Triangle()
        {
            var result = new Int32Collection();

            result.Add(0);
            result.Add(1);
            result.Add(2);

            result.Add(2);
            result.Add(3);
            result.Add(0);

            result.Add(4);
            result.Add(7);
            result.Add(6);

            result.Add(6);
            result.Add(5);
            result.Add(4);

            return result;
        }

        /// <summary>
        /// Построить геометрию плоскости.
        /// </summary>
        /// <param name="args">double? sideSize, double? coordinateY</param>
        public override void BuildGeometry(object[] args)
        {
            if (args.Length == 2)
            {
                double? sideSize = args[0] as double?;
                double? coordinateY = args[1] as double?;

                if (sideSize != null && coordinateY != null)
                {
                    Positions = GetPointsGeometry(new object[] { sideSize, coordinateY });
                    TriangleIndices = Triangle();
                }
            }
        }

        #endregion Методы.
    }
}
