<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report120.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.KPI.report120" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>蓄电池单体电压达标率</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <link rel="shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link rel="icon" type="image/x-icon" href="../favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="../favicon.ico" />
    <script type="text/javascript" src="../Resources/js/public.js?v=3.3.0.0"></script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
        <ext:Viewport ID="MainViewport" runat="server" Layout="BorderLayout">
            <Items>
                <ext:GridPanel ID="MainGridPanel" runat="server" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Region="Center" Height="500px" Title="蓄电池单体电压达标率 = (1-单体电压低于1.85V的蓄电池节数/(蓄电池设备总数*24节))×100%" Icon="ApplicationViewList">
                    <TopBar>
                        <ext:Toolbar ID="TopToolbars" runat="server" Layout="ContainerLayout">
                            <Items>
                                <ext:Toolbar ID="TopToolbar1" runat="server" Flat="true">
                                    <Items>
                                        <ext:ComboBox ID="LscsComboBox" runat="server" Width="250" LabelWidth="50" FieldLabel="Lsc名称" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true" Resizable="true">
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
                                                        <Load Handler="#{LscsComboBox}.setValueAndFireSelect(this.getAt(0).get('Id')); " />
                                                    </Listeners>
                                                </ext:Store>
                                            </Store>
                                        </ext:ComboBox>
                                        <ext:MultiCombo ID="StaTypeMultiCombo" runat="server" Width="250" SelectionMode="All" LabelWidth="50" FieldLabel="局站类型" EmptyText="select an item..." DisplayField="Name" ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true">
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
                                                        <%--<Load Handler="#{StaTypeMultiCombo}.selectAll();" />--%>
                                                    </Listeners>
                                                </ext:Store>
                                            </Store>
                                        </ext:MultiCombo>
                                        <ext:MultiCombo ID="DevTypeMultiCombo" runat="server" Width="250" SelectionMode="All" LabelWidth="50" FieldLabel="设备类型" EmptyText="select an item..." DisplayField="Name" ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true">
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
                                                        <%--<Load Handler="#{DevTypeMultiCombo}.selectAll();" />--%>
                                                    </Listeners>
                                                </ext:Store>
                                            </Store>
                                        </ext:MultiCombo>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="TopToolbar2" runat="server" Flat="true">
                                    <Items>
                                        <ext:ComboBox ID="AlarmDevComboBox" runat="server" Width="250" LabelWidth="50" FieldLabel="告警设备"
                                            Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                            ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                            Resizable="true">
                                            <Store>
                                                <ext:Store ID="AlarmDevStore" runat="server" AutoLoad="true" OnRefreshData="OnAlarmDevRefresh">
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
                                                        <Load Handler="
                                                        if(this.getCount() != 0) {
                                                            var id = this.getAt(0).get('Id');
                                                            #{AlarmDevComboBox}.setValueAndFireSelect(id);
                                                        }" />
                                                    </Listeners>
                                                </ext:Store>
                                            </Store>
                                            <Listeners>
                                                <Select Handler="#{AlarmLogicComboBox}.clearValue(); #{AlarmLogicComboBox}.store.reload();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:ComboBox ID="AlarmLogicComboBox" runat="server" Width="250" LabelWidth="50"
                                            FieldLabel="告警分类" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true"
                                            Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..."
                                            SelectOnFocus="true" Resizable="true">
                                            <Store>
                                                <ext:Store ID="AlarmLogicStore" runat="server" AutoLoad="false" OnRefreshData="OnAlarmLogicRefresh">
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
                                                        <Load Handler="
                                                        if(this.getCount() != 0) {
                                                            var id = this.getAt(0).get('Id');
                                                            #{AlarmLogicComboBox}.setValueAndFireSelect(id);
                                                        }" />
                                                    </Listeners>
                                                </ext:Store>
                                            </Store>
                                            <Listeners>
                                                <Select Handler="#{AlarmNameComboBox}.clearValue(); #{AlarmNameComboBox}.store.reload();" />
                                            </Listeners>
                                        </ext:ComboBox>
                                        <ext:ComboBox ID="AlarmNameComboBox" runat="server" Width="250" LabelWidth="50" FieldLabel="告警名称"
                                            Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                            ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                            Resizable="true">
                                            <Store>
                                                <ext:Store ID="AlarmNameStore" runat="server" AutoLoad="false" OnRefreshData="OnAlarmNameRefresh">
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
                                                        <Load Handler="
                                                        if(this.getCount() != 0) {
                                                            var id = this.getAt(0).get('Id');
                                                            #{AlarmNameComboBox}.setValueAndFireSelect(id);
                                                        }" />
                                                    </Listeners>
                                                </ext:Store>
                                            </Store>
                                            <Listeners>
                                                <%--<Select Handler="#{AlarmLevelMultiCombo}.clearValue(); #{AlarmLevelMultiCombo}.store.reload();" />--%>
                                            </Listeners>
                                        </ext:ComboBox>
                                    </Items>
                                </ext:Toolbar>
                                <ext:Toolbar ID="TopToolbar3" runat="server" Flat="true">
                                    <Items>
                                        <ext:TriggerField ID="BeginFromDate" runat="server" Width="250" LabelWidth="50" FieldLabel="查询时间">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Date" />
                                            </Triggers>
                                            <Listeners>
                                                <TriggerClick Handler="WdatePicker({el:'BeginFromDate',isShowClear:false,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{BeginToDate}.fireEvent('triggerclick')}});" />
                                            </Listeners>
                                        </ext:TriggerField>
                                        <ext:TriggerField ID="BeginToDate" runat="server" Width="250" LabelWidth="50" FieldLabel="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                            LabelSeparator="~">
                                            <Triggers>
                                                <ext:FieldTrigger Icon="Date" />
                                            </Triggers>
                                            <Listeners>
                                                <TriggerClick Handler="WdatePicker({el:'BeginToDate',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'BeginFromDate\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                            </Listeners>
                                        </ext:TriggerField>
                                        <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5" />
                                        <ext:SplitButton ID="QueryBtn" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                            <Menu>
                                                <ext:Menu ID="QueryMenu1" runat="server" RenderToForm="true">
                                                    <Items>
                                                        <ext:MenuItem ID="SaveMenuItem" runat="server" Text="打印/导出" Icon="Printer">
                                                            <DirectEvents>
                                                                <Click OnEvent="SaveBtn_Click" IsUpload="true">
                                                                    <ExtraParams>
                                                                        <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{MainGridPanel})" />
                                                                    </ExtraParams>
                                                                </Click>
                                                            </DirectEvents>
                                                        </ext:MenuItem>
                                                    </Items>
                                                </ext:Menu>
                                            </Menu>
                                            <DirectEvents>
                                                <Click OnEvent="QueryBtn_Click" Success="#{MainPagingToolbar}.doLoad();">
                                                    <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{MainGridPanel}.body.up('div')" />
                                                </Click>
                                            </DirectEvents>
                                        </ext:SplitButton>
                                    </Items>
                                </ext:Toolbar>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Store>
                        <ext:Store ID="MainStore" runat="server" AutoLoad="false" OnRefreshData="OnMainStoreRefresh">
                            <Proxy>
                                <ext:PageProxy />
                            </Proxy>
                            <Reader>
                                <ext:JsonReader IDProperty="ID">
                                    <Fields>
                                        <ext:RecordField Name="ID" Type="Int" />
                                        <ext:RecordField Name="LscID" Type="Int" />
                                        <ext:RecordField Name="LscName" Type="String" />
                                        <ext:RecordField Name="Count" Type="Int" />
                                        <ext:RecordField Name="TotalCount" Type="Int" />
                                        <ext:RecordField Name="Rate" Type="String" />
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                            <BaseParams>
                                <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                <ext:Parameter Name="limit" Value="#{PageSizeComboBox}.getValue()" Mode="Raw" />
                            </BaseParams>
                        </ext:Store>
                    </Store>
                    <LoadMask ShowMask="true" />
                    <ColumnModel ID="MainColumnModel" runat="server">
                        <Columns>
                            <ext:Column Header="序号" DataIndex="ID" Align="Left" Width="50" />
                            <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" Width="200" />
                            <ext:Column Header="单体电压低于1.85V的蓄电池节数" DataIndex="Count" Align="Left" Width="200">
                                <CustomConfig>
                                    <ext:ConfigItem Name="DblClickEnabled" Mode="Value" Value="1" />
                                </CustomConfig>
                            </ext:Column>
                            <ext:Column Header="蓄电池设备总数*24节" DataIndex="TotalCount" Align="Left" Width="200">
                                <CustomConfig>
                                    <ext:ConfigItem Name="DblClickEnabled" Mode="Value" Value="1" />
                                </CustomConfig>
                            </ext:Column>
                            <ext:Column Header="蓄电池单体电压达标率" DataIndex="Rate" Align="Left" Width="200" />
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:CellSelectionModel ID="MainCellSelectionModel" runat="server" />
                    </SelectionModel>
                    <View>
                        <ext:GridView ID="MainGridView" runat="server" ForceFit="false" />
                    </View>
                    <Listeners>
                        <CellDblClick Fn="KPIReport120.GridCellDblClick" Scope="KPIReport120" />
                    </Listeners>
                    <BottomBar>
                        <ext:PagingToolbar ID="MainPagingToolbar" runat="server" PageSize="20" StoreID="MainStore">
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
                                        <Select Handler="#{MainPagingToolbar}.pageSize = parseInt(this.getValue()); #{MainPagingToolbar}.doLoad();" />
                                    </Listeners>
                                </ext:ComboBox>
                            </Items>
                        </ext:PagingToolbar>
                    </BottomBar>
                </ext:GridPanel>
            </Items>
        </ext:Viewport>
    </form>
</body>
</html>