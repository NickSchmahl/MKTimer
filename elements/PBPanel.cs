using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MKTimer.gameLogic;

namespace MKTimer.elements {
    public class PBPanel : Panel {
        private readonly Run? _pbRun;
        public Run? sbRun;
        public Button savePbButton = new();
        private Label pbLabel = new();
        private Label sbLabel = new();

        public PBPanel(MK8DLXTrack track, MK8DLXMode mode) {
            _pbRun = PBParser.GetPbRun(track, mode);
            Initialize();
        }

        private void Initialize() {
            AutoSize = true;
            pbLabel = new Label {
                Location = new Point(0, 0),
                AutoSize = true
            };
            if (_pbRun != null) {
                pbLabel.Text = GetBestString("Personal", _pbRun);
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
            if (sbRun == null) {
                sbRun = new Run(newLapTimes);
            } else if (sbRun != null && newLapTimes.Sum() < sbRun.GetTotalTime()) {
                sbRun = new Run(newLapTimes);
            }
            if (sbRun != null) {
                sbLabel.Text = GetBestString("Session", sbRun);
            }
            if (_pbRun != null) {
                pbLabel.Text = GetBestString("Personal ", _pbRun);
            }
        }

        private static string GetBestString(string sbOrPb, Run run) {
            double ? totalTime = run.GetTotalTime();
            string retString;
            if (totalTime == null) return "-";
            else retString = sbOrPb + " Best: " + TimeParser.GetTimeString((double) totalTime) + " (";
            foreach (MkTime? lapTime in run.laps) 
            {
                if (lapTime == null) retString += " - | ";
                else retString += lapTime.ToString() + " | ";
            }
            retString = retString.Remove(retString.Length-2);
            retString += ") ";

            return retString;
        }
    }
}