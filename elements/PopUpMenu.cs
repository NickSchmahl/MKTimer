using System;
using System.Windows.Forms;
using MKTimer.elements;

namespace MKTimer
{
    public sealed class PopUpMenu : ContextMenuStrip
    {
        private ToolStripMenuItem trackSelection = new("Track Selection");
        private ToolStripMenuItem _paceChoose = new("Pace Choose");
        private Form1 mainForm;

        public PopUpMenu(Form1 mainForm)
        {
            this.mainForm = mainForm;
            
            trackSelection.Click += MenuItem_Click;
            _paceChoose.Click += MenuItem_Click;
            Items.Add(trackSelection);
            Items.Add(_paceChoose);
        }

        private void MenuItem_Click(object? sender, EventArgs e)
        {
            // Handle the click event of the menu items
            if (sender == null) MessageBox.Show("Click");
            else
            {
                if (sender == trackSelection)
                {
                    var newForm = new TrackSelectionForm(mainForm);
                    newForm.AutoSize = true;
                    newForm.ShowDialog();
                }
                else if (sender == _paceChoose)
                {
                    var newForm = new PaceChooseForm(mainForm);
                    newForm.AutoSize = true;
                    newForm.ShowDialog();
                }
            }
        }
    }
}