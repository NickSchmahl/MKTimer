using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MKTimer.elements {
    public class TrackSelectionForm : Form {
        private readonly ComboBox _comboBoxTrack;
        private readonly ComboBox _comboBoxMode;
        private readonly Button _submitButton;
        private readonly Form1 _mainForm;

        public TrackSelectionForm(Form1 mainForm) {
            _mainForm = mainForm;

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

            _comboBoxTrack = new ComboBox
            {
                Location = new Point(50, 50),
                Size = new Size(500, 50),
                DataSource = trackItems,
                DropDownStyle = ComboBoxStyle.DropDownList, // Set dropdown style to prevent user input
                DisplayMember = "DisplayValue"
            };
            Controls.Add(_comboBoxTrack);

            _comboBoxMode = new ComboBox
            {
                Location = new Point(50, 100),
                Size = new Size(200, 50),
                DataSource = modeItems,
                DropDownStyle = ComboBoxStyle.DropDownList, // Set dropdown style to prevent user input
                DisplayMember = "DisplayValue"
            };
            Controls.Add(_comboBoxMode);


            // Initialize the Submit Button
            _submitButton = new Button
            {
                Text = "Submit",
                Location = new Point(50, 150),
                Size = new Size(200, 50)
            };
            _submitButton.Click += SubmitButton_Click; // Attach click event handler
            Controls.Add(_submitButton);
        }

        private void SubmitButton_Click(object? sender, EventArgs e)
        {
            // Handle submit button click event
            if (_comboBoxTrack.SelectedItem?.GetType() == typeof(ComboBoxItem<MK8DLXTrack>) && 
                    _comboBoxMode.SelectedItem?.GetType() == typeof(ComboBoxItem<MK8DLXMode>)) 
            {
                var selectedTrack = ((ComboBoxItem<MK8DLXTrack>) _comboBoxTrack.SelectedItem).Value;
                var selectedMode = ((ComboBoxItem<MK8DLXMode>) _comboBoxMode.SelectedItem).Value;

                _mainForm.UpdateSelection(selectedTrack, selectedMode);

                Close();
            }
        }

        private class ComboBoxItem<T>(T value, string displayValue)
        {
            public T Value { get; } = value;
            // ReSharper disable once UnusedMember.Local
            // Is used in the ComboBox.DisplayMember property
            public string DisplayValue { get; } = displayValue;
        }
    }
}