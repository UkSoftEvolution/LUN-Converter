using LUN_Converter.Other;
using LUN_Converter.View;
using System.Windows;
using System.Windows.Controls;

namespace LUN_Converter.ViewModel
{
    /// <summary>
    /// ViewModel: Для MainView
    /// </summary>
    public class MainViewModel : MVVM
    {
        #region Fields
        private Page activePage; //Активная страница
        #endregion

        #region Constructors
        /// <summary>
        /// Стандартный конструктор для инициализации переменных
        /// </summary>
        public MainViewModel() => ActivePage = new ContentView();
        #endregion

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

        #region Methods
        /// <summary>
        /// Активная страница
        /// </summary>
        public Page ActivePage
        {
            get => activePage;
            set
            {
                activePage = value;
                OnPropertyChanged(nameof(activePage));
            }
        }
        #endregion
    }
}