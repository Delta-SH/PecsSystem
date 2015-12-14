<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LSCManager.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.LSCManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Lsc管理</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link rel="icon" type="image/x-icon" href="favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="favicon.ico" />
    <script type="text/javascript" src="Resources/js/public.js?v=3.3.0.0"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="LSCViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="LSCPanel" runat="server" StripeRows="true" TrackMouseOver="true"
                AutoScroll="true" Region="Center" Height="500px" Title="Lsc管理" Icon="ApplicationViewList">
                <TopBar>
                    <ext:Toolbar ID="LSCToolbars" runat="server">
                        <Items>
                            <ext:Button ID="AddBtn" runat="server" Text="新增" Icon="Add">
                                <Listeners>
                                    <Click Fn="onLSCPanelAddClick" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server" />
                            <ext:Button ID="RefreshBtn" runat="server" Text="刷新" Icon="TableRefresh">
                                <Listeners>
                                    <Click Fn="onLSCPanelRefreshClick" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server" />
                            <ext:Button ID="RestartBtn" runat="server" Text="重启服务" Icon="ServerStart">
                                <DirectEvents>
                                    <Click OnEvent="RestartBtn_Click">
                                        <Confirmation ConfirmRequest="true" Title="确认对话框" Message="您确定要重启服务吗？"/>
                                        <EventMask ShowMask="true" Msg="正在下发重启服务命令..." Target="CustomTarget" CustomTarget="#{LSCPanel}.body.up('div')" />
                                    </Click>
                                </DirectEvents>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store ID="LSCStore" runat="server" OnRefreshData="OnLSCListRefresh" AutoSave="true">
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                        <Reader>
                            <ext:JsonReader IDProperty="LscID">
                                <Fields>
                                    <ext:RecordField Name="LscID" Type="Int" />
                                    <ext:RecordField Name="LscName" Type="String" />
                                    <ext:RecordField Name="LscIP" Type="String" />
                                    <ext:RecordField Name="LscPort" Type="Int" />
                                    <ext:RecordField Name="LscUID" Type="String" />
                                    <ext:RecordField Name="LscPwd" Type="String" />
                                    <ext:RecordField Name="BeatInterval" Type="Int" />
                                    <ext:RecordField Name="BeatDelay" Type="Int" />
                                    <ext:RecordField Name="DBServer" Type="String" />
                                    <ext:RecordField Name="DBPort" Type="Int" />
                                    <ext:RecordField Name="DBName" Type="String" />
                                    <ext:RecordField Name="DBUID" Type="String" />
                                    <ext:RecordField Name="DBPwd" Type="String" />
                                    <ext:RecordField Name="HisDBServer" Type="String" />
                                    <ext:RecordField Name="HisDBPort" Type="Int" />
                                    <ext:RecordField Name="HisDBName" Type="String" />
                                    <ext:RecordField Name="HisDBUID" Type="String" />
                                    <ext:RecordField Name="HisDBPwd" Type="String" />
                                    <ext:RecordField Name="Connected" Type="Boolean" />
                                    <ext:RecordField Name="ChangedTime" Type="String" />
                                    <ext:RecordField Name="Enabled" Type="Boolean" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                        <BaseParams>
                            <ext:Parameter Name="start" Value="0" Mode="Raw" />
                            <ext:Parameter Name="limit" Value="#{LSCPageSizeComboBox}.getValue()" Mode="Raw" />
                        </BaseParams>
                    </ext:Store>
                </Store>
                <LoadMask ShowMask="true" />
                <ColumnModel ID="LSCColumnModel" runat="server">
                    <Columns>
                        <ext:CommandColumn Header="操作" Align="Center" ButtonAlign="Center" Width="200">
                            <Commands>
                                <ext:GridCommand Icon="NoteEdit" Text="编辑" CommandName="Edit">
                                    <ToolTip Text="编辑行" />
                                </ext:GridCommand>
                                <ext:GridCommand Icon="Delete" Text="删除" CommandName="Del">
                                    <ToolTip Text="删除行" />
                                </ext:GridCommand>
                                <ext:GridCommand Icon="DatabaseRefresh" Text="数据同步" CommandName="Sync">
                                    <ToolTip Text="数据同步" />
                                </ext:GridCommand>
                            </Commands>
                        </ext:CommandColumn>
                        <ext:Column Header="编号" DataIndex="LscID" Align="Left" Width="50" />
                        <ext:Column Header="名称" DataIndex="LscName" Align="Left" />
                        <ext:Column Header="通信地址" DataIndex="LscIP" Align="Left" />
                        <ext:Column Header="通信端口" DataIndex="LscPort" Align="Left" />
                        <ext:Column Header="登录账户" DataIndex="LscUID" Align="Left" />
                        <ext:Column Header="登录密码" DataIndex="LscPwd" Align="Center">
                            <Renderer Handler="return '******'" />
                        </ext:Column>
                        <ext:Column Header="通信检测延迟" DataIndex="BeatInterval" Align="Left" />
                        <ext:Column Header="通信中断延迟" DataIndex="BeatDelay" Align="Left" />
                        <ext:Column Header="配置库地址" DataIndex="DBServer" Align="Left" />
                        <ext:Column Header="配置库端口" DataIndex="DBPort" Align="Left" />
                        <ext:Column Header="配置库名称" DataIndex="DBName" Align="Left" />
                        <ext:Column Header="配置库账户" DataIndex="DBUID" Align="Left" />
                        <ext:Column Header="配置库密码" DataIndex="DBPwd" Align="Center">
                            <Renderer Handler="return '******'" />
                        </ext:Column>
                        <ext:Column Header="历史库地址" DataIndex="HisDBServer" Align="Left" />
                        <ext:Column Header="历史库端口" DataIndex="HisDBPort" Align="Left" />
                        <ext:Column Header="历史库名称" DataIndex="HisDBName" Align="Left" />
                        <ext:Column Header="历史库账户" DataIndex="HisDBUID" Align="Left" />
                        <ext:Column Header="历史库密码" DataIndex="HisDBPwd" Align="Center">
                            <Renderer Handler="return '******'" />
                        </ext:Column>
                        <ext:Column Header="通信状态" DataIndex="Connected" Align="Center">
                            <Renderer Fn="setLSCStatus" />
                        </ext:Column>
                        <ext:Column Header="连接/中断时间" DataIndex="ChangedTime" Align="Center" />
                        <ext:CheckColumn Header="启用状态" DataIndex="Enabled" Align="Center" Width="60" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="LSCRowSelectionModel" runat="server" SingleSelect="true" />
                </SelectionModel>
                <View>
                    <ext:GridView ID="LSCGridView" runat="server" ForceFit="false" />
                </View>
                <Listeners>
                    <Command Fn="onLSCPanelCmdClick"/>
                    <RowContextMenu Fn="onLSCGridRowContextMenuShow" />
                </Listeners>
                <BottomBar>
                    <ext:PagingToolbar ID="LSCPagingToolbar" runat="server" PageSize="20" StoreID="LSCStore">
                        <Items>
                            <ext:ComboBox ID="LSCPageSizeComboBox" runat="server" Width="150" LabelWidth="60"
                                FieldLabel="<%$ Resources: GlobalResource, ComboBoxPageSize %>">
                                <Items>
                                    <ext:ListItem Text="10" Value="10" />
                                    <ext:ListItem Text="20" Value="20" />
                                    <ext:ListItem Text="50" Value="50" />
                                    <ext:ListItem Text="100" Value="100" />
                                    <ext:ListItem Text="200" Value="200" />
                                    <ext:ListItem Text="500" Value="500" />
                                </Items>
                                <SelectedItem Value="20" />
                                <Listeners>
                                    <Select Handler="#{LSCPagingToolbar}.pageSize = parseInt(this.getValue()); #{LSCPagingToolbar}.doLoad();" />
                                </Listeners>
                            </ext:ComboBox>
                        </Items>
                    </ext:PagingToolbar>
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    <ext:Menu ID="LSCGridRowMenu" runat="server">
        <Items>
            <ext:MenuItem ID="LSCGridRowItem1" runat="server" Text="编辑" Icon="NoteEdit">
                <Listeners>
                    <Click Fn="onLSCGridRowEditClick" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuSeparator />
            <ext:MenuItem ID="LSCGridRowItem2" runat="server" Text="删除" Icon="Delete">
                <Listeners>
                    <Click Fn="onLSCGridRowDeleteClick" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuSeparator />
            <ext:MenuItem ID="LSCGridRowItem3" runat="server" Text="数据同步" Icon="DatabaseRefresh">
                <Listeners>
                    <Click Fn="onLSCGridRowSyncClick" />
                </Listeners>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    <ext:Window ID="LSCWindow" runat="server" Height="370" Width="650" Collapsible="False"
        Modal="true" InitCenter="true" Padding="10" BodyStyle="background:#fff;" Layout="ColumnLayout"
        Hidden="true">
        <Items>
            <ext:FormPanel ID="FormPanel1" runat="server" Border="false" Header="false" ColumnWidth=".5" Layout="FormLayout" LabelWidth="100">
                <Defaults>
                    <ext:Parameter Name="MsgTarget" Value="side" />
                </Defaults>
                <Items>
                    <ext:NumberField ID="LSCIDField" runat="server" FieldLabel="Lsc编号" AnchorHorizontal="92%" AllowBlank="false" MinValue="0" />
                    <ext:TextField ID="LSCIPField" runat="server" FieldLabel="通信地址" AnchorHorizontal="92%" AllowBlank="false" MaxLength="15" Regex="/^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/" RegexText="格式错误" />
                    <ext:TextField ID="LSCUIDField" runat="server" FieldLabel="登录账户" AnchorHorizontal="92%" AllowBlank="false" MaxLength="50" />
                    <ext:NumberField ID="BeatIntervalField" runat="server" FieldLabel="通信检测延迟(s)" AnchorHorizontal="92%" AllowBlank="false" MinValue="0" AllowDecimals="false" />
                    <ext:TextField ID="DBIPField" runat="server" FieldLabel="配置库地址" AnchorHorizontal="92%" AllowBlank="false" MaxLength="15" Regex="/^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/" RegexText="格式错误" />
                    <ext:TextField ID="DBNameField" runat="server" FieldLabel="配置库名称" AnchorHorizontal="92%" AllowBlank="false" MaxLength="100" />
                    <ext:TextField ID="DBPwdField" runat="server" FieldLabel="配置库密码" AnchorHorizontal="92%" MaxLength="50" InputType="Password" />
                    <ext:SpinnerField ID="HisDBPortField" runat="server" FieldLabel="历史库端口" AnchorHorizontal="92%" MinValue="1" MaxValue="65535" AllowDecimals="false" IncrementValue="1" AllowBlank="false" Text="1433" />
                    <ext:TextField ID="HisDBUIDField" runat="server" FieldLabel="历史库账户" AnchorHorizontal="92%" AllowBlank="false" MaxLength="50" />
                    <ext:Checkbox ID="EnabledCheckbox" runat="server" FieldLabel="启用状态" BoxLabel="勾选表示启用" AnchorHorizontal="92%" LabelAlign="Left" />
                </Items>
            </ext:FormPanel>
            <ext:FormPanel ID="FormPanel2" runat="server" Border="false" Header="false" ColumnWidth=".5" Layout="FormLayout" LabelWidth="100">
                <Defaults>
                    <ext:Parameter Name="MsgTarget" Value="side" />
                </Defaults>
                <Items>
                    <ext:TextField ID="LSCNameField" runat="server" FieldLabel="Lsc名称" AnchorHorizontal="92%" AllowBlank="false" MaxLength="50" />
                    <ext:SpinnerField ID="LSCPortField" runat="server" FieldLabel="通信端口" AnchorHorizontal="92%" MinValue="1" MaxValue="65535" AllowDecimals="false" IncrementValue="1" AllowBlank="false" Text="7000" />
                    <ext:TextField ID="LSCPwdField" runat="server" FieldLabel="登录密码" AnchorHorizontal="92%" AllowBlank="false" MinLength="3" MaxLength="50" InputType="Password" />
                    <ext:NumberField ID="BeatDelayField" runat="server" FieldLabel="通信中断延迟(s)" AnchorHorizontal="92%" AllowBlank="false" MinValue="0" AllowDecimals="false" />
                    <ext:SpinnerField ID="DBPortField" runat="server" FieldLabel="配置库端口" AnchorHorizontal="92%" MinValue="1" MaxValue="65535" AllowDecimals="false" IncrementValue="1" AllowBlank="false" Text="1433" />
                    <ext:TextField ID="DBUIDField" runat="server" FieldLabel="配置库账户" AnchorHorizontal="92%" AllowBlank="false" MaxLength="50" />
                    <ext:TextField ID="HisDBIPField" runat="server" FieldLabel="历史库地址" AnchorHorizontal="92%" AllowBlank="false" MaxLength="15" Regex="/^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/" RegexText="格式错误" />
                    <ext:TextField ID="HisDBNameField" runat="server" FieldLabel="历史库名称" AnchorHorizontal="92%" AllowBlank="false" MaxLength="100" />
                    <ext:TextField ID="HisDBPwdField" runat="server" FieldLabel="历史库密码" AnchorHorizontal="92%" MaxLength="50" InputType="Password" />
                </Items>
            </ext:FormPanel>
        </Items>
        <Buttons>
            <ext:Button ID="SaveBtn" runat="server" Text="保存">
                <Listeners>
                    <Click Handler="var b1 = #{FormPanel1}.getForm().isValid();
                                    var b2 = #{FormPanel2}.getForm().isValid();
                                    if (b1 && b2){
                                        #{LSCStatusBar}.setStatus({text: LanguageSet.Saving, iconCls: 'icon-loading'});
                                        X.LSCManager.SaveLsc({
                                            success : function (result) {
                                                if(!Ext.isEmpty(result, false)){
                                                    var msg = Ext.util.JSON.decode(result,true);
                                                    if(msg != null) {
                                                        if(msg.Status == 200){
                                                            #{LSCStatusBar}.setStatus({text: msg.Msg, iconCls: 'icon-tick'});
                                                            #{LSCPagingToolbar}.doLoad(#{LSCPagingToolbar}.cursor);
                                                        }
                                                        else if(msg.Status == 100){
                                                            #{LSCIDField}.markInvalid(msg.Msg);
                                                            #{LSCStatusBar}.setStatus({text: msg.Msg, iconCls: 'icon-exclamation'});
                                                        }
                                                        else{
                                                            #{LSCStatusBar}.setStatus({text: msg.Msg, iconCls: 'icon-exclamation'});
                                                        }
                                                    }
                                                }
                                            }
                                        });
                                    }
                                    else{
                                        #{LSCStatusBar}.setStatus({text: LanguageSet.ErrorForm, iconCls: 'icon-exclamation'});
                                    }" />
                </Listeners>
            </ext:Button>
            <ext:Button ID="EditBtn" runat="server" Text="更新">
                <Listeners>
                    <Click Handler="var b1 = #{FormPanel1}.getForm().isValid();
                                    var b2 = #{FormPanel2}.getForm().isValid();
                                    if (b1 && b2){
                                        #{LSCStatusBar}.setStatus({text: LanguageSet.Updating, iconCls: 'icon-loading'});
                                        X.LSCManager.UpdateLsc({
                                            success : function (result) {
                                                if(!Ext.isEmpty(result, false)){
                                                    var msg = Ext.util.JSON.decode(result,true);
                                                    if(msg != null) {
                                                        if(msg.Status == 200){
                                                            #{LSCStatusBar}.setStatus({text: msg.Msg, iconCls: 'icon-tick'});
                                                            #{LSCPagingToolbar}.doLoad(#{LSCPagingToolbar}.cursor);
                                                        }
                                                        else{
                                                            #{LSCStatusBar}.setStatus({text: msg.Msg, iconCls: 'icon-exclamation'});
                                                        }
                                                    }
                                                }
                                            }
                                        });
                                    }
                                    else{
                                        #{LSCStatusBar}.setStatus({text: LanguageSet.ErrorForm, iconCls: 'icon-exclamation'});
                                    }" />
                </Listeners>
            </ext:Button>
            <ext:Button ID="CancelBtn" runat="server" Text="取消">
                <Listeners>
                    <Click Handler="#{LSCWindow}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
        <BottomBar>
            <ext:StatusBar ID="LSCStatusBar" runat="server" Height="25" />
        </BottomBar>
    </ext:Window>
    <ext:Window ID="SyncWindow" runat="server" Height="195" Width="300" Collapsible="False"
        Modal="true" InitCenter="true" Padding="10" Hidden="true">
        <Items>
            <ext:RadioGroup ID="SyncRadioGroup" runat="server" ColumnsNumber="1" FieldLabel="选择数据同步方式" LabelAlign="Top">
                <Items>
                    <ext:Radio ID="STRadio" runat="server" BoxLabel="仅同步配置信息" InputValue="ST" Checked="true" />
                    <ext:Radio ID="TCRadio" runat="server" BoxLabel="仅同步标准化信息" InputValue="TC" />
                    <ext:Radio ID="CARadio" runat="server" BoxLabel="同步配置信息并清空告警" InputValue="CA" />
                </Items>
            </ext:RadioGroup>
            <ext:Hidden ID="SyncWindowHF" runat="server" />
        </Items>
        <Buttons>
            <ext:Button ID="SyncBtn" runat="server" Text="确定">
                <DirectEvents>
                    <Click OnEvent="SyncBtn_Click" Before="#{SyncWindow}.hide();">
                        <Confirmation ConfirmRequest="true" Title="确认对话框" Message="您确定要同步数据吗？"/>
                        <EventMask ShowMask="true" Msg="正在下发数据同步命令..." Target="CustomTarget" CustomTarget="#{LSCPanel}.body.up('div')" />
                    </Click>
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="CancelSyncBtn" runat="server" Text="取消">
                <Listeners>
                    <Click Handler="#{SyncWindow}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    </form>
</body>
</html>
