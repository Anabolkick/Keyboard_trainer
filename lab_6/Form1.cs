using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace lab_6
{
    public partial class Form1 : Form
    {
        private Random _rand = new Random();
        private List<RadioButton> _rbList;
        private List<string> _words = new List<string>();
        private int _seconds;
        private int _correctCount;
        private int _wordsToWin;
        private int _correct_chars;

        public double Cps;

        public Form1()
        {
            InitializeComponent();
            _rbList = new List<RadioButton>() { radioButton1, radioButton2, radioButton3, radioButton4, radioButton5 };
            load_words();

            if (File.Exists("../../progress.txt"))
            {
                using (StreamReader sr = new StreamReader("../../progress.txt"))
                {
                    textBox3.Text = sr.ReadToEnd();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _correctCount = 0;
            textBox1.Text = _words[_rand.Next(_words.Count)];
            letter.Text = textBox1.Text[0].ToString();
            var checkedButton = _rbList.FirstOrDefault(r => r.Checked);
            groupBox1.Enabled = false;
            textBox2.Enabled = true;

            switch (checkedButton.Text)
            {
                case "Beginner":
                    timer1.Interval = 4500;
                    _wordsToWin = 5;
                    break;
                case "Easy":
                    timer1.Interval = 2500;
                    _wordsToWin = 15;
                    break;
                case "Medium":
                    timer1.Interval = 1400;
                    _wordsToWin = 30;
                    break;
                case "Hard":
                    timer1.Interval = 900;
                    _wordsToWin = 40;
                    break;
                case "Impossible":
                    timer1.Interval = 450;
                    _wordsToWin = 50;
                    break;

            }
            textBox2.Focus();
            timer1.Start();
            timer2.Start();
            textBox2.Focus();
            button1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            MessageBox.Show("You lose, try again :D", "Lose!");
            textBox2.Enabled = false;
            groupBox1.Enabled = true;
            _seconds = 0;
            _correct_chars = 0;
            button1.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            _seconds++;
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (_words.Count == 0)
            {
                MessageBox.Show("Upload words first!");
                return;
            }

            if (e.KeyData.ToString().ToLower()[0] == textBox1.Text.ToLower()[0])
            {
                textBox1.Text = textBox1.Text.Remove(0, 1);
                _correct_chars++;
                timer1.Stop();
                timer1.Start();

                if (textBox1.Text.Length == 0)
                {
                    textBox1.Text = _words[_rand.Next(_words.Count)];
                    textBox2.Clear();
                    _correctCount++;
                }

                letter.Text = textBox1.Text[0].ToString();

                if (_correctCount == _wordsToWin)
                {
                    timer1.Stop();
                    timer2.Stop();
                    var checkedButton = _rbList.FirstOrDefault(r => r.Checked);
                    if (!textBox3.Text.Contains(checkedButton.Text))
                    {
                        if (textBox3.Text.Length > 0)
                        {
                            textBox3.Text += Environment.NewLine;
                        }
                        textBox3.Text += checkedButton.Text;
                        MessageBox.Show("Congratulations, You complete this level!", "Win!");
                        using (StreamWriter sw = new StreamWriter("../../progress.txt", true))
                        {
                            sw.Write(checkedButton.Text);
                        }
                    }
                    groupBox1.Enabled = true;
                    Save_results saveResForm = new Save_results(this);
                    saveResForm.label1.Text = "Your time: " + _seconds;
                    Cps = Math.Round((double)_correct_chars / _seconds, 3);
                    saveResForm.label2.Text = "Chars per second: " + Cps;
                    saveResForm.Show();

                    _seconds = 0;
                    _correct_chars = 0;
                    button1.Enabled = true;
                }
            }
        }

        private void load_words()
        {
            if (_words.Count == 0)
            {
                string all_words = "";
                using (StreamReader sr = new StreamReader("../../words.txt"))
                {
                    all_words = sr.ReadToEnd();
                }

                var words_arr = all_words.Split(',');

                foreach (var word in words_arr)
                {
                    _words.Add(word);
                }

            }
        }
    }
}
