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
            timer1.Interval = 1000;
            timer1.Start();
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
                    SetLabelText($"新增檔案：{file}（建立時間：{creationTime}）");
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
