using NCBack.Filter;

namespace NCBack.Services
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
        public Uri GetPageUriObject(ObjectPaginationFilter filter, string route);
    }
}