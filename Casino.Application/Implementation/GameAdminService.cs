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
    // Service class responsible for game administration tasks
    public class GameAdminService : IGameAdminService
    {
        // Service for handling file uploads
        private readonly IFileUploadService _fileUploadService;

        // Database context for accessing and manipulating game data
        private readonly CasinoDbContext _casinoDbContext;

        // Constructor injecting file upload and database context services
        public GameAdminService(IFileUploadService fileUploadService, CasinoDbContext casinoDbContext)
        {
            _fileUploadService = fileUploadService;
            _casinoDbContext = casinoDbContext;
        }

        // Retrieves a list of all games from the database
        public IList<Game> Select()
        {
            return _casinoDbContext.Games.ToList();
        }

        // Creates a new game entry and uploads its image
        public async Task Create(GameCreate game)
        {
            // Upload the game image and store the path
            string imageSource = await _fileUploadService.FileUploadAsync(game.Image, Path.Combine("img", "games"));

            // Create a new Game object with the provided information
            Game newGame = new Game()
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Rules = game.Rules,
                ImageSrc = imageSource, // Path of the uploaded image
            };

            // Add the new game to the database and save changes
            _casinoDbContext.Games.Add(newGame);
            _casinoDbContext.SaveChanges();
        }

        // Deletes a game by its ID
        public bool Delete(int id)
        {
            bool deleted = false;

            // Find the game by ID
            Game? game = _casinoDbContext.Games.FirstOrDefault(g => g.Id == id);

            // If the game is found, remove it and save changes
            if (game != null)
            {
                _casinoDbContext.Games.Remove(game);
                _casinoDbContext.SaveChanges();
                deleted = true;
            }

            // Return whether the deletion was successful
            return deleted;
        }

        // Finds a game by its ID
        public Game? Find(int id)
        {
            // Return the first game that matches the given ID or null
            return _casinoDbContext.Games.FirstOrDefault(game => game.Id == id);
        }

        // Updates an existing game's details
        public async Task Update(GameEdit game)
        {
            // Find the original game by ID
            Game originalGame = Find(game.Id);
            if (originalGame == null)
            {
                // If the original game isn't found, exit the method
                return;
            }

            // Remove the original game from the context
            _casinoDbContext.Games.Remove(originalGame);

            string imageSrc;
            // If a new image is provided, upload it, otherwise keep the original image path
            if (game.Image != null)
            {
                string imageSource = await _fileUploadService.FileUploadAsync(game.Image, Path.Combine("img", "games"));
                imageSrc = imageSource;
            }
            else
            {
                imageSrc = originalGame.ImageSrc;
            }

            // Create a new Game object with updated details
            Game updatedGame = new Game()
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Rules = game.Rules,
                ImageSrc = imageSrc,
            };

            // Add the updated game to the database and save changes
            _casinoDbContext.Games.Add(updatedGame);
            _casinoDbContext.SaveChanges();
        }
    }
}