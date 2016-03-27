using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BotApplication.Cards.Interfaces;

namespace BotApplication.Cards
{
    class CardImageService: ICardImageService
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
                    () => new Bitmap(imagePath));
            }

            var client = new HttpClient();
            var bytes = await client.GetByteArrayAsync(card.ImageUrl);

            return await Task.Factory.StartNew(() =>
            {
                File.WriteAllBytes(imagePath, bytes);

                return new Bitmap(
                    new MemoryStream(bytes));
            });
        }
    }
}
