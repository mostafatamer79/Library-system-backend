namespace library.Dtos
{
    public class User_Dto
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        [EmailAddress]
        public string email { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        public Boolean isAdmin { get; set; }
        public Boolean Acceptable { get; set; }
    }
}
    