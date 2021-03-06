﻿using AutoMapper;
using MimicApi.Models;
using MimicApi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicApi.Helpers
{
    public class DTOMapperPrifile : Profile
    {
        public DTOMapperPrifile()
        {
            CreateMap<Word, WordDTO>();
            CreateMap<PaginationList<Word>, PaginationList<WordDTO>>();
        }
    }
}
