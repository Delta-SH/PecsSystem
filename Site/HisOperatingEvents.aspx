<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HisOperatingEvents.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.HisOperatingEvents" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>操作记录</title>
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
    <ext:Viewport ID="OpEventViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="OpEventGridPanel" runat="server" StripeRows="true" AutoExpandColumn="OpDesc"
                TrackMouseOver="true" AutoScroll="true" Region="Center" Height="500px" Title="操作记录"
                Icon="ApplicationViewList">
                <TopBar>
                    <ext:Toolbar ID="OpEventToolbar" runat="server">
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
                            </ext:ComboBox>
                            <ext:MultiCombo ID="EventTypeMultiCombo" runat="server" SelectionMode="All" Width="180"
                                LabelWidth="50" FieldLabel="事件类型" DisplayField="Name" ValueField="Id" ForceSelection="true"
                                TriggerAction="All" Resizable="true">
                                <Store>
                                    <ext:Store ID="EventTypeStore" runat="server" AutoLoad="true" OnRefreshData="OnEventTypeRefresh">
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
                                            <Load Handler="#{EventTypeMultiCombo}.selectAll();" />
                                        </Listeners>
                                    </ext:Store>
                                </Store>
                            </ext:MultiCombo>
                            <ext:TriggerField ID="EventDescField" runat="server" Width="180" LabelWidth="50"
                                FieldLabel="事件描述">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Clear" HideTrigger="false" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="this.setValue('');" />
                                </Listeners>
                            </ext:TriggerField>
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
                                                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{OpEventGridPanel})" />
                                                        </ExtraParams>
                                                    </Click>
                                                </DirectEvents>
                                            </ext:MenuItem>
                                        </Items>
                                    </ext:Menu>
                                </Menu>
                                <DirectEvents>
                                    <Click OnEvent="QueryBtn_Click" Success="#{OpEventPagingToolbar}.doLoad();">
                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{OpEventGridPanel}.body.up('div')" />
                                    </Click>
                                </DirectEvents>
                            </ext:SplitButton>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store ID="OpEventStore" runat="server" AutoLoad="false" OnRefreshData="OnOpEventRefresh">
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                        <Reader>
                            <ext:JsonReader IDProperty="ID">
                                <Fields>
                                    <ext:RecordField Name="ID" Type="Int" />
                                    <ext:RecordField Name="LscName" Type="String" />
                                    <ext:RecordField Name="EventName" Type="String" />
                                    <ext:RecordField Name="UserTypeName" Type="String" />
                                    <ext:RecordField Name="UserName" Type="String" />
                                    <ext:RecordField Name="EventTime" Type="String" />
                                    <ext:RecordField Name="OpDesc" Type="String" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                        <BaseParams>
                            <ext:Parameter Name="start" Value="0" Mode="Raw" />
                            <ext:Parameter Name="limit" Value="#{OpEventPageSizeComboBox}.getValue()" Mode="Raw" />
                        </BaseParams>
                    </ext:Store>
                </Store>
                <LoadMask ShowMask="true" />
                <ColumnModel ID="OpEventColumnModel" runat="server">
                    <Columns>
                        <ext:Column Header="序号" DataIndex="ID" Align="Left" Width="50" />
                        <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" />
                        <ext:Column Header="用户类型" DataIndex="UserTypeName" Align="Left" />
                        <ext:Column Header="用户名称" DataIndex="UserName" Align="Left" />
                        <ext:Column Header="事件名称" DataIndex="EventName" Align="Left" />
                        <ext:Column Header="事件时间" DataIndex="EventTime" Align="Left" />
                        <ext:Column Header="事件描述" DataIndex="OpDesc" Align="Left" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="OpEventRowSelectionModel" runat="server" SingleSelect="true" />
                </SelectionModel>
                <View>
                    <ext:GridView ID="OpEventGridView" runat="server" ForceFit="false" />
                </View>
                <BottomBar>
                    <ext:PagingToolbar ID="OpEventPagingToolbar" runat="server" PageSize="20" StoreID="OpEventStore">
                        <Items>
                            <ext:ComboBox ID="OpEventPageSizeComboBox" runat="server" Width="150" LabelWidth="60"
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
                                    <Select Handler="#{OpEventPagingToolbar}.pageSize = parseInt(this.getValue()); #{OpEventPagingToolbar}.doLoad();" />
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
