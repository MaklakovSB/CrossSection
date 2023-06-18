using CrossSection.Interfaces;
using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CrossSection.Models
{
    public class CubeGeometry : IGeometry, INotifyPropertyChanged
    {
        #region Свойства.

        /// <summary>
        /// Коллекция вершин геометрии сферы.
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
        /// Коллекция индексов вершин геометрии.
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
        /// Величина стороны куба.
        /// </summary>
        public double CubeSide
        {
            get => _cubeSide;
            set
            {
                _cubeSide = value;
                OnPropertyChanged(nameof(CubeSide));
            }
        }
        private double _cubeSide = 0;

        /// <summary>
        /// Процент от размера куба для расчёта фаски.
        /// </summary>
        public double CubeChamferPrecent
        {
            get => _cubeChamferPrecent;
            set
            {
                _cubeChamferPrecent = value;
                OnPropertyChanged(nameof(CubeChamferPrecent));
            }
        }
        private double _cubeChamferPrecent = 0;

        /// <summary>
        /// Величина фаски.
        /// </summary>
        public double СubeChamfer
        {
            get => _cubeChamfer;
            set
            {
                _cubeChamfer = value;
                OnPropertyChanged(nameof(СubeChamfer));
            }
        }
        private double _cubeChamfer = 0;

        #endregion Свойства.

        /// <summary>
        /// Конструктор.
        /// </summary>
        public CubeGeometry()
        {

        }

        #region Методы.

        /// <summary>
        /// Расчёт точек куба.
        /// </summary>
        /// <param name="sideSize">Размер стороны куба.</param>
        /// <param name="chamferPrecent">Процент от величины стороны куба для расчёта фаски.</param>
        /// <returns></returns>
        private Point3DCollection GetPointsOnCube(double sideSize, double chamferPrecent = 0)
        {
            var result = new Point3DCollection();

            CubeSide = sideSize;
            CubeChamferPrecent = chamferPrecent;
            СubeChamfer = CubeSide / 100 * CubeChamferPrecent;

            var coordinate = sideSize / 2;

            for (var yMul = 1; yMul > -2; yMul -= 2)
            {
                for (var j = 36; j < 180; j += 36)
                {
                    var xMul = Math.Sin(j) * 10 / Math.Abs(Math.Sin(j) * 10) * -1;
                    var zMul = Math.Cos(j) * 10 / Math.Abs(Math.Cos(j) * 10);

                    if (CubeChamferPrecent == 0)
                    {
                        result.Add(new Point3D()
                        {
                            X = coordinate * xMul,
                            Y = coordinate * yMul,
                            Z = coordinate * zMul
                        });
                    }
                    else
                    {
                        result.Add(new Point3D()
                        {
                            X = coordinate * xMul,
                            Y = (coordinate - (СubeChamfer / 2)) * yMul,
                            Z = (coordinate - (СubeChamfer / 2)) * zMul
                        });

                        result.Add(new Point3D()
                        {
                            X = (coordinate - (СubeChamfer / 2)) * xMul,
                            Y = coordinate * yMul,
                            Z = (coordinate - (СubeChamfer / 2)) * zMul
                        });

                        result.Add(new Point3D()
                        {
                            X = (coordinate - (СubeChamfer / 2)) * xMul,
                            Y = (coordinate - (СubeChamfer / 2)) * yMul,
                            Z = coordinate * zMul
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Метод триангуляции куба с фаской и без.
        /// </summary>
        /// <returns></returns>
        private Int32Collection CubeTriangle()
        {
            int I = 0;
            int II = 0;
            int III = 0;
            int IV = 0;

            var result = new Int32Collection();
            if (CubeChamferPrecent != 0)
            {
                // Триангулируем восемь треугольников по углам куба.
                int k = 0;
                int n = 1;
                for (var i = 0; i < 2; i++)
                {
                    for (var j = 0; j < 4; j++)
                    {
                        result.Add(k);

                        if (n < 0)
                        {
                            result.Add(k + 1);
                            result.Add(k + 2);
                        }
                        else
                        {
                            result.Add(k + 2);
                            result.Add(k + 1);
                        }
                        k += 3;
                        n *= -1;
                    }

                    n *= -1;
                }

                // Триангулируем 8 фасок - 4 сверху и 4 снизу.
                n = 1;
                for (var i = 0; i < 24; i += 12)
                {
                    for (var j = 0; j < 12; j += 3)
                    {

                        if (n > 0)
                        {
                            I = i + j + 1;          //y
                            II = i + j + 2;         //z
                            III = i + j + 3 + 2;    //z
                            IV = i + j + 3 + 1;     //y
                        }
                        else
                        {
                            I = i + j + 1;          //y
                            II = i + j;             //x

                            if (j + 3 == 12)
                            {
                                III = i;            //x
                                IV = i + 1;         //y
                            }
                            else
                            {
                                III = i + j + 3;    //x
                                IV = i + j + 3 + 1; //y
                            }
                        }

                        if (i < 12)
                        {
                            result.Add(I);
                            result.Add(II);
                            result.Add(III);

                            result.Add(III);
                            result.Add(IV);
                            result.Add(I);
                        }
                        else
                        {
                            result.Add(I);
                            result.Add(IV);
                            result.Add(III);

                            result.Add(III);
                            result.Add(II);
                            result.Add(I);
                        }

                        n *= -1;
                    }
                }

                // Триангулируем 4 оставшиеся фаски.
                n = 1;
                for (var i = 0; i < 4; i++)
                {
                    if (n > 0)
                    {
                        I = i * 3 + 2;         // z
                        II = i * 3;            // x
                        III = (i + 4) * 3;     // x
                        IV = (i + 4) * 3 + 2;  // Z
                    }
                    else
                    {
                        I = i * 3;             // x
                        II = i * 3 + 2;        // z
                        III = (i + 4) * 3 + 2; // Z
                        IV = (i + 4) * 3;      // x
                    }

                    result.Add(I);
                    result.Add(II);
                    result.Add(III);

                    result.Add(III);
                    result.Add(IV);
                    result.Add(I);

                    n *= -1;
                }

                // Триангулируем 4 грани куба.
                n = 1;
                for (var i = 0; i < 4; i++)
                {
                    I = i;
                    II = i + 1;

                    if (i == 3)
                    {
                        II = 0;
                    }

                    III = I + 4;
                    IV = II + 4;

                    I *= 3;
                    II *= 3;
                    III *= 3;
                    IV *= 3;

                    if (n > 0)
                    {
                        I += 2;
                        II += 2;
                        III += 2;
                        IV += 2;
                    }

                    result.Add(II);
                    result.Add(I);
                    result.Add(III);

                    result.Add(III);
                    result.Add(IV);
                    result.Add(II);

                    n *= -1;
                }

                // Триангулируем верхнюю грань.
                I = 0 * 3 + 1;
                II = 1 * 3 + 1;
                III = 2 * 3 + 1;
                IV = 3 * 3 + 1;

                result.Add(I);
                result.Add(II);
                result.Add(III);

                result.Add(III);
                result.Add(IV);
                result.Add(I);

                // Триангулируем нижнюю грань.
                I = 7 * 3 + 1;
                II = 6 * 3 + 1;
                III = 5 * 3 + 1;
                IV = 4 * 3 + 1;

                result.Add(I);
                result.Add(II);
                result.Add(III);

                result.Add(III);
                result.Add(IV);
                result.Add(I);
            }
            else
            {
                // Триангулируем куб без фасок.
                // Триангулируем 4 грани куба.
                var n = 1;
                for (var i = 0; i < 4; i++)
                {
                    I = i;
                    II = i + 4;
                    III = II + 1;
                    IV = I + 1;

                    if (i == 3)
                    {
                        III = 4;
                        IV = 0;
                    }

                    result.Add(I);
                    result.Add(II);
                    result.Add(III);

                    result.Add(III);
                    result.Add(IV);
                    result.Add(I);

                    n *= -1;
                }

                // Триангулируем верхнюю грань.
                I = 0;
                II = 1;
                III = 2;
                IV = 3;

                result.Add(I);
                result.Add(II);
                result.Add(III);

                result.Add(III);
                result.Add(IV);
                result.Add(I);

                // Триангулируем нижнюю грань.
                I = 7;
                II = 6;
                III = 5;
                IV = 4;

                result.Add(I);
                result.Add(II);
                result.Add(III);

                result.Add(III);
                result.Add(IV);
                result.Add(I);
            }

            return result;
        }

        /// <summary>
        /// Получить куб.
        /// </summary>
        /// <param name="sideSize"></param>
        /// <param name="chamferPrecent"></param>
        public void GetCube(double sideSize, double chamferPrecent = 0)
        {
            Positions = GetPointsOnCube(sideSize, chamferPrecent);
            TriangleIndices = CubeTriangle();
        }

        #endregion Методы.

        #region Имплементация INotifyPropertyChanged.

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Имплементация INotifyPropertyChanged.
    }
}
