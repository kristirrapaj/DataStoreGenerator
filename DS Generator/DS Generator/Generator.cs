using System.Data;
using System.IO;
using System.Windows.Documents;
using Microsoft.VisualBasic.FileIO;

namespace DS_Generator
{
    public class Generator
    {
        public int Position { get; set; } // 10 - 13 - 15 - NM con offset di 2, da 10 in su
        public bool Visible { get; set; }
        public bool Editable { get; set; }
        public bool Nullable { get; set; }
        public string Group { get; set; } = "";
        public string Category { get; set; } = "";
        private string _dataFieldName;
        public string File { get; }

        public Generator(string dataFieldName = "", String file = "")
        {
            File = file;
            _dataFieldName = dataFieldName;
            Fetch();
        }

        private void Fetch()
        {
            using var parser = new TextFieldParser(File);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters("\n");
            var lines = new List<string>();
            while (!parser.EndOfData)
            {
                try
                {
                    var line = parser.ReadLine();
                    if (line == null)
                    {
                        throw new Exception();
                    }

                    lines.Add(line);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            foreach (var newline in lines.Select(line => line.Split(",")).Where(newline => _dataFieldName.Equals(newline[0])))
            {
                Group = newline[1];
                Category = newline[2];
                Editable = ConvertValue(newline[3]);
                Visible = ConvertValue(newline[4]);
                Nullable = ConvertValue(newline[5]);
                Position = int.Parse(newline[6]);
            }
        }

        private static bool ConvertValue(string value)
        {
            return value.Equals("True");
        }
    }
}