using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace telegramAPI
{
    public class telegramBot
    {
        public string token = "";
        public string chat_id = "";

        public telegramBot(string token)
        {
            this.token = token;
            this.chat_id = chat_id;
            //"요청이 중단되었습니다. SSL/TLS 보안 채널을 만들 수 없습니다." 오류 방지
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
        }

        // 2. 채팅 ID 받아오기 /////////////////////////////////////////////////////////////
        public string getUpdates()
        {
            chat_id = "";

            string url = string.Format(@"https://api.telegram.org/bot{0}/getUpdates", token);
            WebClient weblient = new WebClient();

            try
            {
                string result = weblient.DownloadString(url);


                if (result.IndexOf("\"ok\":true") > 0)
                {
                    var JsonReult = JObject.Parse(result);         // 마지막 메시지에서 채팅아이디를 받음
                    chat_id = JsonReult["result"].Last["message"]["chat"]["id"].ToString();
                    return chat_id;
                }
                else
                {
                    MessageBox.Show(result);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            return "";
        }

        // 3. 메시지 전송 ///////////////////////////////////////////////////////////////////////
        public bool SendMessage(string text)
        {
            string url = string.Format(@"https://api.telegram.org/bot{0}/sendMessage?chat_id={1}&text={2}", token, chat_id, text);
            Console.WriteLine(url);
            WebClient weblient = new WebClient();

            try
            {
                string result = weblient.DownloadString(url);

                if (result.IndexOf("\"ok\":true") > 0)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show(result);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            return false;
        }
    }
}
