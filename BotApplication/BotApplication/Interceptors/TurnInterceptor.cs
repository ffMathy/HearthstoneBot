using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge;
using AForge.Imaging;
using BotApplication.Helpers.Interfaces;
using BotApplication.Interceptors.Interfaces;
using BotApplication.State;
using BotApplication.State.Interfaces;

namespace BotApplication.Interceptors
{
    class TurnInterceptor: IInterceptor
    {
        private readonly IGameState _gameState;
        private readonly IImageFilter _imageFilter;
        private readonly IOcrHelper _ocrHelper;

        public TurnInterceptor(
            IGameState gameState,
            IImageFilter imageFilter,
            IOcrHelper ocrHelper)
        {
            _gameState = gameState;
            _imageFilter = imageFilter;
            _ocrHelper = ocrHelper;
        }

        public Task OnImageReadyAsync(Bitmap standardImage)
        {
            var relevantImage = _imageFilter.ExcludeColorsOutsideRange(standardImage,
                new Rectangle(1749, 634, 149, 29),
                new IntRange(0, 20),
                new RGB(Color.White));

            var result = _ocrHelper.GetText(relevantImage);
            if (string.Equals(result.Text?.Trim(), "END TURN", StringComparison.OrdinalIgnoreCase))
            {
                _gameState.SwitchTurns(Turn.Local);
            }
            else if (string.Equals(result.Text?.Trim(), "ENEMY TURN", StringComparison.OrdinalIgnoreCase))
            {
                _gameState.SwitchTurns(Turn.Enemy);
            }

            return Task.CompletedTask;
        }
    }
}
