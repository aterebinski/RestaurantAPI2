namespace RestaurantAPI2.Models
{
    public class RestaurantQuery
    {
        public string? SearchPhrase { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
