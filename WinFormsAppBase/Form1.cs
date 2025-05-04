using System.Net.Mail;
using System.Net;
using System.Runtime;
using System.Windows.Forms;
using WinFormsAppBase.Settings;

namespace WinFormsAppBase
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// �T�{�O�_���ư���
        /// </summary>
        private bool isChecking = false;
        /// <summary>
        /// �T��
        /// </summary>
        private string message = "";

        NotifyIcon notifyIconForm = new NotifyIcon();
        ContextMenuStrip contextMenuStripForm = new ContextMenuStrip();

        public Form1()
        {
            InitializeComponent();
            this.Text = AppConfig.Setting.AppName;
        }

        /// <summary>
        /// �B�z�����j�p���ܡ]Resize�^�ɪ��欰
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                HideWindow();
            }
        }

        /// <summary>
        /// ���������]�w
        /// </summary>
        public void AppSettingsForm()
        {
            this.Text = AppConfig.Setting.AppName;
            this.Size = new Size(AppConfig.Setting.WindowWidth, AppConfig.Setting.WindowHeight);

            if (AppConfig.Setting.HideWindowEnable)
            {
                this.Resize += Form1_Resize;
                HideWindow();
            }

            if (AppConfig.Setting.NotifyIconEnable)
            {
                NotifyIconSetting();
            }

        }

        /// <summary>
        /// �Y��k�U��
        /// </summary>
        public void NotifyIconSetting()
        {
            notifyIconForm.Icon = SystemIcons.Application; // �A�i�H�Φۤv���ϥ�
            notifyIconForm.Text = AppConfig.Setting.AppName;
            notifyIconForm.Visible = true;
            //�I��U�}�ҵ���
            notifyIconForm.DoubleClick += (s, ea) => ShowMainForm();
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void HideWindow()
        {
            this.Hide();
            this.ShowInTaskbar = false;
        }

        private void ShowMainForm()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            this.BringToFront();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            AppSettingsForm();
            timer1.Interval = AppConfig.Setting.TimerDate;
            timer1.Start();
        }

        public void Mail(string file) 
        {
            // �إ� SmtpClient ����
            SmtpClient smtp = new SmtpClient(AppConfig.MailSetting.MailHost, AppConfig.MailSetting.MailPort);
            smtp.Credentials = new NetworkCredential(AppConfig.MailSetting.SendMail, AppConfig.MailSetting.SendPassword);
            smtp.EnableSsl = AppConfig.MailSetting.SslEnable;

            // �إ� MailMessage ����
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(AppConfig.MailSetting.SendMail?? ""); // �H��H

            //����H
            string[] receiveMailList = AppConfig.MailSetting.ReceiveMailList.Split(';');
            foreach (var receiveMail in receiveMailList)
            {
                if (!string.IsNullOrEmpty(receiveMail))
                {
                   mail.To.Add(receiveMail);
                }
            }

            //CC�H��
            string[] receiveMailCcList = AppConfig.MailSetting.ReceiveMailCcList.Split(';');
            foreach (var receiveMailCc in receiveMailCcList)
            {
                if (!string.IsNullOrEmpty(receiveMailCc))
                {
                    mail.CC.Add(receiveMailCc);
                }
            }

            mail.Subject = AppConfig.MailSetting.MailSubject;
            mail.Body = AppConfig.MailSetting.MailBody;
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
            DateTime dateTime = DateTime.UtcNow.AddHours(8);
            message = $"�ˬd�O�_���s�W�ɮסA�ˬd�ɶ��G{DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss")}�C\n";
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

            if (AppConfig.FileSetting.LocalFilePathEnable) 
            {
                filePath = $@"{AppConfig.FileSetting.LocalFilePath}";
            }
                
            if (AppConfig.FileSetting.NetworkDriveFilePathEnable) 
            {
                filePath = $@"{AppConfig.FileSetting.NetworkDriveFilePath}";
            }
            
            var files = Directory.GetFiles(filePath);
            DateTime fromTime = DateTime.Now.AddSeconds(-(AppConfig.Setting.FrequencySeconds));

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
