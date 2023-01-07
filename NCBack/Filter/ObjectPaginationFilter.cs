namespace NCBack.Filter
{
    public class ObjectPaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        
        public ObjectPaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }
        public ObjectPaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
    }
}

