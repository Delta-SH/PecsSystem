<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ACVFilterSetting.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.ACVFilterSetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>综合测值表配置</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <script type="text/javascript" src="Resources/js/public.js?v=3.3.0.0"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="ACVSettingViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:Panel ID="ACVCenterPanel" runat="server" Margins="10 0 10 10" Padding="5" Region="Center">
                <TopBar>
                    <ext:Toolbar ID="ACVCenterToolbar1" runat="server">
                        <Items>
                            <ext:ToolbarSpacer ID="ACVCenterToolbarSpacer" runat="server" Width="5" />
                            <ext:ComboBox ID="LscsComboBox" runat="server" Width="200" LabelWidth="50" FieldLabel="Lsc名称"
                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                ForceSelection="true" TriggerAction="All" SelectOnFocus="true" Resizable="true">
                                <Store>
                                    <ext:Store ID="LscsStore" runat="server" AutoLoad="true" OnRefreshData="OnLscsRefresh">
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
                                            <Load Handler="if(this.getCount() > 0){ #{LscsComboBox}.setValueAndFireSelect(this.getAt(0).get('Id')); }" />
                                        </Listeners>
                                    </ext:Store>
                                </Store>
                                <Listeners>
                                    <Select Fn="createACVColumnTreePanel" />
                                </Listeners>
                            </ext:ComboBox>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:TreePanel ID="ColumnTreePanel" runat="server" RootVisible="false" AutoScroll="true"
                        ContainerScroll="true" Animate="true" EnableDD="true" Height="332" OnSubmit="SubmitNodes">
                        <Root>
                            <ext:TreeNode Text="ColumnNames" />
                        </Root>
                        <Listeners>
                            <Click Fn="onACVColumnTreeItemClick" />
                        </Listeners>
                    </ext:TreePanel>
                </Items>
                <BottomBar>
                    <ext:Toolbar ID="ACVCenterToolbar2" runat="server">
                        <Items>
                            <ext:Button ID="AddBtn" runat="server" Text="新增" Icon="Add">
                                <Listeners>
                                    <Click Fn="addACVColumnTreeNode" Buffer="200" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ACVCenterToolbarSeparator1" runat="server" />
                            <ext:Button ID="DelBtn" runat="server" Text="移除" Icon="Delete">
                                <Listeners>
                                    <Click Fn="removeACVColumnTreeNode" Buffer="200" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="ACVCenterToolbarSeparator2" runat="server" />
                            <ext:Button ID="ResetBtn" runat="server" Text="重置" Icon="ArrowRotateAnticlockwise">
                                <Listeners>
                                    <Click Fn="resetACVColumnTreeNode" Buffer="500" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill ID="ACVCenterToolbarFill" runat="server" />
                            <ext:Button ID="SaveBtn" runat="server" Text="保存" Icon="Disk">
                                <Listeners>
                                    <Click Handler="#{ColumnTreePanel}.submitNodes();" Buffer="500" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
            </ext:Panel>
            <ext:Panel ID="ACVEastPanel" runat="server" Title="列属性" Margins="10 10 10 0" Padding="5"
                Width="312" Split="true" Collapsible="false" CollapseMode="Mini" Region="East">
                <Items>
                    <ext:PropertyGrid ID="ColumnPropertyGrid" runat="server" Width="300" Height="360"
                        Hidden="true">
                        <Source>
                            <ext:PropertyGridParameter Name="ColName" Value="" DisplayName="列名称">
                            </ext:PropertyGridParameter>
                            <ext:PropertyGridParameter Name="DevName" Value="" DisplayName="设备名称">
                            </ext:PropertyGridParameter>
                            <ext:PropertyGridParameter Name="NodeType" Value="" DisplayName="测点类型">
                                <Editor>
                                    <ext:ComboBox ID="NodeTypeComboBox" runat="server" Editable="false" TypeAhead="true"
                                        Mode="Local" ForceSelection="true" TriggerAction="All" SelectOnFocus="true" Resizable="true"
                                        DisplayField="Name" ValueField="Id">
                                        <Store>
                                            <ext:Store ID="NodeTypeStore" runat="server" AutoLoad="true" OnRefreshData="OnNodeTypeRefresh">
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
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                </Editor>
                            </ext:PropertyGridParameter>
                            <ext:PropertyGridParameter Name="FilterType" Value="" DisplayName="筛选类型">
                                <Editor>
                                    <ext:ComboBox ID="FilterTypeComboBox" runat="server" Editable="false" TypeAhead="true"
                                        Mode="Local" ForceSelection="true" TriggerAction="All" SelectOnFocus="true" Resizable="true"
                                        DisplayField="Name" ValueField="Id">
                                        <Store>
                                            <ext:Store ID="FilterTypeStore" runat="server" AutoLoad="true" OnRefreshData="OnFilterTypeRefresh">
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
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                </Editor>
                            </ext:PropertyGridParameter>
                            <ext:PropertyGridParameter Name="FilterItem" Value="" DisplayName="筛选内容">
                                <Editor>
                                    <ext:TextArea ID="TextArea1" runat="server" Height="100">
                                    </ext:TextArea>
                                </Editor>
                            </ext:PropertyGridParameter>
                        </Source>
                        <View>
                            <ext:GridView ID="GridView1" runat="server" ForceFit="true" ScrollOffset="2" />
                        </View>
                        <Listeners>
                            <Render Handler="this.getStore().sortInfo = undefined;" />
                            <AfterEdit Fn="onACVColumnPropertyChanged" />
                        </Listeners>
                    </ext:PropertyGrid>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
