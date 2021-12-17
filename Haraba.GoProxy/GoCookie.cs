namespace Haraba.GoProxy
{
    public class GoCookie
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }
        public string Domain { get; set; }
        public bool Secure { get; set; }
        public bool HTTPOnly { get; set; }

        public System.Net.Cookie ToNetCookie()
        {
            return new()
            {
                Name = Name,
                Value = Value,
                Path = Path,
                Domain = Domain,
                Secure = Secure,
                HttpOnly = HTTPOnly
            };
        }
    }
}