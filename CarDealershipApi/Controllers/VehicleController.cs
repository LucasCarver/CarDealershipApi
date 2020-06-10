using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarDealershipApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarDealershipApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly DealershipDbContext _context;
        public VehicleController(DealershipDbContext context)
        {
            _context = context;
        }

        // GET: api/Vehicle
        [HttpGet]
        public async Task<ActionResult<List<Vehicle>>> GetVehicles()
        {
            var vehicles = await _context.Vehicle.ToListAsync();
            return vehicles;
        }

        // GET: api/Vehicle/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Vehicle>> GetVehicle(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                // RETURNS 404 ERROR
                return NotFound();
            }
            else
            {
                return vehicle;
            }
        }

        // DELETE: api/Vehicle/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle == null)
            {
                // RETURNS 404 ERROR
                return NotFound();
            }
            else
            {
                _context.Vehicle.Remove(vehicle);
                await _context.SaveChangesAsync();
                // RETURNS 204 SUCCESS
                return NoContent();
            }
        }

        // POST: api/Vehicle
        // RETURNS 201 STATUS RESPONSE FOR HTTP POST METHODS THAT CREATE NEW RESOURCES ON SERVER
        [HttpPost]
        public async Task<ActionResult<Vehicle>> AddVehicle(Vehicle newVehicle)
        {
            if (ModelState.IsValid)
            {
                _context.Vehicle.Add(newVehicle);
                await _context.SaveChangesAsync();
                // nameof() specifies the URI of newly created vehicle
                // nameof() used to avoid hardcoding the action in the CreatedAtAction call
                return CreatedAtAction(nameof(GetVehicle), new { id = newVehicle.Id }, newVehicle);
            }
            else
            {
                return BadRequest();
            }
        }

        // PUT: api/Vehicle/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutVehicle(int id, Vehicle updatedVehicle)
        {
            if (id != updatedVehicle.Id || !ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Entry(updatedVehicle).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
