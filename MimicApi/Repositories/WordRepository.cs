using Microsoft.EntityFrameworkCore;
using MimicApi.Data;
using MimicApi.Helpers;
using MimicApi.Models;
using MimicApi.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MimicApi.Repositories
{
    public class WordRepository : IWordRepository
    {
        private readonly MimicContext _context;

        public WordRepository(MimicContext context)
        {
            _context = context;
        }


        public PaginationList<Word> GetWords(WordUrlQuery query)
        {
            var words = _context.Words
                .AsQueryable();
            var list = new PaginationList<Word>();

            if (query.Date.HasValue)
            {
                words = words
                    .Where(w => w.CreatedAt > query.Date.Value || w.ModifiedAt > query.Date.Value);
            }

            if (query.Page.HasValue && query.Quantity.HasValue)
            {
                var totalQuantity = words.Count();
                words = words
                    .Skip((query.Page.Value - 1) * query.Quantity.Value)
                    .Take(query.Quantity.Value);

                var pagination = new Pagination()
                {
                    Page = query.Page.Value,
                    Quantity = query.Quantity.Value,
                    TotalItems = totalQuantity,
                    TotalPages = (int)Math.Ceiling((double)totalQuantity / query.Quantity.Value)
                };

                list.Pagination = pagination;
            }

            list.Results.AddRange(words.ToList());

            return list;
        }

        public Word GetWord(int id)
        {
            return _context.Words
                .AsNoTracking()
                .FirstOrDefault(w => w.Id == id);
        }

        public void PostWord(Word word)
        {
            _context.Words.Add(word);
            _context.SaveChanges();
        }

        public void UpdateWord(Word word)
        {
            _context.Words.Update(word);
            _context.SaveChanges();
        }

        public void DeleteWord(int id)
        {
            var word = GetWord(id);
            word.Active = false;
            _context.Words.Update(word);
            _context.SaveChanges();
        }
    }
}
