using NCBack.Models;

namespace NCBack.Filter
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? CityId { get; set; }
        public int? GenderId { get; set; }
        public int? Year { get; set; } 
        public int? Month { get; set; } 

        public int? Date { get; set; } 

        /*public DateTime? Time { get; set; }*/
      

        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
        }

        public PaginationFilter(int pageNumber, int pageSize, int? cityId, int? genderId,
            int? year, int? month, int? date)
        {
            this.Year = year;
            this.Date = date;
            this.Month = month;
            this.GenderId = cityId;
            this.CityId = genderId;
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }

        /*
        public PaginationFilter(int pageNumber, int pageSize, string? gender)
        {
            this.Gender = gender;
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
        public PaginationFilter( string? city,int pageNumber, int pageSize)
        {
            this.City = city;
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
        }
        */
    }
}