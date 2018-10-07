using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace LocationServer
{
    public class StopwatchTextBox
    {
        public TextBox TbTimer { get; set; }
        public StopwatchTextBox(TextBox tb)
        {
            TbTimer = tb;
        }

        private DispatcherTimer timerStopwatch;

        Stopwatch insertStopwatch = new Stopwatch();

        public void Start()
        {
            if (timerStopwatch == null)
            {
                timerStopwatch = new DispatcherTimer();
                timerStopwatch.Interval = TimeSpan.FromMilliseconds(250);
                timerStopwatch.Tick += (sender, e) =>
                {
                    TbTimer.Text = insertStopwatch.Elapsed.ToString();
                };
            }
            insertStopwatch.Start();
            timerStopwatch.Start();
        }

        public void Stop()
        {
            if (timerStopwatch != null)
            {
                timerStopwatch.Stop();
                timerStopwatch = null;
            }

            insertStopwatch.Stop();
        }
    }
}
