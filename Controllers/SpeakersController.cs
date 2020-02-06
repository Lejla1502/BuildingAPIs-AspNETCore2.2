using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpeakersController : ControllerBase
    {
        private readonly ICampRepository repository;
        private readonly IMapper mapper;

        public SpeakersController(ICampRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        //[HttpGet("{speakerId:int}")]
        [HttpGet]
        public async Task<ActionResult<SpeakerModel>> Get(int speakerId)
        {
            try
            {
                var results = await repository.GetSpeakerAsync(speakerId);

                //CampModel[] models = mapper.Map<CampModel[]>(results);
                return mapper.Map<SpeakerModel>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }
    }
}