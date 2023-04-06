using Atea.Models.DTOs;
using Atea.Models.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Atea.Services.Interfaces
{
    public interface IAPICallStatusService
    {
        Task<APICallStatusDTO> GetApiCallStatusById(int id);
        Task<IEnumerable<APICallStatusDTO>> ListAPICallStatusesByDate(ListBlobsRequest request);
        Task CreateApiCallStatus(APICallStatusDTO apiCallStatus);
    }
}