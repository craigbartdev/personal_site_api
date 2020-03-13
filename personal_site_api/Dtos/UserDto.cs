using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace personal_site_api.Dtos
{
    //used in ModelFactory
    public class UserDto
    {
        public string Url { get; set; }
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
    }
}