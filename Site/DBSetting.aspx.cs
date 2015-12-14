using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Configuration;
using Ext.Net;
using Delta.PECS.WebCSC.DBUtility;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "DBSetting")]
    public partial class DBSetting : IFramePageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                try {
                    var builder = new SqlConnectionStringBuilder(SqlHelper.ConnectionStringLocalTransaction);
                    ServerNameTextField.Text = builder.DataSource;
                    DBNameTextField.Text = builder.InitialCatalog;
                    if (builder.IntegratedSecurity) {
                        DBAuthenticationRadioGroup.SetValue("Radio2", true);
                    } else {
                        DBAuthenticationRadioGroup.SetValue("Radio1", true);
                        UIDTextField.Text = builder.UserID;
                        PWDTextField.Text = builder.Password;
                        DBPortSpinnerField.Number = 1433;
                    }
                } catch (Exception err) {
                    WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                    WebUtility.ShowMessage(EnmErrType.Error, err.Message);
                }
            }
        }

        /// <summary>
        /// Database Type ComboBox Refresh
        /// </summary>
        protected void OnDBTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            var data = new List<object>();
            foreach (EnmDBType dbType in Enum.GetValues(typeof(EnmDBType))) {
                data.Add(new {
                    Id = (int)dbType,
                    Name = WebUtility.GetDBTypeName(dbType)
                });
            }

            DBTypeStore.DataSource = data;
            DBTypeStore.DataBind();
        }

        /// <summary>
        /// Test connection strings
        /// </summary>
        /// <returns>result string</returns>
        [DirectMethod(Timeout = 300000)]
        public string ConnectDBString() {
            try {
                var dbType = Int32.Parse(DBTypeComboBox.SelectedItem.Value);
                var enmDBType = Enum.IsDefined(typeof(EnmDBType), dbType) ? (EnmDBType)dbType : EnmDBType.SQLServer;
                if (enmDBType != EnmDBType.SQLServer) { return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 0, "暂不支持该数据库类型"); }
                SqlHelper.TestConnection(SqlHelper.CreateConnectionString(Radio2.Checked, ServerNameTextField.Text.Trim(), (Int32)ServerPortSpinnerField.Number, DBNameTextField.Text.Trim(), UIDTextField.Text.Trim(), PWDTextField.Text.Trim(), 120));
                return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 200, "连接测试成功！");
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 0, err.Message);
            }
        }

        /// <summary>
        /// Save connection strings
        /// </summary>
        /// <returns>result string</returns>
        [DirectMethod(Timeout = 300000)]
        public string SaveDBString() {
            try {
                var dbType = Int32.Parse(DBTypeComboBox.SelectedItem.Value);
                var enmDBType = Enum.IsDefined(typeof(EnmDBType), dbType) ? (EnmDBType)dbType : EnmDBType.SQLServer;
                if (enmDBType != EnmDBType.SQLServer) { return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 0, "暂不支持该数据库类型"); }
                SaveConnectionString(SqlHelper.ConnectionStringLocalName, SqlHelper.CreateConnectionString(Radio2.Checked, ServerNameTextField.Text.Trim(), (Int32)ServerPortSpinnerField.Number, DBNameTextField.Text.Trim(), UIDTextField.Text.Trim(), PWDTextField.Text.Trim(), 120));
                return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 200, "数据源配置成功，请重新登录系统！");
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                return String.Format("{{\"Status\":{0}, \"Msg\":\"{1}\"}}", 0, err.Message);
            }
        }

        /// <summary>
        /// Sets or adds the specified connection string in the ConnectionStrings section
        /// </summary>
        /// <param name="name">ConnectionString name</param>
        /// <param name="connectionString">Connection string</param>
        private void SaveConnectionString(string name, string connectionString) {
            try {
                var config = WebConfigurationManager.OpenWebConfiguration("~");
                if (config.ConnectionStrings.ConnectionStrings[name] != null)
                    config.ConnectionStrings.ConnectionStrings[name].ConnectionString = connectionString;
                else
                    config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings(name, connectionString));

                config.Save(ConfigurationSaveMode.Modified);
            } catch { throw; }
        }
    }
}