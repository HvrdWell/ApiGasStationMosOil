using System;
namespace MosOilConn
{
	public class UserDTO
	{
        public int idUser { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int idCard { get; set; }
        public string phoneNumber { get; set; }
        public string role { get; set; }
        public string authToken { get; set; }
    }
}

