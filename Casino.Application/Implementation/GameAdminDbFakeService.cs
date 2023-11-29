using Casino.Application.Abstraction;
using Casino.Domain.Entities;
using Casino.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Application.Implementation
{
    public class GameAdminDbFakeService : IGameAdminService
    {
        IFileUploadService _fileUploadService;

        public GameAdminDbFakeService(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        public IList<Game> Select()
        {
            return DatabaseFake.Games;
        }

        public async Task Create(GameCreate game)
        {
            if (DatabaseFake.Games != null &&
                DatabaseFake.Games.Count > 0)
            {
                game.Id = DatabaseFake.Games.Last().Id + 1;
            }
            else
            {
                game.Id = 1;
            }

            string imageSource = await _fileUploadService.FileUploadAsync(game.Image, Path.Combine("img", "games"));

            Game x = new Game()
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Rules = game.Rules,
                ImageSrc = imageSource,
            };

            if (DatabaseFake.Games != null)
                DatabaseFake.Games.Add(x);
        }

        public bool Delete(int id)
        {
            bool deleted = false;

            Game? game =
                DatabaseFake.Games.FirstOrDefault(game => game.Id == id);

            if (game != null)
            {
                deleted = DatabaseFake.Games.Remove(game);
            }

            return deleted;
        }

        public Game? Find(int id)
        {
            return DatabaseFake.Games.FirstOrDefault(game => game.Id == id);
        }

        public async Task Update(GameEdit game)
        {
            Game origGame = Find(game.Id);
            int index = DatabaseFake.Games.IndexOf(origGame);


            string ImageSrc;
            if (game.Image != null)
            {
                string imageSource = await _fileUploadService.FileUploadAsync(game.Image, Path.Combine("img", "games"));
                ImageSrc = imageSource;
            } else
            {
                ImageSrc = origGame.ImageSrc;
            }

            DatabaseFake.Games[index] = new Game() { 
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Rules = game.Rules,
                ImageSrc = ImageSrc,
            };
        }
    }
}
