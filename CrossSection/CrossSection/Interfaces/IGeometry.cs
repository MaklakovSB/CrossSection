using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace CrossSection.Interfaces
{
    public interface IGeometry
    {
        /// <summary>
        /// Коллекция вершин геометрии.
        /// </summary>
        Point3DCollection Positions { get; set; }

        /// <summary>
        /// Коллекция индексов вершин геометрии.
        /// </summary>
        Int32Collection TriangleIndices{ get; set; }

        /// <summary>
        /// Построить геометрию.
        /// </summary>
        /// <param name="args"></param>
        void BuildGeometry(object[] args );

    }
}
