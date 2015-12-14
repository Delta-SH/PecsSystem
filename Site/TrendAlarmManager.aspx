<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrendAlarmManager.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.TrendAlarmManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>趋势预警</title>
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
    <ext:Viewport ID="MainViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:TabPanel ID="MainTabPanel" runat="server" Margins="5 0 0 0" Border="false" MinTabWidth="50" EnableTabScroll="true" Region="Center">
                <Items>
                    <ext:GridPanel ID="TrendGridPanel" runat="server" Header="false" Title="实时预警信息" Icon="TagRed" StripeRows="true" TrackMouseOver="true" Layout="FitLayout">
                        <TopBar>
                            <ext:Toolbar ID="TrendGridToolbars" runat="server" Layout="ContainerLayout">
                                <Items>
                                    <ext:Toolbar ID="TrendGridToolbar1" runat="server" Flat="true">
                                        <Items>
                                            <ext:ComboBox ID="LscsComboBox1" runat="server" Width="180" LabelWidth="50" FieldLabel="Lsc名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="LscsStore1" runat="server" AutoLoad="true" OnRefreshData="OnLscsRefresh1">
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
                                                            <Load Handler="#{LscsComboBox1}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="delete #{TrendGridToolbars}.loaded1; #{Area2ComboBox1}.clearValue();#{Area2ComboBox1}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="Area2ComboBox1" runat="server" Width="180" LabelWidth="50" FieldLabel="地区名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="Area2Store1" runat="server" AutoLoad="false" OnRefreshData="OnArea2Refresh1">
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
                                                            <Load Handler="#{Area2ComboBox1}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="delete #{TrendGridToolbars}.loaded1; #{Area3ComboBox1}.clearValue(); #{Area3ComboBox1}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="Area3ComboBox1" runat="server" Width="180" LabelWidth="50" FieldLabel="县市名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="Area3Store1" runat="server" AutoLoad="false" OnRefreshData="OnArea3Refresh1">
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
                                                            <Load Handler="#{Area3ComboBox1}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="delete #{TrendGridToolbars}.loaded1; #{StaComboBox1}.clearValue(); #{StaComboBox1}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="StaComboBox1" runat="server" Width="180" LabelWidth="50" FieldLabel="局站名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="StaStore1" runat="server" AutoLoad="false" OnRefreshData="OnStaRefresh1">
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
                                                            <Load Handler="#{StaComboBox1}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="delete #{TrendGridToolbars}.loaded1; #{DevComboBox1}.clearValue(); #{DevComboBox1}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar ID="TrendGridToolbar2" runat="server" Flat="true">
                                        <Items>
                                            <ext:ComboBox ID="DevComboBox1" runat="server" Width="180" LabelWidth="50" FieldLabel="设备名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="DevStore1" runat="server" AutoLoad="false" OnRefreshData="OnDevRefresh1">
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
                                                            <Load Handler="#{DevComboBox1}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="delete #{TrendGridToolbars}.loaded1; #{NodeComboBox1}.clearValue(); #{NodeComboBox1}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="NodeComboBox1" runat="server" Width="180" LabelWidth="50" FieldLabel="测点名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="NodeStore1" runat="server" AutoLoad="false" OnRefreshData="OnNodeRefresh1">
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
                                                            <Load Handler="#{NodeComboBox1}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="#{TrendGridToolbars}.loaded1 = true; TrendMgr.LoadPageTrendAlarms();" Scope="TrendMgr" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:MultiCombo ID="DevTypeMultiCombo1" runat="server" Width="180" SelectionMode="All"
                                                LabelWidth="50" FieldLabel="预警设备" EmptyText="..." DisplayField="Name"
                                                ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true">
                                                <Store>
                                                    <ext:Store ID="DevTypeStore1" runat="server" AutoLoad="true" OnRefreshData="OnDevTypeRefresh1">
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
                                                            <Load Handler="#{DevTypeMultiCombo1}.selectAll(); #{TrendGridToolbars}.loaded2=true; TrendMgr.LoadPageTrendAlarms();" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Change Handler="TrendMgr.LoadPageTrendAlarms();" Scope="TrendMgr" />
                                                </Listeners>
                                            </ext:MultiCombo>
                                            <ext:MultiCombo ID="AlarmLevelMultiCombo1" runat="server" Width="180" SelectionMode="All"
                                                LabelWidth="50" FieldLabel="预警级别" EmptyText="..." DisplayField="Name"
                                                ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true">
                                                <Store>
                                                    <ext:Store ID="AlarmLevelStore1" runat="server" AutoLoad="true" OnRefreshData="OnAlarmLevelRefresh1">
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
                                                            <Load Handler="#{AlarmLevelMultiCombo1}.selectAll(); #{TrendGridToolbars}.loaded3=true; TrendMgr.LoadPageTrendAlarms();" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Change Handler="TrendMgr.LoadPageTrendAlarms();" Scope="TrendMgr" />
                                                </Listeners>
                                            </ext:MultiCombo>
                                        </Items>
                                    </ext:Toolbar>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh1" runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="TrendGridStore" runat="server" AutoLoad="false" OnRefreshData="OnTrendGridRefresh">
                                <Proxy>
                                    <ext:PageProxy />
                                </Proxy>
                                <Reader>
                                    <ext:JsonReader IDProperty="ID">
                                        <Fields>
                                            <ext:RecordField Name="ID" Type="Int" />
                                            <ext:RecordField Name="LscID" Type="Int" />
                                            <ext:RecordField Name="LscName" Type="String" />
                                            <ext:RecordField Name="Area1Name" Type="String" />
                                            <ext:RecordField Name="Area2Name" Type="String" />
                                            <ext:RecordField Name="Area3Name" Type="String" />
                                            <ext:RecordField Name="StaName" Type="String" />
                                            <ext:RecordField Name="DevName" Type="String" />
                                            <ext:RecordField Name="NodeID" Type="Int" />
                                            <ext:RecordField Name="NodeName" Type="String" />
                                            <ext:RecordField Name="DevTypeID" Type="Int" />
                                            <ext:RecordField Name="DevTypeName" Type="String" />
                                            <ext:RecordField Name="AlarmLevel" Type="Int" />
                                            <ext:RecordField Name="AlarmLevelName" Type="String" />
                                            <ext:RecordField Name="AlarmType" Type="String" />
                                            <ext:RecordField Name="StartValue" Type="String" />
                                            <ext:RecordField Name="AlarmValue" Type="String" />
                                            <ext:RecordField Name="DiffValue" Type="String" />
                                            <ext:RecordField Name="StartTime" Type="String" />
                                            <ext:RecordField Name="AlarmTime" Type="String" />
                                            <ext:RecordField Name="ConfirmName" Type="String" />
                                            <ext:RecordField Name="ConfirmTime" Type="String" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                                <BaseParams>
                                    <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                    <ext:Parameter Name="limit" Value="#{PageSizeComboBox}.getValue()" Mode="Raw" />
                                </BaseParams>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="TrendGridColumnModel" runat="server">
                            <Columns>
                                <ext:Column Header="序号" DataIndex="ID" Align="Left" Width="50"/>
                                <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Center" />
                                <ext:Column Header="地区名称" DataIndex="Area2Name" Align="Center" />
                                <ext:Column Header="县市名称" DataIndex="Area3Name" Align="Center" />
                                <ext:Column Header="局站名称" DataIndex="StaName" Align="Center" />
                                <ext:Column Header="设备名称" DataIndex="DevName" Align="Center" />
                                <ext:Column Header="测点名称" DataIndex="NodeName" Align="Center" />
                                <ext:Column Header="预警设备" DataIndex="DevTypeName" Align="Center" />
                                <ext:Column Header="预警级别" DataIndex="AlarmLevelName" Align="Center" />
                                <ext:Column Header="预警类别" DataIndex="AlarmType" Align="Center" />
                                <ext:Column Header="起始值" DataIndex="StartValue" Align="Left" />
                                <ext:Column Header="触发值" DataIndex="AlarmValue" Align="Left" />
                                <ext:Column Header="差值" DataIndex="DiffValue" Align="Left" />
                                <ext:Column Header="开始时间" DataIndex="StartTime" Align="Center" />
                                <ext:Column Header="触发时间" DataIndex="AlarmTime" Align="Center" />
                                <ext:Column Header="确认人员" DataIndex="ConfirmName" Align="Center" />
                                <ext:Column Header="确认时间" DataIndex="ConfirmTime" Align="Center" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="TrendGridRowSelectionModel" runat="server" SingleSelect="true" />
                        </SelectionModel>
                        <View>
                             <ext:GridView ID="TrendGridView" runat="server" ForceFit="false">
                                <GetRowClass Fn="TrendMgr.GetGridRowClass" />
                             </ext:GridView>
                        </View>
                        <Listeners>
                            <RowContextMenu Fn="TrendMgr.ShowGridRowMenu" Scope="TrendMgr" />
                        </Listeners>
                        <BottomBar>
                            <ext:PagingToolbar ID="TrendGridPagingToolbar" runat="server" PageSize="20" StoreID="TrendGridStore">
                                <Items>
                                    <ext:ComboBox ID="PageSizeComboBox" runat="server" Width="150" LabelWidth="60" FieldLabel="<%$ Resources: GlobalResource, ComboBoxPageSize %>">
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
                                            <Select Handler="#{TrendGridPagingToolbar}.pageSize = parseInt(this.getValue()); #{TrendGridPagingToolbar}.doLoad();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                    <ext:GridPanel ID="TrendCountGridPanel" runat="server" Header="false" Title="统计预警信息" Icon="TagRed" StripeRows="true" TrackMouseOver="true" Layout="FitLayout">
                        <TopBar>
                            <ext:Toolbar ID="TrendCountGridToolbars" runat="server" Layout="ContainerLayout">
                                <Items>
                                    <ext:Toolbar ID="TrendCountGridToolbar1" runat="server" Flat="true">
                                        <Items>
                                            <ext:ComboBox ID="LscsComboBox2" runat="server" Width="180" LabelWidth="50" FieldLabel="Lsc名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="LscsStore2" runat="server" AutoLoad="true" OnRefreshData="OnLscsRefresh2">
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
                                                            <Load Handler="#{LscsComboBox2}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="#{Area2ComboBox2}.clearValue();#{Area2ComboBox2}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="Area2ComboBox2" runat="server" Width="180" LabelWidth="50" FieldLabel="地区名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="Area2Store2" runat="server" AutoLoad="false" OnRefreshData="OnArea2Refresh2">
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
                                                            <Load Handler="#{Area2ComboBox2}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="#{Area3ComboBox2}.clearValue(); #{Area3ComboBox2}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="Area3ComboBox2" runat="server" Width="180" LabelWidth="50" FieldLabel="县市名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="Area3Store2" runat="server" AutoLoad="false" OnRefreshData="OnArea3Refresh2">
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
                                                            <Load Handler="#{Area3ComboBox2}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="#{StaComboBox2}.clearValue(); #{StaComboBox2}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="StaComboBox2" runat="server" Width="180" LabelWidth="50" FieldLabel="局站名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="StaStore2" runat="server" AutoLoad="false" OnRefreshData="OnStaRefresh2">
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
                                                            <Load Handler="#{StaComboBox2}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="#{DevComboBox2}.clearValue(); #{DevComboBox2}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar ID="TrendCountGridToolbar2" runat="server" Flat="true">
                                        <Items>
                                            <ext:ComboBox ID="DevComboBox2" runat="server" Width="180" LabelWidth="50" FieldLabel="设备名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="DevStore2" runat="server" AutoLoad="false" OnRefreshData="OnDevRefresh2">
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
                                                            <Load Handler="#{DevComboBox2}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="#{NodeComboBox2}.clearValue(); #{NodeComboBox2}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="NodeComboBox2" runat="server" Width="180" LabelWidth="50" FieldLabel="测点名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="NodeStore2" runat="server" AutoLoad="false" OnRefreshData="OnNodeRefresh2">
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
                                                            <Load Handler="#{NodeComboBox2}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:MultiCombo ID="DevTypeMultiCombo2" runat="server" Width="180" SelectionMode="All"
                                                LabelWidth="50" FieldLabel="预警设备" EmptyText="..." DisplayField="Name"
                                                ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true">
                                                <Store>
                                                    <ext:Store ID="DevTypeStore2" runat="server" AutoLoad="true" OnRefreshData="OnDevTypeRefresh2">
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
                                                            <Load Handler="#{DevTypeMultiCombo2}.selectAll();" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Change Handler="" />
                                                </Listeners>
                                            </ext:MultiCombo>
                                            <ext:NumberField ID="CountValueNumberField2" runat="server" Width="180" LabelWidth="50" FieldLabel="预警阈值" AllowDecimals="true" Number="5.0" />
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar ID="TrendCountGridToolbar3" runat="server" Flat="true">
                                        <Items>
                                            <ext:ComboBox ID="CountTimeComboBox2" runat="server" FieldLabel="统计单位" LabelWidth="50" Resizable="true" Width="180">
                                                <Items>
                                                    <ext:ListItem Text="每小时" Value="1" />
                                                    <ext:ListItem Text="每天" Value="2" />
                                                    <ext:ListItem Text="每月" Value="3" />
                                                </Items>
                                                <SelectedItem Value="1" />
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="CountTypeComboBox2" runat="server" FieldLabel="统计类型" LabelWidth="50" Resizable="true" Width="180">
                                                <Items>
                                                    <ext:ListItem Text="平均值" Value="1" />
                                                    <ext:ListItem Text="最大值" Value="2" />
                                                    <ext:ListItem Text="最小值" Value="3" />
                                                </Items>
                                                <SelectedItem Value="1" />
                                            </ext:ComboBox>
                                            <ext:TriggerField ID="BeginFromDate2" runat="server" Width="180" LabelWidth="50" FieldLabel="统计时间">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Date" />
                                                </Triggers>
                                                <Listeners>
                                                    <TriggerClick Handler="WdatePicker({el:'BeginFromDate2',isShowClear:false,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{BeginToDate2}.fireEvent('triggerclick')}});" />
                                                </Listeners>
                                            </ext:TriggerField>
                                            <ext:TriggerField ID="BeginToDate2" runat="server" Width="180" LabelWidth="50" FieldLabel="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" LabelSeparator="~">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Date" />
                                                </Triggers>
                                                <Listeners>
                                                    <TriggerClick Handler="WdatePicker({el:'BeginToDate2',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'BeginFromDate2\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                                </Listeners>
                                            </ext:TriggerField>
                                            <ext:ToolbarSpacer ID="TrendCountToolbarSpacer1" runat="server" Width="5" />
                                            <ext:SplitButton ID="TrendCountQueryBtn" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                                <Menu>
                                                    <ext:Menu ID="TrendCountQueryMenu1" runat="server">
                                                        <Items>
                                                            <ext:MenuItem ID="TrendCountSaveMenuItem" runat="server" Text="打印/导出" Icon="Printer">
                                                                <Listeners>
                                                                    <Click Handler="#{TrendCountGridRowMenuItem2}.fireEvent('click');" />
                                                                </Listeners>
                                                            </ext:MenuItem>
                                                        </Items>
                                                    </ext:Menu>
                                                </Menu>
                                                <DirectEvents>
                                                    <Click OnEvent="TrendCountQueryBtn_Click" Success="#{TrendCountGridPagingToolbar}.doLoad();">
                                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{TrendCountGridPanel}.body.up('div')" />
                                                    </Click>
                                                </DirectEvents>
                                            </ext:SplitButton>
                                        </Items>
                                    </ext:Toolbar>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="TrendCountGridStore" runat="server" AutoLoad="false" OnRefreshData="OnTrendCountGridRefresh">
                                <Proxy>
                                    <ext:PageProxy />
                                </Proxy>
                                <Reader>
                                    <ext:JsonReader IDProperty="ID">
                                        <Fields>
                                            <ext:RecordField Name="ID" Type="Int" />
                                            <ext:RecordField Name="LscID" Type="Int" />
                                            <ext:RecordField Name="LscName" Type="String" />
                                            <ext:RecordField Name="Area1Name" Type="String" />
                                            <ext:RecordField Name="Area2Name" Type="String" />
                                            <ext:RecordField Name="Area3Name" Type="String" />
                                            <ext:RecordField Name="StaName" Type="String" />
                                            <ext:RecordField Name="DevName" Type="String" />
                                            <ext:RecordField Name="NodeID" Type="Int" />
                                            <ext:RecordField Name="NodeName" Type="String" />
                                            <ext:RecordField Name="DevTypeID" Type="Int" />
                                            <ext:RecordField Name="DevTypeName" Type="String" />
                                            <ext:RecordField Name="AlarmLevel" Type="Int" />
                                            <ext:RecordField Name="AlarmLevelName" Type="String" />
                                            <ext:RecordField Name="AlarmType" Type="String" />
                                            <ext:RecordField Name="StartValue" Type="String" />
                                            <ext:RecordField Name="AlarmValue" Type="String" />
                                            <ext:RecordField Name="DiffValue" Type="String" />
                                            <ext:RecordField Name="StartTime" Type="String" />
                                            <ext:RecordField Name="EndTime" Type="String" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                                <BaseParams>
                                    <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                    <ext:Parameter Name="limit" Value="#{PageSizeComboBox2}.getValue()" Mode="Raw" />
                                </BaseParams>
                            </ext:Store>
                        </Store>
                        <LoadMask ShowMask="true" />
                        <ColumnModel ID="TrendCountGridColumnModel" runat="server">
                            <Columns>
                                <ext:Column Header="序号" DataIndex="ID" Align="Left" Width="50"/>
                                <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Center" />
                                <ext:Column Header="地区名称" DataIndex="Area2Name" Align="Center" />
                                <ext:Column Header="县市名称" DataIndex="Area3Name" Align="Center" />
                                <ext:Column Header="局站名称" DataIndex="StaName" Align="Center" />
                                <ext:Column Header="设备名称" DataIndex="DevName" Align="Center" />
                                <ext:Column Header="测点名称" DataIndex="NodeName" Align="Center" />
                                <ext:Column Header="预警设备" DataIndex="DevTypeName" Align="Center" />
                                <ext:Column Header="预警级别" DataIndex="AlarmLevelName" Align="Center" />
                                <ext:Column Header="预警类别" DataIndex="AlarmType" Align="Center" />
                                <ext:Column Header="起始值" DataIndex="StartValue" Align="Left" />
                                <ext:Column Header="触发值" DataIndex="AlarmValue" Align="Left" />
                                <ext:Column Header="差值" DataIndex="DiffValue" Align="Left" />
                                <ext:Column Header="开始时间" DataIndex="StartTime" Align="Center" />
                                <ext:Column Header="结束时间" DataIndex="EndTime" Align="Center" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="TrendCountGridRowSelectionModel" runat="server" SingleSelect="true" />
                        </SelectionModel>
                        <View>
                             <ext:GridView ID="TrendCountGridView" runat="server" ForceFit="false">
                                <GetRowClass Fn="TrendMgr.GetCountGridRowClass" />
                             </ext:GridView>
                        </View>
                        <Listeners>
                            <RowContextMenu Fn="TrendMgr.ShowCountGridRowMenu" Scope="TrendMgr" />
                        </Listeners>
                        <BottomBar>
                            <ext:PagingToolbar ID="TrendCountGridPagingToolbar" runat="server" PageSize="20" StoreID="TrendCountGridStore">
                                <Items>
                                    <ext:ComboBox ID="PageSizeComboBox2" runat="server" Width="150" LabelWidth="60" FieldLabel="<%$ Resources: GlobalResource, ComboBoxPageSize %>">
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
                                            <Select Handler="#{TrendCountGridPagingToolbar}.pageSize = parseInt(this.getValue()); #{TrendCountGridPagingToolbar}.doLoad();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                    <ext:GridPanel ID="TrendHisGridPanel" runat="server" Header="false" Title="历史预警信息" Icon="TagRed" StripeRows="true" TrackMouseOver="true" Layout="FitLayout">
                        <TopBar>
                            <ext:Toolbar ID="TrendHisGridToolbars" runat="server" Layout="ContainerLayout">
                                <Items>
                                    <ext:Toolbar ID="TrendHisGridToolbar1" runat="server" Flat="true">
                                        <Items>
                                            <ext:ComboBox ID="LscsComboBox3" runat="server" Width="180" LabelWidth="50" FieldLabel="Lsc名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="LscsStore3" runat="server" AutoLoad="true" OnRefreshData="OnLscsRefresh3">
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
                                                            <Load Handler="#{LscsComboBox3}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="#{Area2ComboBox3}.clearValue();#{Area2ComboBox3}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="Area2ComboBox3" runat="server" Width="180" LabelWidth="50" FieldLabel="地区名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="Area2Store3" runat="server" AutoLoad="false" OnRefreshData="OnArea2Refresh3">
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
                                                            <Load Handler="#{Area2ComboBox3}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="#{Area3ComboBox3}.clearValue(); #{Area3ComboBox3}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="Area3ComboBox3" runat="server" Width="180" LabelWidth="50" FieldLabel="县市名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="Area3Store3" runat="server" AutoLoad="false" OnRefreshData="OnArea3Refresh3">
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
                                                            <Load Handler="#{Area3ComboBox3}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="#{StaComboBox3}.clearValue(); #{StaComboBox3}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="StaComboBox3" runat="server" Width="180" LabelWidth="50" FieldLabel="局站名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="StaStore3" runat="server" AutoLoad="false" OnRefreshData="OnStaRefresh3">
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
                                                            <Load Handler="#{StaComboBox3}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="#{DevComboBox3}.clearValue(); #{DevComboBox3}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar ID="TrendHisGridToolbar2" runat="server" Flat="true">
                                        <Items>
                                            <ext:ComboBox ID="DevComboBox3" runat="server" Width="180" LabelWidth="50" FieldLabel="设备名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="DevStore3" runat="server" AutoLoad="false" OnRefreshData="OnDevRefresh3">
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
                                                            <Load Handler="#{DevComboBox3}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="#{NodeComboBox3}.clearValue(); #{NodeComboBox3}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="NodeComboBox3" runat="server" Width="180" LabelWidth="50" FieldLabel="测点名称"
                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                Resizable="true">
                                                <Store>
                                                    <ext:Store ID="NodeStore3" runat="server" AutoLoad="false" OnRefreshData="OnNodeRefresh3">
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
                                                            <Load Handler="#{NodeComboBox3}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:MultiCombo ID="DevTypeMultiCombo3" runat="server" Width="180" SelectionMode="All"
                                                LabelWidth="50" FieldLabel="预警设备" EmptyText="..." DisplayField="Name"
                                                ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true">
                                                <Store>
                                                    <ext:Store ID="DevTypeStore3" runat="server" AutoLoad="true" OnRefreshData="OnDevTypeRefresh3">
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
                                                            <Load Handler="#{DevTypeMultiCombo3}.selectAll();" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                            </ext:MultiCombo>
                                            <ext:MultiCombo ID="AlarmLevelMultiCombo3" runat="server" Width="180" SelectionMode="All"
                                                LabelWidth="50" FieldLabel="预警级别" EmptyText="..." DisplayField="Name"
                                                ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true">
                                                <Store>
                                                    <ext:Store ID="AlarmLevelStore3" runat="server" AutoLoad="true" OnRefreshData="OnAlarmLevelRefresh3">
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
                                                            <Load Handler="#{AlarmLevelMultiCombo3}.selectAll();" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                            </ext:MultiCombo>
                                            
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar ID="TrendHisGridToolbar3" runat="server" Flat="true">
                                        <Items>
                                            <ext:TriggerField ID="BeginFromDate3" runat="server" Width="180" LabelWidth="50" FieldLabel="触发时间">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Date" />
                                                </Triggers>
                                                <Listeners>
                                                    <TriggerClick Handler="WdatePicker({el:'BeginFromDate3',isShowClear:false,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{BeginToDate3}.fireEvent('triggerclick')}});" />
                                                </Listeners>
                                            </ext:TriggerField>
                                            <ext:TriggerField ID="BeginToDate3" runat="server" Width="180" LabelWidth="50" FieldLabel="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" LabelSeparator="~">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Date" />
                                                </Triggers>
                                                <Listeners>
                                                    <TriggerClick Handler="WdatePicker({el:'BeginToDate3',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'BeginFromDate3\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                                </Listeners>
                                            </ext:TriggerField>
                                            <ext:TriggerField ID="ConfirmFromDate3" runat="server" Width="180" LabelWidth="50" FieldLabel="确认时间">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Date" />
                                                </Triggers>
                                                <Listeners>
                                                    <TriggerClick Handler="WdatePicker({el:'ConfirmFromDate3',isShowClear:true,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{ConfirmToDate3}.fireEvent('triggerclick')}});" />
                                                </Listeners>
                                            </ext:TriggerField>
                                            <ext:TriggerField ID="ConfirmToDate3" runat="server" Width="180" LabelWidth="50" FieldLabel="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" LabelSeparator="~">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Date" />
                                                </Triggers>
                                                <Listeners>
                                                    <TriggerClick Handler="WdatePicker({el:'ConfirmToDate3',isShowClear:true,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'ConfirmFromDate3\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                                </Listeners>
                                            </ext:TriggerField>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar ID="TrendHisGridToolbar4" runat="server" Flat="true">
                                        <Items>
                                            <ext:TriggerField ID="EndFromDate3" runat="server" Width="180" LabelWidth="50" FieldLabel="处理时间">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Date" />
                                                </Triggers>
                                                <Listeners>
                                                    <TriggerClick Handler="WdatePicker({el:'EndFromDate3',isShowClear:true,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{EndToDate3}.fireEvent('triggerclick')}});" />
                                                </Listeners>
                                            </ext:TriggerField>
                                            <ext:TriggerField ID="EndToDate3" runat="server" Width="180" LabelWidth="50" FieldLabel="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" LabelSeparator="~">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Date" />
                                                </Triggers>
                                                <Listeners>
                                                    <TriggerClick Handler="WdatePicker({el:'EndToDate3',isShowClear:true,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'EndFromDate3\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                                </Listeners>
                                            </ext:TriggerField>
                                            <ext:TextField ID="ConfirmNameField3" runat="server" Width="180" LabelWidth="50" FieldLabel="确认人员" />
                                            <ext:TextField ID="EndNameTextField3" runat="server" Width="180" LabelWidth="50" FieldLabel="处理人员" />
                                            <ext:ToolbarSpacer ID="TrendHisToolbarSpacer1" runat="server" Width="5" />
                                            <ext:SplitButton ID="TrendHisQueryBtn" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                                <Menu>
                                                    <ext:Menu ID="TrendHisQueryMenu1" runat="server">
                                                        <Items>
                                                            <ext:MenuItem ID="TrendHisSaveMenuItem" runat="server" Text="打印/导出" Icon="Printer">
                                                                <Listeners>
                                                                    <Click Handler="#{TrendHisGridRowMenuItem2}.fireEvent('click');" />
                                                                </Listeners>
                                                            </ext:MenuItem>
                                                        </Items>
                                                    </ext:Menu>
                                                </Menu>
                                                <DirectEvents>
                                                    <Click OnEvent="TrendHisQueryBtn_Click" Success="#{TrendHisGridPagingToolbar}.doLoad();">
                                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{TrendHisGridPanel}.body.up('div')" />
                                                    </Click>
                                                </DirectEvents>
                                            </ext:SplitButton>
                                        </Items>
                                    </ext:Toolbar>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Store>
                            <ext:Store ID="TrendHisGridStore" runat="server" AutoLoad="false" OnRefreshData="OnTrendHisGridRefresh">
                                <Proxy>
                                    <ext:PageProxy />
                                </Proxy>
                                <Reader>
                                    <ext:JsonReader IDProperty="ID">
                                        <Fields>
                                            <ext:RecordField Name="ID" Type="Int" />
                                            <ext:RecordField Name="LscID" Type="Int" />
                                            <ext:RecordField Name="LscName" Type="String" />
                                            <ext:RecordField Name="Area1Name" Type="String" />
                                            <ext:RecordField Name="Area2Name" Type="String" />
                                            <ext:RecordField Name="Area3Name" Type="String" />
                                            <ext:RecordField Name="StaName" Type="String" />
                                            <ext:RecordField Name="DevName" Type="String" />
                                            <ext:RecordField Name="NodeID" Type="Int" />
                                            <ext:RecordField Name="NodeName" Type="String" />
                                            <ext:RecordField Name="DevTypeID" Type="Int" />
                                            <ext:RecordField Name="DevTypeName" Type="String" />
                                            <ext:RecordField Name="AlarmLevel" Type="Int" />
                                            <ext:RecordField Name="AlarmLevelName" Type="String" />
                                            <ext:RecordField Name="AlarmType" Type="String" />
                                            <ext:RecordField Name="StartValue" Type="String" />
                                            <ext:RecordField Name="AlarmValue" Type="String" />
                                            <ext:RecordField Name="DiffValue" Type="String" />
                                            <ext:RecordField Name="StartTime" Type="String" />
                                            <ext:RecordField Name="AlarmTime" Type="String" />
                                            <ext:RecordField Name="ConfirmName" Type="String" />
                                            <ext:RecordField Name="ConfirmTime" Type="String" />
                                            <ext:RecordField Name="EndName" Type="String" />
                                            <ext:RecordField Name="EndTime" Type="String" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                                <BaseParams>
                                    <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                    <ext:Parameter Name="limit" Value="#{PageSizeComboBox3}.getValue()" Mode="Raw" />
                                </BaseParams>
                            </ext:Store>
                        </Store>
                        <LoadMask ShowMask="true" />
                        <ColumnModel ID="TrendHisGridColumnModel" runat="server">
                            <Columns>
                                <ext:Column Header="序号" DataIndex="ID" Align="Left" Width="50"/>
                                <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Center" />
                                <ext:Column Header="地区名称" DataIndex="Area2Name" Align="Center" />
                                <ext:Column Header="县市名称" DataIndex="Area3Name" Align="Center" />
                                <ext:Column Header="局站名称" DataIndex="StaName" Align="Center" />
                                <ext:Column Header="设备名称" DataIndex="DevName" Align="Center" />
                                <ext:Column Header="测点名称" DataIndex="NodeName" Align="Center" />
                                <ext:Column Header="预警设备" DataIndex="DevTypeName" Align="Center" />
                                <ext:Column Header="预警级别" DataIndex="AlarmLevelName" Align="Center" />
                                <ext:Column Header="预警类别" DataIndex="AlarmType" Align="Center" />
                                <ext:Column Header="起始值" DataIndex="StartValue" Align="Left" />
                                <ext:Column Header="触发值" DataIndex="AlarmValue" Align="Left" />
                                <ext:Column Header="差值" DataIndex="DiffValue" Align="Left" />
                                <ext:Column Header="开始时间" DataIndex="StartTime" Align="Center" />
                                <ext:Column Header="触发时间" DataIndex="AlarmTime" Align="Center" />
                                <ext:Column Header="确认人员" DataIndex="ConfirmName" Align="Center" />
                                <ext:Column Header="确认时间" DataIndex="ConfirmTime" Align="Center" />
                                <ext:Column Header="处理人员" DataIndex="EndName" Align="Center" />
                                <ext:Column Header="处理时间" DataIndex="EndTime" Align="Center" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="TrendHisGridRowSelectionModel" runat="server" SingleSelect="true" />
                        </SelectionModel>
                        <View>
                             <ext:GridView ID="TrendHisGridView" runat="server" ForceFit="false">
                                <GetRowClass Fn="TrendMgr.GetHisGridRowClass" />
                             </ext:GridView>
                        </View>
                        <Listeners>
                            <RowContextMenu Fn="TrendMgr.ShowHisGridRowMenu" Scope="TrendMgr" />
                        </Listeners>
                        <BottomBar>
                            <ext:PagingToolbar ID="TrendHisGridPagingToolbar" runat="server" PageSize="20" StoreID="TrendHisGridStore">
                                <Items>
                                    <ext:ComboBox ID="PageSizeComboBox3" runat="server" Width="150" LabelWidth="60" FieldLabel="<%$ Resources: GlobalResource, ComboBoxPageSize %>">
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
                                            <Select Handler="#{TrendHisGridPagingToolbar}.pageSize = parseInt(this.getValue()); #{TrendHisGridPagingToolbar}.doLoad();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:TabPanel>
        </Items>
    </ext:Viewport>
    <ext:TaskManager ID="MainTaskManager" runat="server">
        <Tasks>
            <ext:Task AutoRun="true" Interval="10000">
                <Listeners>
                    <Update Handler="TrendMgr.LoadPageTrendAlarms();" />
                </Listeners>
            </ext:Task>
        </Tasks>
    </ext:TaskManager>
    <ext:Menu ID="TrendGridRowMenu" runat="server">
        <Items>
            <ext:MenuItem ID="TrendGridRowMenuItem1" runat="server" Text="确认预警" Icon="Accept">
                <DirectEvents>
                    <Click OnEvent="OnConfirmTrendClick">
                        <ExtraParams>
                            <ext:Parameter Name="LscId" Value="this.parentMenu.dataRecord.data.LscID" Mode="Raw" />
                            <ext:Parameter Name="NodeId" Value="this.parentMenu.dataRecord.data.NodeID" Mode="Raw" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:MenuItem>
            <ext:MenuItem ID="TrendGridRowMenuItem2" runat="server" Text="结束预警" Icon="Delete">
                <DirectEvents>
                    <Click OnEvent="OnEndTrendClick">
                        <ExtraParams>
                            <ext:Parameter Name="LscId" Value="this.parentMenu.dataRecord.data.LscID" Mode="Raw" />
                            <ext:Parameter Name="NodeId" Value="this.parentMenu.dataRecord.data.NodeID" Mode="Raw" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:MenuItem>
            <ext:MenuSeparator ID="TrendGridRowSeparator1" runat="server" />
            <ext:CheckMenuItem ID="TrendGridRowMenuItem3" runat="server" Text="显示预警级别色" Checked="true" CheckHandler="function(){ #{TrendGridPagingToolbar}.doRefresh(); }" />
            <ext:MenuSeparator ID="TrendGridRowSeparator2" runat="server" />
            <ext:MenuItem ID="TrendGridRowMenuItem4" runat="server" Text="刷新列表" Icon="TableRefresh">
                <Listeners>
                    <Click Handler="#{TrendGridPagingToolbar}.doRefresh();" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuSeparator ID="TrendGridRowSeparator3" runat="server" />
            <ext:MenuItem ID="TrendGridRowMenuItem5" runat="server" Text="打印/导出" Icon="Printer">
                <DirectEvents>
                    <Click OnEvent="OnTrendGridExport_Click" IsUpload="true">
                        <ExtraParams>
                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{TrendGridPanel})" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    <ext:Menu ID="TrendCountGridRowMenu" runat="server">
        <Items>
            <ext:CheckMenuItem ID="TrendCountGridRowMenuItem1" runat="server" Text="显示预警级别色" Checked="true" CheckHandler="function(){ #{TrendCountGridPagingToolbar}.doRefresh(); }" />
            <ext:MenuSeparator ID="TrendCountGridRowSeparator1" runat="server" />
            <ext:MenuItem ID="TrendCountGridRowMenuItem2" runat="server" Text="打印/导出" Icon="Printer">
                <DirectEvents>
                    <Click OnEvent="OnTrendCountGridExport_Click" IsUpload="true">
                        <ExtraParams>
                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{TrendCountGridPanel})" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    <ext:Menu ID="TrendHisGridRowMenu" runat="server">
        <Items>
            <ext:CheckMenuItem ID="TrendHisGridRowMenuItem1" runat="server" Text="显示预警级别色" Checked="true" CheckHandler="function(){ #{TrendHisGridPagingToolbar}.doRefresh(); }" />
            <ext:MenuSeparator ID="TrendHisGridRowSeparator1" runat="server" />
            <ext:MenuItem ID="TrendHisGridRowMenuItem2" runat="server" Text="打印/导出" Icon="Printer">
                <DirectEvents>
                    <Click OnEvent="OnTrendHisGridExport_Click" IsUpload="true">
                        <ExtraParams>
                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{TrendHisGridPanel})" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    </form>
</body>
</html>
