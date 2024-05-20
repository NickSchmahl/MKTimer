using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using MKTimer.gameLogic;
using Newtonsoft.Json.Linq;

namespace MKTimer.elements;

partial class PacePanel
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

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent(TrackInfo trackInfo)
    {
        components = new System.ComponentModel.Container();
        AutoSize = true;

        // Assuming trackInfo is an instance of TrackInfo
        string trackInTrackInfo = trackInfo.Track.ToString();
        string modeInTrackInfo = trackInfo.Mode.ToString();

        // Read the existing JSON data
        var jsonData = File.ReadAllText(TrackInfo.JsonPath);
        var jsonObject = JObject.Parse(jsonData);

        // Navigate to the paces attribute for the selected track and mode
        if (jsonObject.ContainsKey(trackInTrackInfo) && jsonObject[trackInTrackInfo][modeInTrackInfo] != null)
        {
            JArray paces = (JArray)jsonObject[trackInTrackInfo][modeInTrackInfo]["paces"];
            if (paces == null) return;
            
            // Iterate over each array in paces
            var segCounter = 0;
            foreach (JArray paceArray in paces)
            {
                // Iterate over each string in the array
                foreach (string pace in paceArray)
                {
                    // Add the string to _goals
                    if (pace != "00,000" && pace != "") _goals[segCounter].Add(new MkTime(pace), 0);
                }

                segCounter++;
            }

            CreateSegments(trackInfo.Runs);    
        }
    }

    #endregion
}