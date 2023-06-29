namespace CrossSection.Interfaces
{
    public interface ICrossSection
    {
        /// <summary>
        /// Последняя наименьшая по модулю положительная координата сечения по оси Y.
        /// Если значение отлично от null, то сечение производилось.
        /// Назначение - сохранять уже усечённое состояние при повторном сечении.
        /// </summary>
        double? LastSectionCoordinateUpY { get; set; }

        /// <summary>
        /// Последняя наименьшая по модулю отрицательная координата сечения по оси Y.
        /// Если значение отлично от null, то сечение производилось.
        /// Назначение - сохранять уже усечённое состояние при повторном сечении.
        /// </summary>
        double? LastSectionCoordinateDownY { get; set; }

        /// <summary>
        /// Метод поперечного сечения.
        /// </summary>
        /// <param name="UpY"></param>
        /// <param name="DownY"></param>
        /// <returns></returns>
        IGeometry CrossSection(double UpY, double DownY);
    }
}
