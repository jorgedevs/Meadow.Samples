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

            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 0, value = 1.0m,  date = new DateTime(2021, 02, 01, 14, 35, 25).ToString("g") });  //DateTime.Now.ToString("g") });
            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 1, value = 0.98m, date = new DateTime(2021, 01, 31, 12, 45, 25).ToString("g") });  //DateTime.Now.ToString("g") });
            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 2, value = 0.77m, date = new DateTime(2021, 01, 24, 07, 21, 25).ToString("g") });  //DateTime.Now.ToString("g") });
            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 3, value = 0.56m, date = new DateTime(2021, 01, 22, 10, 33, 25).ToString("g") });  //DateTime.Now.ToString("g") });
            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 4, value = 0.45m, date = new DateTime(2021, 01, 18, 12, 52, 25).ToString("g") });  //DateTime.Now.ToString("g") });
            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 5, value = 0.26m, date = new DateTime(2021, 01, 17, 15, 18, 25).ToString("g") });  //DateTime.Now.ToString("g") });
            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 6, value = 0.20m, date = new DateTime(2021, 01, 13, 20, 49, 25).ToString("g") });  //DateTime.Now.ToString("g") });
            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 7, value = 0.13m, date = new DateTime(2021, 01, 08, 16, 26, 25).ToString("g") });  //DateTime.Now.ToString("g") });
            SoilMoistureEntityReadings.Add(new SoilMoistureEntity() { id = 8, value = 0.05m, date = new DateTime(2021, 01, 05, 19, 56, 25).ToString("g") });  //DateTime.Now.ToString("g") });
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