using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace HangmanGame
{
    public partial class StartForm : Form
    {
        private string imagesPath = @"C:\Users\Yel\source\repos\HangmanGame\HangmanGame\Images";
        private string selectedCategory = "Karma";
        private string selectedDifficulty = "Orta";
        private string selectedImageType = "Adam As";
        private int selectedTimeInSeconds = 30;

        public StartForm()
        {
            InitializeComponent();
            LoadCoverImage();
            UpdateSettingsLabel();
        }

        private void LoadCoverImage()
        {
            string coverPath = Path.Combine(imagesPath, "cover.jpg");
            if (File.Exists(coverPath))
            {
                try
                {
                    Bitmap bmp = new Bitmap(coverPath);
                    pictureBox1.Image = bmp;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Resim yüklenirken hata oluştu: " + ex.Message,
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    pictureBox1.BackColor = Color.SkyBlue;
                }
            }
            else
            {
                MessageBox.Show("Kapak resmi bulunamadı: " + coverPath,
                    "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                pictureBox1.BackColor = Color.SkyBlue;
            }
        }

        private void UpdateSettingsLabel()
        {
            lblSelectedSettings.Text = $"Kategori: {selectedCategory} | Seviye: {selectedDifficulty} | Süre: {selectedTimeInSeconds} sn";
        }

        private void CategoryButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                // Önceki seçilen butonu sıfırla
                btnTarih.BackColor = Color.LightBlue;
                btnCografya.BackColor = Color.LightBlue;
                btnMatematik.BackColor = Color.LightBlue;
                btnGenelKultur.BackColor = Color.LightBlue;
                btnKarma.BackColor = Color.LightBlue;

                // Yeni butonu seç
                clickedButton.BackColor = Color.YellowGreen;
                selectedCategory = clickedButton.Tag.ToString();

                // Etiketi güncelle
                UpdateSettingsLabel();
            }
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            // Ayarlar formunu aç
            SettingsForm settingsForm = new SettingsForm(selectedDifficulty, selectedImageType, selectedTimeInSeconds);
            if (settingsForm.ShowDialog() == DialogResult.OK)
            {
                // Ayarları al
                selectedDifficulty = settingsForm.SelectedDifficulty;
                selectedImageType = settingsForm.SelectedImageType;
                selectedTimeInSeconds = settingsForm.SelectedTimeInSeconds;

                // Etiketi güncelle
                UpdateSettingsLabel();
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                // Mevcut formu gizle
                this.Hide();

                // GameForm'u oluştur ve göster
                GameForm gameForm = new GameForm(selectedCategory, selectedDifficulty, selectedImageType, selectedTimeInSeconds);
                gameForm.FormClosed += (s, args) => this.Close();
                gameForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oyun başlatılırken hata: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // InitializeComponent metodunu buraya koymanız gerekiyor



        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblSelectedSettings = new System.Windows.Forms.Label();
            this.btnSettings = new System.Windows.Forms.Button();
            this.pnlCategories = new System.Windows.Forms.Panel();

            // Kategori butonları
            this.btnTarih = new System.Windows.Forms.Button();
            this.btnCografya = new System.Windows.Forms.Button();
            this.btnMatematik = new System.Windows.Forms.Button();
            this.btnGenelKultur = new System.Windows.Forms.Button();
            this.btnKarma = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlCategories.SuspendLayout();
            this.SuspendLayout();

            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.OrangeRed;
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(350, 400);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(100, 40);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "BAŞLA";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);

            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(300, 80);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 45);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "HANGMAN";

            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(800, 600);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;

            // 
            // pnlCategories
            // 
            this.pnlCategories.BackColor = System.Drawing.Color.Transparent;
            this.pnlCategories.Location = new System.Drawing.Point(50, 150);
            this.pnlCategories.Name = "pnlCategories";
            this.pnlCategories.Size = new System.Drawing.Size(700, 100);
            this.pnlCategories.TabIndex = 3;

            // 
            // btnTarih
            // 
            this.btnTarih.BackColor = System.Drawing.Color.LightBlue;
            this.btnTarih.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnTarih.Location = new System.Drawing.Point(0, 30);
            this.btnTarih.Name = "btnTarih";
            this.btnTarih.Size = new System.Drawing.Size(120, 40);
            this.btnTarih.TabIndex = 4;
            this.btnTarih.Text = "Tarih";
            this.btnTarih.Tag = "Tarih";
            this.btnTarih.UseVisualStyleBackColor = false;
            this.btnTarih.Click += new System.EventHandler(this.CategoryButton_Click);

            // 
            // btnCografya
            // 
            this.btnCografya.BackColor = System.Drawing.Color.LightBlue;
            this.btnCografya.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnCografya.Location = new System.Drawing.Point(140, 30);
            this.btnCografya.Name = "btnCografya";
            this.btnCografya.Size = new System.Drawing.Size(120, 40);
            this.btnCografya.TabIndex = 5;
            this.btnCografya.Text = "Coğrafya";
            this.btnCografya.Tag = "Coğrafya";
            this.btnCografya.UseVisualStyleBackColor = false;
            this.btnCografya.Click += new System.EventHandler(this.CategoryButton_Click);

            // 
            // btnMatematik
            // 
            this.btnMatematik.BackColor = System.Drawing.Color.LightBlue;
            this.btnMatematik.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnMatematik.Location = new System.Drawing.Point(280, 30);
            this.btnMatematik.Name = "btnMatematik";
            this.btnMatematik.Size = new System.Drawing.Size(120, 40);
            this.btnMatematik.TabIndex = 6;
            this.btnMatematik.Text = "Matematik";
            this.btnMatematik.Tag = "Matematik";
            this.btnMatematik.UseVisualStyleBackColor = false;
            this.btnMatematik.Click += new System.EventHandler(this.CategoryButton_Click);

            // 
            // btnGenelKultur
            // 
            this.btnGenelKultur.BackColor = System.Drawing.Color.LightBlue;
            this.btnGenelKultur.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnGenelKultur.Location = new System.Drawing.Point(420, 30);
            this.btnGenelKultur.Name = "btnGenelKultur";
            this.btnGenelKultur.Size = new System.Drawing.Size(120, 40);
            this.btnGenelKultur.TabIndex = 7;
            this.btnGenelKultur.Text = "Genel Kültür";
            this.btnGenelKultur.Tag = "Genel Kültür";
            this.btnGenelKultur.UseVisualStyleBackColor = false;
            this.btnGenelKultur.Click += new System.EventHandler(this.CategoryButton_Click);

            // 
            // btnKarma
            // 
            this.btnKarma.BackColor = System.Drawing.Color.YellowGreen;
            this.btnKarma.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnKarma.Location = new System.Drawing.Point(560, 30);
            this.btnKarma.Name = "btnKarma";
            this.btnKarma.Size = new System.Drawing.Size(120, 40);
            this.btnKarma.TabIndex = 8;
            this.btnKarma.Text = "Karma";
            this.btnKarma.Tag = "Karma";
            this.btnKarma.UseVisualStyleBackColor = false;
            this.btnKarma.Click += new System.EventHandler(this.CategoryButton_Click);

            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.Orange;
            this.btnSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Location = new System.Drawing.Point(650, 20);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(100, 40);
            this.btnSettings.TabIndex = 9;
            this.btnSettings.Text = "Ayarlar";
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.BtnSettings_Click);

            // 
            // lblSelectedSettings
            // 
            this.lblSelectedSettings.AutoSize = true;
            this.lblSelectedSettings.BackColor = System.Drawing.Color.Transparent;
            this.lblSelectedSettings.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lblSelectedSettings.ForeColor = System.Drawing.Color.White;
            this.lblSelectedSettings.Location = new System.Drawing.Point(200, 280);
            this.lblSelectedSettings.Name = "lblSelectedSettings";
            this.lblSelectedSettings.Size = new System.Drawing.Size(400, 19);
            this.lblSelectedSettings.TabIndex = 10;
            this.lblSelectedSettings.Text = "Kategori: Karma | Seviye: Orta | Süre: 30 sn";

            // 
            // StartForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.lblSelectedSettings);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.pnlCategories);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.pictureBox1);

            // Kategori butonlarını panele ekle
            this.pnlCategories.Controls.Add(this.btnTarih);
            this.pnlCategories.Controls.Add(this.btnCografya);
            this.pnlCategories.Controls.Add(this.btnMatematik);
            this.pnlCategories.Controls.Add(this.btnGenelKultur);
            this.pnlCategories.Controls.Add(this.btnKarma);

            this.Name = "StartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hangman Game";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlCategories.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlCategories;
        private System.Windows.Forms.Button btnTarih;
        private System.Windows.Forms.Button btnCografya;
        private System.Windows.Forms.Button btnMatematik;
        private System.Windows.Forms.Button btnGenelKultur;
        private System.Windows.Forms.Button btnKarma;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Label lblSelectedSettings;
    }
}