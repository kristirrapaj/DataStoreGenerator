using System.Data;
using DataStore.Interface;

namespace DS_Generator;

public class DataStore: IDataStore
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public IDataStore Clone()
    {
        throw new NotImplementedException();
    }

    public string DataProviderName { get; set; }
    public string DataProviderType { get; set; }
    public bool IsGeoDatabase { get; set; }
    public string DefaultVersion { get; }
    public string SDEVersion { get; set; }
    public string VarCharParam { get; set; }
    public bool IsUpperFieldInDict(string nameField)
    {
        throw new NotImplementedException();
    }

    public string UpperCaseFields { get; set; }
    public bool CaseSensitive { get; set; }
    public bool HasField(string tableName, string fieldName)
    {
        throw new NotImplementedException();
    }

    public string Database { get; }
    public string Schema { get; }
    public int SRID { get; set; }
    public void SetCurrentVersion()
    {
        throw new NotImplementedException();
    }

    public void StartEdit()
    {
        throw new NotImplementedException();
    }

    public void StopEdit()
    {
        throw new NotImplementedException();
    }

    public bool TranslateGeometries { get; set; }
    public void BeginTransaction(bool editVersion = false, IsolationLevel iLevel = IsolationLevel.ReadCommitted)
    {
        throw new NotImplementedException();
    }

    public void CommitTransaction(bool editVersion = false)
    {
        throw new NotImplementedException();
    }

    public void RollbackTransaction(bool editVersion = false)
    {
        throw new NotImplementedException();
    }

    public void WriteBulk(string destTableName, DataTable tbToWrite, int? timeout = null)
    {
        throw new NotImplementedException();
    }

    public void WriteBulk(string destTableName, DataRow[] rowsToWrite, int? timeout = null)
    {
        throw new NotImplementedException();
    }

    public void UpdateChanges(DataTable tbl, IDbTransaction transaction = null)
    {
        throw new NotImplementedException();
    }

    public void UpdateChanges(DataSet ds, IDbTransaction transaction = null, bool rebuildTableIndexes = false)
    {
        throw new NotImplementedException();
    }

    public void InsertUpdateData(DataTable tbl, IDbTransaction transaction = null)
    {
        throw new NotImplementedException();
    }

    public void InsertUpdateData(DataSet ds, IDbTransaction transaction = null)
    {
        throw new NotImplementedException();
    }

    public List<string> GetTableOrderedList(DataSet ds)
    {
        throw new NotImplementedException();
    }

    public string GetPagedQuery(string query, string sortColumn, int pageIndex, int pageSize)
    {
        throw new NotImplementedException();
    }

    public GeometryErrorEnum LastGetGeometryError { get; }
    public string GetSelectString(string tableName, string[] fields)
    {
        throw new NotImplementedException();
    }

    public string GetViewDefinition(string viewName)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, string> GetViewsDefinitions(string viewsNameFilter)
    {
        throw new NotImplementedException();
    }

    public string FixSpatialQueryOnView(string cmdTxt, string viewName)
    {
        throw new NotImplementedException();
    }

    public string GetTableScript(DataTable table, string tbName, bool createPK = false, bool createSpIndex = false)
    {
        throw new NotImplementedException();
    }

    public string GetCreateTableFromSelectScript(string tableName, string select)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, ColumnDefinition> GetTableDefinition(string tableName, bool useCache = true)
    {
        throw new NotImplementedException();
    }

    public void DisableSDETableTriggers(string tableName)
    {
        throw new NotImplementedException();
    }

    public void EnableSDETableTriggers(string tableName)
    {
        throw new NotImplementedException();
    }

    public string GetEnableAllTriggersScript(string tableName, string tableSchema)
    {
        throw new NotImplementedException();
    }

    public string GetDisableAllTriggersScript(string tableName, string tableSchema)
    {
        throw new NotImplementedException();
    }

    public int GetSDERegistrationID(string tbName)
    {
        throw new NotImplementedException();
    }

    public int GetNewObjectID(string tbName)
    {
        throw new NotImplementedException();
    }

    public int[] GetNewObjectdIDRange(string tbName, int numOids)
    {
        throw new NotImplementedException();
    }

    public string GetNewObjectIDSelectToken(string tbName, string tbAlias)
    {
        throw new NotImplementedException();
    }

    public void UpdateObjectIdGenerationMetadata(string table)
    {
        throw new NotImplementedException();
    }

    public string GetNewGlobalID(string tbName)
    {
        throw new NotImplementedException();
    }

    public string GetNewGlobalIDSelectToken(string tbName, string tbAlias)
    {
        throw new NotImplementedException();
    }

    public string ISNULL_FUNC_NAME { get; }
    public string CONCAT_STRING_KEYWORD { get; }
    public string START_ESCAPE_CHAR { get; }
    public string END_ESCAPE_CHAR { get; }
    public string END_MERGE_CHAR { get; }

    public string GetUpdateScript(string srcTableName, string destTableName, ICollection<DataColumn> updateColumns,
        ICollection<DataColumn> primaryKeycolumns, Dictionary<string, string> colMapping = null, string whereClause = null)
    {
        throw new NotImplementedException();
    }

    public bool IsVersioned(string tbName)
    {
        throw new NotImplementedException();
    }

    public string GetVersionedView(string tableName)
    {
        throw new NotImplementedException();
    }

    public string GetVersionedViewBaseTable(string viewName)
    {
        throw new NotImplementedException();
    }

    public string GetTableName(string entityName)
    {
        throw new NotImplementedException();
    }

    public string GetDateDifferenceExpression(string startDateField, string endDateField, DateDiffPartEnum dateDiffPart)
    {
        throw new NotImplementedException();
    }

    public string GetCurrentDateExpression()
    {
        throw new NotImplementedException();
    }

    public string GetParseDateExpression(string dateExpr, string format)
    {
        throw new NotImplementedException();
    }

    public string GetYearFromDateExpression(string dateField)
    {
        throw new NotImplementedException();
    }

    public string GetBitAndOperationExpression(string fieldName, string fieldValue)
    {
        throw new NotImplementedException();
    }

    public string GetBitOrOperationExpression(string fieldName, string fieldValue)
    {
        throw new NotImplementedException();
    }

    public string IDField { get; set; }
    public int IDFieldLength { get; }
    public int IDFieldPrefixLength { get; }
    public string GeometryField { get; set; }
    public DataColumn GetGeometryColumn(DataTable tbl)
    {
        throw new NotImplementedException();
    }

    public string GetSubstringOperator(string sourceStr, int fromPos, int length)
    {
        throw new NotImplementedException();
    }

    public string GetTrimOperator(string sourceStr)
    {
        throw new NotImplementedException();
    }

    public string GetSDEMetadataTable(string table)
    {
        throw new NotImplementedException();
    }

    public string[] GetExistingTables(string nameFilter = null, string owner = null)
    {
        throw new NotImplementedException();
    }

    public string[] GetExistingViews(string nameFilter = null, string owner = null)
    {
        throw new NotImplementedException();
    }

    public bool ExistsTable(string tableName, string owner = null)
    {
        throw new NotImplementedException();
    }

    public bool ExistsView(string viewName, string owner = null)
    {
        throw new NotImplementedException();
    }

    public string GetDateTextRepresentation(DateTime date)
    {
        throw new NotImplementedException();
    }

    public string GetDateExpression(DateTime date)
    {
        throw new NotImplementedException();
    }

    public string GetDateFromPartsExpression(string yearExpr, string monthExpr, string dayExpr)
    {
        throw new NotImplementedException();
    }

    public string GetDoubleTextRepresentation(double d)
    {
        throw new NotImplementedException();
    }

    public string UCaseExpressionNCS(string expression)
    {
        throw new NotImplementedException();
    }

    public string GetJSONVALUEExpression(string column, string jsonField)
    {
        throw new NotImplementedException();
    }

    public string GetTempTableName()
    {
        throw new NotImplementedException();
    }

    public string GetTempViewName()
    {
        throw new NotImplementedException();
    }

    public void ClearExpiredObjects(int expireDays, string type = null, string objectPrefix = null)
    {
        throw new NotImplementedException();
    }

    public void ClearExpiredTempTables()
    {
        throw new NotImplementedException();
    }

    public void ClearExpiredTempViews()
    {
        throw new NotImplementedException();
    }

    public string WriteTempTable(DataTable tbToWrite, bool createPK = false, bool createSpIndex = false)
    {
        throw new NotImplementedException();
    }

    public string GetTopNRowsQuery(string cmdTxt, int numRows)
    {
        throw new NotImplementedException();
    }

    public string GetEnvelopeComponentScript(EnvelopeComponentEnum component, string shapeField = null, string shapePrefix = null)
    {
        throw new NotImplementedException();
    }

    public DataTable GetTableMetadata(string tableName)
    {
        throw new NotImplementedException();
    }

    public void RebuildIndexOnTable(string tableName, string indexName = null)
    {
        throw new NotImplementedException();
    }

    public void DisableConstraintOnTable(string tableName, string constraintName = null)
    {
        throw new NotImplementedException();
    }

    public void EnableConstraintOnTable(string tableName, string constraintName = null)
    {
        throw new NotImplementedException();
    }

    public bool HasDuplicateValue(string tableName, string fieldName)
    {
        throw new NotImplementedException();
    }

    public bool HasNullValue(string tableName, string fieldName)
    {
        throw new NotImplementedException();
    }
}