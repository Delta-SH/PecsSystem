﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="report102.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.report102" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>直流不间断系统负载率</title>
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
            <ext:GridPanel ID="MainGridPanel" runat="server" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Region="Center" Height="500px" Title="直流不间断系统负载率 = ((电池充电电流+系统当前最大负载电流)/(系统设计容量-冗余模块容量))×100%" Icon="ApplicationViewList">
                <TopBar>
                    <ext:Toolbar ID="TopToolbars" runat="server" Layout="ContainerLayout">
                        <Items>
                            <ext:Toolbar ID="TopToolbar1" runat="server" Flat="true">
                                <Items>
                                    <ext:ComboBox ID="LscsComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="Lsc名称" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true" Resizable="true">
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
                                        <Listeners>
                                            <Select Handler="#{Area2ComboBox}.clearValue();#{Area2ComboBox}.store.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="Area2ComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="地区名称" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true" Resizable="true">
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
                                    <ext:ComboBox ID="Area3ComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="县市名称" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true" Resizable="true">
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
                                    <ext:MultiCombo ID="DevTypeMultiCombo" runat="server" Width="180" SelectionMode="All" LabelWidth="50" FieldLabel="设备类型" EmptyText="select an item..." DisplayField="Name" ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true">
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
                                    <ext:ComboBox ID="FilterTypeList" runat="server" FieldLabel="充电电流" LabelWidth="50" Width="180" Resizable="true">
                                        <Items>
                                            <ext:ListItem Text="按测点名称过滤" Value="0" />
                                            <ext:ListItem Text="按测点标记过滤" Value="1" />
                                        </Items>
                                        <SelectedItem Value="0" />
                                    </ext:ComboBox>
                                    <ext:TextField ID="NodeText" runat="server" Width="180" MaxLength="100" EmptyText="多条件请以；分隔" Text="" />
                                    <ext:ComboBox ID="MFilterTypeList" runat="server" FieldLabel="负载电流" LabelWidth="50" Width="180" Resizable="true">
                                        <Items>
                                            <ext:ListItem Text="按测点名称过滤" Value="0" />
                                            <ext:ListItem Text="按测点标记过滤" Value="1" />
                                        </Items>
                                        <SelectedItem Value="0" />
                                    </ext:ComboBox>
                                    <ext:TextField ID="MNodeText" runat="server" Width="180" MaxLength="100" EmptyText="多条件请以；分隔" Text="" />
                                    <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5" />
                                    <ext:SplitButton ID="QueryBtn" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                        <Menu>
                                            <ext:Menu ID="QueryMenu1" runat="server" RenderToForm="true">
                                                <Items>
                                                    <ext:CheckMenuItem ID="CapacityMenuItem" runat="server" Text="自定义设计容量" HideOnClick="false" Checked="false">
                                                        <Listeners>
                                                            <CheckChange Handler="#{CapacitySpinnerField}.setDisabled(!this.checked);" />
                                                        </Listeners>
                                                    </ext:CheckMenuItem>
                                                    <ext:ComponentMenuItem ID="CapacityComponentMenuItem" runat="server">
                                                        <Component>
                                                            <ext:SpinnerField ID="CapacitySpinnerField" runat="server" Width="180" DecimalPrecision="3" IncrementValue="1" MinValue="0" Number="0" Disabled="true" />
                                                        </Component>
                                                    </ext:ComponentMenuItem>
                                                    <ext:MenuSeparator ID="QueryMenuSeparator1" runat="server" />
                                                    <ext:CheckMenuItem ID="RedundanceMenuItem" runat="server" Text="自定义冗余容量" HideOnClick="false" Checked="false">
                                                        <Listeners>
                                                            <CheckChange Handler="#{RedundanceSpinnerField}.setDisabled(!this.checked);" />
                                                        </Listeners>
                                                    </ext:CheckMenuItem>
                                                    <ext:ComponentMenuItem ID="RedundanceComponentMenuItem" runat="server">
                                                        <Component>
                                                            <ext:SpinnerField ID="RedundanceSpinnerField" runat="server" Width="180" DecimalPrecision="3" IncrementValue="1" MinValue="0" Number="0" Disabled="true" />
                                                        </Component>
                                                    </ext:ComponentMenuItem>
                                                    <ext:MenuSeparator ID="QueryMenuSeparator2" runat="server" />
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
                                            <Click OnEvent="QueryBtn_Click" Failure="return false;" Success="#{MainPagingToolbar}.doLoad();">
                                                <EventMask ShowMask="true" Msg="请求最新数据，请稍后..." Target="CustomTarget" CustomTarget="#{MainGridPanel}.body.up('div')" />
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
                                    <ext:RecordField Name="Area1Name" Type="String" />
                                    <ext:RecordField Name="Area2Name" Type="String" />
                                    <ext:RecordField Name="Area3ID" Type="Int" />
                                    <ext:RecordField Name="Area3Name" Type="String" />
                                    <ext:RecordField Name="BuildingID" Type="Int" />
                                    <ext:RecordField Name="BuildingName" Type="String" />
                                    <ext:RecordField Name="FillCapacity" Type="Float" />
                                    <ext:RecordField Name="CurrentCapacity" Type="Float" />
                                    <ext:RecordField Name="DesignCapacity" Type="Float" />
                                    <ext:RecordField Name="RedundanceCapacity" Type="Float" />
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
                        <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" />
                        <ext:Column Header="地区名称" DataIndex="Area2Name" Align="Left" />
                        <ext:Column Header="县市名称" DataIndex="Area3Name" Align="Left" />
                        <ext:Column Header="机楼名称" DataIndex="BuildingName" Align="Left" />
                        <ext:Column Header="电池充电电流(AH)" DataIndex="FillCapacity" Align="Left" />
                        <ext:Column Header="当前最大负载电流(AH)" DataIndex="CurrentCapacity" Align="Left" />
                        <ext:Column Header="系统设计容量(AH)" DataIndex="DesignCapacity" Align="Left" />
                        <ext:Column Header="冗余模块容量(AH)" DataIndex="RedundanceCapacity" Align="Left" />
                        <ext:Column Header="系统负载率" DataIndex="Rate" Align="Left" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:CellSelectionModel ID="MainCellSelectionModel" runat="server" />
                </SelectionModel>
                <View>
                    <ext:GridView ID="MainGridView" runat="server" ForceFit="false" />
                </View>
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
