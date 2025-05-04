using System.Net.Mail;
using System.Net;
using System.Runtime;
using System.Windows.Forms;

namespace WinFormsAppBase
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// �T�{�O�_���ư���
        /// </summary>
        private bool isChecking = false;

        private string message = "";



        public Form1()
        {
            InitializeComponent();
            this.Text = Settings.AppConfig.Setting.AppName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Mail();
            timer1.Interval = Settings.AppConfig.Setting.TimerDate;
            timer1.Start();
        }

        public void Mail(string file) 
        {
            // �إ� SmtpClient ����
            SmtpClient smtp = new SmtpClient(Settings.AppConfig.Setting.MailHost, Settings.AppConfig.Setting.MailPort);
            smtp.Credentials = new NetworkCredential(Settings.AppConfig.Setting.SendMail, Settings.AppConfig.Setting.SendPassword);
            smtp.EnableSsl = Settings.AppConfig.Setting.EnableSsl;

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
                message += $"�H�H���\\n";
                SetLabelText(message);
            }
            catch (Exception ex)
            {              
                SetLabelText(ex.Message);
            }

        }


        private async void timer1_Tick(object sender, EventArgs e)
        {
            message = $"�ˬd�O�_���s�W�ɮ�\n";
            SetLabelText(message);

            //CheckFile();

            //�קK���ư���
            if (isChecking) { return; }
            isChecking = true;
            await Task.Run(() => { CheckFile(); });
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

            string filePath = "";

            if (Settings.AppConfig.Setting.LocalFilePathEnable) 
            {
                filePath = $@"{Settings.AppConfig.Setting.LocalFilePath}";
            }
                
            if (Settings.AppConfig.Setting.NetworkDriveFilePathEnable) 
            {
                filePath = $@"{Settings.AppConfig.Setting.NetworkDriveFilePath}";
            }
            
            var files = Directory.GetFiles(filePath);
            DateTime fromTime = DateTime.Now.AddSeconds(-(Settings.AppConfig.Setting.FrequencySeconds));

            foreach (var file in files)
            {
                DateTime creationTime = File.GetCreationTime(file);
                if (creationTime >= fromTime)
                {
                    message += $"�s�W�ɮסG{file} �]�إ߮ɶ��G{creationTime}�^\n";                       
                    SetLabelText(message);
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
