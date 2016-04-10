<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Main.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        Ext.form.ComboBox.override({
            getSelectedIndex: function() {
                var r = this.findRecord(this.valueField, this.getValue());
                return (!Ext.isEmpty(r)) ? this.indexOfEx(r) : -1;
            }
        });

        var showNavMenu = function (node, e) {
            var menu = TreeContextMenu;
            if (node.browserEvent) {
                this.menuNode = this.getRootNode();
                this.getSelectionModel().clearSelections();
                e = node;
            } else {
                this.menuNode = node;
                node.select();
            }

            menu.showAt(e.getXY());
            e.stopEvent();
        };

        var expandDepth = function (tree, depth, node) {
            node = node || tree.getRootNode();
            if (node.getDepth() > depth) { return; }
            if (node.hasChildNodes()) {
                node.expand(false);
                node.eachChild(function (n) {
                    if (n.getDepth() > depth) { return; }
                    n.expand(false, false, expandDepth.createDelegate(this, [tree, depth, n]));
                });
            }
        };

        var collapseDepth = function (tree, depth) {
            var node = tree.getRootNode();
            if (node.hasChildNodes()) {
                node.eachChild(function (c) {
                    c.cascade(function (n) {
                        if (n.getDepth() === depth) {
                            n.collapse(true);
                        }
                    });
                });
            }
        };
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ext:BorderLayout ID="BorderLayout" runat="server">
        <Items>
            <ext:Panel ID="SpeechPanel" runat="server" Header="false" Height="100" HideMode="Visibility" Hidden="true" Split="true" CollapseMode="Mini" Region="North">
                <AutoLoad Url="Speaker.aspx" Mode="IFrame" />
            </ext:Panel>
            <ext:TabPanel ID="NavPanel" runat="server" Header="false" Margins="0 0 0 4" EnableTabScroll="true" TabPosition="Bottom" MinTabWidth="50" Split="true" CollapseMode="Mini" Region="West">
                <Items>
                    <ext:TreePanel ID="NavTreePanel" runat="server" Border="false" Header="false" Title="综合群组"
                        Lines="true" CollapseFirst="false" ContainerScroll="true" AutoScroll="true">
                        <TopBar>
                            <ext:Toolbar ID="FindTreeNodeToolBar" runat="server">
                                <Items>
                                    <ext:TextField ID="TreeTriggerField" runat="server" Width="200" EmptyText="输入局站名称...">
                                        <Listeners>
                                            <Change Fn="treeFilterChange" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5" />
                                    <ext:Button ID="btnTreeSearch" runat="server" Icon="Magnifier" ToolTip="搜索局站">
                                        <Listeners>
                                            <Click Fn="navFilterSearch" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarFill ID="TreeToolbarFill" runat="server" />
                                    <ext:Button ID="TreeMenuButton" runat="server" Icon="Cog" ToolTip="选项" Hidden="true">
                                        <Menu>
                                            <ext:Menu ID="TreeMenu" runat="server">
                                                <Items>
                                                    <ext:MenuItem ID="TreeMenuItem1" runat="server" Text="展开所有" IconCls="icon-expand-all">
                                                        <Listeners>
                                                            <Click Handler="#{NavTreePanel}.expandAll();" />
                                                        </Listeners>
                                                    </ext:MenuItem>
                                                    <ext:MenuSeparator ID="TreeMenuSeparator1" runat="server" />
                                                    <ext:MenuItem ID="TreeMenuItem2" runat="server" Text="折叠所有" IconCls="icon-collapse-all">
                                                        <Listeners>
                                                            <Click Handler="#{NavTreePanel}.collapseAll();" />
                                                        </Listeners>
                                                    </ext:MenuItem>
                                                </Items>
                                            </ext:Menu>
                                        </Menu>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Loader>
                            <ext:PageTreeLoader OnNodeLoad="NavTreeNodesLoaded" />
                        </Loader>
                        <Listeners>
                            <Activate Fn="activeTreeTab" />
                            <Click Fn="onTreeItemClick" />
                            <ContextMenu Fn="showNavMenu" StopEvent="true" />
                        </Listeners>
                    </ext:TreePanel>
                    <ext:TreePanel ID="UserDefindTreePanel" runat="server" Border="false" Header="false"
                        Title="自定义群组" Lines="true" CollapseFirst="false" ContainerScroll="true" AutoScroll="true">
                        <TopBar>
                            <ext:Toolbar ID="FindUserDefindTreeNodeToolBar" runat="server">
                                <Items>
                                    <ext:TextField ID="UserDefindTriggerField" runat="server" Width="200" EmptyText="输入局站名称...">
                                        <Listeners>
                                            <Change Fn="treeFilterChange" />
                                        </Listeners>
                                    </ext:TextField>
                                    <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="5" />
                                    <ext:Button ID="btnUserDefindSearch" runat="server" Icon="Magnifier" ToolTip="搜索局站">
                                        <Listeners>
                                            <Click Fn="udFilterSearch" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarFill ID="UserDefindToolbarFill" runat="server" />
                                    <ext:Button ID="UserDefindMenuButton" runat="server" Icon="Cog" ToolTip="选项" Hidden="true">
                                        <Menu>
                                            <ext:Menu ID="UserDefindMenu" runat="server">
                                                <Items>
                                                    <ext:MenuItem ID="UserDefindMenuItem1" runat="server" Text="展开所有" IconCls="icon-expand-all">
                                                        <Listeners>
                                                            <Click Handler="#{UserDefindTreePanel}.expandAll();" />
                                                        </Listeners>
                                                    </ext:MenuItem>
                                                    <ext:MenuSeparator ID="UserDefindMenuSeparator1" runat="server" />
                                                    <ext:MenuItem ID="UserDefindMenuItem2" runat="server" Text="折叠所有" IconCls="icon-collapse-all">
                                                        <Listeners>
                                                            <Click Handler="#{UserDefindTreePanel}.collapseAll();" />
                                                        </Listeners>
                                                    </ext:MenuItem>
                                                </Items>
                                            </ext:Menu>
                                        </Menu>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Loader>
                            <ext:PageTreeLoader OnNodeLoad="UserDefindTreeNodesLoaded" />
                        </Loader>
                        <Listeners>
                            <Activate Fn="activeTreeTab" />
                            <Click Fn="onTreeItemClick" />
                            <ContextMenu Fn="onUserDefindTreePanelMenuShow" />
                        </Listeners>
                    </ext:TreePanel>
                </Items>
                <Listeners>
                    <BeforeRender Fn="setNavPanelWidth" />
                    <Collapse Handler="setCollapseChecked(#{ViewMenuItem1});" />
                    <Expand Handler="setExpandChecked(#{ViewMenuItem1});" />
                </Listeners>
            </ext:TabPanel>
            <ext:TabPanel ID="NodesTabPanel" runat="server" Margins="0 4 0 0" MinTabWidth="50" EnableTabScroll="true" Region="Center">
                <Items>
                    <ext:GridPanel ID="NodesGridTabPanel1" runat="server" Header="false" StripeRows="true"
                        Title="测点列表" AutoExpandColumn="NodeName" Icon="ApplicationViewColumns" TrackMouseOver="true"
                        Layout="FitLayout" CloseAction="Hide">
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh1" runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="NodesGridStore1" runat="server" GroupField="TypeName" OnSubmitData="NodesGridStore_Submit">
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
                                            <ext:RecordField Name="CamUrl" Type="String" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                                <WriteBaseParams>
                                    <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{NodesGridTabPanel1})" />
                                    <ext:Parameter Name="ExportTitle" Value="getNodesExportTitle(#{NodesGridTabPanel1})" Mode="Raw" />
                                </WriteBaseParams>
                                <DirectEventConfig IsUpload="true" />
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="NodesGridColumnModel1" runat="server">
                            <Columns>
                                <ext:Column Header="序号" DataIndex="DotID" Align="Left" Groupable="false" Width="60" />
                                <ext:Column Header="类型" DataIndex="TypeName" Align="Center" Groupable="false" Width="60" />
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
                                        <ext:ImageCommand CommandName="Cam" Icon="Webcam" Text="视频">
                                            <ToolTip Text="远程视频" />
                                        </ext:ImageCommand>
                                    </Commands>
                                    <PrepareCommand Fn="prepareCommand" />
                                </ext:ImageCommandColumn>
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="NodesGridRowSelectionModel1" runat="server" />
                        </SelectionModel>
                        <View>
                            <ext:GroupingView ID="NodesGridGroupingView1" runat="server" EnableRowBody="true" EnableGroupingMenu="false" ForceFit="true" GroupTextTpl="{text} ({[values.rs.length]} 条)">
                                <GetRowClass Fn="setNodesColor" />
                            </ext:GroupingView>
                        </View>
                        <Listeners>
                            <Activate Fn="onNodesGridTabPanelActivate" />
                            <Command Fn="onNodesGridTabPanelCmdClick" />
                            <RowContextMenu Fn="onNodesGridRowContextMenuShow" />
                        </Listeners>
                    </ext:GridPanel>
                    <ext:GridPanel ID="NodesGridTabPanel2" runat="server" Header="false" StripeRows="true"
                        Title="遥信量表" AutoExpandColumn="NodeName" Icon="ApplicationViewColumns" TrackMouseOver="true"
                        Layout="FitLayout" CloseAction="Hide">
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh2"
                                runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="NodesGridStore2" runat="server" OnSubmitData="NodesGridStore_Submit">
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
                                            <ext:RecordField Name="CamUrl" Type="String" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                                <WriteBaseParams>
                                    <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{NodesGridTabPanel2})" />
                                    <ext:Parameter Name="ExportTitle" Value="getNodesExportTitle(#{NodesGridTabPanel2})"
                                        Mode="Raw" />
                                </WriteBaseParams>
                                <DirectEventConfig IsUpload="true" />
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="NodesGridColumnModel2" runat="server">
                            <Columns>
                                <ext:Column Header="序号" DataIndex="DotID" Align="Left" Groupable="false" Width="60" />
                                <ext:Column Header="类型" DataIndex="TypeName" Align="Center" Groupable="false" Width="60" />
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
                                <GetRowClass Fn="setNodesColor" />
                            </ext:GridView>
                        </View>
                        <Listeners>
                            <Activate Fn="onNodesGridTabPanelActivate" />
                            <RowContextMenu Fn="onNodesGridRowContextMenuShow" />
                        </Listeners>
                    </ext:GridPanel>
                    <ext:GridPanel ID="NodesGridTabPanel3" runat="server" Header="false" StripeRows="true"
                        Title="遥控量表" AutoExpandColumn="NodeName" Icon="ApplicationViewColumns" TrackMouseOver="true"
                        Layout="FitLayout" CloseAction="Hide">
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh3"
                                runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="NodesGridStore3" runat="server" OnSubmitData="NodesGridStore_Submit">
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
                                            <ext:RecordField Name="CamUrl" Type="String" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                                <WriteBaseParams>
                                    <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{NodesGridTabPanel3})" />
                                    <ext:Parameter Name="ExportTitle" Value="getNodesExportTitle(#{NodesGridTabPanel3})"
                                        Mode="Raw" />
                                </WriteBaseParams>
                                <DirectEventConfig IsUpload="true" />
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="NodesGridColumnModel3" runat="server">
                            <Columns>
                                <ext:Column Header="序号" DataIndex="DotID" Align="Left" Groupable="false" Width="60" />
                                <ext:Column Header="类型" DataIndex="TypeName" Align="Center" Groupable="false" Width="60" />
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
                                <GetRowClass Fn="setNodesColor" />
                            </ext:GridView>
                        </View>
                        <Listeners>
                            <Activate Fn="onNodesGridTabPanelActivate" />
                            <Command Fn="onNodesGridTabPanelCmdClick" />
                            <RowContextMenu Fn="onNodesGridRowContextMenuShow" />
                        </Listeners>
                    </ext:GridPanel>
                    <ext:GridPanel ID="NodesGridTabPanel4" runat="server" Header="false" StripeRows="true"
                        Title="遥测量表" AutoExpandColumn="NodeName" Icon="ApplicationViewColumns" TrackMouseOver="true"
                        Layout="FitLayout" CloseAction="Hide">
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh4"
                                runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="NodesGridStore4" runat="server" OnSubmitData="NodesGridStore_Submit">
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
                                            <ext:RecordField Name="CamUrl" Type="String" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                                <WriteBaseParams>
                                    <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{NodesGridTabPanel4})" />
                                    <ext:Parameter Name="ExportTitle" Value="getNodesExportTitle(#{NodesGridTabPanel4})"
                                        Mode="Raw" />
                                </WriteBaseParams>
                                <DirectEventConfig IsUpload="true" />
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="NodesGridColumnModel4" runat="server">
                            <Columns>
                                <ext:Column Header="序号" DataIndex="DotID" Align="Left" Groupable="false" Width="60" />
                                <ext:Column Header="类型" DataIndex="TypeName" Align="Center" Groupable="false" Width="60" />
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
                                <GetRowClass Fn="setNodesColor" />
                            </ext:GridView>
                        </View>
                        <Listeners>
                            <Activate Fn="onNodesGridTabPanelActivate" />
                            <RowContextMenu Fn="onNodesGridRowContextMenuShow" />
                        </Listeners>
                    </ext:GridPanel>
                    <ext:GridPanel ID="NodesGridTabPanel5" runat="server" Header="false" StripeRows="true"
                        Title="遥调量表" AutoExpandColumn="NodeName" Icon="ApplicationViewColumns" TrackMouseOver="true"
                        Layout="FitLayout" CloseAction="Hide">
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh5"
                                runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="NodesGridStore5" runat="server" OnSubmitData="NodesGridStore_Submit">
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
                                            <ext:RecordField Name="CamUrl" Type="String" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                                <WriteBaseParams>
                                    <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{NodesGridTabPanel5})" />
                                    <ext:Parameter Name="ExportTitle" Value="getNodesExportTitle(#{NodesGridTabPanel5})"
                                        Mode="Raw" />
                                </WriteBaseParams>
                                <DirectEventConfig IsUpload="true" />
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="NodesGridColumnModel5" runat="server">
                            <Columns>
                                <ext:Column Header="序号" DataIndex="DotID" Align="Left" Groupable="false" Width="60" />
                                <ext:Column Header="类型" DataIndex="TypeName" Align="Center" Groupable="false" Width="60" />
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
                                <GetRowClass Fn="setNodesColor" />
                            </ext:GridView>
                        </View>
                        <Listeners>
                            <Activate Fn="onNodesGridTabPanelActivate" />
                            <Command Fn="onNodesGridTabPanelCmdClick" />
                            <RowContextMenu Fn="onNodesGridRowContextMenuShow" />
                        </Listeners>
                    </ext:GridPanel>
                    <ext:GridPanel ID="NodesGridTabPanel6" runat="server" Header="false" StripeRows="true"
                        Title="归类统计" Icon="ApplicationViewColumns" Layout="FitLayout" CloseAction="Hide">
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh6"
                                runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="NodesGridStore6" runat="server" IgnoreExtraFields="false" OnSubmitData="NodesGridStore6_Submit">
                                <Reader>
                                    <ext:JsonReader />
                                </Reader>
                                <WriteBaseParams>
                                    <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{NodesGridTabPanel6})" />
                                    <ext:Parameter Name="NodeName" Mode="Raw" Value="getCurNodeName()" />
                                </WriteBaseParams>
                                <DirectEventConfig IsUpload="true" />
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="NodesGridColumnModel6" runat="server">
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CellSelectionModel ID="NodesGridRowSelectionModel6" runat="server" />
                        </SelectionModel>
                        <View>
                            <ext:LockingGridView ID="NodesGridView6" runat="server" ForceFit="false">
                            </ext:LockingGridView>
                        </View>
                        <Listeners>
                            <Activate Fn="onNodesGridTabPanelActivate" />
                            <CellContextMenu Fn="onACSGridCellContextMenuShow" />
                            <CellDblClick Fn="onACSGridCellDblClick" />
                        </Listeners>
                    </ext:GridPanel>
                    <ext:GridPanel ID="NodesGridTabPanel7" runat="server" Header="false" StripeRows="true"
                        Title="综合测值" Icon="ApplicationViewColumns" AutoScroll="true" Layout="FitLayout"
                        CloseAction="Hide">
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh7"
                                runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="NodesGridStore7" runat="server" IgnoreExtraFields="false" AutoLoad="false"
                                OnRefreshData="NodesGridStore7_Refresh">
                                <Proxy>
                                    <ext:PageProxy />
                                </Proxy>
                                <Reader>
                                    <ext:JsonReader />
                                </Reader>
                                <BaseParams>
                                    <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                    <ext:Parameter Name="limit" Value="20" Mode="Raw" />
                                    <ext:Parameter Name="NodeId" Value="getCurNodeId()" Mode="Raw" />
                                    <ext:Parameter Name="NodeType" Value="getCurNodeType()" Mode="Raw" />
                                    <ext:Parameter Name="NodeName" Value="getCurNodeName()" Mode="Raw" />
                                </BaseParams>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="NodesGridColumnModel7" runat="server">
                        </ColumnModel>
                        <SelectionModel>
                            <ext:CellSelectionModel ID="NodesGridRowSelectionModel7" runat="server" />
                        </SelectionModel>
                        <View>
                            <ext:LockingGridView ID="NodesGridView7" runat="server" ForceFit="false">
                            </ext:LockingGridView>
                        </View>
                        <Listeners>
                            <Activate Fn="onNodesGridTabPanelActivate" />
                            <CellContextMenu Fn="onACVGridCellContextMenuShow" />
                            <CellDblClick Fn="onACVGridCellDblClick" />
                        </Listeners>
                        <BottomBar>
                            <ext:PagingToolbar ID="NodesGridTabPanel7PagingToolbar" runat="server" PageSize="20">
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                    <ext:Panel ID="NodesGridTabPanel8" runat="server" Header="false" Title="组态界面" Icon="ShapeUngroup"
                        Layout="FitLayout" CloseAction="Hide">
                    </ext:Panel>
                </Items>
            </ext:TabPanel>
            <ext:Panel ID="AlarmsPanel" runat="server" Margins="0 4 4 4" Height="200" Title="活动告警列表" Icon="ApplicationViewColumns" Split="true" CollapseMode="Mini" Layout="BorderLayout" Region="South">
                <Items>
                    <ext:GridPanel ID="AlarmsGridPanel" runat="server" Header="false" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Border="false" Region="Center">
                        <TopBar>
                            <ext:Toolbar ID="AlarmsToolbars" runat="server" Layout="ContainerLayout" HideMode="Display">
                                <Items>
                                    <ext:Toolbar ID="AlarmsToolbar1" runat="server" Flat="true">
                                        <Items>
                                            <ext:ComboBox ID="LscsComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="Lsc名称" DisplayField="Name" ValueField="Id" Editable="true" ForceSelection="false" SelectOnFocus="true" Mode="Local" TriggerAction="All" EmptyText="..." Resizable="true">
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
                                                            <Load Handler="#{LscsComboBox}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="delete #{AlarmsToolbars}.loaded1;#{Area2ComboBox}.clearValue();#{Area2ComboBox}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="Area2ComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="地区名称" DisplayField="Name" ValueField="Id" Editable="true" ForceSelection="false" SelectOnFocus="true" Mode="Local" TriggerAction="All" EmptyText="..." Resizable="true">
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
                                                    <Select Handler="delete #{AlarmsToolbars}.loaded1;#{Area3ComboBox}.clearValue(); #{Area3ComboBox}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="Area3ComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="县市名称" DisplayField="Name" ValueField="Id" Editable="true" ForceSelection="false" SelectOnFocus="true" Mode="Local" TriggerAction="All" EmptyText="..." Resizable="true">
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
                                                <Listeners>
                                                    <Select Handler="delete #{AlarmsToolbars}.loaded1;#{StaComboBox}.clearValue(); #{StaComboBox}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="StaComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="局站名称" DisplayField="Name" ValueField="Id" Editable="true" ForceSelection="false" SelectOnFocus="true" Mode="Local" TriggerAction="All" EmptyText="..." Resizable="true">
                                                <Store>
                                                    <ext:Store ID="StaStore" runat="server" AutoLoad="false" OnRefreshData="OnStaRefresh">
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
                                                            <Load Handler="#{StaComboBox}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="delete #{AlarmsToolbars}.loaded1;#{DevComboBox}.clearValue(); #{DevComboBox}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="DevComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="设备名称" DisplayField="Name" ValueField="Id" Editable="true" ForceSelection="false" SelectOnFocus="true" Mode="Local" TriggerAction="All" EmptyText="..." Resizable="true">
                                                <Store>
                                                    <ext:Store ID="DevStore" runat="server" AutoLoad="false" OnRefreshData="OnDevRefresh">
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
                                                            <Load Handler="#{DevComboBox}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="delete #{AlarmsToolbars}.loaded1;#{NodeComboBox}.clearValue(); #{NodeComboBox}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer3" runat="server" Width="5" />
                                            <ext:Button ID="SetConditionBtn" runat="server" Text="筛选告警" StandOut="true" IconCls="icon-filter" IconAlign="Right">
                                                <DirectEvents>
                                                    <Click OnEvent="SetConditionBtn_Click" ViewStateMode="Enabled" Before="if(!#{AlarmsToolbars}.loaded1 || !#{AlarmsToolbars}.loaded2 || !#{AlarmsToolbars}.loaded3) {return false;}" Success="#{AlarmsGridPagingToolbar}.doLoad();">
                                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{AlarmsGridPanel}.body.up('div')" />
                                                    </Click>
                                                </DirectEvents>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                    <ext:Toolbar ID="AlarmsToolbar2" runat="server" Flat="true">
                                        <Items>
                                            <ext:ComboBox ID="NodeComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="测点名称" DisplayField="Name" ValueField="Id" Editable="true" ForceSelection="false" SelectOnFocus="true" Mode="Local" TriggerAction="All" EmptyText="..." Resizable="true">
                                                <Store>
                                                    <ext:Store ID="NodeStore" runat="server" AutoLoad="false" OnRefreshData="OnNodeRefresh">
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
                                                            <Load Handler="#{NodeComboBox}.setValueAndFireSelect(this.getAt(0).get('Id'));#{AlarmsToolbars}.loaded1=true;" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="AlarmDevComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="告警设备" DisplayField="Name" ValueField="Id" Editable="true" ForceSelection="false" SelectOnFocus="true" Mode="Local" TriggerAction="All" EmptyText="..." Resizable="true">
                                                <Store>
                                                    <ext:Store ID="AlarmDevStore" runat="server" AutoLoad="true" OnRefreshData="OnAlarmDevRefresh">
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
                                                            <Load Handler="#{AlarmDevComboBox}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="delete #{AlarmsToolbars}.loaded2;#{AlarmLogicComboBox}.clearValue(); #{AlarmLogicComboBox}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="AlarmLogicComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="告警逻辑" DisplayField="Name" ValueField="Id" Editable="true" ForceSelection="false" SelectOnFocus="true" Mode="Local" TriggerAction="All" EmptyText="..." Resizable="true">
                                                <Store>
                                                    <ext:Store ID="AlarmLogicStore" runat="server" AutoLoad="false" OnRefreshData="OnAlarmLogicRefresh">
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
                                                            <Load Handler="#{AlarmLogicComboBox}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                                <Listeners>
                                                    <Select Handler="delete #{AlarmsToolbars}.loaded2;#{AlarmNameComboBox}.clearValue(); #{AlarmNameComboBox}.store.reload();" />
                                                </Listeners>
                                            </ext:ComboBox>
                                            <ext:ComboBox ID="AlarmNameComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="告警名称" DisplayField="Name" ValueField="Id" Editable="true" ForceSelection="false" SelectOnFocus="true" Mode="Local" TriggerAction="All" EmptyText="..." Resizable="true">
                                                <Store>
                                                    <ext:Store ID="AlarmNameStore" runat="server" AutoLoad="false" OnRefreshData="OnAlarmNameRefresh">
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
                                                            <Load Handler="#{AlarmNameComboBox}.setValueAndFireSelect(this.getAt(0).get('Id'));#{AlarmsToolbars}.loaded2=true;" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                            </ext:ComboBox>
                                            <ext:MultiCombo ID="AlarmLevelMultiCombo" runat="server" Width="180" LabelWidth="50" FieldLabel="告警级别" DisplayField="Name" ValueField="Id" SelectionMode="All" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." Resizable="true">
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
                                                            <Load Handler="#{AlarmLevelMultiCombo}.selectAll();#{AlarmsToolbars}.loaded3=true;" />
                                                        </Listeners>
                                                    </ext:Store>
                                                </Store>
                                            </ext:MultiCombo>
                                            <ext:ToolbarSpacer ID="ToolbarSpacer4" runat="server" Width="5" />
                                            <ext:SplitButton ID="OtherOptionsButton" runat="server" Text="其他选项" StandOut="true">
                                                <Menu>
                                                    <ext:Menu ID="OtherOptionsMenu" runat="server">
                                                        <Items>
                                                            <ext:CheckMenuItem ID="OtherOptionsMenuItem1" runat="server" Text="仅显示未确认告警" HideOnClick="false">
                                                                <Listeners>
                                                                    <CheckChange Handler="if(this.checked){
                                                                        #{OtherOptionsMenuItem2}.setChecked(false);
                                                                    }" />
                                                                </Listeners>
                                                            </ext:CheckMenuItem>
                                                            <ext:CheckMenuItem ID="OtherOptionsMenuItem2" runat="server" Text="仅显示已确认告警" HideOnClick="false">
                                                                <Listeners>
                                                                    <CheckChange Handler="if(this.checked){
                                                                        #{OtherOptionsMenuItem1}.setChecked(false);
                                                                    }" />
                                                                </Listeners>
                                                            </ext:CheckMenuItem>
                                                            <ext:MenuSeparator />
                                                            <ext:CheckMenuItem ID="OtherOptionsMenuItem3" runat="server" Text="仅显示标准化告警" HideOnClick="false">
                                                                <Listeners>
                                                                    <CheckChange Handler="if(this.checked){
                                                                        #{OtherOptionsMenuItem4}.setChecked(false);
                                                                    }" />
                                                                </Listeners>
                                                            </ext:CheckMenuItem>
                                                            <ext:CheckMenuItem ID="OtherOptionsMenuItem4" runat="server" Text="仅显示非标准化告警" HideOnClick="false">
                                                                <Listeners>
                                                                    <CheckChange Handler="if(this.checked){
                                                                        #{OtherOptionsMenuItem3}.setChecked(false);
                                                                    }" />
                                                                </Listeners>
                                                            </ext:CheckMenuItem>
                                                            <ext:MenuSeparator />
                                                            <ext:CheckMenuItem ID="OtherOptionsMenuItem5" runat="server" Text="仅显示非工程告警" HideOnClick="false">
                                                                <Listeners>
                                                                    <CheckChange Handler="if(this.checked){
                                                                        #{OtherOptionsMenuItem6}.setChecked(false);
                                                                    }" />
                                                                </Listeners>
                                                            </ext:CheckMenuItem>
                                                            <ext:CheckMenuItem ID="OtherOptionsMenuItem6" runat="server" Text="仅显示工程告警" HideOnClick="false">
                                                                <Listeners>
                                                                    <CheckChange Handler="if(this.checked){
                                                                        #{OtherOptionsMenuItem5}.setChecked(false);
                                                                    }" />
                                                                </Listeners>
                                                            </ext:CheckMenuItem>
                                                        </Items>
                                                        <Listeners>
                                                            <ItemClick Handler="X.Home.OnOtherOptionsMenuClick(menuItem.text,{success : function(result) { doAlarmsGridLoad(); }});" />
                                                        </Listeners>
                                                    </ext:Menu>
                                                </Menu>
                                                <Listeners>
                                                    <Click Handler="this.menu.show(this.getEl());" />
                                                </Listeners>
                                            </ext:SplitButton>
                                        </Items>
                                    </ext:Toolbar>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh8" runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="AlarmsStore" runat="server" AutoLoad="false" OnRefreshData="OnAlarmsRefresh">
                                <Proxy>
                                    <ext:PageProxy />
                                </Proxy>
                                <Reader>
                                    <ext:JsonReader IDProperty="ID">
                                        <Fields>
                                            <ext:RecordField Name="ID" Type="Int" />
                                            <ext:RecordField Name="LscID" Type="Int" />
                                            <ext:RecordField Name="LscName" Type="String" />
                                            <ext:RecordField Name="SerialNO" Type="Int" />
                                            <ext:RecordField Name="Area1Name" Type="String" />
                                            <ext:RecordField Name="Area2Name" Type="String" />
                                            <ext:RecordField Name="Area3Name" Type="String" />
                                            <ext:RecordField Name="Area4Name" Type="String" />
                                            <ext:RecordField Name="StaName" Type="String" />
                                            <ext:RecordField Name="DevName" Type="String" />
                                            <ext:RecordField Name="DevDesc" Type="String" />
                                            <ext:RecordField Name="NodeID" Type="Int" />
                                            <ext:RecordField Name="NodeType" Type="Int" />
                                            <ext:RecordField Name="NodeName" Type="String" />
                                            <ext:RecordField Name="AlarmID" Type="Int" />
                                            <ext:RecordField Name="AlarmDesc" Type="String" />
                                            <ext:RecordField Name="AlarmDevice" Type="String" />
                                            <ext:RecordField Name="AlarmLogic" Type="String" />
                                            <ext:RecordField Name="AlarmName" Type="String" />
                                            <ext:RecordField Name="AlarmClass" Type="String" />
                                            <ext:RecordField Name="AlarmLevel" Type="Int" />
                                            <ext:RecordField Name="AlarmLevelName" Type="String" />
                                            <ext:RecordField Name="NMAlarmID" Type="String" />
                                            <ext:RecordField Name="StartTime" Type="String" />
                                            <ext:RecordField Name="EndTime" Type="String" />
                                            <ext:RecordField Name="ConfirmMarking" Type="Int" />
                                            <ext:RecordField Name="ConfirmMarkingName" Type="String" />
                                            <ext:RecordField Name="ConfirmTime" Type="String" />
                                            <ext:RecordField Name="ConfirmName" Type="String" />
                                            <ext:RecordField Name="TimeInterval" Type="String" />
                                            <ext:RecordField Name="ProjName" Type="String" />
                                            <ext:RecordField Name="MAlm" Type="String" />
                                            <ext:RecordField Name="Conn" Type="String" />
                                            <ext:RecordField Name="TurnCount" Type="Int" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                                <BaseParams>
                                    <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                    <ext:Parameter Name="limit" Value="#{PageSizeComboBox}.getValue()" Mode="Raw" />
                                </BaseParams>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="AlarmsColumnModel" runat="server">
                            <Columns>
                                <ext:Column Header="告警级别" DataIndex="AlarmLevelName" Align="Center" Locked="true" Width="100" />
                                <ext:Column Header="告警时间" DataIndex="StartTime" Align="Center" Locked="true" Width="150" />
                                <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" />
                                <ext:Column Header="地区名称" DataIndex="Area2Name" Align="Left" />
                                <ext:Column Header="县市名称" DataIndex="Area3Name" Align="Left" />
                                <ext:Column Header="局站名称" DataIndex="StaName" Align="Left" />
                                <ext:Column Header="设备名称" DataIndex="DevName" Align="Left" />
                                <ext:Column Header="设备描述" DataIndex="DevDesc" Align="Left" />
                                <ext:Column Header="测点名称" DataIndex="NodeName" Align="Left" />
                                <ext:Column Header="告警描述" DataIndex="AlarmDesc" Align="Left" />
                                <ext:Column Header="告警设备类型" DataIndex="AlarmDevice" Align="Left" />
                                <ext:Column Header="告警逻辑分类" DataIndex="AlarmLogic" Align="Left" />
                                <ext:Column Header="告警标准名称" DataIndex="AlarmName" Align="Left" />
                                <ext:Column Header="告警类别" DataIndex="AlarmClass" Align="Left" />
                                <ext:Column Header="处理标识" DataIndex="ConfirmMarkingName" Align="Center" />
                                <ext:Column Header="处理时间" DataIndex="ConfirmTime" Align="Center" />
                                <ext:Column Header="处理人员" DataIndex="ConfirmName" Align="Left" />
                                <ext:Column Header="告警历时" DataIndex="TimeInterval" Align="Center" />
                                <ext:Column Header="动环监控告警ID" DataIndex="NMAlarmID" Align="Center" />
                                <ext:Column Header="工程预约" DataIndex="ProjName" Align="Center">
                                    <Renderer Fn="doProjName" />
                                </ext:Column>
                                <ext:Column Header="主告警" DataIndex="MAlm" Align="Center">
                                    <Renderer Fn="doMAlm" />
                                </ext:Column>
                                <ext:Column Header="关联告警" DataIndex="Conn" Align="Center">
                                    <Renderer Fn="doConn" />
                                </ext:Column>
                                <ext:Column Header="翻转次数" DataIndex="TurnCount" Align="Left" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="AlarmsRowSelectionModel" runat="server" SingleSelect="true">
                                <DirectEvents>
                                    <RowSelect OnEvent="OnSubAlarmsRefresh">
                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{SubAlarmsPanel}" />
                                        <ExtraParams>
                                            <ext:Parameter Name="LscID" Value="record.data.LscID" Mode="Raw" />
                                            <ext:Parameter Name="SerialNO" Value="record.data.SerialNO" Mode="Raw" />
                                            <ext:Parameter Name="AlarmID" Value="record.data.AlarmID" Mode="Raw" />
                                            <ext:Parameter Name="ProjName" Value="record.data.ProjName" Mode="Raw" />
                                        </ExtraParams>
                                    </RowSelect>
                                </DirectEvents>
                            </ext:RowSelectionModel>
                        </SelectionModel>
                        <View>
                            <ext:LockingGridView ID="AlarmsGridView" runat="server" ForceFit="false">
                                <GetRowClass Fn="setAlarmsColor" />
                            </ext:LockingGridView>
                        </View>
                        <Listeners>
                            <RowContextMenu Fn="onAlarmsGridRowContextMenuShow" />
                        </Listeners>
                        <BottomBar>
                            <ext:PagingToolbar ID="AlarmsGridPagingToolbar" runat="server" PageSize="20" StoreID="AlarmsStore">
                                <Items>
                                    <ext:Button runat="server" Icon="ArrowOut" ToolTip="全屏显示" NavigateUrl="ActiveAlarms.aspx" Target="_blank" />
                                    <ext:ToolbarSeparator runat="server" />
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
                                            <Select Handler="#{AlarmsGridPagingToolbar}.pageSize = parseInt(this.getValue()); #{AlarmsGridPagingToolbar}.doLoad();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ToolbarSeparator runat="server" />
                                </Items>
                                <Listeners>
                                    <AfterRender Handler="this.items.add(#{PageSizeComboBox});" />
                                </Listeners>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                    <ext:GridPanel ID="SubAlarmsPanel" runat="server" Header="false" Height="50px" CollapseMode="Mini" Collapsible="true" Split="true" Collapsed="true" Border="false" Region="South">
                        <Plugins>
                            <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh9"
                                runat="server" />
                        </Plugins>
                        <Store>
                            <ext:Store ID="SubAlarmsStore" runat="server" AutoLoad="false">
                                <Reader>
                                    <ext:JsonReader IDProperty="AlarmID">
                                        <Fields>
                                            <ext:RecordField Name="SerialNO" Type="Int" />
                                            <ext:RecordField Name="AlarmID" Type="Int" />
                                            <ext:RecordField Name="NMAlarmID" Type="String" />
                                            <ext:RecordField Name="AlarmLogType" Type="String" />
                                            <ext:RecordField Name="SubAlarmLogType" Type="String" />
                                            <ext:RecordField Name="DevEffect" Type="String" />
                                            <ext:RecordField Name="OperEffect" Type="String" />
                                            <ext:RecordField Name="AlarmInterpret" Type="String" />
                                            <ext:RecordField Name="OtherInfo" Type="String" />
                                            <ext:RecordField Name="Correlation" Type="String" />
                                            <ext:RecordField Name="ProjName" Type="String" />
                                        </Fields>
                                    </ext:JsonReader>
                                </Reader>
                            </ext:Store>
                        </Store>
                        <ColumnModel ID="SubAlarmsColumnModel" runat="server">
                            <Columns>
                                <ext:Column Header="序号" DataIndex="SerialNO" Align="Left" />
                                <ext:Column Header="动环监控告警ID" DataIndex="NMAlarmID" Align="Left" />
                                <ext:Column Header="告警逻辑分类" DataIndex="AlarmLogType" Align="Left" />
                                <ext:Column Header="告警逻辑子类" DataIndex="SubAlarmLogType" Align="Left" />
                                <ext:Column Header="该事件对设备影响" DataIndex="DevEffect" Align="Left" />
                                <ext:Column Header="该事件对业务影响" DataIndex="OperEffect" Align="Left" />
                                <ext:Column Header="告警解释" DataIndex="AlarmInterpret" Align="Left">
                                    <Renderer Fn="setAlarmsInterpretTips" />
                                </ext:Column>
                                <ext:Column Header="告警关联标识" DataIndex="Correlation" Align="Center" />
                                <ext:Column Header="其他信息" DataIndex="OtherInfo" Align="Left" />
                                <ext:Column Header="工程标识" DataIndex="ProjName" Align="Center" />
                            </Columns>
                        </ColumnModel>
                        <SelectionModel>
                            <ext:RowSelectionModel ID="SubAlarmsRowSelectionModel" runat="server" SingleSelect="true" />
                        </SelectionModel>
                        <View>
                            <ext:GridView ID="SubAlarmsGridView" runat="server" ForceFit="true" />
                        </View>
                        <Listeners>
                            <Collapse Handler="setCollapseChecked(#{ViewMenuItem3});" />
                            <Expand Handler="setExpandChecked(#{ViewMenuItem3});" />
                        </Listeners>
                    </ext:GridPanel>
                </Items>
                <Listeners>
                    <BeforeRender Fn="setAlarmPanelHeight" />
                    <Collapse Handler="setCollapseChecked(#{ViewMenuItem2});" />
                    <Expand Handler="setExpandChecked(#{ViewMenuItem2});" />
                </Listeners>
            </ext:Panel>
        </Items>
    </ext:BorderLayout>
    <ext:TaskManager ID="HomeTaskManager" runat="server" AutoRunDelay="2000">
        <Tasks>
            <ext:Task AutoRun="true" Interval="15000">
                <Listeners>
                    <Update Handler="doPageRefresh();" />
                </Listeners>
            </ext:Task>
        </Tasks>
    </ext:TaskManager>
    <ext:Menu ID="UserDefindTreePanelMenu" runat="server">
        <Items>
            <ext:MenuItem ID="UserDefindTreePanelMenuItem1" runat="server" Text="新增群组" Icon="Add">
                <DirectEvents>
                    <Click OnEvent="ShowUserDefindGroupSettingWindow">
                        <ExtraParams>
                            <ext:Parameter Name="Cmd" Value="Add" Mode="Value" />
                            <ext:Parameter Name="NodeID" Value="this.parentMenu.node.id" Mode="Raw" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:MenuItem>
            <ext:MenuSeparator ID="UserDefindTreePanelMenuSeparator1" runat="server" />
            <ext:MenuItem ID="UserDefindTreePanelMenuItem2" runat="server" Text="删除群组" Icon="Delete">
                <Listeners>
                    <Click Fn="delUserDefindGroup" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuItem ID="UserDefindTreePanelMenuItem3" runat="server" Text="设置群组" Icon="ApplicationEdit">
                <DirectEvents>
                    <Click OnEvent="ShowUserDefindGroupSettingWindow">
                        <ExtraParams>
                            <ext:Parameter Name="Cmd" Value="Edit" Mode="Value" />
                            <ext:Parameter Name="NodeID" Value="this.parentMenu.node.id" Mode="Raw" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    <ext:Menu ID="NodesGridRowMenu" runat="server">
        <Items>
            <ext:MenuItem ID="NodesGridRowItem1" runat="server" Text="实时曲线" Icon="ChartCurve">
                <Listeners>
                    <Click Handler="openNewPage('RealTimeCurve.aspx?LscID='+this.parentMenu.dataRecord.data.LscID+'&NodeID='+this.parentMenu.dataRecord.data.NodeID+'&NodeType='+this.parentMenu.dataRecord.data.NodeType);" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuItem ID="NodesGridRowItem2" runat="server" Text="历史曲线" Icon="ChartCurve">
                <Menu>
                    <ext:Menu ID="NodesGridRowQueryMenu" runat="server">
                        <Items>
                            <ext:MenuItem ID="NodesGridRowQueryItem1" runat="server" Text="实时曲线" Icon="ChartCurve">
                                <Listeners>
                                    <Click Handler="openNewPage('HisRealTimeCurve.aspx?LscID='+this.parentMenu.parentMenu.dataRecord.data.LscID+'&NodeID='+this.parentMenu.parentMenu.dataRecord.data.NodeID+'&NodeType='+this.parentMenu.parentMenu.dataRecord.data.NodeType);" />
                                </Listeners>
                            </ext:MenuItem>
                            <ext:MenuItem ID="NodesGridRowQueryItem2" runat="server" Text="统计曲线" Icon="ChartCurve">
                                <Listeners>
                                    <Click Handler="openNewPage('HisCountCurve.aspx?LscID='+this.parentMenu.parentMenu.dataRecord.data.LscID+'&NodeID='+this.parentMenu.parentMenu.dataRecord.data.NodeID+'&NodeType='+this.parentMenu.parentMenu.dataRecord.data.NodeType);" />
                                </Listeners>
                            </ext:MenuItem>
                        </Items>
                    </ext:Menu>
                </Menu>
            </ext:MenuItem>
            <ext:MenuItem ID="NodesGridRowItem3" runat="server" Text="历史告警" Icon="ApplicationViewDetail">
                <Listeners>
                    <Click Handler="openNewPage('HisAlarms.aspx?LscID='+this.parentMenu.dataRecord.data.LscID+'&NodeID='+this.parentMenu.dataRecord.data.NodeID+'&NodeType='+this.parentMenu.dataRecord.data.NodeType);" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuSeparator ID="NodesGridRowSeparator1" runat="server" />
            <ext:MenuItem ID="NodesGridRowItem4" runat="server" Text="刷新列表" Icon="TableRefresh">
                <Listeners>
                    <Click Handler="doNodesGridLoad();" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuSeparator ID="NodesGridRowSeparator2" runat="server" />
            <ext:MenuItem ID="NodesGridRowItem5" runat="server" Text="打印/导出" Icon="Printer">
                <Listeners>
                    <Click Fn="onNodesExportClick" />
                </Listeners>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    <ext:Menu ID="AlarmsGridRowMenu" runat="server">
        <Items>
            <ext:MenuItem ID="AlarmsGridRowItem1" runat="server" Text="告警确认" Icon="PageLightning">
                <DirectEvents>
                    <Click OnEvent="OnConfirmAlarmClick">
                        <ExtraParams>
                            <ext:Parameter Name="LscID" Value="this.parentMenu.dataRecord.data.LscID" Mode="Raw" />
                            <ext:Parameter Name="SerialNO" Value="this.parentMenu.dataRecord.data.SerialNO" Mode="Raw" />
                            <ext:Parameter Name="ConfirmMarking" Value="this.parentMenu.dataRecord.data.ConfirmMarking"
                                Mode="Raw" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:MenuItem>
            <ext:MenuItem ID="AlarmsGridRowItem2" runat="server" Text="告警定位" Icon="Find">
                <Listeners>
                    <Click Fn="onAlarmsFinderClick" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuItem ID="AlarmsGridRowItem3" runat="server" Text="次告警列表" Icon="ApplicationViewDetail">
                <DirectEvents>
                    <Click OnEvent="ShowSAlmDetail">
                        <ExtraParams>
                            <ext:Parameter Name="LscID" Value="this.parentMenu.dataRecord.data.LscID" Mode="Raw" />
                            <ext:Parameter Name="SerialNO" Value="this.parentMenu.dataRecord.data.SerialNO" Mode="Raw" />
                            <ext:Parameter Name="StaName" Value="this.parentMenu.dataRecord.data.StaName" Mode="Raw" />
                            <ext:Parameter Name="DevName" Value="this.parentMenu.dataRecord.data.DevName" Mode="Raw" />
                            <ext:Parameter Name="NodeName" Value="this.parentMenu.dataRecord.data.NodeName" Mode="Raw" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:MenuItem>
            <ext:MenuSeparator ID="AlarmsGridRowSeparator1" runat="server" />
            <ext:MenuItem ID="AlarmsGridRowItem4" runat="server" Text="刷新列表" Icon="TableRefresh">
                <Listeners>
                    <Click Handler="doPageRefresh(1);" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuSeparator ID="AlarmsGridRowSeparator2" runat="server" />
            <ext:MenuItem ID="AlarmsGridRowItem5" runat="server" Text="打印/导出" Icon="Printer">
                <DirectEvents>
                    <Click OnEvent="OnAlarmsExport_Click" IsUpload="true">
                        <ExtraParams>
                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{AlarmsGridPanel})" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    <ext:Menu ID="ACSRowMenu" runat="server">
        <Items>
            <ext:MenuItem ID="ACSRowItem1" runat="server" Text="刷新列表" Icon="TableRefresh">
                <Listeners>
                    <Click Handler="doPageRefresh(2);" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuSeparator ID="ACSRowSeparator1" runat="server" />
            <ext:MenuItem ID="ACSRowItem2" runat="server" Text="设置" Icon="Cog">
                <DirectEvents>
                    <Click OnEvent="ShowAlarmsCountSettingWindow" />
                </DirectEvents>
            </ext:MenuItem>
            <ext:MenuSeparator ID="ACSRowSeparator2" runat="server" />
            <ext:MenuItem ID="ACSRowItem3" runat="server" Text="打印/导出" Icon="Printer">
                <Listeners>
                    <Click Handler="#{NodesGridTabPanel6}.submitData(false);" />
                </Listeners>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    <ext:Menu ID="ACVRowMenu" runat="server">
        <Items>
            <ext:MenuItem ID="ACVRowItem1" runat="server" Text="实时曲线" Icon="ChartCurve">
                <Listeners>
                    <Click Handler="openNewPage('RealTimeCurve.aspx?LscID='+this.parentMenu.dataRecord.data.LscID+'&NodeID='+this.parentMenu.dataRecord.data.NodeID+'&NodeType='+this.parentMenu.dataRecord.data.NodeType);" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuItem ID="ACVRowItem2" runat="server" Text="历史曲线" Icon="ChartCurve">
                <Menu>
                    <ext:Menu ID="Menu2" runat="server">
                        <Items>
                            <ext:MenuItem ID="MenuItem3" runat="server" Text="实时曲线" Icon="Magnifier">
                                <Listeners>
                                    <Click Handler="openNewPage('HisRealTimeCurve.aspx?LscID='+this.parentMenu.parentMenu.dataRecord.data.LscID+'&NodeID='+this.parentMenu.parentMenu.dataRecord.data.NodeID+'&NodeType='+this.parentMenu.parentMenu.dataRecord.data.NodeType);" />
                                </Listeners>
                            </ext:MenuItem>
                            <ext:MenuItem ID="MenuItem4" runat="server" Text="统计曲线" Icon="Magnifier">
                                <Listeners>
                                    <Click Handler="openNewPage('HisCountCurve.aspx?LscID='+this.parentMenu.parentMenu.dataRecord.data.LscID+'&NodeID='+this.parentMenu.parentMenu.dataRecord.data.NodeID+'&NodeType='+this.parentMenu.parentMenu.dataRecord.data.NodeType);" />
                                </Listeners>
                            </ext:MenuItem>
                        </Items>
                    </ext:Menu>
                </Menu>
            </ext:MenuItem>
            <ext:MenuItem ID="ACVRowItem3" runat="server" Text="历史告警" Icon="ApplicationViewDetail">
                <Listeners>
                    <Click Handler="openNewPage('HisAlarms.aspx?LscID='+this.parentMenu.dataRecord.data.LscID+'&NodeID='+this.parentMenu.dataRecord.data.NodeID+'&NodeType='+this.parentMenu.dataRecord.data.NodeType);" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuSeparator ID="ACVRowSeparator1" runat="server" />
            <ext:MenuItem ID="ACVRowItem4" runat="server" Text="刷新列表" Icon="TableRefresh">
                <Listeners>
                    <Click Handler="doNodesGridLoad();" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuSeparator ID="ACVRowSeparator2" runat="server" />
            <ext:MenuItem ID="ACVRowItem5" runat="server" Text="设置" Icon="Cog">
                <DirectEvents>
                    <Click OnEvent="ShowNodesCountSettingWindow" />
                </DirectEvents>
            </ext:MenuItem>
            <ext:MenuSeparator ID="ACVRowSeparator3" runat="server" />
            <ext:MenuItem ID="ACVRowItem6" runat="server" Text="打印/导出" Icon="Printer">
                <Listeners>
                    <Click Fn="exportACVGridNodes" />
                </Listeners>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    <ext:Store ID="ACVRowStore" runat="server" IgnoreExtraFields="false">
        <Reader>
            <ext:JsonReader />
        </Reader>
    </ext:Store>
    <ext:Window ID="ControlWindow" runat="server" Icon="CogEdit" Padding="5" Height="150"
        Width="300" Collapsible="False" Modal="true" InitCenter="true" Hidden="true">
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
    <ext:Window ID="SettingWindow" runat="server" Icon="TableEdit" Padding="5" Height="150"
        Width="300" Collapsible="False" Modal="true" InitCenter="true" Hidden="true">
        <Content>
            <ext:SpinnerField ID="SettingSpinnerField" runat="server" FieldLabel="模拟量输出值" Number="0.000"
                AllowDecimals="true" DecimalPrecision="3" IncrementValue="1" />
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
    <ext:Menu ID="TreeContextMenu" runat="server" EnableScrolling="false">
            <Items>
                <ext:MenuItem ID="NavTreeMenuItem1" runat="server" Text="展开一级" IconCls="icon-expand-all">
                    <Listeners>
                        <Click Handler="expandDepth(#{NavTreePanel},1)" />
                    </Listeners>
                </ext:MenuItem>
                <ext:MenuItem ID="NavTreeMenuItem2" runat="server" Text="折叠一级" IconCls="icon-collapse-all">
                    <Listeners>
                        <Click Handler="collapseDepth(#{NavTreePanel},1)" />
                    </Listeners>
                </ext:MenuItem>
                <ext:MenuSeparator runat="server" />
                <ext:MenuItem ID="NavTreeMenuItem3" runat="server" Text="展开二级" IconCls="icon-expand-all">
                    <Listeners>
                        <Click Handler="expandDepth(#{NavTreePanel},2)" />
                    </Listeners>
                </ext:MenuItem>
                <ext:MenuItem ID="NavTreeMenuItem4" runat="server" Text="折叠二级" IconCls="icon-collapse-all">
                    <Listeners>
                        <Click Handler="collapseDepth(#{NavTreePanel},2)" />
                    </Listeners>
                </ext:MenuItem>
                <ext:MenuSeparator runat="server" />
                <ext:MenuItem ID="NavTreeMenuItem5" runat="server" Text="展开三级" IconCls="icon-expand-all">
                    <Listeners>
                        <Click Handler="expandDepth(#{NavTreePanel},3)" />
                    </Listeners>
                </ext:MenuItem>
                <ext:MenuItem ID="NavTreeMenuItem6" runat="server" Text="折叠三级" IconCls="icon-collapse-all">
                    <Listeners>
                        <Click Handler="collapseDepth(#{NavTreePanel},3)" />
                    </Listeners>
                </ext:MenuItem>
            </Items>
        </ext:Menu>
</asp:Content>