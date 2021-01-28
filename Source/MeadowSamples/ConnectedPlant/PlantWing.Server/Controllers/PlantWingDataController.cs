using Microsoft.AspNetCore.Mvc;
using PlantWing.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PlantWing.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlantWingDataController : ControllerBase
    {
        static List<SoilMoistureEntity> SoilMoistureEntityReadings;

        static PlantWingDataController()
        {
            SoilMoistureEntityReadings = new List<SoilMoistureEntity>();

            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 0, value = 1.0m,   date = DateTime.Now.ToString("g") });
            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 1, value = 0.98m,  date = DateTime.Now.ToString("g") });
            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 2, value = 0.77m,  date = DateTime.Now.ToString("g") });
            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 3, value = 0.56m,  date = DateTime.Now.ToString("g") });
            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 4, value = 0.45m,  date = DateTime.Now.ToString("g") });
            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 5, value = 0.26m,  date = DateTime.Now.ToString("g") });
        }

        public PlantWingDataController() { }

        [HttpGet]
        public IActionResult Get()
        {
            SoilMoistureEntity[] readings = new SoilMoistureEntity[SoilMoistureEntityReadings.Count];
            SoilMoistureEntityReadings.CopyTo(readings);

            return new JsonResult(readings);
        }

        [HttpGet("{id}")]
        public ActionResult<SoilMoistureEntity> GetClimateReading(long id)
        {
            var item = SoilMoistureEntityReadings.FirstOrDefault(x => x.id == id);

            if (item != null)
            {
                return item;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<SoilMoistureEntity> PostClimateReading(SoilMoistureEntity item)
        {            
            if (SoilMoistureEntityReadings.Contains(item))
            {
                Conflict("Already exists");
            }
            
            item.id = SoilMoistureEntityReadings.Count + 1;            
            SoilMoistureEntityReadings.Add(item);

            return CreatedAtAction(nameof(GetClimateReading), new { id = item.id }, item);
        }
    }
}