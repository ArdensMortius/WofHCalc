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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WofHCalc.Views
{
    /// <summary>
    /// Interaction logic for confirm_action.xaml
    /// </summary>
    public partial class confirm_action : Window
    {
        DispatcherTimer timer;
        DispatcherTimer timerHelp;
        int left_to_wait;
        public confirm_action(string msg, int t=0)
        {
            InitializeComponent();
            Message.Content = msg;
            Timer.Visibility= Visibility.Collapsed;
            if (t > 0)
            {
                BYes.IsEnabled = false;
                Timer.Content = $" ({t})";
                int left_to_wait = t;
                Timer.Visibility = Visibility.Visible;
                timer = new DispatcherTimer();
                timerHelp = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(t);
                timerHelp.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += Timer_Tick;
                timerHelp.Tick += TimerHelp_Tick;
                timer.Start();
                timerHelp.Start();
            }
            
        }

        private void TimerHelp_Tick(object? sender, EventArgs e)
        {            
            Timer.Content = $" ({--left_to_wait})";
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            timer.Stop();
            BYes.IsEnabled = true;
            Timer.Visibility = Visibility.Collapsed;
        }
    }
}
