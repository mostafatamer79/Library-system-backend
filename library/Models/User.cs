namespace library.Models
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(100)]
        [EmailAddress]
        public string email { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        public Boolean Acceptable { get; set; }
        public Boolean isAdmin { get; set; }
    }
}
