using FluentFTP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ftptest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FtpClient client = new FtpClient(textBox1.Text);
            client.Credentials = new System.Net.NetworkCredential("root", "888888");
            try
            {
                client.Connect();
                richTextBox1.AppendText("正在连接FTP服务器\n");
            }
            catch(Exception er1)
            {
                richTextBox1.AppendText("连接FTP服务器失败"+er1.ToString()+"\n");
            }
            try
            {
                client.DownloadFile(@"D:\new\ts.db", "/hmi/database/SampleDataStore/SampleDataStore.db");
                richTextBox1.AppendText("正在下载数据文件\n");
            }
            catch(Exception er2)
            {
                richTextBox1.AppendText("下载数据文件失败" + er2.ToString() + "\n");
            }
            client.Disconnect();
            richTextBox1.AppendText("已断开与FTP服务器的连接\n");
        }
    }
}
