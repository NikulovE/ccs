using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CCS.Classic
{
    /// <summary>
    /// Interaction logic for TimePicker.xaml
    /// </summary>
    public partial class TimePicker : UserControl
    {
        public TimePicker()
        {
            InitializeComponent();
        }

        public TimeSpan Time
        {
            get {
                return (TimeSpan)GetValue(TimeProperty);
            }
            set {
                SetValue(TimeProperty, value);
            }
        }
        public static readonly DependencyProperty TimeProperty =
        DependencyProperty.Register("Time", typeof(TimeSpan), typeof(TimePicker),
        new UIPropertyMetadata(DateTime.Now.TimeOfDay, new PropertyChangedCallback(OnValueChanged)));

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimePicker control = obj as TimePicker;
            control.Hours = ((TimeSpan)e.NewValue).Hours;
            control.Minutes = ((TimeSpan)e.NewValue).Minutes;
            control.Seconds = ((TimeSpan)e.NewValue).Seconds;
        }

        public int Hours
        {
            get { return (int)GetValue(HoursProperty); }
            set { SetValue(HoursProperty, value); }
        }

        private List<int> hours = new List<int> { 00, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };

        public List<int> HoursArr
        {
            get
            {
                return hours;
            }
            set
            {
                this.hours = value;
            }
        }
        private List<int> minutes = new List<int> { 00, 10, 20, 30, 40, 50 };

        public List<int> MinutesArr
        {
            get
            {
                return minutes;
            }
            set
            {
                this.minutes = value;
            }
        }

        public static readonly DependencyProperty HoursProperty =
        DependencyProperty.Register("Hours", typeof(int), typeof(TimePicker),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));

        public int Minutes
        {
            get { return (int)GetValue(MinutesProperty); }
            set { SetValue(MinutesProperty, value); }
        }
        public static readonly DependencyProperty MinutesProperty =
        DependencyProperty.Register("Minutes", typeof(int), typeof(TimePicker),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));

        public int Seconds
        {
            get { return (int)GetValue(SecondsProperty); }
            set { SetValue(SecondsProperty, value); }
        }

        public static readonly DependencyProperty SecondsProperty =
        DependencyProperty.Register("Seconds", typeof(int), typeof(TimePicker),
        new UIPropertyMetadata(0, new PropertyChangedCallback(OnTimeChanged)));


        private static void OnTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            TimePicker control = obj as TimePicker;
            control.Time = new TimeSpan(control.Hours, control.Minutes, control.Seconds);
        }

        public event RoutedEventHandler TimeChanged;


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TimeChanged != null)
            {
                TimeChanged(this, e);
            }
        }

    }
}
