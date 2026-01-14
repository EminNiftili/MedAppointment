namespace MedAppointment.DataTransferObjects.UserDtos
{
    public record TraditionalUserRegisterDto
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string FatherName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
