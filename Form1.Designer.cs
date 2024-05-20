using System.Windows.Forms;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.IO;
using System;
using System.Linq;
using MKTimer.elements;
using MKTimer.gameLogic;

namespace MKTimer
{
    partial class Form1
    {
        private TimerGridView timerGridView;
        private TrackSelectionPanel trackSelectionPanel;
        private RunCountPanel runCountPanel;
        private PopUpMenu popUpMenu;
        public TrackInfo trackInfo { get; set; }
        private PacePanel PacePanel;

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
            // Components
            initializeTimerGridView();
            initializeTrackSelectionPanel();
            initializeRunCountPanel();
            initializePacePanel();
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
            Controls.Remove(PacePanel);
            initializeTimerGridView();
            initializeTrackSelectionPanel();
            initializeRunCountPanel();
            initializePacePanel();
            timerGridView.runCountPanel = runCountPanel;
        }
        
        public void UpdatePacePanel()
        {
            Controls.Remove(PacePanel);
            initializePacePanel();
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

        private void initializePacePanel()
        {
            PacePanel = new PacePanel(trackInfo);
            PacePanel.Location = new Point(100, 600);
            PacePanel.MouseClick += Form1_MouseClick;
            PacePanel.KeyDown += KeyDownAction;
            
            Controls.Add(PacePanel);
        }

        private void initializeRunCountPanel() 
        {
            runCountPanel = new RunCountPanel(trackInfo.RunCount, trackInfo.SecondsPlayed) {
                Location = new Point(0, 50),
                Size = new Size(timerGridView.Size.Width, 50)
            };
            trackSelectionPanel.MouseClick += Form1_MouseClick;
            trackSelectionPanel.KeyDown += KeyDownAction;

            Controls.Add(runCountPanel);
        }

        private void initializeTrackSelectionPanel()
        {
            trackSelectionPanel = new TrackSelectionPanel(trackInfo.Track, trackInfo.Mode);
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
            MkTime[] lapTimes = new MkTime[3];
            bool allFilled = true;
            Run sob = trackInfo.Sob?.Copy();

            // Calculate the sum of the first three columns in the current row
            for (int i = 1; i < TimerGridView.SUM_COLUMN_INDEX; i++)
            {
                if (this.timerGridView[i, rowIndex].Value != null)
                {
                    MkTime parsed_time = new(this.timerGridView[i, rowIndex].Value.ToString());
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
                            trackInfo.Sob = sob;
                            timerGridView.fillRowWithRun(2, timerGridView.sob);
                        }
                    } else {
                        allFilled = false;
                    }
                } else {
                    allFilled = false;
                }
            }
            
            // Update pace panel
            var colIndex = e.ColumnIndex;
            var newTime = new MkTime(timerGridView[colIndex, rowIndex].Value.ToString());
            PacePanel.UpdateAchieved(newTime, colIndex-1);

            // Update the value of the Sum column in the current row
            if (allFilled) {
                timerGridView[TimerGridView.SUM_COLUMN_INDEX, rowIndex].Value = TimeParser.GetTimeString(sum);
                timerGridView.updateSb(lapTimes);

                if (sob == null){
                    timerGridView.sob = new Run(lapTimes);
                    trackInfo.Sob = new Run(lapTimes);
                    timerGridView.fillRowWithRun(2, timerGridView.sob);
                } 
                
                // Update pacel panel
                PacePanel.UpdateAchieved(new MkTime(timerGridView[TimerGridView.SUM_COLUMN_INDEX, rowIndex].Value.ToString()), 3);
            } else {
                this.timerGridView[TimerGridView.SUM_COLUMN_INDEX, rowIndex].Value = "-";
            }
        }

        private void savePbClick(object sender, EventArgs e)
        {
            if (timerGridView.sbRun != null && (trackInfo.Pb == null || trackInfo.Pb.GetTotalTime() >= timerGridView.sbRun.GetTotalTime())) {
                timerGridView.pbRun = timerGridView.sbRun;
                trackInfo.Pb = timerGridView.pbRun;
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
                { "track", trackInfo.Track.ToString() },
                { "mode", trackInfo.Mode.ToString() }
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
