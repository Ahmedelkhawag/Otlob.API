using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otlob.Core.Specifications
{
    public class ProductSpecificationParams
    {
        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        private int pageSize = 5;
        private int MAxSize = 10;
        public int PageSize
        {
            get => pageSize;
            set => pageSize = value > MAxSize ? MAxSize : value; 
        }
        public int pageNumber { get; set; } = 1;

    }
}
