<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SystemLogs.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.SystemLogs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统日志</title>
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
    <ext:Viewport ID="LogViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="LogGridPanel" runat="server" AutoExpandColumn="Message" StripeRows="true"
                TrackMouseOver="true" AutoScroll="true" Region="Center" Height="500px" Title="系统日志"
                Icon="ApplicationViewList">
                <TopBar>
                    <ext:Toolbar ID="LogGridToolbars" runat="server">
                        <Items>
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
                            <ext:MultiCombo ID="LogLevelMultiCombo" runat="server" SelectionMode="All" Width="180"
                                LabelWidth="50" FieldLabel="日志级别" DisplayField="Name" ValueField="Id" ForceSelection="true"
                                TriggerAction="All" Resizable="true">
                                <Store>
                                    <ext:Store ID="LogLevelStore" runat="server" AutoLoad="true" OnRefreshData="OnLogLevelRefresh">
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
                                            <Load Handler="#{LogLevelMultiCombo}.selectAll();" />
                                        </Listeners>
                                    </ext:Store>
                                </Store>
                            </ext:MultiCombo>
                            <ext:MultiCombo ID="LogTypeMultiCombo" runat="server" SelectionMode="All" Width="180"
                                LabelWidth="50" FieldLabel="日志类型" DisplayField="Name" ValueField="Id" ForceSelection="true"
                                TriggerAction="All" Resizable="true">
                                <Store>
                                    <ext:Store ID="LogTypeStore" runat="server" AutoLoad="true" OnRefreshData="OnLogTypeRefresh">
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
                                            <Load Handler="#{LogTypeMultiCombo}.selectAll();" />
                                        </Listeners>
                                    </ext:Store>
                                </Store>
                            </ext:MultiCombo>
                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5" />
                            <ext:SplitButton ID="QueryBtn" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                <Menu>
                                    <ext:Menu ID="QueryMenu1" runat="server">
                                        <Items>
                                            <ext:MenuItem ID="SaveMenuItem" runat="server" Text="打印/导出" Icon="Printer">
                                                <DirectEvents>
                                                    <Click OnEvent="SaveBtn_Click" IsUpload="true">
                                                        <ExtraParams>
                                                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{LogGridPanel})" />
                                                        </ExtraParams>
                                                    </Click>
                                                </DirectEvents>
                                            </ext:MenuItem>
                                        </Items>
                                    </ext:Menu>
                                </Menu>
                                <DirectEvents>
                                    <Click OnEvent="QueryBtn_Click" Success="#{LogGridPagingToolbar}.doLoad();">
                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{LogGridPanel}.body.up('div')" />
                                    </Click>
                                </DirectEvents>
                            </ext:SplitButton>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store ID="LogGridStore" runat="server" OnRefreshData="OnLogGridRefresh" AutoLoad="false">
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                        <Reader>
                            <ext:JsonReader IDProperty="EventID">
                                <Fields>
                                    <ext:RecordField Name="EventID" Type="Int" />
                                    <ext:RecordField Name="EventTime" Type="String" />
                                    <ext:RecordField Name="EventLevel" Type="String" />
                                    <ext:RecordField Name="EventType" Type="String" />
                                    <ext:RecordField Name="Message" Type="String" />
                                    <ext:RecordField Name="Url" Type="String" />
                                    <ext:RecordField Name="ClientIP" Type="String" />
                                    <ext:RecordField Name="Operator" Type="String" />
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
                <ColumnModel ID="LogGridColumnModel" runat="server">
                    <Columns>
                        <ext:Column Header="事件编号" DataIndex="EventID" Align="Left" />
                        <ext:Column Header="事件时间" DataIndex="EventTime" Align="Center" />
                        <ext:Column Header="事件级别" DataIndex="EventLevel" Align="Left" />
                        <ext:Column Header="事件类型" DataIndex="EventType" Align="Left" />
                        <ext:Column Header="事件信息" DataIndex="Message" Align="Left">
                            <Renderer Fn="SysLogs.setMessageTips" Scope="SysLogs" />
                        </ext:Column>
                        <ext:Column Header="请求路径" DataIndex="Url" Align="Left" />
                        <ext:Column Header="客户端地址" DataIndex="ClientIP" Align="Left" />
                        <ext:Column Header="操作员" DataIndex="Operator" Align="Left" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="LogGridRowSelectionModel" runat="server" SingleSelect="true" />
                </SelectionModel>
                <View>
                    <ext:GridView ID="LogGridGridView" runat="server" ForceFit="false" />
                </View>
                <BottomBar>
                    <ext:PagingToolbar ID="LogGridPagingToolbar" runat="server" PageSize="20" StoreID="LogGridStore">
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
                                    <Select Handler="#{LogGridPagingToolbar}.pageSize = parseInt(this.getValue()); #{LogGridPagingToolbar}.doLoad();" />
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
