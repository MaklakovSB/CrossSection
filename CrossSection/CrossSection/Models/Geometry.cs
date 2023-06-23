using CrossSection.Interfaces;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CrossSection.Models
{
    public abstract class Geometry : IGeometry, INotifyPropertyChanged
    {
        /// <summary>
        /// Коллекция вершин геометрии.
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
        /// Построить геметрию.
        /// </summary>
        public abstract void BuildGeometry(object[] args);

        /// <summary>
        /// Получить вершины геометрии.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected abstract Point3DCollection GetPointsGeometry(object[] args);

        /// <summary>
        /// Триангуляция вершин геометрии.
        /// </summary>
        /// <returns></returns>
        protected abstract Int32Collection Triangle();

        #region Имплементация INotifyPropertyChanged.

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Имплементация INotifyPropertyChanged.
    }
}
