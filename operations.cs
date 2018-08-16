using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Net;
using System.Web;
using System.IO;
using DropNet;
using DropNet.Exceptions;
using RestSharp;
using DropNet.Models;
namespace Panda
{
    public partial class operations : Form
    {
        SqlConnection con;
        public static string username;
        string fname,f;
        int bufferSize = 1024, bufferDivide = 100;
        string encfile;
        
        //public static string fname;
        OpenFileDialog diag;
        FileStream stream;
        MD5CryptoServiceProvider serviceMD5;
        SHA1CryptoServiceProvider serviceSHA1;
        DropNetClient _client = new DropNetClient("b7oxugbxulgsem6", "b7oxugbxulgsem6", "QX-8FEljL0AAAAAAAAAABjTJmnBAjs3-xPcnL7fD2ss_DZWzLlD6NO7YJNLdkv2v");
     
        public operations()
        {
            string strcon = "Data Source=.\\SQLEXPRESS;AttachDbFilename=" + "d:\\audit.mdf;Integrated Security=True;Connect Timeout=30;User Instance=True";
            con = new SqlConnection(strcon);
            serviceMD5 = new MD5CryptoServiceProvider();
            serviceSHA1 = new SHA1CryptoServiceProvider();
            InitializeComponent();

          
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {

        }
        public void download(string filename1)
        {
            _client = _client = new DropNetClient("b7oxugbxulgsem6", "hczdlsodguxn0jt", "QX-8FEljL0AAAAAAAAAABjTJmnBAjs3-xPcnL7fD2ss_DZWzLlD6NO7YJNLdkv2v");
            _client.UserLogin = new UserLogin { Token = "QX-8FEljL0AAAAAAAAAABjTJmnBAjs3-xPcnL7fD2ss_DZWzLlD6NO7YJNLdkv2v", Secret = "hczdlsodguxn0jt" };

            string filename = filename1;
            String file2="";
                 file2 = filename.Replace("\\", "") + "_part1";

                _client.GetFileAsync("/" + file2,
                    (response) =>
                    {

                        using (FileStream fs = new FileStream(Application.StartupPath+"\\blockd\\" + file2, FileMode.Create))
                        {
                            for (int i = 0; i < response.RawBytes.Length; i++)
                            {
                                fs.WriteByte(response.RawBytes[i]);
                            }
                            fs.Seek(0, SeekOrigin.Begin);
                            for (int i = 0; i < response.RawBytes.Length; i++)
                            {
                                if (response.RawBytes[i] != fs.ReadByte())
                                {
                                    MessageBox.Show("Error writing data for " + filename);
                                    return;
                                }
                            }
                        }
                        Cursor.Current = Cursors.Default;
                      //  MessageBox.Show("file downloaded");
                    },
                    (error) =>
                    {
                       // MessageBox.Show("error downloading");
                    });

                String file3 = filename.Replace("\\", "") + "_part2";

                _client.GetFileAsync("/" + file3,
                    (response) =>
                    {

                        using (FileStream fs = new FileStream(Application.StartupPath + "\\blockd\\" + file3, FileMode.Create))
                        {
                            for (int i = 0; i < response.RawBytes.Length; i++)
                            {
                                fs.WriteByte(response.RawBytes[i]);
                            }
                            fs.Seek(0, SeekOrigin.Begin);
                            for (int i = 0; i < response.RawBytes.Length; i++)
                            {
                                if (response.RawBytes[i] != fs.ReadByte())
                                {
                                    MessageBox.Show("Error writing data for " + filename);
                                    return;
                                }
                            }
                        }
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("file downloaded");

                        merg(filename);
                      
                    },
                    (error) =>
                    {
                        MessageBox.Show("error downloading");
                    });
            
        }
        public void merg(String filenm)
        {
            string fpath = filenm.Replace("\\", "");
            byte[] dpart1 = File.ReadAllBytes(Application.StartupPath+"\\blockd\\" + fpath + "_part1");
            byte[] dpart2 = File.ReadAllBytes(Application.StartupPath + "\\blockd\\" + fpath + "_part2");
            int len = dpart1.Length + dpart2.Length;
            byte[] df = new byte[len];

            for (int i = 0; i < dpart1.Length; i++)
            {
                df[i] = dpart1[i];
            }
            for (int i = 0; i < dpart2.Length; i++)
            {
                df[i + dpart1.Length] = dpart2[i];
            }
            File.WriteAllBytes(Application.StartupPath + "\\merge\\" + fpath, df);

            DecryptFile(Application.StartupPath + "\\merge\\" + fpath);
        }
        private void button1_Click(object sender, EventArgs e)
        {
 }
        public void DecryptFile(string filename)
        {
            try
            {
                string fileEncrypted = filename;

                string password = "abcd";

                byte[] bytesToBeDecrypted = File.ReadAllBytes(filename);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesDecrypted = AES.AES_Decrypt(bytesToBeDecrypted, passwordBytes);
                string s1 = filename.Substring(filename.LastIndexOf("\\"));

                string file = Application.StartupPath + "\\dec" + s1;
                File.WriteAllBytes(file, bytesDecrypted);
            }
            catch { }
        }

        private void operations_Load(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.TopLevel = false;
            f.Dock = DockStyle.Fill;
            f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
           
            tabPage1.Controls.Add(f);
            f.Show();
            /*
            Button b = new Button();
            b.Size = new Size(100, 100);
            b.Location = new Point(100, 100);
            b.BackColor = Color.Black;
            tabPage1.Controls.Add(b);*/

        }

        private void button2_Click(object sender, EventArgs e)
        {
          
        }

        private void listBox2_MouseClick(object sender, MouseEventArgs e)
        {
            
            
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
           /* BackgroundWorker getFileSHA1 = new BackgroundWorker();
            getFileSHA1.DoWork += new DoWorkEventHandler(SHA1_DoWork);
            getFileSHA1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SHA1_Completed);
            getFileSHA1.RunWorkerAsync();*/
        }
        public void SHA1_DoWork(object sender, DoWorkEventArgs e)
        {
          
            stream = new FileStream(f, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
            byte[] buffer = new byte[bufferSize];
            int readCount;
            HashAlgorithm algorithm = SHA1.Create();

            while ((readCount = stream.Read(buffer, 0, bufferSize)) > 0)
            {
                algorithm.TransformBlock(buffer, 0, readCount, buffer, 0);
            }
            algorithm.TransformFinalBlock(buffer, 0, readCount);
            string result = System.BitConverter.ToString(algorithm.Hash).Replace("-", "");
            e.Result = result;
        }

        public void SHA1_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
         }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

     
      
    }
}

