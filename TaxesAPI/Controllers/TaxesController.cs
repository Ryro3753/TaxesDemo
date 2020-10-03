using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaxesAPI.Models;
using TaxesAPI.Services;

namespace TaxesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxesController : ControllerBase
    {
        private readonly TaxesContext _context;
        private readonly ITaxesService _businessService;
        private readonly IMapper _mapper;



        public TaxesController(TaxesContext context, ITaxesService businessService, IMapper mapper)
        {
            _context = context;
            _businessService = businessService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaxesItem>>> GetTaxesItem()
        {
            return await _context.TaxesItem.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaxesItem>> GetTaxesItem(int id)
        {
            var taxesItem = await _businessService.ReadAsync(id); ;

            if (taxesItem == null)
            {
                return NotFound();
            }

            return taxesItem;
        }

        [HttpPost]
        public async Task<ActionResult<int>> SaveTaxesItem(TaxesItemDTO taxesItem)
        {
            if (ModelState.IsValid)
            {
                var item = _mapper.Map<TaxesItem>(taxesItem);
                return await _businessService.SaveAsync(item);
            }
            else
                return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TaxesItem>> DeleteTaxesItem(int id)
        {
            return await _businessService.DeleteAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<int>> ReadCVS(string path)
        {
            return await _businessService.ReadCSVAsync(path);
        }
        
    }
}
