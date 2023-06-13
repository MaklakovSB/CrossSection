using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CrossSection.Models
{
    public class Visual3DModel : INotifyPropertyChanged
    {
        #region Свойства

        /// <summary>
        /// Вершины образующие модель.
        /// </summary>
        public Point3DCollection Positions 
        {
            get => _positions;
            set
            {
                _positions = value;
                OnPropertyChanged(nameof(Positions));
            }
        }
        private Point3DCollection _positions = new Point3DCollection();

        /// <summary>
        /// Последовательность вершин.
        /// </summary>
        public Int32Collection TriangleIndices 
        {
            get => _triangleIndices;
            set
            {
                _triangleIndices = value;
                OnPropertyChanged(nameof(TriangleIndices));
            }
        }
        private Int32Collection _triangleIndices = new Int32Collection();

        /// <summary>
        /// Угловой шаг сферы.
        /// </summary>
        public double AngleStep 
        {
            get => _angleStep;
            set
            {
                _angleStep = value;
                OnPropertyChanged(nameof(AngleStep));
            }
        }
        private double _angleStep = 0;

        /// <summary>
        /// Радиус сферы.
        /// </summary>
        public double Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                OnPropertyChanged(nameof(Radius));
            }
        }
        private double _radius = 0;

        /// <summary>
        /// Количество расчётных точек основной окружности.
        /// </summary>
        public int MainCirclePointCount
        {
            get => _mainCirclePointCount;
            set
            {
                _mainCirclePointCount = value;
                OnPropertyChanged(nameof(MainCirclePointCount));
            }
        }
        private int _mainCirclePointCount = 0;

        /// <summary>
        /// Количество поперечных поясов сферы.
        /// </summary>
        public int CrossCircleCount
        {
            get => _crossCircleCount;
            set
            {
                _crossCircleCount = value;
                OnPropertyChanged(nameof(CrossCircleCount));
            }
        }
        private int _crossCircleCount = 0;

        public int PositionsCount
        {
            get => _positionsCount;
            set
            {
                _positionsCount = value;
                OnPropertyChanged(nameof(PositionsCount));
            }
        }
        private int _positionsCount = 0;
        #endregion

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Visual3DModel()
        {
        }

        #region Методы

        /// <summary>
        /// Расчёт набора точек принадлежащих окружности лежащей на плоскости XY.
        /// </summary>
        /// <param name="angleStep">Шаг в градусах угла для расчёта точек на окружности.</param>
        /// <param name="radius">Радиус окружности.</param>
        /// <param name="Z">Координата Z для всех возвращаемых точек.</param>
        /// <returns>Возвращает набор точек окружности лежащей на плоскости XY.</returns>
        private List<Point3D> GetPointsOnCircleXY(double angleStep, double radius, double Z = 0)
        {
            var result = new List<Point3D>();
            for (double i = 0; i < 360; i += angleStep)
            {
                var x = Math.Cos(i / (180 / Math.PI));
                var y = Math.Sin(i / (180 / Math.PI)) * -1;
                result.Add(new Point3D(x * radius, y * radius, Z));
            }

            return result;
        }

        /// <summary>
        /// Расчёт набора точек принадлежащих окружности лежащей на плоскости XZ.
        /// </summary>
        /// <param name="angleStep">Шаг в градусах угла для расчёта точек на окружности.</param>
        /// <param name="radius">Радиус окружности.</param>
        /// <param name="Y">Координата Y для всех возвращаемых точек.</param>
        /// <returns>Возвращает набор точек окружности лежащей на плоскости XZ.</returns>
        private List<Point3D> GetPointsOnCircleXZ(double angleStep, double radius, double Y = 0)
        {
            var result = new List<Point3D>();
            for (double i = 0; i < 360; i += angleStep)
            {
                var x = Math.Cos(i / (180 / Math.PI));
                var z = Math.Sin(i / (180 / Math.PI));
                result.Add(new Point3D(x * radius, Y, z * radius));
            }

            return result;
        }

        /// <summary>
        /// Расчёт точек принадлежащих сфере.
        /// </summary>
        /// <param name="angleStep"></param>
        /// <param name="radius"></param>
        /// <returns>Возвращает список точек принадлежащих сфере с заданным угловым шагом и радиусом.</returns>
        private Point3DCollection GetPointsOnSphere(double angleStep, double radius)
        {
            var result = new Point3DCollection();
            var mainCircle = GetPointsOnCircleXY(angleStep, radius);
            MainCirclePointCount = mainCircle.Count;
            AngleStep = angleStep;
            Radius = radius;

            // Получаем северный полюс сферы и заносим в результирующий список.
            var northPoleIndex = mainCircle.Count / 4;
            result.Add(new Point3D()
            {
                X = mainCircle[northPoleIndex].X,
                Y = mainCircle[northPoleIndex].Y,
                Z = mainCircle[northPoleIndex].Z
            });

            // Получаем стартовый индекс точки на основной окружности.
            var startPointIndex = northPoleIndex + 1;

            // Получаем количество итераций чтобы пройти по точкам основной
            // окружности между северной и южной точкой.
            var iteration = (mainCircle.Count / 2) - 1;
            CrossCircleCount = iteration;

            // Проходим по точкам от северной до южной и генерируем окружности поперечного сечения.
            for (var i = startPointIndex; i < startPointIndex + iteration; i++)
            {
                // Радиус поперечного сечения.
                var crossRadius = mainCircle[i].X * -1;

                // Список точек поперечной окружности.
                var crossCircle = GetPointsOnCircleXZ(angleStep, crossRadius, mainCircle[i].Y);

                // Заносим точки поперечной окружности в результирующий список.
                foreach(var point in crossCircle)
                {
                    result.Add(new Point3D()
                    {
                        X = point.X,
                        Y = point.Y,
                        Z = point.Z
                    });
                }
            }

            // Получаем южный полюс сферы и заносим в результирующий список.
            var southPoleIndex = northPoleIndex * 3;
            result.Add(new Point3D()
            {
                X = mainCircle[southPoleIndex].X,
                Y = mainCircle[southPoleIndex].Y,
                Z = mainCircle[southPoleIndex].Z
            });

            PositionsCount = result.Count;
            return result;
        }

        private Int32Collection Triangle()
        {
            var result = new Int32Collection();

            // Триангулируем северный полюс сферы.
            for (int i = 0; i < MainCirclePointCount; i++)
            {
                Int32 I = 0;
                Int32 II = i + 1;
                Int32 III = II + 1;

                if (II == MainCirclePointCount)
                {
                    III = 1;
                }

                result.Add(I);
                result.Add(II);
                result.Add(III);
            }

            // Проходим по первым точкам поперечных окружностей от первой точки самой северной окружности
            // по первую точку предпоследней поперечной окружности(точки последней окружности будут соеденины
            // с южным полюсом сферы).
            // Если окружностей поперечного сечения n, то делаем n - 1 проходов.
            for (int i = 1; i < (CrossCircleCount - 1) * MainCirclePointCount; i += MainCirclePointCount)
            {
                for (int j = i; j < i + MainCirclePointCount; j++)
                {
                    Int32 I = j;
                    Int32 II = I + MainCirclePointCount;
                    Int32 III = II + 1;

                    if (j + 1 == i + MainCirclePointCount)
                    {
                        III = i + MainCirclePointCount;
                    }

                    result.Add(I);
                    result.Add(II);
                    result.Add(III);
                }

                for (int j = i; j < i + MainCirclePointCount; j++)
                {
                    Int32 I = j;
                    Int32 II = I + MainCirclePointCount + 1;
                    Int32 III = I + 1;

                    if (j + 1 == i + MainCirclePointCount)
                    {
                        II = i + MainCirclePointCount;
                        III = i;
                    }

                    result.Add(I);
                    result.Add(II);
                    result.Add(III);
                }
            }

            var lastPoint = (MainCirclePointCount * CrossCircleCount) + 1;
            var firstPointOfLastCrossCircle = (MainCirclePointCount * (CrossCircleCount - 1)) + 1;

            // Триангулируем южный полюс сферы.
            for (int i = firstPointOfLastCrossCircle; i < lastPoint; i++)
            {
                Int32 I = lastPoint;
                Int32 II = i + 1;
                Int32 III = i;

                if (II == lastPoint)
                {
                    II = firstPointOfLastCrossCircle;
                }

                result.Add(I);
                result.Add(II);
                result.Add(III);
            }

            return result;
        }

        public void GetSphere(double angleStep, double radius)
        {
            Positions = GetPointsOnSphere(angleStep, radius);
            TriangleIndices = Triangle();
        }
        #endregion

        #region Имплементация INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
