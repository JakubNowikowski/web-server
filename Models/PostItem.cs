using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class PostItem : IValidatableObject
    {
        public long Id { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
        public string content { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            
            if (userId < 0)
                results.Add(new ValidationResult("Specified user ID was out of range", new[] { "userId" }));

            if (string.IsNullOrWhiteSpace(content))
                results.Add(new ValidationResult("Post was empty", new[] { "content" }));

            return results;
        }
    }
}