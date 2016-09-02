<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="energy002.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.Energy.energy002" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用电趋势分析</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <link rel="shortcut icon" type="image/x-icon" href="../favicon.ico" />
    <link rel="icon" type="image/x-icon" href="../favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="../favicon.ico" />
    <script type="text/javascript" src="../Resources/echarts/echarts.common.min.js"></script>
    <script type="text/javascript" src="../Resources/echarts/shine.js"></script>
    <script type="text/javascript" src="../Resources/js/public.js?v=3.3.0.0"></script>
    <script type="text/javascript">
        Ext.onReady(function () {
            Energy002.initChart();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" /> 
        <ext:Viewport ID="MainViewport" runat="server" Layout="BorderLayout">
            <Items>
                <ext:Panel ID="currentLayout" runat="server" Border="false" Layout="VBoxLayout" Region="Center" Title="用电趋势分析列表">
                    <LayoutConfig>
                        <ext:VBoxLayoutConfig Padding="0" Align="Stretch" />
                    </LayoutConfig>
                    <TopBar>
                        <ext:Toolbar ID="Condition1" runat="server">
                            <Items>
                                <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="3" />
                                <ext:DropDownField ID="RangeField" runat="server" FieldLabel="统计范围" EmptyText="请选择统计范围..." Editable="false" TriggerIcon="Combo" Mode="ValueText" Width="250" LabelWidth="50" Text="全部" UnderlyingValue="root">
                                    <Component>
                                        <ext:TreePanel ID="RangeTreePanel" runat="server" Header="false" Lines="true" AutoScroll="true" Animate="true" EnableDD="false" ContainerScroll="true" RootVisible="true" Height="300">
                                            <Root>
                                                <ext:TreeNode NodeID="root" Text="全部" Icon="World"/>
                                            </Root>
                                            <Listeners>
                                                <Click Fn="Energy002.itemClick" />
                                            </Listeners>
                                        </ext:TreePanel>
                                    </Component>
                                    <SyncValue Fn="Energy002.syncValue" />
                                </ext:DropDownField>
                                <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="5" />
                                <ext:ComboBox ID="PeriodField" runat="server" Width="200" LabelWidth="50" FieldLabel="统计周期" ForceSelection="true" SelectOnFocus="true" Resizable="true">
                                    <Items>
                                        <ext:ListItem Text="按月统计" Value="0" />
                                        <ext:ListItem Text="按周统计" Value="1" />
                                        <ext:ListItem Text="按日统计" Value="2" />
                                    </Items>
                                    <SelectedItem Value="0" />
                                </ext:ComboBox>
                                <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="5" />
                                <ext:TriggerField ID="StartDate" runat="server" Width="200" LabelWidth="50" FieldLabel="统计时段">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Date" />
                                    </Triggers>
                                    <Listeners>
                                        <TriggerClick Handler="WdatePicker({el:'StartDate',isShowClear:false,readOnly:true,isShowWeek:true,dateFmt:'yyyy-MM-dd',maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{EndDate}.fireEvent('triggerclick')}});" />
                                    </Listeners>
                                </ext:TriggerField>
                                <ext:TriggerField ID="EndDate" runat="server" Width="200" LabelWidth="50" FieldLabel="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" LabelSeparator="~">
                                    <Triggers>
                                        <ext:FieldTrigger Icon="Date" />
                                    </Triggers>
                                    <Listeners>
                                        <TriggerClick Handler="WdatePicker({el:'EndDate',isShowClear:false,readOnly:true,isShowWeek:true,dateFmt:'yyyy-MM-dd',minDate:'#F{$dp.$D(\'StartDate\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                    </Listeners>
                                </ext:TriggerField>
                                <ext:ToolbarSpacer ID="ToolbarSpacer4" runat="server" Width="5" />
                                <ext:Button ID="QueryButton" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                    <DirectEvents>
                                        <Click OnEvent="QueryButtonClick" Success="#{MainPagingToolbar}.doLoad();Energy002.refreshChart();">
                                            <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{currentLayout}.body.up('div')" />
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                                <ext:ToolbarSeparator runat="server" />
                                <ext:Button ID="SaveButton" runat="server" Text="导出" Icon="Printer" StandOut="true">
                                    <DirectEvents>
                                        <Click OnEvent="SaveButtonClick" IsUpload="true">
                                            <ExtraParams>
                                                <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{MainGridPanel})" />
                                            </ExtraParams>
                                        </Click>
                                    </DirectEvents>
                                </ext:Button>
                            </Items>
                        </ext:Toolbar>
                    </TopBar>
                    <Items>
                        <ext:Panel ID="ChartsContainer" runat="server" Height="200" Border="false">
                            <Content>
                                <div id="LineChart" style="height:200px;width:100%;"></div>
                            </Content>
                        </ext:Panel>
                        <ext:GridPanel ID="MainGridPanel" runat="server" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Border="true" Flex="1">
                            <Store>
                                <ext:Store ID="MainStore" runat="server" AutoLoad="false" IgnoreExtraFields="false" OnRefreshData="MainStoreRefresh">
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
                             <ColumnModel runat="server">
                            </ColumnModel>
                            <SelectionModel>
                                <ext:CellSelectionModel runat="server" />
                            </SelectionModel>
                            <Listeners>
                                <CellDblClick Fn="Energy002.cellDblClick" Scope="Energy002" />
                            </Listeners>
                            <LoadMask ShowMask="true" />
                            <View>
                                <ext:GridView runat="server" ForceFit="false" AutoFill="true" />
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
                </ext:Panel>
            </Items>
        </ext:Viewport>  
    </form>
</body>
</html>
