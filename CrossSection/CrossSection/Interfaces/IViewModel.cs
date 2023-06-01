using System;

namespace CrossSection.Interfaces
{
    /// <summary>
    /// Интерфейс для модели представления.
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// Событие закрытия.
        /// </summary>
        event Action Close;

        /// <summary>
        /// Метод закрытия.
        /// </summary>
        void OnClose();
    }
}
