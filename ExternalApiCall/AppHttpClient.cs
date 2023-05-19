using CodeChallenge.Helpers;
using Newtonsoft.Json;

namespace CodeChallenge.ExternalApiCall;


public class AppHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    public APIResponseMessage APIMessage { get; protected set; }
    private readonly ILogger<AppHttpClient> _logger;
    //private const string end_point= $"";

    public AppHttpClient(IHttpClientFactory httpClientFactory, ILogger<AppHttpClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

 
    public async Task<Root> List(int page = 0, int results = 0, string seed = null)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("YourApiClient");
            var response = await httpClient.GetAsync($"?page={page}&results={results}&seed={seed}");


            if (!response.IsReponseValid(out var msg))
            {
                return null;
            }

            var rawResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine(rawResponse);

            using var respContent = response.Content;
            string json = await respContent.ReadAsStringAsync().ConfigureAwait(false);

            using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var streamReader = new StreamReader(stream);
            using var jsonTextReader = new JsonTextReader(streamReader);

            var responseObj = JsonConvert.DeserializeObject<Root>(json);

            if (responseObj == null || responseObj.Results.Count < 1)
            {
                _logger.LogError("Load List", $"Response: {response.StatusCode}", response.Content.ReadAsStream().ToString());
                APIMessage.TechnicalMessage = $"Empty / Invalid Deserialized Message";
                return null;
            }

            if (responseObj.Info == null)
            {
                APIMessage.FriendlyMessage = "Unable to load items! Please try again later";
                APIMessage.TechnicalMessage = $"Unable to load items! Please try again later";
                return null;
            }


            return responseObj;

        }

        catch (Exception ex)
        {
            _logger.LogError("Load List", $"Response: {ex.Message}");
            APIMessage.FriendlyMessage = "Process Error Occurred! Please try again later";
            APIMessage.TechnicalMessage = $"Error: {ex.GetBaseException().Message}";
            return null;
        }
    }
 

}

