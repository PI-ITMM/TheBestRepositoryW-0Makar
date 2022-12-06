﻿using Finance.Infrastructure.CustomExceptions;
using Finance.Infrastructure;
using Finance.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Finance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperationController : ControllerBase
    {
        private readonly IFinanceOperationService _service;

        public OperationController(IFinanceOperationService service)
        {
            _service = service;
        }      

        [HttpPost]
        public async Task<ActionResult<FinanceOperation>> PostAsync(FinanceOperation operation)
        {
            if (operation == null)
            {
                return BadRequest();
            }

            await _service.CreateAsync(operation);

            return Ok(operation);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinanceOperation>>> GetAsync()
        {
            return new ObjectResult(await _service.GetAsync());
        }

        [HttpGet("{dataStr}")]
        public async Task<ActionResult<FinanceOperation>> GetAsync(string dataStr)
        {
            try
            {
                var result = await _service.GetByDataAsync(dataStr);

                return new ObjectResult(result);
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            //catch(System.Exception ex)
            //{
            //    return NotFound();
            //}
        }

        [HttpGet("{dataBeginnigStr}/{dataEndStr}")]
        public async Task<ActionResult<FinanceOperation>> GetAsync(string dataBeginnigStr, string dataEndStr)
        {
            try
            {
                var result = await _service.GetByPeriodAsync(dataBeginnigStr, dataEndStr);

                return new ObjectResult(result);
            }
            catch (BadRequestException)
            {
                return BadRequest();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

    }
}