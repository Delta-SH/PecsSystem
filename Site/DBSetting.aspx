<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DBSetting.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.DBSetting" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>数据源配置</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="DBSettingViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:FormPanel ID="DBSettingFormPanel" runat="server" Header="false" Padding="10" ButtonAlign="Right" Layout="FormLayout" Region="Center">
                <Defaults>
                    <ext:Parameter Name="MsgTarget" Value="side" />
                </Defaults>
                <Items>
                    <ext:ComboBox ID="DBTypeComboBox" runat="server" FieldLabel="数据库类型(T)" AnchorHorizontal="97%"
                        Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                        ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                        Resizable="true" Disabled="true">
                        <Store>
                            <ext:Store ID="DBTypeStore" runat="server" AutoLoad="true" OnRefreshData="OnDBTypeRefresh">
                                <Proxy>
                                    <ext:PageProxy />
                                </Proxy>
                                <Reader>
                                    <ext:JsonReader IDProperty="Id">
                                        <Fields>
                                            <ext:RecordField Name="Id" Type="String" />
                                            <ext:RecordField Name="Name" Type="String" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                                <Listeners>
                                    <Load Handler="#{DBTypeComboBox}.setValue(this.getAt(0).get('Id'));" />
                                </Listeners>
                            </ext:Store>
                        </Store>
                    </ext:ComboBox>
                    <ext:TextField ID="ServerNameTextField" runat="server" FieldLabel="服务器地址(IP)" AnchorHorizontal="97%"
                        AllowBlank="false" MaxLength="15" Regex="/^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/"
                        RegexText="格式错误" />
                    <ext:SpinnerField ID="ServerPortSpinnerField" runat="server" FieldLabel="数据库端口(P)" MinValue="1" MaxValue="65535" AllowDecimals="false" Text="1433" AnchorHorizontal="97%"/>
                    <ext:TextField ID="DBNameTextField" runat="server" FieldLabel="数据库名称(N)" AnchorHorizontal="97%"
                        AllowBlank="false" MaxLength="100" />
                    <ext:RadioGroup ID="DBAuthenticationRadioGroup" runat="server" FieldLabel="认证方式(A)"
                        AnchorHorizontal="97%">
                        <Items>
                            <ext:Radio ID="Radio1" runat="server" BoxLabel="SQL Server账户" InputValue="0" Checked="true">
                                <Listeners>
                                    <Check Handler="#{UIDTextField}.setDisabled(true);#{PWDTextField}.setDisabled(true);" />
                                </Listeners>
                            </ext:Radio>
                            <ext:Radio ID="Radio2" runat="server" BoxLabel="Windows认证" InputValue="1">
                                <Listeners>
                                    <Check Handler="#{UIDTextField}.setDisabled(false);#{PWDTextField}.setDisabled(false);" />
                                </Listeners>
                            </ext:Radio>
                        </Items>
                    </ext:RadioGroup>
                    <ext:TextField ID="UIDTextField" runat="server" FieldLabel="用户名(U)" AnchorHorizontal="97%"
                        AllowBlank="false" MaxLength="50" />
                    <ext:TextField ID="PWDTextField" runat="server" FieldLabel="密码(P)" AnchorHorizontal="97%"
                        InputType="Password" MinLength="3" MaxLength="50" />
                    <ext:SpinnerField ID="DBPortSpinnerField" runat="server" FieldLabel="通信端口(Port)"
                        AnchorHorizontal="97%" MinValue="0" MaxValue="65535" AllowDecimals="false" IncrementValue="1"
                        Number="1433" />
                </Items>
                <Buttons>
                    <ext:Button ID="ConBtn" runat="server" Text="测试">
                        <Listeners>
                            <Click Handler="if (#{DBSettingFormPanel}.getForm().isValid()){
                                                #{DBSettingStatusBar}.setStatus({text: LanguageSet.TestConnected, iconCls: 'icon-loading'});
                                                X.DBSetting.ConnectDBString({
                                                    success : function (result) {
                                                        if(!Ext.isEmpty(result, false)){
                                                            var msg = Ext.util.JSON.decode(result,true);
                                                            if(msg != null) {
                                                                if(msg.Status == 200){#{DBSettingStatusBar}.setStatus({text: msg.Msg, iconCls: 'icon-tick'});} else{#{DBSettingStatusBar}.setStatus({text: msg.Msg, iconCls: 'icon-exclamation'});}
                                                            }
                                                        }
                                                    }
                                                });
                                            } else{#{DBSettingStatusBar}.setStatus({text: LanguageSet.ErrorForm, iconCls: 'icon-exclamation'});}" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="SaveBtn" runat="server" Text="保存">
                        <Listeners>
                            <Click Handler="if (#{DBSettingFormPanel}.getForm().isValid()){
                                                if(window.confirm(LanguageSet.ExitConfirm)){
                                                    #{DBSettingStatusBar}.setStatus({text: LanguageSet.Saving, iconCls: 'icon-loading'});
                                                    X.DBSetting.SaveDBString({
                                                        success : function (result) {
                                                            if(!Ext.isEmpty(result, false)){
                                                                var msg = Ext.util.JSON.decode(result,true);
                                                                if(msg != null) {
                                                                    if(msg.Status == 200){#{DBSettingStatusBar}.setStatus({text: msg.Msg, iconCls: 'icon-tick'});
                                                                    } else{#{DBSettingStatusBar}.setStatus({text: msg.Msg, iconCls: 'icon-exclamation'});}
                                                                }
                                                            }
                                                        }
                                                    });
                                                }
                                            } else{#{DBSettingStatusBar}.setStatus({text: LanguageSet.ErrorForm, iconCls: 'icon-exclamation'});}" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="CancelBtn" runat="server" Text="取消">
                        <Listeners>
                            <Click Handler="parent.ReportWindow.close();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <BottomBar>
                    <ext:StatusBar ID="DBSettingStatusBar" runat="server" Height="25" AnchorHorizontal="100%" />
                </BottomBar>
            </ext:FormPanel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
