using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using MimicApi.Data;
using MimicApi.Helpers;
using MimicApi.Models;
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
        private readonly MimicContext _mimicContext;
        public WordsController(MimicContext mimicContext)
        {
            _mimicContext = mimicContext;
        }

        [Route("")]
        [HttpGet]
        public ActionResult GetWords([FromQuery]WordUrlQuery query)
        {
            var words = _mimicContext.Words
                .AsQueryable();

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

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(pagination));

                if (query.Page > pagination.TotalPages)
                {
                    return NotFound();
                }
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
