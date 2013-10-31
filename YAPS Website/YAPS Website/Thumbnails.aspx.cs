using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;

namespace YAPS_Website
{
    public partial class Wedding : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {     
        }

        public static int CompareStrings(string s1, string s2)
        {
            string v1 = Path.GetFileName(s1).Remove(Path.GetFileName(s1).Length - 4);
            string v2 = Path.GetFileName(s2).Remove(Path.GetFileName(s2).Length - 4);
            try
            {
                int a = int.Parse(v1);
                int b = int.Parse(v2);
                if (a > b)
                    return 1;
                else if (a < b)
                    return -1;
                else return 0;
            }
            catch(Exception ex) {
                return 0;
            
            }
            
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            string address = (Request.QueryString["id"]);
          
            List<Card> cards = new List<Card>();
            string[] imagesdirectory = Directory.GetFiles(Server.MapPath(address), "*.jpg");
            string[] contentdirectory = Directory.GetFiles(Server.MapPath(address), "*.txt");

            Array.Sort(imagesdirectory, CompareStrings);
            Array.Sort(contentdirectory, CompareStrings);

            for (int i = 0; i < (imagesdirectory.Length); i++)
            {
                if (contentdirectory.Length==imagesdirectory.Length)
                {
                    cards.Add(new Card(address + "/" + Path.GetFileName(imagesdirectory[i]) + "?lightbox[width]=600&lightbox[height]=400", File.ReadAllText(contentdirectory[i])));
                }
            }

            ListView1.DataSource = cards;
            ListView1.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {

                if (mobileno.Text != String.Empty && messagebody.Text != String.Empty)
                {
                    SendMessage();

                    mobileno.Text = String.Empty;
                    messagebody.Text = String.Empty;
                    smsstatus.Visible = true;
                    smsstatus.Text = "Message Sent";
                }
                else
                {
                    smsstatus.Visible = true;
                    smsstatus.Text = "Not Sented";
                    mobileno.Text = String.Empty;
                    messagebody.Text = String.Empty;
                }
            }
            catch (Exception)
            {
                
            }
        }

        private void SendMessage()
        {
            string _msg = messagebody.Text;
            string ozSURL = "http://127.0.0.1"; //where Ozeki NG SMS Gateway is running
            string ozSPort = "9501"; //port number where Ozeki NG SMS Gateway is listening
            string ozUser = HttpUtility.UrlEncode("admin"); //username for successful login
            string ozPassw = HttpUtility.UrlEncode("nikki"); //user’s password
            string ozMessageType = "SMS:TEXT"; //type of message
            string ozRecipients = HttpUtility.UrlEncode("+91" + mobileno.Text); //who will get the message
            string ozMessageData = HttpUtility.UrlEncode(_msg); //body of message
            string createdURL = ozSURL + ":" + ozSPort + "/httpapi" +
            "?action=sendMessage" +
            "&username=" + ozUser +
            "&password=" + ozPassw +
            "&messageType=" + ozMessageType +
            "&recipient=" + ozRecipients +
            "&messageData=" + ozMessageData;

            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(createdURL);
            HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();
            System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
            string responseString = respStreamReader.ReadToEnd();
            respStreamReader.Close();
            myResp.Close();
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            try
            {

                if (mobileno.Text != String.Empty && messagebody.Text != String.Empty)
                {
                    SendMessage();

                    mobileno.Text = String.Empty;
                    messagebody.Text = String.Empty;
                    smsstatus.Visible = true;
                    smsstatus.Text = "Message Sent";
                }
                else
                {
                    smsstatus.Visible = true;
                    smsstatus.Text = "Not Sented";
                    mobileno.Text = String.Empty;
                    messagebody.Text = String.Empty;
                }
            }
            catch (Exception)
            {

            }
        }
    }    
}