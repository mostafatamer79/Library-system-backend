namespace library.Dtos
{
    public class Borrow_Dto
    {
        public int userid { get; set; }
        public int bookid { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public Boolean acceptable { get; set; }

    }
}
