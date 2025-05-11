using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HangmanGame
{
    public partial class GameForm : Form
    {
        // Veri yapıları
        private Dictionary<string, Dictionary<string, List<Tuple<string, string>>>> categories;
        private string currentWord;
        private string displayedWord;
        private List<string> incorrectGuesses;
        private int wrongAttempts;
        private int score;
        private Random random;
        private string imagesPath = @"C:\Users\Yel\source\repos\HangmanGame\HangmanGame\Images\";

        // Ayarlar
        private string selectedCategory;
        private string selectedDifficulty;
        private string selectedImageType;
        private int selectedTimeInSeconds;

        // Zamanlayıcı
        private Timer gameTimer;
        private int remainingTime;
        private Label lblTimer;
        private Label lblGameInfo;

        // Sınıf sabitleri
        private const int MAX_ATTEMPTS = 10;
        private const int STARTING_SCORE = 100;
        private const int WRONG_GUESS_PENALTY = 10;

        public GameForm()
        {
            InitializeComponent();

            // Varsayılan değerler
            selectedCategory = "Karma";
            selectedDifficulty = "Orta";
            selectedImageType = "Adam As";
            selectedTimeInSeconds = 30;

            // Soru kategorilerini ve kelimeleri yükle
            LoadCategories();

            // Zamanlayıcıyı ayarla
            SetupTimer();

            // Bilgi etiketini oluştur
            CreateGameInfoLabel();

            // Oyunu başlat
            InitializeGame();
        }

        public GameForm(string category, string difficulty, string imageType, int timeInSeconds)
        {
            InitializeComponent();

            // Ayarları kaydet
            this.selectedCategory = category;
            this.selectedDifficulty = difficulty;
            this.selectedImageType = imageType;
            this.selectedTimeInSeconds = timeInSeconds;

            // Soru kategorilerini ve kelimeleri yükle
            LoadCategories();

            // Zamanlayıcıyı ayarla
            SetupTimer();

            // Bilgi etiketini oluştur
            CreateGameInfoLabel();

            // Oyunu başlat
            InitializeGame();
        }

        private void CreateGameInfoLabel()
        {
            lblGameInfo = new Label();
            lblGameInfo.AutoSize = true;
            lblGameInfo.Font = new Font("Arial", 10, FontStyle.Bold);
            lblGameInfo.Location = new Point(450, 70);
            lblGameInfo.Size = new Size(300, 20);
            lblGameInfo.Text = $"Kategori: {selectedCategory} - Seviye: {selectedDifficulty} - Süre: {selectedTimeInSeconds} sn";
            lblGameInfo.ForeColor = Color.DarkBlue;
            this.Controls.Add(lblGameInfo);
        }

        private void LoadCategories()
        {
            categories = new Dictionary<string, Dictionary<string, List<Tuple<string, string>>>>();

            // Kategorileri oluştur
            string[] categoryNames = { "Tarih", "Coğrafya", "Matematik", "Genel Kültür", "Karma" };
            string[] difficultyLevels = { "Kolay", "Orta", "Zor" };

            foreach (string category in categoryNames)
            {
                categories[category] = new Dictionary<string, List<Tuple<string, string>>>();

                foreach (string difficulty in difficultyLevels)
                {
                    categories[category][difficulty] = new List<Tuple<string, string>>();
                }
            }

            // Tarih kategorisi
            categories["Tarih"]["Kolay"].Add(new Tuple<string, string>("istanbul", "Türkiye'nin en kalabalık şehri"));
            categories["Tarih"]["Kolay"].Add(new Tuple<string, string>("fatih", "İstanbul'u fetheden padişah"));
            categories["Tarih"]["Kolay"].Add(new Tuple<string, string>("piramit", "Mısır'ın ünlü yapıları"));
            categories["Tarih"]["Kolay"].Add(new Tuple<string, string>("roma", "Colosseum'un bulunduğu eski imparatorluk"));
            categories["Tarih"]["Kolay"].Add(new Tuple<string, string>("mumya", "Eski Mısır'da korunan cesetler"));
            categories["Tarih"]["Kolay"].Add(new Tuple<string, string>("savaş", "Ülkeler arası silahlı çatışma"));
            categories["Tarih"]["Kolay"].Add(new Tuple<string, string>("kral", "Bir ülkeyi yöneten erkek hükümdar"));
            categories["Tarih"]["Kolay"].Add(new Tuple<string, string>("bayrak", "Ülkeleri temsil eden sembol"));
            categories["Tarih"]["Kolay"].Add(new Tuple<string, string>("şövalye", "Ortaçağda zırh giyen savaşçı"));
            categories["Tarih"]["Kolay"].Add(new Tuple<string, string>("kale", "Savunma amaçlı yapılan büyük yapı"));

            categories["Tarih"]["Orta"].Add(new Tuple<string, string>("rönesans", "Avrupa'da 14-17. yüzyıllar arasındaki kültürel hareket"));
            categories["Tarih"]["Orta"].Add(new Tuple<string, string>("reform", "16. yüzyılda Avrupa'daki dini hareket"));
            categories["Tarih"]["Orta"].Add(new Tuple<string, string>("magna carta", "İngiltere'de 1215'te imzalanan önemli belge"));
            categories["Tarih"]["Orta"].Add(new Tuple<string, string>("haçlı", "Kudüs'ü almak için yapılan seferler"));
            categories["Tarih"]["Orta"].Add(new Tuple<string, string>("tapınak", "Dini törenler için kullanılan yapı"));
            categories["Tarih"]["Orta"].Add(new Tuple<string, string>("cumhuriyet", "Halkın seçtiği kişilerce yönetilen devlet biçimi"));
            categories["Tarih"]["Orta"].Add(new Tuple<string, string>("devrim", "Bir düzeni yıkarak yerine yenisini getirme"));
            categories["Tarih"]["Orta"].Add(new Tuple<string, string>("imparator", "Birden fazla ülkeyi yöneten kişi"));
            categories["Tarih"]["Orta"].Add(new Tuple<string, string>("antlaşma", "Ülkeler arası yazılı anlaşma"));
            categories["Tarih"]["Orta"].Add(new Tuple<string, string>("sömürge", "Başka bir ülkenin yönetimi altındaki toprak"));

            categories["Tarih"]["Zor"].Add(new Tuple<string, string>("mezopotamya", "Dicle ve Fırat nehirleri arasındaki bölge"));
            categories["Tarih"]["Zor"].Add(new Tuple<string, string>("hammurabi", "Babil'in en ünlü kralı"));
            categories["Tarih"]["Zor"].Add(new Tuple<string, string>("helenistik", "Büyük İskender sonrası Yunan kültürü dönemi"));
            categories["Tarih"]["Zor"].Add(new Tuple<string, string>("paleolitik", "Eski Taş Devri"));
            categories["Tarih"]["Zor"].Add(new Tuple<string, string>("vikingler", "Kuzey Avrupa'da yaşayan savaşçı topluluk"));
            categories["Tarih"]["Zor"].Add(new Tuple<string, string>("hanedanlık", "Aynı aileden gelen hükümdarlar dizisi"));
            categories["Tarih"]["Zor"].Add(new Tuple<string, string>("oligarşi", "Yönetimin az sayıda kişinin elinde olduğu sistem"));
            categories["Tarih"]["Zor"].Add(new Tuple<string, string>("feodalizm", "Ortaçağ Avrupa'sındaki toplumsal düzen"));
            categories["Tarih"]["Zor"].Add(new Tuple<string, string>("arkeoloji", "Eski uygarlıkları inceleyen bilim dalı"));
            categories["Tarih"]["Zor"].Add(new Tuple<string, string>("hiyeroglif", "Eski Mısır yazı sistemi"));

            // Coğrafya kategorisi
            categories["Coğrafya"]["Kolay"].Add(new Tuple<string, string>("ankara", "Türkiye'nin başkenti"));
            categories["Coğrafya"]["Kolay"].Add(new Tuple<string, string>("amazon", "Dünyanın en büyük yağmur ormanı"));
            categories["Coğrafya"]["Kolay"].Add(new Tuple<string, string>("everest", "Dünyanın en yüksek dağı"));
            categories["Coğrafya"]["Kolay"].Add(new Tuple<string, string>("nil", "Mısır'ın meşhur nehri"));
            categories["Coğrafya"]["Kolay"].Add(new Tuple<string, string>("sahra", "Dünyanın en büyük çölü"));
            categories["Coğrafya"]["Kolay"].Add(new Tuple<string, string>("türkiye", "İstanbul'un bulunduğu ülke"));
            categories["Coğrafya"]["Kolay"].Add(new Tuple<string, string>("dünya", "Üzerinde yaşadığımız gezegen"));
            categories["Coğrafya"]["Kolay"].Add(new Tuple<string, string>("okyanus", "Dünyadaki en büyük su kütlesi"));
            categories["Coğrafya"]["Kolay"].Add(new Tuple<string, string>("ada", "Etrafı suyla çevrili kara parçası"));
            categories["Coğrafya"]["Kolay"].Add(new Tuple<string, string>("göl", "Etrafı tamamen kara ile çevrili büyük su kütlesi"));

            // Matematik kategorisi
            categories["Matematik"]["Kolay"].Add(new Tuple<string, string>("toplama", "Sayıları birbirine ekleme işlemi"));
            categories["Matematik"]["Kolay"].Add(new Tuple<string, string>("çıkarma", "Bir sayıdan diğerini eksiltme işlemi"));
            categories["Matematik"]["Kolay"].Add(new Tuple<string, string>("çarpma", "Bir sayıyı kendisiyle belirtilen sayı kadar toplama"));
            categories["Matematik"]["Kolay"].Add(new Tuple<string, string>("bölme", "Bir sayıyı eşit parçalara ayırma"));
            categories["Matematik"]["Kolay"].Add(new Tuple<string, string>("sıfır", "0 rakamı"));
            categories["Matematik"]["Kolay"].Add(new Tuple<string, string>("üçgen", "Üç kenarlı şekil"));
            categories["Matematik"]["Kolay"].Add(new Tuple<string, string>("kare", "Dört kenarı eşit olan dörtgen"));
            categories["Matematik"]["Kolay"].Add(new Tuple<string, string>("daire", "Merkezi belli olan yuvarlak şekil"));
            categories["Matematik"]["Kolay"].Add(new Tuple<string, string>("sayı", "Matematiksel değer"));
            categories["Matematik"]["Kolay"].Add(new Tuple<string, string>("kesir", "Pay ve paydadan oluşan sayı"));

            // Genel Kültür kategorisi
            categories["Genel Kültür"]["Kolay"].Add(new Tuple<string, string>("telefon", "İletişim kurulmak için kullanılır."));
            categories["Genel Kültür"]["Kolay"].Add(new Tuple<string, string>("bilgisayar", "İnternete girmek için kullanılan cihaz"));
            categories["Genel Kültür"]["Kolay"].Add(new Tuple<string, string>("fil", "Büyük bir memeli"));
            categories["Genel Kültür"]["Kolay"].Add(new Tuple<string, string>("bisiklet", "2 tekerlekli pedal kullanan bir araç"));
            categories["Genel Kültür"]["Kolay"].Add(new Tuple<string, string>("evren", "Her şey"));
            categories["Genel Kültür"]["Kolay"].Add(new Tuple<string, string>("yunus", "Zeki deniz canlısı"));
            categories["Genel Kültür"]["Kolay"].Add(new Tuple<string, string>("dağ", "Dünya yüzeyinde oluşan büyük bir şey"));
            categories["Genel Kültür"]["Kolay"].Add(new Tuple<string, string>("çikolata", "Kakao çekirdiklerinden yapılan tatlı bir yiyecek"));
            categories["Genel Kültür"]["Kolay"].Add(new Tuple<string, string>("kitap", "Sayfalardan oluşan okuma materyali"));
            categories["Genel Kültür"]["Kolay"].Add(new Tuple<string, string>("müzik", "Seslerden oluşan sanat"));

            // Karma kategorisini diğer kategorilerden rastgele seçilen kelimelerle doldurun
            // Karma kategorisini diğer kategorilerden rastgele seçilen kelimelerle doldurun
            Random rand = new Random();
            foreach (string difficulty in difficultyLevels)
            {
                foreach (string category in categoryNames)
                {
                    if (category != "Karma" && categories[category][difficulty].Count > 0)
                    {
                        int randomIndex = rand.Next(categories[category][difficulty].Count);
                        categories["Karma"][difficulty].Add(categories[category][difficulty][randomIndex]);
                    }
                }
            }
        }

        private void SetupTimer()
        {
            // Timer'ı oluştur ve ayarla
            gameTimer = new Timer();
            gameTimer.Interval = 1000; // 1 saniye
            gameTimer.Tick += GameTimer_Tick;

            // Timer etiketini oluştur
            lblTimer = new Label();
            lblTimer.AutoSize = true;
            lblTimer.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            lblTimer.Location = new Point(50, 350);
            lblTimer.Size = new Size(100, 20);
            lblTimer.Text = "SÜRE: " + selectedTimeInSeconds;
            lblTimer.ForeColor = Color.Red;

            // Etiketi form kontrollerine ekle
            this.Controls.Add(lblTimer);
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            remainingTime--;
            lblTimer.Text = "SÜRE: " + remainingTime;

            // Süre bittiğinde oyunu sonlandır
            if (remainingTime <= 0)
            {
                gameTimer.Stop();
                this.BackColor = Color.Red;
                MessageBox.Show("Süre doldu! Kelime: " + currentWord, "Oyun bitti", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Girişi devre dışı bırak
                txtGuess.Enabled = false;
                btnGuess.Enabled = false;
            }
        }

        private void InitializeGame()
        {
            random = new Random();

            // Seçilen kategoriden ve zorluk seviyesinden rastgele bir kelime seç
            if (categories.ContainsKey(selectedCategory) &&
                categories[selectedCategory].ContainsKey(selectedDifficulty) &&
                categories[selectedCategory][selectedDifficulty].Count > 0)
            {
                int wordIndex = random.Next(categories[selectedCategory][selectedDifficulty].Count);
                var selectedWord = categories[selectedCategory][selectedDifficulty][wordIndex];
                currentWord = selectedWord.Item1.ToLower();
                lblClue.Text = selectedWord.Item2; // İpucu
            }
            else
            {
                // Eğer seçilen kategori veya zorluk seviyesi bulunamazsa, varsayılan kelimeler kullan
                string[] defaultWords = { "bilgisayar", "telefon", "televizyon", "internet", "program" };
                string[] defaultClues = {
                        "Üzerinde çalıştığınız elektronik cihaz",
                        "İletişim için kullanılan mobil cihaz",
                        "İzleme amaçlı elektronik alet",
                        "Dünya çapında bilgi ağı",
                        "Yazılım ürünü"
                    };

                int wordIndex = random.Next(defaultWords.Length);
                currentWord = defaultWords[wordIndex].ToLower();
                lblClue.Text = defaultClues[wordIndex];
            }

            displayedWord = new string('_', currentWord.Length);

            incorrectGuesses = new List<string>();
            wrongAttempts = 0;
            score = STARTING_SCORE;

            // Arayüzü güncelle
            lblWordLength.Text = currentWord.Length.ToString();
            lblScore.Text = score.ToString() + " P";
            lblIncorrectGuesses.Text = "";
            txtGuess.Text = "";
            txtGuess.Enabled = true;
            btnGuess.Enabled = true;
            txtGuess.Focus();

            // Arka plan rengini sıfırla
            this.BackColor = SystemColors.Control;

            // Gizli kelimeyi alt çizgilerle göster
            StringBuilder displayWord = new StringBuilder();
            for (int i = 0; i < currentWord.Length; i++)
            {
                if (currentWord[i] == ' ')
                {
                    displayWord.Append(" ");
                }
                else
                {
                    displayWord.Append("_");
                }

                if (i < currentWord.Length - 1)
                    displayWord.Append(" ");
            }
            lblWord.Text = displayWord.ToString();

            // İlk resmi göster
            UpdateHangmanImage();

            // Zamanlayıcıyı başlat
            remainingTime = selectedTimeInSeconds;
            lblTimer.Text = "SÜRE: " + remainingTime;
            gameTimer.Start();
        }

        private void UpdateDisplayedWord()
        {
            char[] display = displayedWord.ToCharArray();

            // Görselleştirme için karakterler arasına boşluk ekle
            StringBuilder formattedDisplay = new StringBuilder();
            for (int i = 0; i < display.Length; i++)
            {
                formattedDisplay.Append(display[i].ToString());
                if (i < display.Length - 1)
                    formattedDisplay.Append(" ");
            }

            lblWord.Text = formattedDisplay.ToString();
        }

        private void UpdateHangmanImage()
        {
            // Seçilen resim türüne göre dosya adını belirleme
            string imagePrefix = "man"; // Varsayılan

            switch (selectedImageType)
            {
                case "Adam As":
                    imagePrefix = "man";
                    break;
                case "Çöp Adam":
                    imagePrefix = "stick";
                    break;
                case "Çiçek":
                    imagePrefix = "flower";
                    break;
                case "Balon":
                    imagePrefix = "balloon";
                    break;
            }

            // Resim dosyası adı (jpg olarak değiştirdim çünkü mevcut kodunuz jpg kullanıyor)
            string imageName = imagePrefix + "-" + wrongAttempts.ToString("00");
            string imagePath = Path.Combine(imagesPath, imageName + ".jpg");

            // Önceki resmi temizle
            if (pbHangman.Image != null)
            {
                pbHangman.Image.Dispose();
            }

            // Resmi yükle
            if (File.Exists(imagePath))
            {
                try
                {
                    pbHangman.Image = Image.FromFile(imagePath);
                }
                catch
                {
                    // Resim yüklenemezse, basit bir arka plan rengi ayarla
                    pbHangman.BackColor = Color.LightGray;
                }
            }
            else
            {
                // Resim bulunamazsa, basit bir arka plan rengi ayarla
                pbHangman.BackColor = Color.LightGray;
            }
        }

        private void btnGuess_Click(object sender, EventArgs e)
        {
            MakeGuess();
        }

        private void txtGuess_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Enter tuşuna basıldığında tahmin yap
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                MakeGuess();
            }
        }

        private void MakeGuess()
        {
            string guessText = txtGuess.Text.Trim().ToLower();

            // Boş girişi kontrol et
            if (string.IsNullOrEmpty(guessText))
            {
                MessageBox.Show("Lütfen bir harf girin.", "Geçersiz", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Birden fazla harf girişi yapılabilir, her harfi ayrı ayrı işle
            bool anyCorrectGuess = false;
            bool allAlreadyGuessed = true;

            foreach (char guess in guessText)
            {
                // Harf olmayan karakterleri atla
                if (!char.IsLetter(guess))
                {
                    continue;
                }

                // Harf daha önce tahmin edilmiş mi kontrol et
                if (incorrectGuesses.Contains(guess.ToString()) || displayedWord.Contains(guess.ToString()))
                {
                    continue; // Bu harf zaten tahmin edilmiş, bir sonraki harfe geç
                }

                // En az bir yeni harf var
                allAlreadyGuessed = false;

                bool correctGuess = false;
                // Tahmin edilen harf kelimede var mı kontrol et
                char[] tempDisplay = displayedWord.ToCharArray();
                for (int i = 0; i < currentWord.Length; i++)
                {
                    if (currentWord[i] == guess)
                    {
                        tempDisplay[i] = guess;
                        correctGuess = true;
                    }
                }

                if (correctGuess)
                {
                    // Görüntülenen kelimeyi güncelle
                    displayedWord = new string(tempDisplay);
                    anyCorrectGuess = true;
                }
                else
                {
                    // Yanlış tahmin
                    incorrectGuesses.Add(guess.ToString());
                    wrongAttempts++;

                    // Skoru azalt
                    score -= WRONG_GUESS_PENALTY;
                }
            }

            // Eğer tüm harfler zaten tahmin edilmişse
            if (allAlreadyGuessed && !string.IsNullOrEmpty(guessText.Trim()))
            {
                MessageBox.Show("Bu harfleri zaten tahmin ettiniz.", "Tekrarlayan harf", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGuess.Text = "";
                return;
            }

            // Arayüzü güncelle
            UpdateDisplayedWord();
            lblIncorrectGuesses.Text = string.Join(", ", incorrectGuesses);
            lblScore.Text = score.ToString() + " P";
            UpdateHangmanImage();

            // Oyuncu kazandı mı kontrol et
            if (!displayedWord.Contains("_"))
            {
                gameTimer.Stop(); // Timer'ı durdur
                this.BackColor = Color.Green;
                MessageBox.Show("Tebrikler! Bu skorla oyunu bitirdin: " + score + "!", "Kazandın", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Oyunu kazandıktan sonra girişi devre dışı bırak
                txtGuess.Enabled = false;
                btnGuess.Enabled = false;
            }
            // Oyuncu kaybetti mi kontrol et
            else if (wrongAttempts >= MAX_ATTEMPTS)
            {
                gameTimer.Stop(); // Timer'ı durdur
                this.BackColor = Color.Red;
                MessageBox.Show("Kaybettin. Kelime: " + currentWord, "Oyun bitti", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Oyunu kaybettikten sonra girişi devre dışı bırak
                txtGuess.Enabled = false;
                btnGuess.Enabled = false;
            }

            txtGuess.Text = "";
            txtGuess.Focus();
        }

        private void btnEndGame_Click(object sender, EventArgs e)
        {
            // Onay mesajı göster
            DialogResult result = MessageBox.Show("Oyundan çıkmak istiyor musun?", "Oyunu bitir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (gameTimer != null)
                    gameTimer.Stop(); // Timer'ı durdur
                MessageBox.Show("Oyundan ayrılıyorsun görüşürüz", "oyun bitti", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void btnShowClue_Click(object sender, EventArgs e)
        {
            // İpucunu bir mesaj kutusu içinde göster
            MessageBox.Show(lblClue.Text, "İpucu", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Diğer InitializeComponent kodunuzu değiştirmeden bırakın (zaten verilmiş)
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblWord = new System.Windows.Forms.Label();
            this.txtGuess = new System.Windows.Forms.TextBox();
            this.btnGuess = new System.Windows.Forms.Button();
            this.lblWordLengthText = new System.Windows.Forms.Label();
            this.lblWordLength = new System.Windows.Forms.Label();
            this.lblIncorrectGuessesText = new System.Windows.Forms.Label();
            this.lblIncorrectGuesses = new System.Windows.Forms.Label();
            this.lblScoreText = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.btnEndGame = new System.Windows.Forms.Button();
            this.pbHangman = new System.Windows.Forms.PictureBox();
            this.lblClue = new System.Windows.Forms.Label();
            this.btnShowClue = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbHangman)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(350, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(158, 32);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "HANGMAN";
            // 
            // lblWord
            // 
            this.lblWord.BackColor = System.Drawing.Color.LightGray;
            this.lblWord.Font = new System.Drawing.Font("Courier New", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWord.Location = new System.Drawing.Point(50, 100);
            this.lblWord.Name = "lblWord";
            this.lblWord.Size = new System.Drawing.Size(350, 40);
            this.lblWord.TabIndex = 1;
            this.lblWord.Text = "_ _ _ _ _";
            this.lblWord.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtGuess
            // 
            this.txtGuess.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGuess.Location = new System.Drawing.Point(200, 170);
            this.txtGuess.MaxLength = 7;
            this.txtGuess.Name = "txtGuess";
            this.txtGuess.Size = new System.Drawing.Size(50, 29);
            this.txtGuess.TabIndex = 2;
            this.txtGuess.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtGuess.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtGuess_KeyPress);
            // 
            // btnGuess
            // 
            this.btnGuess.BackColor = System.Drawing.Color.Green;
            this.btnGuess.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuess.ForeColor = System.Drawing.Color.White;
            this.btnGuess.Location = new System.Drawing.Point(300, 400);
            this.btnGuess.Name = "btnGuess";
            this.btnGuess.Size = new System.Drawing.Size(100, 40);
            this.btnGuess.TabIndex = 3;
            this.btnGuess.Text = "Tahmin Et";
            this.btnGuess.UseVisualStyleBackColor = false;
            this.btnGuess.Click += new System.EventHandler(this.btnGuess_Click);
            // 
            // lblWordLengthText
            // 
            this.lblWordLengthText.AutoSize = true;
            this.lblWordLengthText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWordLengthText.Location = new System.Drawing.Point(50, 230);
            this.lblWordLengthText.Name = "lblWordLengthText";
            this.lblWordLengthText.Size = new System.Drawing.Size(92, 17);
            this.lblWordLengthText.TabIndex = 4;
            this.lblWordLengthText.Text = "Kelime uzunluğu:";
            // 
            // lblWordLength
            // 
            this.lblWordLength.AutoSize = true;
            this.lblWordLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWordLength.Location = new System.Drawing.Point(170, 230);
            this.lblWordLength.Name = "lblWordLength";
            this.lblWordLength.Size = new System.Drawing.Size(17, 17);
            this.lblWordLength.TabIndex = 5;
            this.lblWordLength.Text = "5";
            // 
            // lblIncorrectGuessesText
            // 
            this.lblIncorrectGuessesText.AutoSize = true;
            this.lblIncorrectGuessesText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncorrectGuessesText.Location = new System.Drawing.Point(50, 260);
            this.lblIncorrectGuessesText.Name = "lblIncorrectGuessesText";
            this.lblIncorrectGuessesText.Size = new System.Drawing.Size(120, 17);
            this.lblIncorrectGuessesText.TabIndex = 6;
            this.lblIncorrectGuessesText.Text = "Yanlış tahminler:";
            // 
            // lblIncorrectGuesses
            // 
            this.lblIncorrectGuesses.AutoSize = true;
            this.lblIncorrectGuesses.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIncorrectGuesses.Location = new System.Drawing.Point(170, 260);
            this.lblIncorrectGuesses.Name = "lblIncorrectGuesses";
            this.lblIncorrectGuesses.Size = new System.Drawing.Size(0, 17);
            this.lblIncorrectGuesses.TabIndex = 7;

            // 
            // lblScoreText
            // 
            this.lblScoreText.AutoSize = true;
            this.lblScoreText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreText.Location = new System.Drawing.Point(50, 320);
            this.lblScoreText.Name = "lblScoreText";
            this.lblScoreText.Size = new System.Drawing.Size(70, 20);
            this.lblScoreText.TabIndex = 8;
            this.lblScoreText.Text = "SKOR:";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScore.Location = new System.Drawing.Point(125, 320);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(57, 20);
            this.lblScore.TabIndex = 9;
            this.lblScore.Text = "100 P";
            // 
            // btnEndGame
            // 
            this.btnEndGame.BackColor = System.Drawing.Color.Orange;
            this.btnEndGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEndGame.ForeColor = System.Drawing.Color.White;
            this.btnEndGame.Location = new System.Drawing.Point(420, 400);
            this.btnEndGame.Name = "btnEndGame";
            this.btnEndGame.Size = new System.Drawing.Size(130, 40);
            this.btnEndGame.TabIndex = 10;
            this.btnEndGame.Text = "Oyunu bitir";
            this.btnEndGame.UseVisualStyleBackColor = false;
            this.btnEndGame.Click += new System.EventHandler(this.btnEndGame_Click);
            // 
            // pbHangman
            // 
            this.pbHangman.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbHangman.Location = new System.Drawing.Point(450, 100);
            this.pbHangman.Name = "pbHangman";
            this.pbHangman.Size = new System.Drawing.Size(300, 250);
            this.pbHangman.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbHangman.TabIndex = 11;
            this.pbHangman.TabStop = false;
            // 
            // lblClue
            // 
            this.lblClue.AutoSize = true;
            this.lblClue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClue.Location = new System.Drawing.Point(50, 200);
            this.lblClue.Name = "lblClue";
            this.lblClue.Size = new System.Drawing.Size(33, 15);
            this.lblClue.TabIndex = 12;
            this.lblClue.Text = "Clue";
            // 
            // btnShowClue
            // 
            this.btnShowClue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(190)))), ((int)(((byte)(70)))));
            this.btnShowClue.Location = new System.Drawing.Point(260, 170);
            this.btnShowClue.Name = "btnShowClue";
            this.btnShowClue.Size = new System.Drawing.Size(80, 29);
            this.btnShowClue.TabIndex = 13;
            this.btnShowClue.Text = "İpucu?";
            this.btnShowClue.UseVisualStyleBackColor = false;
            this.btnShowClue.Click += new System.EventHandler(this.btnShowClue_Click);
            // 
            // GameForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.btnShowClue);
            this.Controls.Add(this.lblClue);
            this.Controls.Add(this.pbHangman);
            this.Controls.Add(this.btnEndGame);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.lblScoreText);
            this.Controls.Add(this.lblIncorrectGuesses);
            this.Controls.Add(this.lblIncorrectGuessesText);
            this.Controls.Add(this.lblWordLength);
            this.Controls.Add(this.lblWordLengthText);
            this.Controls.Add(this.btnGuess);
            this.Controls.Add(this.txtGuess);
            this.Controls.Add(this.lblWord);
            this.Controls.Add(this.lblTitle);
            this.Name = "GameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hangman Game";
            ((System.ComponentModel.ISupportInitialize)(this.pbHangman)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblWord;
        private System.Windows.Forms.TextBox txtGuess;
        private System.Windows.Forms.Button btnGuess;
        private System.Windows.Forms.Label lblWordLengthText;
        private System.Windows.Forms.Label lblWordLength;
        private System.Windows.Forms.Label lblIncorrectGuessesText;
        private System.Windows.Forms.Label lblIncorrectGuesses;
        private System.Windows.Forms.Label lblScoreText;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Button btnEndGame;
        private System.Windows.Forms.PictureBox pbHangman;
        private System.Windows.Forms.Label lblClue;
        private System.Windows.Forms.Button btnShowClue;
    }
}
    


