<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AcEvents.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.AcEvents" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>门禁事件统计</title>
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
            <ext:GridPanel ID="MainGridPanel" runat="server" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Region="Center" Height="500px" Title="门禁事件记录" Icon="ApplicationViewList">
                <TopBar>
                    <ext:Toolbar ID="TopToolbars" runat="server">
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
                                    <Select Handler="#{EventTypeMultiCombo}.clearValue();#{EventTypeMultiCombo}.store.reload();" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:MultiCombo ID="EventTypeMultiCombo" runat="server" SelectionMode="All" Width="180"
                                LabelWidth="50" FieldLabel="事件类型" DisplayField="Name" ValueField="Id" ForceSelection="true"
                                TriggerAction="All" Resizable="true" EmptyText="默认全部">
                                <Store>
                                    <ext:Store ID="EventTypeStore" runat="server" AutoLoad="false" OnRefreshData="OnEventTypeRefresh">
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Reader>
                                            <ext:JsonReader IDProperty="Id">
                                                <Fields>
                                                    <ext:RecordField Name="Id" Type="Int" />
                                                    <ext:RecordField Name="Name" Type="String" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                        <Listeners>
                                            <%--<Load Handler="#{EventTypeMultiCombo}.selectAll();" />--%>
                                        </Listeners>
                                    </ext:Store>
                                </Store>
                            </ext:MultiCombo>
                            <ext:ComboBox ID="CountTypeComboBox" runat="server" FieldLabel="统计类型" LabelWidth="50" Resizable="true" Width="180">
                                <Items>
                                    <ext:ListItem Text="告警事件统计" Value="0" />
                                    <ext:ListItem Text="普通事件统计" Value="1" />
                                    <ext:ListItem Text="无效刷卡统计" Value="2" />
                                    <ext:ListItem Text="有效刷卡统计" Value="3" />
                                </Items>
                                <SelectedItem Value="3" />
                            </ext:ComboBox>
                            <ext:TriggerField ID="FromDate" runat="server" Width="180" LabelWidth="50" FieldLabel="开始时间">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Date" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="WdatePicker({el:'FromDate',isShowClear:false,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{ToDate}.fireEvent('triggerclick')}});" />
                                </Listeners>
                            </ext:TriggerField>
                            <ext:TriggerField ID="ToDate" runat="server" Width="180" LabelWidth="50" FieldLabel="结束时间">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Date" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="WdatePicker({el:'ToDate',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'FromDate\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                </Listeners>
                            </ext:TriggerField>
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
                                    <Click OnEvent="QueryBtn_Click" Success="#{MainPagingToolbar}.doLoad();">
                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{MainGridPanel}.body.up('div')" />
                                    </Click>
                                </DirectEvents>
                            </ext:SplitButton>
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
                                    <ext:RecordField Name="NetGroupID" Type="Int" />
                                    <ext:RecordField Name="NetGroupName" Type="String" />
                                    <ext:RecordField Name="PointName" Type="String" />
                                    <ext:RecordField Name="UserID" Type="String" />
                                    <ext:RecordField Name="UserName" Type="String" />
                                    <ext:RecordField Name="Department" Type="String" />
                                    <ext:RecordField Name="MessageID" Type="Int" />
                                    <ext:RecordField Name="Message" Type="String" />
                                    <ext:RecordField Name="EventTime" Type="String" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                        <BaseParams>
                            <ext:Parameter Name="start" Value="0" Mode="Raw" />
                            <ext:Parameter Name="limit" Value="#{MainPageSizeComboBox}.getValue()" Mode="Raw" />
                        </BaseParams>
                    </ext:Store>
                </Store>
                <LoadMask ShowMask="true" />
                <ColumnModel ID="MainColumnModel" runat="server">
                    <Columns>
                        <ext:Column Header="序号" DataIndex="ID" Align="Left" Width="50" />
                        <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" />
                        <ext:Column Header="所属机构" DataIndex="NetGroupName" Align="Left" />
                        <ext:Column Header="所属门点" DataIndex="PointName" Align="Left" />
                        <ext:Column Header="人员姓名" DataIndex="UserName" Align="Left" />
                        <ext:Column Header="所属单位" DataIndex="Department" Align="Left" />
                        <ext:Column Header="事件消息" DataIndex="Message" Align="Left" />
                        <ext:Column Header="事件时间" DataIndex="EventTime" Align="Left" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="MainRowSelectionModel" runat="server" SingleSelect="true" />
                </SelectionModel>
                <View>
                    <ext:GridView ID="MainGridView" runat="server" ForceFit="false" />
                </View>
                <BottomBar>
                    <ext:PagingToolbar ID="MainPagingToolbar" runat="server" PageSize="20" StoreID="MainStore">
                        <Items>
                            <ext:ComboBox ID="MainPageSizeComboBox" runat="server" Width="150" LabelWidth="60"
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
