using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MKTimer.gameLogic;

namespace MKTimer.elements;

public partial class PacePanel : Panel
{
    public PacePanel(TrackInfo trackInfo)
    {
        InitializeComponent(trackInfo);
    }

    private void CreateSegment(List<MkTime>[] goals, Run[] runs)
    {
        var segmentCounter = 0;
        foreach (var times in goals)
        {
            var segment = new Panel()
            {
                Size = new Size(250, times.Count * 50),
                Location = new Point(segmentCounter * 250, 0)
            };
            
            var counter = 0;
            foreach (var time in times)
            {
                var occurrences = segmentCounter < 3 
                    ? runs.Select(run => time.Matches(run.laps[segmentCounter])).Count(isTrue => isTrue) 
                    : runs.Select(run => time.Matches(new MkTime(run.GetTotalTime()))).Count(isTrue => isTrue);
                
                var label = new Label()
                {
                    Text = time + ": " + occurrences,
                    AutoSize = true,
                    Location = new Point(0, counter * 50),
                };
                segment.Controls.Add(label);

                counter++;
            }
            Controls.Add(segment);
            segmentCounter++;
        }
    }
}