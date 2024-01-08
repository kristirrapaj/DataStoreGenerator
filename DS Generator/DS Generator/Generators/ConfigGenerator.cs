using DataStore.Interface;

namespace DS_Generator.Generators;

internal class ConfigGenerator
{
    public static void Entry()
    {
        var destPath = @"D:\Cristian\Generated FieldsConfigFiles";

        if (!System.IO.Directory.Exists(destPath))
        {
            Console.WriteLine("Directory '" + destPath + "' non presente nel sistema ");
            Console.ReadLine();
            return;
        }

        string[] tables = new string[] { "EXT_MAP_NOTE_POINT", "EXT_MAP_NOTE_LINE", "EXT_MAP_NOTE_POLY" };
        GenerateGridConfigs(destPath, tables);

        //string destPath = @"E:\Lavoro\STMap\STMapClient\src\Server\ConfigGenerator\GridConfigGenerator\XmlSrc\Modello_Logico.xml";
        //GenerateModel(destPath);

        Console.WriteLine("Configurazioni create in '" + destPath + "'");
        Console.ReadLine();
    }

    /*private static void GenerateModel(string destPath)
    {
        // Parametri per la generazione del modello di DSSOpRete - Acea 2.0
        var connStr = "Data Source=VMORA11IT:1521/ora11r2pbas;User Id=retepa;Password=retepa";
        var schema = "retepa";
        IDataStore dataStore = global::DataStore.Oracle.STGeomDataStore.OracleSTGeomDataStore(connStr: connStr, schema: schema);

        SqlXMLModelBuilder builder = new ModelBuilder.SqlXMLModelBuilder(dataStore);

        dsERModel ds = builder.GetModel();

        ds.WriteXml(destPath);
    }*/

    private static void GenerateGridConfigs(string destPath, string[] tables)
    {
        // Parametri per la generazione del modello di DSSOpRete - Acea 2.0
        //string connStr = "Data Source=VMORA11IT:1521/ora11r2pbas;User Id=retepa;Password=retepa";
        //string connStr = "Data Source=vmoracle18ent:1521/ora18dev;User Id=sde;Password=sde";
        //string schema = "SDE";
        // IDataStore dataStore = new DataStore.Oracle.STGeomDataStore.OracleSTGeomDataStore(connStr: connStr, schema: schema);
        // dataStore.DataProviderType = "ORACLE";
        //string[] tables = new string[] { "ACQ_PERDITA", "ACQ_ZONA_ISPEZ" };

        //string connStr = "Data Source=vmsql2008dev;Initial Catalog=SITGAS_SYSTEM;Persist Security Info=True;User ID=sitgas_rete;Password=sitgas_rete";
        var connStr =
            "Data Source=vmsql2016dev;Initial Catalog=RETEAC;Persist Security Info=True;User ID=reteac;Password=reteac";
        var schema = "RETEAC";
        //string connStr = "Data Source=vmsql2016dev;Initial Catalog=IRIS_ACQUA;Persist Security Info=True;User ID=IRIS_ACQUA;Password=iris_acqua";
        //string schema = "IRIS_ACQUA";
        //string connStr = "Data Source=vmsql2016dev;Initial Catalog=GDB_NETIND;Persist Security Info=True;User ID=sde;Password=sde";
        //string schema = "sde";
        //IDataStore dataStore = new DataStore.SQLServerDataStore.SQLServerGeomDataStore(connStr: connStr, schema: schema);
        //dataStore.DataProviderType = "SQL_SERVER";

        //string connStr = "Data Source=vmsql2016dev;Initial Catalog=SITGAS_RETE;Persist Security Info=True;User ID=SITGAS_RETE;Password=sitgas_rete";
        //string schema = "SITGAS_RETE";

        //string connStr = "Data Source=vmsql2016dev;Initial Catalog=SWMS_BM;Persist Security Info=True;User ID=swms_bm;Password=swms_bm";
        //string schema = "swms_bm";

        IDataStore dataStore = new DataStore.SQLServerDataStore.SQLServerGeomDataStore(connStr, schema);
        dataStore.DataProviderType = "SQL_SERVER";

        var generator = new SqlGenerator(dataStore);

        // string[] tables = new string[] { "PI_CENTRALINA", "PI_LINEA_ELETTRICA", "PI_PUNTO_LUCE", "PI_SOSTEGNO", "PI_SOTTOQUADRO", "PI_ZONA_VERIFICA", "VIEW_RG_STRADA" };
        //string[] tables = new string[] { "AMM_FIUME", "FGN_TRATTAMENTO_PROGETTO", "FGN_ACCUMULO_PROGETTO", "FGN_CONDOTTA_MONITORATA" };
        //string[] tables = new string[] // { "VIEW_TARGET_ACCUMULI", "VIEW_TARGET_ACCUMULI_INADD", "VIEW_TARGET_ACCUMULI_INRETI", "VIEW_TARGET_ACQ_CONDOTTA", "VIEW_TARGET_ADDUTTRICI", "VIEW_TARGET_ADDUTT_COM_SERV", "VIEW_TARGET_ADDUTT_INRETI", "VIEW_TARGET_ADDUTT_TRONCHI", "VIEW_TARGET_COLLETTORI", "VIEW_TARGET_COLLETT_COM_SERV", "VIEW_TARGET_COLLETT_TRONCHI", "VIEW_TARGET_CONDOTTEMARINE", "VIEW_TARGET_DEPURATORI", "VIEW_TARGET_DEPURAT_INCOLL", "VIEW_TARGET_DISTRIBUZIONI", "VIEW_TARGET_DISTRIB_COM_SERV", "VIEW_TARGET_DISTRIB_LOC_SERV", "VIEW_TARGET_DISTRIB_TRONCHI", "VIEW_TARGET_FGN_CONDOTTA", "VIEW_TARGET_FIUMI", "VIEW_TARGET_FIUMI_INPOTAB", "VIEW_TARGET_FIUMI_INRETI", "VIEW_TARGET_FOGNATURE", "VIEW_TARGET_FOGNAT_COM_SERV", "VIEW_TARGET_FOGNAT_LOC_SERV", "VIEW_TARGET_FOGNAT_TRONCHI", "VIEW_TARGET_LAGHI", "VIEW_TARGET_LAGHI_INPOTAB", "VIEW_TARGET_LAGHI_INRETI", "VIEW_TARGET_MARI", "VIEW_TARGET_MARI_INPOTAB", "VIEW_TARGET_MARI_INRETI", "VIEW_TARGET_MARI_POMPE", "VIEW_TARGET_POMPAGGI", "VIEW_TARGET_POMPAGGI_INPOTAB", "VIEW_TARGET_POMPAGGI_INSERBA", "VIEW_TARGET_POMPAGGI_POMPE", "VIEW_TARGET_POTABILIZZATORI", "VIEW_TARGET_POTAB_INCAPTAZ", "VIEW_TARGET_POTAB_INRETI", "VIEW_TARGET_POTAB_POMPE", "VIEW_TARGET_POZZI", "VIEW_TARGET_POZZI_INPOTAB", "VIEW_TARGET_POZZI_INRETI", "VIEW_TARGET_POZZI_POMPE", "VIEW_TARGET_SCARICATORI", "VIEW_TARGET_SCARICAT_INFOG", "VIEW_TARGET_SOLLEVAMENTI", "VIEW_TARGET_SORGENTI", "VIEW_TARGET_SORGENTI_INPOTAB", "VIEW_TARGET_SORGENTI_INRETI" };
        //    { "RG_PDR_DOMANDA_POLY", "VIEW_RG_PDR_DOMANDA"};

        generator.Generate(destPath, tables);
    }
}