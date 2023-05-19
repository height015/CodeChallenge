using System;

using CodeChallenge.ExternalApiCall;
using CodeChallenge.Service;

namespace WebApplication.Service;


public class RandomUserManager : IRandomUserService
{
    private AppHttpClient _service;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<AppHttpClient> _logger;
    public string MessageInfo { get; private set; }
    public RandomUserManager(IHttpClientFactory httpClientFactory, ILogger<AppHttpClient> logger)
	{
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _service = new AppHttpClient(httpClientFactory, logger);

    }

    public Task<Result> GetRandomUser(string seed)
    {
        throw new NotImplementedException();
    }

    public async Task<Root> List(int page = 0, int results = 0, string seed = null)
    {
        var users = new Root();

        try
        {
            var response = await _service.List(page,results,seed);

            if (response == null || response.Results.Count < 1)
            {
                MessageInfo = string.IsNullOrEmpty(_service.APIMessage.FriendlyMessage) ? "Unable to complete your request! please try again": _service.APIMessage.FriendlyMessage;
                return response;

            }
            return response;
        }
        catch (Exception ex)
        {
            MessageInfo = "Process Error Occured! please try again later";
            return users;
        }
    }
}

