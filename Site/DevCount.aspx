﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DevCount.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.DevCount" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>设备统计</title>
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
    <ext:Viewport ID="DevCntViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="DevCntGridPanel" runat="server" Icon="ApplicationViewColumns" Title="设备统计" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Border="false" Region="Center">
                <TopBar>
                    <ext:Toolbar ID="TopToolbars" runat="server" Layout="ContainerLayout">
                        <Items>
                            <ext:Toolbar ID="TopToolbar1" runat="server" Flat="true">
                                <Items>
                                    <ext:ComboBox ID="CountTypeComboBox" runat="server" Width="300" LabelWidth="50" FieldLabel="统计方式"
                                        ForceSelection="true" SelectOnFocus="true" Resizable="true">
                                        <Items>
                                            <ext:ListItem Text="按LSC统计" Value="0" />
                                            <ext:ListItem Text="按地区统计" Value="1" />
                                            <ext:ListItem Text="按县市统计" Value="2" />
                                            <ext:ListItem Text="按局站统计" Value="3" />
                                        </Items>
                                        <SelectedItem Value="0" />
                                        <Listeners>
                                            <Select Handler="#{DevCountField}.setValue('','', false);" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:DropDownField ID="DevCountField" runat="server" FieldLabel="统计选项" EmptyText="select an item..." Editable="false" TriggerIcon="Combo" Mode="ValueText" Width="350" LabelWidth="50">
                                        <Component>
                                            <ext:TreePanel ID="DevCountTreePanel" runat="server" Header="false" Lines="true"
                                                AutoScroll="true" Animate="true" EnableDD="false" ContainerScroll="true" RootVisible="false"
                                                Height="300">
                                                <Root>
                                                    <ext:TreeNode Text="Root" />
                                                </Root>
                                                <Listeners>
                                                    <CheckChange Fn="DevCnt.checkChanged" />
                                                </Listeners>
                                                <SelectionModel>
                                                    <ext:MultiSelectionModel ID="MultiSelectionModel1" runat="server" />
                                                </SelectionModel>
                                            </ext:TreePanel>
                                        </Component>
                                        <Listeners>
                                            <Expand Handler="this.component.getRootNode().expand(false);" Single="false" Delay="200" />
                                        </Listeners>
                                        <SyncValue Fn="DevCnt.syncValue" />
                                    </ext:DropDownField>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar ID="TopToolbar2" runat="server" Flat="true">
                                <Items>
                                    <ext:MultiCombo ID="StaTypeMultiCombo" runat="server" Width="300" SelectionMode="All"
                                        LabelWidth="50" FieldLabel="局站类型" EmptyText="[All Items]" DisplayField="Name"
                                        ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true">
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
                                    <ext:MultiCombo ID="DevTypeMultiCombo" runat="server" Width="350" SelectionMode="All"
                                        LabelWidth="50" FieldLabel="设备类型" EmptyText="select an item..." DisplayField="Name"
                                        ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true">
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
                                                    <Load Handler="#{DevTypeMultiCombo}.selectAll();" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                    </ext:MultiCombo>
                                    <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5" />
                                    <ext:SplitButton ID="QueryBtn" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                        <Menu>
                                            <ext:Menu ID="QueryMenu" runat="server">
                                                <Items>
                                                    <ext:MenuItem ID="SaveMenuItem" runat="server" Text="打印/导出" Icon="Printer">
                                                        <DirectEvents>
                                                            <Click OnEvent="SaveBtn_Click" IsUpload="true">
                                                                <ExtraParams>
                                                                    <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{DevCntGridPanel})" />
                                                                </ExtraParams>
                                                            </Click>
                                                        </DirectEvents>
                                                    </ext:MenuItem>
                                                </Items>
                                            </ext:Menu>
                                        </Menu>
                                        <DirectEvents>
                                            <Click OnEvent="QueryBtn_Click" Success="#{DevCntGridPagingToolbar}.doLoad();">
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{DevCntGridPanel}.body.up('div')" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:SplitButton>
                                </Items>
                            </ext:Toolbar>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store ID="DevCntStore" runat="server" AutoLoad="false" IgnoreExtraFields="false" OnRefreshData="OnDevCntRefresh">
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                        <Reader>
                            <ext:JsonReader />
                        </Reader>
                        <BaseParams>
                            <ext:Parameter Name="start" Value="0" Mode="Raw" />
                            <ext:Parameter Name="limit" Value="#{PageSizeComboBox}.getValue()" Mode="Raw" />
                        </BaseParams>
                        <DirectEventConfig IsUpload="true" />
                    </ext:Store>
                </Store>
                <ColumnModel ID="DevCntColumnModel" runat="server">
                </ColumnModel>
                <SelectionModel>
                    <ext:CellSelectionModel ID="DevCntCellSelectionModel" runat="server" />
                </SelectionModel>
                <Listeners>
                    <CellDblClick Fn="DevCnt.onGridCellDblClick" Scope="DevCnt" />
                </Listeners>
                <LoadMask ShowMask="true" />
                <View>
                    <ext:LockingGridView ID="DevCntGridView" runat="server" ForceFit="false" />
                </View>
                <BottomBar>
                    <ext:PagingToolbar ID="DevCntGridPagingToolbar" runat="server" PageSize="20" StoreID="DevCntStore">
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
                                    <Select Handler="#{DevCntGridPagingToolbar}.pageSize = parseInt(this.getValue()); #{DevCntGridPagingToolbar}.doLoad();" />
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
