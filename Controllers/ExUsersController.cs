using Microsoft.AspNetCore.Mvc;
using CodeChallenge.ExternalApiCall;
using CodeChallenge.Service;
using CodeChallenge.Helper;
using System.Net;

namespace WebApplication.Controllers;

public class ExUsersController : ControllerBase
{
    private readonly IRandomUserService _randomUserService;
    private readonly ILogger<ExUsersController> _logger;
    public ExUsersController(IRandomUserService randomUserService, ILogger<ExUsersController>  logger)
	{
        _randomUserService = randomUserService;
        _logger = logger;
    }

    [HttpGet("list")]
    public async Task<IActionResult> _ItemList(int page = 0, int results = 0, string seed = null)
    {

        try
        {
            var retList = (await getUsers(page,results,seed));

            if (retList == null || !retList.Results.Any())
            {
               return BadRequest("list is empty!");
                
            }
            _logger.LogInformation("Fetched on {Time}", DateTime.Now);
            return Ok(retList);
        }
        catch (Exception ex)
        {
 
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, ex.InnerException);
            return BadRequest(new List<Root>());
        }
    }

    [HttpGet("get-user")]
    public async Task<IActionResult> GetUser(int page = 0, int results = 0, string seed = null)
    {

        try
        {
            var retList = (await getUsers(page, results, seed)).Results.Take(1).ToList();

            if (retList == null || !retList.Any())
            {
                return BadRequest("list is empty!");

            }
            _logger.LogInformation("Fetched on {Time}", DateTime.Now);
            return Ok(retList);
        }
        catch (Exception ex)
        {

            _logger.LogError(ex.StackTrace, ex.Source, ex.Message, ex.InnerException);
            return BadRequest(new ApiStatusResponse(HttpStatusCode.BadRequest,
             $"Internal Problem, Pleaase try again"));
         
        }
    }

    private async Task<Root> getUsers(int page = 0, int results = 0, string seed = null)
    {
        try
        {
            var retList = await _randomUserService.List(page,results,seed) ?? new Root();

            if (retList == null || !retList.Results.Any())
            {
                return new Root();
            }

            return retList;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.StackTrace, ex.Source, ex.Message);
            return new Root();
        }
    }
}




