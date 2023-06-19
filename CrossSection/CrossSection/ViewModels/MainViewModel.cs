using CrossSection.Delegates;
using CrossSection.Interfaces;
using CrossSection.Models;
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
        private double _cubeChamferPrecent = 2;

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
        private double _sphereAngleStep = 10;

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
        public ICommand GetCubeMain3DObjectModelCommand
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

        #endregion Свойства.

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainViewModel()
        {

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

        #endregion Методы.
    }
}
