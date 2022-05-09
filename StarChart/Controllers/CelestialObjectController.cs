using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
       private readonly ApplicationDbContext _context;

       public CelestialObjectController(ApplicationDbContext context)
       {
           _context=context;
       }

       [HttpGet("{id:int}",Name = "GetById")]
       public IActionResult GetById(int id)
       {
          var celestialObject = _context.CelestialObjects.FirstOrDefault(so => so.Id == id);
          if (celestialObject == null)
              return NotFound();
          _context.CelestialObjects
              .ForEachAsync(o =>
              {
                  if (o.OrbitedObjectId == celestialObject.Id)
                  {
                      o.Satellites = celestialObject.Satellites;
                  }
              });



          return Ok(celestialObject);
       }

       [HttpGet("{name}")]
       public IActionResult GetByName(string name)
       {
           var celestialObjects = _context.CelestialObjects.Where(co => co.Name == name);
           if (!celestialObjects.Any())
               return NotFound();
           foreach (var co in celestialObjects)
           {
               _context.CelestialObjects.ForEachAsync(o =>
               {
                   if (o.OrbitedObjectId == co.Id)
                   {
                       o.Satellites = co.Satellites;
                   }
               });
           }

           return Ok(celestialObjects);
       }

       [HttpGet]
       public IActionResult GetAll()
       {
           _context.CelestialObjects.ForEachAsync(co => co.Satellites = new List<CelestialObject>());
           return Ok(_context.CelestialObjects);
       }


    }


}
