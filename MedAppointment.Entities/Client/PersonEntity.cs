namespace MedAppointment.Entities.Client
{
    public class PersonEntity : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string FatherName { get; set; } = null!;
        public long? ImageId { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public DateTime BirthDate { get; set; }

        public ImageEntity? Image { get; set; }
        public UserEntity? User { get; set; }
    }
}
