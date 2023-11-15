using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace telegramAPI
{
    public partial class Form1 : Form
    {        

        public Form1()
        {
            InitializeComponent();
        }        

        class model
        {
            public model(int idx, string senderid, string destid, string title, string msg)
            {
                this.idx = idx;
                this.senderid = senderid;
                this.destid = destid;
                this.title = title;
                Msg = msg;
            }

            public string destid { get; }
            public string title { get; }
            public string Msg { get; }
            public int idx { get; }
            public string senderid { get; }
        }

        connection con;
        List<model> modelList;

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                modelList = new List<model>();
                var sql = "SELECT idx,senderid,destid,title,msg FROM omms.msg_interface_history_telegram WHERE issend = 0";
                var dT = con.DataSelect(sql);
                var dR = dT.Rows;
                foreach (DataRow r in dR)
                {

                    var idx = Convert.ToInt16(r["idx"]?.ToString());
                    var title = r["title"]?.ToString();
                    var msg = r["msg"]?.ToString();
                    var senderid = r["senderid"]?.ToString();
                    var destid = r["destid"]?.ToString();                    

                    modelList.Add(new model(idx, senderid, destid, title, msg));
                }

                await Task.Factory.StartNew(() => sendMessage());

            }
            catch (Exception ex)
            {
            }            
        }

        private async void sendMessage()
        {             
            foreach (model msg in modelList)            
            {
                var bot = new telegramBot(txtToken.Text);
                var id = bot.getUpdates();
                bot.SendMessage(msg.title + "\n" + msg.Msg);
                var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                var sql = $"UPDATE omms.msg_interface_history_telegram SET send_time = '{time}' , issend = 1 " +
                         $"WHERE idx = {msg.idx}";
                await Task.Factory.StartNew(() => con.DataExeCute(sql));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            con = new connection();
            con.DbConnection();
        }
    }
}
