using System.Xml;
using System;
using System.IO;

namespace SimpleAES 
{
    static class ConfigurationManager
    {
        public static string ConfigurationFilePath = "config.xml";
        public static string Load(string key)
        {
            if (!File.Exists(ConfigurationFilePath))
            {
                TextWriter tw = new StreamWriter(ConfigurationFilePath, true);
                tw.WriteLine("<?xml version='1.0' encoding='utf-8' ?> ");
                tw.WriteLine("<Keys>");
                tw.WriteLine("  <Key >key</Key>");
                tw.WriteLine("  <Vector>vector</Vector>");
                tw.WriteLine("</Keys> ");
                tw.Close();
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(ConfigurationFilePath);
            return Convert.ToString(doc.SelectSingleNode(key).InnerText);
        }
    }
}
