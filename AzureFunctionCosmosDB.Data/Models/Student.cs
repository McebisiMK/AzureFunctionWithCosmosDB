using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AzureFunctionCosmosDB.Data.Models
{
    public class Student
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "firstname")]
        public string FirstName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "lastname")]
        public string LastName { get; set; }

        [Required]
        [JsonProperty(PropertyName = "school")]
        public string School { get; set; }
    }
}
