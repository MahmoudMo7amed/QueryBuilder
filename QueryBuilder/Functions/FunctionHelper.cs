using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QueryBuilder
{
    internal class FunctionFormateReader
    {
        private static string SearchSimpleFunctions(string FunctionName)
        {
            XDocument Doc = XDocument.Parse(GetResourceTextFile());

            IEnumerable<XElement> _Elements = Doc.Descendants("SimpleFunction").Where(w => w.Attribute("Name").Value == FunctionName);
            string _Return = string.Empty;
            if (_Elements.Count() > 0)
            {
                _Return = _Elements.First().Value;
            }
            return _Return;
        }

        private static string GetResourceTextFile()
        {
            string result = string.Empty;
            string filename = "Functions.xml";
            using (Stream stream = typeof(Function).Assembly.
                //GetManifestResourceStream("assembly.folder." + filename))
                       GetManifestResourceStream("QueryBuilder.Functions." + filename))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    result = sr.ReadToEnd();
                }
            }
            return result;
        }

        internal static Function GetSimpleFunction(string FunctionName, string columnName = "*", string Alias = null)
        {
            Function _Return = null;
            string _strFunctionContent = SearchSimpleFunctions(FunctionName);
            if (!string.IsNullOrEmpty(_strFunctionContent))
            {
                string _strFunctionSql = string.Format(_strFunctionContent, columnName);
                _Return = new Function(() => { return _strFunctionSql; }, Alias);
            }
            return _Return;
        }

    }
}
