using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class User : IValidatableObject
    {
        public long Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Id < 0)
                results.Add(new ValidationResult("Specified ID was out of range", new[] { "Id" }));
            if (string.IsNullOrWhiteSpace(userName))
                results.Add(new ValidationResult("User name was not specified", new[] { "userName" }));

            return results;
        }
    }
}
