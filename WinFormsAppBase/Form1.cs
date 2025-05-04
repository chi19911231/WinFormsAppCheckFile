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
        /// 確認是否重複執行
        /// </summary>
        private bool isChecking = false;
        /// <summary>
        /// 訊息
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
        /// 處理視窗大小改變（Resize）時的行為
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
        /// 視窗相關設定
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
        /// 縮到右下方
        /// </summary>
        public void NotifyIconSetting()
        {
            notifyIconForm.Icon = SystemIcons.Application; // 你可以用自己的圖示
            notifyIconForm.Text = AppConfig.Setting.AppName;
            notifyIconForm.Visible = true;
            //點兩下開啟視窗
            notifyIconForm.DoubleClick += (s, ea) => ShowMainForm();
        }

        /// <summary>
        /// 視窗隱藏
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
            // 建立 SmtpClient 物件
            SmtpClient smtp = new SmtpClient(AppConfig.MailSetting.MailHost, AppConfig.MailSetting.MailPort);
            smtp.Credentials = new NetworkCredential(AppConfig.MailSetting.SendMail, AppConfig.MailSetting.SendPassword);
            smtp.EnableSsl = AppConfig.MailSetting.SslEnable;

            // 建立 MailMessage 物件
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(AppConfig.MailSetting.SendMail?? ""); // 寄件人

            //收件人
            string[] receiveMailList = AppConfig.MailSetting.ReceiveMailList.Split(';');
            foreach (var receiveMail in receiveMailList)
            {
                if (!string.IsNullOrEmpty(receiveMail))
                {
                   mail.To.Add(receiveMail);
                }
            }

            //CC信件
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

            //mail.Attachments.Add(new Attachment($@"D:\\Test\\新文字文件123.txt"));
            //mail.Attachments.Add(new Attachment($@"D:\\Test\\新文字文件456.txt"));
            //mail.Attachments.Add(new Attachment($@"D:\\Test\\新文字文件789.txt"));

            try
            {
                smtp.Send(mail);
                message += $"寄信成功\n";
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
            message = $"檢查是否有新增檔案，檢查時間：{DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss")}。\n";
            SetLabelText(message);

            //CheckFile();

            //避免重複執行
            if (isChecking) { return; }
            isChecking = true;
            await Task.Run(() => { CheckFile(); });
            isChecking = false;

            //釋放記憶體
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// 檢查資料夾是否新增檔案
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
                    message += $"新增檔案：{file} （建立時間：{creationTime}）\n";                       
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
