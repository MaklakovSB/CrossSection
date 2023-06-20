using CrossSection.Delegates;
using CrossSection.Interfaces;
using CrossSection.Models;
using System.Collections.Generic;
using System.Windows.Input;

namespace CrossSection.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Поля.

        private CubeGeometry _cubeGeometry = new CubeGeometry();
        private SphereGeometry _sphereGeometry = new SphereGeometry();

        #endregion Поля.

        #region Свойства.

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
        private double _cubeSide = 10;

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
        private double _cubeChamferPrecent = 1;

        /// <summary>
        /// Угловой шаг сферы.
        /// </summary>
        public double SphereAngleStep
        {
            get => _sphereAngleStep;
            set
            {
                _sphereAngleStep = value;
                OnPropertyChanged(nameof(SphereAngleStep));
            }
        }
        private double _sphereAngleStep = 3;

        /// <summary>
        /// Радиус сферы.
        /// </summary>
        public double SphereRadius
        {
            get => _sphereRadius;
            set
            {
                _sphereRadius = value;
                OnPropertyChanged(nameof(SphereRadius));
            }
        }
        private double _sphereRadius = 5;

        /// <summary>
        /// Текушая геометрия.
        /// </summary>
        public IGeometry CurrentGeometry
        {
            get => _currentGeometry;
            set
            {
                _currentGeometry = value;
                OnPropertyChanged(nameof(CurrentGeometry));
            }
        }
        private IGeometry _currentGeometry;

        /// <summary>
        /// Верхняя плоскость.
        /// </summary>
        public PlaneGeometry PlaneUp
        {
            get => _planeUp;
            set
            {
                _planeUp = value;
                OnPropertyChanged(nameof(PlaneUp));
            }
        }
        private PlaneGeometry _planeUp = new PlaneGeometry();

        /// <summary>
        /// Нижняя плоскость.
        /// </summary>
        public PlaneGeometry PlaneDown
        {
            get => _planeDown;
            set
            {
                _planeDown = value;
                OnPropertyChanged(nameof(PlaneDown));
            }
        }
        private PlaneGeometry _planeDown = new PlaneGeometry();

        /// <summary>
        /// Размер стороны верхней плоскости.
        /// </summary>
        public double PlaneUpSide
        {
            get => _planeUpSide;
            set
            {
                _planeUpSide = value;
                OnPropertyChanged(nameof(PlaneUpSide));
                PlaneUp.Positions = null;
                PlaneUp.TriangleIndices = null;
            }
        }
        private double _planeUpSide = 20;

        /// <summary>
        /// Размер стороны нижней плоскости.
        /// </summary>
        public double PlaneDownSide
        {
            get => _planeDownSide;
            set
            {
                _planeDownSide = value;
                OnPropertyChanged(nameof(PlaneDownSide));
                PlaneDown.Positions = null;
                PlaneDown.TriangleIndices = null;
            }
        }
        private double _planeDownSide = 20;

        /// <summary>
        /// Координата Y верхней плоскости.
        /// </summary>
        public double PlaneUpCoordinateY
        {
            get => _planeUpCoordinateY;
            set
            {
                _planeUpCoordinateY = value;
                OnPropertyChanged(nameof(PlaneUpCoordinateY));
                PlaneUp.Positions = null;
                PlaneUp.TriangleIndices = null;
            }
        }
        private double _planeUpCoordinateY = 5;

        /// <summary>
        /// Координата Y нижней плоскости.
        /// </summary>
        public double PlaneDownCoordinateY
        {
            get => _planeDownCoordinateY;
            set
            {
                _planeDownCoordinateY = value;
                OnPropertyChanged(nameof(PlaneDownCoordinateY));
                PlaneDown.Positions = null;
                PlaneDown.TriangleIndices = null;
            }
        }
        private double _planeDownCoordinateY = -5;


        /// <summary>
        /// Допустимые значения шага угла для расчёта сферы.
        /// </summary>
        public Dictionary<double, string> StepAngle
        {
            get => _stepAngle;
        }
        private Dictionary<double, string> _stepAngle = SphereGeometry.StepAngle.Source.StepAngleDictonary;

        /// <summary>
        /// Команда очистить текущую геометрию.
        /// </summary>
        public ICommand ClearCurrentGeometryCommand
        {
            get
            {
                if (_clearCurrentGeometryCommand == null)
                {
                    _clearCurrentGeometryCommand = new DelegateCommand(ClearCurrentGeometry);
                }

                return _clearCurrentGeometryCommand;
            }
        }
        private ICommand _clearCurrentGeometryCommand;

        /// <summary>
        /// Команда получить сферу.
        /// </summary>
        public ICommand GetSphereCurrentGeometryCommand
        {
            get
            {
                if (_getSphereCurrentGeometryCommand == null)
                {
                    _getSphereCurrentGeometryCommand = new DelegateCommand(GetSphereCurrentGeometry);
                }

                return _getSphereCurrentGeometryCommand;
            }
        }
        private ICommand _getSphereCurrentGeometryCommand;

        /// <summary>
        /// Команда получить куб.
        /// </summary>
        public ICommand GetCubeCurrentGeometryCommand
        {
            get
            {
                if (_getCubeCurrentGeometryCommand == null)
                {
                    _getCubeCurrentGeometryCommand = new DelegateCommand(GetCubeCurrentGeometry);
                }

                return _getCubeCurrentGeometryCommand;
            }
        }
        private ICommand _getCubeCurrentGeometryCommand;

        //GetPlaneUpGeometryCommand

        /// <summary>
        /// Команда .
        /// </summary>
        public ICommand GetPlaneUpGeometryCommand
        {
            get
            {
                if (_getPlaneUpGeometryCommand == null)
                {
                    _getPlaneUpGeometryCommand = new DelegateCommand(GetPlaneUpGeometry);
                }

                return _getPlaneUpGeometryCommand;
            }
        }
        private ICommand _getPlaneUpGeometryCommand;

        /// <summary>
        /// Команда .
        /// </summary>
        public ICommand GetPlaneDownGeometryCommand
        {
            get
            {
                if (_getPlaneDownGeometryCommand == null)
                {
                    _getPlaneDownGeometryCommand = new DelegateCommand(GetPlaneDownGeometry);
                }

                return _getPlaneDownGeometryCommand;
            }
        }
        private ICommand _getPlaneDownGeometryCommand;

        #endregion Свойства.

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainViewModel()
        {
            //var g = SphereGeometry.StepAngle.Source.StepAngleDictonary.ContainsKey(2.0);

            //PlaneUp.BuildGeometry(new object[] { (double?)20, (double?)3.7 });

            //PlaneDown.BuildGeometry(new object[] { (double?)20, (double?)-2.7 });
        }

        #region Методы.

        /// <summary>
        /// Метод очистки текущей геометрии.
        /// </summary>
        private void ClearCurrentGeometry()
        {
            CurrentGeometry = null;
        }

        /// <summary>
        /// Метод получения и отображения сферы.
        /// </summary>
        private void GetSphereCurrentGeometry()
        {
            _sphereGeometry.BuildGeometry(new object[] { (double?)SphereAngleStep, (double?)SphereRadius });
            CurrentGeometry = _sphereGeometry;
        }

        /// <summary>
        /// Метод получения и отображения куба.
        /// </summary>
        private void GetCubeCurrentGeometry()
        {
            _cubeGeometry.BuildGeometry(new object[] { (double?)CubeSide, (double?)CubeChamferPrecent });
            CurrentGeometry = _cubeGeometry;
        }

        private void GetPlaneUpGeometry()
        {
            PlaneUp.BuildGeometry(new object[] { (double?)PlaneUpSide, (double?)PlaneUpCoordinateY });
        }

        private void GetPlaneDownGeometry()
        {
            PlaneDown.BuildGeometry(new object[] { (double?)PlaneDownSide, (double?)PlaneDownCoordinateY });
        }

        #endregion Методы.
    }
}
