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
    public class GameAdminService : IGameAdminService
    {
        IFileUploadService _fileUploadService;
        CasinoDbContext _casinoDbContext;

        public GameAdminService(IFileUploadService fileUploadService, CasinoDbContext casinoDbContext)
        {
            _fileUploadService = fileUploadService;
            _casinoDbContext = casinoDbContext;
        }

        public IList<Game> Select()
        {
            return _casinoDbContext.Games.ToList();
        }

        public async Task Create(GameCreate game)
        {
           string imageSource = await _fileUploadService.FileUploadAsync(game.Image, Path.Combine("img", "games"));


            Game x = new Game()
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Rules = game.Rules,
                ImageSrc = imageSource,
            };

            _casinoDbContext.Games.Add(x);
            _casinoDbContext.SaveChanges();
            
        }

        public bool Delete(int id)
        {
            bool deleted = false;

            Game? game =
                _casinoDbContext.Games.FirstOrDefault(game => game.Id == id);

            if (game != null)
            {
                _casinoDbContext.Games.Remove(game);
                _casinoDbContext.SaveChanges();

                deleted = true;
            }

            return deleted;
        }

        public Game? Find(int id)
        {
            return _casinoDbContext.Games.FirstOrDefault(game => game.Id == id);
        }

        public async Task Update(GameEdit game)
        {
            Game origGame = Find(game.Id);
            if(origGame == null)
            {
                return;
            }
            _casinoDbContext.Games.Remove(origGame);


            string ImageSrc;
            if (game.Image != null)
            {
                string imageSource = await _fileUploadService.FileUploadAsync(game.Image, Path.Combine("img", "games"));
                ImageSrc = imageSource;
            }
            else
            {
                ImageSrc = origGame.ImageSrc;
            }

            Game x = new Game()
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Rules = game.Rules,
                ImageSrc = ImageSrc,
            };

            _casinoDbContext.Games.Add(x);
            _casinoDbContext.SaveChanges();
        }
    }
}
