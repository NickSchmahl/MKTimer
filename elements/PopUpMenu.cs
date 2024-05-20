using System;
using System.Windows.Forms;
using MKTimer.elements;

namespace MKTimer
{
    public class PopUpMenu : ContextMenuStrip
    {
        private ToolStripMenuItem trackSelection = new("Track Selection");
        private Form1 mainForm;

        public PopUpMenu(Form1 mainForm)
        {
            this.mainForm = mainForm;
            ToolStripMenuItem menuItem1 = new ToolStripMenuItem("Menu Item 1");
            menuItem1.Click += MenuItem_Click;
            trackSelection.Click += MenuItem_Click;
            Items.Add(menuItem1);
            Items.Add(trackSelection);
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
                    return;
                }

                ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
                MessageBox.Show("Clicked: " + clickedItem.Text);
            }
        }
    }
}