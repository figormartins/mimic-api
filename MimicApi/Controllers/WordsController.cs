using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MimicApi.Data;
using MimicApi.Helpers;
using MimicApi.Models;
using MimicApi.Repositories.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MimicApi.Controllers
{
    [Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly IWordRepository _wordRepository;
        public WordsController(IWordRepository wordRepository)
        {
            _wordRepository = wordRepository;
        }

        [Route("")]
        [HttpGet]
        public ActionResult GetWords([FromQuery]WordUrlQuery query)
        {
            var words = _wordRepository.GetWords(query);

            if (query.Page > words.Pagination.TotalPages)
            {
                return NotFound();
            }

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(words.Pagination));

            return Ok(words.ToList());
        }

        [Route("{id:int}")]
        [HttpGet]
        public ActionResult GetWord(int id)
        {
            var word = _wordRepository.GetWord(id);

            if (word == null)
                return NotFound();

            return Ok(word);
        }

        [Route("")]
        [HttpPost]
        public ActionResult PostWord([FromBody]Word word)
        {
            _wordRepository.PostWord(word);

            return Created($"/api/words/{word.Id}", word);
        }

        [Route("{id:int}")]
        [HttpPut]
        public ActionResult UpdateWord(int id, [FromBody]Word word)
        {
            var obj = _wordRepository.GetWord(id);

            if (obj == null)
                return NotFound();

            word.Id = id;
            _wordRepository.UpdateWord(word);

            return Ok();
        }

        [Route("{id:int}")]
        [HttpDelete]
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
