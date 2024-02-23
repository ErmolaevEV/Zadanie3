using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zadanie3
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Hide();

            Form1 authForm = new Form1();
            authForm.ShowDialog();

            if (!authClass.InAuthenticated)
            {
                Application.Exit();
            }

            toolStrip1.Text = $"{authClass.auth_fio}! {authClass.auth_id.ToString()}";

            switch (authClass.auth_role)
            {
                case 3:
                    button1.Enabled = true;
                    button2.Enabled = true;
                    break;
                case 2:
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    break;
                case 1:
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button4.Enabled = true;
                    break;
                default:
                    MessageBox.Show("Извините, вас не существует, нам жаль");
                    Application.Exit();
                    break;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 component_UserMgmt = new Form3();
            component_UserMgmt.ShowDialog();
        }
    }
}
