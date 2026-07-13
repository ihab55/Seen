namespace SeenCL.DTOs
{
    public class UserAdminUpdateDTO
    {

        public int UserID { get; set; }
        public string FirstName { get; set; }


        public string LastName { get; set; }


        public string UserName { get; set; }


        public string Email { get; set; }


        public int? Height { get; set; }

        public int? Weight { get; set; }

        public double? FateRate { get; set; }

        public int? DeviceID { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsProfileCompleted { get; set; }
    }
}
