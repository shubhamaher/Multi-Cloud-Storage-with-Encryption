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
    public partial class frmLogin : Form
    {
        SqlConnection con;
        public static string uname;
        public static int log_flg = 0;
        public frmLogin()
        {
            string strcon = "Data Source=.\\SQLEXPRESS;AttachDbFilename=" +"D:\\DYNAMIC AUDIT V3-1\\AUDIT.MDF;Integrated Security=True;Connect Timeout=30;User Instance=True";
            con = new SqlConnection(strcon);
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                int flag = 0;
                string q = "select * from registration";
                SqlDataAdapter cmd = new SqlDataAdapter(q, con);
                DataSet ds = new DataSet();
                cmd.Fill(ds);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    uname=ds.Tables[0].Rows[i][0].ToString();
                    String pass=ds.Tables[0].Rows[i][3].ToString();
                    if (uname.Equals(txtuser.Text))
                    {
                        
                        if (pass.Equals(txtpwd.Text))
                        {
                            Form1.username = uname;
                            operations.username = uname;
                            operations o = new operations();
                            o.Show();
                               this.Hide();
                           // this.Close();
                           // log_flg = 2;
                            return;
                        }
                        else
                        {
                            MessageBox.Show("invalid password");
                            break;
                        }
                    }
                    else
                    {
                        flag = 1;

                    }
                }
                if (flag == 1)
                {
                    MessageBox.Show("invalid username");
                }
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("error");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //log_flg = 1;
            Formreg r = new Formreg();
            r.Show();
            this.Hide();
           
            
         
        }
       
    }
}
