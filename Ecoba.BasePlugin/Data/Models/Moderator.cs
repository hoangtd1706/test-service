using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ecoba.BasePlugin.Data.Models
{
    public class Moderator
    {
        [Key]
        public int Id { get; set; }
        public string UserNumber { get; set; }
        public string Role { get; set; }
    }
}
