using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;
using MKTimer.gameLogic;
using Newtonsoft.Json.Linq;

namespace MKTimer.elements;

partial class PaceChooseForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent(TrackInfo trackInfo)
    {
        // Initialize the component container
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "PaceChoose";
        
        // Initialize the list of ComboBox items for tracks
        List<ComboBoxItem<MK8DLXTrack>> trackItems = new List<ComboBoxItem<MK8DLXTrack>>();
        foreach (MK8DLXTrack value in Enum.GetValues(typeof(MK8DLXTrack)))
        {
            trackItems.Add(new ComboBoxItem<MK8DLXTrack>(value, value.GetTrackName()));
        }

        // Initialize the list of ComboBox items for modes
        List<ComboBoxItem<MK8DLXMode>> modeItems = new List<ComboBoxItem<MK8DLXMode>>();
        foreach (MK8DLXMode value in Enum.GetValues(typeof(MK8DLXMode)))
        {
            modeItems.Add(new ComboBoxItem<MK8DLXMode>(value, value.getMode()));
        }
        
        // Initialize the ComboBox for track selection
        _comboBoxTrack = new ComboBox
        {
            Location = new Point(50, 50),
            Size = new Size(500, 50),
            DataSource = trackItems,
            DropDownStyle = ComboBoxStyle.DropDownList,
            DisplayMember = "DisplayValue"
        };

        // Initialize the ComboBox for mode selection
        _comboBoxMode = new ComboBox
        {
            Location = new Point(550, 50), // Adjust the X coordinate to place this ComboBox next to the first one
            Size = new Size(200, 50),
            DataSource = modeItems,
            DropDownStyle = ComboBoxStyle.DropDownList,
            DisplayMember = "DisplayValue"
        };
        
        InitializeSegmentLabels();
        InitializeTextBoxes();
        
        // Initialize the submit button
        Button submitButton = new Button
        {
            Location = new Point(50, 400), // Adjust these values as needed
            Size = new Size(100, 50),
            Text = "Submit"
        };
        

        // Add items to the Controls collection
        Controls.Add(_comboBoxTrack);
        Controls.Add(_comboBoxMode);
        Controls.Add(submitButton);
        
        // Attach the SelectedIndexChanged event handlers
        _comboBoxTrack.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
        _comboBoxMode.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
        // Add the click event handler
        submitButton.Click += SubmitButton_Click;
    }
    
    private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Get the selected track and mode
        var selectedTrack = ((ComboBoxItem<MK8DLXTrack>)_comboBoxTrack.SelectedItem).Value;
        var selectedMode = ((ComboBoxItem<MK8DLXMode>)_comboBoxMode.SelectedItem).Value;

        var paces = new TrackInfo(selectedTrack, selectedMode).Paces;
        // Update the TextBoxes
        if (paces == null) return;
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < paces[i].Count; j++)
            {
                _textBoxes[j,i].Text = paces[i][j].ToString();
            }
        }
    }
    
    private void SubmitButton_Click(object sender, EventArgs e)
    {
        // Initialize a parent JArray
        JArray parentArray = new JArray();

        for (int i = 0; i < 4; i++)
        {
            JArray childArray = new JArray();
            for (int j = 0; j < 4; j++)
            {
                childArray.Add(_textBoxes[j, i].Text);
            }

            parentArray.Add(childArray);
        }

        // Get the selected track and mode
        var selectedTrack = ((ComboBoxItem<MK8DLXTrack>)_comboBoxTrack.SelectedItem).Value.ToString();
        var selectedMode = ((ComboBoxItem<MK8DLXMode>)_comboBoxMode.SelectedItem).Value.ToString();

        // Read the existing JSON data
        var jsonData = File.ReadAllText(TrackInfo.JsonPath);
        var jsonObject = JObject.Parse(jsonData);

        // Check if the JObject already contains an entry for the selected track
        if (jsonObject.ContainsKey(selectedTrack))
        {
            // Check if the track entry contains an entry for the selected mode
            if (jsonObject[selectedTrack][selectedMode] != null)
            {
                // Replace the paces attribute with the new JArray
                jsonObject[selectedTrack][selectedMode]["paces"] = parentArray;
            }
            else
            {
                // Add a new entry for the selected mode with the paces attribute
                jsonObject[selectedTrack][selectedMode] = new JObject { { "paces", parentArray } };
            }
        }
        else
        {
            // Add a new entry for the selected track with the selected mode and the paces attribute
            jsonObject[selectedTrack] = new JObject { { selectedMode, new JObject { { "paces", parentArray } } } };
        }

        // Write the updated JObject back to the file
        File.WriteAllText(TrackInfo.JsonPath, jsonObject.ToString());

        Close();
    }
    
    private class ComboBoxItem<T>(T value, string displayValue)
    {
        public T Value { get; } = value;
        // ReSharper disable once UnusedMember.Local
        // Is used in the ComboBox.DisplayMember property
        public string DisplayValue { get; } = displayValue;
    }

    #endregion
}