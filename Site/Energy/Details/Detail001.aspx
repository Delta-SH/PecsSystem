<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Detail001.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.Detail001" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>能耗详单</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <script type="text/javascript" src="/Resources/js/public.js?v=3.3.0.0"></script>
    <script type="text/javascript">
        var ShowContextMenu = function(el, rowIndex, e) {
            e.preventDefault();
            el.getSelectionModel().selectRow(rowIndex);
            MainGridRowMenu.dataRecord = el.store.getAt(rowIndex);
            MainGridRowMenu.showAt(e.getXY());
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="MainGridPanel" runat="server" Icon="ApplicationViewColumns" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Border="false" Region="Center">
                <Plugins>
                    <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh1" runat="server" />
                </Plugins>
                <Store>
                    <ext:Store ID="MainStore" runat="server" AutoLoad="true" OnRefreshData="OnStoreRefresh" OnSubmitData="OnStoreSubmit">
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                        <Reader>
                            <ext:JsonReader>
                                <Fields>
                                    <ext:RecordField Name="Name" Type="String" />
                                    <ext:RecordField Name="Start" Type="String" />
                                    <ext:RecordField Name="End" Type="String" />
                                    <ext:RecordField Name="Value" Type="Float" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                        <BaseParams>
                            <ext:Parameter Name="start" Value="0" Mode="Raw" />
                            <ext:Parameter Name="limit" Value="#{PageSizeComboBox}.getValue()" Mode="Raw" />
                        </BaseParams>
                        <WriteBaseParams>
                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{MainGridPanel})" />
                        </WriteBaseParams>
                        <DirectEventConfig IsUpload="true" />
                    </ext:Store>
                </Store>
                <ColumnModel runat="server">
                    <Columns>
                        <ext:Column Header="名称" DataIndex="Name" Align="Left" />
                        <ext:Column Header="开始日期" DataIndex="Start" Align="Left" />
                        <ext:Column Header="结束日期" DataIndex="End" Align="Left" />
                        <ext:Column Header="用电量" DataIndex="Value" Align="Left" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel runat="server" SingleSelect="true" />
                </SelectionModel>
                <LoadMask ShowMask="true" />
                <View>
                    <ext:GridView runat="server" ForceFit="false" />
                </View>
                <Listeners>
                    <RowContextMenu Fn="ShowContextMenu"/>
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
    <ext:Menu ID="MainGridRowMenu" runat="server">
        <Items>
            <ext:MenuItem runat="server" Text="打印/导出" Icon="Printer">
                <Listeners>
                    <Click Handler="#{MainGridPanel}.submitData(false);" />
                </Listeners>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    </form>
</body>
</html>
