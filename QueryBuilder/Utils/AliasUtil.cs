using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QueryBuilder.Utils
{
    public class AliasUtil
    {
        public static string GetAlias()
        {
            return "Alias1";
        }

        public static string[] getNameAndAlias(string NameAndAlias )
        {
            string[] ar = null;
            if (NameAndAlias.Trim().Contains(' '))
            {
                ar = NameAndAlias.Split(' ');
                if (ar.Length > 2)
                {
                    throw new ArgumentException("column name contains too many spaces ", "Name");
                }
            
            }
            return ar;

        }
        /// <summary>
        /// generate the next alias of agiven name 
        /// ex: if we passed Alias1 function should return Alias2
        /// </summary>
        /// <param name="Alias"></param>
        /// <returns></returns>
        public static string GetNextAlias(string Alias)
        {
            if (string.IsNullOrEmpty(Alias))
            {
                Alias = GetAlias();
            }
            else
            {
                Regex _Regex = new Regex(@"\d+", RegexOptions.IgnorePatternWhitespace);
                Match _Match = _Regex.Match(Alias);
                if (_Match != null)
                {

                    Alias = _Regex.Replace(Alias, new MatchEvaluator(m =>
                    {
                        int _intValue = int.Parse(m.Value);
                        _intValue += 1;
                        return _intValue.ToString();
                    }));
                }
            }

            return Alias;
        }

    }
}
