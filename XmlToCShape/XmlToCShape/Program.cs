using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlToCShape
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-----------------------------------ReadByXmlDocument--------------------------------------");
            //foreach (var item in ReadXml.ReadByXmlDocument())
            //{
            //    Console.WriteLine(item.BookType + "\t"+item.BookISBN+"\t" + item.BookName + "\t" + item.BookPrice + " \t" + item.BookAuthor );
            //}
            //Console.WriteLine("-----------------------------------ReadByXmlTextReader--------------------------------------");
            //foreach (var item in ReadXml.ReadByXmlTextReader())
            //{
            //    Console.WriteLine(item.BookType + "\t"+item.BookISBN+"\t" + item.BookName + "\t" + item.BookPrice + " \t" + item.BookAuthor );
            //}
            //Console.WriteLine("-----------------------------------ReadByLinqToXml--------------------------------------");
            //foreach (var item in ReadXml.ReadByLinqToXml())
            //{
            //    Console.WriteLine(item.BookType + "\t" + item.BookISBN + "\t" + item.BookName + "           \t" + item.BookPrice + "        \t" + item.BookAuthor);
            //}

            ReadXml.XmlTextWriterDemo();
            Console.ReadKey();
        }
    }
}
