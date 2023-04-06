using System;

namespace Atea.Models.Requests
{
    public class ListBlobsRequest
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}