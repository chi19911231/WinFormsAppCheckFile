using System.Net.Mail;
using System.Net;
using System.Runtime;

namespace WinFormsAppBase
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// �T�{�O�_���ư���
        /// </summary>
        private bool isChecking = false;

        public Form1()
        {
            InitializeComponent();
            this.Text = Settings.AppConfig.Setting.AppName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //Mail();

            timer1.Interval = 1000;
            timer1.Start();
        }

        public void Mail(string file) 
        {

            // �إ� SmtpClient ����
            SmtpClient smtp = new SmtpClient(Settings.AppConfig.Setting.MailHost, Settings.AppConfig.Setting.MailPort);
            smtp.Credentials = new NetworkCredential(Settings.AppConfig.Setting.SendMail, Settings.AppConfig.Setting.SendPassword);
            smtp.EnableSsl = true;

            // �إ� MailMessage ����
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress( Settings.AppConfig.Setting.SendMail?? ""); // �H��H


            //����H
            string[] receiveMailList = Settings.AppConfig.Setting.ReceiveMailList.Split(';');
            foreach (var receiveMail in receiveMailList)
            {
                if (!string.IsNullOrEmpty(receiveMail))
                {
                   mail.To.Add(receiveMail);
                }
            }

            //CC�H��
            string[] receiveMailCcList = Settings.AppConfig.Setting.ReceiveMailCcList.Split(';');
            foreach (var receiveMailCc in receiveMailCcList)
            {
                if (!string.IsNullOrEmpty(receiveMailCc))
                {
                    mail.CC.Add(receiveMailCc);
                }
            }

            mail.Subject = Settings.AppConfig.Setting.MailSubject;
            mail.Body = Settings.AppConfig.Setting.MailBody;
            mail.Attachments.Add(new Attachment($@"{file}"));

            //mail.Attachments.Add(new Attachment($@"D:\\Test\\�s��r���123.txt"));
            //mail.Attachments.Add(new Attachment($@"D:\\Test\\�s��r���456.txt"));
            //mail.Attachments.Add(new Attachment($@"D:\\Test\\�s��r���789.txt"));




            try
            {
                smtp.Send(mail);
                SetLabelText("�H�H���\");
            }
            catch (Exception ex)
            {              
                SetLabelText(ex.Message);
            }

        }


        private async void timer1_Tick(object sender, EventArgs e)
        {
            //�קK���ư���
            if (isChecking) { return; }
            isChecking = true;
            await Task.Run( () => { CheckFile(); } );
            isChecking = false;

            //����O����
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// �ˬd��Ƨ��O�_�s�W�ɮ�
        /// </summary>
        public void CheckFile() 
        {
            string localPath = $@"{Settings.AppConfig.Setting.LocalFilePath}";
            DateTime fromTime = DateTime.Now.AddSeconds(-(Settings.AppConfig.Setting.FrequencySeconds));
            
            var files = Directory.GetFiles(localPath);
            foreach (var file in files)
            {
                DateTime creationTime = File.GetCreationTime(file);
                if (creationTime >= fromTime)
                {
                    SetLabelText($"�s�W�ɮסG{file} �]�إ߮ɶ��G{creationTime}�^");


                    Mail(file);
                }
            }





        }

        private void SetLabelText(string text)
        {
            labelShow.Invoke(new Action(() => labelShow.Text = text));
            //if (labelShow.InvokeRequired)
            //{
            //    labelShow.Invoke(new Action(() => labelShow.Text = text));
            //}
            //else
            //{
            //    labelShow.Text = text;
            //}
        }


    }
}
