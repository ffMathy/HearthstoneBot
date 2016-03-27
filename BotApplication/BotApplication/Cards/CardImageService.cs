using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using BotApplication.Cards.Interfaces;

namespace BotApplication.Cards
{
    class CardImageService : ICardImageService
    {
        private readonly string _cardImageDirectory;

        public CardImageService()
        {
            _cardImageDirectory = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "HearthstoneBot",
                "CardImages");
            if (!Directory.Exists(_cardImageDirectory))
            {
                Directory.CreateDirectory(_cardImageDirectory);
            }
        }

        public async Task<Image> GetCardImageAsync(ICard card)
        {
            var extension = Path.GetExtension(card.ImageUrl);
            var imagePath = Path.Combine(
                _cardImageDirectory,
                $"{card.Id}{extension}");
            if (File.Exists(imagePath))
            {
                return await Task.Factory.StartNew(
                    () => CropCardImage(new Bitmap(imagePath)));
            }

            var client = new HttpClient();
            var bytes = await client.GetByteArrayAsync(card.ImageUrl);

            return await Task.Factory.StartNew(() =>
            {
                var bitmap = CropCardImage(new Bitmap(
                    new MemoryStream(bytes)));

                const double maximumRotation = 27 * 2;
                for (var amountOfCards = 1; amountOfCards < 10; amountOfCards++)
                {
                    var rotationIncrement = maximumRotation/amountOfCards;
                    for (var cardOffset = 0; cardOffset <= amountOfCards; cardOffset++)
                    {
                        var rotation = (int)((cardOffset - amountOfCards/2d)*rotationIncrement);
                        if (rotation == 0) continue;

                        var rotatedImage = RotateCardImage(bitmap, rotation);
                        var rotatedPath = GetRotatedPath(imagePath, rotation);
                        rotatedImage.Save(rotatedPath);
                    }
                }

                bitmap.Save(imagePath);

                return bitmap;
            });
        }

        private static string GetRotatedPath(string imagePath, int angle)
        {
            var direction = angle == 0 ? 
                string.Empty : 
                (angle > 0 ? "-r" : "-l");
            return Path.Combine(
                Path.GetDirectoryName(imagePath),
                Path.GetFileNameWithoutExtension(imagePath) + "-rotated-" + Math.Abs(angle) + direction + Path.GetExtension(imagePath));
        }

        private static Bitmap RotateCardImage(Image bitmap, int angle)
        {
            angle = angle % 360;
            if (angle > 180)
            {
                angle -= 360;
            }

            var pf = bitmap.PixelFormat;

            var sin = (float)Math.Abs(Math.Sin(angle * Math.PI / 180.0)); // this function takes radians
            var cos = (float)Math.Abs(Math.Cos(angle * Math.PI / 180.0)); // this one too
            var newImageWidth = sin * bitmap.Height + cos * bitmap.Width;
            var newImageHeight = sin * bitmap.Width + cos * bitmap.Height;
            var originX = 0f;
            var originY = 0f;

            if (angle > 0)
            {
                if (angle <= 90)
                {
                    originX = sin*bitmap.Height;
                }
                else
                {
                    originX = newImageWidth;
                    originY = newImageHeight - sin*bitmap.Width;
                }
            }
            else
            {
                if (angle >= -90)
                {
                    originY = sin*bitmap.Width;
                }
                else
                {
                    originX = newImageWidth - sin*bitmap.Height;
                    originY = newImageHeight;
                }
            }

            var newImage = new Bitmap((int)newImageWidth, (int)newImageHeight, pf);

            var graphics = Graphics.FromImage(newImage);
            graphics.Clear(Color.Transparent);
            graphics.TranslateTransform(originX, originY); // offset the origin to our calculated values
            graphics.RotateTransform(angle); // set up rotate
            graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphics.DrawImageUnscaled(bitmap, 0, 0); // draw the image at 0, 0
            graphics.Dispose();

            return newImage;

        }

        private static Bitmap CropCardImage(Image image)
        {
            var newWidth = image.Width / 100d * 65;
            var newHeight = image.Height / 100d * 28;

            var offsetX = image.Width / 100d * 15;
            var offsetY = image.Height / 100d * 22;

            var copy = new Bitmap((int)newWidth, (int)newHeight, image.PixelFormat);

            using (var graphics = Graphics.FromImage(copy))
            {
                graphics.DrawImage(image,
                    new Rectangle((int)-offsetX, (int)-offsetY, image.Width, image.Height));
            }
            return copy;
        }
    }
}
