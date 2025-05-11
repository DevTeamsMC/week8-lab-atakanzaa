using System;
using System.Windows.Forms;
using System.IO;

namespace HangmanGame
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Gerekli dizinleri kontrol et
                CheckRequiredDirectories();

                // Örnek resimleri oluştur (proje için gerekirse)
                CreateSampleImages();

                // Uygulamayı başlat
                Application.Run(new StartForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Uygulama başlatılırken bir hata oluştu: " + ex.Message,
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void CheckRequiredDirectories()
        {
            // Images klasörü
            string imagesPath = Path.Combine(Application.StartupPath, "Images");
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }
        }

        private static void CreateSampleImages()
        {
            // Eğer resimler yoksa, basit bir resim oluşturma - gerçek projede bu fonksiyonu atla
            // Gerçek projede, resimlerinizi eklemelisiniz
            /* 
            Not: Bu sadece bir örnektir. Gerçek projede bu kod yerine şunları yapmanız gerekiyor:
            1. Images klasörüne man-00.png, man-01.png, ... man-10.png resimlerini ekleyin (Adam as)
            2. Images klasörüne stick-00.png, stick-01.png, ... stick-10.png resimlerini ekleyin (Çöp adam)
            3. Images klasörüne flower-00.png, flower-01.png, ... flower-10.png resimlerini ekleyin (Çiçek)
            4. Images klasörüne balloon-00.png, balloon-01.png, ... balloon-10.png resimlerini ekleyin (Balon)
            5. Images klasörüne cover.jpg ve cover2.jpg resimlerini ekleyin (Arka plan resimleri)
            */
        }
    }
}