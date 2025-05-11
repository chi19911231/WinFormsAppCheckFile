using System.Net.Mail;
using System.Net;
using System.Runtime;
using System.Windows.Forms;
using WinFormsAppBase.Settings;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Collections.Generic;
using System.Text;


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

        /// <summary>
        /// 重試
        /// </summary>
        private int retry;

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

            //if (AppConfig.Setting.WindowCloseEnable)
            //{
            //    CheckFile();
            //    //關閉程式
            //    Application.Exit();
            //}
            //else
            //{
            //    timer1.Interval = AppConfig.Setting.TimerDate;
            //    timer1.Start();
            //}

        }

        public void Mail(string file)
        {
            // 建立 SmtpClient 物件
            SmtpClient smtp = new SmtpClient(AppConfig.MailSetting.MailHost, AppConfig.MailSetting.MailPort);
            smtp.Credentials = new NetworkCredential(AppConfig.MailSetting.SendMail, AppConfig.MailSetting.SendPassword);
            smtp.EnableSsl = AppConfig.MailSetting.SslEnable;

            // 建立 MailMessage 物件
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(AppConfig.MailSetting.SendMail ?? ""); // 寄件人

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

            try
            {
                smtp.Send(mail);
                message += $"寄信成功\n";
                SetLabelText(message);
            }
            catch (Exception ex)
            {
                SetLabelText($"例外原因:{ex.Message}");
            }

        }


        private async void timer1_Tick(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.UtcNow.AddHours(8);
            message = $"檢查是否有新增檔案，檢查時間：{DateTime.UtcNow.AddHours(8).ToString("yyyy-MM-dd HH:mm:ss")}。\n";
            SetLabelText(message);

            //避免重複執行
            if (isChecking) { return; };
            isChecking = true;
            try
            {
                await Task.Run(() => CheckFile());
            }
            catch (Exception ex)
            {
                SetLabelText(ex.ToString());
            }
            finally
            {
                isChecking = false;
            }

            //釋放記憶體
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }



        /// <summary>
        /// 檢查資料夾是否新增檔案
        /// </summary>
        public void CheckFile()
        {

            message += $"執行檔案檢查中。\n";
            SetLabelText(message);

            string filePath = "";
            if (AppConfig.FileSetting.LocalFilePathEnable)
            {
                filePath = $@"{AppConfig.FileSetting.LocalFilePath}";
            }

            if (AppConfig.FileSetting.NetworkDriveFilePathEnable)
            {
                filePath = $@"{AppConfig.FileSetting.NetworkDriveFilePath}";
            }

            bool existFile = Directory.Exists(filePath);
            if (existFile)
            {
                string[] files = Directory.GetFiles(filePath);
                DateTime fromTime = DateTime.Now.AddSeconds(-(AppConfig.FileSetting.FrequencySeconds));

                foreach (var file in files)
                {
                    DateTime creationTime = File.GetCreationTime(file);

                    retry = 0;
                    while (retry++ <= AppConfig.Setting.RetryMax)
                    {
                        try
                        {
                            if (creationTime >= fromTime)
                            {
                                message += $"新增檔案：{file} （建立時間：{creationTime}）。\n";
                                SetLabelText(message);
                                Mail(file);
                            }
                            break;
                        }
                        catch (IOException)
                        {
                            message += $"執行第{retry}次，檔案正在被暫時性存取，檔案：{file} （建立時間：{creationTime}）。\n";
                            SetLabelText(message);
                            Thread.Sleep(AppConfig.Setting.StopSeconds); // 等5秒再試
                        }
                    }
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


        private void LoadData()
        {
            //string mdfPath = @"D:\DBTEST\AppCheckFile.mdf";

            string mdfPath = @"D:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\AppCheckFile.mdf";
            //string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={mdfPath};Integrated Security=True;Connect Timeout=30";
            string connStr = @"Data Source=localhost;Initial Catalog=AppCheckFile;Integrated Security=True;";




            //using (SqlConnection conn = new SqlConnection(connStr))
            //{


            //    conn.Open();
            //    //string sql = "INSERT INTO[dbo].[UserInformation] ([SNo], [UserName], [Mail])VALUES(1, 2, 3)";



            //   string sql = textBox1.Text;

            //    using (SqlCommand cmd = new SqlCommand(sql, conn))
            //    {
            //        int rowsAffected = cmd.ExecuteNonQuery();
            //        MessageBox.Show($"成功寫入 {rowsAffected} 筆資料！");
            //    }

            //    conn.Close();
            //}

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    //conn.Open();
                    //string query = "SELECT * FROM UserInformation";
                    //SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    //DataTable dt = new DataTable();
                    //adapter.Fill(dt);


                    conn.Open();
                    //string query = "SELECT * FROM UserInformation";
                    string query = textBox1.Text;
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    conn.Close();

                    // 組合字串
                    StringBuilder sb = new StringBuilder();

                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (var item in row.ItemArray)
                        {
                            sb.Append(item.ToString() + " ");
                        }
                        sb.AppendLine(); // 換行
                    }

                    textBox2.Multiline = true;
                    textBox2.ScrollBars = ScrollBars.Vertical;
                    textBox2.Text = sb.ToString();


                }
                catch (Exception ex)
                {
                    MessageBox.Show("錯誤: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
