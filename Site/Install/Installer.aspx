<%@ Page Language="C#" AutoEventWireup="true" Theme="Install" CodeBehind="Installer.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.Installer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>动力环境监控中心系统 安装向导</title>
    <link rel="shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link rel="icon" type="image/x-icon" href="../favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="../favicon.ico" />
</head>
<body class="container-main">
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager runat="server" ID="ScriptManager1" EnableScriptGlobalization="true" EnableScriptLocalization="true" />
    <table border="0" cellpadding="0" cellspacing="0" class="container-installer">
        <tr class="top">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
                &nbsp;
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
        <tr class="Middle">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
                <asp:Label ID="pageTitle" runat="server" CssClass="header-text" Text="动力环境监控中心系统 安装向导"/>
                <br />
                <asp:Image ID="imgHeader" runat="server" CssClass="header-img" />
                <asp:Panel ID="pnlWizard" runat="server" CssClass="content">
                    <asp:Wizard ID="wzdInstaller" runat="server" DisplaySideBar="False" OnActiveStepChanged="wzdInstaller_OnActiveStepChanged"
                        OnNextButtonClick="wzdInstaller_OnNextButtonClick" ActiveStepIndex="0">
                        <StepNavigationTemplate>
                            <div style="width:100%; padding:0px 10px 0px 0px;">
                                <asp:Button ID="btnStepPrev" runat="server" CommandName="MovePrevious" Width="100"
                                    Text="上一步" Source="file" />
                                <asp:Button ID="btnStepNext" runat="server" CommandName="MoveNext" Width="100px"
                                    Text="下一步" />
                            </div>
                        </StepNavigationTemplate>
                        <StartNavigationTemplate>
                            <div style="width:100%; padding:0px 10px 0px 0px;">
                                <asp:Button ID="btnStepNext" runat="server" CommandName="MoveNext" Width="100" Text="下一步" />
                            </div>
                        </StartNavigationTemplate>
                        <WizardSteps>
                            <asp:WizardStep ID="stpWelcome" runat="server">
                                <table class="wizard-step" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="lblWelcome" runat="server" Text="Step 1：欢迎使用动力环境监控中心系统！" CssClass="title"/>
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <p>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;动力环境监控中心系统(PECS)是<a href="http://www.deltagreentech.com.cn/"
                                                    target="_blank">中达电通股份有限公司</a>新一代的全新智能动力环境监控系统管理平台。 此向导将指导您完成对<strong>动力环境监控中心系统</strong>的配置。
                                                <br />
                                                <br />
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;要完成此向导，您必须了解一些关于您的数据库服务器（"连接字符串"）的信息。如果有必要，请联系您的ISP。
                                                如果您正在安装在本地机器或服务器上，您可能需要从您的系统管理员处得到一些信息。
                                                <br />
                                                <br />
                                                提示：在您开始任何操作之前，<b>请不要忘记备份。</b>
                                            </p>
                                            <p>
                                                <asp:RadioButton ID="rbInstall" runat="server" Text="安装系统" AutoPostBack="True" GroupName="InstallUpgrade" Checked="True"/>
                                                <br />
                                                <asp:RadioButton ID="rbUpgrade" runat="server" Text="升级系统" GroupName="InstallUpgrade"/>
                                            </p>
                                            <p>
                                                请点击 <b>"下一步"</b> 启动安装向导
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:WizardStep>
                            <asp:WizardStep ID="stpUserServer" runat="server">
                                <table class="wizard-step" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="4" valign="top">
                                            <asp:Label ID="lblSQLServer" runat="server" Text="Step 2：SQL Server服务器配置" CssClass="title"/>
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" style="width:80px;">
                                            <asp:Label ID="lblServerName" runat="server" Text="配置库地址："/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServerName" CssClass="textbox" runat="server"/>
                                        </td>
                                        <td nowrap="nowrap" style="width:80px;">
                                            <asp:Label ID="lblHisServerName" runat="server" Text="历史库地址："/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHisServerName" CssClass="textbox" runat="server"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" style="width:80px;">
                                            <asp:Label ID="lblServerPort" runat="server" Text="配置库端口："/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtServerPort" CssClass="textbox" runat="server" Text="1433"/>
                                        </td>
                                        <td nowrap="nowrap" style="width:80px;">
                                            <asp:Label ID="lblHisServerPort" runat="server" Text="历史库端口："/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHisServerPort" CssClass="textbox" runat="server" Text="1433"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:RadioButton ID="rbSQLAuthentication" runat="server" Text="使用SQL Server账户" AutoPostBack="True" GroupName="AuthenticationType" Checked="True"/>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButton ID="rbHisSQLAuthentication" runat="server" Text="使用SQL Server账户" AutoPostBack="True" GroupName="HisAuthenticationType" Checked="True"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" style="padding-left:20px;width:80px;">
                                            <asp:Label ID="lblUsername" runat="server" Text="用户名："/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtUsername" CssClass="textbox" runat="server"/>
                                        </td>
                                        <td nowrap="nowrap" style="padding-left:20px;width:80px;">
                                            <asp:Label ID="lblHisUsername" runat="server" Text="用户名："/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHisUsername" CssClass="textbox" runat="server"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" style="padding-left:20px;width:80px;">
                                            <asp:Label ID="lblPassword" runat="server" Text="密&nbsp;&nbsp;&nbsp;&nbsp;码："/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPassword" CssClass="textbox" runat="server" TextMode="Password"/>
                                        </td>
                                        <td nowrap="nowrap" style="padding-left:20px;width:80px;">
                                            <asp:Label ID="lblHisPassword" runat="server" Text="密&nbsp;&nbsp;&nbsp;&nbsp;码："/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHisPassword" CssClass="textbox" runat="server" TextMode="Password"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:RadioButton ID="rbWindowsAuthentication" runat="server" Text="Windows集成身份认证" AutoPostBack="True" GroupName="AuthenticationType"/>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButton ID="rbHisWindowsAuthentication" runat="server" Text="Windows集成身份认证" AutoPostBack="True" GroupName="HisAuthenticationType"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </asp:WizardStep>
                            <asp:WizardStep ID="stpDatabase" runat="server" AllowReturn="false">
                                <table class="wizard-step" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="4" valign="top">
                                            <asp:Label ID="lblDatabase" runat="server" Text="Step 3：SQL Server数据库配置" CssClass="title"/>
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblSetting" runat="server" Text="配置库管理" CssClass="title"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:RadioButton ID="rbCreateNew" runat="server" Text="创建全新配置库" AutoPostBack="True" GroupName="DatabaseType" Checked="True"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" style="padding-left:20px;width:80px;">
                                            <asp:Label ID="lblNewDatabaseName" runat="server" Text="配置库名称:"/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNewDatabaseName" CssClass="textbox" runat="server" Enabled="False"/>
                                        </td>
                                        <td nowrap="nowrap" style="width:80px;">
                                            <asp:Label ID="lblNewDatabasePath" runat="server" Text="配置库目录:"/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNewDatabasePath" CssClass="textbox" runat="server" Enabled="False"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:RadioButton ID="rbUseExisting" runat="server" Text="使用现有配置库" AutoPostBack="True" GroupName="DatabaseType"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" style="padding-left:20px;width:80px;">
                                            <asp:Label ID="lblExistingDatabaseName" runat="server" Text="配置库名称:"/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtExistingDatabaseName" CssClass="textbox" runat="server"/>
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox runat="server" ID="chkDontCheckDatabase" Text="不检查配置库是否存在" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Label ID="lblHistory" runat="server" Text="历史库管理" CssClass="title"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:RadioButton ID="rbHisCreateNew" runat="server" Text="创建全新历史库" AutoPostBack="True" GroupName="HisDatabaseType" Checked="True"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" style="padding-left:20px;width:80px;">
                                            <asp:Label ID="lblHisNewDatabaseName" runat="server" Text="历史库名称:"/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHisNewDatabaseName" CssClass="textbox" runat="server" Enabled="False"/>
                                        </td>
                                        <td nowrap="nowrap" style="width:80px;">
                                            <asp:Label ID="lblHisNewDatabasePath" runat="server" Text="历史库目录:"/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHisNewDatabasePath" CssClass="textbox" runat="server" Enabled="False"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:RadioButton ID="rbHisUseExisting" runat="server" Text="使用现有历史库" AutoPostBack="True" GroupName="HisDatabaseType"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td nowrap="nowrap" style="padding-left:20px;width:80px;">
                                            <asp:Label ID="lblHisExistingDatabaseName" runat="server" Text="历史库名称:"/>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHisExistingDatabaseName" CssClass="textbox" runat="server"/>
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox runat="server" ID="chkHisDontCheckDatabase" Text="不检查历史库是否存在" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:Panel ID="pnlLog" runat="server" Visible="False" Width="100%">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lblLog" runat="server" Text="安装日志：" CssClass="title"/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Panel runat="server" ID="pnlGroupLog">
                                                                <asp:TextBox ID="txtLog" runat="server" CssClass="log" TextMode="MultiLine" ReadOnly="True" />
                                                            </asp:Panel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                            </asp:WizardStep>
                            <asp:WizardStep ID="stpConnectionString" runat="server" AllowReturn="false" StepType="Start">
                                <asp:Panel ID="pnlConnectionString" runat="server">
                                    <asp:Label ID="lblConnectionString" runat="server" Text="Step 4：SQL Server连接字符串" CssClass="title" Visible="False"/>
                                    <hr />
                                    <table class="wizard-step" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left">
                                                <asp:Label ID="lblErrorConnMessage" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:WizardStep>
                            <asp:WizardStep ID="stpLscSetting" runat="server" StepType="Start">
                                <table class="wizard-step" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="lblLscSetting" runat="server" Text="Step 5：LSC信息配置" CssClass="title"/>
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="bottom">
                                            <asp:LinkButton ID="AddBtn" runat="server" OnClick="AddBtn_Click" Font-Underline="false" Text="新增记录»" />
                                            <asp:Panel ID="pnlLsc" runat="server" Width="630px" Height="300px" ScrollBars="Auto" BackColor="White" BorderColor="#d6d6d6" BorderStyle="Solid" Style="margin:5px 0 10px 0;">
                                                <asp:GridView ID="gvLsc" runat="server" AutoGenerateColumns="false" DataKeyNames="LscID" SkinID="GridViewSkin" OnRowCommand="gvLsc_RowCommand" BorderStyle="None" Width="1500">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="操作">
                                                            <ItemTemplate>
                                                                <div style="padding:0 5px;">
                                                                    <asp:LinkButton ID="EditBtn" runat="server" CommandName="Select" Text="编辑" CommandArgument='<%# Eval("LscID") %>'/>
                                                                    <asp:LinkButton ID="DeleteBtn" runat="server" CommandName="Del" Text="删除" CommandArgument='<%# Eval("LscID") %>' OnClientClick="return confirm('您确认要删除该记录吗?');"/>
                                                                </div>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"/>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="LscID" HeaderText="Lsc编号" ReadOnly="True" />
                                                        <asp:BoundField DataField="LscName" HeaderText="Lsc名称" />
                                                        <asp:BoundField DataField="LscIP" HeaderText="通信地址" />
                                                        <asp:BoundField DataField="LscPort" HeaderText="通信端口" />
                                                        <asp:BoundField DataField="LscUID" HeaderText="登录账户" />
                                                        <asp:TemplateField HeaderText="登录密码">
                                                            <ItemTemplate>
                                                                <asp:Label ID="LscPwdLbl" runat="server" Text="******" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="BeatInterval" HeaderText="检测延迟" />
                                                        <asp:BoundField DataField="BeatDelay" HeaderText="中断延迟" />
                                                        <asp:BoundField DataField="DBServer" HeaderText="配置库地址" />
                                                        <asp:BoundField DataField="DBPort" HeaderText="配置库端口" />
                                                        <asp:BoundField DataField="DBName" HeaderText="配置库名称" />
                                                        <asp:BoundField DataField="DBUID" HeaderText="配置库账户" />
                                                        <asp:TemplateField HeaderText="配置库密码">
                                                            <ItemTemplate>
                                                                <asp:Label ID="DBPwdLbl" runat="server" Text="******" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="HisDBServer" HeaderText="历史库地址" />
                                                        <asp:BoundField DataField="HisDBPort" HeaderText="历史库端口" />
                                                        <asp:BoundField DataField="HisDBName" HeaderText="历史库名称" />
                                                        <asp:BoundField DataField="HisDBUID" HeaderText="历史库账户" />
                                                        <asp:TemplateField HeaderText="历史库密码">
                                                            <ItemTemplate>
                                                                <asp:Label ID="HisDBPwdLbl" runat="server" Text="******" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="通信状态">
                                                            <ItemTemplate>
                                                                <img width="16" height="16" alt='<%# Convert.ToBoolean(Eval("Connected")) ? "已连接" : "已断开" %>' title='<%# Convert.ToBoolean(Eval("Connected")) ? "已连接" : "已断开" %>' src='<%# Convert.ToBoolean(Eval("Connected")) ? "../Resources/images/Tips/T-Connected.png" : "../Resources/images/Tips/T-Disconnected.png" %>' />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="连接/中断时间" DataField="ChangedTime" DataFormatString="{0:G}">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="使能">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="EnabledCheckBox" runat="server" Checked='<%# Bind("Enabled") %>' Enabled="false" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle Width="100px" Wrap="False" />
                                                    <RowStyle Wrap="false" />
                                                    <EmptyDataTemplate>
                                                        <div style="padding:5px;">
                                                            暂无数据记录！
                                                        </div>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="MainPanel" runat="server" Style="display:none;" CssClass="modalPopup" Width="600px">
                                    <asp:Panel ID="TitlePanel" runat="server" Style="cursor:move; background-color:#507CD1; font-weight:bold; color:#FFFFFF; padding:5px; text-align:left;">
                                        <asp:Label ID="TitleLabel" runat="server"/>
                                    </asp:Panel>
                                    <asp:Panel ID="ContentPanel" runat="server">
                                        <table class="modal-panel" cellpadding="0" cellspacing="1" border="0">
                                            <tbody>
                                                <tr>
                                                    <td class="formtitle">
                                                        Lsc编号:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="LscIDTextBox" runat="server" MaxLength="10" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="LscIDTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="LscIDTextBox" ErrorMessage="*" ValidationGroup="Save" ValidationExpression="^\d+$"/>
                                                    </td>
                                                    <td class="formtitle">
                                                        Lsc名称:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="LscNameTextBox" runat="server" MaxLength="50" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="LscNameTextBox" Display="Dynamic" ErrorMessage="*" ValidationGroup="Save"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formtitle">
                                                        通信地址:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="LscIPTextBox" runat="server" MaxLength="15" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="LscIPTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="LscIPTextBox" ErrorMessage="*" ValidationGroup="Save" ValidationExpression="^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$"/>
                                                    </td>
                                                    <td class="formtitle">
                                                        通信端口:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="LscPortTextBox" runat="server" MaxLength="5" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="LscPortTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="LscPortTextBox" ErrorMessage="*" ValidationGroup="Save" ValidationExpression="^\d+$"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formtitle">
                                                        登录账户:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="LscUIDTextBox" runat="server" MaxLength="50" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="LscUIDTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                    </td>
                                                    <td class="formtitle">
                                                        登录密码:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="LscPwdTextBox" runat="server" MaxLength="50" Width="150px" TextMode="Password"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="LscPwdTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formtitle">
                                                        检测延迟(s):
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="BeatIntervalTextBox" runat="server" MaxLength="10" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="BeatIntervalTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="BeatIntervalTextBox" ErrorMessage="*" ValidationGroup="Save" ValidationExpression="^\d+$"/>
                                                    </td>
                                                    <td class="formtitle">
                                                        中断延迟(s):
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="BeatDelayTextBox" runat="server" MaxLength="10" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="BeatDelayTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="BeatDelayTextBox" ErrorMessage="*" ValidationGroup="Save" ValidationExpression="^\d+$"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formtitle">
                                                        配置库地址:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="DBIPTextBox" runat="server" MaxLength="15" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="DBIPTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="DBIPTextBox" ErrorMessage="*" ValidationGroup="Save" ValidationExpression="^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$"/>
                                                    </td>
                                                    <td class="formtitle">
                                                        配置库端口:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="DBPortTextBox" runat="server" MaxLength="10" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="DBPortTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="DBPortTextBox" ErrorMessage="*" ValidationGroup="Save" ValidationExpression="^\d+$"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formtitle">
                                                        配置库名称:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="DBNameTextBox" runat="server" MaxLength="50" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="DBNameTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                    </td>
                                                    <td class="formtitle">
                                                        配置库账户:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="DBUIDTextBox" runat="server" MaxLength="50" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="DBUIDTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formtitle">
                                                        配置库密码:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="DBPwdTextBox" runat="server" MaxLength="50" Width="150px" TextMode="Password"/>
                                                    </td>
                                                    <td class="formtitle">
                                                        历史库地址:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="HisDBIPTextBox" runat="server" MaxLength="15" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="HisDBIPTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="HisDBIPTextBox" ErrorMessage="*" ValidationGroup="Save" ValidationExpression="^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formtitle">
                                                        历史库端口:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="HisDBPortTextBox" runat="server" MaxLength="10" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="HisDBPortTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="HisDBPortTextBox" ErrorMessage="*" ValidationGroup="Save" ValidationExpression="^\d+$"/>
                                                    </td>
                                                    <td class="formtitle">
                                                        历史库名称:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="HisDBNameTextBox" runat="server" MaxLength="50" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="HisDBNameTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formtitle">
                                                        历史库账户:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="HisDBUIDTextBox" runat="server" MaxLength="50" Width="150px"/>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="HisDBUIDTextBox" ErrorMessage="*" ValidationGroup="Save"/>
                                                    </td>
                                                    <td class="formtitle">
                                                        历史库密码:
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="HisDBPwdTextBox" runat="server" MaxLength="50" Width="150px" TextMode="Password"/>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="formtitle">
                                                        启用状态:
                                                    </td>
                                                    <td colspan="3">
                                                        <asp:CheckBox ID="EnabledCheckBox" runat="server" Checked="true" Text="（勾选表示启用）" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="text-align:right;" colspan="4">
                                                        <asp:Button ID="UpdateBtn" runat="server" Text="更新" ValidationGroup="Save" OnClick="UpdateBtn_Click"/>
                                                        <asp:Button ID="SaveBtn" runat="server" Text="保存" ValidationGroup="Save" OnClick="SaveBtn_Click"/>
                                                        <asp:Button ID="CancelBtn" runat="server" Text="取消"/>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                </asp:Panel>
                                <asp:HiddenField ID="HF" runat="server" />
                                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender" runat="server" TargetControlID="HF" PopupControlID="MainPanel" BackgroundCssClass="modalBackground" CancelControlID="CancelBtn" DropShadow="true" PopupDragHandleControlID="TitlePanel" />
                            </asp:WizardStep>
                            <asp:WizardStep ID="stpStartService" runat="server" StepType="Step">
                                <table class="wizard-step" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td valign="top">
                                            <asp:Label ID="Label2" runat="server" Text="Step 6：启动DataService服务" CssClass="title" />
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <p>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"DataService"服务程序主要负责与数据管理程序"iSmartAccess"应用程序的信息通讯、接收"iSmartAccess"应用程序发送来的各种消息、
                                                向"iSmartAccess"应用程序请求Web应用程序所需的各种信息、将Web发送的测点控制和设置指令消息转发给"iSmartAccess"应用程序等工作。
                                            </p>
                                            <p>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Web应用程序的运行所需的各种信息均需要"DataService"服务程序的提供和支持，所以在运行Web程序时，请确认"DataService"服务程序已经正常运行。
                                            </p>
                                            <p>
                                                关于"DataService"服务的启动方法，请参见《<a href="../Resources/doc/DataServiceInstall.pdf" target="_blank">DataService安装部署说明</a>》。
                                            </p>
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <br />
                                            <p>
                                                启动完成后，请点击 <b>"下一步"</b> 完成安装向导
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </asp:WizardStep>
                            <asp:WizardStep ID="stpFinish" runat="server" StepType="Complete">
                                <asp:Panel ID="pnlFinished" runat="server">
                                    <table class="wizard-step" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblCompleted" runat="server" Text="恭喜您，动力环境监控中心系统已经配置成功！" Font-Size="14px" Font-Bold="true" ForeColor="#cc0000" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="pnlChangeAdminCredentials" runat="server" style="border:solid 2px #d6d6d6;background:#fff; margin:0 auto; padding:20px 5px;">
                                                    重要提示：由于动力环境监控中心系统的运行需要Web服务"DataService"的支持，所以在使用该系统之前，请确认"DataService"服务已成功启动。
                                                    关于"DataService"服务的安装部署说明，请参见《<a href="../Resources/doc/DataServiceInstall.pdf" target="_blank">DataService安装部署说明</a>》。
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <p>
                                                    一旦您完成以上步骤，您可以通过点击以下链接开始使用动力环境监控中心系统。
                                                </p>
                                                <p>
                                                    如果您需要关于如何使用动力环境监控中心系统的信息，请参考《<a href="../Resources/doc/WebInstruction.pdf" target="_blank">系统使用手册</a>》。
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:LinkButton ID="btnWebSite" runat="server" Text="点击进入系统" OnClick="btnGoToSite_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:WizardStep>
                        </WizardSteps>
                    </asp:Wizard>
                </asp:Panel>
                <asp:Panel ID="pnlPermission" runat="server" CssClass="content" Visible="false">
                    <div style="text-align:center;padding:10px 20px;font-size:11pt;font-weight:bold;">
                        <asp:Label ID="lblPermissionTile" runat="server" Text="文件/文件夹权限警告" />
                    </div>
                    <div style="text-align:left;padding:0 20px 30px 20px;font-size:10pt;">
                        <asp:Label ID="lblPermission" runat="server" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlButtons" runat="server" Visible="false">
                    <asp:Button ID="btnPermissionTest" runat="server" Width="100px" Text="测试权限" OnClick="btnPermissionTest_Click" />
                    <asp:Button ID="btnPermissionSkip" runat="server" Width="100px" Text="跳过" OnClick="btnPermissionSkip_Click" Visible="false"/>
                </asp:Panel>
                <asp:Panel ID="pnlPermissionSuccess" runat="server" Visible="false">
                    <div style="text-align:center; padding:20px 20px 30px 20px; font-size:11pt; font-weight:bold;">
                        <asp:Label ID="lblPermissionSuccess" runat="server" />
                    </div>
                    <asp:Button ID="btnPermissionContinue" runat="server" Width="100px" Text="继续" OnClick="btnPermissionContinue_Click" />
                </asp:Panel>
            </td>
            <td class="right">
            </td>
        </tr>
        <tr class="Bottom">
            <td class="left">
                &nbsp;
            </td>
            <td class="center">
                &nbsp;
            </td>
            <td class="right">
                &nbsp;
            </td>
        </tr>
    </table>
    <div class="container-footer">
        <div style="text-align: right; padding: 0px 0px 0px 0px;">
            <asp:Label ID="lblVersion" runat="server" />
        </div>
    </div>
    <asp:Panel ID="pnlError" runat="server" CssClass="container-error">
        <div style="text-align: left; padding: 0px 0px 0px 0px;">
            <asp:Label ID="lblError" runat="server" CssClass="error-label" />
        </div>
    </asp:Panel>
    </form>
</body>
</html>