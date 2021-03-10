using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoWork.DTO
{
    public class RegisterError
    {
        public string email { get; set; }
        public string password{ get; set; }
    public string confirm { get; set; }
        public string role { get; set; }
        public string phone { get; set; }
        public string name { get; set; }
        public RegisterError()
        {

        }
    }
}