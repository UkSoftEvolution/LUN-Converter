using LUN_Converter.Other;
using System.Windows;

namespace LUN_Converter.ViewModel
{
    /// <summary>
    /// ViewModel: Для MainView
    /// </summary>
    public class MainViewModel
    {
        #region Commands
        /// <summary>
        /// Команда для кнопки сворачивания программы
        /// </summary>
        public RelayCommand Minimized => new RelayCommand(obj =>
        {
            Window window = (obj as Window);
            window.WindowState = WindowState.Minimized;
        });
        /// <summary>
        /// Команда для кнопки закрытия программы
        /// </summary>
        public RelayCommand Close => new RelayCommand(obj =>
        {
            Window window = (obj as Window);
            window.Close();
        });
        #endregion
    }
}