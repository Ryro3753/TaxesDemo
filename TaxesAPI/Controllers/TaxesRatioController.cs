using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxesAPI.Models;
using TaxesAPI.Services;

namespace TaxesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxesRatioController : ControllerBase
    {
        private readonly TaxesContext _context;
        private readonly ITaxesRatioService _businessRatioService;
        private readonly IMapper _mapper;

        public TaxesRatioController(TaxesContext context, ITaxesRatioService businessRatioService, IMapper mapper)
        {
            _context = context;
            _businessRatioService = businessRatioService;
            _mapper = mapper;
        }

        [HttpPost("{dt,municipality}")]
        public async Task<double> GetRatio(DateTime dt, string municipality)
        {
            return await _businessRatioService.GetRatio(dt, municipality);
        }
    }
}
