using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace MKTimer;

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

        // Consists of 4 lists for each segment with the goal times
        List<MKTime>[] goals = {[], [], [], []};
        
        goals[0].Add(new MKTime("22,8xx"));
        goals[1].Add(new MKTime("21,6xx"));
        goals[2].Add(new MKTime("21,6xx"));
        goals[3].Add(new MKTime("1:05,xxx"));

        CreateSegment(goals, trackInfo.Runs);
    }

    #endregion
}