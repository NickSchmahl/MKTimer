using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MKTimer {
    public class PBPanel : Panel {
        public Run? pbRun;
        public Run? sbRun;
        public Button savePbButton = new();
        private Label pbLabel = new();
        private Label sbLabel = new();

        public PBPanel(MK8DLXTrack track, MK8DLXMode mode) {
            pbRun = PBParser.GetPbRun(track, mode);
            initialize();
        }

        private void initialize() {
            AutoSize = true;
            pbLabel = new Label {
                Location = new Point(0, 0),
                AutoSize = true
            };
            if (pbRun != null) {
                pbLabel.Text = GetBestString("Personal", pbRun);
            } else {
                pbLabel.Text = "Personal Best: -";
            }

            sbLabel = new Label {
                Location = new Point(0, 50),
                AutoSize = true
            };
            if (sbRun != null) {
                sbLabel.Text = GetBestString("Session", sbRun);
            } else {
                sbLabel.Text = "Session Best: -";
            }

            savePbButton = new Button
            {
                Text = "Save PB",
                Location = new Point(1000, 0),
                AutoSize = true
            };

            Controls.Add(pbLabel);
            Controls.Add(sbLabel);
            Controls.Add(savePbButton);
        }

        public void Update(double?[] newLapTimes) {
            if (sbRun == null && newLapTimes != null) {
                sbRun = new Run(newLapTimes);
            } else if (newLapTimes != null && sbRun != null && newLapTimes.Sum() < sbRun.GetTotalTime()) {
                sbRun = new Run(newLapTimes);
            }
            if (sbRun != null) {
                sbLabel.Text = GetBestString("Session", sbRun);
            }
            if (pbRun != null) {
                pbLabel.Text = GetBestString("Personal ", pbRun);
            }
        }

        private static string GetBestString(string sbOrPb, Run run) {
            double ? total_time = run.GetTotalTime();
            string retString;
            if (total_time == null) return "-";
            else retString = sbOrPb + " Best: " + TimeParser.GetTimeString((double) total_time) + " (";
            if (run.laps != null) {
                foreach (MKTime? lap_time in run.laps) 
                {
                    if (lap_time == null) retString += " - | ";
                    else retString += lap_time.ToString() + " | ";
                }
                retString = retString.Remove(retString.Length-2);
                retString += ") ";
            }

            return retString;
        }
    }
}