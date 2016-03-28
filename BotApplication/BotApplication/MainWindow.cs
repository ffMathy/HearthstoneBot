using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using BotApplication.Cards.Interfaces;
using BotApplication.Interceptors.Interfaces;
using BotApplication.Strategies.Interfaces;
using Tesseract;
using Image = System.Drawing.Image;

namespace BotApplication
{
    public partial class MainWindow : Form
    {
        private readonly IAggregateInterceptor _aggregateInterceptor;

        private MainWindow()
        {
            
        }

        public MainWindow(
            IAggregateInterceptor aggregateInterceptor,
            ImageDebuggerForm imageDebuggerForm)
        {
            _aggregateInterceptor = aggregateInterceptor;

            imageDebuggerForm.Show();

            Shown += MainWindow_Shown;
            Load += MainWindow_Load;

            InitializeComponent();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            Hide();
        }

        private async void MainWindow_Load(object sender, EventArgs e)
        {
            await _aggregateInterceptor.StartAsync();


            //var originalImage = _imageConverter.ConvertToFormat(new Bitmap(@"C:\Users\mathi\OneDrive\Billeder\Screenshots\2016-03-27 (4).png"), PixelFormat.Format24bppRgb);

            //var minimum = 200;
            //var filter = new ColorFiltering(
            //    new IntRange(minimum, 255),
            //    new IntRange(minimum, 255),
            //    new IntRange(minimum, 255));
            //filter.ApplyInPlace(originalImage);

            //var tesseract = new TesseractEngine(Environment.CurrentDirectory, "hearthstone", EngineMode.Default);
            //var page = tesseract.Process(originalImage, new Rect(330, 550, 660, 620), PageSegMode.Auto);
            //MessageBox.Show(page.GetText());

            //pictureBox1.Image = originalImage;

            //var card = cards.Single(x => x.Name == "Life Tap");
            //var cardTemplate = (Bitmap)await _cardImageService.GetCardImageAsync(card);

            //var cardTemplateImage = _imageConverter.ConvertToFormat(cardTemplate, PixelFormat.Format24bppRgb);

            //var templateMatching = new ExhaustiveTemplateMatching(0.5f);
            //var matches = templateMatching.ProcessImage(originalImage, cardTemplateImage, new Rectangle(300, 300, 700 - 300, 900 - 300));

            //var highestMatchRectangle = matches
            //    .OrderByDescending(x => x.Similarity)
            //    .First()
            //    .Rectangle;

            //using (var graphics = Graphics.FromImage(originalImage))
            //{
            //    graphics.DrawRectangle(Pens.Red, highestMatchRectangle);
            //}

            //MessageBox.Show(highestMatchRectangle.ToString());
        }
    }
}
