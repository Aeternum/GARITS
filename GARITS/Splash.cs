using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GARITS
{
    public partial class Splash : Form
    {
        private string[] loadingStrings;
        private int currentLabel = 0;
        private int timerLoops = 0;

        public Splash()
        {
            InitializeComponent();
            loadingStrings = new String[11] {"Finding the Higgs Boson...","Exceeding CPU Quota...", "Self Affirming...", "Tokenizing Innovation...", "Dissolving Relationships...", "Measuring Cosmic Microwave Background...", "Processing...", "Dissasembling World...", "Searching for credit card numbers...", "Compensating for bugs...","Adjusting for Doppler Shift..." };
            label1.Text = loadingStrings[0];
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timerLoops++;
            if (progressBar1.Value % 5 == 0)
            {
                
                label1.Text = loadingStrings[currentLabel];
                currentLabel++;
            }
            Application.DoEvents();
            if (progressBar1.Value >= 100)
            {
                
                timer1.Stop();
                timer2.Start(); //Hacked to make progres bar animation finish at the end
            }
            else
            {
                progressBar1.Value += 2;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
