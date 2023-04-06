using System;

namespace Atea.Models.DTOs
{
    public class APICallStatusDTO
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public Guid? BlobGuid { get; set; }
        public DateTime UtcTimestamp { get; set; }
    }
}