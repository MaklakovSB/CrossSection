using System.Windows;

namespace CrossSection.Interfaces
{
    public interface IView
    {
        /// <summary>
        /// Владелец представления.
        /// </summary>
        Window Owner { get; set; }

        /// <summary>
        /// Контекст - модель представления.
        /// </summary>
        IViewModel ViewModel { get; set; }

        /// <summary>
        /// Метод отображения представления.
        /// </summary>
        void Show();

        /// <summary>
        /// Метод отображения представления в диалоговом режиме.
        /// </summary>
        /// <returns></returns>
        bool? ShowDialog();

        /// <summary>
        /// Метод закрытия представления.
        /// </summary>
        void Close();
    }
}
