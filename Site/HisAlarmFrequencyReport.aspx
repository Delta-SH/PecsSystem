<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HisAlarmFrequencyReport.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.HisAlarmFrequencyReport" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>超频告警统计</title>
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
    <ext:Viewport ID="MainViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="MainGridPanel" runat="server" Icon="ApplicationViewColumns" Title="超短告警统计" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Border="false" Region="Center">
                <TopBar>
                    <ext:Toolbar ID="TopToolbars" runat="server" Layout="ContainerLayout">
                        <Items>
                            <ext:Toolbar ID="TopToolbar1" runat="server" Flat="true">
                                <Items>
                                    <ext:ComboBox ID="CountTypeComboBox" runat="server" Width="360" LabelWidth="50" FieldLabel="统计方式" ForceSelection="true" SelectOnFocus="true" Resizable="true">
                                        <Items>
                                            <ext:ListItem Text="按LSC统计" Value="0" />
                                            <ext:ListItem Text="按地区统计" Value="1" />
                                            <ext:ListItem Text="按县市统计" Value="2" />
                                            <ext:ListItem Text="按局站统计" Value="3" />
                                        </Items>
                                        <SelectedItem Value="0" />
                                        <Listeners>
                                            <Select Handler="#{CountItemField}.clearValue();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:DropDownField ID="CountItemField" runat="server" FieldLabel="统计选项" EmptyText="select an item..." Editable="false" TriggerIcon="Combo" Mode="ValueText" Width="360" LabelWidth="50" AllowBlank="false">
                                        <Component>
                                            <ext:TreePanel ID="CountItemTreePanel" runat="server" Header="false" Lines="true" AutoScroll="true" Animate="true" EnableDD="false" ContainerScroll="true" RootVisible="false" Height="300">
                                                <Root>
                                                    <ext:TreeNode Text="Root" />
                                                </Root>
                                                <Listeners>
                                                    <CheckChange Fn="HisAlmFre.checkChanged" />
                                                </Listeners>
                                                <SelectionModel>
                                                    <ext:MultiSelectionModel ID="MultiSelectionModel1" runat="server" />
                                                </SelectionModel>
                                            </ext:TreePanel>
                                        </Component>
                                        <Listeners>
                                            <Expand Handler="this.component.getRootNode().expand(false);" Single="false" Delay="200" />
                                        </Listeners>
                                        <SyncValue Fn="HisAlmFre.syncValue" />
                                    </ext:DropDownField>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar ID="TopToolbar2" runat="server" Flat="true">
                                <Items>
                                    <ext:TriggerField ID="BeginFromDate" runat="server" Width="180" LabelWidth="50" FieldLabel="告警时间">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Date" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="WdatePicker({el:'BeginFromDate',isShowClear:false,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{BeginToDate}.fireEvent('triggerclick')}});" />
                                        </Listeners>
                                    </ext:TriggerField>
                                    <ext:TriggerField ID="BeginToDate" runat="server" Width="180" LabelWidth="50" FieldLabel="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                        LabelSeparator="~">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Date" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="WdatePicker({el:'BeginToDate',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'BeginFromDate\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                        </Listeners>
                                    </ext:TriggerField>
                                    <ext:MultiCombo ID="AlarmLevelMultiCombo" runat="server" SelectionMode="All" Width="180" LabelWidth="50" FieldLabel="告警级别" EmptyText="select an item..." DisplayField="Name" ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true" AllowBlank="false">
                                        <Store>
                                            <ext:Store ID="AlarmLevelStore" runat="server" AutoLoad="true" OnRefreshData="OnAlarmLevelRefresh">
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
                                                    <Load Handler="#{AlarmLevelMultiCombo}.selectAll();" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                    </ext:MultiCombo>
                                    <ext:DisplayField ID="MinDisplayField" runat="server" FieldLabel="超频阈值(次)" LabelWidth="70" />
                                    <ext:SpinnerField ID="MinSpinnerField" runat="server" Width="105" AllowBlank="false" AllowDecimals="false" IncrementValue="1" MinValue="1" Number="5" EmptyText="sets a value..." />
                                    <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5" />
                                    <ext:SplitButton ID="QueryBtn" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                        <Menu>
                                            <ext:Menu ID="QueryMenu" runat="server">
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
                                            <Click OnEvent="QueryBtn_Click" Success="#{MainGridPagingToolbar}.doLoad();">
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
                    <ext:Store ID="MainGridStore" runat="server" AutoLoad="false" IgnoreExtraFields="false" OnRefreshData="OnMainGridRefresh">
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
                <ColumnModel ID="MainGridColumnModel" runat="server">
                </ColumnModel>
                <SelectionModel>
                    <ext:CellSelectionModel ID="MainGridCellSelectionModel" runat="server" />
                </SelectionModel>
                <Listeners>
                    <CellDblClick Fn="HisAlmFre.onGridCellDblClick" Scope="HisAlmFre" />
                </Listeners>
                <LoadMask ShowMask="true" />
                <View>
                    <ext:LockingGridView ID="MainGridView" runat="server" ForceFit="false" />
                </View>
                <BottomBar>
                    <ext:PagingToolbar ID="MainGridPagingToolbar" runat="server" PageSize="20" StoreID="MainGridStore">
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
                                    <Select Handler="#{MainGridPagingToolbar}.pageSize = parseInt(this.getValue()); #{MainGridPagingToolbar}.doLoad();" />
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
