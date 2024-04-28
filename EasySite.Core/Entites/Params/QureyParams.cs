namespace EasySite.Core.Entites.Params
{
    public class QureyParams
    {
        //public string? Search { get; set; }
        public string? Sort { get; set; }
        //public int? PageIndex { get; set; } = 1;

        public int PageIndex { get; set; } = 1;

        private int pageSize = 25;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 100 ? value = 100 : value; }
        }
        public bool IsPagination { get; set; } = true;

        private string? search;
        public string? Search
        {
            get { return search; }
            set { search = value.ToLower(); }
        }
    }
}
