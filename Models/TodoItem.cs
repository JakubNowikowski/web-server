using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        //public string Name { get; set; }
        //public bool IsComplete { get; set; }

        //public bool XD { get; set; }


        //public long productId { get; set; }
        public string productName { get; set; }
        public string productCode { get; set; }
        public string releaseDate { get; set; }
        public string description { get; set; }
        public float price { get; set; }
        public float starRating { get; set; }
        public string imageUrl { get; set; }

    }
}
