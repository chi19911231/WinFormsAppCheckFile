using System.Net.Mail;
using System.Net;
using System.Runtime;

namespace WinFormsAppBase
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 確認是否重複執行
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

            // 建立 SmtpClient 物件
            SmtpClient smtp = new SmtpClient(Settings.AppConfig.Setting.MailHost, Settings.AppConfig.Setting.MailPort);
            smtp.Credentials = new NetworkCredential(Settings.AppConfig.Setting.SendMail, Settings.AppConfig.Setting.SendPassword);
            smtp.EnableSsl = true;

            // 建立 MailMessage 物件
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress( Settings.AppConfig.Setting.SendMail?? ""); // 寄件人


            //收件人
            string[] receiveMailList = Settings.AppConfig.Setting.ReceiveMailList.Split(';');
            foreach (var receiveMail in receiveMailList)
            {
                if (!string.IsNullOrEmpty(receiveMail))
                {
                   mail.To.Add(receiveMail);
                }
            }

            //CC信件
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

            //mail.Attachments.Add(new Attachment($@"D:\\Test\\新文字文件123.txt"));
            //mail.Attachments.Add(new Attachment($@"D:\\Test\\新文字文件456.txt"));
            //mail.Attachments.Add(new Attachment($@"D:\\Test\\新文字文件789.txt"));




            try
            {
                smtp.Send(mail);
                SetLabelText("寄信成功");
            }
            catch (Exception ex)
            {              
                SetLabelText(ex.Message);
            }

        }


        private async void timer1_Tick(object sender, EventArgs e)
        {
            //避免重複執行
            if (isChecking) { return; }
            isChecking = true;
            await Task.Run( () => { CheckFile(); } );
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
            string localPath = $@"{Settings.AppConfig.Setting.LocalFilePath}";
            DateTime fromTime = DateTime.Now.AddSeconds(-(Settings.AppConfig.Setting.FrequencySeconds));
            
            var files = Directory.GetFiles(localPath);
            foreach (var file in files)
            {
                DateTime creationTime = File.GetCreationTime(file);
                if (creationTime >= fromTime)
                {
                    SetLabelText($"新增檔案：{file} （建立時間：{creationTime}）");


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
