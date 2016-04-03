using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using AForge;
using AForge.Imaging;
using BotApplication.Cards.Interfaces;
using BotApplication.Helpers.Interfaces;
using BotApplication.Strategies.Interfaces;
using Tesseract;
using Point = System.Drawing.Point;

namespace BotApplication.Cards
{
    public class CardImageScanner : ICardImageScanner
    {
        private readonly ICardAggregator _cardAggregator;
        private readonly IOcrHelper _ocrHelper;
        private readonly IImageFilter _imageFilter;
        private readonly ILogger _logger;

        #region Early game

        private const int CardWidthEarlyGame = 288;
        private const int CardHeightEarlyGame = 414;

        private const int TextLabelOffsetYEarlyGame = 169;
        private const int TextLabelHeightEarlyGame = 55;

        private const int GemWidthEarlyGame = 40;
        private const int GemHeightEarlyGame = 50;

        private const int GemOffsetXEarlyGame = 108;
        private const int GemOffsetYEarlyGame = 206;

        private const int ManaCostOffsetXEarlyGame = 0;
        private const int ManaCostOffsetYEarlyGame = 0;
        private const int ManaCostSizeEarlyGame = 50;

        #endregion

        #region Played

        private const int CardWidthPlayed = 345;
        private const int CardHeightPlayed = 498;

        private const int TextLabelOffsetYPlayed = 192;
        private const int TextLabelHeightPlayed = 92;

        private const int GemWidthPlayed = 48;
        private const int GemHeightPlayed = 63;

        private const int GemOffsetXPlayed = 150;
        private const int GemOffsetYPlayed = 272;

        private const int ManaCostOffsetXPlayed = 0;
        private const int ManaCostOffsetYPlayed = 0;
        private const int ManaCostSizePlayed = 68;

        #endregion

        #region Hovered

        private const int CardWidthHovered = 385;
        private const int CardHeightHovered = 565;

        private const int TextLabelOffsetYHovered = 250;
        private const int TextLabelHeightHovered = 70;

        private const int GemWidthHovered = 64;
        private const int GemHeightHovered = 64;

        private const int GemOffsetXHovered = 167;
        private const int GemOffsetYHovered = 300;

        private const int ManaCostOffsetXHovered = 0;
        private const int ManaCostOffsetYHovered = 0;
        private const int ManaCostSizeHovered = 56;

        #endregion

        public CardImageScanner(
            ICardAggregator cardAggregator,
            IOcrHelper ocrHelper,
            IImageFilter imageFilter,
            ILogger logger)
        {
            _cardAggregator = cardAggregator;
            _ocrHelper = ocrHelper;
            _imageFilter = imageFilter;
            _logger = logger;
        }

        private async Task<Tuple<Rectangle, ICard[]>> InferCardCandidatesOrderedByRelevancyFromImageCardLocationAsync(
            Bitmap image,
            Point location,
            Size cardSize,
            int textLabelOffsetY,
            int textLabelHeight,
            Rectangle gemArea,
            Rectangle manaCostArea)
        {
            using (var cardImage = _imageFilter.CropBitmap(image,
                new Rectangle(location, cardSize)))
            {
                using (var graphics = Graphics.FromImage(cardImage))
                {
                    graphics.FillRectangle(Brushes.Black, gemArea);
                }

                if (!IsValidCard(cardImage, manaCostArea))
                {
                    return Tuple.Create<Rectangle, ICard[]>(default(Rectangle), null);
                }

                _logger.LogGameEvent("Mana crystal in card found.");

                using (var textScanImage = _imageFilter.ExcludeColorsOutsideRange(cardImage,
                    new Rectangle(0, textLabelOffsetY, cardImage.Width, textLabelHeight),
                    new IntRange(200, 255)))
                {
                    var result = _ocrHelper.GetTextInRegion(textScanImage,
                        new Rect(0, 0, textScanImage.Width, textLabelHeight));
                    if (result.Text == null)
                    {
                        ImageDebuggerForm.DebugImage(textScanImage);
                        return Tuple.Create<Rectangle, ICard[]>(default(Rectangle), null);
                    }

                    var cards =
                        (await _cardAggregator.GetCardCandidatesOrderedByRelevancyFromNameAsync(result.Text)).ToArray();
                    if (!cards.Any())
                    {
                        //ImageDebuggerForm.DebugImage(cardImage);
                    }

                    var area = result.Area;
                    return Tuple.Create(
                        new Rectangle(
                            area.X,
                            area.Y,
                            area.Width,
                            area.Height),
                        cards);
                }
            }
        }

        public async Task<CardImageScanResult> InferPlayedCardFromImageCardLocationAsync(Bitmap image, Point location)
        {
            var candidates = await
                    InferCardCandidatesOrderedByRelevancyFromImageCardLocationAsync(image, location,
                        new Size(CardWidthPlayed, CardHeightPlayed),
                        TextLabelOffsetYPlayed,
                        TextLabelHeightPlayed,
                        new Rectangle(GemOffsetXPlayed, GemOffsetYPlayed, GemWidthPlayed, GemHeightPlayed),
                        new Rectangle(ManaCostOffsetXPlayed, ManaCostOffsetYPlayed, ManaCostSizePlayed, ManaCostSizePlayed));
            return new CardImageScanResult()
            {
                CardPosition = candidates.Item1,
                Match = candidates.Item2?.FirstOrDefault()
            };
        }

        public async Task<CardImageScanResult> InferEarlyGameCardFromImageCardLocationAsync(Bitmap image, Point location)
        {
            var candidates = await
                    InferCardCandidatesOrderedByRelevancyFromImageCardLocationAsync(image, location,
                        new Size(CardWidthEarlyGame, CardHeightEarlyGame),
                        TextLabelOffsetYEarlyGame,
                        TextLabelHeightEarlyGame,
                        new Rectangle(GemOffsetXEarlyGame, GemOffsetYEarlyGame, GemWidthEarlyGame, GemHeightEarlyGame),
                        new Rectangle(ManaCostOffsetXEarlyGame, ManaCostOffsetYEarlyGame, ManaCostSizeEarlyGame, ManaCostSizeEarlyGame));
            return new CardImageScanResult()
            {
                CardPosition = candidates.Item1,
                Match = candidates.Item2.FirstOrDefault()
            };
        }

        public async Task<CardImageScanResult> InferHoveredCardFromImageCardLocationAsync(Bitmap image, Point location)
        {
            var candidates = await
                    InferCardCandidatesOrderedByRelevancyFromImageCardLocationAsync(image, location,
                        new Size(CardWidthHovered, CardHeightHovered),
                        TextLabelOffsetYHovered,
                        TextLabelHeightHovered,
                        new Rectangle(GemOffsetXHovered, GemOffsetYHovered, GemWidthHovered, GemHeightHovered),
                        new Rectangle(ManaCostOffsetXHovered, ManaCostOffsetYHovered, ManaCostSizeHovered, ManaCostSizeHovered));
            return new CardImageScanResult()
            {
                CardPosition = candidates.Item1,
                Match = candidates.Item2.FirstOrDefault()
            };
        }

        private bool IsValidCard(Bitmap image, Rectangle manaCostArea)
        {
            using (var regularManaImage = _imageFilter.ExcludeColorsOutsideRange(
                image,
                manaCostArea,
                new IntRange(240, 255),
                new IntRange(240, 255),
                new IntRange(254, 255)))
            {
                using (var redManaImage = _imageFilter.ExcludeColorsOutsideRange(
                    image,
                    manaCostArea,
                    new IntRange(254, 255),
                    new IntRange(0, 1),
                    new IntRange(0, 1)))
                {
                    return IsValidManaCrystal(regularManaImage) || IsValidManaCrystal(redManaImage);
                }
            }
        }

        private static bool IsValidManaCrystal(Bitmap image)
        {
            var statistics = new ImageStatistics(image);
            return statistics.PixelsCountWithoutBlack > ManaCostSizeEarlyGame;
        }
    }
}