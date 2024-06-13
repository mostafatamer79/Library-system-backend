namespace library.Dtos
{
    public class Book_Dto
    {
        public string Title { get; set; }

        public string Author { get; set; }

        public string Genre { get; set; }
        public int rate { get; set; }
        public IFormFile poster { get; set; }
        public bool status { get; set; }
        public int count { get; set; }

    }
}
