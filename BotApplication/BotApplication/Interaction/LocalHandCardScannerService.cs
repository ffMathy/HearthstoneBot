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

        public LocalHandCardScannerService(
            ICardImageScanner cardImageScanner,
            IMouseInteractor mouseInteractor,
            IGameState gameState,
            ILogger logger,
            IAggregateInterceptor aggregateInterceptor,
            IImageFilter imageFilter)
        {
            _cardImageScanner = cardImageScanner;
            _mouseInteractor = mouseInteractor;
            _gameState = gameState;
            _logger = logger;
            _aggregateInterceptor = aggregateInterceptor;
            _imageFilter = imageFilter;

            gameState.TurnChanged += GameState_TurnChanged;
        }

        private async void GameState_TurnChanged(object sender, EventArgs e)
        {
            await RunCardScanIfNeeded();
        }

        private async Task RunCardScanIfNeeded()
        {
            if (_gameState.CurrentTurn == Turn.Local)
            {
                _logger.LogGameEvent("Scanning for cards in hand.");

                while (_aggregateInterceptor.CurrentImage == null)
                {
                    await Task.Delay(25);
                }

                var image = await WaitForNextFrame(_aggregateInterceptor.CurrentImage);

                var startingPoint = new Point(1536, image.Height-3);
                var destinationOffset = 488;

                var lastBlankSpotRegion = default(Rectangle);

                var x = startingPoint.X;
                const int decrementFactor = 5;
                while (x > destinationOffset)
                {
                    x -= decrementFactor;

                    var pixelSpotPoint = GeneratePixelSpotPoint(x, startingPoint);

                    var pixelSpotRegion = new Rectangle(pixelSpotPoint.X, pixelSpotPoint.Y, 2, 2);
                    if (lastBlankSpotRegion == default(Rectangle))
                    {
                        lastBlankSpotRegion = pixelSpotRegion;
                    }

                    image = await WaitForNextFrame(image);

                    var isInsideCard = IsInsideCard(image, pixelSpotRegion);
                    if (isInsideCard)
                    {
                        while (isInsideCard)
                        {
                            x -= decrementFactor;

                            _mouseInteractor.MoveMouseHumanly(GeneratePixelSpotPoint(x, startingPoint));
                            image = await WaitForNextFrame(image);

                            isInsideCard = IsInsideCard(image, pixelSpotRegion);
                        }

                        _logger.LogGameEvent("Candidate detected.");

                        await Task.Delay(2500);

                        image = await WaitForNextFrame(image);

                        var cardPoint = new Point(x - 160, startingPoint.Y - 630);
                        var card = await _cardImageScanner.InferHoveredCardFromImageCardLocationAsync(
                            image,
                            cardPoint);
                        if (card.Match != null)
                        {
                            _logger.LogGameEvent("Found " + card.Match.Name + " in hand.");
                        }
                        else
                        {
                            _logger.LogGameEvent("False positive.");
                        }

                        _logger.LogGameEvent("Moving to next candidate.");

                        var isSpotInsideCard = false;
                        while (!isSpotInsideCard)
                        {
                            x -= decrementFactor;

                            _mouseInteractor.MoveMouseHumanly(GeneratePixelSpotPoint(x, startingPoint));
                            image = await WaitForNextFrame(image);

                            isSpotInsideCard = IsInsideCard(image, lastBlankSpotRegion);
                        }

                        _logger.LogGameEvent("Next candidate entry point found.");
                    }
                    else
                    {
                        lastBlankSpotRegion = pixelSpotRegion;
                        //using (var originalImage = _imageFilter.CropBitmap(image, pixelSpotRegion))
                        //{
                        //    ImageDebuggerForm.DebugImage(originalImage);
                        //}
                    }
                }
            }

            _logger.LogGameEvent("Hand card scan completed.");
        }

        private async Task<Bitmap> WaitForNextFrame(Bitmap currentImage)
        {
            while (currentImage == _aggregateInterceptor.CurrentImage)
            {
                await Task.Delay(25);
            }
            return _aggregateInterceptor.CurrentImage;
        }

        private static Point GeneratePixelSpotPoint(int x, Point startingPoint)
        {
            return new Point(x, startingPoint.Y);
        }

        private bool IsInsideCard(Bitmap image, Rectangle pixelSpotRegion)
        {
            bool isInsideCard;
            using (var pixelSpot = _imageFilter.ExcludeColorsOutsideRange(image,
                pixelSpotRegion,
                new IntRange(100, 255)))
            {
                var statistics = new ImageStatistics(pixelSpot);
                isInsideCard = statistics.PixelsCountWithoutBlack > 2;
            }
            return isInsideCard;
        }

        public void Start()
        {
        }
    }
}
