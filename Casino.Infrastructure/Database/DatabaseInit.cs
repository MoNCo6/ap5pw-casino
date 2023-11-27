using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Casino.Domain.Entities;

namespace Casino.Infrastructure.Database
{
    internal class DatabaseInit
    {
        public IList<Member> GetMembers()
        {
            IList<Member> members = new List<Member>();

            members.Add(
                new Member()
                {
                    Id = 1,
                    Forename = "Jozef",
                    Surname = "Dlhy",
                    Email = "jozef@dlhy.com",
                    Username = "Dlhy69",
                    Password = "456321",
                }
            );

            members.Add(
                new Member()
                {
                    Id = 2,
                    Forename = "Peter",
                    Surname = "Mudry",
                    Email = "peter@mudry.com",
                    Username = "Mudry69",
                    Password = "123456",
                }
            );

            members.Add(
               new Member()
               {
                   Id = 3,
                   Forename = "Martin",
                   Surname = "Pekny",
                   Email = "martin@pekny.com",
                   Username = "Pekny69",
                   Password = "785921",
               }
           );

            members.Add(
                new Member()
                {
                    Id = 4,
                    Forename = "Ludmila",
                    Surname = "Tenka",
                    Email = "ludmila@tenka.com",
                    Username = "Tenka69",
                    Password = "264875913",
                }
            );

            members.Add(
               new Member()
               {
                   Id = 5,
                   Forename = "Ivana",
                   Surname = "Hrozna",
                   Email = "ivana@hrozna.com",
                   Username = "Hrozna69",
                   Password = "75913",
               }
           );

            members.Add(
                new Member()
                {
                    Id = 6,
                    Forename = "Erik",
                    Surname = "Spomaleny",
                    Email = "erik@spomaleny.com",
                    Username = "Spomaleny69",
                    Password = "134679",
                }
            );

            return members;
        }

        public IList<Game> GetGames()
        {
            IList<Game> games = new List<Game>();

            games.Add(
                new Game()
                {
                    Id = 1,
                    Title = "Respin Joker",
                    Description = "Tu sa bude nachadzat popis RESPIN JOKERA",
                    Rules = "18+",
                    ImageSrc = "/img/games/respinjoker.jpg"
                }
            );

            games.Add(
                new Game()
                {
                    Id = 2,
                    Title = "Rullette",
                    Description = "Tu sa bude nachadzat popis RULLETTY",
                    Rules = "18+",
                    ImageSrc = "/img/games/Roulette.png"
                }
            );

            games.Add(
               new Game()
               {
                   Id = 3,
                   Title = "Poker",
                   Description = "Tu sa bude nachadzat popis POKRU",
                   Rules = "18+",
                   ImageSrc = "/img/games/joker.jpg"
               }
           );

            return games;
        }

        public IList<Carousel> GetCarousels()
        {
            IList<Carousel> carousels = new List<Carousel>();


            carousels.Add(new Carousel()
            {
                Id = 1,
                ImageSrc = "/img/carousel/carousel1.jpg",
                ImageAlt = "First slide"
            });


            carousels.Add(new Carousel()
            {
                Id = 2,
                ImageSrc = "/img/carousel/carousel2.jpg",
                ImageAlt = "Second slide"
            });


            carousels.Add(new Carousel()
            {
                Id = 3,
                ImageSrc = "/img/carousel/carousel3.jpg",
                ImageAlt = "Third slide"
            });


            return carousels;
        }


    }
}
