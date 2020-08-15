﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyLibrary.DTOs;
using MyLibrary.ViewModels;

namespace MyAPI.Services.Interfaces
{
    public interface IEstablishmentsNewsService
    {
        List<NewsDTO> GetAll();
        void Create(EstablishmentNewsVwMdl news);
    }
}
