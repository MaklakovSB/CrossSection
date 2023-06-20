using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// 
        /// </summary>
        /// <param name="sideSize"></param>
        /// <param name="chamferPrecent"></param>
        /// <returns></returns>
        private Point3DCollection GetPoints(double sideSize, double coordinateY)
        {
            var result = new Point3DCollection();
            var coordinate = sideSize / 2;

            for (var i = 1; i > -2 ;i -= 2)
            {
                for (var j = 36; j < 180; j += 36)
                {
                    var xMul = Math.Sin(j) * 10 / Math.Abs(Math.Sin(j) * 10) * -1;
                    var zMul = Math.Cos(j) * 10 / Math.Abs(Math.Cos(j) * 10);

                    result.Add(new Point3D()
                    {
                        X = coordinate * xMul,
                        Y = coordinateY + (0.025 * i),
                        Z = coordinate * zMul
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Int32Collection Triangle()
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
        /// <param name="args"></param>
        public override void BuildGeometry(object[] args)
        {
            if (args.Length == 2)
            {
                double? sideSize = args[0] as double?;
                double? coordinateY = args[1] as double?;

                if (sideSize != null && coordinateY != null)
                {
                    Positions = GetPoints((double)sideSize, (double)coordinateY);
                    TriangleIndices = Triangle();
                }
            }
        }

        #endregion Методы.
    }
}
