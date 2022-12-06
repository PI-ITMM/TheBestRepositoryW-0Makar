using Finance.Infrastructure;
using Finance.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TypeOperationController : ControllerBase
    {
        private readonly ITypeOperationService _service;
        public async Task<ActionResult<TypeOperation>> DeleteAsync(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Ok();
        }
        public TypeOperationController(ITypeOperationService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeOperation>>> GetAsync()
        {
            return new ObjectResult(await _service.GetAsync());
        }

        [HttpGet("{type}")]
        public async Task<ActionResult<TypeOperation>> GetAsync(bool type)
        {
            var operation = await _service.GetByTypeAsync(type);

            if (operation == null)
            {
                return NotFound();
            }

            return new ObjectResult(operation);
        }


        [HttpPost]
        public async Task<ActionResult<TypeOperation>> PostAsync(TypeOperation operation)
        {
            if (operation == null)
            {
                return BadRequest();
            }

            await _service.CreateAsync(operation);

            return Ok(operation);
        }       
    }
}
