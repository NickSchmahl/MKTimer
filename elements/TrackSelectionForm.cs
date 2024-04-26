using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace MKTimer {
    public class TrackSelectionForm : Form {
        private ComboBox comboBoxTrack;
        private ComboBox comboBoxMode;
        private Button submitButton;
        private Form1 mainForm;

        public TrackSelectionForm(Form1 mainForm) {
            this.mainForm = mainForm;

            // Initialize the ComboBox
            List<ComboBoxItem<MK8DLXTrack>> trackItems = [];
            foreach (MK8DLXTrack value in Enum.GetValues(typeof(MK8DLXTrack)))
            {
                trackItems.Add(new ComboBoxItem<MK8DLXTrack>(value, value.GetTrackName()));
            }

            List<ComboBoxItem<MK8DLXMode>> modeItems = [];
            foreach (MK8DLXMode value in Enum.GetValues(typeof(MK8DLXMode)))
            {
                modeItems.Add(new ComboBoxItem<MK8DLXMode>(value, value.getMode()));
            }

            comboBoxTrack = new ComboBox
            {
                Location = new System.Drawing.Point(50, 50),
                Size = new System.Drawing.Size(500, 50),
                DataSource = trackItems,
                DropDownStyle = ComboBoxStyle.DropDownList, // Set dropdown style to prevent user input
                DisplayMember = "DisplayValue"
            };
            Controls.Add(comboBoxTrack);

            comboBoxMode = new ComboBox
            {
                Location = new System.Drawing.Point(50, 100),
                Size = new System.Drawing.Size(200, 50),
                DataSource = modeItems,
                DropDownStyle = ComboBoxStyle.DropDownList, // Set dropdown style to prevent user input
                DisplayMember = "DisplayValue"
            };
            Controls.Add(comboBoxMode);


            // Initialize the Submit Button
            submitButton = new Button
            {
                Text = "Submit",
                Location = new System.Drawing.Point(50, 150),
                Size = new System.Drawing.Size(200, 50)
            };
            submitButton.Click += SubmitButton_Click; // Attach click event handler
            Controls.Add(submitButton);

            AutoSize = true;
        }

        private void SubmitButton_Click(object? sender, EventArgs e)
        {
            // Handle submit button click event
            if (comboBoxTrack.SelectedItem?.GetType() == typeof(ComboBoxItem<MK8DLXTrack>) && 
                    comboBoxMode.SelectedItem?.GetType() == typeof(ComboBoxItem<MK8DLXMode>)) 
            {
                MK8DLXTrack selectedTrack = ((ComboBoxItem<MK8DLXTrack>) comboBoxTrack.SelectedItem).Value;
                MK8DLXMode selectedMode = ((ComboBoxItem<MK8DLXMode>) comboBoxMode.SelectedItem).Value;

                mainForm.UpdateSelection(selectedTrack, selectedMode);

                Close();
            }
        }

        public class ComboBoxItem<T>
        {
            public T Value { get; }
            public string DisplayValue { get; }

            public ComboBoxItem(T value, string displayValue)
            {
                Value = value;
                DisplayValue = displayValue;
            }
        }
    }
}