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
        public string? Group { get; set; }
        public string? Category { get; set; }
        private string? _DataFieldName;
        public string? File { get; }

        public Generator(String dataFieldName, String file)
        {
            this.File = file;
            this._DataFieldName = dataFieldName;
            this.Fetch();
        }

        private void Fetch()
        {
            using (TextFieldParser parser = new TextFieldParser(this.File))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("\n");
                var lines = new List<String>();
                while (!parser.EndOfData)
                {
                    lines.Add(parser.ReadLine());
                }

                foreach (var line in lines)
                {
                    var newline = line.Split(",");
                    if (this._DataFieldName.Equals(newline[0]))
                    {
                        this.Group = newline[1];
                        this.Category = newline[2];
                        this.Editable = ConvertValue(newline[3]);
                        this.Visible = ConvertValue(newline[4]);
                        this.Nullable = ConvertValue(newline[5]);
                        this.Position = int.Parse(newline[6]);
                    }
                }
            }
        }

        private bool ConvertValue(string value)
        {
            if (value.Equals("True"))
            {
                return true;
            }

            return false;
        }
    }
}