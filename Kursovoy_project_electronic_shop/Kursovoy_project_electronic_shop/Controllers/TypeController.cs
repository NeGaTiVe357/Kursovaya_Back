using Kursovoy_project_electronic_shop.Interfaces;
using Kursovoy_project_electronic_shop.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using Microsoft.AspNetCore.Authorization;

namespace Kursovoy_project_electronic_shop.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class TypeController : ControllerBase
    {
        private readonly ITypeService _typeService;

        public TypeController(ITypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpPost]
        public ActionResult CreateType(string name)
        {
            if (name == null)
            {
                return BadRequest();
            }

            if (!_typeService.CheckRegex(name))
            {
                ModelState.AddModelError("", "Invalid type name format");

                return BadRequest(ModelState);
            }

            if (_typeService.CheckTypeName(name))
            {
                ModelState.AddModelError("", "Type already exists");

                return BadRequest(ModelState);
            }

            if (!_typeService.CreateType(name))
            {
                ModelState.AddModelError("", "Failed to create type");

                return BadRequest(ModelState);
            }

            return Ok("Type created");
        }

        [HttpGet]
        public ActionResult<List<Contracts.Type>> GetTypes()
        {
            var types = _typeService.GetTypes();

            if (types == null)
            {
                return NotFound("No types found");
            }

            return Ok(types);
        }

        [HttpPut]
        public ActionResult UpdateType(Guid typeUid, string name)
        {
            if (name == null)
            {
                return BadRequest();
            }

            if (!_typeService.CheckRegex(name))
            {
                ModelState.AddModelError("", "Invalid type name format");

                return BadRequest(ModelState);
            }

            if (_typeService.CheckTypeName(name))
            {
                ModelState.AddModelError("", "Type already exists");

                return BadRequest(ModelState);
            }

            if (!_typeService.UpdateType(typeUid, name))
            {
                ModelState.AddModelError("", "Failed to update type");

                return BadRequest(ModelState);
            }

            return Ok("Type updated");
        }

        [HttpDelete]
        public ActionResult DeleteType(Guid typeUid)
        {
            if (!_typeService.DeleteType(typeUid))
            {
                ModelState.AddModelError("", "Failed to delete type");

                return BadRequest(ModelState);
            }

            return Ok("Type deleted");
        }
    }
}
