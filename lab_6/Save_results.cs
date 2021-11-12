using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab_6
{
    public partial class Save_results : Form
    {
        private Form1 form1;
        public Save_results(Form1 f1) 
        {
            InitializeComponent();
            form1 = f1;
        }

        private void save_btn_click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter("../../highscore.txt", true);
            sw.WriteLine(textBox1.Text + " " +form1.Cps);
            sw.Close();
            Close();
        }

    }
}
