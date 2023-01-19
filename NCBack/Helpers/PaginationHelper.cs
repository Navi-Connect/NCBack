using NCBack.Filter;
using NCBack.Models;
using NCBack.Services;
using NCBack.Wrappers;

namespace NCBack.Helpers
{
    public class PaginationHelper
    {
        public static ObjectPageResponse<object> respose;

        /*public static EventPagedResponse <List<Event>> CreatePagedReponse(
           List<Event> pagedData, PaginationFilter validFilter,int totalRecords, IUriService uriService,  string? route)
        {
            respose = new EventPagedResponse <List<Event>>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages)); 
            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                    ? uriService.GetPageUri(new PaginationFilter(
                        validFilter.PageNumber + 1, 
                        validFilter.PageSize), route)
                    : null!;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                    ? uriService.GetPageUri(new PaginationFilter(
                        validFilter.PageNumber - 1, validFilter.PageSize ), route)
                    : null!;
            respose.FirstPage = uriService.GetPageUri(new PaginationFilter(
                1, validFilter.PageSize), route);
            respose.LastPage = uriService.GetPageUri(new PaginationFilter(
                roundedTotalPages, validFilter.PageSize), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }*/

        //AgeFrom = Возраст от
        //AgeTo = Возраст до
        public static object? CreatePagedReponse(object pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
        {
            respose = new ObjectPageResponse<object>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                    ? uriService.GetPageUri(new PaginationFilter(
                        validFilter.PageNumber + 1, 
                        validFilter.PageSize, validFilter.CityId, validFilter.GenderId, validFilter.Year,validFilter.Month,validFilter.Date), route)
                    : null!;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                    ? uriService.GetPageUri(new PaginationFilter(
                        validFilter.PageNumber - 1, validFilter.PageSize, validFilter.CityId, validFilter.GenderId,  validFilter.Year,validFilter.Month,validFilter.Date), route)
                    : null!;
            respose.FirstPage = uriService.GetPageUri(new PaginationFilter(
                1, validFilter.PageSize, validFilter.CityId, validFilter.GenderId,  validFilter.Year,validFilter.Month,validFilter.Date), route);
            respose.LastPage = uriService.GetPageUri(new PaginationFilter(
                roundedTotalPages, validFilter.PageSize, validFilter.CityId, validFilter.GenderId,  validFilter.Year,validFilter.Month,validFilter.Date), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
        
        public static ObjectPageResponse<object> CreatePagedObjectReponse(object pagedData, ObjectPaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
        {
            var respose = new ObjectPageResponse<object>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                    ? uriService.GetPageUriObject(new ObjectPaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
                    : null!;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                    ? uriService.GetPageUriObject(new ObjectPaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
                    : null!;
            respose.FirstPage = uriService.GetPageUriObject(new ObjectPaginationFilter(1, validFilter.PageSize), route);
            respose.LastPage = uriService.GetPageUriObject(new ObjectPaginationFilter(roundedTotalPages, validFilter.PageSize), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
        
        
        /*public static ObjectPageResponse<object> CreatePagedEventReponse(object pagedData , ObjectPaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
        {
            var respose = new ObjectPageResponse<object>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                    ? uriService.GetPageUriObject(new ObjectPaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
                    : null!;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                    ? uriService.GetPageUriObject(new ObjectPaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
                    : null!;
            respose.FirstPage = uriService.GetPageUriObject(new ObjectPaginationFilter(1, validFilter.PageSize), route);
            respose.LastPage = uriService.GetPageUriObject(new ObjectPaginationFilter(roundedTotalPages, validFilter.PageSize), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }*/
        /*public static object? CreatePagedReponseGender(object pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
        {
            respose = new EventPagedResponse<object>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                    ? uriService.GetPageUriGender(new PaginationFilter(
                        validFilter.PageNumber + 1, 
                        validFilter.PageSize, validFilter.Gender), route)
                    : null!;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                    ? uriService.GetPageUriGender(new PaginationFilter(
                        validFilter.PageNumber - 1, validFilter.PageSize, validFilter.Gender), route)
                    : null!;
            respose.FirstPage = uriService.GetPageUriGender(new PaginationFilter(
                1, validFilter.PageSize, validFilter.Gender), route);
            respose.LastPage = uriService.GetPageUriGender(new PaginationFilter(
                roundedTotalPages, validFilter.PageSize, validFilter.Gender), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
        
        public static object? CreatePagedReponseCity(object pagedData, PaginationFilter validFilter, int totalRecords, IUriService uriService, string route)
        {
            respose = new EventPagedResponse<object>(pagedData, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                    ? uriService.GetPageUriCity(new PaginationFilter(
                        validFilter.PageNumber + 1, 
                        validFilter.PageSize, validFilter.City), route)
                    : null!;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                    ? uriService.GetPageUriCity(new PaginationFilter(
                        validFilter.PageNumber - 1, validFilter.PageSize, validFilter.City), route)
                    : null!;
            respose.FirstPage = uriService.GetPageUriCity(new PaginationFilter(
                1, validFilter.PageSize, validFilter.City), route);
            respose.LastPage = uriService.GetPageUriCity(new PaginationFilter(
                roundedTotalPages, validFilter.PageSize, validFilter.City), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }*/
    }

   
}

