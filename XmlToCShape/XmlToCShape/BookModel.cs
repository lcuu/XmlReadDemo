using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlToCShape
{
    public class BookModel
    {
        public string BookType { set; get; }
        public string BookISBN { set; get; }
        public string BookName { set; get; }
        public string BookAuthor { set; get; }
        public double BookPrice { set; get; }
    }
}
