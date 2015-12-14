<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActiveAlarms.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.ActiveAlarms" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>实时告警</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link rel="icon" type="image/x-icon" href="favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="favicon.ico" />
    <script type="text/javascript" src="Resources/js/public.js?v=3.3.0.0"></script>
    <script type="text/javascript">
        Ext.form.ComboBox.override({
            getSelectedIndex: function () {
                var r = this.findRecord(this.valueField, this.getValue());
                return (!Ext.isEmpty(r)) ? this.indexOfEx(r) : -1;
            }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="MainViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="AlarmsGridPanel" runat="server" Title="实时告警列表" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Border="false" Region="Center">
                <TopBar>
                    <ext:Toolbar ID="AlarmsToolbars" runat="server" Layout="ContainerLayout">
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
                                                    <ItemClick Handler="X.ActiveAlarms.OnOtherOptionsMenuClick(menuItem.text,{success : function(result) { doAlarmsGridLoad(); }});" />
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
                        <GetRowClass Handler="if (!#{AlarmsGridCheckMenuItem1}.checked) { return false; } return setGridRowColor(record);" />
                    </ext:LockingGridView>
                </View>
                <Listeners>
                    <RowContextMenu Fn="onAlarmsGridRowContextMenuShow" />
                </Listeners>
                <BottomBar>
                    <ext:PagingToolbar ID="AlarmsGridPagingToolbar" runat="server" PageSize="50" StoreID="AlarmsStore">
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
                                <SelectedItem Value="50" />
                                <Listeners>
                                    <Select Handler="#{AlarmsGridPagingToolbar}.pageSize = parseInt(this.getValue()); #{AlarmsGridPagingToolbar}.doLoad();" />
                                </Listeners>
                            </ext:ComboBox>
                        </Items>
                    </ext:PagingToolbar>
                </BottomBar>
            </ext:GridPanel>
            <ext:GridPanel ID="SubAlarmsPanel" runat="server" AutoExpandColumn="AlarmInterpret" Header="false" Height="60" CollapseMode="Mini" Collapsible="true" Split="true" Collapsed="false" Border="false" Region="South">
                <Plugins>
                    <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh9" runat="server" />
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
                        <ext:Column Header="告警编号" DataIndex="SerialNO" Align="Left" />
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
                    <Collapse Handler="#{AlarmsGridCheckMenuItem2}.setChecked(false);" />
                    <Expand Handler="#{AlarmsGridCheckMenuItem2}.setChecked(true);" />
                </Listeners>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    <ext:TaskManager ID="MainTaskManager" runat="server" AutoRunDelay="2000">
        <Tasks>
            <ext:Task AutoRun="true" Interval="15000">
                <Listeners>
                    <Update Handler="#{AlarmsGridPagingToolbar}.doRefresh();" />
                </Listeners>
            </ext:Task>
        </Tasks>
    </ext:TaskManager>
    <ext:Menu ID="AlarmsGridRowMenu" runat="server">
        <Items>
            <ext:MenuItem ID="AlarmsGridRowItem1" runat="server" Text="告警确认" Icon="PageLightning">
                <Listeners>
                    <Click Handler="X.ActiveAlarms.SetConfirmAlarm(this.parentMenu.dataRecord.data.LscID,this.parentMenu.dataRecord.data.SerialNO,this.parentMenu.dataRecord.data.ConfirmMarking);" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuItem ID="AlarmsGridRowItem2" runat="server" Text="全页确认" Icon="PageLightning">
                <Listeners>
                    <Click Handler="X.ActiveAlarms.SetPageConfirmAlarms(#{AlarmsGridPagingToolbar}.cursor, #{AlarmsGridPagingToolbar}.pageSize);" />
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
            <ext:CheckMenuItem ID="AlarmsGridCheckMenuItem1" runat="server" Text="显示告警级别色" Checked="true" CheckHandler="function(){ #{AlarmsGridPagingToolbar}.doRefresh(); }" />
            <ext:CheckMenuItem ID="AlarmsGridCheckMenuItem2" runat="server" Text="显示告警子表" Checked="true" CheckHandler="function(){ if(this.checked) {#{SubAlarmsPanel}.expand();} else {#{SubAlarmsPanel}.collapse();} }" />
            <ext:MenuSeparator ID="AlarmsGridRowSeparator2" runat="server" />
            <ext:MenuItem ID="AlarmsGridRowItem4" runat="server" Text="刷新列表" Icon="TableRefresh">
                <Listeners>
                    <Click Handler="#{AlarmsGridPagingToolbar}.doRefresh();" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuSeparator ID="AlarmsGridRowSeparator3" runat="server" />
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
    </form>
</body>
</html>
