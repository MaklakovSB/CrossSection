using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CrossSection.Models
{
    public class Visual3DModel : INotifyPropertyChanged
    {
        private CubeGeometry _cubeGeometry = new CubeGeometry();
        private SphereGeometry _sphereGeometry = new SphereGeometry();

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

        #endregion

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Visual3DModel()
        {
        }

        #region Методы

        /// <summary>
        /// Получить сферу.
        /// </summary>
        /// <param name="angleStep"></param>
        /// <param name="radius"></param>
        public void GetSphere(double angleStep, double radius)
        {
            _sphereGeometry.GetSphere(angleStep, radius);
            Positions = _sphereGeometry.Positions;
            TriangleIndices = _sphereGeometry.TriangleIndices;
        }

        /// <summary>
        /// Получить куб.
        /// </summary>
        /// <param name="cubeSide"></param>
        /// <param name="cubeChamferPrecent"></param>
        public void GetCube(double cubeSide, double cubeChamferPrecent = 0)
        {
            _cubeGeometry.GetCube(cubeSide, cubeChamferPrecent);
            Positions = _cubeGeometry.Positions;
            TriangleIndices = _cubeGeometry.TriangleIndices;
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
