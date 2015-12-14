<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ACSFilterSetting.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.ACSFilterSetting" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>告警统计表配置</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <script type="text/javascript" src="Resources/js/public.js?v=3.3.0.0"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Store ID="ConditionStore" runat="server">
        <Reader>
            <ext:JsonReader IDProperty="Id">
                <Fields>
                    <ext:RecordField Name="Id" Type="String" />
                    <ext:RecordField Name="Name" Type="String" />
                </Fields>
            </ext:JsonReader>
        </Reader>
    </ext:Store>
    <ext:Viewport ID="TCSettingViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:Panel ID="TCCenterPanel" runat="server" Margins="10 0 10 10" Padding="5" Region="Center">
                <TopBar>
                    <ext:Toolbar ID="TCCenterToolbar1" runat="server">
                        <Items>
                            <ext:ToolbarSpacer ID="TCCenterToolbarSpacer" runat="server" Width="5" />
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
                                    <Select Fn="createColumnTreePanel" />
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
                            <Click Fn="onColumnTreeItemClick" />
                        </Listeners>
                    </ext:TreePanel>
                </Items>
                <BottomBar>
                    <ext:Toolbar ID="TCCenterToolbar2" runat="server">
                        <Items>
                            <ext:Button ID="AddBtn" runat="server" Text="新增" Icon="Add">
                                <Listeners>
                                    <Click Fn="addColumnTreeNode" Buffer="200" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="TCCenterToolbarSeparator1" runat="server" />
                            <ext:Button ID="DelBtn" runat="server" Text="移除" Icon="Delete">
                                <Listeners>
                                    <Click Fn="removeColumnTreeNode" Buffer="200" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarSeparator ID="TCCenterToolbarSeparator2" runat="server" />
                            <ext:Button ID="ResetBtn" runat="server" Text="重置" Icon="ArrowRotateAnticlockwise">
                                <Listeners>
                                    <Click Fn="resetColumnTreeNode" Buffer="500" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill ID="TCCenterToolbarFill" runat="server" />
                            <ext:Button ID="SaveBtn" runat="server" Text="保存" Icon="Disk">
                                <Listeners>
                                    <Click Handler="#{ColumnTreePanel}.submitNodes();" Buffer="500" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
            </ext:Panel>
            <ext:Panel ID="TCEastPanel" runat="server" Title="列属性" Margins="10 10 10 0" Padding="5"
                Width="312" Split="true" Collapsible="false" CollapseMode="Mini" Region="East">
                <Items>
                    <ext:PropertyGrid ID="ColumnPropertyGrid1" runat="server" Width="300" Height="360"
                        Hidden="true">
                        <Source>
                            <ext:PropertyGridParameter Name="ColName" Value="" DisplayName="列名称" />
                            <ext:PropertyGridParameter Name="FilterType" Value="" DisplayName="筛选类型">
                                <Editor>
                                    <ext:ComboBox ID="FilterTypeComboBox1" runat="server" Editable="false" TypeAhead="true"
                                        Mode="Local" ForceSelection="true" TriggerAction="All" SelectOnFocus="true" Resizable="true"
                                        DisplayField="Name" ValueField="Id">
                                        <Store>
                                            <ext:Store ID="FilterTypeStore1" runat="server" AutoLoad="true" OnRefreshData="OnFilterTypeRefresh1">
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
                            <ext:PropertyGridParameter Name="FilterItem" Value="" DisplayName="筛选条件">
                                <Editor>
                                    <ext:MultiCombo ID="ConditionMultiCombo" runat="server" StoreID="ConditionStore"
                                        SelectionMode="All" Delimiter=";" ValueField="Id" DisplayField="Name">
                                    </ext:MultiCombo>
                                </Editor>
                            </ext:PropertyGridParameter>
                        </Source>
                        <View>
                            <ext:GridView ID="GridView1" runat="server" ForceFit="true" ScrollOffset="2" />
                        </View>
                        <Listeners>
                            <Render Handler="this.getStore().sortInfo = undefined;" />
                            <AfterEdit Fn="onColumnPropertyChanged" />
                        </Listeners>
                    </ext:PropertyGrid>
                    <ext:PropertyGrid ID="ColumnPropertyGrid2" runat="server" Width="300" Height="360"
                        Hidden="true">
                        <Source>
                            <ext:PropertyGridParameter Name="ColName" Value="" DisplayName="列名称" />
                            <ext:PropertyGridParameter Name="FilterType" Value="" DisplayName="筛选类型">
                                <Editor>
                                    <ext:ComboBox ID="FilterTypeComboBox2" runat="server" Editable="false" TypeAhead="true"
                                        Mode="Local" ForceSelection="true" TriggerAction="All" SelectOnFocus="true" Resizable="true"
                                        DisplayField="Name" ValueField="Id">
                                        <Store>
                                            <ext:Store ID="FilterTypeStore2" runat="server" AutoLoad="true" OnRefreshData="OnFilterTypeRefresh2">
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
                            <ext:PropertyGridParameter Name="FilterItem1" Value="" DisplayName="最小时长(秒)">
                                <Editor>
                                    <ext:SpinnerField ID="SpinnerField1" runat="server" MinValue="0" AllowDecimals="false"
                                        IncrementValue="1" />
                                </Editor>
                            </ext:PropertyGridParameter>
                            <ext:PropertyGridParameter Name="FilterItem2" Value="" DisplayName="最大时长(秒)">
                                <Editor>
                                    <ext:SpinnerField ID="SpinnerField2" runat="server" MinValue="0" AllowDecimals="false"
                                        IncrementValue="1" />
                                </Editor>
                            </ext:PropertyGridParameter>
                        </Source>
                        <View>
                            <ext:GridView ID="GridView2" runat="server" ForceFit="true" ScrollOffset="2" />
                        </View>
                        <Listeners>
                            <Render Handler="this.getStore().sortInfo = undefined;" />
                            <AfterEdit Fn="onColumnPropertyChanged" />
                        </Listeners>
                    </ext:PropertyGrid>
                    <ext:PropertyGrid ID="ColumnPropertyGrid3" runat="server" Width="300" Height="360"
                        Hidden="true">
                        <Source>
                            <ext:PropertyGridParameter Name="ColName" Value="" DisplayName="列名称" />
                            <ext:PropertyGridParameter Name="FilterType" Value="" DisplayName="筛选类型">
                                <Editor>
                                    <ext:ComboBox ID="FilterTypeComboBox3" runat="server" Editable="false" TypeAhead="true"
                                        Mode="Local" ForceSelection="true" TriggerAction="All" SelectOnFocus="true" Resizable="true"
                                        DisplayField="Name" ValueField="Id">
                                        <Store>
                                            <ext:Store ID="FilterTypeStore3" runat="server" AutoLoad="true" OnRefreshData="OnFilterTypeRefresh3">
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
                                    <ext:TextArea ID="TextArea1" runat="server" Height="100" />
                                </Editor>
                            </ext:PropertyGridParameter>
                        </Source>
                        <View>
                            <ext:GridView ID="GridView3" runat="server" ForceFit="true" ScrollOffset="2" />
                        </View>
                        <Listeners>
                            <Render Handler="this.getStore().sortInfo = undefined;" />
                            <AfterEdit Fn="onColumnPropertyChanged" />
                        </Listeners>
                    </ext:PropertyGrid>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>