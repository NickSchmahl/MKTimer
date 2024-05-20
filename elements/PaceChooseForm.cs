using System.Drawing;
using System.Windows.Forms;
using MKTimer.gameLogic;

namespace MKTimer.elements;

public partial class PaceChooseForm : Form
{
    private ComboBox _comboBoxTrack;
    private ComboBox _comboBoxMode;
    private readonly TextBox[,] _textBoxes = new TextBox[4, 4];
    
    public PaceChooseForm(TrackInfo trackInfo)
    {
        _comboBoxTrack = new();
        _comboBoxMode = new();
        InitializeComponent(trackInfo);
    }

    private void InitializeTextBoxes()
    {
        // Initialize the TextBoxes for paces
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                _textBoxes[i, j] = new TextBox
                {
                    Location = new Point(50 + j * 220, 160 + i * 60), // Adjust these values as needed
                    Size = new Size(200, 50),
                };

                // Add the TextBox to the form's controls
                Controls.Add(_textBoxes[i, j]);
            }
        }
    }

    private void InitializeSegmentLabels()
    {
        // Initialize the Labels for segments
        for (int j = 0; j < 4; j++)
        {
            Label segmentLabel = new Label
            {
                Location = new Point(50 + j * 220, 100), // Adjust these values as needed
                Size = new Size(200, 50),
                Text = $"Segment {j + 1}"
            };

            // Add the Label to the form's controls
            Controls.Add(segmentLabel);
        }
    }
}