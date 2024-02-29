using Newtonsoft.Json;

namespace SportEventsApiServices.Models
{
    public class PaginationResponse<T>
    {
        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; }

        [JsonProperty(PropertyName = "pageNumber")]
        public int PageNumber { get; set; }

        [JsonProperty(PropertyName = "totalPage")]
        public int TotalPage { get; set; }

        [JsonProperty(PropertyName = "totalItem")]
        public int TotalItem { get; set; }
/*
        [JsonProperty(PropertyName = "continuationToken")]
        public string ContinuationToken { get; set; } = null;*/

        [JsonProperty(PropertyName = "items")]
        public List<T> Items { get; set; } = new List<T>();
    }
}
