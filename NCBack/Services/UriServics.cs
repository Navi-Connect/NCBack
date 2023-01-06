using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using NCBack.Filter;

namespace NCBack.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPageUri(PaginationFilter filter, string route)
        {
            var _enpointUri = new Uri(string.Concat(_baseUri, route));

            var modifiedUri =
                QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "CityId", filter.CityId.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "GenderId", filter.GenderId.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "AgeFrom", filter.AgeFrom.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "AgeTo", filter.AgeTo.ToString());
            modifiedUri =
                QueryHelpers.AddQueryString(modifiedUri, "Time", $"{filter.Year}-{filter.Month}-{filter.Date}");
            return new Uri(modifiedUri);
        }
        
    }
}