using System;
using System.Drawing;
using System.Windows.Forms;

namespace MKTimer {
    public class RunCountPanel : Panel {
        private readonly Label runCountLabel = new();
        private readonly Label timerLabel = new();
        public readonly Timer timer = new();
        public static int secondsCounter = 0;
        private readonly int baseCount;

        public RunCountPanel(int baseCount, int baseSecondsPlayed) 
        {
            this.baseCount = baseCount;
            secondsCounter = baseSecondsPlayed;

            runCountLabel.Text = (baseCount + TimerGridView.RUN_COUNT).ToString();
            runCountLabel.Size = new Size(100, 50);

            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            timerLabel.Text = TimerString();
            timerLabel.Size = new Size(500, 50);
            timerLabel.Location = new Point(100, 0);

            Controls.Add(runCountLabel);
            Controls.Add(timerLabel);
        }

        public void UpdateRowCount() 
        {
            runCountLabel.Text = (baseCount + TimerGridView.RUN_COUNT).ToString();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            // Update the label with the current time
            secondsCounter++;
            timerLabel.Text = TimerString();
        }

        private string TimerString()
        {
            int hours = secondsCounter / 3660;
            int minutes = secondsCounter/ 60 % 60;
            int seconds = secondsCounter % 60;

            string result = "";
            if (hours > 0 && hours < 10) result += "0" + hours + ":";
            else if (hours >= 10) result += hours + ":";

            if (minutes == 0) result += "00";
            else if (minutes > 0 && minutes < 10) result += "0" + minutes;
            else if (minutes >= 10) result += minutes;

            result += ":";

            if (seconds == 0) result += "00";
            else if (seconds > 0 && seconds < 10) result += "0" + seconds;
            else if (seconds >= 10) result += seconds;

            return result;
        }
    }
}