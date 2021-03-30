using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using S22.Imap;

namespace TZIRozraha2112
{
    public partial class Form1 : Form
    {
        static Form1 f;
        public Form1()
        {
            InitializeComponent();
            f = this;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var message = new MailMessage(textBox1.Text, textBox3.Text);
            message.Subject = textBox4.Text;
            message.Body = textBox5.Text;

          



            using (SmtpClient mailer = new SmtpClient("smtp.gmail.com", 587))
            {
                mailer.Credentials = new NetworkCredential(textBox1.Text, textBox2.Text);
                mailer.EnableSsl = true;
                mailer.Send(message);
            }

            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Text = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StartRecieving();
        }

        private void StartRecieving()
        {
            Task.Run(() =>
            {
                using (ImapClient client = new ImapClient("imap.gmail.com", 993, textBox1.Text,
                    textBox2.Text, AuthMethod.Login, true))
                {
                    if(client.Supports("IDLE") == false)
                    {
                        MessageBox.Show("Server does not support IDLE");
                        return;
                    }
                    client.NewMessage += new EventHandler<IdleMessageEventArgs>(OnNewMessage);
                    while (true) ;
                }
            });
            
        }

        static void OnNewMessage(object sender, IdleMessageEventArgs e)
        {
            MessageBox.Show("New message recieved!");
            MailMessage m = e.Client.GetMessage(e.MessageUID, FetchOptions.Normal);
            f.Invoke((MethodInvoker)delegate
            {
               f.textBox6.AppendText($"From: {m.From}\n Theme: {m.Subject}\n Message: {m.Body}\n");
                
            });
        }
    }
}
