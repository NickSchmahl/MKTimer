using System.Drawing;
using System.Windows.Forms;

namespace MKTimer {
    public class TrackSelectionPanel : Panel {
        private readonly Label trackName;
        public Label gameMode;
        public TrackSelectionPanel(MK8DLXTrack track, MK8DLXMode mode) {
            trackName = new Label
            {
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = track.GetTrackName(),
                TextAlign = ContentAlignment.MiddleLeft,
                Location = new Point(0, 0)
            };

            gameMode = new Label 
            {
                Font = new Font("Arial", 12, FontStyle.Bold),
                Text = mode.getMode(),
                TextAlign = ContentAlignment.MiddleRight,
                Location = new Point(this.Size.Width / 2, 0)
            };

            this.Controls.Add(trackName);
            this.Controls.Add(gameMode);
        }

        public void arrangeLabels() {
            this.trackName.Size = new Size(this.Size.Width / 2, 50);
            this.trackName.Location = new Point(0, 0);

            this.gameMode.Size = new Size(this.Size.Width / 2, 50);
            this.gameMode.Location = new Point(this.Size.Width / 2, 0);
        }
    }

}