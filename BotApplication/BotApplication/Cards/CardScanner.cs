using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using BotApplication.Cards.Interfaces;
using BotApplication.Helpers.Interfaces;
using Tesseract;

namespace BotApplication.Cards
{
    public class CardScanner : ICardScanner
    {
        private readonly ICardAggregator _cardAggregator;
        private readonly IOcrHelper _ocrHelper;

        private const int CardWidthPlayed = 400;
        private const int CardHeightPlayed = 450;

        private const int CardWidthEarlyGame = 288;
        private const int CardHeightEarlyGame = 414;

        private const int TextLabelHeightPlayed = 92;
        private const int TextLabelOffsetYPlayed = 192;

        private const int TextLabelHeightEarlyGame = 64;
        private const int TextLabelOffsetYEarlyGame = 184;

        private const int GemWidthEarlyGame = 30;
        private const int GemHeightEarlyGame = 43;

        private const int GemOffsetXEarlyGame = 118;
        private const int GemOffsetYEarlyGame = 225;

        private const int ManaCostOffsetEarlyGame = -30;
        private const int ManaCostOffsetSizeEarlyGame = 80;

        public CardScanner(
            ICardAggregator cardAggregator,
            IOcrHelper ocrHelper)
        {
            _cardAggregator = cardAggregator;
            _ocrHelper = ocrHelper;
        }

        private async Task<IEnumerable<ICard>> InferCardCandidatesOrderedByRelevancyFromImageCardLocationAsync(
            Bitmap image,
            Point location,
            Point cardSize,
            int textLabelOffsetY,
            int textLabelHeight)
        {
            var name = _ocrHelper.GetTextInRegion(image,
                new Rect(location.X, location.Y + textLabelOffsetY, cardSize.X, textLabelHeight));
            if (name == null) return null;

            var cards = await _cardAggregator.GetCardCandidatesOrderedByRelevancyFromNameAsync(name);
            return cards;
        }

        public async Task<ICard> InferPlayedCardFromImageCardLocationAsync(Bitmap image, Point location)
        {
            var candidates = await
                    InferCardCandidatesOrderedByRelevancyFromImageCardLocationAsync(image, location,
                        new Point(CardWidthPlayed, CardHeightPlayed),
                        TextLabelOffsetYPlayed,
                        TextLabelHeightPlayed);
            return candidates?.FirstOrDefault();
        }

        public async Task<ICard> InferEarlyGameCardFromImageCardLocationAsync(Bitmap image, Point location)
        {
            if (!IsValidCardAtLocation(image, location))
            {
                return null;
            }

            using (var graphics = Graphics.FromImage(image))
            {
                graphics.FillRectangle(Brushes.Black,
                    location.X + GemOffsetXEarlyGame,
                    location.Y + GemOffsetYEarlyGame,
                    GemWidthEarlyGame,
                    GemHeightEarlyGame);
            }
            var candidates = await
                    InferCardCandidatesOrderedByRelevancyFromImageCardLocationAsync(image, location,
                        new Point(CardWidthEarlyGame, CardHeightEarlyGame),
                        TextLabelOffsetYEarlyGame,
                        TextLabelHeightEarlyGame);
            return candidates?.FirstOrDefault(x => x.Collectible);
        }

        private bool IsValidCardAtLocation(Bitmap image, Point location)
        {
            //TODO: maybe try contrast increase dramatically in mana orb before analyzing it?
            var manaCost = _ocrHelper.GetTextInRegion(image,
                new Rect(location.X + ManaCostOffsetEarlyGame, location.Y + ManaCostOffsetEarlyGame + 25, ManaCostOffsetSizeEarlyGame, ManaCostOffsetSizeEarlyGame));

            int result;
            if (int.TryParse(manaCost, out result))
            {
                return true;
            }

            return false;
        }
    }
}