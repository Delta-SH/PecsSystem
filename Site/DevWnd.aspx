<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DevWnd.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.DevWnd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>设备详单</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <script type="text/javascript" src="Resources/js/public.js?v=3.3.0.0"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="DevWndViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="DevWndGridPanel" runat="server" Icon="ApplicationViewColumns" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Border="false" Region="Center">
                <Plugins>
                    <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh1" runat="server" />
                </Plugins>
                <Store>
                    <ext:Store ID="DevWndStore" runat="server" AutoLoad="true" OnRefreshData="OnDevWndRefresh" OnSubmitData="OnDevWnd_Submit">
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
                                    <ext:RecordField Name="DevID" Type="String" />
                                    <ext:RecordField Name="DevName" Type="String" />
                                    <ext:RecordField Name="DevTypeName" Type="String" />
                                    <ext:RecordField Name="DevDesc" Type="String" />
                                    <ext:RecordField Name="ProdName" Type="String" />
                                    <ext:RecordField Name="MID" Type="String" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                        <BaseParams>
                            <ext:Parameter Name="start" Value="0" Mode="Raw" />
                            <ext:Parameter Name="limit" Value="#{PageSizeComboBox}.getValue()" Mode="Raw" />
                        </BaseParams>
                        <WriteBaseParams>
                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{DevWndGridPanel})" />
                        </WriteBaseParams>
                        <DirectEventConfig IsUpload="true" />
                    </ext:Store>
                </Store>
                <ColumnModel ID="DevWndColumnModel" runat="server">
                    <Columns>
                        <ext:Column Header="序号" DataIndex="ID" Align="Left" />
                        <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" />
                        <ext:Column Header="地区" DataIndex="Area2Name" Align="Left" />
                        <ext:Column Header="县市" DataIndex="Area3Name" Align="Left" />
                        <ext:Column Header="局站" DataIndex="StaName" Align="Left" />
                        <ext:Column Header="设备编号" DataIndex="DevID" Align="Left" />
                        <ext:Column Header="设备名称" DataIndex="DevName" Align="Left" />
                        <ext:Column Header="设备类型" DataIndex="DevTypeName" Align="Left" />
                        <ext:Column Header="设备描述" DataIndex="DevDesc" Align="Left" />
                        <ext:Column Header="设备厂家" DataIndex="ProdName" Align="Left" />
                        <ext:Column Header="MID" DataIndex="MID" Align="Left" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="DevWndRowSelectionModel" runat="server" SingleSelect="true" />
                </SelectionModel>
                <LoadMask ShowMask="true" />
                <View>
                    <ext:GridView ID="DevWndGridView" runat="server" ForceFit="false" />
                </View>
                <Listeners>
                    <RowContextMenu Fn="DevWnd.showGridRowContextMenu" Scope="DevWnd" />
                </Listeners>
                <BottomBar>
                    <ext:PagingToolbar ID="DevWndGridPagingToolbar" runat="server" PageSize="20" StoreID="DevWndStore">
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
                                    <Select Handler="#{DevWndGridPagingToolbar}.pageSize = parseInt(this.getValue()); #{DevWndGridPagingToolbar}.doLoad();" />
                                </Listeners>
                            </ext:ComboBox>
                        </Items>
                    </ext:PagingToolbar>
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    <ext:Menu ID="DevWndGridRowMenu" runat="server">
        <Items>
            <ext:MenuItem ID="DevWndGridRowItem1" runat="server" Text="打印/导出" Icon="Printer">
                <Listeners>
                    <Click Handler="#{DevWndGridPanel}.submitData(false);" />
                </Listeners>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    </form>
</body>
</html>
