/*
 * Copyright 2015-2018 Mohawk College of Applied Arts and Technology
 *
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: justin
 * Date: 2018-2-9
 */

/*
 * This product includes software developed by Borland Software Corp.
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SanteDB.Core.Data.Warehouse;
using System.Data;
using SanteDB.Core.Model.Map;
using SanteDB.Core.Diagnostics;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace SanteDB.OrmLite.Providers
{
   /// <summary>
   /// Represents a FirebirdSQL provider
   /// </summary>
   public class FirebirdSQLProvider : IDbProvider
   {

       // Trace source
       private Tracer m_tracer = Tracer.GetTracer(typeof(PostgreSQLProvider));

       // DB provider factory
       private DbProviderFactory m_provider = null;

       // Parameter regex
       private readonly Regex m_parmRegex = new Regex(@"\?");

       /// <summary>
       /// Gets or sets the connection string for the provider
       /// </summary>
       public string ConnectionString { get; set; }

       /// <summary>
       /// Gets the features that this provider 
       /// </summary>
       public SqlEngineFeatures Features
       {
           get
           {
               return SqlEngineFeatures.AutoGenerateTimestamps |
                   SqlEngineFeatures.ReturnedInserts |
                   SqlEngineFeatures.FetchOffset;
           }
       }

       /// <summary>
       /// Gets the name of the provider
       /// </summary>
       public string Name
       {
           get
           {
               return "fbsql";
           }
       }

       /// <summary>
       /// Gets or sets the readonly connection string
       /// </summary>
       public string ReadonlyConnectionString { get; set; }

       /// <summary>
       /// Gets or sets whether SQL tracing is supported
       /// </summary>
       public bool TraceSql { get; set; }

       /// <summary>
       /// Clone a connection
       /// </summary>
       /// <param name="source">The connection context to clone</param>
       /// <returns>The cloned connection</returns>
       public DataContext CloneConnection(DataContext source)
       {
           return source.IsReadonly ? this.GetReadonlyConnection() : this.GetWriteConnection();
       }

       /// <summary>
       /// Convert a value to the specified type
       /// </summary>
       /// <param name="toType">The type to convert to</param>
       /// <param name="value">The value to be converted</param>
       public object ConvertValue(object value, Type toType)
       {
           object retVal = null;
           if (value != DBNull.Value)
               MapUtil.TryConvert(value, toType, out retVal);
           return retVal;
       }

       /// <summary>
       /// Turn the specified SQL statement into a count statement
       /// </summary>
       /// <param name="sqlStatement">The SQL statement to be counted</param>
       /// <returns>The count statement</returns>
       public SqlStatement Count(SqlStatement sqlStatement)
       {
           return new SqlStatement(this, "SELECT COUNT(*) FROM (").Append(sqlStatement.Build()).Append(") Q0");
       }

       /// <summary>
       /// Create a command
       /// </summary>
       public IDbCommand CreateCommand(DataContext context, SqlStatement stmt)
       {
           var finStmt = stmt.Build();

#if DB_DEBUG
           if(System.Diagnostics.Debugger.IsAttached)
               this.Explain(context, CommandType.Text, finStmt.SQL, finStmt.Arguments.ToArray());
#endif 

           return this.CreateCommandInternal(context, CommandType.Text, finStmt.SQL, finStmt.Arguments.ToArray());
       }

       /// <summary>
       /// Create command internally
       /// </summary>
       private IDbCommand CreateCommandInternal(DataContext context, CommandType type, String sql, params object[] parms)
       {

           var pno = 0;

           sql = this.m_parmRegex.Replace(sql, o => $"@parm{pno++}");

           if (pno != parms.Length && type == CommandType.Text)
               throw new ArgumentOutOfRangeException(nameof(sql), $"Parameter mismatch query expected {pno} but {parms.Length} supplied");


           IDbCommand cmd = context.GetPreparedCommand(sql);
           if (cmd == null)
           {
               cmd = context.Connection.CreateCommand();
               cmd.Transaction = context.Transaction;
               cmd.CommandType = type;
               cmd.CommandText = sql;

               if (this.TraceSql)
                   this.m_tracer.TraceVerbose("[{0}] {1}", type, sql);

               pno = 0;
               foreach (var itm in parms)
               {
                   var parm = cmd.CreateParameter();
                   var value = itm;

                   // Parameter type
                   if (value is String) parm.DbType = System.Data.DbType.String;
                   else if (value is DateTime) parm.DbType = System.Data.DbType.DateTime;
                   else if (value is DateTimeOffset) parm.DbType = DbType.DateTimeOffset;
                   else if (value is Int32) parm.DbType = System.Data.DbType.Int32;
                   else if (value is Boolean) parm.DbType = System.Data.DbType.Boolean;
                   else if (value is byte[])
                       parm.DbType = System.Data.DbType.Binary;
                   else if (value is Guid || value is Guid?)
                       parm.DbType = System.Data.DbType.Guid;
                   else if (value is float || value is double) parm.DbType = System.Data.DbType.Double;
                   else if (value is Decimal) parm.DbType = System.Data.DbType.Decimal;
                   else if (value == null) parm.DbType = DbType.Object;
                   // Set value
                   if (itm == null)
                       parm.Value = DBNull.Value;
                   else
                       parm.Value = itm;

                   if (type == CommandType.Text)
                       parm.ParameterName = $"parm{pno++}";
                   parm.Direction = ParameterDirection.Input;

                   if (this.TraceSql)
                       this.m_tracer.TraceVerbose("\t [{0}] {1} ({2})", cmd.Parameters.Count, parm.Value, parm.DbType);


                   cmd.Parameters.Add(parm);
               }

               // Prepare command
               if (context.PrepareStatements && !cmd.CommandText.StartsWith("EXPLAIN"))
               {
                   if (!cmd.Parameters.OfType<IDataParameter>().Any(o => o.DbType == DbType.Object) &&
                       context.Transaction == null)
                       cmd.Prepare();

                   context.AddPreparedCommand(cmd);
               }
           }
           else
           {
               if (cmd.Parameters.Count != parms.Length)
                   throw new ArgumentOutOfRangeException(nameof(parms), "Argument count mis-match");

               for (int i = 0; i < parms.Length; i++)
                   (cmd.Parameters[i] as IDataParameter).Value = parms[i] ?? DBNull.Value;
           }

           return cmd;
       }

       /// <summary>
       /// Create a command
       /// </summary>
       /// <param name="context">The data context to create the command on</param>
       /// <param name="sql">The SQL contents</param>
       /// <param name="parms">The parameter values</param>
       /// <returns>The constructed command</returns>
       public IDbCommand CreateCommand(DataContext context, string sql, params object[] parms)
       {
           return this.CreateCommandInternal(context, CommandType.Text, sql, parms);

       }
       
       /// <summary>
       /// Create SQL keyword
       /// </summary>
       /// <param name="keywordType">The type of keyword</param>
       /// <returns>The SQL equivalent</returns>
       public string CreateSqlKeyword(SqlKeyword keywordType)
       {
           switch (keywordType)
           {
               case SqlKeyword.ILike:
               case SqlKeyword.Like:
                   return "LIKE";
               case SqlKeyword.Lower:
                   return "LOWER";
               case SqlKeyword.Upper:
                   return "UPPER";
               default:
                   throw new ArgumentOutOfRangeException(nameof(keywordType));
           }
       }

       /// <summary>
       /// Create a stored procedure execution 
       /// </summary>
       /// <param name="context">The context of the command</param>
       /// <param name="spName">The stored procedure name</param>
       /// <param name="parms">The parameters to be created</param>
       /// <returns>The constructed command object</returns>
       public IDbCommand CreateStoredProcedureCommand(DataContext context, string spName, params object[] parms)
       {
           return this.CreateCommandInternal(context, CommandType.StoredProcedure, spName, parms);
       }

       /// <summary>
       /// Create an EXISTS statement
       /// </summary>
       /// <param name="sqlStatement">The statement to determine EXISTS on</param>
       /// <returns>The constructed statement</returns>
       public SqlStatement Exists(SqlStatement sqlStatement)
       {
           return new SqlStatement(this, "SELECT CASE WHEN EXISTS (").Append(sqlStatement.Build()).Append(") THEN true ELSE false END");
       }

       /// <summary>
       /// Get provider factory
       /// </summary>
       /// <returns>The FirebirdSQL provider </returns>
       private DbProviderFactory GetProviderFactory()
       {
           if (this.m_provider == null) // HACK for Mono
               this.m_provider = typeof(DbProviderFactories).GetMethod("GetFactory", new Type[] { typeof(String) }).Invoke(null, new object[] { "Fbsql" }) as DbProviderFactory;

           if (this.m_provider == null)
               throw new InvalidOperationException("Missing FirebirdSQL provider");
           return this.m_provider;
       }

       /// <summary>
       /// Get a readonly connection
       /// </summary>
       public DataContext GetReadonlyConnection()
       {
           var conn = this.GetProviderFactory().CreateConnection();
           conn.ConnectionString = this.ConnectionString;
           return new DataContext(this, conn, true);
       }

       public DataContext GetWriteConnection()
       {
           var conn = this.GetProviderFactory().CreateConnection();
           conn.ConnectionString = this.ConnectionString;
           return new DataContext(this, conn, false);
       }

       /// <summary>
       /// Get a lock object for the specified database connection
       /// </summary>
       /// <param name="connection">The connection to lock</param>
       /// <returns>The lock object for the connection</returns>
       public object Lock(IDbConnection connection)
       {
           return new object();
       }

       /// <summary>
       /// Maps the specified data type
       /// </summary>
       /// <param name="type"></param>
       /// <returns></returns>
       public string MapDatatype(SchemaPropertyType type)
       {
           switch(type)
           {
               case SchemaPropertyType.Binary:
                   return "BLOB";
               case SchemaPropertyType.Boolean:
                   return "BOOLEAN";
               case SchemaPropertyType.Date:
                   return "DATE";
               case SchemaPropertyType.TimeStamp:
               case SchemaPropertyType.DateTime:
                   return "TIMESTAMP";
               case SchemaPropertyType.Decimal:
                   return "DECIMAL";
               case SchemaPropertyType.Float:
                   return "FLOAT";
               case SchemaPropertyType.Integer:
                   return "BIGINT";
               case SchemaPropertyType.String:
                   return "VARCHAR(256)";
               case SchemaPropertyType.Uuid:
                   return "UUID";
               default:
                   throw new NotSupportedException($"Schema type {type} not supported by FirebirdSQL provider");
           }
       }

       /// <summary>
       /// Perform a returning command
       /// </summary>
       /// <param name="sqlStatement">The SQL statement to "return"</param>
       /// <param name="returnColumns">The columns to return</param>
       /// <returns>The returned colums</returns>
       public SqlStatement Returning(SqlStatement sqlStatement, params ColumnMapping[] returnColumns)
       {
           if (returnColumns.Length == 0)
               return sqlStatement;
           return sqlStatement.Append($" RETURNING {String.Join(",", returnColumns.Select(o => o.Name))}");

       }
   }
}
