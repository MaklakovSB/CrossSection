using CrossSection.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CrossSection.Models
{
    // Класс - геометрия сферы.
    // Расчитывает точки лежащие на сфере и триангулирует их против часовой стрелки - наружу.
    // Расчёт точек производится по входным параметрам - шаг угла и радиус. Для правильноого
    // расчёта важно чтобы значение угла шага делило число 90 нацело без остатка. Подходящие
    // значения шага угла: 1, 2, 3, 5, 6, 9, 10, 15, 18, (30, 45). При значениях шага 30 и 45
    // получаемая геометрия уже не является сферой т.е выходит за рамки задачи построения сферы.
    // Все расчёты ведутся исходя из следующих полжений: 0 градусов - справа и соответствует
    // восточному направлению и положительному направлению оси X; ось Y положительно направлена
    // вверх; ось Z отрицательно возрастает вдаль; расчёт начинается с нуля градусов с шагом
    // против часовой стрелки.
    // Сначала вычисляются точки окружности лежащей на плоскости XY и по количеству точек лежащих
    // между точками полюсов определяется, то сколько поперечных окружностей будет построено для 
    // получения сферы, затем расчитываются точки поперечных окружностей начиная с точек самой
    // "северной" окружности и завершая расчётом точек самой "южной" поперечной окружности.
    // Точки поперечных окружностей расчитываются в порядке против часовой стрелки от нуля градусов -
    // от положительного участка оси X в отрицательные значения оси Z, на которой при подходяшем шаге
    // угла будет лежать первая точка второй четверти равная 90 градусам, и т.д..
    // Коллекция точек сферы имеет предопределённую структуру, первая и последняя точка это "северный" и
    // "южный" полюса соответственно. Поэтому эти две точки всегда имеют определённый индекс в
    // коллекции, что важно для триангуляции, таким образом, точка северного полюса всегда имеет индекс 0,
    // а точка южного полюса имеет последний допустимый индекс коллекции. Между этими двумя элементами
    // коллекции располагаются точки поперечных окружностей начиная от самой "севарной" к самой "южной"
    // точки этих окружностей расположены в порядке их расчёта - от нуля нрадусов против часовой стрелки.
    public class SphereGeometry: Geometry, ICrossSection
    {
        public sealed class StepAngle
        {
            public static StepAngle Source
            {
                get
                {
                    return _source;
                }
            }
            private static readonly StepAngle _source = new StepAngle();

            public Dictionary<double, string> StepAngleDictonary { get; private set; }

            private StepAngle() 
            {
                StepAngleDictonary = new Dictionary<double, string>();
                StepAngleDictonary.Add(1.0, "1.00");
                StepAngleDictonary.Add(2.0, "2.00");
                StepAngleDictonary.Add(3.0, "3.00");
                StepAngleDictonary.Add(5.0, "5.00");
                StepAngleDictonary.Add(6.0, "6.00");
                StepAngleDictonary.Add(9.0, "9.00");
                StepAngleDictonary.Add(10.0, "10.00");
                StepAngleDictonary.Add(15.0, "15.00");
                StepAngleDictonary.Add(18.0, "18.00");
                StepAngleDictonary.Add(30.0, "30.00");
                StepAngleDictonary.Add(45.0, "45.00");
                StepAngleDictonary.Add(90.0, "90.00");
            }

            static StepAngle() { }

        }

        #region Свойства.

        /// <summary>
        /// Угловой шаг расчёта вершин сферы.
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
        /// Количество поперечных окружностей сферы.
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

        /// <summary>
        /// Количество точек.
        /// </summary>
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

        #endregion Свойства.

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SphereGeometry()
        {

        }

        #region Методы.

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

                var x = Math.Round( Math.Cos(i / (180 / Math.PI)), 4);
                var y = Math.Round(Math.Sin(i / (180 / Math.PI)), 4);
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
                var x = Math.Round(Math.Cos(i / (180 / Math.PI)), 4);
                var z = Math.Round(Math.Sin(i / (180 / Math.PI)), 4);
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
                X = mainCircle[northPoleIndex ].X,
                Y = mainCircle[northPoleIndex ].Y,
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
                foreach (var point in crossCircle)
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

        /// <summary>
        /// Триангуляция вершин сферы.
        /// </summary>
        /// <returns></returns>
        private Int32Collection SphereTriangle()
        {
            var result = new Int32Collection();
            Int32 I;
            Int32 II;
            Int32 III;

            // Триангулируем северный полюс сферы.
            for (int i = 0; i < MainCirclePointCount; i++)
            {
                I = 0;
                II = i + 1;
                III = II + 1;

                if (II == MainCirclePointCount)
                {
                    III = 1;
                }

                result.Add(I);
                result.Add(III);
                result.Add(II);
            }

            // Скорректируем количество попреречных колец в соответствии со списком вершин.
            CrossCircleCount = (Positions.Count - 2) / MainCirclePointCount;

            // Проходим по первым точкам поперечных окружностей от первой точки самой северной окружности
            // по первую точку предпоследней поперечной окружности(точки последней окружности будут соеденины
            // с южным полюсом сферы).
            // Если окружностей поперечного сечения n, то делаем n - 1 проходов.
            for (int i = 1; i < (CrossCircleCount - 1) * MainCirclePointCount; i += MainCirclePointCount)
            {
                for (int j = i; j < i + MainCirclePointCount; j++)
                {
                    I = j;
                    II = I + MainCirclePointCount;
                    III = II + 1;

                    if (j + 1 == i + MainCirclePointCount)
                    {
                        III = i + MainCirclePointCount;
                    }

                    result.Add(I);
                    result.Add(III);
                    result.Add(II);
                }

                for (int j = i; j < i + MainCirclePointCount; j++)
                {
                    I = j;
                    II = I + MainCirclePointCount + 1;
                    III = I + 1;

                    if (j + 1 == i + MainCirclePointCount)
                    {
                        II = i + MainCirclePointCount;
                        III = i;
                    }

                    result.Add(I);
                    result.Add(III);
                    result.Add(II);
                }
            }

            var lastPoint = (MainCirclePointCount * CrossCircleCount) + 1;
            var firstPointOfLastCrossCircle = (MainCirclePointCount * (CrossCircleCount - 1)) + 1;

            // Триангулируем южный полюс сферы.
            for (int i = firstPointOfLastCrossCircle; i < lastPoint; i++)
            {
                I = lastPoint;
                II = i + 1;
                III = i;

                if (II == lastPoint)
                {
                    II = firstPointOfLastCrossCircle;
                }

                result.Add(I);
                result.Add(III);
                result.Add(II);
            }

            return result;
        }

        /// <summary>
        /// Построить геометрию сферы.
        /// </summary>
        /// <param name="args"></param>
        public override void BuildGeometry(object[] args)
        {
            if (args.Length == 2)
            {
                double? angleStep = args[0] as double?;
                double? radius = args[1] as double?;

                if (angleStep != null && radius != null)
                {
                    Positions = GetPointsOnSphere((double)angleStep, (double)radius);
                    TriangleIndices = SphereTriangle();
                }
            }
        }

        /// <summary>
        /// Реализация поперечного сечения сферы двумя поперечными плоскостями.
        /// </summary>
        /// <param name="UpY"></param>
        /// <param name="DownY"></param>
        /// <returns></returns>
        public IGeometry CrossSection(double UpY, double DownY)
        {
            IGeometry result = new SphereGeometry();

            if(UpY == 0 && DownY == 0)
            {
                result.Positions = null;
                result.TriangleIndices = null;
                return result;
            }

            result.BuildGeometry(new object[] { (double?)AngleStep, (double?)Radius });

            // 1. Определить не совпадает ли верхняя и нижняя плоскости с поперечным кольцом сферы по оси Y.
            // если совпадает, то это кольцо и есть поперечное сечение, если нет, то нужно добавить кольцо в
            // геометрию.

            // Новый набор вершин.
            var crossSphere = new Point3DCollection();

            var upY = UpY;
            if (upY > Radius)
                upY = Radius;

            // Добавляем первой вершину северного полюса
            // с модификацией координаты Y.
            crossSphere.Add(new Point3D()
            {
                X = result.Positions[0].X,
                Y = upY,
                Z = result.Positions[0].Z
            });

            // Условие сечения верхнего полушария.
            if (UpY < Radius)
            {
                if (UpY > 0)
                {
                    //Проверить не совпадает ли плоскость сечения с попречной окружностью
                    //в составе сферы исключая экватор.
                    var startPointUpCrossCircle = result.Positions.Where(c => c.Y == UpY);

                    // совпадает - с этого кольца начинаем строить усечённую
                    // сферу первой точкой северный полюс с корректировкой
                    // координаты Y.
                    var testCount = startPointUpCrossCircle.Count();

                    if (testCount > 0)
                    {
                        if (testCount == MainCirclePointCount)
                        {
                            var firstElement = result.Positions.First(c => c.Y == UpY);
                            var lastElement = result.Positions.Last(c => c.Y == 0);

                            for (var i = result.Positions.IndexOf(firstElement); i < result.Positions.IndexOf(lastElement) + 1; i++)
                            {
                                crossSphere.Add(new Point3D()
                                {
                                    X = result.Positions[i].X,
                                    Y = result.Positions[i].Y,
                                    Z = result.Positions[i].Z
                                });
                            }
                        }
                        // Построена усечённая сфера до экватора включительно.
                    }
                    else
                    {
                        // нет совпадений, значит строим поперечную окружность с нужным шагом и радиусом,
                        // эта окружность будет первой после северного полюса после неё выбрать окружность
                        // с 0 < Y < UpY
                        //var sinus = Radius / UpY;
                        var sinus = UpY / Radius;
                        var crossRadius = Math.Sqrt(1 - sinus * sinus) * Radius;
                        var crossCircle = GetPointsOnCircleXZ(AngleStep, crossRadius, UpY);

                        // Добавляем окружность поперечного сечения.
                        foreach(var point in crossCircle)
                        {
                            crossSphere.Add(new Point3D()
                            {
                                X = point.X,
                                Y = point.Y,
                                Z = point.Z
                            });
                        }

                        // Находим все точки от первой Y < UpY до последней Y == 0.
                        var firstElement = result.Positions.First(c => c.Y < UpY);
                        var lastElement = result.Positions.Last(c => c.Y == 0);

                        for (var i = result.Positions.IndexOf(firstElement); i < result.Positions.IndexOf(lastElement) + 1; i++)
                        {
                            crossSphere.Add(new Point3D()
                            {
                                X = result.Positions[i].X,
                                Y = result.Positions[i].Y,
                                Z = result.Positions[i].Z
                            });
                        }
                        // Построена усечённая сфера до экватора включительно.
                    }
                }
                else if(UpY == 0)
                {
                    // Находим все точки от первой Y == 0 до последней Y == 0.
                    var firstElement = result.Positions.First(c => c.Y == 0);
                    var lastElement = result.Positions.Last(c => c.Y == 0);

                    for (var i = result.Positions.IndexOf(firstElement); i < result.Positions.IndexOf(lastElement) + 1; i++)
                    {
                        crossSphere.Add(new Point3D()
                        {
                            X = result.Positions[i].X,
                            Y = result.Positions[i].Y,
                            Z = result.Positions[i].Z
                        });
                    }
                    // Построена усечённая сфера до экватора включительно.
                }
            }
            else
            {
                var lastElement = result.Positions.Last(c => c.Y == 0);

                for (var i = 1; i < result.Positions.IndexOf(lastElement) + 1; i++)
                {
                    crossSphere.Add(new Point3D()
                    {
                        X = result.Positions[i].X,
                        Y = result.Positions[i].Y,
                        Z = result.Positions[i].Z
                    });
                }
                // Построена усечённая сфера до экватора включительно.
            }

            // Условие сечения нижнего полушария.
            if (DownY > -Radius)
            {
                if (DownY < 0)
                {
                    //Проверить не совпадает ли плоскость сечения с попречной окружностью
                    //в составе сферы исключая экватор.
                    var startPointDownCrossCircle = result.Positions.Where(c => c.Y == DownY);
                    var testCount = startPointDownCrossCircle.Count();

                    if (testCount > 0)
                    {
                        // Совпадает - это кольцо завершающее, после него только южный полюс.
                        if (testCount == MainCirclePointCount)
                        {
                            var firstElement = result.Positions.First(c => c.Y < 0);
                            var lastElement = result.Positions.Last(c => c.Y == DownY);

                            for(var i = result.Positions.IndexOf(firstElement); i < result.Positions.IndexOf(lastElement) + 1; i++)
                            {
                                crossSphere.Add(new Point3D()
                                {
                                    X = result.Positions[i].X,
                                    Y = result.Positions[i].Y,
                                    Z = result.Positions[i].Z
                                });
                            }
                        }
                        // Построена усечённая сфера от экватора до кольца нижнего сечения исключая южный полюс.
                    }
                    else
                    {
                        // Не совпадает - нужно добавить кольцо поперечного сечения, но
                        // только после добавления колец от экватоа(исключая экватор) до
                        // кольца с Y > DownY.

                        // Добавление колец от экватора(исключая экватор) до
                        // кольца с Y > DownY
                        // Находим все точки от первой Y < 0 до последней Y < 0.
                        var firstElement = result.Positions.First(c => c.Y < 0);
                        var lastElement = result.Positions.Last(c => c.Y > DownY);

                        for (var i = result.Positions.IndexOf(firstElement); i < result.Positions.IndexOf(lastElement) + 1; i++)
                        {
                            crossSphere.Add(new Point3D()
                            {
                                X = result.Positions[i].X,
                                Y = result.Positions[i].Y,
                                Z = result.Positions[i].Z
                            });
                        }

                        // Добавить кольцо поперечного сечения
                        var sinus = DownY / Radius;
                        var crossRadius = Math.Sqrt(1 - sinus * sinus) * Radius;
                        var crossCircle = GetPointsOnCircleXZ(AngleStep, crossRadius, DownY);

                        // Добавляем окружность поперечного сечения.
                        foreach (var point in crossCircle)
                        {
                            crossSphere.Add(new Point3D()
                            {
                                X = point.X,
                                Y = point.Y,
                                Z = point.Z
                            });
                        }
                        // Построена усечённая сфера от экватора до кольца нижнего сечения исключая южный полюс.
                    }
                }
            }
            else
            {
                //var lastElement = result.Positions.Last(c => c.Y == 0);
                var firstElement = result.Positions.Last(c => c.Y == 0);

                for (var i = result.Positions.IndexOf(firstElement); i < result.Positions.Count; i++)
                {
                    crossSphere.Add(new Point3D()
                    {
                        X = result.Positions[i].X,
                        Y = result.Positions[i].Y,
                        Z = result.Positions[i].Z
                    });
                }
                // Построена усечённая сфера до экватора включительно.
            }

            var downY = DownY;
            if (downY < -Radius)
                downY = -Radius;

            // Добавляем последней вершину южного полюса
            // с модификацией координаты Y.
            crossSphere.Add(new Point3D()
            {
                X = result.Positions[result.Positions.Count - 1].X,
                Y = downY,
                Z = result.Positions[result.Positions.Count - 1].Z
            });

            Positions = crossSphere;
            TriangleIndices = SphereTriangle();
            result.Positions = Positions;
            result.TriangleIndices = TriangleIndices;

            return result;
        }

        #endregion Методы.
    }
}
