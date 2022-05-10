using System;
using System.Collections.Generic;
using System.Text;

namespace Ecoba.BasePlugin.Services.Models
{
    public class AddFromExcelResult<T> where T : class
    {
        public T Item { get; set; }
        public bool IsAdded { get; set; }
        public string Message { get; set; }
    }
}
