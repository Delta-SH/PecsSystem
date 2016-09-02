using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Web.Security;
using System.Security.Principal;
using System.Security.AccessControl;
using Delta.PECS.WebCSC.DBUtility;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.Site {
    public partial class Installer : System.Web.UI.Page {
        #region Fields
        private List<string> upgradeableVersions = new List<string>() { "V3.5.0.0 Build160401" };
        private string currentVersion = WebConfigurationManager.AppSettings["CurrentVersion"];
        private string newVersion = WebConfigurationManager.AppSettings["UpdateVersion"];
        #endregion

        #region Utilities
        protected bool createDatabase() {
            addResult(String.Format("创建新数据库 {0}", this.txtNewDatabaseName.Text));
            var connectionString = InstallerHelper.CreateConnectionString(this.TrustedConnection, this.txtServerName.Text.Trim(), Int32.Parse(this.txtServerPort.Text.Trim()), "master", this.txtUsername.Text.Trim(), ViewState["install.password"].ToString(), 120);
            var error = InstallerHelper.CreateDatabase(this.txtNewDatabaseName.Text.Trim(), connectionString, this.txtNewDatabasePath.Text.Trim());
            if (!String.IsNullOrEmpty(error)) {
                this.pnlLog.Visible = true;
                addResult(String.Format("创建数据库错误： {0}", error));
                return false;
            }
            return true;
        }

        protected bool createHisDatabase() {
            addResult(String.Format("创建新数据库 {0}", this.txtHisNewDatabaseName.Text));
            var connectionString = InstallerHelper.CreateConnectionString(this.HisTrustedConnection, this.txtHisServerName.Text.Trim(), Int32.Parse(this.txtHisServerPort.Text.Trim()), "master", this.txtHisUsername.Text.Trim(), ViewState["install.hisPassword"].ToString(), 120);
            var error = InstallerHelper.CreateDatabase(this.txtHisNewDatabaseName.Text.Trim(), connectionString, this.txtHisNewDatabasePath.Text.Trim());
            if (!String.IsNullOrEmpty(error)) {
                this.pnlLog.Visible = true;
                addResult(String.Format("创建数据库错误： {0}", error));
                return false;
            }
            return true;
        }

        protected bool installDatabase(string connectionString) {
            using (var scope = new System.Transactions.TransactionScope()) {
                var scriptsFolder = Server.MapPath("~/Install/Scripts");
                var createDatabaseFile = String.Format(@"{0}\{1}", scriptsFolder, "WebCSC_createDatabase.sql");
                var error = proceedSQLScripts(createDatabaseFile, connectionString);
                if (!String.IsNullOrEmpty(error)) {
                    this.pnlLog.Visible = true;
                    addResult(String.Format("捕获异常： {0}", error));
                    return false;
                }

                string createDataFile = String.Format(@"{0}\{1}", scriptsFolder, "WebCSC_createData.sql");
                error = proceedSQLScripts(createDataFile, connectionString);
                if (!String.IsNullOrEmpty(error)) {
                    this.pnlLog.Visible = true;
                    addResult(String.Format("捕获异常： {0}", error));
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        protected bool installHisDatabase(string connectionString) {
            using (var scope = new System.Transactions.TransactionScope()) {
                var scriptsFolder = Server.MapPath("~/Install/Scripts");
                var createDatabaseFile = String.Format(@"{0}\{1}", scriptsFolder, "WebCSC_createHisDatabase.sql");
                var error = proceedSQLScripts(createDatabaseFile, connectionString);
                if (!String.IsNullOrEmpty(error)) {
                    this.pnlLog.Visible = true;
                    addResult(String.Format("捕获异常： {0}", error));
                    return false;
                }

                var createDataFile = String.Format(@"{0}\{1}", scriptsFolder, "WebCSC_createHisData.sql");
                error = proceedSQLScripts(createDataFile, connectionString);
                if (!String.IsNullOrEmpty(error)) {
                    this.pnlLog.Visible = true;
                    addResult(String.Format("捕获异常： {0}", error));
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        protected bool upgradeDatabase(string newVersion, string connectionString) {
            using (var scope = new System.Transactions.TransactionScope()) {
                var scriptsFolder = Server.MapPath("~/Install/Scripts");
                var upgradeFile = String.Format(@"{0}\{1}\{2}", scriptsFolder, newVersion, "WebCSC_upgrade.sql");
                var error = proceedSQLScripts(upgradeFile, connectionString);
                if (!String.IsNullOrEmpty(error)) {
                    this.pnlLog.Visible = true;
                    addResult(String.Format("捕获异常： {0}", error));
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        protected bool upgradeHisDatabase(string newVersion, string connectionString) {
            using (var scope = new System.Transactions.TransactionScope()) {
                var scriptsFolder = Server.MapPath("~/Install/Scripts");
                var upgradeFile = String.Format(@"{0}\{1}\{2}", scriptsFolder, newVersion, "WebCSC_his_upgrade.sql");
                var error = proceedSQLScripts(upgradeFile, connectionString);
                if (!String.IsNullOrEmpty(error)) {
                    this.pnlLog.Visible = true;
                    addResult(String.Format("捕获异常： {0}", error));
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

        protected string proceedSQLScripts(string pathToScriptFile, string connectionString) {
            addResult(String.Format("从文件中运行脚本： {0}", pathToScriptFile));
            var statements = new List<string>();
            using (var stream = File.OpenRead(pathToScriptFile)) {
                using (var reader = new StreamReader(stream, Encoding.Default)) {
                    var statement = String.Empty;
                    while ((statement = readNextStatementFromStream(reader)) != null) {
                        statements.Add(statement);
                    }
                }
            }

            try {
                foreach (var stmt in statements) {
                    using (var conn = new SqlConnection(connectionString)) {
                        conn.Open();
                        var command = new SqlCommand(stmt, conn);
                        command.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            } catch (Exception ex) { return ex.Message; }
            return String.Empty;
        }

        protected string readNextStatementFromStream(StreamReader reader) {
            var sb = new StringBuilder();
            string lineOfText;
            while (true) {
                lineOfText = reader.ReadLine();
                if (lineOfText == null) {
                    if (sb.Length > 0)
                        return sb.ToString();
                    else
                        return null;
                }
                if (lineOfText.TrimEnd().ToUpper() == "GO") { break; }
                sb.Append(lineOfText + Environment.NewLine);
            }
            return sb.ToString();
        }

        protected void addResult(string result) {
            mResult = String.Format("{0}\n{1}", result, mResult);
        }

        protected void handleError(string message) {
            this.lblError.Text = message;
        }

        private int QueryStringInt(string name, int defaultValue) {
            string resultStr = Request.QueryString[name];
            if (!String.IsNullOrEmpty(resultStr)) { return Int32.Parse(resultStr); }
            return defaultValue;
        }

        private bool checkPermissions(string path, bool checkRead, bool checkWrite, bool checkModify, bool checkDelete) {
            var flag = false;
            var flag2 = false;
            var flag3 = false;
            var flag4 = false;
            var flag5 = false;
            var flag6 = false;
            var flag7 = false;
            var flag8 = false;
            var current = WindowsIdentity.GetCurrent();
            System.Security.AccessControl.AuthorizationRuleCollection rules = null;
            try {
                rules = Directory.GetAccessControl(path).GetAccessRules(true, true, typeof(SecurityIdentifier));
            } catch { return true; }

            try {
                foreach (FileSystemAccessRule rule in rules) {
                    if (!current.User.Equals(rule.IdentityReference)) { continue; }
                    if (AccessControlType.Deny.Equals(rule.AccessControlType)) {
                        if ((FileSystemRights.Delete & rule.FileSystemRights) == FileSystemRights.Delete) { flag4 = true; }
                        if ((FileSystemRights.Modify & rule.FileSystemRights) == FileSystemRights.Modify) { flag3 = true; }
                        if ((FileSystemRights.Read & rule.FileSystemRights) == FileSystemRights.Read) { flag = true; }
                        if ((FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write) { flag2 = true; }
                        continue;
                    }
                    if (AccessControlType.Allow.Equals(rule.AccessControlType)) {
                        if ((FileSystemRights.Delete & rule.FileSystemRights) == FileSystemRights.Delete) { flag8 = true; }
                        if ((FileSystemRights.Modify & rule.FileSystemRights) == FileSystemRights.Modify) { flag7 = true; }
                        if ((FileSystemRights.Read & rule.FileSystemRights) == FileSystemRights.Read) { flag5 = true; }
                        if ((FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write) { flag6 = true; }
                    }
                }

                foreach (IdentityReference reference in current.Groups) {
                    foreach (FileSystemAccessRule rule2 in rules) {
                        if (!reference.Equals(rule2.IdentityReference)) { continue; }
                        if (AccessControlType.Deny.Equals(rule2.AccessControlType)) {
                            if ((FileSystemRights.Delete & rule2.FileSystemRights) == FileSystemRights.Delete) { flag4 = true; }
                            if ((FileSystemRights.Modify & rule2.FileSystemRights) == FileSystemRights.Modify) { flag3 = true; }
                            if ((FileSystemRights.Read & rule2.FileSystemRights) == FileSystemRights.Read) { flag = true; }
                            if ((FileSystemRights.Write & rule2.FileSystemRights) == FileSystemRights.Write) { flag2 = true; }
                            continue;
                        }
                        if (AccessControlType.Allow.Equals(rule2.AccessControlType)) {
                            if ((FileSystemRights.Delete & rule2.FileSystemRights) == FileSystemRights.Delete) { flag8 = true; }
                            if ((FileSystemRights.Modify & rule2.FileSystemRights) == FileSystemRights.Modify) { flag7 = true; }
                            if ((FileSystemRights.Read & rule2.FileSystemRights) == FileSystemRights.Read) { flag5 = true; }
                            if ((FileSystemRights.Write & rule2.FileSystemRights) == FileSystemRights.Write) { flag6 = true; }
                        }
                    }
                }

                var flag9 = !flag4 && flag8;
                var flag10 = !flag3 && flag7;
                var flag11 = !flag && flag5;
                var flag12 = !flag2 && flag6;
                var flag13 = true;
                if (checkRead) { flag13 = flag13 && flag11; }
                if (checkWrite) { flag13 = flag13 && flag12; }
                if (checkModify) { flag13 = flag13 && flag10; }
                if (checkDelete) { flag13 = flag13 && flag9; }
                return flag13;
            } catch { }
            return false;
        }
        #endregion

        #region Methods
        public string GetNewVersion() {
            return newVersion;
        }
        #endregion

        #region Handlers
        protected void Page_Load(Object sender, EventArgs e) {
            lblVersion.Text = String.Format("版本： {0}", GetNewVersion());
            if (!Page.IsPostBack) {
                if (InstallerHelper.ConnectionStringIsSet() && InstallerHelper.LscDataIsSet()) { Response.Redirect("~/Login.aspx", true); }
                var checkPermission = Convert.ToBoolean(this.QueryStringInt("checkpermission", 1));
                var testAgain = Convert.ToBoolean(this.QueryStringInt("testagain", 0));
                var rootDir = HttpContext.Current.Server.MapPath("~/");
                var dirsToCheck = new List<string>();
                dirsToCheck.Add(rootDir);
                dirsToCheck.Add(rootDir + "Install\\Scripts");
                dirsToCheck.Add(rootDir + "Logs\\RUN");
                dirsToCheck.Add(rootDir + "TempImages");
                foreach (var dir in dirsToCheck) {
                    if (!checkPermissions(dir, true, true, true, true) && checkPermission) {
                        pnlWizard.Visible = false;
                        imgHeader.Visible = false;
                        pnlPermission.Visible = true;
                        pnlButtons.Visible = true;
                        lblPermission.Text = String.Format(@"<b>警告:</b> {0} 账户对文件夹 {1} 没有完全控制权限，请对该文件夹配置这些权限。", System.Security.Principal.WindowsIdentity.GetCurrent().Name, dir);
                        return;
                    }
                }

                var filesToCheck = new List<string>();
                filesToCheck.Add(rootDir + "ConnectionStrings.config");
                filesToCheck.Add(rootDir + "web.config");
                foreach (var file in filesToCheck) {
                    if (!checkPermissions(file, false, true, true, true) && checkPermission) {
                        pnlWizard.Visible = false;
                        imgHeader.Visible = false;
                        pnlPermission.Visible = true;
                        pnlButtons.Visible = true;
                        lblPermission.Text = String.Format(@"<b>警告:</b> {0} 账户对文件 {1} 没有完全控制权限，请对该文件配置这些权限。", System.Security.Principal.WindowsIdentity.GetCurrent().Name, file);
                        return;
                    }
                }

                if (testAgain) {
                    pnlWizard.Visible = false;
                    pnlPermission.Visible = false;
                    pnlButtons.Visible = false;
                    pnlPermissionSuccess.Visible = true;
                    lblPermissionSuccess.Text = "完全控制权限已经配置成功！";
                    return;
                }
            }

            pnlWizard.Visible = true;
            pnlPermission.Visible = false;
            pnlButtons.Visible = false;
            if (!Page.IsPostBack) {
                var setData = Convert.ToBoolean(this.QueryStringInt("_sl", 0));
                if (setData) {
                    this.gvLsc_DataBind();
                    wzdInstaller.ActiveStepIndex = this.wzdInstaller.WizardSteps.IndexOf(this.stpLscSetting);
                } else {
                    if (HttpContext.Current != null) {
                        txtServerName.Text = WebUtility.GetServerIP();
                        txtHisServerName.Text = WebUtility.GetServerIP();
                    }

                    this.TrustedConnection = false;
                    this.HisTrustedConnection = false;
                    wzdInstaller.ActiveStepIndex = 0;
                }
            } else {
                if (ViewState["install.password"] == null) {
                    ViewState["install.password"] = this.txtPassword.Text;
                }
            }

            this.pnlLog.Visible = false;
            this.lblError.Text = String.Empty;
            mResult = String.Empty;
        }

        protected void Page_PreRender(object sender, EventArgs e) {
            if (this.Install) {
                this.rbCreateNew.Enabled = true;
                this.rbHisCreateNew.Enabled = true;
                this.rbUseExisting.Enabled = true;
                this.rbHisUseExisting.Enabled = true;
                this.pnlChangeAdminCredentials.Visible = true;
            } else {
                this.rbCreateNew.Enabled = false;
                this.rbCreateNew.Checked = false;
                this.rbHisCreateNew.Enabled = false;
                this.rbHisCreateNew.Checked = false;
                this.rbUseExisting.Enabled = true;
                this.rbUseExisting.Checked = true;
                this.rbHisUseExisting.Enabled = true;
                this.rbHisUseExisting.Checked = true;
                this.pnlChangeAdminCredentials.Visible = false;
            }
            this.txtPassword.Enabled = this.rbSQLAuthentication.Checked;
            this.txtUsername.Enabled = this.rbSQLAuthentication.Checked;
            this.txtHisPassword.Enabled = this.rbHisSQLAuthentication.Checked;
            this.txtHisUsername.Enabled = this.rbHisSQLAuthentication.Checked;
            this.txtExistingDatabaseName.Enabled = this.rbUseExisting.Checked;
            this.txtHisExistingDatabaseName.Enabled = this.rbHisUseExisting.Checked;
            this.txtNewDatabaseName.Enabled = this.rbCreateNew.Checked;
            this.txtNewDatabasePath.Enabled = this.rbCreateNew.Checked;
            this.txtHisNewDatabaseName.Enabled = this.rbHisCreateNew.Checked;
            this.txtHisNewDatabasePath.Enabled = this.rbHisCreateNew.Checked;
            this.chkDontCheckDatabase.Enabled = this.rbUseExisting.Checked;
            this.chkHisDontCheckDatabase.Enabled = this.rbHisUseExisting.Checked;

            if (this.pnlLog.Visible) { this.txtLog.Text = mResult; }
            string imgName = null;
            if (this.wzdInstaller.ActiveStepIndex == this.wzdInstaller.WizardSteps.IndexOf(this.stpWelcome)) { imgName = "header_1.png"; } else if (this.wzdInstaller.ActiveStepIndex == this.wzdInstaller.WizardSteps.IndexOf(this.stpUserServer)) { imgName = "header_2.png"; } else if (this.wzdInstaller.ActiveStepIndex == this.wzdInstaller.WizardSteps.IndexOf(this.stpDatabase)) { imgName = "header_3.png"; } else if (this.wzdInstaller.ActiveStepIndex == this.wzdInstaller.WizardSteps.IndexOf(this.stpConnectionString)) { imgName = "header_3.png"; } else if (this.wzdInstaller.ActiveStepIndex == this.wzdInstaller.WizardSteps.IndexOf(this.stpLscSetting)) { imgName = "header_4.png"; } else if (this.wzdInstaller.ActiveStepIndex == this.wzdInstaller.WizardSteps.IndexOf(this.stpStartService)) { imgName = "header_5.png"; } else if (this.wzdInstaller.ActiveStepIndex == this.wzdInstaller.WizardSteps.IndexOf(this.stpFinish)) { imgName = "header_6.png"; }
            imgHeader.ImageUrl = String.Format("~/Resources/images/Install/{0}", imgName);
        }

        protected void btnPermissionContinue_Click(object sender, EventArgs e) {
            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path));
        }

        protected void btnPermissionSkip_Click(object sender, EventArgs e) {
            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path) + "?checkpermission=0");
        }

        protected void btnPermissionTest_Click(object sender, EventArgs e) {
            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Path) + "?testagain=1");
        }

        protected void btnGoToSite_Click(object sender, EventArgs e) {
            Response.Redirect("~/Login.aspx", true);
        }

        protected void wzdInstaller_OnActiveStepChanged(object sender, EventArgs e) {
        }

        protected void wzdInstaller_OnNextButtonClick(object sender, WizardNavigationEventArgs e) {
            if (this.wzdInstaller.ActiveStepIndex == this.wzdInstaller.WizardSteps.IndexOf(this.stpWelcome)) {
                this.Install = this.rbInstall.Checked;
            } else if (this.wzdInstaller.ActiveStepIndex == this.wzdInstaller.WizardSteps.IndexOf(this.stpUserServer)) {
                ViewState["install.password"] = this.txtPassword.Text;
                ViewState["install.hisPassword"] = this.txtHisPassword.Text;
                this.TrustedConnection = this.rbWindowsAuthentication.Checked;
                this.HisTrustedConnection = this.rbHisWindowsAuthentication.Checked;
                if (String.IsNullOrEmpty(this.txtServerName.Text.Trim())) {
                    handleError("请输入配置库服务器的IP地址");
                    e.Cancel = true;
                    return;
                }
                if (String.IsNullOrEmpty(this.txtHisServerName.Text.Trim())) {
                    handleError("请输入历史库服务器的IP地址");
                    e.Cancel = true;
                    return;
                }

                var error = InstallerHelper.TestConnection(this.TrustedConnection, this.txtServerName.Text.Trim(), Int32.Parse(this.txtServerPort.Text.Trim()), String.Empty, this.txtUsername.Text.Trim(), ViewState["install.password"].ToString());
                if (!String.IsNullOrEmpty(error)) {
                    handleError(error);
                    e.Cancel = true;
                    return;
                }
                error = InstallerHelper.TestConnection(this.HisTrustedConnection, this.txtHisServerName.Text.Trim(), Int32.Parse(this.txtHisServerPort.Text.Trim()), String.Empty, this.txtHisUsername.Text.Trim(), ViewState["install.hisPassword"].ToString());
                if (!String.IsNullOrEmpty(error)) {
                    handleError(error);
                    e.Cancel = true;
                    return;
                }
            } else if (this.wzdInstaller.ActiveStepIndex == this.wzdInstaller.WizardSteps.IndexOf(this.stpDatabase)) {
                var database = String.Empty;
                if (rbCreateNew.Checked) { database = txtNewDatabaseName.Text.Trim(); } else { database = txtExistingDatabaseName.Text.Trim(); }
                this.ConnectionString = InstallerHelper.CreateConnectionString(this.TrustedConnection, this.txtServerName.Text.Trim(), Int32.Parse(this.txtServerPort.Text.Trim()), database, this.txtUsername.Text.Trim(), ViewState["install.password"].ToString(), 120);

                var hisDatabase = String.Empty;
                if (rbHisCreateNew.Checked) { hisDatabase = txtHisNewDatabaseName.Text.Trim(); } else { hisDatabase = txtHisExistingDatabaseName.Text.Trim(); }
                this.HisConnectionString = InstallerHelper.CreateConnectionString(this.HisTrustedConnection, this.txtHisServerName.Text.Trim(), Int32.Parse(this.txtHisServerPort.Text.Trim()), hisDatabase, this.txtHisUsername.Text.Trim(), ViewState["install.hisPassword"].ToString(), 120);

                if (this.rbUseExisting.Checked) {
                    if (String.IsNullOrEmpty(database)) {
                        handleError("请输入配置库名称");
                        e.Cancel = true;
                        return;
                    } else {
                        if (!chkDontCheckDatabase.Checked) {
                            if (!InstallerHelper.DatabaseExists(this.TrustedConnection, this.txtServerName.Text.Trim(), Int32.Parse(this.txtServerPort.Text.Trim()), database, this.txtUsername.Text.Trim(), ViewState["install.password"].ToString())) {
                                handleError(String.Format("配置库 '{0}' 不存在！", database));
                                e.Cancel = true;
                                return;
                            }
                        }
                    }
                } else {
                    if (!createDatabase()) {
                        handleError(String.Format("创建配置库错误： {0}", this.txtNewDatabaseName.Text));
                        e.Cancel = true;
                        return;
                    } else {
                        this.txtExistingDatabaseName.Text = this.txtNewDatabaseName.Text;
                        this.rbCreateNew.Checked = false;
                        this.rbUseExisting.Checked = true;
                    }
                }

                if (this.rbHisUseExisting.Checked) {
                    if (String.IsNullOrEmpty(hisDatabase)) {
                        handleError("请输入历史库名称");
                        e.Cancel = true;
                        return;
                    } else {
                        if (!chkHisDontCheckDatabase.Checked) {
                            if (!InstallerHelper.DatabaseExists(this.HisTrustedConnection, this.txtHisServerName.Text.Trim(), Int32.Parse(this.txtHisServerPort.Text.Trim()), hisDatabase, this.txtHisUsername.Text.Trim(), ViewState["install.hisPassword"].ToString())) {
                                handleError(String.Format("历史库 '{0}' 不存在！", database));
                                e.Cancel = true;
                                return;
                            }
                        }
                    }
                } else {
                    if (!createHisDatabase()) {
                        handleError(String.Format("创建历史库错误： {0}", this.txtHisNewDatabaseName.Text));
                        e.Cancel = true;
                        return;
                    } else {
                        this.txtHisExistingDatabaseName.Text = this.txtHisNewDatabaseName.Text;
                        this.rbHisCreateNew.Checked = false;
                        this.rbHisUseExisting.Checked = true;
                    }
                }

                if (this.Install) {
                    if (!installDatabase(ConnectionString)) {
                        handleError("在配置库安装过程中捕获异常");
                        e.Cancel = true;
                        return;
                    }

                    if (!installHisDatabase(HisConnectionString)) {
                        handleError("在历史库安装过程中捕获异常");
                        e.Cancel = true;
                        return;
                    }
                } else {
                    if (String.IsNullOrEmpty(currentVersion)) {
                        handleError("无法找到当前版本，升级失败");
                        e.Cancel = true;
                        return;
                    }

                    if (currentVersion == newVersion) {
                        handleError(String.Format("已经是最新版本 '{0}'", currentVersion));
                        e.Cancel = true;
                        return;
                    }

                    if (!upgradeableVersions.Contains(currentVersion)) {
                        handleError(String.Format("从版本 '{0}' 升级到版本 '{1}' 无效", currentVersion, newVersion));
                        e.Cancel = true;
                        return;
                    }

                    bool flag1 = false;
                    foreach (var version in upgradeableVersions) {
                        if (currentVersion == version) {
                            flag1 = true;
                            continue;
                        }

                        if (flag1) {
                            if (!upgradeDatabase(version, ConnectionString)) {
                                handleError("更新配置库脚本时捕获异常");
                                e.Cancel = true;
                                return;
                            }

                            if (!upgradeHisDatabase(version, ConnectionString)) {
                                handleError("更新历史库脚本时捕获异常");
                                e.Cancel = true;
                                return;
                            }
                        }
                    }
                }

                var setCurrentVersionError = InstallerHelper.SetCurrentVersion(newVersion);
                if (!String.IsNullOrEmpty(setCurrentVersionError)) {
                    this.pnlLog.Visible = true;
                    addResult(String.Format("设置新版本时捕获异常: {0}", setCurrentVersionError));
                    return;
                }

                this.pnlLog.Visible = false;
                if (InstallerHelper.SaveConnectionString(SqlHelper.ConnectionStringLocalName, ConnectionString) && InstallerHelper.SaveConnectionString(SqlHelper.HisConnectionStringLocalName, HisConnectionString)) {
                    this.gvLsc_DataBind();
                    wzdInstaller.ActiveStepIndex = this.wzdInstaller.WizardSteps.IndexOf(this.stpLscSetting);
                } else {
                    wzdInstaller.ActiveStepIndex = this.wzdInstaller.WizardSteps.IndexOf(this.stpConnectionString);
                    lblErrorConnMessage.Text = String.Format("此安装无法更新服务器上的ConnectionStrings.config配置文件，这可能由于修改此文件的系统权限被限制。请用记事本打开服务器上的ConnectionStrings.config配置文件，手动添加以下信息到 &lt;connectionStrings&gt;&lt;/connectionStrings&gt;节点之内: <br/><b>&lt;clear /&gt;<br/>&lt;add name=\"{0}\" connectionString=\"{1}\" /&gt;<br/>&lt;add name=\"{2}\" connectionString=\"{3}\" /&gt;</b><br/>", SqlHelper.ConnectionStringLocalName, ConnectionString, SqlHelper.HisConnectionStringLocalName, HisConnectionString);
                    lblErrorConnMessage.Visible = true;
                }
            } else if (this.wzdInstaller.ActiveStepIndex == this.wzdInstaller.WizardSteps.IndexOf(this.stpConnectionString)) {
                if (InstallerHelper.ConnectionString != ConnectionString || InstallerHelper.HisConnectionString != HisConnectionString) {
                    handleError("已存在的连接字符串与生成的连接字符串不匹配，请确认连接字符串是否正确。");
                    e.Cancel = true;
                    return;
                } else {
                    this.gvLsc_DataBind();
                    wzdInstaller.ActiveStepIndex = this.wzdInstaller.WizardSteps.IndexOf(this.stpLscSetting);
                }
            } else if (this.wzdInstaller.ActiveStepIndex == this.wzdInstaller.WizardSteps.IndexOf(this.stpLscSetting)) {
                if (!InstallerHelper.LscDataExists()) {
                    handleError("必须配置LSC信息，否则系统无法正常使用。");
                    e.Cancel = true;
                    return;
                }
            } else if (this.wzdInstaller.ActiveStepIndex == this.wzdInstaller.WizardSteps.IndexOf(this.stpStartService)) { }
        }
        #endregion

        #region Properties
        private bool Install {
            get { if (ViewState["Install"] != null) { return (bool)ViewState["Install"]; } return true; }
            set { ViewState["Install"] = value; }
        }

        private bool TrustedConnection {
            get { if (ViewState["TrustedConnection"] != null) { return (bool)ViewState["TrustedConnection"]; } return false; }
            set { ViewState["TrustedConnection"] = value; }
        }

        private bool HisTrustedConnection {
            get { if (ViewState["HisTrustedConnection"] != null) { return (bool)ViewState["HisTrustedConnection"]; } return false; }
            set { ViewState["HisTrustedConnection"] = value; }
        }

        private string ConnectionString {
            get { if (ViewState["connString"] == null) { ViewState["connString"] = String.Empty; } return (string)ViewState["connString"]; }
            set { ViewState["connString"] = value; }
        }

        private string HisConnectionString {
            get { if (ViewState["hisConnString"] == null) { ViewState["hisConnString"] = String.Empty; } return (string)ViewState["hisConnString"]; }
            set { ViewState["hisConnString"] = value; }
        }

        private string mResult {
            get { return (string)ViewState["result"]; }
            set { ViewState["result"] = value; }
        }
        #endregion

        #region Data
        protected void gvLsc_RowCommand(object sender, GridViewCommandEventArgs e) {
            try {
                if (String.IsNullOrEmpty(e.CommandArgument.ToString())) { return; }
                var lscId = Int32.Parse(e.CommandArgument.ToString());
                var lscEntity = new BLsc();
                switch (e.CommandName) {
                    case "Select":
                        LscIDTextBox.Text = String.Empty;
                        LscNameTextBox.Text = String.Empty;
                        LscIPTextBox.Text = String.Empty;
                        LscPortTextBox.Text = String.Empty;
                        LscUIDTextBox.Text = String.Empty;
                        LscPwdTextBox.Attributes["value"] = String.Empty;
                        BeatIntervalTextBox.Text = String.Empty;
                        BeatDelayTextBox.Text = String.Empty;
                        DBIPTextBox.Text = String.Empty;
                        DBPortTextBox.Text = String.Empty;
                        DBNameTextBox.Text = String.Empty;
                        DBUIDTextBox.Text = String.Empty;
                        DBPwdTextBox.Attributes["value"] = String.Empty;
                        HisDBIPTextBox.Text = String.Empty;
                        HisDBPortTextBox.Text = String.Empty;
                        HisDBNameTextBox.Text = String.Empty;
                        HisDBUIDTextBox.Text = String.Empty;
                        HisDBPwdTextBox.Attributes["value"] = String.Empty;
                        EnabledCheckBox.Checked = false;
                        LscIDTextBox.Enabled = false;
                        TitleLabel.Text = "编辑记录";
                        UpdateBtn.Visible = true;
                        SaveBtn.Visible = false;
                        var lsc = lscEntity.GetLsc(lscId);
                        if (lsc != null) {
                            LscIDTextBox.Text = lsc.LscID.ToString();
                            LscNameTextBox.Text = lsc.LscName;
                            LscIPTextBox.Text = lsc.LscIP;
                            LscPortTextBox.Text = lsc.LscPort.ToString();
                            LscUIDTextBox.Text = lsc.LscUID;
                            LscPwdTextBox.Attributes["value"] = lsc.LscPwd;
                            BeatIntervalTextBox.Text = lsc.BeatInterval.ToString();
                            BeatDelayTextBox.Text = lsc.BeatDelay.ToString();
                            DBIPTextBox.Text = lsc.DBServer;
                            DBPortTextBox.Text = lsc.DBPort.ToString();
                            DBNameTextBox.Text = lsc.DBName;
                            DBUIDTextBox.Text = lsc.DBUID;
                            DBPwdTextBox.Attributes["value"] = lsc.DBPwd;
                            HisDBIPTextBox.Text = lsc.HisDBServer;
                            HisDBPortTextBox.Text = lsc.HisDBPort.ToString();
                            HisDBNameTextBox.Text = lsc.HisDBName;
                            HisDBUIDTextBox.Text = lsc.HisDBUID;
                            HisDBPwdTextBox.Attributes["value"] = lsc.HisDBPwd;
                            EnabledCheckBox.Checked = lsc.Enabled;
                        }
                        ModalPopupExtender.Show();
                        break;
                    case "Del":
                        lscEntity.DelLsc(lscId);
                        var lscs = lscEntity.GetLscs();
                        gvLsc.DataSource = lscs;
                        gvLsc.SelectedIndex = -1;
                        gvLsc.DataBind();
                        break;
                    default:
                        break;
                }
            } catch (Exception err) { handleError(err.Message); }
        }

        protected void gvLsc_DataBind() {
            try {
                var connectionString = this.ConnectionString;
                if (String.IsNullOrEmpty(connectionString)) {
                    connectionString = InstallerHelper.ConnectionString;
                }

                var lscs = new List<LscInfo>();
                if (!String.IsNullOrEmpty(connectionString)) {
                    var lscEntity = new BLsc();
                    lscs = lscEntity.GetLscs(connectionString);
                }

                gvLsc.DataSource = lscs;
                gvLsc.DataBind();
            } catch (Exception err) { handleError(err.Message); }
        }

        protected void AddBtn_Click(object sender, EventArgs e) {
            LscIDTextBox.Text = String.Empty;
            LscNameTextBox.Text = String.Empty;
            LscIPTextBox.Text = String.Empty;
            LscPortTextBox.Text = String.Empty;
            LscUIDTextBox.Text = String.Empty;
            LscPwdTextBox.Attributes["value"] = String.Empty;
            BeatIntervalTextBox.Text = "20";
            BeatDelayTextBox.Text = "20";
            DBIPTextBox.Text = String.Empty;
            DBPortTextBox.Text = "1433";
            DBNameTextBox.Text = String.Empty;
            DBUIDTextBox.Text = String.Empty;
            DBPwdTextBox.Attributes["value"] = String.Empty;
            HisDBIPTextBox.Text = String.Empty;
            HisDBPortTextBox.Text = "1433";
            HisDBNameTextBox.Text = String.Empty;
            HisDBUIDTextBox.Text = String.Empty;
            HisDBPwdTextBox.Attributes["value"] = String.Empty;
            EnabledCheckBox.Checked = false;
            LscIDTextBox.Enabled = true;
            TitleLabel.Text = "新增记录";
            UpdateBtn.Visible = false;
            SaveBtn.Visible = true;
            ModalPopupExtender.Show();
        }

        protected void UpdateBtn_Click(object sender, EventArgs e) {
            try {
                var lsc = new LscInfo();
                lsc.LscID = Int32.Parse(LscIDTextBox.Text.Trim());
                lsc.LscName = LscNameTextBox.Text.Trim();
                lsc.LscIP = LscIPTextBox.Text.Trim();
                lsc.LscPort = Int32.Parse(LscPortTextBox.Text.Trim());
                lsc.LscUID = LscUIDTextBox.Text.Trim();
                lsc.LscPwd = LscPwdTextBox.Text.Trim();
                lsc.BeatInterval = Int32.Parse(BeatIntervalTextBox.Text.Trim());
                lsc.BeatDelay = Int32.Parse(BeatDelayTextBox.Text.Trim());
                lsc.DBServer = DBIPTextBox.Text.Trim();
                lsc.DBPort = Int32.Parse(DBPortTextBox.Text.Trim());
                lsc.DBName = DBNameTextBox.Text.Trim();
                lsc.DBUID = DBUIDTextBox.Text.Trim();
                lsc.DBPwd = DBPwdTextBox.Text.Trim();
                lsc.HisDBServer = HisDBIPTextBox.Text.Trim();
                lsc.HisDBPort = Int32.Parse(HisDBPortTextBox.Text.Trim());
                lsc.HisDBName = HisDBNameTextBox.Text.Trim();
                lsc.HisDBUID = HisDBUIDTextBox.Text.Trim();
                lsc.HisDBPwd = HisDBPwdTextBox.Text.Trim();
                lsc.Connected = false;
                lsc.ChangedTime = DateTime.Now;
                lsc.Enabled = EnabledCheckBox.Checked;

                var lscEntity = new BLsc();
                lscEntity.UpdateLsc(lsc);
                var lscs = lscEntity.GetLscs();
                gvLsc.DataSource = lscs;
                gvLsc.DataBind();
            } catch (Exception err) { handleError(err.Message); }
        }

        protected void SaveBtn_Click(object sender, EventArgs e) {
            try {
                var lsc = new LscInfo();
                lsc.LscID = Int32.Parse(LscIDTextBox.Text.Trim());
                lsc.LscName = LscNameTextBox.Text.Trim();
                lsc.LscIP = LscIPTextBox.Text.Trim();
                lsc.LscPort = Int32.Parse(LscPortTextBox.Text.Trim());
                lsc.LscUID = LscUIDTextBox.Text.Trim();
                lsc.LscPwd = LscPwdTextBox.Text.Trim();
                lsc.BeatInterval = Int32.Parse(BeatIntervalTextBox.Text.Trim());
                lsc.BeatDelay = Int32.Parse(BeatDelayTextBox.Text.Trim());
                lsc.DBServer = DBIPTextBox.Text.Trim();
                lsc.DBPort = Int32.Parse(DBPortTextBox.Text.Trim());
                lsc.DBName = DBNameTextBox.Text.Trim();
                lsc.DBUID = DBUIDTextBox.Text.Trim();
                lsc.DBPwd = DBPwdTextBox.Text.Trim();
                lsc.HisDBServer = HisDBIPTextBox.Text.Trim();
                lsc.HisDBPort = Int32.Parse(HisDBPortTextBox.Text.Trim());
                lsc.HisDBName = HisDBNameTextBox.Text.Trim();
                lsc.HisDBUID = HisDBUIDTextBox.Text.Trim();
                lsc.HisDBPwd = HisDBPwdTextBox.Text.Trim();
                lsc.Connected = false;
                lsc.ChangedTime = DateTime.Now;
                lsc.Enabled = EnabledCheckBox.Checked;

                var lscEntity = new BLsc();
                if (!lscEntity.CheckLsc(lsc.LscID)) {
                    lscEntity.AddLsc(lsc);
                    var lscs = lscEntity.GetLscs();
                    gvLsc.DataSource = lscs;
                    gvLsc.DataBind();
                } else {
                    handleError("编号已存在，保存失败！");
                }
            } catch (Exception err) { handleError(err.Message); }
        }
        #endregion
    }
}