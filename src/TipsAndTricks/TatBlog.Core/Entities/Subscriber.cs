using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities
{
    public class Subscriber : IEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime SignUpDate { get; set; }
        public DateTime UnSignUpDate { get; set; }
        public string Reason { get; set; }
        public bool Status { get; set; }// status can signup(true) or unsignup(false)
        public bool Flag { get; set; }
        public string Notes { get; set; }
    }
}
