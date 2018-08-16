using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Panda
{
    public partial class Formreg : Form
    {
        SqlConnection con;
        public Formreg()
        {
            string strcon = "Data Source=.\\SQLEXPRESS;AttachDbFilename="+"D:\\DYNAMIC AUDIT V3-1\\AUDIT.MDF;Integrated Security=True;Connect Timeout=30;User Instance=True";
            con = new SqlConnection(strcon);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                if (tuname.Text == "")
                {
                    MessageBox.Show("please enter username");
                    return;
                }
                else if (tcontact.Text.Length != 10 )
                {
                    MessageBox.Show("please enter vallid contact");
                    return;
                }
                else if (temail.Text == "")
                {
                    MessageBox.Show("please enter email-id");
                    return;
                }
                else if (tpass.Text == "")
                {
                    MessageBox.Show("please enter password");
                    return;
                }

                else if (textBox5.Text != tpass.Text || textBox5.Text == "")
                {
                    MessageBox.Show("please confirm password");
                    return;
                }
               // MessageBox.Show("open");
                string sql = "insert into registration values('" + tuname.Text + "'," + tcontact.Text + ",'" + temail.Text + "','" + tpass.Text + "')";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Data inserted successfully");
            }
            catch
            {
                MessageBox.Show("Data not inserted");
            }
            con.Close();
        }

        private void Formreg_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmLogin f = new frmLogin();
            f.Show();
            this.Hide();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
