using MimicApi.Helpers;
using MimicApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicApi.Repositories.Contracts
{
    public interface IWordRepository
    {
        PaginationList<Word> GetWords(WordUrlQuery query);
        Word GetWord(int id);
        void PostWord(Word word);
        void UpdateWord(Word word);
        void DeleteWord(int id);
    }
}
