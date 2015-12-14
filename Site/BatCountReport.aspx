<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BatCountReport.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.BatCountReport" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>电池放电统计</title>
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
    <ext:Viewport ID="PageViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="MainGridPanel" runat="server" StripeRows="true" Border="false" TrackMouseOver="true" AutoScroll="true" Region="Center" Title="电池放电次数列表" Icon="ApplicationViewList">
                <TopBar>
                    <ext:Toolbar ID="TopToolbars" runat="server" Layout="ContainerLayout">
                        <Items>
                            <ext:Toolbar ID="TopToolbar1" runat="server" Flat="true">
                                <Items>
                                    <ext:ComboBox ID="LscsComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="Lsc名称"
                                        Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                        ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                        Resizable="true">
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
                                            <Select Handler="#{Area2ComboBox}.clearValue();#{Area2ComboBox}.store.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="Area2ComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="地区名称"
                                        Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                        ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                        Resizable="true">
                                        <Store>
                                            <ext:Store ID="Area2Store" runat="server" AutoLoad="false" OnRefreshData="OnArea2Refresh">
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
                                                    <Load Handler="#{Area2ComboBox}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Handler="#{Area3ComboBox}.clearValue(); #{Area3ComboBox}.store.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="Area3ComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="县市名称"
                                        Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                        ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                        Resizable="true">
                                        <Store>
                                            <ext:Store ID="Area3Store" runat="server" AutoLoad="false" OnRefreshData="OnArea3Refresh">
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
                                                    <Load Handler="#{Area3ComboBox}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Handler="#{StaComboBox}.clearValue(); #{StaComboBox}.store.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="StaComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="局站名称"
                                        Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                        ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                        Resizable="true">
                                        <Store>
                                            <ext:Store ID="StaStore" runat="server" AutoLoad="false" OnRefreshData="OnStaRefresh">
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
                                                    <Load Handler="#{StaComboBox}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Handler="#{DevComboBox}.clearValue(); #{DevComboBox}.store.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar ID="TopToolbar2" runat="server" Flat="true">
                                <Items>
                                    <ext:ComboBox ID="DevComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="设备名称"
                                        Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                        ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                        Resizable="true">
                                        <Store>
                                            <ext:Store ID="DevStore" runat="server" AutoLoad="false" OnRefreshData="OnDevRefresh">
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
                                                    <Load Handler="#{DevComboBox}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                    <ext:NumberField ID="DevIndexNumberField" runat="server" Width="180" LabelWidth="50" AllowDecimals="false" FieldLabel="电池组号" EmptyText="电池组的编号" />
                                    <ext:TriggerField ID="BeginFromDate" runat="server" Width="180" LabelWidth="50" FieldLabel="开始时间">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Date" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="WdatePicker({el:'BeginFromDate',isShowClear:false,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{BeginToDate}.fireEvent('triggerclick')}});" />
                                        </Listeners>
                                    </ext:TriggerField>
                                    <ext:TriggerField ID="BeginToDate" runat="server" Width="180" LabelWidth="50" FieldLabel="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                        LabelSeparator="~">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Date" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="WdatePicker({el:'BeginToDate',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'BeginFromDate\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                        </Listeners>
                                    </ext:TriggerField>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar ID="TopToolbar3" runat="server" Flat="true">
                                <Items>
                                    <ext:TriggerField ID="EndFromDate" runat="server" Width="180" LabelWidth="50" FieldLabel="结束时间">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Date" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="WdatePicker({el:'EndFromDate',isShowClear:true,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{EndToDate}.fireEvent('triggerclick')}});" />
                                        </Listeners>
                                    </ext:TriggerField>
                                    <ext:TriggerField ID="EndToDate" runat="server" Width="180" LabelWidth="50" FieldLabel="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                        LabelSeparator="~">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Date" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="WdatePicker({el:'EndToDate',isShowClear:true,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'EndFromDate\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                        </Listeners>
                                    </ext:TriggerField>
                                    <ext:NumberField ID="FromMinNumberField" runat="server" MinValue="0" Width="180" LabelWidth="50" FieldLabel="放电历时" EmptyText="最小历时(分钟)">
                                    </ext:NumberField>
                                    <ext:NumberField ID="ToMinNumberField" runat="server" MinValue="0" Width="180" LabelWidth="50" FieldLabel="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" LabelSeparator="~" EmptyText="最大历时(分钟)">
                                    </ext:NumberField>
                                    <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5" />
                                    <ext:SplitButton ID="QueryBtn" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                        <Menu>
                                            <ext:Menu ID="QueryMenu1" runat="server">
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
                                            <Click OnEvent="QueryBtn_Click" Success="#{BottomPagingToolbar}.doLoad();">
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
                    <ext:Store ID="GridStore" runat="server" AutoLoad="false" OnRefreshData="OnGridStoreRefresh">
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
                                    <ext:RecordField Name="DevID" Type="Int" />
                                    <ext:RecordField Name="DevName" Type="String" />
                                    <ext:RecordField Name="DevIndex" Type="Int" />
                                    <ext:RecordField Name="FDCount" Type="Int" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                        <BaseParams>
                            <ext:Parameter Name="start" Value="0" Mode="Raw" />
                            <ext:Parameter Name="limit" Value="#{PageSizeComboBox}.getValue()" Mode="Raw" />
                        </BaseParams>
                    </ext:Store>
                </Store>
                <ColumnModel ID="GridColumnModel" runat="server">
                    <Columns>
                        <ext:Column Header="序号" DataIndex="ID" Align="Left" />
                        <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" />
                        <ext:Column Header="地区名称" DataIndex="Area2Name" Align="Left" />
                        <ext:Column Header="县市名称" DataIndex="Area3Name" Align="Left" />
                        <ext:Column Header="局站名称" DataIndex="StaName" Align="Left" />
                        <ext:Column Header="设备名称" DataIndex="DevName" Align="Left" />
                        <ext:Column Header="电池组号" DataIndex="DevIndex" Align="Left" />
                        <ext:Column Header="放电次数" DataIndex="FDCount" Align="Left">
                            <CustomConfig>
                                <ext:ConfigItem Name="DblClickEnabled" Value="1" Mode="Value" />
                            </CustomConfig>
                        </ext:Column>
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:CellSelectionModel ID="GridCellSelectionModel" runat="server" />
                </SelectionModel>
                <View>
                    <ext:GridView ID="GridGridView" runat="server" ForceFit="false" />
                </View>
                <Listeners>
                    <CellDblClick Fn="BatCR.onGridCellDblClick" Scope="BatCR" />
                </Listeners>
                <LoadMask ShowMask="true" />
                <BottomBar>
                    <ext:PagingToolbar ID="BottomPagingToolbar" runat="server" PageSize="20" StoreID="GridStore">
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
                                    <Select Handler="#{BottomPagingToolbar}.pageSize = parseInt(this.getValue()); #{BottomPagingToolbar}.doLoad();" />
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
