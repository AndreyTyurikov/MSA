namespace UserMS.DTO
{
    public class UserDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime registeredAt { get; set; }
    }
}
