namespace Meetups.Application.Features.Feed.GetMeetups.Internal;

using System;

public class Request
{
    public PaginationModel Pagination { get; }
    
    public FiltersModel Filters { get; }

    public Request(PaginationModel pagination, FiltersModel filters)
    {
        Pagination = pagination;
        Filters = filters;
    }
}

public class PaginationModel
{
    public int PageNumber { get; }
    
    public int PageSize { get; }
    
    public OrderingOption OrderBy { get; }

    public PaginationModel(int pageNumber, int pageSize, OrderingOption orderBy)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        OrderBy = orderBy;
    }
}

public enum OrderingOption
{
    TopicAlphabetically,
    TopicReverseAlphabetically,
    DurationAscending,
    DurationDescending,
    SignUpsCountAscending,
    SignUpsCountDescending,
}

public class FiltersModel
{
    public Guid? CityId { get; }
    
    public string Search { get; }

    public FiltersModel(Guid? cityId, string search)
    {
        CityId = cityId;
        Search = search;
    }
}