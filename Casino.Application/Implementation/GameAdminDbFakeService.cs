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

        public async Task Create(Game game)
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
            game.ImageSrc = imageSource;

            if (DatabaseFake.Games != null)
                DatabaseFake.Games.Add(game);
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

        public async Task Update(Game game)
        {
            Game origGame = Find(game.Id);
            int index = DatabaseFake.Games.IndexOf(origGame);
            DatabaseFake.Games[index] = game;

            if (game.Image != null)
            {
                string imageSource = await _fileUploadService.FileUploadAsync(game.Image, Path.Combine("img", "games"));
                game.ImageSrc = imageSource;
            }
        }
    }
}
