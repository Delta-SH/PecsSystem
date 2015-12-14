<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaAvailabilityReport.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.StaAvailabilityReport" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>局站可用率</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link rel="icon" type="image/x-icon" href="favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="favicon.ico" />
    <script type="text/javascript" src="Resources/amcharts/amcharts.js?v=3.3.0.0"></script>
    <script type="text/javascript" src="Resources/js/amcharts.js?v=3.3.0.0"></script>
    <script type="text/javascript">
        //<![CDATA[
        AmCharts.ready(function() {
            StaAvailability.renderAmSerialChart();
        });
        //]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="PageViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:Panel ID="ChartPanel" runat="server" AutoScroll="true" Region="Center" Title="局站可用率(可用率={1-(故障中断时长+工程屏蔽时长)/统计时长}×100%)" Icon="ChartBar">
                <TopBar>
                    <ext:Toolbar ID="TopToolbars" runat="server" Layout="ContainerLayout">
                        <Items>
                            <ext:Toolbar ID="TopToolbar1" runat="server" Flat="true">
                                <Items>
                                    <ext:ComboBox ID="CountTypeComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="统计方式" ForceSelection="true" SelectOnFocus="true" Resizable="true">
                                        <Items>
                                            <ext:ListItem Text="按LSC统计" Value="0" />
                                            <ext:ListItem Text="按地区统计" Value="1" />
                                            <ext:ListItem Text="按县市统计" Value="2" />
                                            <ext:ListItem Text="按局站统计" Value="3" />
                                        </Items>
                                        <SelectedItem Value="0" />
                                        <Listeners>
                                            <Select Handler="#{RateCountField}.setValue('','', false);" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:DropDownField ID="RateCountField" runat="server" FieldLabel="统计选项" EmptyText="select an item..." Editable="false" TriggerIcon="Combo" Mode="ValueText" Width="360" LabelWidth="50">
                                        <Component>
                                            <ext:TreePanel ID="RateCountTreePanel" runat="server" Header="false" Lines="true" Height="300" AutoScroll="true" Animate="true" EnableDD="false" ContainerScroll="true" RootVisible="false">
                                                <Root>
                                                    <ext:TreeNode Text="Root" />
                                                </Root>
                                                <Listeners>
                                                    <CheckChange Fn="StaAvailability.checkChanged" />
                                                </Listeners>
                                                <SelectionModel>
                                                    <ext:MultiSelectionModel ID="MultiSelectionModel1" runat="server" />
                                                </SelectionModel>
                                            </ext:TreePanel>
                                        </Component>
                                        <Listeners>
                                            <Expand Handler="this.component.getRootNode().expand(false);" Single="false" Delay="200" />
                                        </Listeners>
                                        <SyncValue Fn="StaAvailability.syncValue" />
                                    </ext:DropDownField>
                                    <ext:MultiCombo ID="StaTypeMultiCombo" runat="server" Width="300" SelectionMode="All" LabelWidth="50" FieldLabel="局站类型" EmptyText="[All Items]" DisplayField="Name" ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true">
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
                                            </ext:Store>
                                        </Store>
                                    </ext:MultiCombo>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar ID="TopToolbar2" runat="server" Flat="true">
                                <Items>
                                    <ext:ComboBox ID="ChartTypeComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="图表类型" ForceSelection="true" SelectOnFocus="true" Resizable="true">
                                        <Items>
                                            <ext:ListItem Text="柱状图" Value="column" />
                                            <ext:ListItem Text="饼状图" Value="pie" />
                                        </Items>
                                        <SelectedItem Value="column" />
                                        <Listeners>
                                            <Select Fn="StaAvailability.selectType" Scope="StaAvailability" />
                                        </Listeners>
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
                                    <ext:MultiCombo ID="AlarmNameMultiCombo" runat="server" Width="300" SelectionMode="All" LabelWidth="50" FieldLabel="告警名称" EmptyText="..." DisplayField="Name" ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true">
                                        <Store>
                                            <ext:Store ID="AlarmNameStore" runat="server" AutoLoad="true" OnRefreshData="OnAlarmNameRefresh">
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
                                                    <Load Handler="#{AlarmNameMultiCombo}.selectAll();" />
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
                                                                    <ext:Parameter Name="ColumnNames" Mode="Raw" Value="StaAvailability.getColumnNames(#{MainGridPanel})" />
                                                                </ExtraParams>
                                                            </Click>
                                                        </DirectEvents>
                                                    </ext:MenuItem>
                                                </Items>
                                            </ext:Menu>
                                        </Menu>
                                        <DirectEvents>
                                            <Click OnEvent="QueryBtn_Click" Success="#{BottomPagingToolbar}.doLoad();StaAvailability.setData();">
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{MainGridPanel}.body.up('div')" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:SplitButton>
                                </Items>
                            </ext:Toolbar>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Content>
                    <div id="chartContainer">
                    </div>
                </Content>
                <Listeners>
                    <BeforeRender Fn="StaAvailability.setCenterPanel" Scope="StaAvailability" />
                </Listeners>
            </ext:Panel>
            <ext:GridPanel ID="MainGridPanel" runat="server" StripeRows="true" Header="false"
                TrackMouseOver="true" AutoScroll="true" Split="true" Collapsible="true" CollapseMode="Mini"
                Border="false" Region="South">
                <Store>
                    <ext:Store ID="GridStore" runat="server" AutoLoad="false" IgnoreExtraFields="false" OnRefreshData="OnGridStoreRefresh">
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
                <LoadMask ShowMask="true" />
                <ColumnModel ID="GridColumnModel" runat="server">
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="GridRowSelectionModel" runat="server" SingleSelect="true" />
                </SelectionModel>
                <View>
                    <ext:LockingGridView ID="GridLockingGridView" runat="server" ForceFit="false" />
                </View>
                <Listeners>
                    <BeforeRender Fn="StaAvailability.setSouthPanel" Scope="StaAvailability" />
                </Listeners>
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
