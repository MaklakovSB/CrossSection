using CrossSection.Interfaces;
using System;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CrossSection.Models
{
    /// <summary>
    /// Класс - геометрия куба.
    /// Расчитывает точки вершин куба по двум аргументам: SideSize - размер стороны куба;
    /// ChamferPrecent - размер фаски как процент от размера куба.
    /// Если размер фаски будет равен нулю, то количество вершин куба будет равняться восьми.
    /// Если размер фаски будет отличен от нуля, то каждая из восьми вершин будет утроена и 
    /// общее количество вершин будет равняться двадцати четырём. 
    /// Вершины в коллекции Positions располагаются в порядке их расчёта - против часовой стрелки
    /// - если наблюдение происходит вдоль убывания оси Y (сверху-вниз убывающая часть оси Z 
    /// направлена вверх, растущая часть оси X направлена вправо на восток, что соответствует нулю градусов),
    /// то сначала будут расчитаны верхние-ближние точки куба, а затем нижние-дальние. Первая вершина (или 
    /// в случае с ненулевой фаской - набор из трёх вершин) будет находиться в верхней-правой четверти
    /// где координата X - положительна, а Z - отрицательна. Вторая вершина или набор из трёх вершин при 
    /// ненулевой фаске будет(ут) располагаться в левой-верхней четверти при отрицательных значениях
    /// координат по осям X и Z и т.д. где первые четыре шага расчитываются с положительными значения 
    /// координат по оси Y, а вторые четыре с отрицательными значениями координат по оси Y.
    /// В случае с ненулевой фаской вместо одной вершины куба за одну итерацию расчитывается три вершины.
    /// Например если сторона куба равна 10 и задана фаска 1 процент, то вместо первой вершины с координатами
    /// XYZ = {5, 5, -5} будет добавлено три вершины со следующими координатами: 
    /// {X = 5, Y = 5 со смещением на 0.5% в сторону нуля, Z = 5 со смещением на 0.5% в сторону нуля}, 
    /// {X = 5 со смещением на 0.5% в сторону нуля, Y = 5, 5 со смещением на 0.5% в сторону нуля}, 
    /// {X = 5 со смещением на 0.5% в сторону нуля, Y = 5 со смещением на 0.5% в сторону нуля, -5}.
    /// Из этого следует, что порядок расположения восьми наборов из трёх вершин в коллекции Positions
    /// взаимосвязано с координатами вершин куба таким образом, что в первой вершине набора коодинатой
    /// без отклонения к нулю будет X, во торой - Y, в третей - Z. Таким образом становится известно
    /// как найти каждую вершину и какой основной грани она принадлежит не анализируя значение координат вершин.
    /// </summary>
    public class CubeGeometry : Geometry, ICrossSection
    {
        #region Свойства.

        /// <summary>
        /// Величина стороны куба.
        /// </summary>
        public double SideSize
        {
            get => _sideSize;
            set
            {
                _sideSize = value;
                OnPropertyChanged(nameof(SideSize));
            }
        }
        private double _sideSize = 0;

        /// <summary>
        /// Процент от размера куба для расчёта фаски.
        /// </summary>
        public double ChamferPrecent
        {
            get => _chamferPrecent;
            set
            {
                _chamferPrecent = value;
                OnPropertyChanged(nameof(ChamferPrecent));
            }
        }
        private double _chamferPrecent = 0;

        /// <summary>
        /// Величина фаски.
        /// </summary>
        public double ChamferSize
        {
            get => _chamferSize;
            set
            {
                _chamferSize = value;
                OnPropertyChanged(nameof(ChamferSize));
            }
        }
        private double _chamferSize = 0;

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
        /// <param name="args">sideSize - размер стороны куба, chamferPrecent - процент от величины стороны куба для расчёта фаски.</param>
        /// <returns></returns>
        protected override Point3DCollection GetPointsGeometry(object[] args)
        {
            var result = new Point3DCollection();

            if (args.Length == 2)
            {
                double? sideSize = args[0] as double?;
                double? chamferPrecent = args[1] as double?;

                if (sideSize != null && chamferPrecent != null)
                {
                    SideSize = (double)sideSize;
                    ChamferPrecent = (double)chamferPrecent;
                    ChamferSize = SideSize / 100 * ChamferPrecent;

                    var coordinate = sideSize / 2;

                    for (var yMul = 1; yMul > -2; yMul -= 2)
                    {
                        for (var j = 45; j < 360; j += 90)
                        {
                            var xMul = Math.Cos(j / (180 / Math.PI)) / Math.Abs(Math.Cos(j / (180 / Math.PI)));
                            var zMul = Math.Sin(j / (180 / Math.PI)) / Math.Abs(Math.Sin(j / (180 / Math.PI))) * - 1;

                            if (ChamferPrecent == 0)
                            {
                                result.Add(new Point3D()
                                {
                                    X = (double)coordinate * xMul,
                                    Y = (double)coordinate * yMul,
                                    Z = (double)coordinate * zMul
                                });
                            }
                            else
                            {
                                result.Add(new Point3D()
                                {
                                    X = (double)coordinate * xMul,
                                    Y = (double)(coordinate - (ChamferSize / 2)) * yMul,
                                    Z = (double)(coordinate - (ChamferSize / 2)) * zMul
                                });

                                result.Add(new Point3D()
                                {
                                    X = (double)(coordinate - (ChamferSize / 2)) * xMul,
                                    Y = (double)coordinate * yMul,
                                    Z = (double)(coordinate - (ChamferSize / 2)) * zMul
                                });

                                result.Add(new Point3D()
                                {
                                    X = (double)(coordinate - (ChamferSize / 2)) * xMul,
                                    Y = (double)(coordinate - (ChamferSize / 2)) * yMul,
                                    Z = (double)coordinate * zMul
                                });
                            }
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Метод триангуляции куба с фаской и без.
        /// </summary>
        /// <returns></returns>
        protected override Int32Collection Triangle()
        {
            int I = 0;
            int II = 0;
            int III = 0;
            int IV = 0;

            var result = new Int32Collection();
            if (ChamferPrecent != 0)
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
        /// Построить геометрию куба.
        /// </summary>
        /// <param name="args"></param>
        public override void BuildGeometry(object[] args)
        {
            if (args.Length == 2)
            {
                double? sideSize = args[0] as double?;
                double? chamferPrecent = args[1] as double?;

                if (sideSize != null && chamferPrecent != null)
                {
                    Positions = GetPointsGeometry(new object[] { sideSize, chamferPrecent });
                    TriangleIndices = Triangle();
                }
            }
        }

        /// <summary>
        /// Реализация поперечного сечения куба двумя поперечными плоскостями.
        /// </summary>
        /// <param name="UpY">Плоскость в положительной части оси Y.</param>
        /// <param name="DownY">Плоскость в отрицательной части оси Y.</param>
        /// <returns></returns>
        public IGeometry CrossSection(double UpY, double DownY)
        {
            IGeometry result = new CubeGeometry();

            if (UpY == 0 && DownY == 0)
            {
                result.Positions = null;
                result.TriangleIndices = null;
                return result;
            }

            result.BuildGeometry(new object[] { (double?)SideSize, (double?)ChamferPrecent });

            // Новый набор вершин.
            var crossCube = new Point3DCollection();

            // Построим усечённый куб.
            var coordinate = SideSize / 2;

            for (var yMul = 1; yMul > -2; yMul -= 2)
            {
                double coordinateY;
                if (yMul > 0)
                {
                    if (UpY < coordinate)
                    {
                        coordinateY = UpY;
                    }
                    else
                    {
                        coordinateY = coordinate;
                    }
                }
                else if (yMul < 0)
                {
                    if (Math.Abs(DownY) < coordinate)
                    {
                        coordinateY = Math.Abs(DownY);
                    }
                    else
                    {
                        coordinateY = coordinate;
                    }
                }
                else
                {
                    throw new Exception("Переменная yMul не должна быть равна нулю.");
                }

                for (var j = 45; j < 360; j += 90)
                {
                    var xMul = Math.Cos(j / (180 / Math.PI)) / Math.Abs(Math.Cos(j / (180 / Math.PI)));
                    var zMul = Math.Sin(j / (180 / Math.PI)) / Math.Abs(Math.Sin(j / (180 / Math.PI))) * -1;

                    if (ChamferPrecent == 0)
                    {
                        crossCube.Add(new Point3D()
                        {
                            X = (double)coordinate * xMul,
                            Y = (double)coordinateY * yMul,
                            Z = (double)coordinate * zMul
                        });
                    }
                    else
                    {
                        crossCube.Add(new Point3D()
                        {
                            X = (double)coordinate * xMul,
                            Y = (double)(coordinateY - (ChamferSize / 2)) * yMul,
                            Z = (double)(coordinate - (ChamferSize / 2)) * zMul
                        });

                        crossCube.Add(new Point3D()
                        {
                            X = (double)(coordinate - (ChamferSize / 2)) * xMul,
                            Y = (double)coordinateY * yMul,
                            Z = (double)(coordinate - (ChamferSize / 2)) * zMul
                        });

                        crossCube.Add(new Point3D()
                        {
                            X = (double)(coordinate - (ChamferSize / 2)) * xMul,
                            Y = (double)(coordinateY - (ChamferSize / 2)) * yMul,
                            Z = (double)coordinate * zMul
                        });
                    }
                }
            }

            Positions = crossCube;
            TriangleIndices = Triangle();
            result.Positions = Positions;
            result.TriangleIndices = TriangleIndices;

            return result;
        }

        #endregion Методы.
    }
}
