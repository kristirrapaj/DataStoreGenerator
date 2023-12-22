using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DS_Generator.Database;

public class DatabaseManager
{
    public List<DataBaseConfiguration>? LoadDatabaseConfigurations(string jsonFilePath)
    {
        string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
        return JsonConvert.DeserializeObject<List<DataBaseConfiguration>>(jsonContent);
    }
}