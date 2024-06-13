namespace library.Models
{
    public class BorrowBook
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public User user { get; set; }
        public int userid { get; set; }
        public Book book { get; set; }
        public int bookid { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }   
        public Boolean acceptable { get; set; }
    }
}
