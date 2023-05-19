using System;
using CodeChallenge.ExternalApiCall;

namespace CodeChallenge.Service;

public interface IRandomUserService
{
    //Task<int> Add(RegDownloadCountObj regObj);
    Task<Root> List(int page=0, int results =0, string seed = null);
    Task<Result> GetRandomUser(string seed);
}

