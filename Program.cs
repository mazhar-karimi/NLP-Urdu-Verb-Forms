
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Task
{
    class Program
    {
        public string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        static void Main(string[] args)
        {
            Program ob = new Program();

            var stemmers = new string[] { "ا", "ی", "ے", "یں", "تا", "تی",
                "تے", "تیں", "نا", "نی", "نے", "نیں", "وں", "یں", "و",
                "ئے", "لیا", "لئے", "والئے",
                "واے","وائی","لیجئے"};

            var verbs = File.ReadAllLines(@"E:\mazhar karimi\UrduVerbList.txt");

            List<string> verbforms = new List<string>();

            foreach (var verb in verbs)
            {
                var aftt = ob.RemoveDiacritics(verb.Trim());

                if (aftt.Trim().EndsWith("نا"))
                {
                    var brk = aftt.Replace("نا", "");

                    string verbformline = String.Empty;
                    verbformline += aftt.Trim() + "\t";

                    verbformline += brk.Trim() + "\t";

                    foreach (var stem in stemmers)
                    {
                        if (stem == "ا".Trim() && brk.EndsWith("ا".Trim()))
                        {
                            var word = "";
                            verbformline += word + "\t";
                        }
                        else if (stem.Trim().StartsWith("ل") && brk.EndsWith("ل".Trim()))
                        {
                            var word = brk.Trim() + " " +stem.Trim();
                            verbformline += word + "\t";
                        }
                        else
                        {
                            var word = brk.Trim() + stem.Trim();
                            verbformline += word + "\t";
                        }
                    }

                    verbforms.Add(verbformline);
                }
            }
            
            File.WriteAllLines(@"E:\mazhar karimi\urduverbswithforms.txt", verbforms, Encoding.UTF8);

            Console.WriteLine();
            Console.ReadKey();
        }

    }
}
