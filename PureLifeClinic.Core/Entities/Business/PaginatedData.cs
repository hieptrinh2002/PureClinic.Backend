namespace PureLifeClinic.Core.Entities.Business
{
    public class PaginatedData<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalCount { get; set; }

        public PaginatedData(IEnumerable<T> data, int totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }
    }
}
