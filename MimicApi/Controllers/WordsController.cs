using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MimicApi.Data;
using MimicApi.Helpers;
using MimicApi.Models;
using MimicApi.Models.DTO;
using MimicApi.Repositories.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MimicApi.Controllers
{
    [Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly IWordRepository _wordRepository;
        private readonly IMapper _mapper;
        public WordsController(IWordRepository wordRepository, IMapper mapper)
        {
            _wordRepository = wordRepository;
            _mapper = mapper;
        }

        [HttpGet("", Name = "GetWords")]
        public ActionResult GetWords([FromQuery]WordUrlQuery query)
        {
            var words = _wordRepository.GetWords(query);

            if (words.Results.Count == 0)
            {
                return NotFound();
            }

            if (words.Pagination != null)
            {
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(words.Pagination));
            }

            var list = _mapper.Map<PaginationList<Word>, PaginationList<WordDTO>>(words);

            foreach (var word in list.Results)
            {
                word.Links = new List<LinkDTO>();
                word.Links.Add(new LinkDTO("self", Url.Link("GetWord", new { id = word.Id }), "GET"));
            }

            list.Links.Add(new LinkDTO("self", Url.Link("GetWords", query), "GET"));

            return Ok(list);
        }

        [HttpGet("{id:int}", Name = "GetWord")]
        public ActionResult GetWord(int id)
        {
            var word = _wordRepository.GetWord(id);

            if (word == null)
                return NotFound();

            WordDTO wordDTO = _mapper.Map<Word, WordDTO>(word);

            wordDTO.Links = new List<LinkDTO>();
            wordDTO.Links.Add(
                new LinkDTO("self", Url.Link("GetWord", new { id = wordDTO.Id }), "GET")
            ); ;
            wordDTO.Links.Add(
                new LinkDTO("update", Url.Link("UpdateWord", new { id = wordDTO.Id }), "PUT")
            ); ;
            wordDTO.Links.Add(
                new LinkDTO("delete", Url.Link("DeleteWord", new { id = wordDTO.Id }), "DELETE")
            ); ;

            return Ok(wordDTO);
        }

        [Route("")]
        [HttpPost]
        public ActionResult PostWord([FromBody]Word word)
        {
            _wordRepository.PostWord(word);

            return Created($"/api/words/{word.Id}", word);
        }

        [HttpPut("{id:int}", Name = "UpdateWord")]
        public ActionResult UpdateWord(int id, [FromBody]Word word)
        {
            var obj = _wordRepository.GetWord(id);

            if (obj == null)
                return NotFound();

            word.Id = id;
            _wordRepository.UpdateWord(word);

            return Ok();
        }

        [HttpDelete("{id:int}", Name = "DeleteWord")]
        public ActionResult DeleteWord(int id)
        {
            var word = _wordRepository.GetWord(id);

            if (word == null)
                return NotFound();

            _wordRepository.DeleteWord(id);

            return NoContent(); 
        }
    }
}
