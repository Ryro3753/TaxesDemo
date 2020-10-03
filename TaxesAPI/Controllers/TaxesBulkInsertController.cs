using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxesAPI.Services;

namespace TaxesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxesBulkInsertController : ControllerBase
    {
        private readonly ITaxesService _businessService;



        public TaxesBulkInsertController( ITaxesService businessService)
        {
            _businessService = businessService;
        }

        [HttpPost]
        public async Task<ActionResult<int>> ReadCVS(string path) //This Implementation has missing part from reading file
        {
            return await _businessService.ReadCSVAsync(path);
        }
    }
    
}
