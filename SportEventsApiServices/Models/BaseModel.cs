
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportEventsApiServices.Models
{
    public class BaseModel
    {
        public BaseModel()
        {
            CreatedBy = "System";
            CreatedTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }
        [JsonProperty("createdTime")]
        public DateTime? CreatedTime { get; set; }
        [JsonProperty("lastModifiedBy")]
        public string LastModifiedBy { get; set; } = string.Empty;
        [JsonProperty("lastModifiedTime")]
        public DateTime? LastModifiedTime { get; set; }
        [JsonProperty("activeFlag")]
        public string ActiveFlag { get; set; } = "Y";
    }
}
