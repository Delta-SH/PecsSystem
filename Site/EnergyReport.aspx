﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnergyReport.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.EnergyReport" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>能耗管理</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link rel="icon" type="image/x-icon" href="favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="favicon.ico" />
    <script type="text/javascript" src="Resources/js/public.js?v=3.3.0.0"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X"
        IDMode="Explicit" />
    <ext:Viewport ID="PageViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="MainGridPanel" runat="server" StripeRows="true" Border="false"
                TrackMouseOver="true" AutoScroll="true" Region="Center" Title="能耗管理" Icon="ApplicationViewList">
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
                                    </ext:ComboBox>
                                    <ext:TriggerField ID="CountItemField" runat="server" Width="180" LabelWidth="50"
                                        FieldLabel="统计名称">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" HideTrigger="false" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="this.setValue('');" />
                                        </Listeners>
                                    </ext:TriggerField>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar ID="TopToolbar2" runat="server" Flat="true">
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
                                    <ext:RecordField Name="LscName" Type="String" />
                                    <ext:RecordField Name="Area1Name" Type="String" />
                                    <ext:RecordField Name="Area2Name" Type="String" />
                                    <ext:RecordField Name="Area3Name" Type="String" />
                                    <ext:RecordField Name="StaName" Type="String" />
                                    <ext:RecordField Name="StaTypeName" Type="String" />
                                    <ext:RecordField Name="SumValue" Type="Float" />
                                    <ext:RecordField Name="YoYValue" Type="Float" />
                                    <ext:RecordField Name="YoY" Type="String" />
                                    <ext:RecordField Name="QoQValue" Type="Float" />
                                    <ext:RecordField Name="QoQ" Type="String" />
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
                        <ext:Column Header="局站类型" DataIndex="StaTypeName" Align="Left" />
                        <ext:Column Header="合计度数" DataIndex="SumValue" Align="Left" />
                        <ext:Column Header="去年同期" DataIndex="YoYValue" Align="Left" />
                        <ext:Column Header="同比增长率" DataIndex="YoY" Align="Left" />
                        <ext:Column Header="上月同期" DataIndex="QoQValue" Align="Left" />
                        <ext:Column Header="环比增长率" DataIndex="QoQ" Align="Left" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="GridRowSelectionModel" runat="server" SingleSelect="true" />
                </SelectionModel>
                <View>
                    <ext:GridView ID="GridGridView" runat="server" ForceFit="false" />
                </View>
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
