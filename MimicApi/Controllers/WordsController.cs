using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MimicApi.Data;
using MimicApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicApi.Controllers
{
    [Route("api/words")]
    public class WordsController : ControllerBase
    {
        private readonly MimicContext _mimicContext;
        public WordsController(MimicContext mimicContext)
        {
            _mimicContext = mimicContext;
        }

        [Route("")]
        [HttpGet]
        public ActionResult GetWords(DateTime? date)
        {
            var words = _mimicContext.Words
                .AsQueryable();

            if (date.HasValue)
            {
                words = words
                    .Where(w => w.CreatedAt > date.Value || w.ModifiedAt > date.Value);
            }

            return Ok(words);
        }

        [Route("{id:int}")]
        [HttpGet]
        public ActionResult GetWord(int id)
        {
            var word = _mimicContext.Words.Find(id);

            if (word == null)
                return NotFound();

            return Ok(word);
        }

        [Route("")]
        [HttpPost]
        public ActionResult PostWord([FromBody]Word word)
        {
            _mimicContext.Words.Add(word);
            _mimicContext.SaveChanges();

            return Created($"/api/words/{word.Id}", word);
        }

        [Route("{id:int}")]
        [HttpPut]
        public ActionResult UpdateWord(int id, [FromBody]Word word)
        {
            var obj = _mimicContext.Words
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == id);

            if (obj == null)
                return NotFound();

            word.Id = id;
            _mimicContext.Words.Update(word);
            _mimicContext.SaveChanges();

            return Ok();
        }

        [Route("{id:int}")]
        [HttpDelete]
        public ActionResult DeleteWord(int id)
        {
            var word = _mimicContext.Words.Find(id);

            if (word == null)
                return NotFound();

            word.Active = false;
            _mimicContext.Words.Update(word);
            _mimicContext.SaveChanges();

            return NoContent();
        }
    }
}
