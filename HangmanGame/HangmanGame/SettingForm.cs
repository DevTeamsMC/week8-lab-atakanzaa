using System;
using System.Drawing;
using System.Windows.Forms;

namespace HangmanGame
{
    public partial class SettingsForm : Form
    {
        public string SelectedDifficulty { get; private set; }
        public string SelectedImageType { get; private set; }
        public int SelectedTimeInSeconds { get; private set; }

        private RadioButton rbEasy, rbMedium, rbHard;
        private RadioButton rbHangman, rbStickman, rbFlower, rbBalloon;
        private TrackBar trackBarTime;
        private Label lblTimeValue;

        public SettingsForm(string currentDifficulty, string currentImageType, int currentTimeInSeconds)
        {
            InitializeComponent();
            SelectedDifficulty = currentDifficulty;
            SelectedImageType = currentImageType;
            SelectedTimeInSeconds = currentTimeInSeconds;
            SetupSettingsForm();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SettingsForm
            // 
            this.ClientSize = new System.Drawing.Size(500, 400);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Ayarlar";
            this.ResumeLayout(false);
        }

        private void SetupSettingsForm()
        {
            // Ana panel
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            this.Controls.Add(mainPanel);

            // Başlık
            Label lblTitle = new Label();
            lblTitle.Text = "Oyun Ayarları";
            lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitle.Location = new Point(150, 20);
            lblTitle.Size = new Size(200, 30);
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            mainPanel.Controls.Add(lblTitle);

            // Zorluk seviyesi bölümü
            GroupBox gbDifficulty = new GroupBox();
            gbDifficulty.Text = "Zorluk Seviyesi";
            gbDifficulty.Location = new Point(50, 70);
            gbDifficulty.Size = new Size(180, 120);
            mainPanel.Controls.Add(gbDifficulty);

            rbEasy = new RadioButton();
            rbEasy.Text = "Kolay";
            rbEasy.Location = new Point(20, 30);
            rbEasy.Checked = (SelectedDifficulty == "Kolay");
            gbDifficulty.Controls.Add(rbEasy);

            rbMedium = new RadioButton();
            rbMedium.Text = "Orta";
            rbMedium.Location = new Point(20, 60);
            rbMedium.Checked = (SelectedDifficulty == "Orta");
            gbDifficulty.Controls.Add(rbMedium);

            rbHard = new RadioButton();
            rbHard.Text = "Zor";
            rbHard.Location = new Point(20, 90);
            rbHard.Checked = (SelectedDifficulty == "Zor");
            gbDifficulty.Controls.Add(rbHard);

            // Resim türü bölümü
            GroupBox gbImageType = new GroupBox();
            gbImageType.Text = "Resim Türü";
            gbImageType.Location = new Point(250, 70);
            gbImageType.Size = new Size(200, 120);
            mainPanel.Controls.Add(gbImageType);

            rbHangman = new RadioButton();
            rbHangman.Text = "Adam As";
            rbHangman.Location = new Point(20, 30);
            rbHangman.Checked = (SelectedImageType == "Adam As");
            gbImageType.Controls.Add(rbHangman);

            rbStickman = new RadioButton();
            rbStickman.Text = "Çöp Adam";
            rbStickman.Location = new Point(20, 60);
            rbStickman.Checked = (SelectedImageType == "Çöp Adam");
            gbImageType.Controls.Add(rbStickman);

            rbFlower = new RadioButton();
            rbFlower.Text = "Çiçek";
            rbFlower.Location = new Point(110, 30);
            rbFlower.Checked = (SelectedImageType == "Çiçek");
            gbImageType.Controls.Add(rbFlower);

            rbBalloon = new RadioButton();
            rbBalloon.Text = "Balon";
            rbBalloon.Location = new Point(110, 60);
            rbBalloon.Checked = (SelectedImageType == "Balon");
            gbImageType.Controls.Add(rbBalloon);

            // Süre ayarı bölümü
            GroupBox gbTime = new GroupBox();
            gbTime.Text = "Süre (Saniye)";
            gbTime.Location = new Point(50, 210);
            gbTime.Size = new Size(400, 80);
            mainPanel.Controls.Add(gbTime);

            trackBarTime = new TrackBar();
            trackBarTime.Minimum = 10;
            trackBarTime.Maximum = 120;
            trackBarTime.TickFrequency = 10;
            trackBarTime.Value = SelectedTimeInSeconds;
            trackBarTime.Location = new Point(20, 30);
            trackBarTime.Size = new Size(300, 45);
            trackBarTime.ValueChanged += TrackBarTime_ValueChanged;
            gbTime.Controls.Add(trackBarTime);

            lblTimeValue = new Label();
            lblTimeValue.Text = SelectedTimeInSeconds.ToString() + " saniye";
            lblTimeValue.Location = new Point(330, 30);
            lblTimeValue.Size = new Size(60, 20);
            gbTime.Controls.Add(lblTimeValue);

            // Kaydet ve İptal butonları
            Button btnSave = new Button();
            btnSave.Text = "Kaydet";
            btnSave.Location = new Point(150, 310);
            btnSave.Size = new Size(80, 30);
            btnSave.BackColor = Color.Green;
            btnSave.ForeColor = Color.White;
            btnSave.Click += BtnSave_Click;
            mainPanel.Controls.Add(btnSave);

            Button btnCancel = new Button();
            btnCancel.Text = "İptal";
            btnCancel.Location = new Point(250, 310);
            btnCancel.Size = new Size(80, 30);
            btnCancel.BackColor = Color.Red;
            btnCancel.ForeColor = Color.White;
            btnCancel.Click += BtnCancel_Click;
            mainPanel.Controls.Add(btnCancel);
        }

        private void TrackBarTime_ValueChanged(object sender, EventArgs e)
        {
            lblTimeValue.Text = trackBarTime.Value.ToString() + " saniye";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Seçilen zorluk seviyesini belirle
            if (rbEasy.Checked) SelectedDifficulty = "Kolay";
            else if (rbMedium.Checked) SelectedDifficulty = "Orta";
            else if (rbHard.Checked) SelectedDifficulty = "Zor";

            // Seçilen resim türünü belirle
            if (rbHangman.Checked) SelectedImageType = "Adam As";
            else if (rbStickman.Checked) SelectedImageType = "Çöp Adam";
            else if (rbFlower.Checked) SelectedImageType = "Çiçek";
            else if (rbBalloon.Checked) SelectedImageType = "Balon";

            // Seçilen süreyi belirle
            SelectedTimeInSeconds = trackBarTime.Value;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}