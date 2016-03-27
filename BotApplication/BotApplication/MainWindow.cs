using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BotApplication.Cards.Interfaces;

namespace BotApplication
{
    public partial class MainWindow : Form
    {
        private readonly ICardAggregator _cardAggregator;
        private readonly ICardImageService _cardImageService;

        private MainWindow()
        {
            
        }

        public MainWindow(
            ICardAggregator cardAggregator,
            ICardImageService cardImageService)
        {
            _cardAggregator = cardAggregator;
            _cardImageService = cardImageService;

            Load += MainWindow_Load;

            InitializeComponent();
        }

        private async void MainWindow_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Loading cards.");

            var cards = await _cardAggregator.LoadCardsAsync();

            Console.WriteLine("Loaded " + cards.Count + " cards.");
            Console.WriteLine("Fetching card images.");

            var cardImageTasks = cards.Select(_cardImageService.GetCardImageAsync);
            var cardImages = await Task.WhenAll(cardImageTasks);

            Console.WriteLine("Done fetching card images.");
        }
    }
}
