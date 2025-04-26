using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF_FitnessClub.Windows
{
    /// <summary>
    /// Логика взаимодействия для ControlTestWindow.xaml
    /// </summary>
    public partial class ControlTestWindow : Window
    {
        private DateTime _lastProfileUpdate;
        private DateTime _lastTrainingUpdate;

        public ControlTestWindow()
        {
            InitializeComponent();
            
            _lastProfileUpdate = DateTime.Now;
            _lastTrainingUpdate = DateTime.Now;
        }
        
        #region События профиля клиента
        
        private void ProfileControl_ProfileDataChanged(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan timeSinceLastUpdate = currentTime - _lastProfileUpdate;
            _lastProfileUpdate = currentTime;

        }
        
        private void ResetProfileButton_Click(object sender, RoutedEventArgs e)
        {
            profileControl.Age = 30;
            profileControl.Height = 170;
            profileControl.Weight = 70.0;
            
            MessageBox.Show("Профиль сброшен к стандартным значениям.", "Сброс профиля", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        #endregion
        
        #region События плана тренировки
        
        private void TrainingControl_TrainingPlanChanged(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan timeSinceLastUpdate = currentTime - _lastTrainingUpdate;
            _lastTrainingUpdate = currentTime;
            

        }

        private void BeginnerPlanButton_Click(object sender, RoutedEventArgs e)
        {
            trainingControl.Duration = 30;
            trainingControl.ExercisesCount = 5;
            trainingControl.Difficulty = 0; 
        }
        
        private void IntermediatePlanButton_Click(object sender, RoutedEventArgs e)
        {
            trainingControl.Duration = 60;
            trainingControl.ExercisesCount = 10;
            trainingControl.Difficulty = 1; 
        }
        
        private void AdvancedPlanButton_Click(object sender, RoutedEventArgs e)
        {
            trainingControl.Duration = 90;
            trainingControl.ExercisesCount = 15;
            trainingControl.Difficulty = 2; 
        }
        
        private void ProPlanButton_Click(object sender, RoutedEventArgs e)
        {
            trainingControl.Duration = 120;
            trainingControl.ExercisesCount = 25;
            trainingControl.Difficulty = 3; 
        }
        
        #endregion
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
} 