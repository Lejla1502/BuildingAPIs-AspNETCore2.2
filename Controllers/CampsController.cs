﻿using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository repository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public CampsController(ICampRepository repository,IMapper mapper, LinkGenerator linkGenerator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get( bool includeTalks=false)
        {
           // try
           // {
                var results = await repository.GetAllCampsAsync(includeTalks);

                CampModel[] models = mapper.Map<CampModel[]>(results);
                return models;
            /*}
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }*/
        }
        
        [HttpGet("{moniker}")]
        public async Task<ActionResult<CampModel>> Get(string moniker, bool includeTalks = false)
        {
            try
            {
                var result =await repository.GetCampAsync(moniker, includeTalks);
                if (result == null) return NotFound();
                return mapper.Map<CampModel>(result);
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime theDate,bool includeTalks=false)
        {
            try
            {
                var results = await repository.GetAllCampsByEventDate(theDate, includeTalks);
                if (!results.Any()) return NotFound();
                return mapper.Map<CampModel[]>(results);

            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<CampModel>> Post(CampModel model)
        {
            try
            {
                var existing = await repository.GetCampAsync(model.Moniker);
                if (existing != null)
                    return BadRequest("Moniker in Use");

                var location = linkGenerator.GetPathByAction("Get","Camps", new { moniker=model.Moniker});
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Could not use current moniker");  
                }

                var camp = mapper.Map<Camp>(model);
                repository.Add(camp);
                if (await repository.SaveChangesAsync())
                {
                    return Created(location,mapper.Map<CampModel>(camp));
                }
                /* Camp c = new Camp();
                 c.Name = model.Name;
                 c.Moniker = model.Moniker;

                 await repository.SaveChangesAsync();*/
                
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

        [HttpPut("{moniker}")]
        public async Task<ActionResult<CampModel>> Put(string moniker, CampModel model)
        {
            try
            {
                var oldCamp = await repository.GetCampAsync(moniker);
                if (oldCamp == null) return NotFound($"Could not find camp with moniker {moniker}");

                mapper.Map(model, oldCamp);

                if (await repository.SaveChangesAsync())
                    return mapper.Map<CampModel>(oldCamp);
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

        [HttpDelete("{moniker}")]
        public async Task<IActionResult> Delete(string moniker)
        {
            try
            {
                var oldCamp = await repository.GetCampAsync(moniker);
                if (oldCamp == null) return NotFound();
                repository.Delete(oldCamp);

                if (await repository.SaveChangesAsync())
                    return Ok();

            }
            catch
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest("Failed to delete the camp");
        }
    }
}