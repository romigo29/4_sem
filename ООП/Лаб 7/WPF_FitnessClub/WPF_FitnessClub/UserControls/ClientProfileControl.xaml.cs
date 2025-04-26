using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF_FitnessClub.UserControls
{
    /// <summary>
    /// Логика взаимодействия для ClientProfileControl.xaml
    /// </summary>
    public partial class ClientProfileControl : UserControl
    {
        private static readonly Regex _numberRegex = new Regex(@"^[0-9]*$");

        #region Dependency Properties

        public static readonly DependencyProperty AgeProperty =
            DependencyProperty.Register(
                "Age",
                typeof(int),
                typeof(ClientProfileControl),
                new FrameworkPropertyMetadata(
                    30, 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnProfileDataChanged),
                    new CoerceValueCallback(CoerceAge)),
                new ValidateValueCallback(ValidateAge));

        public static new readonly DependencyProperty HeightProperty =
            DependencyProperty.Register(
                "Height",
                typeof(int),
                typeof(ClientProfileControl),
                new FrameworkPropertyMetadata(
                    170, 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnProfileDataChanged),
                    new CoerceValueCallback(CoerceHeight)),
                new ValidateValueCallback(ValidateHeight));

        public static readonly DependencyProperty WeightProperty =
            DependencyProperty.Register(
                "Weight",
                typeof(double),
                typeof(ClientProfileControl),
                new FrameworkPropertyMetadata(
                    70.0, 
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    new PropertyChangedCallback(OnProfileDataChanged),
                    new CoerceValueCallback(CoerceWeight)),
                new ValidateValueCallback(ValidateWeight));

        public static readonly DependencyProperty BMIProperty =
            DependencyProperty.Register(
                "BMI",
                typeof(double),
                typeof(ClientProfileControl),
                new PropertyMetadata(0.0));

        public static readonly DependencyProperty BMIBackgroundProperty =
            DependencyProperty.Register(
                "BMIBackground",
                typeof(Brush),
                typeof(ClientProfileControl),
                new PropertyMetadata(Brushes.LightGray));

        #endregion

        #region События

        public event EventHandler ProfileDataChanged;

        #endregion

        public ClientProfileControl()
        {
            InitializeComponent();
            UpdateBMI();
            
            AgeTextBox.LostFocus += TextBox_LostFocus;
            HeightTextBox.LostFocus += TextBox_LostFocus;
            WeightTextBox.LostFocus += TextBox_LostFocus;
        }

        #region Public Properties

        public int Age
        {
            get { return (int)GetValue(AgeProperty); }
            set { SetValue(AgeProperty, value); }
        }

        public new int Height
        {
            get { return (int)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        public double Weight
        {
            get { return (double)GetValue(WeightProperty); }
            set { SetValue(WeightProperty, value); }
        }


        public double BMI
        {
            get { return (double)GetValue(BMIProperty); }
            private set { SetValue(BMIProperty, value); }
        }

        public Brush BMIBackground
        {
            get { return (Brush)GetValue(BMIBackgroundProperty); }
            private set { SetValue(BMIBackgroundProperty, value); }
        }

        #endregion

        #region Validation Methods

        private static bool ValidateAge(object value)
        {
            if (value is int age)
            {
                return age >= 0; 
            }
            return false;
        }

        private static object CoerceAge(DependencyObject d, object baseValue)
        {
            int age = (int)baseValue;
            
            if (age > 120)
                return 120;
            
            return baseValue;
        }

        private static bool ValidateHeight(object value)
        {
            if (value is int height)
            {
                return height >= 0;
            }
            return false;
        }

        private static object CoerceHeight(DependencyObject d, object baseValue)
        {
            int height = (int)baseValue;
            
            if (height < 30)
                return 30;
            
 
            if (height > 300)
                return 300;
            
            return baseValue;
        }

        private static bool ValidateWeight(object value)
        {
            if (value is double weight)
            {
                return weight >= 0 && !double.IsNaN(weight) && !double.IsInfinity(weight);
            }
            return false;
        }

        private static object CoerceWeight(DependencyObject d, object baseValue)
        {
            double weight = (double)baseValue;
            
            if (weight < 1)
                return 1.0;
     
            if (weight > 500)
                return 500.0;
            
            return Math.Round(weight, 1);
        }

        #endregion

        #region Event Handlers

        private static void OnProfileDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ClientProfileControl control = (ClientProfileControl)d;
            control.UpdateBMI();
            control.ProfileDataChanged?.Invoke(control, EventArgs.Empty);
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
                if (textBox == AgeTextBox)
                {
                    Age = 30;
                }
                else if (textBox == HeightTextBox)
                {
                    Height = 170;
                }
                else if (textBox == WeightTextBox)
                {
                    Weight = 70.0;
                }
                
                textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateTarget();
            }
        }

        #endregion

        #region Helper Methods

        private void UpdateBMI()
        {
            if (Height <= 0)
            {
                BMI = 0;
                BMIBackground = Brushes.LightGray;
                return;
            }

            double heightInMeters = Height / 100.0;
            double bmi = Weight / (heightInMeters * heightInMeters);
            BMI = Math.Round(bmi, 1);

            if (BMI < 18.5)
            {
                BMIBackground = new SolidColorBrush(Color.FromRgb(173, 216, 230)); // LightBlue
            }
            else if (BMI >= 18.5 && BMI < 25)
            {
    
                BMIBackground = new SolidColorBrush(Color.FromRgb(144, 238, 144)); // LightGreen
            }
            else if (BMI >= 25 && BMI < 30)
            {
                BMIBackground = new SolidColorBrush(Color.FromRgb(255, 255, 0)); // Yellow
            }
            else
            {
                BMIBackground = new SolidColorBrush(Color.FromRgb(255, 99, 71)); // Tomato
            }
        }

        #endregion
    }
} 