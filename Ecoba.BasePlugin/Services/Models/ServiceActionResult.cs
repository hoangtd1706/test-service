using System;
using System.Collections.Generic;
using System.Text;

namespace Ecoba.BasePlugin.Services.Models
{
    public class ServiceActionResult<T> where T : class
    {
        public T Item { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
    }
}
