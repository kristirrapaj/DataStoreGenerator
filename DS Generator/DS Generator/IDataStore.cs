using System.Data;

namespace DataStore.Interface
{
    
    public interface IDataStore: IDisposable
    {


        /// <summary>
        /// Duplica il datastore (utile per usare parallelismo sulle query
        /// </summary>
        /// <returns></returns>
        IDataStore Clone();

        /// <summary>
        /// DataProvider Name del datastore
        /// </summary>
        string DataProviderName { get; set; }

        /// <summary>
        /// DataProvider Type del datastore
        /// </summary>
        string DataProviderType { get; set; }

        /// <summary>
        /// Indica se il database è un geodatabase
        /// </summary>
        bool IsGeoDatabase { get; set; }

        /// <summary>
        /// Restituisce il nome della versione di default.
        /// </summary>
        string DefaultVersion { get; }

        /// <summary>
        /// Imposta o restituisce la versione SDE sulla quale si vuole lavorare.
        /// </summary>
        string SDEVersion { get; set; }



        /// <summary>
        /// Indica il tipo di parametro varchar da utilizzare (NVarChar oppure VarChar)
        /// </summary>
         string VarCharParam { get; set; }

        /// <summary>
        /// Indica se il namefiled passato è presente nel dizionario dei UpperCaseField ora situati nel datastore.
        /// </summary>
        /// <param name="nameField"></param>
        /// <returns></returns>
        bool IsUpperFieldInDict(string nameField);

        /// <summary>
        /// Indica se il db ha una lista di Upper fields
        /// </summary>
        string UpperCaseFields { get; set; }
        /// <summary>
        /// Indica se il db è case sensitive
        /// </summary>
        bool CaseSensitive { get; set; }

        /// <summary>
        /// Indica se un un oggetto del geodatabase ha un certo campo.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        bool HasField(string tableName, string fieldName);

#warning Las3: Non deve essere esposta a classi al di fuori dei DataStore
        /// <summary>
        /// Restituisce un'istanza della connessione.
        /// </summary>
        //IDbConnection Connection { get; }

        /// <summary>
        /// Restituisce il database della connessione.
        /// </summary>
        string Database { get; }

        /// <summary>
        /// Restituisce lo schema da utilizzare nelle query.
        /// </summary>
        string Schema { get; }

        /// <summary>
        /// Restituisce l'SRID delle geometrie
        /// </summary>
        int SRID { get; set; }

        /// <summary>
        /// Imposta la versione sde corrente nel contesto della connessione.
        /// Apre la connessione se imposta la versione, altrimenti lascia chiusa.
        /// </summary>
        /// <param name="conn"></param>
        void SetCurrentVersion();

        /// <summary>
        /// Inizia una sessione di editing sulla versione indicata nella connessione del context
        /// ha anche l'efetto di SetCurrentVersion
        /// Apre la connessione se imposta la versione, altrimenti lascia chiusa.
        /// </summary>
        /// <param name="conn"></param>
        void StartEdit();

        /// <summary>
        /// Finisce una sessione di editing sulla versione indicata nella connessione del context
        /// ha anche l'efetto di SetCurrentVersion
        /// Apre la connessione se imposta la versione, altrimenti lascia chiusa.
        /// </summary>
        /// <param name="conn"></param>
        void StopEdit();

        /// <summary>
        /// Indicase il DataStore deve trasformare le geometrie in lettura dal tipo nativo del DB a IGeometry
        /// </summary>
        bool TranslateGeometries { get; set; }

        /// <summary>
        /// Avvia una transazione.
        /// </summary>
        void BeginTransaction(bool editVersion = false, IsolationLevel iLevel = IsolationLevel.ReadCommitted);

        /// <summary>
        /// Salva una transazione.
        /// </summary>
        void CommitTransaction(bool editVersion = false);

        /// <summary>
        /// Annulla una transazione.
        /// </summary>
        void RollbackTransaction(bool editVersion = false);


#warning Las3: dobbiamo deprecarla in favore di Save
        /// <summary>
        /// Effettua una bulk copy.
        /// </summary>
        /// <param name="destTableName"></param>
        /// <param name="tbToWrite"></param>
        void WriteBulk(string destTableName, DataTable tbToWrite, int? timeout = null);


        /// <summary>
        /// Effettua una bulk copy.
        /// </summary>
        /// <param name="destTableName"></param>
        /// <param name="rowsToWrite"></param>
        void WriteBulk(string destTableName, DataRow[] rowsToWrite, int? timeout = null);

        /// <summary>
        /// Effettua il salvataggio dei dati nel DataTable.
        /// Esegue le operazionidi Delete/Insert/Update in funzione dello stato delle righe
        /// Se necessario utilizza operazioni Bulk.
        /// </summary>
        void UpdateChanges(DataTable tbl, IDbTransaction transaction = null);

        /// <summary>
        /// Effettua il salvataggio di tutte le tabelle del database 
        /// Se necessario utilizza operazioni Bulk.
        /// Esegue le operazionidi Delete/Insert/Update in funzione dello stato delle righe
        /// Esegue un analisi delle relazioni per determinare l'ordine di aggiornamento
        /// </summary>
        void UpdateChanges(DataSet ds, IDbTransaction transaction = null, bool rebuildTableIndexes = false);

        /// <summary>
        /// Effettua il salvataggio di una tabella in bulk verificando quali righe inserire e quali aggiornare
        /// </summary>
        /// <param name="tbl"></param>
        /// <param name="transaction"></param>
        void InsertUpdateData(DataTable tbl, IDbTransaction transaction = null);

        /// <summary>
        /// Effettua il salvataggio di tutte le tabelle del dataset in bulk verificando quali righe inserire e quali aggiornare.
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="transaction"></param>
        void InsertUpdateData(DataSet ds, IDbTransaction transaction = null);

        /// <summary>
        /// Restituisce una lista di nomi di tabelle in ordine di aggiornamento.
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        List<string> GetTableOrderedList(DataSet ds);


#warning Las3: Non deve essere esposta a classi terze
        /// <summary>
        /// Crea un parametro a partire da nome e valore.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        //IDbDataParameter CreateParameter(string name, object value);

        /// <summary>
        /// Restituisce la query paginata a partire dalla query indicata.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="sortColumn"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        string GetPagedQuery(string query, string sortColumn, int pageIndex, int pageSize);

        /// <summary>
        /// Restituisce l'eventuale errore che si è verificato durante l'ultima chiamata a GetGeometry
        /// </summary>
        GeometryErrorEnum LastGetGeometryError { get; }


#warning Las3: Non deve essere esposta a classi al di fuori dei DataStore
        //object GetDbGeometry(IGeometry geometry);
        

        /// <summary>
        /// Restituisce la stringa da utilizzare per una select a partire dai nomi dei campi.
        /// Effettua gli escape necessari quando i campi coincidono con parole chiave.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        string GetSelectString(string tableName, string[] fields);

        /// <summary>
        /// Restituisce la definizione della view indicata.
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        string GetViewDefinition(string viewName);

        /// <summary>
        /// Restituisce la definizione delle viste che hanno il nome che inizia con viewsNameFilter.
        /// </summary>
        /// <param name="viewNameFilter"></param>
        /// <returns>Dizionario avente come chiave il nome della vista e come valore la sua definizione.</returns>
        Dictionary<string, string> GetViewsDefinitions(string viewsNameFilter);

        /// <summary>
        /// Restituisce la query per correggere un bug di Oracle quando si effettuano query spaziali su view che contengono una UNION ALL.
        /// Gli altri provider dovrebbero restituisce la query esattamente come passata nell'argomento cmdTxt.
        /// </summary>
        /// <param name="cmdTxt"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        string FixSpatialQueryOnView(string cmdTxt, string viewName);

        /// <summary>
        /// Restituisce lo script di creazione della tabella a partire dal DataTable indicato.
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        string GetTableScript(DataTable table, string tbName, bool createPK = false, bool createSpIndex = false);

        /// <summary>
        /// Restituisce lo script per creare una tabella direttamente da una query
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="select"></param>
        /// <returns></returns>
        string GetCreateTableFromSelectScript(string tableName, string select);

        /// <summary>
        /// Restituisce la struttura della tabella indicata leggendo la definizione dal DB.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Dictionary<string, ColumnDefinition> GetTableDefinition(string tableName, bool useCache = true);

        /// <summary>
        /// Disabilita i triggers di una tabella
        /// </summary>
        /// <param name="tableName"></param>
        void DisableSDETableTriggers(string tableName);

        /// <summary>
        /// Abilita i triggers di una tabella
        /// </summary>
        /// <param name="tableName"></param>
        void EnableSDETableTriggers(string tableName);

        /// <summary>
        /// Restituisce lo script per abilitare un triggger.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableSchema"></param>
        /// <returns></returns>
        string GetEnableAllTriggersScript(string tableName, string tableSchema);

        /// <summary>
        /// Restituisce lo script per disabilitare un triggger.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableSchema"></param>
        /// <returns></returns>
        string GetDisableAllTriggersScript(string tableName, string tableSchema);


        /// <summary>
        /// Restituisce il registration id di una tabella del geodatabase
        /// </summary>
        /// <param name="tbName"></param>
        /// <returns></returns>
        int GetSDERegistrationID(string tbName);


        /// <summary>
        /// Restituisce un nuovo objectdi per una tabella
        /// </summary>
        /// <param name="tbName"></param>
        /// <returns></returns>
        int GetNewObjectID(string tbName);

        /// <summary>
        /// Restituisce un range di objectid (min, max) da utilizzare per l'inserimento in una tabella
        /// </summary>
        /// <param name="tbName"></param>
        /// <param name="numOids"></param>
        /// <returns></returns>
        int[] GetNewObjectdIDRange(string tbName, int numOids);

        //Restituisce la clausola select per la generazione di nuovi ObjectId
        string GetNewObjectIDSelectToken(string tbName, string tbAlias);

        /// <summary>
        /// Aggiorna la tabella di metadati per la generazione degli objectid
        /// </summary>
        /// <param name="table"></param>
        void UpdateObjectIdGenerationMetadata(string table);

        //Restituisce un nuovo globalID per una tabella
        string GetNewGlobalID(string tbName);

        //Restituisce la clausola select per la generazione di nuovi GloablID
        string GetNewGlobalIDSelectToken(string tbName, string tbAlias);

        string ISNULL_FUNC_NAME
        {
            get;
        }

        string CONCAT_STRING_KEYWORD
        {
            get;
        }

        string START_ESCAPE_CHAR
        {
            get;
        }

        string END_ESCAPE_CHAR 
        {
            get;
        }

        /// <summary>
        /// Restituisce il carattere di chiusura delle espressioni MERGE.
        /// </summary>
        /// <returns></returns>
        string END_MERGE_CHAR { get; }

        /// <summary>
        /// Restituisce uno script per aggiornare una tabella a partire da un'altra
        /// </summary>
        /// <param name="srcTableName"></param>
        /// <param name="destTableName"></param>
        /// <returns></returns>
        string GetUpdateScript(string srcTableName, string destTableName, ICollection<DataColumn> updateColumns, ICollection<DataColumn> primaryKeycolumns, Dictionary<string, string> colMapping = null, string whereClause = null);

        /// <summary>
        /// Indica se la tabella è versionata.
        /// </summary>
        /// <param name="tbName"></param>
        /// <returns></returns>
        bool IsVersioned(string tbName);

        /// <summary>
        /// Restituisce il nome della view associata alla tabella indicata.
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>        
        string GetVersionedView(string tableName);

        /// <summary>
        /// Restituisce il nome della tabella associata alla view indicata.
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        string GetVersionedViewBaseTable(string viewName);

        /// <summary>
        /// Restituisce il nome della tabella da interrogare per l'entità entityname
        /// In pratica controlla se l'entità è un oggetto versionato ed eventualmente restituisce il nome della view
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        string GetTableName(string entityName);

        /// <summary>
        /// Restituisce l'espressione per ottenere la differenza tra due date espressa nell'unità di misura indicata da dateDiffPart.
        /// </summary>
        /// <param name="startDateField"></param>
        /// <param name="endDateField"></param>
        /// <param name="dateDiffPart"></param>
        /// <returns></returns>
        string GetDateDifferenceExpression(string startDateField, string endDateField, DateDiffPartEnum dateDiffPart);

        /// <summary>
        /// Restituisce l'espressione per ottenere la data corrente.
        /// </summary>
        /// <returns></returns>
        string GetCurrentDateExpression();

        /// <summary>
        /// Restituisce l'espressione per fare il parse di una data secondo un certo formato.
        /// </summary>
        /// <param name="dateExpr"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        string GetParseDateExpression(string dateExpr, string format);

        /// <summary>
        /// Restituisce l'espressione per ottenere l'anno a partire da una data.
        /// </summary>
        /// <param name="dateField"></param>
        /// <returns></returns>
        string GetYearFromDateExpression(string dateField);

        /// <summary>
        /// Restituisce l'espressione per calcolare l'and bit a bit del campo indicato con il valore indicato.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        string GetBitAndOperationExpression(string fieldName, string fieldValue);

        /// <summary>
        /// Restituisce l'espressione per calcolare l'or bit a bit del campo indicato con il valore indicato.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        string GetBitOrOperationExpression(string fieldName, string fieldValue);

        /// <summary>
        /// Restituisce il nome delle PK (da file di configurazione)
        /// </summary>
        string IDField { get; set; }

        /// <summary>
        /// Restituisce la lunghezza del campo chiave primaria
        /// </summary>
        int IDFieldLength { get; }

        /// <summary>
        /// Restituisce la lunghezza del prefisso della chiave primaria
        /// </summary>
        int IDFieldPrefixLength { get; }

        /// <summary>
        /// Restituisce il nome del campo Geometrico predefinito
        /// </summary>
        string GeometryField { get; set; }

        /// <summary>
        /// Restituisce la prima colonna che contiene una colonna geometrica.
        /// Nei datasets le geometrie dovrebbero sempre essere di tipo IGeometry
        /// </summary>
        /// <param name="tbl">Il DataTable</param>
        /// <returns>Il DataColumn se esiste la colonna geometrica, Null se la tabella è vuota o non ha la colonna geometrica</returns>
        DataColumn GetGeometryColumn(DataTable tbl);

        //Restituisce il costrutto substring in funzione del tipo di datastore
        string GetSubstringOperator(string sourceStr, int fromPos, int length);

        //Restituisce il costrutto trim in funzione del tipo di datastore
        string GetTrimOperator(string sourceStr);

        string GetSDEMetadataTable(string table);

        /// <summary>
        /// Restituisce i nomi delle tabelle esistenti nel db applicando un eventuale filtro sui nomi
        /// Per ora sul filtro facciamo inizia per, poi vediamo come estendere
        /// </summary>
        /// <param name="nameFilter"></param>
        /// <returns></returns>
        string[] GetExistingTables(string nameFilter = null, string owner = null);

        /// <summary>
        /// Restituisce i nomi delle view esistenti nel db applicando un eventuale filtro sui nomi
        /// Per ora sul filtro facciamo inizia per, poi vediamo come estendere
        /// </summary>
        /// <param name="nameFilter"></param>
        /// <returns></returns>
        string[] GetExistingViews(string nameFilter = null, string owner = null);

        /// <summary>
        /// Verifica se una tabella esiste nel database
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        bool ExistsTable(string tableName, string owner = null);

        /// <summary>
        /// Verifica se una view esiste nel database
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        bool ExistsView(string viewName, string owner = null);

        /// <summary>
        /// Restituisce la rappresentazione testuale di una data.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        string GetDateTextRepresentation(DateTime date);

        /// <summary>
        /// Restituisce l'espressione corrispondente alla data, da utilizzare nelle clausole where.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        string GetDateExpression(DateTime date);

        /// <summary>
        /// Restituisce l'espressione per la creazione di una data a partire dai suoi componenti
        /// </summary>
        /// <param name="yearExpr"></param>
        /// <param name="monthExpr"></param>
        /// <param name="dayExpr"></param>
        /// <returns></returns>
        string GetDateFromPartsExpression(string yearExpr, string monthExpr, string dayExpr);

        /// <summary>
        /// Restituisce la rappresentazione testuale di un double.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        string GetDoubleTextRepresentation(double d);

        /// <summary>
        /// Trasforma un espressione in ucase per usarla in una query in modalità non case sensitive.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        string UCaseExpressionNCS(string expression);

        /// <summary>
        /// Restituisce un espressione per estrarre un valore da un campo di tipo JSON
        /// </summary>
        /// <param name="column"></param>
        /// <param name="jsonField"></param>
        /// <returns></returns>
        string GetJSONVALUEExpression(string column, string jsonField);


        #region Tabelle Temporanee 

        string GetTempTableName();

        string GetTempViewName();

        void ClearExpiredObjects(int expireDays, string type = null, string objectPrefix = null);

        void ClearExpiredTempTables();

        void ClearExpiredTempViews();

        #endregion Tabelle Temporanee 

        /// <summary>
        /// Scrive la tabella indicata in una tabella temporanea. Restituisce il nome della tabella.
        /// </summary>
        /// <param name="tbToWrite"></param>
        /// <returns></returns>
        string WriteTempTable(DataTable tbToWrite, bool createPK = false, bool createSpIndex = false);

        /// <summary>
        /// Restituisce la query per ottenere le prime N righe a partire dal comando passato.
        /// </summary>
        /// <param name="cmdTxt"></param>
        /// <param name="numRows"></param>
        /// <returns></returns>
        string GetTopNRowsQuery(string cmdTxt, int numRows);

        /// <summary>
        /// Restituisce lo script per ottenere la componente dell'envelope indicata.
        /// </summary>
        /// <param name="shapePrefix"></param>
        /// <param name="shapeField"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        string GetEnvelopeComponentScript(EnvelopeComponentEnum component, string shapeField = null, string shapePrefix = null);

        /// <summary>
        /// Restituisce i metadati di una tabella
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        DataTable GetTableMetadata(string tableName);

        /// <summary>
        /// Ricostruisce un indice di una tabella
        /// Se indexName è null ricostruisce tutti gli indici della tabella
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="indexName"></param>
        void RebuildIndexOnTable(string tableName, string indexName = null);

        /// <summary>
        /// Disabilita un constraint di una tabella
        /// Se constraintName è null disabilita tutti i constraints della tabella
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="constraintName"></param>
        void DisableConstraintOnTable(string tableName, string constraintName = null);

        /// <summary>
        /// Abilita un constraint di una tabella
        /// Se constraintName è null abilita tutti i constraints della tabella
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="constraintName"></param>
        void EnableConstraintOnTable(string tableName, string constraintName = null);

        /// <summary>
        /// Verifica se nella tabella tableName alla colonna fieldName c'è almeno un valore duplicato
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        bool HasDuplicateValue(string tableName, string fieldName);

        /// <summary>
        /// Verifica se nella tabella tableName alla colonna fieldName c'è almeno un valore null o vuoto
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        bool HasNullValue(string tableName, string fieldName);
    }

    public enum EnvelopeComponentEnum
    {
        MinX,
        MinY,
        MaxX,
        MaxY
    }

    public enum GeometryErrorEnum
    {
        /// <summary>
        /// Nessun errore.
        /// </summary>
        NoError,
        /// <summary>
        /// Uno o più elementi curvi sono stati ignorati nella generazione della geometria.
        /// </summary>
        HasCurve
    }

    public class ColumnDefinition
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public string ProviderDataType { get; set; }
        public int DataLength { get; set; }
        public int DataPrecision { get; set; }
        public int DataScale { get; set; }
        public bool Nullable { get; set; }
        public bool IsPK { get; set; }
    }

    public enum DateDiffPartEnum
    {
        Seconds,
        Minutes,
        Hours,
        Days
    }

}
