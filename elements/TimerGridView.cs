using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MKTimer {
    public class TimerGridView: DataGridView {

        public const int COLUMN_COUNT = 5;
        public const int SUM_COLUMN_INDEX = COLUMN_COUNT - 1;
        public static int RUN_COUNT = 0;
        public Run? pbRun;
        public Run? sbRun;
        public Run? sob;
        private MK8DLXTrack track;
        private MK8DLXMode mode;
        public RunCountPanel? runCountPanel;

        public TimerGridView(TrackInfo trackInfo) {
            track = trackInfo.track;
            mode = trackInfo.mode;
            RUN_COUNT = 0;

            ((System.ComponentModel.ISupportInitialize) this).BeginInit();
            SuspendLayout();

            // DataGridView
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Size = new Size((COLUMN_COUNT-1) * 250 + 150, 500);
            ColumnCount = COLUMN_COUNT;
            RowCount = 4;
            RowHeadersVisible = false;
            MultiSelect = false;

            // Add header
            Columns[0].Name = "Run";
            Columns[0].Width = 100;
            Columns[1].Name = "Round 1";
            Columns[2].Name = "Round 2";
            Columns[3].Name = "Round 3";
            Columns[4].Name = "Sum";

            // Add input fields to every other cell
            for (int i = 0; i < RowCount; i++) {
                for (int j = 0; j < COLUMN_COUNT; j++) {
                    DataGridViewTextBoxCell textBoxCell = new();
                    this[j, i] = textBoxCell;
                    if (j == 0 || j == SUM_COLUMN_INDEX) 
                    {
                        textBoxCell.Style.BackColor = Color.LightGray;
                        this[j, i].ReadOnly = true;
                    }
                }
                this[0, i].Value = i-2;
            }

            // Add PB, SB and SoB Row
            this[0,0].Value = "PB";
            this[0,1].Value = "SB";
            this[0,2].Value = "SoB";
            pbRun = trackInfo.pb;
            if (pbRun != null) fillRowWithRun(0, pbRun);
            sob = trackInfo.sob;
            if (sob != null) fillRowWithRun(2, sob);

            Rows[0].Frozen = true;
            Rows[0].ReadOnly = true;
            Rows[0].DefaultCellStyle.ForeColor = Color.DimGray;
            Rows[1].Frozen = true;
            Rows[1].ReadOnly = true;
            Rows[1].DefaultCellStyle.ForeColor = Color.DimGray;
            Rows[2].Frozen = true;
            Rows[2].ReadOnly = true;
            Rows[2].DefaultCellStyle.ForeColor = Color.DimGray;

            // Default selected cell
            Rows[0].Cells[0].Selected = false;
            Rows[3].Cells[1].Selected = true;

            // Handler for events
            UserAddedRow += UserAddedRowHandler;
        }

        public void fillRowWithRun(int rowIndex, Run run) {
            for (int i = 1; i < SUM_COLUMN_INDEX; i++) {
                if(run.laps != null) 
                {
                    double? lap_time = run.laps[i-1];
                    if (lap_time == null) this[i, rowIndex].Value = "-";
                    else this[i, rowIndex].Value = TimeParser.GetTimeString((double) lap_time);
                }
            }
            double? total_time = run.GetTotalTime();
            if (total_time == null) this[SUM_COLUMN_INDEX, rowIndex].Value = "-";
            else this[SUM_COLUMN_INDEX, rowIndex].Value = TimeParser.GetTimeString((double) total_time);
        }

        public void updateSb(double?[] lapTimes) {
            if (sbRun == null && lapTimes != null) {
                sbRun = new Run(lapTimes);
                fillRowWithRun(1, sbRun);
            } else if (lapTimes != null && sbRun != null && lapTimes.Sum() < sbRun.GetTotalTime()) {
                sbRun = new Run(lapTimes);
                fillRowWithRun(1, sbRun);
            }
        }

        private void UserAddedRowHandler(object? sender, DataGridViewRowEventArgs e)
        {
            RUN_COUNT++;

            e.Row.Cells[0].Value = e.Row.Index - 2;
            e.Row.Cells[0].Style.BackColor = Color.LightGray;
            e.Row.Cells[0].ReadOnly = true;
            e.Row.Cells[SUM_COLUMN_INDEX].Style.BackColor = Color.LightGray;
            e.Row.Cells[SUM_COLUMN_INDEX].ReadOnly = true;

            runCountPanel?.UpdateRowCount();
        }
    }
}