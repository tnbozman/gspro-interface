using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OcrChangeDetection.UI
{
    public partial class LabelInputDialog : Form
    {
        public string EnteredLabel => textBox1.Text;
        public LabelInputDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(label1.Text))
            {
                MessageBox.Show("Please enter a valid label.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.DialogResult = DialogResult.OK;
            // Close the dialog
            this.Close();
        }
    }
}
