using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Configuration;

namespace Delta.PECS.WebCSC.DBUtility
{
    /// <summary>
    /// Represents an installer helper
    /// </summary>
    public partial class InstallerHelper
    {
        /// <summary>
        /// Checks if the connection string is set
        /// </summary>
        /// <returns></returns>
        public static bool ConnectionStringIsSet() {
            return !String.IsNullOrEmpty(ConnectionString) && !String.IsNullOrEmpty(HisConnectionString);
        }

        /// <summary>
        /// Check if the lsc objects is set
        /// </summary>
        public static bool LscDataIsSet() {
            return LscDataExists();
        }

        /// <summary>
        /// Gets or sets the connection string that is used to connect to the storage
        /// </summary>
        public static string ConnectionString {
            get { return SqlHelper.ConnectionStringLocalTransaction; }
        }

        /// <summary>
        /// Gets or sets the connection string that is used to connect to the storage
        /// </summary>
        public static string HisConnectionString {
            get { return SqlHelper.HisConnectionStringLocalTransaction; }
        }

        /// <summary>
        /// Redirects user to the installation page
        /// </summary>
        public static void RedirectToInstallationPage(string[] parms) {
            if (HttpContext.Current != null) {
                var thisPage = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
                if (!thisPage.ToLower().Contains("installer.aspx")) {
                    if (parms != null && parms.Length > 0)
                        HttpContext.Current.Response.Redirect(String.Format("~/Install/Installer.aspx?_ct={0}&{1}", DateTime.Now.Ticks, String.Join("&", parms)), true);
                    else
                        HttpContext.Current.Response.Redirect(String.Format("~/Install/Installer.aspx?_ct={0}", DateTime.Now.Ticks), true);
                }
            }
        }

        /// <summary>
        /// Tests the given connection parameters
        /// </summary>
        /// <param name="trustedConnection">Avalue that indicates whether User ID and Password are specified in the connection (when false) or whether the current Windows account credentials are used for authentication (when true)</param>
        /// <param name="serverName">The name or network address of the instance of SQL Server to connect to</param>
        /// <param name="databasePort">The port of the database associated with the connection</param>
        /// <param name="databaseName">The name of the database associated with the connection</param>
        /// <param name="userName">The user ID to be used when connecting to SQL Server</param>
        /// <param name="password">The password for the SQL Server account</param>
        /// <returns>Returns true if an attempt to open the database by using the connection succeeds.</returns>
        public static string TestConnection(bool trustedConnection, string serverName, int databasePort, string databaseName, string userName, string password) {
            try {
                var connectionString = CreateConnectionString(trustedConnection, serverName, databasePort, databaseName, userName, password, 10);
                using (var conn = new SqlConnection(connectionString)) { conn.Open(); }
                return String.Empty;
            } catch (Exception ex) { return ex.Message; }
        }

        /// <summary>
        /// Creates a database on the server.
        /// </summary>
        /// <param name="databaseName">Database name</param>
        /// <param name="connectionString">Connection string</param>
        /// <param name="filePath">Database file path</param>
        /// <returns>Error</returns>
        public static string CreateDatabase(string databaseName, string connectionString, string filePath) {
            try {
                var query = String.Format(@"CREATE DATABASE [{0}]", databaseName);
                if (!String.IsNullOrEmpty(filePath)) {
                    filePath = filePath.Trim();
                    if (!Directory.Exists(filePath)) { Directory.CreateDirectory(filePath); }
                    query = String.Format(@"CREATE DATABASE [{0}] ON PRIMARY (NAME = N'{0}',FILENAME = N'{1}\{0}.mdf') LOG ON (NAME = N'{0}_log',FILENAME = N'{1}\{0}_log.ldf')", databaseName, filePath);
                }

                using (var conn = new SqlConnection(connectionString)) {
                    SqlHelper.ExecuteNonQuery(conn, CommandType.Text, query, null);
                    return String.Empty;
                }
            } catch (Exception ex) { return ex.Message; }
        }

        /// <summary>
        /// Checks if the specified database exists, returns true if database exists
        /// </summary>
        /// <param name="trustedConnection">A value that indicates whether User ID and Password are specified in the connection (when false) or whether the current Windows account credentials are used for authentication (when true)</param>
        /// <param name="serverName">The name or network address of the instance of SQL Server to connect to</param>
        /// <param name="databasePort">The port of the database associated with the connection</param>
        /// <param name="databaseName">The name of the database associated with the connection</param>
        /// <param name="userName">The user ID to be used when connecting to SQL Server</param>
        /// <param name="password">The password for the SQL Server account</param>
        /// <returns>Returns true if the database exists.</returns>
        public static bool DatabaseExists(bool trustedConnection, string serverName, int databasePort, string databaseName, string userName, string password) {
            int dbCnt = 0;
            try {
                var connectionString = CreateConnectionString(trustedConnection, serverName, databasePort, "master", userName, password, 120);
                var query = "SELECT Count(1) FROM sysdatabases WHERE name = '" + databaseName.Replace("'", "''") + "'";
                using (var conn = new SqlConnection(connectionString)) {
                    var cnt = SqlHelper.ExecuteScalar(conn, CommandType.Text, query, null);
                    if (cnt != null && cnt != DBNull.Value) { dbCnt = Convert.ToInt32(cnt); }
                }
            } catch { }
            return dbCnt > 0;
        }

        /// <summary>
        /// Checks if the lsc data exists, returns true if the lsc data exists
        /// </summary>
        /// <returns>Returns true if the lsc data exists.</returns>
        public static bool LscDataExists() {
            int lscCnt = 0;
            try {
                var query = "SELECT Count(1) FROM [dbo].[TM_LSC] WHERE [Enabled] = 1;";
                using (var conn = new SqlConnection(ConnectionString)) {
                    var cnt = SqlHelper.ExecuteScalar(conn, CommandType.Text, query, null);
                    if (cnt != null && cnt != DBNull.Value) { lscCnt = Convert.ToInt32(cnt); }
                }
            } catch { }
            return lscCnt > 0;
        }

        /// <summary>
        /// Create contents of connection strings used by the SqlConnection class
        /// </summary>
        /// <param name="trustedConnection">Avalue that indicates whether User ID and Password are specified in the connection (when false) or whether the current Windows account credentials are used for authentication (when true)</param>
        /// <param name="serverName">The name or network address of the instance of SQL Server to connect to</param>
        /// <param name="databasePort">The port of the database associated with the connection</param>
        /// <param name="databaseName">The name of the database associated with the connection</param>
        /// <param name="userName">The user ID to be used when connecting to SQL Server</param>
        /// <param name="password">The password for the SQL Server account</param>
        /// <param name="timeout">The connection timeout</param>
        /// <returns>Connection string</returns>
        public static string CreateConnectionString(bool trustedConnection, string serverName, int databasePort, string databaseName, string userName, string password, int timeout) {
            var builder = new SqlConnectionStringBuilder();
            builder.IntegratedSecurity = trustedConnection;
            builder.DataSource = String.Format("{0},{1}", serverName, databasePort);
            builder.InitialCatalog = databaseName;
            if (!trustedConnection) { builder.UserID = userName; builder.Password = password; }
            builder.PersistSecurityInfo = false;
            builder.MultipleActiveResultSets = true;
            builder.ConnectTimeout = timeout;
            builder.Pooling = true;
            builder.MinPoolSize = 5;
            builder.MaxPoolSize = 500;
            return builder.ConnectionString;
        }

        /// <summary>
        /// Sets or adds the specified connection string in the ConnectionStrings section
        /// </summary>
        /// <param name="name">ConnectionString name</param>
        /// <param name="connectionString">Connection string</param>
        public static bool SaveConnectionString(string name, string connectionString) {
            try {
                var config = WebConfigurationManager.OpenWebConfiguration("~");
                if (config.ConnectionStrings.ConnectionStrings[name] != null)
                    config.ConnectionStrings.ConnectionStrings[name].ConnectionString = connectionString;
                else
                    config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(name, connectionString));

                config.Save(ConfigurationSaveMode.Modified);
                return true;
            } catch { }
            return false;
        }

        /// <summary>
        /// Sets a version of installed application
        /// </summary>
        /// <param name="version">Version</param>
        /// <returns>Error</returns>
        public static string SetCurrentVersion(string version) {
            try {
                var config = WebConfigurationManager.OpenWebConfiguration("~");
                if (config.AppSettings.Settings["CurrentVersion"].Value != null)
                    config.AppSettings.Settings["CurrentVersion"].Value = version;
                else
                    config.AppSettings.Settings.Add("CurrentVersion", version);

                config.Save(ConfigurationSaveMode.Modified);
                return String.Empty;
            } catch (Exception ex) { return ex.Message; }
        }
    }
}