using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.IO;
using System;
using System.Linq;

namespace MKTimer
{
    partial class Form1
    {
        private TimerGridView timerGridView;
        private TrackSelectionPanel trackSelectionPanel;
        private RunCountPanel runCountPanel;
        private PopUpMenu popUpMenu;
        private TrackInfo trackInfo;

        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            initializeTimerGridView();
            initializeTrackSelectionPanel();
            initializeRunCountPanel();
            timerGridView.runCountPanel = runCountPanel;

            popUpMenu = new(this);

            // MouseClickAction
            MouseClick += Form1_MouseClick;
            FormClosed += FormClosedAction;
            KeyDown += KeyDownAction;

            // Form
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            // this.ClientSize = new System.Drawing.Size(this.dataGridView1.Size.Width, this.dataGridView1.Size.Height);
            this.Text = "MKTimer";
            this.ResumeLayout(false);
        }

        public void UpdateSelection(MK8DLXTrack track, MK8DLXMode mode)
        {
            this.trackInfo = new TrackInfo(track, mode);

            Controls.Remove(timerGridView);
            Controls.Remove(trackSelectionPanel);
            Controls.Remove(runCountPanel);
            initializeTimerGridView();
            initializeTrackSelectionPanel();
            initializeRunCountPanel();
            timerGridView.runCountPanel = runCountPanel;
        }

        private void initializeTimerGridView()
        {
            timerGridView = new TimerGridView(trackInfo);
            timerGridView.Location = new Point(0, 100);
            timerGridView.CellEndEdit += cellEndEdit;
            timerGridView.MouseClick += Form1_MouseClick;
            timerGridView.KeyDown += KeyDownAction;

            Controls.Add(timerGridView);
            ((System.ComponentModel.ISupportInitialize)(this.timerGridView)).EndInit();
        }

        private void initializeRunCountPanel() 
        {
            runCountPanel = new RunCountPanel(trackInfo.runCount, trackInfo.secondsPlayed) {
                Location = new Point(0, 50),
                Size = new Size(timerGridView.Size.Width, 50)
            };
            trackSelectionPanel.MouseClick += Form1_MouseClick;
            trackSelectionPanel.KeyDown += KeyDownAction;

            Controls.Add(runCountPanel);
        }

        private void initializeTrackSelectionPanel()
        {
            trackSelectionPanel = new TrackSelectionPanel(trackInfo.track, trackInfo.mode);
            trackSelectionPanel.Location = new Point(0, 0);
            trackSelectionPanel.Size = new Size(timerGridView.Size.Width, 50);
            trackSelectionPanel.arrangeLabels();
            trackSelectionPanel.MouseClick += Form1_MouseClick;
            trackSelectionPanel.KeyDown += KeyDownAction;
            
            Controls.Add(this.trackSelectionPanel);
        }

        private void cellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            double sum = 0;
            MKTime[] lapTimes = new MKTime[3];
            bool allFilled = true;
            Run sob = trackInfo.sob?.Copy();

            // Calculate the sum of the first three columns in the current row
            for (int i = 1; i < TimerGridView.SUM_COLUMN_INDEX; i++)
            {
                if (this.timerGridView[i, rowIndex].Value != null)
                {
                    MKTime parsed_time = new(this.timerGridView[i, rowIndex].Value.ToString());
                    if (parsed_time.ToDouble() != 0) {
                        lapTimes[i-1] = parsed_time;
                        sum += parsed_time.ToDouble();
                        timerGridView[i, rowIndex].Value = parsed_time.ToString();
                        timerGridView.runs[rowIndex-3].laps[i-1] = parsed_time;

                        // Update sob
                        if (sob != null && sob.laps != null && sob.laps[i-1].ToDouble() > parsed_time.ToDouble()) 
                        {
                            sob.laps[i-1] = parsed_time;
                            timerGridView.sob = sob;
                            trackInfo.sob = sob;
                            timerGridView.fillRowWithRun(2, timerGridView.sob);
                        }
                    } else {
                        allFilled = false;
                    }
                } else {
                    allFilled = false;
                }
            }

            // Update the value of the Sum column in the current row
            if (allFilled) {
                timerGridView[TimerGridView.SUM_COLUMN_INDEX, rowIndex].Value = TimeParser.GetTimeString(sum);
                timerGridView.updateSb(lapTimes);

                if (sob == null){
                    timerGridView.sob = new Run(lapTimes);
                    trackInfo.sob = new Run(lapTimes);
                    timerGridView.fillRowWithRun(2, timerGridView.sob);
                } 
            } else {
                this.timerGridView[TimerGridView.SUM_COLUMN_INDEX, rowIndex].Value = "-";
            }
        }

        private void savePbClick(object sender, EventArgs e)
        {
            if (timerGridView.sbRun != null && (trackInfo.pb == null || trackInfo.pb.GetTotalTime() >= timerGridView.sbRun.GetTotalTime())) {
                timerGridView.pbRun = timerGridView.sbRun;
                trackInfo.pb = timerGridView.pbRun;
                this.timerGridView.fillRowWithRun(0, timerGridView.pbRun);
            }
            trackInfo.StoreInformation(timerGridView.runs);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            // Show the context menu when the user right-clicks on the form
            if (e.Button == MouseButtons.Right)
            {
                popUpMenu.Show(this, e.Location);
            }
        }

        private void FormClosedAction(object sender, FormClosedEventArgs e)
        {
            JObject startupRun = new JObject {
                { "track", trackInfo.track.ToString() },
                { "mode", trackInfo.mode.ToString() }
            };
            if (File.Exists(startFile)) File.WriteAllText(startFile, startupRun.ToString());
            else Console.WriteLine("Couldn't find start.json");
        }

        private void KeyDownAction(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S) savePbClick(sender, e);
            else if (e.Control && e.KeyCode == Keys.T)
            {
                if (runCountPanel.timer.Enabled) runCountPanel.timer.Stop();
                else runCountPanel.timer.Start();
            } 
        }
    }
}
