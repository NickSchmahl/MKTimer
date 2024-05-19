using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MKTimer.gameLogic;

namespace MKTimer.elements
{
    /// <summary>
    /// Represents a panel that displays pace information for a track.
    /// </summary>
    public partial class PacePanel : Panel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PacePanel"/> class.
        /// </summary>
        /// <param name="trackInfo">The track information.</param>
        public PacePanel(TrackInfo trackInfo)
        {
            InitializeComponent(trackInfo);
        }

        /// <summary>
        /// Creates a segment of the pace panel.
        /// </summary>
        /// <param name="goals">The list of goals for each segment.</param>
        /// <param name="runs">The runs for each segment.</param>
        private void CreateSegment(List<MkTime>[] goals, Run[] runs)
        {
            var segmentCounter = 0;
            foreach (var times in goals)
            {
                // Create a new panel for each segment
                var segment = new Panel()
                {
                    Size = new Size(250, times.Count * 50),
                    Location = new Point(segmentCounter * 250, 0)
                };

                for (var counter = 0; counter < times.Count; counter++)
                {
                    var time = times[counter];
                    var occurrences = runs.Count(run =>
                        time.Matches(segmentCounter < 3
                            ? run.laps[segmentCounter]
                            : new MkTime(run.GetTotalTime()))
                    );

                    var label = new Label
                    {
                        Text = $"{time}: {occurrences}",
                        AutoSize = true,
                        Location = new Point(0, counter * 50),
                    };
                    segment.Controls.Add(label);
                }

                Controls.Add(segment);
                segmentCounter++;
            }
        }
    }
}