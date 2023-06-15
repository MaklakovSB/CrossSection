using CrossSection.Delegates;
using CrossSection.Models;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace CrossSection.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
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
        /// Основная 3d модель.
        /// </summary>
        public Visual3DModel Main3DObjectModel 
        {
            get => _main3DObjectModel; 
        }
        private Visual3DModel _main3DObjectModel = new Visual3DModel();

        /// <summary>
        /// Команда очистить основную 3d модель.
        /// </summary>
        public ICommand ClearMain3DObjectModelCommand
        {
            get
            {
                if (_clearMain3DObjectModelCommand == null)
                {
                    _clearMain3DObjectModelCommand = new DelegateCommand(ClearMain3DObjectModel);
                }

                return _clearMain3DObjectModelCommand;
            }
        }
        private ICommand _clearMain3DObjectModelCommand;

        /// <summary>
        /// Команда получить сферу.
        /// </summary>
        public ICommand GetSphereMain3DObjectModelCommand
        {
            get
            {
                if (_getSphereMain3DObjectModelCommand == null)
                {
                    _getSphereMain3DObjectModelCommand = new DelegateCommand(GetSphereMain3DObjectModel);
                }

                return _getSphereMain3DObjectModelCommand;
            }
        }
        private ICommand _getSphereMain3DObjectModelCommand;

        /// <summary>
        /// Команда получить куб.
        /// </summary>
        public ICommand GetCubeMain3DObjectModelCommand
        {
            get
            {
                if (_getCubeMain3DObjectModelCommand == null)
                {
                    _getCubeMain3DObjectModelCommand = new DelegateCommand(GetCubeMain3DObjectModel);
                }

                return _getCubeMain3DObjectModelCommand;
            }
        }
        private ICommand _getCubeMain3DObjectModelCommand;

        #endregion Свойства.

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainViewModel()
        {
            Main3DObjectModel.Positions.Add(new Point3D { X = -20, Y = 0, Z = -20 });
            Main3DObjectModel.Positions.Add(new Point3D { X = 20, Y = 0, Z = -20 });
            Main3DObjectModel.Positions.Add(new Point3D { X = 20, Y = 0, Z = 20 });
            Main3DObjectModel.Positions.Add(new Point3D { X = -20, Y = 0, Z = 20 });

            Main3DObjectModel.TriangleIndices.Add(2);
            Main3DObjectModel.TriangleIndices.Add(1);
            Main3DObjectModel.TriangleIndices.Add(0);

            Main3DObjectModel.TriangleIndices.Add(3);
            Main3DObjectModel.TriangleIndices.Add(2);
            Main3DObjectModel.TriangleIndices.Add(0);
        }

        /// <summary>
        /// Метод очистки основной 3D модели.
        /// </summary>
        private void ClearMain3DObjectModel()
        {
            Main3DObjectModel.TriangleIndices = null;
            Main3DObjectModel.Positions = null;
        }

        /// <summary>
        /// Метод получения и отображения сферы.
        /// </summary>
        private void GetSphereMain3DObjectModel()
        {
            Main3DObjectModel.GetSphere(SphereAngleStep, SphereRadius);
        }

        /// <summary>
        /// Метод получения и отображения куба.
        /// </summary>
        private void GetCubeMain3DObjectModel()
        {
            Main3DObjectModel.GetCube(CubeSide, CubeChamferPrecent);
        }
    }
}
