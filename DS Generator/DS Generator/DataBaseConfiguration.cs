namespace DS_Generator;

public class DataBaseConfiguration
{
    public string ConnectionString { get; set; }
    public string Schema { get; set; }
}

public class DatabaseSwitcher
{
    public static void SwitchDatabase(DataBaseConfiguration configuration)
    {
        // Use the configuration to switch the database connection and schema
        // Implement logic to switch the database connection and schema
        // For example, update a static connection string variable or use a connection pool
    }
}
