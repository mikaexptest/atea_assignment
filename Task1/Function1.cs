using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Atea.Common.Constants;
using Atea.Models.DTOs;
using Atea.Models.Models;
using Atea.Models.Requests;
using Atea.Models.Responses;
using Atea.Services.Interfaces;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Task1
{
    public class Function1
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IAPICallStatusService _apiCallStatusService;
        private static readonly HttpClient httpClient = new HttpClient();

        public Function1(AppSettingsModel appSettings, IAPICallStatusService apiCallStatusService)
        {
            _appSettings = appSettings;
            _apiCallStatusService = apiCallStatusService;
        }

        [FunctionName("Function1")]
        public async Task Run([TimerTrigger("* * * * *")] TimerInfo myTimer, ExecutionContext context, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var blobGuid = Guid.NewGuid();
            var apiCallStatus = new APICallStatusDTO
            {
                Status = true,
                BlobGuid = blobGuid,
                UtcTimestamp = DateTime.Now
            };

            try
            {
                //Call API
                HttpRequestMessage newRequest = new HttpRequestMessage(HttpMethod.Get, AteaConstants.PUBLIC_API_URL);

                //Read Server Response
                HttpResponseMessage response = await httpClient.SendAsync(newRequest);
                var responseData = await response.Content.ReadAsAsync<PublicAPIResponse>();

                //Builds the URI to the blob storage.
                UriBuilder fullUri = new UriBuilder()
                {
                    Scheme = "https",
                    Host = string.Format("{0}.blob.core.windows.net", _appSettings.StorageAccountName),
                    Path = string.Format("{0}/{1}", _appSettings.StorageContainerName, $"{blobGuid}.json"),
                    Query = _appSettings.StorageContainerSASToken
                };

                //Get an instance of BlobClient using the URI.
                var blobClient = new BlobClient(fullUri.Uri, null);

                //Upload stuff in the blob.
                await blobClient.UploadAsync(BinaryData.FromString(JsonConvert.SerializeObject(responseData, Formatting.Indented)));

                //Store Success API call to Table.
                await _apiCallStatusService.CreateApiCallStatus(apiCallStatus);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                //Store Failed API call to Table.
                apiCallStatus.Status = false;
                apiCallStatus.BlobGuid = null;
                await _apiCallStatusService.CreateApiCallStatus(apiCallStatus);
            }
        }

        [FunctionName("ListBlobs")]
        public async Task<IActionResult> List(
           [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "listblobs")] ListBlobsRequest request,
           //HttpRequest req,
           ILogger log
           )
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                var apiCallStatus = await _apiCallStatusService.ListAPICallStatusesByDate(request);

                return new ObjectResult(apiCallStatus)
                {
                    StatusCode = (int?)HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                //return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [FunctionName("GetBlobByGuid")]
        public async Task<IActionResult> GetBlobByGuid(
           [HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "getblobbyguid")] GetBlobByGuidRequest request,
           ILogger log
           )
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                //Builds the URI to the blob storage.
                UriBuilder fullUri = new UriBuilder()
                {
                    Scheme = "https",
                    Host = string.Format("{0}.blob.core.windows.net", _appSettings.StorageAccountName),
                    Path = string.Format("{0}/{1}", _appSettings.StorageContainerName, $"{request.BlobGuid}.json"),
                    Query = _appSettings.StorageContainerSASToken
                };

                //Get an instance of BlobClient using the URI.
                var blobClient = new BlobClient(fullUri.Uri, null);

                //Upload stuff in the blob.
                var result = await blobClient.DownloadContentAsync();

                return new ObjectResult(result.Value.Content.ToString())
                {
                    StatusCode = (int?)HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
                //return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}