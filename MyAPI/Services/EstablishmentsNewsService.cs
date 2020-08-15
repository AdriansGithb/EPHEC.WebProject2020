using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyAPI.Data;
using MyAPI.Services.Interfaces;
using MyLibrary.DTOs;
using MyLibrary.Entities;
using MyLibrary.ViewModels;

namespace MyAPI.Services
{
    public class EstablishmentsNewsService : IEstablishmentsNewsService
    {
        private readonly AppDbContext _context;

        public EstablishmentsNewsService(AppDbContext context)
        {
            _context = context;
        }

        public List<NewsDTO> GetAll()
        {
            try
            {
                var allNewswithEstab = _context.EstablishmentsNews
                    .Include(x => x.Establishment)
                    .ToList();

                List<NewsDTO> allNewsDtos = new List<NewsDTO>();
                foreach (var news in allNewswithEstab)
                {
                    NewsDTO newsDto = new NewsDTO
                    {
                        Id = news.Id,
                        EstablishmentId = news.EstablishmentId,
                        EstablishmentName = news.Establishment.Name,
                        CreatedDate = news.CreatedDate,
                        UpdatedDate = news.UpdatedDate,
                        Title = news.Title
                    };
                    allNewsDtos.Add(newsDto);
                }

                return allNewsDtos;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Create(EstablishmentNewsVwMdl news)
        {
            try
            {
                EstablishmentsNews newNews = new EstablishmentsNews
                {
                    Id = null,
                    EstablishmentId = news.EstablishmentId,
                    Establishment = null,
                    CreatedDate = news.CreatedDate,
                    UpdatedDate = news.UpdatedDate,
                    Title = news.Title,
                    Text = news.Text,
                    Picture = null
                };

                _context.EstablishmentsNews.Add(newNews);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public EstablishmentNewsVwMdl Get(string id)
        {
            try
            {
                var newswithEstab = _context.EstablishmentsNews
                    .Where(x=>x.Id==id)
                    .Include(x => x.Establishment)
                    .FirstOrDefault();

                EstablishmentNewsVwMdl news = new EstablishmentNewsVwMdl()
                    {
                        Id = newswithEstab.Id,
                        EstablishmentId = newswithEstab.EstablishmentId,
                        EstablishmentName = newswithEstab.Establishment.Name,
                        CreatedDate = newswithEstab.CreatedDate,
                        UpdatedDate = newswithEstab.UpdatedDate,
                        Title = newswithEstab.Title,
                        Text = newswithEstab.Text
                    };

                return news;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Edit(EstablishmentNewsVwMdl news)
        {
            try
            {
                var dbNews = _context.EstablishmentsNews
                    .FirstOrDefault(x => x.Id == news.Id);

                dbNews.Title = news.Title;
                dbNews.Text = news.Text;
                dbNews.UpdatedDate = news.UpdatedDate;

                _context.EstablishmentsNews.Update(dbNews);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(string id)
        {
            try
            {
                var news = _context.EstablishmentsNews
                    .FirstOrDefault(x => x.Id == id);

                _context.Remove(news);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}
