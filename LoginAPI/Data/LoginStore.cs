using LoginAPI.Models.DTO;

namespace LoginAPI.Data
{
    public static class LoginStore
    {
        public static List<LoginDTO> LoginList = new List<LoginDTO>
            {
                new LoginDTO {Id = 1, Username="Moayad", Password = "NWDXHN5"},
                new LoginDTO {Id = 2, Username="Ahmed", Password = "NECJNC12"}

            };
    }
}
