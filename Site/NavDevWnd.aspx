<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NavDevWnd.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.NavDevWnd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>设备列表信息</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <script type="text/javascript" src="Resources/js/map.dev.js?v=3.3.0.0"></script>
    <script type="text/javascript">
        Ext.onReady(function() {
            NavDev.InitClass();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="NavDevWndViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:TreePanel ID="NavTreePanel" runat="server" Margins="4 0 4 4" Title="设备列表" Lines="true" CollapseFirst="false" ContainerScroll="true" AutoScroll="true" Split="true" CollapseMode="Mini" Width="200" Region="West">
                <TopBar>
                    <ext:Toolbar ID="NavTreeToolbar" runat="server">
                        <Items>
                            <ext:TextField ID="DevFilterTextField" runat="server" Width="160" EmptyText="输入设备名称..."></ext:TextField>
                            <ext:ToolbarSpacer ID="NavTreeToolbarSpacer1" runat="server" Width="5" />
                            <ext:Button ID="DevFilterButton" runat="server" Icon="Magnifier" ToolTip="查询">
                                <Listeners>
                                    <Click Fn="NavDev.OnFilterTextFieldClick" Scope="NavDev"/>
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Root>
                </Root>
                <Listeners>
                    <Click Fn="NavDev.OnTreeItemClick" Scope="NavDev"/>
                </Listeners>
            </ext:TreePanel>
            <ext:TabPanel ID="NodesTabPanel" runat="server" Margins="4 4 4 0" MinTabWidth="50" EnableTabScroll="true" Region="Center">
                <Items>
                    <ext:GridPanel ID="NodesGridTabPanel1" runat="server" Header="false" StripeRows="true" Title="测点列表" AutoExpandColumn="NodeName" Icon="ApplicationViewColumns" TrackMouseOver="true" Layout="FitLayout" CloseAction="Hide">
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh1" runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="NodesGridStore1" runat="server" GroupField="TypeName">
                                <Reader>
                                    <ext:JsonReader IDProperty="NodeID">
                                        <Fields>
                                            <ext:RecordField Name="LscID" Type="Int" />
                                            <ext:RecordField Name="DevID" Type="Int" />
                                            <ext:RecordField Name="DotID" Type="Int" />
                                            <ext:RecordField Name="NodeID" Type="Int" />
                                            <ext:RecordField Name="NodeName" Type="String" />
                                            <ext:RecordField Name="NodeType" Type="Int" />
                                            <ext:RecordField Name="TypeName" Type="String" />
                                            <ext:RecordField Name="Value" Type="Float" />
                                            <ext:RecordField Name="ValueName" Type="String" />
                                            <ext:RecordField Name="Datetime" Type="String" />
                                            <ext:RecordField Name="Status" Type="Int" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="NodesGridColumnModel1" runat="server">
                            <Columns>
                                <ext:Column Header="序号" DataIndex="DotID" Align="Left" Groupable="false" />
                                <ext:Column Header="类型" DataIndex="TypeName" Align="Center" Groupable="false" />
                                <ext:Column Header="测点名称" DataIndex="NodeName" Align="Left" Groupable="false" />
                                <ext:Column Header="监测值" DataIndex="ValueName" Align="Left" Groupable="false" />
                                <ext:Column Header="告警时间" DataIndex="Datetime" Align="Center" Groupable="false" />
                                <ext:ImageCommandColumn Header="操作" Align="Center">
                                    <Commands>
                                        <ext:ImageCommand CommandName="Ctrl" Icon="CogEdit" Text="控制">
                                            <ToolTip Text="遥控参数值设置" />
                                        </ext:ImageCommand>
                                        <ext:ImageCommand CommandName="Set" Icon="CogEdit" Text="设置">
                                            <ToolTip Text="遥调参数值设置" />
                                        </ext:ImageCommand>
                                    </Commands>
                                    <PrepareCommand Fn="NavDev.PrepareCommand" />
                                </ext:ImageCommandColumn>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="NodesGridRowSelectionModel1" runat="server" />
                        </SelectionModel>
                        <View>
                            <ext:GroupingView ID="NodesGridGroupingView1" runat="server" EnableRowBody="true" EnableGroupingMenu="false" ForceFit="true" GroupTextTpl="{text} ({[values.rs.length]} 条)">
                                <GetRowClass Fn="NavDev.SetGridNodesRowColor" />
                            </ext:GroupingView>
                        </View>
                        <Listeners>
                            <Activate Fn="NavDev.OnNodesGridTabPanelActivate" Scope="NavDev" />
                            <Command Fn="NavDev.OnNodesGridTabPanelCmdClick" Scope="NavDev"/>
                            <RowContextMenu Fn="NavDev.OnNodesGridRowContextMenuShow" Scope="NavDev" />
                        </Listeners>
                    </ext:GridPanel>
                    <ext:GridPanel ID="NodesGridTabPanel2" runat="server" Header="false" StripeRows="true" Title="遥信量表" AutoExpandColumn="NodeName" Icon="ApplicationViewColumns" TrackMouseOver="true" Layout="FitLayout" CloseAction="Hide">
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh2" runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="NodesGridStore2" runat="server">
                                <Reader>
                                    <ext:JsonReader IDProperty="NodeID">
                                        <Fields>
                                            <ext:RecordField Name="LscID" Type="Int" />
                                            <ext:RecordField Name="DevID" Type="Int" />
                                            <ext:RecordField Name="DotID" Type="Int" />
                                            <ext:RecordField Name="NodeID" Type="Int" />
                                            <ext:RecordField Name="NodeName" Type="String" />
                                            <ext:RecordField Name="NodeType" Type="Int" />
                                            <ext:RecordField Name="TypeName" Type="String" />
                                            <ext:RecordField Name="Value" Type="Float" />
                                            <ext:RecordField Name="ValueName" Type="String" />
                                            <ext:RecordField Name="Datetime" Type="String" />
                                            <ext:RecordField Name="Status" Type="Int" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="NodesGridColumnModel2" runat="server">
                            <Columns>
                                <ext:Column Header="序号" DataIndex="DotID" Align="Left" Groupable="false" />
                                <ext:Column Header="类型" DataIndex="TypeName" Align="Center" Groupable="false" />
                                <ext:Column Header="测点名称" DataIndex="NodeName" Align="Left" Groupable="false" />
                                <ext:Column Header="监测值" DataIndex="ValueName" Align="Left" Groupable="false" />
                                <ext:Column Header="告警时间" DataIndex="Datetime" Align="Center" Groupable="false" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="NodesGridRowSelectionModel2" runat="server" />
                        </SelectionModel>
                        <View>
                            <ext:GridView ID="NodesGridView2" runat="server" ForceFit="true">
                                <GetRowClass Fn="NavDev.SetGridNodesRowColor" />
                            </ext:GridView>
                        </View>
                        <Listeners>
                            <Activate Fn="NavDev.OnNodesGridTabPanelActivate" Scope="NavDev" />
                            <RowContextMenu Fn="NavDev.OnNodesGridRowContextMenuShow" Scope="NavDev" />
                        </Listeners>
                    </ext:GridPanel>
                    <ext:GridPanel ID="NodesGridTabPanel3" runat="server" Header="false" StripeRows="true" Title="遥控量表" AutoExpandColumn="NodeName" Icon="ApplicationViewColumns" TrackMouseOver="true" Layout="FitLayout" CloseAction="Hide">
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh3" runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="NodesGridStore3" runat="server">
                                <Reader>
                                    <ext:JsonReader IDProperty="NodeID">
                                        <Fields>
                                            <ext:RecordField Name="LscID" Type="Int" />
                                            <ext:RecordField Name="DevID" Type="Int" />
                                            <ext:RecordField Name="DotID" Type="Int" />
                                            <ext:RecordField Name="NodeID" Type="Int" />
                                            <ext:RecordField Name="NodeName" Type="String" />
                                            <ext:RecordField Name="NodeType" Type="Int" />
                                            <ext:RecordField Name="TypeName" Type="String" />
                                            <ext:RecordField Name="Value" Type="Float" />
                                            <ext:RecordField Name="ValueName" Type="String" />
                                            <ext:RecordField Name="Datetime" Type="String" />
                                            <ext:RecordField Name="Status" Type="Int" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="NodesGridColumnModel3" runat="server">
                            <Columns>
                                <ext:Column Header="序号" DataIndex="DotID" Align="Left" Groupable="false" />
                                <ext:Column Header="类型" DataIndex="TypeName" Align="Center" Groupable="false" />
                                <ext:Column Header="测点名称" DataIndex="NodeName" Align="Left" Groupable="false" />
                                <ext:Column Header="监测值" DataIndex="ValueName" Align="Left" Groupable="false" />
                                <ext:ImageCommandColumn Header="操作" Align="Center">
                                    <Commands>
                                        <ext:ImageCommand CommandName="Ctrl" Icon="CogEdit" Text="控制">
                                            <ToolTip Text="遥控参数值设置" />
                                        </ext:ImageCommand>
                                    </Commands>
                                </ext:ImageCommandColumn>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="NodesGridRowSelectionModel3" runat="server" />
                        </SelectionModel>
                        <View>
                            <ext:GridView ID="NodesGridView3" runat="server" ForceFit="true">
                                <GetRowClass Fn="NavDev.SetGridNodesRowColor" />
                            </ext:GridView>
                        </View>
                        <Listeners>
                            <Activate Fn="NavDev.OnNodesGridTabPanelActivate" Scope="NavDev" />
                            <Command Fn="NavDev.OnNodesGridTabPanelCmdClick" Scope="NavDev"/>
                            <RowContextMenu Fn="NavDev.OnNodesGridRowContextMenuShow" Scope="NavDev" />
                        </Listeners>
                    </ext:GridPanel>
                    <ext:GridPanel ID="NodesGridTabPanel4" runat="server" Header="false" StripeRows="true" Title="遥测量表" AutoExpandColumn="NodeName" Icon="ApplicationViewColumns" TrackMouseOver="true" Layout="FitLayout" CloseAction="Hide">
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh4" runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="NodesGridStore4" runat="server">
                                <Reader>
                                    <ext:JsonReader IDProperty="NodeID">
                                        <Fields>
                                            <ext:RecordField Name="LscID" Type="Int" />
                                            <ext:RecordField Name="DevID" Type="Int" />
                                            <ext:RecordField Name="DotID" Type="Int" />
                                            <ext:RecordField Name="NodeID" Type="Int" />
                                            <ext:RecordField Name="NodeName" Type="String" />
                                            <ext:RecordField Name="NodeType" Type="Int" />
                                            <ext:RecordField Name="TypeName" Type="String" />
                                            <ext:RecordField Name="Value" Type="Float" />
                                            <ext:RecordField Name="ValueName" Type="String" />
                                            <ext:RecordField Name="Datetime" Type="String" />
                                            <ext:RecordField Name="Status" Type="Int" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="NodesGridColumnModel4" runat="server">
                            <Columns>
                                <ext:Column Header="序号" DataIndex="DotID" Align="Left" Groupable="false" />
                                <ext:Column Header="类型" DataIndex="TypeName" Align="Center" Groupable="false" />
                                <ext:Column Header="测点名称" DataIndex="NodeName" Align="Left" Groupable="false" />
                                <ext:Column Header="监测值" DataIndex="ValueName" Align="Left" Groupable="false" />
                                <ext:Column Header="告警时间" DataIndex="Datetime" Align="Center" Groupable="false" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="NodesGridRowSelectionModel4" runat="server" />
                        </SelectionModel>
                        <View>
                            <ext:GridView ID="NodesGridView4" runat="server" ForceFit="true">
                                <GetRowClass Fn="NavDev.SetGridNodesRowColor" />
                            </ext:GridView>
                        </View>
                        <Listeners>
                            <Activate Fn="NavDev.OnNodesGridTabPanelActivate" Scope="NavDev" />
                            <RowContextMenu Fn="NavDev.OnNodesGridRowContextMenuShow" Scope="NavDev" />
                        </Listeners>
                    </ext:GridPanel>
                    <ext:GridPanel ID="NodesGridTabPanel5" runat="server" Header="false" StripeRows="true" Title="遥调量表" AutoExpandColumn="NodeName" Icon="ApplicationViewColumns" TrackMouseOver="true" Layout="FitLayout" CloseAction="Hide">
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh5" runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="NodesGridStore5" runat="server">
                                <Reader>
                                    <ext:JsonReader IDProperty="NodeID">
                                        <Fields>
                                            <ext:RecordField Name="LscID" Type="Int" />
                                            <ext:RecordField Name="DevID" Type="Int" />
                                            <ext:RecordField Name="DotID" Type="Int" />
                                            <ext:RecordField Name="NodeID" Type="Int" />
                                            <ext:RecordField Name="NodeName" Type="String" />
                                            <ext:RecordField Name="NodeType" Type="Int" />
                                            <ext:RecordField Name="TypeName" Type="String" />
                                            <ext:RecordField Name="Value" Type="Float" />
                                            <ext:RecordField Name="ValueName" Type="String" />
                                            <ext:RecordField Name="Datetime" Type="String" />
                                            <ext:RecordField Name="Status" Type="Int" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="NodesGridColumnModel5" runat="server">
                            <Columns>
                                <ext:Column Header="序号" DataIndex="DotID" Align="Left" Groupable="false" />
                                <ext:Column Header="类型" DataIndex="TypeName" Align="Center" Groupable="false" />
                                <ext:Column Header="测点名称" DataIndex="NodeName" Align="Left" Groupable="false" />
                                <ext:Column Header="监测值" DataIndex="ValueName" Align="Left" Groupable="false" />
                                <ext:ImageCommandColumn Header="操作" Align="Center">
                                    <Commands>
                                        <ext:ImageCommand CommandName="Set" Icon="CogEdit" Text="设置">
                                            <ToolTip Text="遥调参数值设置" />
                                        </ext:ImageCommand>
                                    </Commands>
                                </ext:ImageCommandColumn>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="NodesGridRowSelectionModel5" runat="server" />
                        </SelectionModel>
                        <View>
                            <ext:GridView ID="NodesGridView5" runat="server" ForceFit="true">
                                <GetRowClass Fn="NavDev.SetGridNodesRowColor" />
                            </ext:GridView>
                        </View>
                        <Listeners>
                            <Activate Fn="NavDev.OnNodesGridTabPanelActivate" Scope="NavDev" />
                            <Command Fn="NavDev.OnNodesGridTabPanelCmdClick" Scope="NavDev"/>
                            <RowContextMenu Fn="NavDev.OnNodesGridRowContextMenuShow" Scope="NavDev" />
                        </Listeners>
                    </ext:GridPanel>
                </Items>
            </ext:TabPanel>
        </Items>
    </ext:Viewport>
    <ext:Window ID="ControlWindow" runat="server" Icon="CogEdit" Padding="5" Height="150" Width="300" Collapsible="False" Modal="true" InitCenter="true" Hidden="true">
        <Items>
            <ext:RadioGroup ID="ControlRadioGroup" runat="server" ColumnsNumber="1">
                <Items>
                    <ext:Radio ID="Radio1" runat="server" BoxLabel="常开控制(0)" InputValue="0" Checked="true" />
                    <ext:Radio ID="Radio2" runat="server" BoxLabel="常闭控制(1)" InputValue="1" />
                    <ext:Radio ID="Radio3" runat="server" BoxLabel="脉冲控制(2)" InputValue="2" />
                </Items>
            </ext:RadioGroup>
            <ext:Hidden ID="ControlWindowHF" runat="server" />
        </Items>
        <Buttons>
            <ext:Button ID="ControlDoButton" runat="server" Text="确定">
                <DirectEvents>
                    <Click OnEvent="ControlDoButton_Click" />
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="ControlCancelButton" runat="server" Text="取消">
                <Listeners>
                    <Click Handler="#{ControlWindow}.hide();#{ControlRadioGroup}.setValue('Radio1', true);" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window ID="SettingWindow" runat="server" Icon="TableEdit" Padding="5" Height="150" Width="300" Collapsible="False" Modal="true" InitCenter="true" Hidden="true">
        <Content>
            <ext:SpinnerField ID="SettingSpinnerField" runat="server" FieldLabel="模拟量输出值" Number="0.000" AllowDecimals="true" DecimalPrecision="3" IncrementValue="1" />
            <ext:Hidden ID="SettingWindowHF" runat="server" />
        </Content>
        <Buttons>
            <ext:Button ID="SettingDoButton" runat="server" Text="确定">
                <DirectEvents>
                    <Click OnEvent="SettingDoButton_Click" />
                </DirectEvents>
            </ext:Button>
            <ext:Button ID="SettingCancelButton" runat="server" Text="取消">
                <Listeners>
                    <Click Handler="#{SettingWindow}.hide();#{SettingSpinnerField}.setValue(0);" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Menu ID="NodesGridRowMenu" runat="server">
        <Items>
            <ext:MenuItem ID="NodesGridRowItem1" runat="server" Text="刷新列表" Icon="TableRefresh">
                <Listeners>
                    <Click Handler="NavDev.ReLoadNodesGrid();" Scope="NavDev" />
                </Listeners>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    <ext:TaskManager ID="MainTaskManager" runat="server">
        <Tasks>
            <ext:Task AutoRun="true" Interval="5000">
                <Listeners>
                    <Update Handler="NavDev.ReLoadNodesGrid();" />
                </Listeners>
            </ext:Task>
        </Tasks>
    </ext:TaskManager>
    </form>
</body>
</html>
