using NCBack.Filter;

namespace NCBack.Services
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
        
    }
}