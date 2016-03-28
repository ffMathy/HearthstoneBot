using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge;
using AForge.Imaging;
using BotApplication.Cards.Interfaces;
using BotApplication.Helpers.Interfaces;
using Tesseract;
using Point = System.Drawing.Point;

namespace BotApplication.Cards
{
    public class CardScanner : ICardScanner
    {
        private readonly ICardAggregator _cardAggregator;
        private readonly IOcrHelper _ocrHelper;
        private readonly IImageFilter _imageFilter;

        private const int CardWidthPlayed = 345;
        private const int CardHeightPlayed = 498;

        private const int CardWidthEarlyGame = 288;
        private const int CardHeightEarlyGame = 414;

        private const int TextLabelOffsetYPlayed = 192;
        private const int TextLabelHeightPlayed = 92;

        private const int TextLabelOffsetYEarlyGame = 150;
        private const int TextLabelHeightEarlyGame = 55;

        private const int GemWidthEarlyGame = 40;
        private const int GemHeightEarlyGame = 50;

        private const int GemOffsetXEarlyGame = 108;
        private const int GemOffsetYEarlyGame = 206;

        private const int GemWidthPlayed = 48;
        private const int GemHeightPlayed = 63;

        private const int GemOffsetXPlayed = 150;
        private const int GemOffsetYPlayed = 272;

        private const int ManaCostOffsetXEarlyGame = 0;
        private const int ManaCostOffsetYEarlyGame = 0;
        private const int ManaCostSizeEarlyGame = 50;

        private const int ManaCostOffsetXPlayed = 0;
        private const int ManaCostOffsetYPlayed = 0;
        private const int ManaCostSizePlayed = 68;

        public CardScanner(
            ICardAggregator cardAggregator,
            IOcrHelper ocrHelper,
            IImageFilter imageFilter)
        {
            _cardAggregator = cardAggregator;
            _ocrHelper = ocrHelper;
            _imageFilter = imageFilter;
        }

        private async Task<IEnumerable<ICard>> InferCardCandidatesOrderedByRelevancyFromImageCardLocationAsync(
            Bitmap image,
            Point location,
            Size cardSize,
            int textLabelOffsetY,
            int textLabelHeight,
            Rectangle gemArea,
            Rectangle manaCostArea)
        {
            var cardImage = _imageFilter.CropBitmap(image,
                new Rectangle(location, cardSize));
            if (!IsValidCard(cardImage, manaCostArea))
            {
                return null;
            }

            using (var graphics = Graphics.FromImage(cardImage))
            {
                graphics.FillRectangle(Brushes.Black, gemArea);
            }

            ImageDebuggerForm.DebugImage(cardImage);

            var textScanImage = _imageFilter.ExcludeColorsOutsideRange(cardImage,
                new Rectangle(0, textLabelOffsetY, cardImage.Width, textLabelHeight), 
                new IntRange(200, 255));

            var name = _ocrHelper.GetTextInRegion(textScanImage, new Rect(0, 0, textScanImage.Width, textLabelHeight));
            if (name == null) return null;

            var cards = await _cardAggregator.GetCardCandidatesOrderedByRelevancyFromNameAsync(name);
            return cards;
        }

        public async Task<ICard> InferPlayedCardFromImageCardLocationAsync(Bitmap image, Point location)
        {
            var candidates = await
                    InferCardCandidatesOrderedByRelevancyFromImageCardLocationAsync(image, location,
                        new Size(CardWidthPlayed, CardHeightPlayed),
                        TextLabelOffsetYPlayed,
                        TextLabelHeightPlayed,
                        new Rectangle(GemOffsetXPlayed, GemOffsetYPlayed, GemWidthPlayed, GemHeightPlayed),
                        new Rectangle(ManaCostOffsetXPlayed, ManaCostOffsetYPlayed, ManaCostSizePlayed, ManaCostSizePlayed));
            return candidates?.FirstOrDefault();
        }

        public async Task<ICard> InferEarlyGameCardFromImageCardLocationAsync(Bitmap image, Point location)
        {
            var candidates = await
                    InferCardCandidatesOrderedByRelevancyFromImageCardLocationAsync(image, location,
                        new Size(CardWidthEarlyGame, CardHeightEarlyGame),
                        TextLabelOffsetYEarlyGame,
                        TextLabelHeightEarlyGame,
                        new Rectangle(GemOffsetXEarlyGame, GemOffsetYEarlyGame, GemWidthEarlyGame, GemHeightEarlyGame),
                        new Rectangle(ManaCostOffsetXEarlyGame, ManaCostOffsetYEarlyGame, ManaCostSizeEarlyGame, ManaCostSizeEarlyGame));
            return candidates?.FirstOrDefault(x => x.Collectible);
        }

        private bool IsValidCard(Bitmap image, Rectangle manaCostArea)
        {
            image = _imageFilter.ExcludeColorsOutsideRange(
                image,
                manaCostArea,
                new IntRange(240, 255),
                new IntRange(240, 255),
                new IntRange(254, 255));

            var statistics = new ImageStatistics(image);
            return statistics.PixelsCountWithoutBlack > ManaCostSizeEarlyGame * 1.25;
        }
    }
}