using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ecoba.BasePlugin.Data.Models
{
    public class PluginConfig
    {
        [Key]
        public string Key { get; set; }
        public string Json { get; set; }
    }
}
