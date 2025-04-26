using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF_FitnessClub.UserControls
{
    /// <summary>
    /// Логика взаимодействия для TrainingPlanControl.xaml
    /// </summary>
    public partial class TrainingPlanControl : UserControl
    {
        private static readonly Regex _numberRegex = new Regex(@"^[0-9]*$");

        #region Dependency Properties

        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(
                "Duration",
                typeof(int),
                typeof(TrainingPlanControl),
                new FrameworkPropertyMetadata(
                    60, 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnTrainingPlanChanged),
                    new CoerceValueCallback(CoerceDuration)),
                new ValidateValueCallback(ValidateDuration));

        public static readonly DependencyProperty ExercisesCountProperty =
            DependencyProperty.Register(
                "ExercisesCount",
                typeof(int),
                typeof(TrainingPlanControl),
                new FrameworkPropertyMetadata(
                    10,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnTrainingPlanChanged),
                    new CoerceValueCallback(CoerceExercisesCount)),
                new ValidateValueCallback(ValidateExercisesCount));

        public static readonly DependencyProperty DifficultyProperty =
            DependencyProperty.Register(
                "Difficulty",
                typeof(int),
                typeof(TrainingPlanControl),
                new FrameworkPropertyMetadata(
                    1, 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnTrainingPlanChanged),
                    new CoerceValueCallback(CoerceDifficulty)),
                new ValidateValueCallback(ValidateDifficulty));

        public static readonly DependencyProperty IntensityProperty =
            DependencyProperty.Register(
                "Intensity",
                typeof(string),
                typeof(TrainingPlanControl),
                new PropertyMetadata("Средняя"));

        public static readonly DependencyProperty IntensityBackgroundProperty =
            DependencyProperty.Register(
                "IntensityBackground",
                typeof(Brush),
                typeof(TrainingPlanControl),
                new PropertyMetadata(Brushes.LightGray));

        #endregion

        #region События

        public event EventHandler TrainingPlanChanged;

        #endregion

        public TrainingPlanControl()
        {
            InitializeComponent();
            UpdateIntensity();
            
            DurationTextBox.LostFocus += TextBox_LostFocus;
            ExercisesCountTextBox.LostFocus += TextBox_LostFocus;
        }

        #region Public Properties
        public int Duration
        {
            get { return (int)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        public int ExercisesCount
        {
            get { return (int)GetValue(ExercisesCountProperty); }
            set { SetValue(ExercisesCountProperty, value); }
        }
        public int Difficulty
        {
            get { return (int)GetValue(DifficultyProperty); }
            set { SetValue(DifficultyProperty, value); }
        }
        public string Intensity
        {
            get { return (string)GetValue(IntensityProperty); }
            private set { SetValue(IntensityProperty, value); }
        }
        public Brush IntensityBackground
        {
            get { return (Brush)GetValue(IntensityBackgroundProperty); }
            private set { SetValue(IntensityBackgroundProperty, value); }
        }

        #endregion

        #region Validation Methods

        private static bool ValidateDuration(object value)
        {
            if (value is int duration)
            {
                return duration >= 0; 
            }
            return false;
        }

        private static object CoerceDuration(DependencyObject d, object baseValue)
        {
            int duration = (int)baseValue;
 
            if (duration < 5)
                return 5;
            
            if (duration > 180)
                return 180;
            
            return baseValue;
        }

        private static bool ValidateExercisesCount(object value)
        {
            if (value is int count)
            {
                return count >= 0; 
            }
            return false;
        }

        private static object CoerceExercisesCount(DependencyObject d, object baseValue)
        {
            int count = (int)baseValue;
            
            if (count < 1)
                return 1;
            

            if (count > 50)
                return 50;
            
            return baseValue;
        }

        private static bool ValidateDifficulty(object value)
        {
            if (value is int difficulty)
            {
                return difficulty >= 0;
            }
            return false;
        }

        private static object CoerceDifficulty(DependencyObject d, object baseValue)
        {
            int difficulty = (int)baseValue;
            
            if (difficulty < 0)
                return 0;
            
            if (difficulty > 3)
                return 3;
            
            return baseValue;
        }

        #endregion

        #region Event Handlers

        private static void OnTrainingPlanChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TrainingPlanControl control = (TrainingPlanControl)d;
            control.UpdateIntensity();
            control.TrainingPlanChanged?.Invoke(control, EventArgs.Empty);
        }

 
        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !_numberRegex.IsMatch(e.Text);
        }
        
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;
            
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                if (textBox == DurationTextBox)
                {
                    Duration = 60;
                }
                else if (textBox == ExercisesCountTextBox)
                {
                    ExercisesCount = 10;
                }
                
                textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            }
        }

        #endregion

        #region Helper Methods

        private void UpdateIntensity()
        {

            double intensityValue = (Duration * ExercisesCount * (Difficulty + 1)) / 100.0;
            

            if (intensityValue < 10)
            {
                Intensity = "Низкая";
                IntensityBackground = new SolidColorBrush(Color.FromRgb(173, 216, 230)); 
            }
            else if (intensityValue >= 10 && intensityValue < 30)
            {
                Intensity = "Средняя";
                IntensityBackground = new SolidColorBrush(Color.FromRgb(144, 238, 144));
            }
            else if (intensityValue >= 30 && intensityValue < 60)
            {
                Intensity = "Высокая";
                IntensityBackground = new SolidColorBrush(Color.FromRgb(255, 255, 0)); 
            }
            else
            {
                Intensity = "Экстремальная";
                IntensityBackground = new SolidColorBrush(Color.FromRgb(255, 99, 71)); 
            }
        }

        #endregion
    }
} 