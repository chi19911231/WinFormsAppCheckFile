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
            timer1.Interval = 1000;
            timer1.Start();
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
                    SetLabelText($"�s�W�ɮסG{file}�]�إ߮ɶ��G{creationTime}�^");
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
