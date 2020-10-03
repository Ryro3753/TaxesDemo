using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaxesAPI.Models
{
    public class DTOProfile : AutoMapper.Profile
    {
        public DTOProfile()
        {
            CreateMap<TaxesItemDTO, TaxesItem>();
        }
    }
}
