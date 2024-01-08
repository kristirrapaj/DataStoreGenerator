using System.Data;
using System.Globalization;
using System.IO;
using DataStore.Interface;
using GridConfig.Interface;

namespace DS_Generator.Generators;

public class SqlGenerator
{
    private const string File = "../../../Data 1.csv";
    private IDataStore mDataStore;

    private Dictionary<string, Dictionary<string, string>> mDomainInfoDict;
    private Dictionary<string, string> mSubTypeFieldInfoDict;
    private Dictionary<string, Dictionary<string, string>> mSubTypeInfoDict;
    private Dictionary<string, Dictionary<string, string>> mFieldsInfoDict;
    private Dictionary<string, Dictionary<string, relInfo>> mRelationInfoDict;

    private class relInfo
    {
        public string REL_TB_NAME;
        public string REL_FK_FIELD;
    }

    public SqlGenerator(IDataStore dataStore)
    {
        mDataStore = dataStore;
    }

    public void Generate(string path, string[] tables)
    {
        //tr.TABLE_NAME in ('FGN_CADITOIA','FGN_CONDOTTA','FGN_IMP_SOLLEV','FGN_MISURATORE','FGN_PNT_PREL','FGN_PNT_SCARICO','FGN_PRODUTTIVO','FGN_SFIORO','FGN_TRATTAMENTO')


        //c.TABLE_NAME in ('FGN_CADITOIA','FGN_CONDOTTA','FGN_IMP_SOLLEV','FGN_MISURATORE','FGN_PNT_PREL','FGN_PNT_SCARICO','FGN_PRODUTTIVO','FGN_SFIORO','FGN_TRATTAMENTO')                    
        //AND
        //(
        //c.TABLE_NAME IN
        //(
        //SELECT TABLE_NAME

        //FROM all_tab_cols

        //WHERE COLUMN_NAME = 'IDGIS'
        //)

        //OR

        //c.TABLE_NAME like '%JUNCTIONS%'

        //OR c.TABLE_NAME like 'TA_%'

        //OR c.TABLE_NAME like 'MII_%'

        var getModelQuery = GetModelQuery(tables);


        var tbModel = mDataStore.GetTable(getModelQuery);

        mDomainInfoDict = GetDomainInfoFromDB();
        mSubTypeFieldInfoDict = GetSubtypeFieldInfoFromDB();
        mSubTypeInfoDict = GetSubtypeInfo();
        mFieldsInfoDict = GetFieldsInfo();
        try
        {
            mRelationInfoDict = GetRelationInfo();
        }
        catch
        {
            mRelationInfoDict = null;
        }

        var tbTableNames = tbModel.AsDataView().ToTable(true, new string[] { "TABLE_NAME" });

        foreach (DataRow dr in tbTableNames.Rows)
        {
            var tableName = dr["TABLE_NAME"].ToString();

            GenerateConfigFile(path, tableName, tbModel, false);
            GenerateConfigFile(path, tableName, tbModel, true);
        }
    }

    private string GetModelQuery(string[] tables)
    {
        var getModelQuery = "";

        if (mDataStore.DataProviderType.ToUpper() == "ORACLE")
            getModelQuery =
                @"select c.TABLE_NAME, c.COLUMN_NAME, c.DATA_TYPE, c.DATA_LENGTH, c.DATA_PRECISION, c.DATA_SCALE, c.NULLABLE, tr.IMV_VIEW_NAME
                                from all_tab_cols c 
                                left join SDE.TABLE_REGISTRY tr on c.TABLE_NAME = tr.TABLE_NAME
                                WHERE <TABLE_FILTER>                
                                AND c.COLUMN_NAME NOT IN('SE_ANNO_CAD_DATA')
                                AND c.COLUMN_NAME NOT LIKE('SYS_%')
                                AND c.TABLE_NAME NOT LIKE 'SYS_USER_FLAGS%'
                                AND c.DATA_TYPE NOT IN
                                ('SDO_ELEM_INFO_ARRAY', 'SDO_ORDINATE_ARRAY')
                                order by c.TABLE_NAME, c.COLUMN_NAME";
        else if (mDataStore.DataProviderType.ToUpper() == "SQL_SERVER")
            getModelQuery =
                @"select c.TABLE_NAME, c.COLUMN_NAME, c.DATA_TYPE, c.CHARACTER_MAXIMUM_LENGTH as DATA_LENGTH, c.NUMERIC_PRECISION as DATA_PRECISION, c.NUMERIC_SCALE as DATA_SCALE, SUBSTRING(c.IS_NULLABLE, 1, 1) as NULLABLE, tr.IMV_VIEW_NAME
                                from INFORMATION_SCHEMA.COLUMNS c
                                left join SDE.sde_TABLE_REGISTRY tr on c.TABLE_NAME = tr.TABLE_NAME
                                WHERE
                                <TABLE_FILTER>     
                                AND c.COLUMN_NAME NOT IN('SE_ANNO_CAD_DATA')
                                AND c.COLUMN_NAME NOT LIKE('SYS_%')
                                AND c.TABLE_NAME NOT LIKE 'SYS_USER_FLAGS%'
                                order by c.TABLE_NAME, c.COLUMN_NAME";

        var tablefilter = "";

        if (tables != null && tables.Length > 0)
            tablefilter = "(c.TABLE_NAME IN ('" + tables.Aggregate((s1, s2) => s1 + "' , '" + s2) + "'))";

        getModelQuery = getModelQuery.Replace("<TABLE_FILTER>", tablefilter);

        return getModelQuery;
    }
    
    private void GenerateConfigFile(string path, string tableName, DataTable tbModel, bool buildEditConfig)
    {
        var dbTableName = mDataStore.Schema.ToUpper() + "." + tableName;

        if (mDataStore.DataProviderType == "SQL_SERVER") dbTableName = mDataStore.Database + "." + dbTableName;

        var modelRows = tbModel.Select("TABLE_NAME = '" + tableName + "'");

        var colSettingsDS = new ColumnSettingsDS();

        AddFlagsRow(colSettingsDS);

        var idx = 0;

        foreach (var modelRow in modelRows)
        {
            var columnName = modelRow["COLUMN_NAME"].ToString();
            if (columnName == "FLAGS" || columnName == "GDB_GEOMATTR_DATA") continue;
            var aliasName = columnName;
            if (mFieldsInfoDict.ContainsKey(tableName) && mFieldsInfoDict[tableName].ContainsKey(columnName))
                aliasName = mFieldsInfoDict[tableName][columnName];

            var dataType = modelRow["DATA_TYPE"].ToString().ToUpper();
            var dataLength = modelRow.IsNull("DATA_LENGTH") ? -1 : Convert.ToInt32(modelRow["DATA_LENGTH"]);
            var dataPrecision = modelRow.IsNull("DATA_PRECISION") ? -1 : Convert.ToInt32(modelRow["DATA_PRECISION"]);
            var dataScale = modelRow.IsNull("DATA_SCALE") ? -1 : Convert.ToInt32(modelRow["DATA_SCALE"]);
            var nullable = modelRow["NULLABLE"].ToString();
            var isVersioned = !modelRow.IsNull("IMV_VIEW_NAME");

            var colSettingsRow = colSettingsDS.COLUMN_SETTING.NewCOLUMN_SETTINGRow();


            idx++;
            var position = idx;
            var vis = GetColVisibility(columnName);

            var edit = GetColEditable(columnName) && vis;

            var keyField = IsKeyField(columnName);

            var generator = new Generator(columnName, File);

            colSettingsRow.DATA_FIELD_NAME = columnName;
            colSettingsRow.TYPE = GetCSharpType(dataType, dataPrecision, dataScale);
            colSettingsRow.CAPTION = GetAlias(columnName, aliasName);
            colSettingsRow.ORDINAL = idx;
            colSettingsRow.SHOW_IN_INFO = vis;
            colSettingsRow.POSITION = generator.Position;
            colSettingsRow.VISIBLE = generator.Visible;
            colSettingsRow.EDITABLE = generator.Editable;
            colSettingsRow.NULLABLE = generator.Nullable;
            colSettingsRow.GROUP = generator.Group;
            colSettingsRow.CATEGORY = generator.Category;


            if (IsColToDisable(columnName)) colSettingsRow.DISABLED = true;

            if (keyField) colSettingsRow.SHOW_IN_SUMMARY = true;

            if (columnName == "ID_COMUNE" || columnName == "COD_COMUNE")
            {
                colSettingsRow.IS_LOOKUP = true;

                if (mDataStore.IDField == "IDGIS")
                {
                    colSettingsRow.LOOKUP_TABLE_NAME = "AMM_COMUNE";
                    colSettingsRow.LOOKUP_KEY_FIELD = "CODICE_ISTAT";
                    colSettingsRow.LOOKUP_TEXT_FIELD = "DENOM";
                }
                else
                {
                    colSettingsRow.LOOKUP_TABLE_NAME = "RG_COMUNE";
                    colSettingsRow.LOOKUP_KEY_FIELD = "CODICE_ISTAT";
                    colSettingsRow.LOOKUP_TEXT_FIELD = "DENOM";
                }

                if (!buildEditConfig)
                {
                    colSettingsRow.USE_AUTOCOMPLETE = true;
                }
                else
                {
                    colSettingsRow.USE_AUTOCOMPLETE = false;
                    colSettingsRow.DEPENDENCY_FIELD = "ID_STRADA";
                }
            }
            else if (columnName == "ID_STRADA" || columnName == "COD_STRADA")
            {
                colSettingsRow.IS_LOOKUP = true;

                if (mDataStore.IDField == "IDGIS")
                {
                    if (!buildEditConfig)
                    {
                        colSettingsRow.LOOKUP_KEY_FIELD = "IDGIS";
                        colSettingsRow.LOOKUP_TABLE_NAME = "AMM_STRADA";
                        colSettingsRow.LOOKUP_TEXT_FIELD = "DESCRIZ";
                        colSettingsRow.USE_AUTOCOMPLETE = true;
                    }
                    else
                    {
                        colSettingsRow.LOOKUP_KEY_FIELD = "IDGIS";
                        colSettingsRow.LOOKUP_TABLE_NAME = "VIEW_AMM_STRADA";
                        colSettingsRow.LOOKUP_TEXT_FIELD = "DENOM";
                        colSettingsRow.LOOKUP_FILTER = "GEOM_WITHIN_DISTANCE = 100";
                        colSettingsRow.USE_AUTOCOMPLETE = false;
                    }
                }
                else
                {
                    if (!buildEditConfig)
                    {
                        colSettingsRow.LOOKUP_KEY_FIELD = "RG_COD";
                        colSettingsRow.LOOKUP_TABLE_NAME = "VIEW_RG_STRADA";
                        colSettingsRow.LOOKUP_TEXT_FIELD = "DESCRIZ";
                        colSettingsRow.USE_AUTOCOMPLETE = true;
                    }
                    else
                    {
                        colSettingsRow.LOOKUP_KEY_FIELD = "ID_STRADA";
                        colSettingsRow.LOOKUP_TABLE_NAME = "VIEW_SITGAS_GEO_INFO";
                        colSettingsRow.LOOKUP_TEXT_FIELD = "STRADA";
                        colSettingsRow.LOOKUP_FILTER = "GEOM_WITHIN_DISTANCE = 100";
                        colSettingsRow.USE_AUTOCOMPLETE = false;
                    }
                }
            }
            // gestione domini
            else if (mDomainInfoDict.ContainsKey(dbTableName) && mDomainInfoDict[dbTableName].ContainsKey(columnName))
            {
                var domainName = mDomainInfoDict[dbTableName][columnName];

                colSettingsRow.DOMAIN = domainName;
                colSettingsRow.IS_LOOKUP = true;
                colSettingsRow.LOOKUP_TABLE_NAME = "VIEW_AG_DOMAIN";
                colSettingsRow.LOOKUP_FILTER = "DOMAIN = '" + domainName + "'";
                colSettingsRow.LOOKUP_KEY_FIELD = "CODE";
                colSettingsRow.LOOKUP_TEXT_FIELD = "DESCR";
                colSettingsRow.USE_AUTOCOMPLETE = false;
            }
            else if (mSubTypeFieldInfoDict.ContainsKey(dbTableName))
            {
                // gestione sottotipi
                var subType = mSubTypeFieldInfoDict[dbTableName];

                if (subType == columnName)
                {
                    colSettingsRow.IS_LOOKUP = true;
                    colSettingsRow.LOOKUP_TABLE_NAME = "VIEW_AG_SUBTYPE";
                    colSettingsRow.LOOKUP_FILTER = "FEATURE_CLASS = '" + dbTableName + "'";
                    colSettingsRow.LOOKUP_KEY_FIELD = "CODE";
                    colSettingsRow.LOOKUP_TEXT_FIELD = "DESCR";
                    colSettingsRow.USE_AUTOCOMPLETE = false;
                }
            }

            if (mRelationInfoDict != null && mRelationInfoDict.ContainsKey(columnName))
                if (mRelationInfoDict[columnName].ContainsKey(tableName))
                {
                    colSettingsRow.FK_FIELD = columnName;
                    colSettingsRow.REL_TABLE_NAME = mRelationInfoDict[columnName][tableName].REL_TB_NAME;
                    colSettingsRow.REL_FK_FIELD = mRelationInfoDict[columnName][tableName].REL_FK_FIELD;
                }


            if (colSettingsRow.TYPE == "decimal")
            {
                // Type correction for Ids
                if (colSettingsRow.DATA_FIELD_NAME.StartsWith("ID_"))
                    colSettingsRow.TYPE = "Int32";
                else
                    // formato default
                    colSettingsRow.FORMAT = "{\"decimalPlaces\": 2, \"thousandsSeparator\": \".\"}";
            }

            if (IsMultilineString(columnName, colSettingsRow.TYPE)) colSettingsRow.TYPE = "MultilineString";

            colSettingsDS.COLUMN_SETTING.AddCOLUMN_SETTINGRow(colSettingsRow);
        }

        var fileName = tableName;
        if (buildEditConfig) fileName += "_EDIT";

        colSettingsDS.WriteXml(Path.Combine(path, fileName + "Config.xml"));
    }

    private bool GetColVisibility(string columnName)
    {
        var vis = columnName == "UID" || columnName == "LID" || columnName == "ID"
                  || columnName == "OBJECTID"
                  //|| columnName.StartsWith("ID_") 
                  || columnName.ToUpper() == "SHAPE" || columnName.ToUpper() == "ENABLED"
            ? false
            : true;

        if (columnName == "ID_COMUNE" || columnName == "ID_STRADA"
                                      || columnName == "COD_COMUNE" || columnName == "COD_STRADA")
            vis = true;

        return vis;
    }

    private bool IsColToDisable(string columnName)
    {
        return columnName.ToUpper() == "SHAPE";
    }

    private string GetAlias(string columnName, string aliasName)
    {
        if (aliasName != columnName) return aliasName;

        var textInfo = new CultureInfo("en-US", false).TextInfo;

        var calculatedAlias = columnName.Replace('_', ' ').ToLower();
        calculatedAlias = textInfo.ToTitleCase(calculatedAlias);

        return calculatedAlias;
    }


    private bool GetColEditable(string columnName)
    {
        var edit = true;

        if (columnName == "ID_COMUNE" || columnName == "COD_COMUNE" || columnName == "IDGIS") edit = false;

        return edit;
    }

    /// <summary>
    /// Indica se la colonna è chiave nella tabella
    /// </summary>
    /// <param name="columnName"></param>
    /// <returns></returns>
    private bool IsKeyField(string columnName)
    {
        //Trovare il modo di capire quando un campo è campo chiave
        var isKeyField = false;

        if (columnName == mDataStore.IDField) isKeyField = true;

        return isKeyField;
    }

    private void AddFlagsRow(ColumnSettingsDS ds)
    {
        if (ds.COLUMN_SETTING.FindByDATA_FIELD_NAME("FLAGS") == null)
        {
            var colSettingsRow = ds.COLUMN_SETTING.NewCOLUMN_SETTINGRow();

            colSettingsRow.DATA_FIELD_NAME = "FLAGS";
            colSettingsRow.TYPE = "Int32";
            colSettingsRow.ORDINAL = 0;
            colSettingsRow.CAPTION = "FLAGS";
            colSettingsRow.POSITION = 0;
            colSettingsRow.VISIBLE = false;
            colSettingsRow.SHOW_IN_INFO = false;
            colSettingsRow.EDITABLE = false;
            colSettingsRow.NULLABLE = true;
            colSettingsRow.DISABLED = true;

            ds.COLUMN_SETTING.AddCOLUMN_SETTINGRow(colSettingsRow);
        }
    }

    private bool IsMultilineString(string columnName, string type)
    {
        return type == "string" && (columnName == "NOTE" || columnName == "ANNOTAZIONI");
    }

    private string GetCSharpType(string dataType, decimal dataPrecision, decimal dataScale)
    {
        switch (dataType)
        {
            case "BOOLEAN":
            case "BIT":

                return "Boolean";
            case "CHAR":
            case "NVARCHAR":
            case "VARCHAR":
            case "NVARCHAR2":
            case "VARCHAR2":
            case "CLOB":
            case "NCLOB":
            case "BLOB":
            case "LONG":

                return "string";
            case "TEXT":
            case "NTEXT":

                return "text";
            case "DATE":
            case "SMALLDATETIME":
            case "TIMESTAMP(6)":
            case "DATETIME":
            case "DATETIME2":

                return "DateTime";

            case "NUMBER":
            case "NUMERIC":

                if (dataPrecision == 10 && dataScale == 0)
                    return "Int32";
                else if (dataPrecision == 38 && dataScale == 0)
                    return "Int64";
                else
                    return "decimal";

            case "SMALLINT":
                return "Int16";
            case "INT":
                return "Int32";
            case "BIGINT":
                return "Int64";

            case "FLOAT":
            case "DECIMAL":
                return "decimal";
            case "ST_POINT":
            case "ST_GEOMETRY":
            case "GEOMETRY":

                return "IGeometry";
            case "UNIQUEIDENTIFIER":
                return "string";
        }

        return null;
    }


    private Dictionary<string, Dictionary<string, string>> GetDomainInfoFromDB()
    {
        var tbDomain = mDataStore.GetTable("SELECT * FROM SDE.VIEW_AG_DOMAIN");

        var cmdTxt = "SELECT * FROM SDE.VIEW_AG_DOMAIN_FIELD WHERE DOMAIN_NAME IS NOT NULL";

        var tb = mDataStore.GetTable(cmdTxt);

        var domainInfoDict = new Dictionary<string, Dictionary<string, string>>();

        foreach (DataRow dr in tb.Rows)
        {
            var tbName = dr["TB_NAME"].ToString();

            if (!domainInfoDict.ContainsKey(tbName)) domainInfoDict[tbName] = new Dictionary<string, string>();

            var fieldsDict = domainInfoDict[tbName];

            var fieldName = dr["FIELD_NAME"].ToString();
            var domainName = dr["DOMAIN_NAME"].ToString();

            fieldsDict[fieldName] = domainName;
        }

        return domainInfoDict;
    }

    private Dictionary<string, string> GetSubtypeFieldInfoFromDB()
    {
        var cmdTxt = "SELECT * FROM SDE.VIEW_AG_SUBTYPE_FIELD";

        var tb = mDataStore.GetTable(cmdTxt);

        var subtypeInfoDict = new Dictionary<string, string>();

        foreach (DataRow dr in tb.Rows)
        {
            var tbName = dr["TABLE_NAME"].ToString();
            var subtypeField = dr["SUB_TYPE_FIELD"].ToString();

            subtypeInfoDict[tbName] = subtypeField;
        }

        return subtypeInfoDict;
    }

    private Dictionary<string, Dictionary<string, relInfo>> GetRelationInfo()
    {
        var relationInfoDict = new Dictionary<string, Dictionary<string, relInfo>>();

        var cmdTxt = "SELECT * FROM SDE.RELATION_FIELD_INFO";
        DataTable tb = null;
        try
        {
            tb = mDataStore.GetTable(cmdTxt);
        }
        catch
        {
        }

        if (tb != null)
            foreach (DataRow dr in tb.Rows)
            {
                var fkField = dr["FK_FIELD"].ToString();

                if (!relationInfoDict.ContainsKey(fkField))
                    relationInfoDict[fkField] = new Dictionary<string, relInfo>();

                var fieldsDict = relationInfoDict[fkField];

                var tbName = dr["TB_NAME"].ToString();
                var relTbName = dr["REL_TB_NAME"].ToString();
                var pkField = dr["REL_FK_FIELD"].ToString();

                var rInfo = new relInfo();
                rInfo.REL_TB_NAME = relTbName;
                rInfo.REL_FK_FIELD = pkField;
                if (fieldsDict.ContainsKey(tbName))
                    fieldsDict[tbName] = rInfo;
                else
                    fieldsDict.Add(tbName, rInfo);
            }

        return relationInfoDict;
    }

    private Dictionary<string, Dictionary<string, string>> GetSubtypeInfo()
    {
        var cmdTxt = "SELECT * FROM SDE.VIEW_AG_SUBTYPE";

        var tb = mDataStore.GetTable(cmdTxt);

        var resDict = new Dictionary<string, Dictionary<string, string>>();

        foreach (DataRow dr in tb.Rows)
        {
            var tbName = dr["FEATURE_CLASS"].ToString();
            var tbNameParts = tbName.Split('.');
            tbName = tbNameParts[tbNameParts.Length - 1];

            if (!resDict.ContainsKey(tbName)) resDict[tbName] = new Dictionary<string, string>();

            var code = dr["CODE"].ToString();
            var descr = dr["DESCR"].ToString();

            resDict[tbName].Add(code, descr);
        }

        return resDict;
    }

    private Dictionary<string, Dictionary<string, string>> GetFieldsInfo()
    {
        var cmdTxt = "SELECT * FROM SDE.VIEW_AG_FIELD_INFO";

        var tb = mDataStore.GetTable(cmdTxt);

        var resDict = new Dictionary<string, Dictionary<string, string>>();

        foreach (DataRow dr in tb.Rows)
        {
            var tbName = dr["TABLE_NAME"].ToString();
            var tbNameParts = tbName.Split('.');
            tbName = tbNameParts[tbNameParts.Length - 1];

            if (!resDict.ContainsKey(tbName)) resDict[tbName] = new Dictionary<string, string>();

            var fieldName = dr["FIELD_NAME"].ToString();
            var aliasName = dr["ALIAS_NAME"].ToString();

            resDict[tbName].Add(fieldName, aliasName);
        }

        return resDict;
    }
}