using Kursovoy_project_electronic_shop.Contracts;
using Kursovoy_project_electronic_shop.Interfaces;
using Kursovoy_project_electronic_shop.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kursovoy_project_electronic_shop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        [HttpPost]
        public ActionResult CreateManufacturer(string name)
        {
            if (name == null)
            {
                return BadRequest();
            }

            if (!_manufacturerService.CheckRegex(name))
            {
                ModelState.AddModelError("", "Invalid manufacturer name format");

                return BadRequest(ModelState);
            }

            if (_manufacturerService.CheckManufacturerName(name))
            {
                ModelState.AddModelError("", "Manufacturer already exists");

                return BadRequest(ModelState);
            }

            if (!_manufacturerService.CreateManufacturer(name))
            {
                ModelState.AddModelError("", "Failed to create manufacturer");

                return BadRequest(ModelState);
            }

            return Ok("Manufacturer created");
        }

        [HttpGet]
        public ActionResult<List<Manufacturer>> GetManufacturers()
        {
            var manufacturers = _manufacturerService.GetManufacturers();

            if (manufacturers == null)
            {
                return NotFound("No manufacturers found");
            }

            return Ok(manufacturers);
        }

        [HttpPut]
        public ActionResult UpdateManufacturer(Guid manufacturerUid, string name)
        {
            if (name == null)
            {
                return BadRequest();
            }

            if (!_manufacturerService.CheckRegex(name))
            {
                ModelState.AddModelError("", "Invalid manufacturer name format");

                return BadRequest(ModelState);
            }

            if (_manufacturerService.CheckManufacturerName(name))
            {
                ModelState.AddModelError("", "Manufacturer already exists");

                return BadRequest(ModelState);
            }

            if (!_manufacturerService.UpdateManufacturer(manufacturerUid, name))
            {
                ModelState.AddModelError("", "Failed to update manufacturer");

                return BadRequest(ModelState);
            }

            return Ok("Manufacturer updated");
        }

        [HttpDelete]
        public ActionResult DeleteManufacturer(Guid manufacturerUid)
        {
            if (!_manufacturerService.DeleteManufacturer(manufacturerUid))
            {
                ModelState.AddModelError("", "Failed to delete manufacturer");

                return BadRequest(ModelState);
            }

            return Ok("Manufacturer deleted");
        }
    }
}
