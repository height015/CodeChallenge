using System;
namespace CodeChallenge.Domain;

public class QueryParameters
{
    public string SortBy { get; set; }
    public string SortDirection { get; set; }
    public Dictionary<string, string> FilterBy { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}

