namespace CountriesAPI.Models
{
    public class Data<T>
    {
        public List<T> Objects { get; set; }
        public Meta Meta { get; set; }
    }
}
