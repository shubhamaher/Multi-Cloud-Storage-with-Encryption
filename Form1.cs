using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Net;
using System.Web;
using System.IO;
using DropNet;
using DropNet.Exceptions;
using RestSharp;
using DropNet.Models;
namespace Panda
{
    public partial class Form1 : Form
    {
        SqlConnection con;
        public static int log_flg = 0;
        int bufferSize = 1024, bufferDivide = 100;
        string encfile;
        public static string username;
        public static string fname;
        OpenFileDialog diag;
        FileStream stream;
        MD5CryptoServiceProvider serviceMD5;
        SHA1CryptoServiceProvider serviceSHA1;
        DropNetClient _client = new DropNetClient("biiqqvfg85c7xej", "4sessrgrtccoxnc", "mteIhoZWcVAAAAAAAAAACGYLFlxTAk-tW2wsJp2fKfg7obSJGPehHLqYkLmLOxKm");
       
        public Form1()
        {
            string strcon = "Data Source=.\\SQLEXPRESS;AttachDbFilename="+ "D:\\DYNAMIC AUDIT V3-1\\AUDIT.MDF;Integrated Security=True;Connect Timeout=30;User Instance=True";
            con = new SqlConnection(strcon);
            
            serviceMD5 = new MD5CryptoServiceProvider();
            serviceSHA1 = new SHA1CryptoServiceProvider();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            label2.Text = ofd.FileName;
            string s1 = label2.Text.Substring(label2.Text.LastIndexOf("\\"));
            encfile = s1;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            if (label2.Text == "-------")
            {
                MessageBox.Show("Please select any file");
                return;
            }
            EncryptFile(label2.Text);

            Cursor.Current = Cursors.Default;
            MessageBox.Show("file encrypted successfully");

        }

        public void EncryptFile(string filename)
        {
            string file = filename;
            string password = "abcd";

            byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES.AES_Encrypt(bytesToBeEncrypted,passwordBytes);
            string s1 = filename.Substring(filename.LastIndexOf("\\"));
            encfile = s1;
            string fileEncrypted = Application.StartupPath + "\\enc" + s1;
            fname = fileEncrypted;
           
            File.WriteAllBytes(fileEncrypted, bytesEncrypted);
        }
        public void DecryptFile(string filename)
        {
            string fileEncrypted = filename;
            string password = txtkey.Text;

            byte[] bytesToBeDecrypted = File.ReadAllBytes(fileEncrypted);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES.AES_Decrypt(bytesToBeDecrypted, passwordBytes);
            string s1 = filename.Substring(filename.LastIndexOf("\\"));

            string file = Application.StartupPath + "\\dec" + s1;
            File.WriteAllBytes(file, bytesDecrypted);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label3.Text = "welcome " + username;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            if (label2.Text == "-------")
            {
                MessageBox.Show("Please select any file");
                return;
            }
            string s1 = label2.Text.Substring(label2.Text.LastIndexOf("\\"));
            encfile = s1;
              BackgroundWorker getFileSHA1 = new BackgroundWorker();
              getFileSHA1.DoWork += new DoWorkEventHandler(SHA1_DoWork);
              getFileSHA1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SHA1_Completed);
              getFileSHA1.RunWorkerAsync();

              Cursor.Current = Cursors.WaitCursor;
        }



        public void SHA1_DoWork(object sender, DoWorkEventArgs e)
        {
            fname = label2.Text;
            stream = new FileStream(fname, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
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
            stream.Close();

            textBox1.Text = ((string)e.Result).ToLowerInvariant();
           
          // System.Windows.Forms.Clipboard.SetText(textBox6.Text);
            MessageBox.Show("Digital signature generated");
        }
        public void uploadEnconcloud(string filename)
        {
            byte[] fileContents;
            byte[] key = ASCIIEncoding.ASCII.GetBytes("TestZone");
            byte[] iv = ASCIIEncoding.ASCII.GetBytes("TestZone");

            using (FileStream inputeFile = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                using (MemoryStream encryptedStream = new MemoryStream())
                {
                    using (CryptoStream cryptostream = new CryptoStream(encryptedStream, new DESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write))
                    {
                        byte[] bytearrayinput = new byte[inputeFile.Length];
                        inputeFile.Read(bytearrayinput, 0, bytearrayinput.Length);
                        cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
                        fileContents = encryptedStream.ToArray();
                    }
                }
            }

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ftp.drivehq.com/" + Path.GetFileName(filename));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new  NetworkCredential("cloudaudit", "cloudaudit");
            request.ContentLength = fileContents.Length;

            Stream requestStream = request.GetRequestStream();
            requestStream.Write(fileContents, 0, fileContents.Length);
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();
        }
        public void uploadOnCloud()
        {
            Cursor.Current = Cursors.WaitCursor;

            _client = new DropNetClient("biiqqvfg85c7xej", "4sessrgrtccoxnc", "mteIhoZWcVAAAAAAAAAACGYLFlxTAk-tW2wsJp2fKfg7obSJGPehHLqYkLmLOxKm");
            _client.UserLogin = new UserLogin { Token = "mteIhoZWcVAAAAAAAAAACGYLFlxTAk-tW2wsJp2fKfg7obSJGPehHLqYkLmLOxKm", Secret = "4sessrgrtccoxnc" };
            int idx2 = fname.LastIndexOf("\\") + 1;
          
            string fnamepart1 = fname.Substring(idx2) + "_part1";
            string fnamepart2 = fname.Substring(idx2) + "_part2";

            for (int i = 0; i < 2; i++)
            {
                string filename="";
                if(i==0)
                    filename = Application.StartupPath+"\\block\\"+fnamepart1;
                else
                    filename = Application.StartupPath + "\\block\\" + fnamepart2;
                          
                var x = @"/" + Path.GetFileName(filename);

                MetaData mm = _client.UploadFile("/", Path.GetFileName(filename), File.ReadAllBytes(@"" + filename));

            }
            Cursor.Current = Cursors.Default;

            MessageBox.Show("file uploaded successfully");
            
        }
        private void button4_Click(object sender, EventArgs e)
        {
             try
            {

                Cursor.Current = Cursors.WaitCursor;
                uploadOnCloud();
               // uploadEnconcloud(label2.Text);
                con.Open();
              
                 string sql ="insert into Upload_data values('"+username+"','"+encfile+"','"+textBox1.Text+"')";
                 SqlCommand cmd = new SqlCommand(sql, con);
                 cmd.ExecuteNonQuery();
                 MessageBox.Show("File uploaded successfully");

                 Cursor.Current = Cursors.Default;

            }
             catch(Exception ex)
             {
                 MessageBox.Show("Data not inserted");
             }
             con.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        public void split( String fpath)
        {

            int idx2 = fpath.LastIndexOf("\\") + 1;
            string fnamepart1 = fpath.Substring(idx2) + "_part1";
            string fnamepart2 = fpath.Substring(idx2) + "_part2";


            byte[] fb = File.ReadAllBytes(fpath);
            int t1 = fb.Length / 2;
            int t2 = fb.Length - t1;
            byte[] part1 = new byte[t1];
            byte[] part2 = new byte[t2];

            for (int i = 0; i < t1; i++)
            {
                part1[i] = fb[i];
            }

            for (int i = 0; i < t2; i++)
            {
                part2[i] = fb[i + t1];
            }
            File.WriteAllBytes(Application.StartupPath+"\\block\\" + fnamepart1, part1);
            File.WriteAllBytes(Application.StartupPath + "\\block\\" + fnamepart2, part2);

           
        }
        private void button5_Click_1(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {

                string fname2 = Application.StartupPath + "\\enc" + encfile;
                split(fname2);
                MessageBox.Show("file splited");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:"+ex);
            }
            Cursor.Current = Cursors.Default;
        
        
        }


    }
}