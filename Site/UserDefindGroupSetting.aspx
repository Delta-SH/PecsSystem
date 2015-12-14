<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDefindGroupSetting.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.UserDefindGroupSetting" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>自定义群组配置</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <script type="text/javascript" src="Resources/js/public.js?v=3.3.0.0"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="UDGSettingViewport" runat="server" Layout="BorderLayout" HideMode="Visibility" Hidden="true">
        <Items>
            <ext:TreePanel ID="UDGSettingWestPanel" runat="server" Width="250" Lines="true" Margins="5 0 5 5"
                Split="true" Collapsible="false" CollapseMode="Mini" ContainerScroll="true" EnableDD="true"
                AutoScroll="true" Region="West" OnSubmit="UDGSettingWestPanelSubmitNodes">
                <TopBar>
                    <ext:Toolbar ID="UDGSettingWestTopBar" runat="server">
                        <Items>
                            <ext:TextField ID="UDGroupName" runat="server" FieldLabel="群组名称" LabelWidth="50"
                                Width="240" MaxLength="50" AllowBlank="false" EnableKeyEvents="true">
                                <Listeners>
                                    <KeyUp Fn="UDGSetting.setUDGroupTreeName" Scope="UDGSetting" Buffer="500" />
                                </Listeners>
                            </ext:TextField>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Root>
                    <ext:TreeNode Text="Undefined Node" />
                </Root>
                <Listeners>
                    <BeforeNodeDrop Fn="UDGSetting.dragUDGGroupNode" Scope="UDGSetting" />
                    <AfterRender Fn="UDGSetting.setUDGroupName" Scope="UDGSetting" />
                </Listeners>
                <BottomBar>
                    <ext:Toolbar ID="UDGSettingWestBottomBar" runat="server">
                        <Items>
                            <ext:Button ID="PreviewBtn" runat="server" Text="预览" Icon="Eye">
                                <Listeners>
                                    <Click Fn="UDGSetting.previewUDGroupTree" Scope="UDGSetting" Buffer="500" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill ID="DGSettingWestBottomBarFill" runat="server" />
                            <ext:Button ID="SaveBtn" runat="server" Text="保存" Icon="Disk">
                                <Listeners>
                                    <Click Handler="UDGSetting.previewUDGroupTree();#{UDGSettingWestPanel}.submitNodes();"
                                        Buffer="500" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
            </ext:TreePanel>
            <ext:TreePanel ID="UDGSettingCenterPanel" runat="server" Margins="5 5 5 0" Padding="5"
                Lines="true" ContainerScroll="true" AutoScroll="true" Region="Center">
                <TopBar>
                    <ext:Toolbar ID="UDGSettingCenterTopBar" runat="server" Layout="ContainerLayout">
                        <Items>
                            <ext:Toolbar ID="UDGSettingCenterTopBar1" runat="server" Flat="true">
                                <Items>
                                    <ext:ComboBox ID="StaTypeComboBox" runat="server" FieldLabel="局站类型" LabelWidth="55"
                                        Width="180" ValueField="Id" DisplayField="Name" Editable="true" TypeAhead="true"
                                        Mode="Local" ForceSelection="true" SelectOnFocus="true" TriggerAction="All" EmptyText="Loading..."
                                        Resizable="true">
                                        <Store>
                                            <ext:Store ID="StaTypeStore" runat="server" AutoLoad="true" OnRefreshData="OnStaTypeRefresh">
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
                                                    <Load Handler="#{StaTypeComboBox}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="DevTypeComboBox" runat="server" FieldLabel="设备类型" LabelWidth="55"
                                        Width="180" ValueField="Id" DisplayField="Name" Editable="true" TypeAhead="true"
                                        Mode="Local" ForceSelection="true" SelectOnFocus="true" TriggerAction="All" EmptyText="Loading..."
                                        Resizable="true">
                                        <Store>
                                            <ext:Store ID="DevTypeStore" runat="server" AutoLoad="true" OnRefreshData="OnDevTypeRefresh">
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
                                                    <Load Handler="#{DevTypeComboBox}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar ID="UDGSettingCenterTopBar2" runat="server" Flat="true">
                                <Items>
                                    <ext:ComboBox ID="FilterTypeComboBox" runat="server" FieldLabel="节点过滤" LabelWidth="55"
                                        Width="180" Resizable="true">
                                        <Items>
                                            <ext:ListItem Text="按局站名称" Value="0" />
                                            <ext:ListItem Text="按设备名称" Value="1" />
                                        </Items>
                                        <SelectedItem Value="0" />
                                    </ext:ComboBox>
                                    <ext:TextField ID="FilterTextField" runat="server" Width="180" AllowBlank="true">
                                        <Listeners>
                                            <Change Fn="UDGSetting.setGroupNodesFilter" Scope="UDGSetting" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:ToolbarSpacer ID="UDGSettingCenterToolbarSpacer1" runat="server" Width="5" />
                                    <ext:Button ID="OK_Btn" runat="server" Text="确定" StandOut="true">
                                        <Listeners>
                                            <Click Fn="UDGSetting.setGroupNodesFilter" Scope="UDGSetting" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Root>
                    <ext:TreeNode Text="Undefined Node" />
                </Root>
                <BottomBar>
                    <ext:Toolbar ID="UDGSettingCenterBottomBar" runat="server">
                        <Items>
                            <ext:Checkbox ID="selectAll" runat="server" BoxLabel="全选子节点" Height="24" />
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
                <Listeners>
                    <CheckChange Fn="UDGSetting.groupNodesCheckChange" Scope="UDGSetting" />
                </Listeners>
            </ext:TreePanel>
            <ext:Hidden ID="LscIDHidden" runat="server" />
            <ext:Hidden ID="UDGroupIDHidden" runat="server" />
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
