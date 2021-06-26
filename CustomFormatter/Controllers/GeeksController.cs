using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace CustomFormatter.Apis
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeeksController : ControllerBase
    {
        private readonly IEnumerable<Geek> _geeks;

        public GeeksController()
        {
            _geeks = new List<Geek>()
            {
                new Geek() { Id = 1, Name = "Fiyaz", Expertise="Javascript", Rating = 3.0M },
                new Geek() { Id = 2, Name = "Rick", Expertise = ".Net", Rating = 5.0M }
            };
        }

        [FormatFilter]
        [HttpGet]
        [HttpGet("/api/[controller].{format}")]
        public IEnumerable<Geek> Get()
        {
            return _geeks;
        }

        [FormatFilter]
        [HttpGet("{id}")]
        [HttpGet("/api/[controller]/{id}.{format?}")]
        public Geek Get(int id)
        {

            return _geeks.SingleOrDefault(g => g.Id == id);
        }

        [HttpPost]
        public Geek Post([FromBody]Geek geek)
        {
            return geek;
        }
    }
}
