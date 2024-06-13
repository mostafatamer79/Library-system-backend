namespace library.Models
{
    public class Book
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(100)]
        public string Author { get; set; }
        [MaxLength(100)]
        public string Genre { get; set; }   
        public float rate { get; set; }
        public string poster { get; set; }
        public int count { get; set; }
        public Boolean status { get; set; }

    }
}
