using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace XmlToCShape
{
    //  1.XmlDocument方法优点是便于查找
    //  2.XmlTextReader方法是流读取内存占用少
    //  3.Linq to Xml 最新方法也是推荐方法，代码少易于理解

    public static class ReadXml
    {
        public const string xmlPath = @"../../demo.xml";

        #region 方法一：1.使用 XmlDocument(DOM模式)
        public static List<BookModel> ReadByXmlDocument()
        {
            List<BookModel> bookModeList = new List<BookModel>();

            // 首先声明一个XmlDocument对象,然后调用Load方法,从指定的路径加载XML文件.
            XmlDocument doc = new XmlDocument();
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;//忽略文档里面的注释

            using (XmlReader reader = XmlReader.Create(xmlPath, settings))
            {
                doc.Load(reader);
                // doc.Load(@"d:/demo.xml");
                // 然后可以通过调用SelectSingleNode得到指定的结点,通过GetAttribute得到具体的属性值.参看下面的代码
                // 得到根节点bookstore
                XmlNode xn = doc.SelectSingleNode("bookstore");
                // 得到根节点的所有子节点
                XmlNodeList xnl = xn.ChildNodes;

                foreach (XmlNode xn1 in xnl)
                {
                    BookModel bookModel = new BookModel();
                    // 将节点转换为元素，便于得到节点的属性值
                    XmlElement xe = (XmlElement)xn1;
                    // 得到Type和ISBN两个属性的属性值
                    bookModel.BookISBN = xe.GetAttribute("ISBN").ToString();
                    bookModel.BookType = xe.GetAttribute("Type").ToString();
                    // 得到Book节点的所有子节点
                    XmlNodeList xnl0 = xe.ChildNodes;
                    bookModel.BookName = xnl0.Item(0).InnerText;
                    bookModel.BookAuthor = xnl0.Item(1).InnerText;
                    bookModel.BookPrice = Convert.ToDouble(xnl0.Item(2).InnerText);

                    bookModeList.Add(bookModel);
                }
            }
            bookModeList.Add(new BookModel());

            return bookModeList;
        }

        /// <summary>
        /// 向文件中添加新的数据的时候,
        /// 首先也是通过XmlDocument加载整个文档,
        /// 然后通过调用SelectSingleNode方法获得根结点,
        /// 通过CreateElement方法创建元素,
        /// 用CreateAttribute创建属性,
        /// 用AppendChild把当前结点挂接在其它结点上,
        /// 用SetAttributeNode设置结点的属性.具体代码如下:
        /// </summary>
        public static void XmlDocumentCreateInsertDemo()
        {
            //加载文件并选出要结点:
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.SelectSingleNode("bookstore");
            //创建一个结点,并设置结点的属性:
            XmlElement xelKey = doc.CreateElement("test");
            XmlAttribute xelType = doc.CreateAttribute("Type");
            xelType.Value = "testvalue";
            xelKey.SetAttribute("Type", "testvalue2");
            xelKey.SetAttributeNode(xelType);
            //注释节点
            XmlComment xco = doc.CreateComment("这是注释");
            //内容节点
            XmlElement xelAuthor = doc.CreateElement("author");
            xelAuthor.InnerText = "sss";
            xelKey.AppendChild(xelAuthor);
            xelKey.AppendChild(xco);
            root.AppendChild(xelKey);
            doc.Save(xmlPath);
        }

        /// <summary>
        /// 想要删除某一个结点,直接找到其父结点,
        /// 然后调用RemoveChild方法即可,现在关键的问题是如何找到这个结点,
        /// 上面的SelectSingleNode可以传入一个Xpath表,
        /// 我们通过书的ISBN号来找到这本书所在的结点
        /// </summary>
        public static void XmlDocumentSeachDelUpdateDemo()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNode root = doc.DocumentElement;
            string strPath = string.Format("/bookstore/book[@ISBN=\"{0}\"]", "7-111-19149-6");
            XmlElement selectXe = (XmlElement)root.SelectSingleNode(strPath);
            //删除
            //selectXe.ParentNode.RemoveChild(selectXe);
            selectXe.SetAttribute("Type", "asdfasdf");//也可以通过SetAttribute来增加一个属性
            selectXe.SetAttribute("Typeasdf", "wxcd");//也可以通过SetAttribute来增加一个属性
            selectXe.GetElementsByTagName("title").Item(0).InnerText = "33ee";
            XmlElement appxle = doc.CreateElement("windy");
            appxle.InnerText = "wwws";
            selectXe.AppendChild(appxle);
            doc.Save(xmlPath);
        }

        #endregion


        #region 方法二：2.使用 XmlTextReader（流模式）
        public static List<BookModel> ReadByXmlTextReader()
        {
            XmlTextReader reader = new XmlTextReader(xmlPath);
            List<BookModel> modelList = new List<BookModel>();
            BookModel model = new BookModel();
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "book")
                    {
                        model.BookType = reader.GetAttribute("Type");
                        model.BookISBN = reader.GetAttribute("ISBN");
                    }
                    if (reader.Name == "title")
                    {
                        model.BookName = reader.ReadElementContentAsString();
                    }
                    if (reader.Name == "author")
                    {
                        model.BookAuthor = reader.ReadElementString().Trim();
                    }
                    if (reader.Name == "price")
                    {
                        model.BookPrice = Convert.ToDouble(reader.ReadElementString().Trim());
                    }
                }

                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    modelList.Add(model);
                    model = new BookModel();
                }
            }
            reader.Close();
            modelList.Add(new BookModel());
            return modelList;
        }

        /// <summary>
        /// 然后可以通过WriteStartElement和WriteElementString方法来创建元素,
        /// 这两者的区别就是如果有子结点的元素,那么创建的时候就用WriteStartElement,
        /// 然后去创建子元素,创建完毕后,要调用相应的WriteEndElement来告诉编译器,创建完毕,
        /// 用WriteElementString来创建单个的元素,用WriteAttributeString来创建属性
        /// </summary>
        public static void XmlTextWriterDemo()
        {
            XmlTextWriter myXmlTextWriter = new XmlTextWriter(xmlPath, null);
            //使用 Formatting 属性指定希望将 XML 设定为何种格式。 这样，子元素就可以通过使用 Indentation 和 IndentChar 属性来缩进。
            myXmlTextWriter.Formatting = Formatting.Indented;
            myXmlTextWriter.WriteStartDocument(true);
            myXmlTextWriter.WriteStartElement("root");
            myXmlTextWriter.WriteComment("我测试XML编写");
            myXmlTextWriter.WriteStartElement("person");
            myXmlTextWriter.WriteAttributeString("name", "windy");
            myXmlTextWriter.WriteAttributeString("age", "20");
            myXmlTextWriter.WriteElementString("city", "wuxi");
            myXmlTextWriter.WriteEndElement();
            myXmlTextWriter.WriteEndElement();
            myXmlTextWriter.Flush();
            myXmlTextWriter.Close();
        }
        #endregion


        #region 方法三：3.使用 Linq to Xml（Linq模式）
        public static List<BookModel> ReadByLinqToXml()
        {
            XElement xe = XElement.Load(xmlPath);
            List<BookModel> modelList = new List<BookModel>();
            foreach (var ele in xe.Elements())
            {
                BookModel model = new BookModel();
                model.BookAuthor = ele.Element("author").Value;
                model.BookName = ele.Element("title").Value;
                model.BookPrice = Convert.ToDouble(ele.Element("price").Value);
                model.BookISBN = ele.Attribute("ISBN").Value;
                model.BookType = ele.Attribute("Type").Value;
                modelList.Add(model);
            }
            modelList.Add(new BookModel());

            return modelList;
        }


        /// <summary>
        /// 插入一条数据
        /// </summary>
        public static void InsertByLinqToXml()
        {
            XElement xe = XElement.Load(@"d:/demoLinq.xml");
            XElement record = new XElement(
                new XElement("root",
                new XElement("book",
                new XAttribute("Type", "选修课"),
               new XAttribute("ISBN", "7-111-19149-1"),
               new XElement("title", "计算机操作系统"),
               new XElement("author", "7-111-19149-1"),
               new XElement("price", 28.00))));
            xe.Add(record);
            record.Save(xmlPath);
        }

        /// <summary>
        /// 删除选中的数据
        /// </summary>
        public static void DeleteByLinqToXml()
        {
            XElement xe = XElement.Load(xmlPath);
            string id = "";
            var elements = from ele in xe.Elements("book")
                           where (string)ele.Attribute("ISBN") == id
                           select ele;
            if (elements.Count() > 0)
            {
                elements.First().Remove();
            }
            xe.Save(xmlPath);
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        public static void UpdateByLinqToXml()
        {
            XElement xe = XElement.Load(xmlPath);
            string id = "7-111-19149-1";
            var elements = from ele in xe.Elements()
                           where (string)ele.Attribute("ISBN") == id
                           select ele;
            if (elements.Count() > 0)
            {
                XElement first = elements.First();
                ///设置新的属性
                first.SetAttributeValue("Type", "11212");
                ///替换新的节点
                first.ReplaceNodes(
                         new XElement("title", "3435"),
                         new XElement("author", "55656"),
                         new XElement("price", "9999")
                         );
            }
            xe.Save(xmlPath);
        }

        #endregion
    }
}
