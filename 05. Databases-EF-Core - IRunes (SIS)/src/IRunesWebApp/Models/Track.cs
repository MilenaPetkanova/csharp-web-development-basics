namespace IRunesWebApp.Models
{
    public class Track : BaseEntity<string>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public decimal Price { get; set; }
    }
}