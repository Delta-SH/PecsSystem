<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DivWnd.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.DivWnd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>测值详单</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <script type="text/javascript" src="Resources/js/public.js?v=3.3.0.0"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="WndViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="WndGridPanel" runat="server" Icon="ApplicationViewColumns" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Border="false" Region="Center">
                <Plugins>
                    <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh1" runat="server" />
                </Plugins>
                <Store>
                    <ext:Store ID="WndStore" runat="server" AutoLoad="true" OnRefreshData="OnWndRefresh" OnSubmitData="OnWnd_Submit">
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
                                    <ext:RecordField Name="Value" Type="Int" />
                                    <ext:RecordField Name="ValueDesc" Type="String" />
                                    <ext:RecordField Name="UpdateTime" Type="String" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                        <BaseParams>
                            <ext:Parameter Name="start" Value="0" Mode="Raw" />
                            <ext:Parameter Name="limit" Value="#{PageSizeComboBox}.getValue()" Mode="Raw" />
                        </BaseParams>
                        <WriteBaseParams>
                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{WndGridPanel})" />
                        </WriteBaseParams>
                        <DirectEventConfig IsUpload="true" />
                    </ext:Store>
                </Store>
                <ColumnModel ID="WndColumnModel" runat="server">
                    <Columns>
                        <ext:Column Header="序号" DataIndex="ID" Align="Left" />
                        <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" />
                        <ext:Column Header="地区名称" DataIndex="Area2Name" Align="Left" />
                        <ext:Column Header="县市名称" DataIndex="Area3Name" Align="Left" />
                        <ext:Column Header="局站名称" DataIndex="StaName" Align="Left" />
                        <ext:Column Header="设备名称" DataIndex="DevName" Align="Left" />
                        <ext:Column Header="测点名称" DataIndex="NodeName" Align="Left" />
                        <ext:Column Header="监测值" DataIndex="Value" Align="Left" />
                        <ext:Column Header="触发时间" DataIndex="UpdateTime" Align="Left" />
                        <ext:Column Header="测值描述" DataIndex="ValueDesc" Align="Left" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="WndRowSelectionModel" runat="server" SingleSelect="true" />
                </SelectionModel>
                <LoadMask ShowMask="true" />
                <View>
                    <ext:GridView ID="WndGridView" runat="server" ForceFit="false" />
                </View>
                <Listeners>
                    <RowContextMenu Fn="DivWnd.showGridRowContextMenu" Scope="DivWnd" />
                </Listeners>
                <BottomBar>
                    <ext:PagingToolbar ID="WndGridPagingToolbar" runat="server" PageSize="20" StoreID="WndStore">
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
                                    <Select Handler="#{WndGridPagingToolbar}.pageSize = parseInt(this.getValue()); #{WndGridPagingToolbar}.doLoad();" />
                                </Listeners>
                            </ext:ComboBox>
                        </Items>
                    </ext:PagingToolbar>
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    <ext:Menu ID="WndGridRowMenu" runat="server">
        <Items>
            <ext:MenuItem ID="WndGridRowItem1" runat="server" Text="打印/导出" Icon="Printer">
                <Listeners>
                    <Click Handler="#{WndGridPanel}.submitData(false);" />
                </Listeners>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    </form>
</body>
</html>
