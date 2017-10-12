using FluentFTP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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
            client.Credentials = new System.Net.NetworkCredential("root", "1234567");
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public class glb
        {
            public static bool startcheck;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            //glb.startcheck = true;
            button2.Enabled = false;
            button3.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "server=.;database=qptest;uid=web;pwd=q123wdsa";
            conn.Open();
            SqlCommand comm = new SqlCommand();
            comm.Connection = conn;
            comm.CommandType = CommandType.Text;
            comm.CommandText = "select * from needupdate where status = 0 order by stationid";
            SqlDataReader dr = comm.ExecuteReader();
            while(dr.Read())
            {
                var stationid = Convert.ToInt32(dr["stationid"]);
                if(stationid==1)
                {
                    /***
                    FtpClient client = new FtpClient(textBox1.Text);
                    client.Credentials = new System.Net.NetworkCredential("root", "1234567");
                    try
                    {
                        client.Connect();
                        richTextBox1.AppendText("正在连接FTP服务器\n");
                    }
                    catch (Exception er1)
                    {
                        richTextBox1.AppendText("连接FTP服务器失败" + er1.ToString() + "\n");
                    }
                    try
                    {
                        client.DownloadFile(@"D:\new\ts.db", "/hmi/database/SampleDataStore/SampleDataStore.db");
                        richTextBox1.AppendText("正在下载数据文件\n");
                    }
                    catch (Exception er2)
                    {
                        richTextBox1.AppendText("下载数据文件失败" + er2.ToString() + "\n");
                    }
                    client.Disconnect();
                    richTextBox1.AppendText("已断开与FTP服务器的连接\n");
                    ***/
                    var conn2 = new SQLiteConnection();
                    conn2.ConnectionString = "Data Source=D:\\new\\ts.db";
                    conn2.Open();
                    var comm2 = new SQLiteCommand();
                    comm2.Connection = conn2;
                    comm2.CommandType = CommandType.Text;
                    comm2.CommandText = "SELECT STRFTIME('%Y-%m-%d %H:%m:%S',substr(time,0,20)) as time,ch0,ch1,ch2,ch3 FROM SrcDat WHERE time BETWEEN '2017-10-11 14:26:00' AND '2017-10-11 14:26:10' order by time";
                    var dr2 = comm2.ExecuteReader();
                    while(dr2.Read())
                    {
                        SqlConnection conn3 = new SqlConnection();
                        conn3.ConnectionString = "server=.;database=qptest;uid=web;pwd=q123wdsa";
                        conn3.Open();
                        SqlCommand comm3 = new SqlCommand();
                        comm3.Connection = conn3;
                        comm3.CommandType = CommandType.Text;
                        comm3.CommandText = "insert into datatable values('" + dr2["time"] + "'," + dr2["ch0"].ToString() + "," + dr2["ch1"].ToString() + "," + dr2["ch2"].ToString() + "," + dr2["ch3"].ToString() +  ")";
                        var dr3 = comm3.ExecuteNonQuery();
                        if(dr3 > 0)
                        {
                            richTextBox1.AppendText("数据插入成功");
                            comm3.CommandText = "update needupdate set status = 1 where stationid = " + stationid.ToString() + " and datadate = '" + dr["datadate"].ToString() + "'";
                            var dr4 = comm3.ExecuteNonQuery();
                            if(dr4 > 0)
                            {
                                richTextBox1.AppendText("数据更新成功");
                            }
                        }
                        conn3.Close();
                    }
                    dr2.Close();
                    conn2.Close();
                }
            }
            dr.Close();
            conn.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            //glb.startcheck = false;
            button2.Enabled = true;
            button3.Enabled = false;
        }

        public static DateTime TSTDT(double unixTimeStamp)
        {
            var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
}
