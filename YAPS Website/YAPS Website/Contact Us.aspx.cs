using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
//using System.Web.Mail;


namespace YAPS_Website
{
    public partial class Contact_Us : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            if (ClientName.Text == "" || ClientEmail.Text == "" || ClientContactNo.Text == "" || ClientComment.Text == "")
            {
                MailStatus.Text = "Please fill all details.";
                ClientName.Text = ClientEmail.Text = ClientContactNo.Text = ClientComment.Text = "";
            }

            else
            {
                MailAddress add;
                try
                {
                    add = new System.Net.Mail.MailAddress(ClientEmail.Text);
                    try
                    {

                        MailMessage msg = new MailMessage();
                        msg.From = new MailAddress(ClientEmail.Text);
                        msg.To.Add("yamunaartprint@gmail.com");
                        msg.Subject = "Inquiry";
                        msg.Body = "A Contact Request Form has been sent from .  <br/><br/>" + "Name: " + ClientName.Text + "<br/>Contact No: " + ClientContactNo.Text + "<br />Reply Back Email: " + ClientEmail.Text + "<br/><br />" + ClientComment.Text;

                        msg.IsBodyHtml = true;
                        SmtpClient client = new SmtpClient();
                        client.Host = "smtp.gmail.com";
                        client.Credentials = new System.Net.NetworkCredential("yamunaartprint@gmail.com", "yamunaart");
                        client.EnableSsl = true;
                        client.Port = 587;
                        client.Send(msg);
                        client.DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis;
                        MailStatus.Text = "Thank You";
                        ClientName.Text = ClientEmail.Text = ClientComment.Text = ClientContactNo.Text = "";
                    }
                    catch (Exception exx)
                    {
                        MailStatus.Text = "Error Sending Message Please Try Later";
                    }

                }
                catch (Exception ex)
                {
                    MailStatus.Text = "Invalid Email Address";
                }
            }
        }
    }
}