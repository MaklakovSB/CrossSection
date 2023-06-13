using CrossSection.Delegates;
using CrossSection.Models;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace CrossSection.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Основная модель.
        /// </summary>
        public Visual3DModel Main3DObjectModel 
        {
            get => _main3DObjectModel; 
        }
        private Visual3DModel _main3DObjectModel = new Visual3DModel();

        /// <summary>
        /// 
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
        /// 
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
        /// 
        /// </summary>
        private void ClearMain3DObjectModel()
        {
            Main3DObjectModel.TriangleIndices = null;//.Clear();
            Main3DObjectModel.Positions = null;
            OnPropertyChanged(nameof(Main3DObjectModel.TriangleIndices));
            OnPropertyChanged(nameof(Main3DObjectModel.Positions));
        }

        private void GetSphereMain3DObjectModel()
        {
            Main3DObjectModel.TriangleIndices = null;
            Main3DObjectModel.Positions = null;
            OnPropertyChanged(nameof(Main3DObjectModel.TriangleIndices));
            OnPropertyChanged(nameof(Main3DObjectModel.Positions));

            Main3DObjectModel.GetSphere(10, 12);
        }
    }
}
