using System;

namespace Atea.Data.Entities
{
    public class APICallStatus
    {
        public int Id { get; set; }
        public bool Status { get; set; }
        public Guid? BlobGuid { get; set; }
        public DateTime UtcTimestamp { get; set; }
    }
}