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
using AForge.Imaging;
using BotApplication.Cards.Interfaces;
using Image = System.Drawing.Image;

namespace BotApplication
{
    public partial class MainWindow : Form
    {
        private readonly ICardAggregator _cardAggregator;
        private readonly ICardImageService _cardImageService;
        private readonly IImageConverter _imageConverter;

        private MainWindow()
        {
            
        }

        public MainWindow(
            ICardAggregator cardAggregator,
            ICardImageService cardImageService,
            IImageConverter imageConverter)
        {
            _cardAggregator = cardAggregator;
            _cardImageService = cardImageService;
            _imageConverter = imageConverter;

            Load += MainWindow_Load;

            InitializeComponent();
        }

        private async void MainWindow_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Loading cards.");

            var cards = await _cardAggregator.LoadCardsAsync();

            Console.WriteLine("Loaded " + cards.Count + " cards.");
        }
    }
}
