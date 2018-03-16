using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Barrage.UI
{
    /***
    * ===========================================================
    * 创建人：yuanj
    * 创建时间：2017/11/09 8:55:09
    * 说明：SharedPreference是一种轻型的数据存储方式，实际上是基于XML文件存储的“key-value”键值对数据。通常用来存储程序的一些配置信息。
    * 其存储在“C:\ProgramData/程序名/shared_prefs目录下。
    * ==========================================================
    * */
    public class SharedPreference
    {
        private string folderName = "shared_prefs";//配置文件夹名称
        private string fileName;
        private string path;//配置文件路径


        public SharedPreference(string fileName = "shaerd.xml")
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = "shaerd.xml";
            }
            if (fileName.LastIndexOf(".xml") == -1)
            {
                fileName += ".xml";
            }
            this.fileName = fileName;
            Init();
        }

        private void Init()
        {
            string appdata = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
            Assembly a = Assembly.GetEntryAssembly();
            path = Path.Combine(appdata, a.GetName().Name);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = Path.Combine(path, folderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, fileName);
            if (!File.Exists(path))
            {
                CreateXML();
            }
        }

        #region 添加
        /// <summary>
        /// 添加键值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;
            if (string.IsNullOrWhiteSpace(value))
                value = "";
            if (Exists("string", key))
            {
                //存在
                UpdateTagAttr("string", key, value);
            }
            else
            {
                CreateTagAttr("string", key, value);
            }
        }
        /// <summary>
        /// 添加键值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, int value)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;
            if (Exists("int", key))
            {
                //存在
                UpdateTagAttr("int", key, value);
            }
            else
            {
                CreateTagAttr("int", key, value);
            }
        }
        /// <summary>
        /// 添加键值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, DateTime value)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;
            if (Exists("DateTime", key))
            {
                //存在
                UpdateTagAttr("DateTime", key, value);
            }
            else
            {
                CreateTagAttr("DateTime", key, value);
            }
        }
        /// <summary>
        /// 添加键值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, double value)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;
            if (Exists("double", key))
            {
                //存在
                UpdateTagAttr("double", key, value);
            }
            else
            {
                CreateTagAttr("double", key, value);
            }
        }
        /// <summary>
        /// 添加键值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, float value)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;
            if (Exists("float", key))
            {
                //存在
                UpdateTagAttr("float", key, value);
            }
            else
            {
                CreateTagAttr("float", key, value);
            }
        }
        /// <summary>
        /// 添加键值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, long value)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;
            if (Exists("long", key))
            {
                //存在
                UpdateTagAttr("long", key, value);
            }
            else
            {
                CreateTagAttr("long", key, value);
            }
        }
        #endregion

        #region 获取值
        public string GetStringValue(string key)
        {
            return ReadByKey("string", key);
        }
        public int GetIntValue(string key)
        {
            int result = 0;
            int.TryParse(ReadByKey("int", key), out result);
            return result;
        }

        public DateTime GetDateTimeValue(string key)
        {
            DateTime result;
            DateTime.TryParse(ReadByKey("DateTime", key), out result);
            return result;
        }
        public double GetDoubleValue(string key)
        {
            double result = 0;
            double.TryParse(ReadByKey("double", key), out result);
            return result;
        }
        public float GetFloatValue(string key)
        {
            float result = 0;
            float.TryParse(ReadByKey("float", key), out result);
            return result;
        }
        public long GetLongValue(string key)
        {
            long result = 0;
            long.TryParse(ReadByKey("long", key), out result);
            return result;
        }
        #endregion


        /// <summary>
        /// 添加属性节点
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void CreateTagAttr(string tag, string key, object value)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlElement root = doc.DocumentElement;
            AddXmlNode(doc, root, tag, key, value);
            doc.Save(path);
        }

        /// <summary>
        /// 更新节点属性值
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void UpdateTagAttr(string tag, string key, object value)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlElement root = doc.DocumentElement;
            XmlNodeList list = root.GetElementsByTagName(tag);
            foreach (XmlElement item in list)
            {
                string name = item.GetAttribute("name");
                if (name == key)
                {
                    item.InnerText = value.ToString();
                    doc.Save(path);
                    break;
                }
            }
        }

        private string ReadByKey(string tag, string key)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlElement root = doc.DocumentElement;
            XmlNodeList list = root.GetElementsByTagName(tag);
            foreach (XmlElement item in list)
            {
                string name = item.GetAttribute("name");
                if (name == key)
                {
                    return item.InnerText;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 判断是否存在某个节点
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool Exists(string tag, string key)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlElement root = doc.DocumentElement;
            XmlNodeList list = root.GetElementsByTagName(tag);
            foreach (XmlElement item in list)
            {
                string name = item.GetAttribute("name");
                if (name == key)
                {
                    return true;
                }
            }
            return false;
        }


        private void AddXmlNode(XmlDocument doc, XmlElement parent, string tag, string name, object value)
        {
            //创建节点（二级）
            XmlElement node = doc.CreateElement(tag);
            node.SetAttribute("name", name);
            node.InnerText = value.ToString();
            parent.AppendChild(node);
        }

        private void CreateXML()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", "yes");
                doc.AppendChild(dec);
                //创建一个根节点（一级）
                XmlElement root = doc.CreateElement("map");
                doc.AppendChild(root);
                doc.Save(path);
            }
            catch (System.UnauthorizedAccessException e)
            {
                throw;
            }
        }

    }
}
