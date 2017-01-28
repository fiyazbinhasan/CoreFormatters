using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CustomFormatter.Apis
{
    [Route("api/[controller]")]
    public class GeeksController : Controller
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

        // GET: api/geeks
        [FormatFilter]
        [HttpGet]
        [HttpGet("/api/[controller].{format}")]
        public IEnumerable<Geek> Get()
        {
            return _geeks;
        }

        // GET api/geeks/5
        [FormatFilter]
        [HttpGet("{id}")]
        [Route("/api/[controller]/{id}.{format?}")]
        public Geek Get(int id)
        {

            return _geeks.SingleOrDefault(g => g.Id == id);
        }

        // POST api/geeks
        [HttpPost]
        public Geek Post([FromBody]Geek geek)
        {
            return geek;
        }
    }
}
