using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AForge;
using AForge.Imaging;
using Autofac;
using BotApplication.Cards.Interfaces;
using BotApplication.Helpers.Interfaces;
using BotApplication.Interaction.Interfaces;
using BotApplication.Interceptors.Interfaces;
using BotApplication.State;
using BotApplication.State.Interfaces;
using Point = System.Drawing.Point;

namespace BotApplication.Interaction
{
    class LocalHandCardScannerService : IStartable
    {
        private readonly ICardImageScanner _cardImageScanner;
        private readonly IMouseInteractor _mouseInteractor;
        private readonly IGameState _gameState;
        private readonly ILogger _logger;
        private readonly IAggregateInterceptor _aggregateInterceptor;
        private readonly IImageFilter _imageFilter;
        private readonly ILocalPlayer _localPlayer;

        public LocalHandCardScannerService(
            ICardImageScanner cardImageScanner,
            IMouseInteractor mouseInteractor,
            IGameState gameState,
            ILogger logger,
            IAggregateInterceptor aggregateInterceptor,
            IImageFilter imageFilter,
            ILocalPlayer localPlayer)
        {
            _cardImageScanner = cardImageScanner;
            _mouseInteractor = mouseInteractor;
            _gameState = gameState;
            _logger = logger;
            _aggregateInterceptor = aggregateInterceptor;
            _imageFilter = imageFilter;
            _localPlayer = localPlayer;

            gameState.TurnChanged += GameState_TurnChanged;
        }

        private async void GameState_TurnChanged(object sender, EventArgs e)
        {
            await Task.Delay(3000);
            await RunCardScanIfNeeded();
        }

        private async Task RunCardScanIfNeeded()
        {
            if (_gameState.CurrentTurn == Turn.Local)
            {
                var currentMousePosition = _mouseInteractor.CurrentLocation;

                _logger.LogGameEvent("Scanning for cards in hand.");
                _localPlayer.ClearCardsInHand();

                while (_aggregateInterceptor.CurrentImage == null)
                {
                    await Task.Delay(25);
                }

                var image = await WaitForNextFrame(_aggregateInterceptor.CurrentImage);

                var mouseStartingPoint = new Point(1536, 1332);
                const int destinationXOffset = 488;
                const int mouseYOffset = 30;

                var lastBlankSpotRegion = default(Rectangle);

                var x = mouseStartingPoint.X;
                const int decrementFactor = 30;

                bool isInsideCard = false;
                while (!isInsideCard && x > destinationXOffset)
                {
                    x -= decrementFactor;

                    var pixelSpotPoint = new Point(x, mouseStartingPoint.Y);

                    var pixelSpotRegion = new Rectangle(pixelSpotPoint.X, pixelSpotPoint.Y, 5, 5);
                    if (lastBlankSpotRegion == default(Rectangle))
                    {
                        lastBlankSpotRegion = pixelSpotRegion;
                    }

                    _mouseInteractor.MoveMouseHumanly(new Point(x, mouseStartingPoint.Y + mouseYOffset));
                    image = await WaitForNextFrame(image);

                    isInsideCard = IsInsideCard(image, pixelSpotRegion);
                    if (isInsideCard)
                    {
                        var matches = new List<ICard>();

                        var cardOffsetY = 0;
                        for (var cardOffsetX = 0; cardOffsetX <= 40; cardOffsetX += 20)
                        {
                            var cardPoint = new Point(x - 140 - cardOffsetX, mouseStartingPoint.Y - 570 - cardOffsetY);
                            var card = await _cardImageScanner.InferHoveredCardFromImageCardLocationAsync(
                                image,
                                cardPoint);
                            if (card.Match != null)
                            {
                                matches.Add(card.Match);
                                break;
                            }
                        }

                        if (matches.Count > 0)
                        {
                            _localPlayer.AddCardToHand(matches
                                .OrderByDescending(c => c.Name.Length)
                                .First());
                        }
                        else
                        {
                            _logger.LogGameEvent("False positive.");
                        }

                        while (isInsideCard && x > destinationXOffset)
                        {
                            x -= decrementFactor;

                            _mouseInteractor.MoveMouseHumanly(new Point(x, mouseStartingPoint.Y + mouseYOffset));
                            image = await WaitForNextFrame(image);

                            isInsideCard = IsInsideCard(image, pixelSpotRegion);
                        }

                        lastBlankSpotRegion = pixelSpotRegion;

                        image = await WaitForNextFrame(image);
                    }
                    else
                    {
                        lastBlankSpotRegion = pixelSpotRegion;
                    }
                }

                _mouseInteractor.MoveMouseHumanly(currentMousePosition);
                _logger.LogGameEvent("Hand card scan completed.");
            }
        }

        private async Task<Bitmap> WaitForNextFrame(Bitmap currentImage)
        {
            while (currentImage == _aggregateInterceptor.CurrentImage)
            {
                await Task.Delay(1);
            }
            return _aggregateInterceptor.CurrentImage;
        }

        private bool IsInsideCard(Bitmap image, Rectangle pixelSpotRegion)
        {
            var normalRange = new IntRange(0, 150);
            using (var pixelSpot = _imageFilter.ExcludeColorsOutsideRange(image,
                pixelSpotRegion,
                normalRange,
                normalRange,
                new IntRange(150, 255)))
            {
                var statistics = new ImageStatistics(pixelSpot);
                return statistics.PixelsCountWithoutBlack > 3;
            }
        }

        public void Start()
        {
        }
    }
}
