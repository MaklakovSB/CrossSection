using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossSection.Interfaces
{
    public interface ICrossSection
    {
        /// <summary>
        /// Метод поперечного сечения.
        /// </summary>
        /// <param name="UpY"></param>
        /// <param name="DownY"></param>
        /// <returns></returns>
        IGeometry CrossSection(double UpY, double DownY);
    }
}
