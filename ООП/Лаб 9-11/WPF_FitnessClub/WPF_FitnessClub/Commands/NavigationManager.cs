using System;
using System.Windows;
using System.Windows.Controls;

namespace WPF_FitnessClub
{
    /// <summary>
    /// Менеджер навигации для переключения между различными страницами
    /// </summary>
    public class NavigationManager
    {
        private static NavigationManager _instance;
        private ContentControl _contentControl;
        private UserControl _currentView;

        /// <summary>
        /// Получение единственного экземпляра менеджера навигации
        /// </summary>
        public static NavigationManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new NavigationManager();
                return _instance;
            }
        }

        /// <summary>
        /// Текущий отображаемый пользовательский контрол
        /// </summary>
        public UserControl CurrentView => _currentView;

        /// <summary>
        /// Инициализация менеджера навигации
        /// </summary>
        /// <param name="contentControl">Контрол, в котором будет отображаться содержимое</param>
        public void Initialize(ContentControl contentControl)
        {
            _contentControl = contentControl;
        }

        /// <summary>
        /// Навигация к указанному пользовательскому контролу
        /// </summary>
        /// <param name="content">Контент для отображения</param>
        public void NavigateTo(UserControl content)
        {
            if (_contentControl == null)
                throw new InvalidOperationException("Менеджер навигации не инициализирован. Вызовите метод Initialize.");

            _contentControl.Content = content;
            _currentView = content;
            OnContentChanged(content);
        }

        /// <summary>
        /// Событие, возникающее при изменении контента
        /// </summary>
        public event EventHandler<object> ContentChanged;

        /// <summary>
        /// Вызов события изменения контента
        /// </summary>
        /// <param name="content">Новый контент</param>
        protected virtual void OnContentChanged(object content)
        {
            ContentChanged?.Invoke(this, content);
        }
    }
} 