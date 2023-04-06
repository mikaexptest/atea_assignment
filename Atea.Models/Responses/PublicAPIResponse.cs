using System.Collections.Generic;

namespace Atea.Models.Responses
{
    public class PublicAPIResponse
    {
        public PublicAPIResponse()
        {
            Entries = new List<PublicAPIEntriesResponse>();
        }

        public int Count { get; set; }
        public List<PublicAPIEntriesResponse> Entries { get; set; }
    }
}