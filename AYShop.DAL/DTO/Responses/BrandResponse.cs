using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AYShop.DAL.DTO.Responses
{
    public class BrandResponse
    {
        public string Name { get; set; }
        [JsonIgnore]
        public string MainImage { get; set; }
        public string MainImageURL => $"https://localhost:7101/images/{MainImage}";

    }
}
