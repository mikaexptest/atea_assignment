using Atea.Data;
using Atea.Data.Entities;
using Atea.Models.DTOs;
using Atea.Models.Requests;
using Atea.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Atea.Services.Services
{
    public class APICallStatusService : IAPICallStatusService
    {
        protected AteaDbContext _context;

        public APICallStatusService(AteaDbContext context)
        {
            _context = context;
        }

        public async Task<APICallStatusDTO> GetApiCallStatusById(int id)
        {
            var apiCallStatus = await _context.APICallStatuses.FirstOrDefaultAsync(u => u.Id == id);
            if (apiCallStatus == null)
                return null;

            var result = new APICallStatusDTO
            {
                Id = apiCallStatus.Id,
                Status = apiCallStatus.Status,
                BlobGuid = apiCallStatus.BlobGuid,
                UtcTimestamp = apiCallStatus.UtcTimestamp
            };

            return result;
        }

        public async Task<IEnumerable<APICallStatusDTO>> ListAPICallStatusesByDate(ListBlobsRequest request)
        {
            //DateRange must be provided.
            if (request.FromDate == null || request.ToDate == null)
            {
                return null;
            }

            var apiCallStatuses = await _context.APICallStatuses.Where(x => x.UtcTimestamp >= request.FromDate && x.UtcTimestamp <= request.ToDate).ToListAsync();
            if (apiCallStatuses == null)
                return null;

            var result = apiCallStatuses.Select(x => APICallStatusMap(x)).AsEnumerable();

            return result;
        }

        public async Task CreateApiCallStatus(APICallStatusDTO apiCallStatus)
        {
            if (apiCallStatus == null)
                return;

            var result = new APICallStatus
            {
                Id = apiCallStatus.Id,
                Status = apiCallStatus.Status,
                BlobGuid = apiCallStatus.BlobGuid,
                UtcTimestamp = apiCallStatus.UtcTimestamp
            };

            _context.APICallStatuses.Add(result);
            await _context.SaveChangesAsync();
        }

        private static APICallStatusDTO APICallStatusMap(APICallStatus apiCallStatus)
        {
            return new APICallStatusDTO
            {
                Id = apiCallStatus.Id,
                Status = apiCallStatus.Status,
                BlobGuid = apiCallStatus.BlobGuid,
                UtcTimestamp = apiCallStatus.UtcTimestamp
            };
        }
    }
}