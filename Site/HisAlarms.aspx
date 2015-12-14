<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HisAlarms.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.HisAlarms" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>历史告警</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link rel="icon" type="image/x-icon" href="favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="favicon.ico" />
    <script type="text/javascript" src="Resources/js/public.js?v=3.3.0.0"></script>
    <script type="text/javascript">
        Ext.form.ComboBox.override({
            getSelectedIndex: function() {
                var r = this.findRecord(this.valueField, this.getValue());
                return (!Ext.isEmpty(r)) ? this.indexOfEx(r) : -1;
            }
        });
    </script>
</head>
<body>
    <form id="form1" runat="server" style="">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="HisAlarmsViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="HisAlarmsGridPanel" runat="server" StripeRows="true" Border="false" TrackMouseOver="true" AutoScroll="true" Region="Center" Title="历史告警" Icon="ApplicationViewList">
                <TopBar>
                    <ext:Toolbar ID="HisAlarmsToolbars" runat="server" Layout="ContainerLayout">
                        <Items>
                            <ext:Toolbar ID="HisAlarmsToolbar1" runat="server" Flat="true">
                                <Items>
                                    <ext:ComboBox ID="LscsComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="Lsc名称"
                                        Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                        ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                        Resizable="true">
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
                                                    <Load Handler="
                                                    var sctId = #{LscHF}.getValue();#{LscHF}.setValue('');
                                                    if(this.getCount() != 0){
                                                        if(Ext.isEmpty(sctId, false) 
                                                        || this.findExact('Id',sctId) == -1){
                                                            sctId = this.getAt(0).get('Id');
                                                        }
                                                        #{LscsComboBox}.setValueAndFireSelect(sctId);
                                                    }" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Handler="#{Area2ComboBox}.clearValue();#{Area2ComboBox}.store.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="Area2ComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="地区名称"
                                        Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                        ForceSelection="false" TriggerAction="All" EmptyText="..." SelectOnFocus="true"
                                        Resizable="true">
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
                                                    <Load Handler="
                                                    var sctId = #{Area2HF}.getValue();#{Area2HF}.setValue('');
                                                    if(this.getCount() != 0) {
                                                        if(Ext.isEmpty(sctId, false)
                                                        || this.findExact('Id',sctId) == -1){
                                                            sctId = this.getAt(0).get('Id');
                                                        }
                                                        #{Area2ComboBox}.setValueAndFireSelect(sctId);
                                                    } else {
                                                        #{Area2ComboBox}.fireEvent('select');
                                                    }" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Handler="#{Area3ComboBox}.clearValue(); #{Area3ComboBox}.store.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="Area3ComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="县市名称"
                                        Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                        ForceSelection="false" TriggerAction="All" EmptyText="..." SelectOnFocus="true"
                                        Resizable="true">
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
                                                    <Load Handler="
                                                    var sctId = #{Area3HF}.getValue();#{Area3HF}.setValue('');
                                                    if(this.getCount() != 0) {
                                                        if(Ext.isEmpty(sctId, false)
                                                        || this.findExact('Id',sctId) == -1){
                                                            sctId = this.getAt(0).get('Id');
                                                        }
                                                        #{Area3ComboBox}.setValueAndFireSelect(sctId);
                                                    } else {
                                                        #{Area3ComboBox}.fireEvent('select');
                                                    }" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Handler="#{StaComboBox}.clearValue(); #{StaComboBox}.store.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="StaComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="局站名称"
                                        Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                        ForceSelection="false" TriggerAction="All" EmptyText="..." SelectOnFocus="true"
                                        Resizable="true">
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
                                                    <Load Handler="
                                                    var sctId = #{StaHF}.getValue();#{StaHF}.setValue('');
                                                    if(this.getCount() != 0) {
                                                        if(Ext.isEmpty(sctId, false)
                                                        || this.findExact('Id',sctId) == -1){
                                                            sctId = this.getAt(0).get('Id');
                                                        }
                                                        #{StaComboBox}.setValueAndFireSelect(sctId);
                                                    }" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Handler="#{DevComboBox}.clearValue(); #{DevComboBox}.store.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="DevComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="设备名称"
                                        Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                        ForceSelection="false" TriggerAction="All" EmptyText="..." SelectOnFocus="true"
                                        Resizable="true">
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
                                                    <Load Handler="
                                                    var sctId = #{DevHF}.getValue();#{DevHF}.setValue('');
                                                    if(this.getCount() != 0) {
                                                        if(Ext.isEmpty(sctId, false)
                                                        || this.findExact('Id',sctId) == -1) {
                                                            sctId = this.getAt(0).get('Id');
                                                        }
                                                        #{DevComboBox}.setValueAndFireSelect(sctId);
                                                    }" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Handler="#{NodeComboBox}.clearValue(); #{NodeComboBox}.store.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar ID="HisAlarmsToolbar2" runat="server" Flat="true">
                                <Items>
                                    <ext:ComboBox ID="NodeComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="测点名称"
                                        Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                        ForceSelection="false" TriggerAction="All" EmptyText="..." SelectOnFocus="true"
                                        Resizable="true">
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
                                                    <Load Handler="
                                                    var sctId = #{NodeHF}.getValue();#{NodeHF}.setValue('');
                                                    if(this.getCount() != 0) {
                                                        if(Ext.isEmpty(sctId, false) 
                                                        || this.findExact('Id',sctId) == -1) {
                                                            sctId = this.getAt(0).get('Id');
                                                        }
                                                        #{NodeComboBox}.setValueAndFireSelect(sctId);
                                                    }" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="AlarmDevComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="告警设备"
                                        Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                        ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                        Resizable="true">
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
                                            <Select Handler="#{AlarmLogicComboBox}.clearValue(); #{AlarmLogicComboBox}.store.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="AlarmLogicComboBox" runat="server" Width="180" LabelWidth="50"
                                        FieldLabel="告警逻辑" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true"
                                        Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..."
                                        SelectOnFocus="true" Resizable="true">
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
                                            <Select Handler="#{AlarmNameComboBox}.clearValue(); #{AlarmNameComboBox}.store.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="AlarmNameComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="告警名称"
                                        Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                        ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                        Resizable="true">
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
                                                    <Load Handler="#{AlarmNameComboBox}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                        <Listeners>
                                            <Select Handler="#{AlarmLevelMultiCombo}.clearValue(); #{AlarmLevelMultiCombo}.store.reload();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                    <ext:MultiCombo ID="AlarmLevelMultiCombo" runat="server" SelectionMode="All" Width="180"
                                        LabelWidth="50" FieldLabel="告警级别" EmptyText="--" DisplayField="Name" ValueField="Id"
                                        ForceSelection="true" TriggerAction="All" Resizable="true">
                                        <Store>
                                            <ext:Store ID="AlarmLevelStore" runat="server" AutoLoad="false" OnRefreshData="OnAlarmLevelRefresh">
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
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar ID="HisAlarmsToolbar3" runat="server" Flat="true">
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
                                    <ext:TriggerField ID="EndFromDate" runat="server" Width="180" LabelWidth="50" FieldLabel="结束时间">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Date" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="WdatePicker({el:'EndFromDate',isShowClear:true,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{EndToDate}.fireEvent('triggerclick')}});" />
                                        </Listeners>
                                    </ext:TriggerField>
                                    <ext:TriggerField ID="EndToDate" runat="server" Width="180" LabelWidth="50" FieldLabel="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                        LabelSeparator="~">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Date" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="WdatePicker({el:'EndToDate',isShowClear:true,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'EndFromDate\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                        </Listeners>
                                    </ext:TriggerField>
                                    <ext:TriggerField ID="ConfirmNameTextField" runat="server" Width="180" LabelWidth="50"
                                        FieldLabel="确认人">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Clear" HideTrigger="false" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="this.setValue('');" />
                                        </Listeners>
                                    </ext:TriggerField>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar ID="HisAlarmsToolbar4" runat="server" Flat="true">
                                <Items>
                                    <ext:TriggerField ID="ConfirmFromDate" runat="server" Width="180" LabelWidth="50"
                                        FieldLabel="确认时间">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Date" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="WdatePicker({el:'ConfirmFromDate',isShowClear:true,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{ConfirmToDate}.fireEvent('triggerclick')}});" />
                                        </Listeners>
                                    </ext:TriggerField>
                                    <ext:TriggerField ID="ConfirmToDate" runat="server" Width="180" LabelWidth="50" FieldLabel="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                                        LabelSeparator="~">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Date" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="WdatePicker({el:'ConfirmToDate',isShowClear:true,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'ConfirmFromDate\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                        </Listeners>
                                    </ext:TriggerField>
                                    <ext:NumberField ID="FromMinNumberField" runat="server" MinValue="0" Width="180"
                                        LabelWidth="50" FieldLabel="告警历时" EmptyText="分钟">
                                    </ext:NumberField>
                                    <ext:NumberField ID="ToMinNumberField" runat="server" MinValue="0" Width="180" LabelWidth="50"
                                        FieldLabel="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" LabelSeparator="~" EmptyText="分钟">
                                    </ext:NumberField>
                                    <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5" />
                                    <ext:SplitButton ID="QueryBtn" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                        <Menu>
                                            <ext:Menu ID="QueryMenu1" runat="server">
                                                <Items>
                                                    <ext:CheckMenuItem ID="OtherOptionsMenuItem1" runat="server" Text="仅查询未确认告警" HideOnClick="false">
                                                        <Listeners>
                                                            <CheckChange Handler="if(this.checked){#{OtherOptionsMenuItem2}.setChecked(false);}" />
                                                        </Listeners>
                                                    </ext:CheckMenuItem>
                                                    <ext:CheckMenuItem ID="OtherOptionsMenuItem2" runat="server" Text="仅查询已确认告警" HideOnClick="false">
                                                        <Listeners>
                                                            <CheckChange Handler="if(this.checked){#{OtherOptionsMenuItem1}.setChecked(false);}" />
                                                        </Listeners>
                                                    </ext:CheckMenuItem>
                                                    <ext:MenuSeparator />
                                                    <ext:CheckMenuItem ID="OtherOptionsMenuItem3" runat="server" Text="仅查询标准化告警" HideOnClick="false">
                                                        <Listeners>
                                                            <CheckChange Handler="if(this.checked){#{OtherOptionsMenuItem4}.setChecked(false);}" />
                                                        </Listeners>
                                                    </ext:CheckMenuItem>
                                                    <ext:CheckMenuItem ID="OtherOptionsMenuItem4" runat="server" Text="仅查询非标准化告警" HideOnClick="false">
                                                        <Listeners>
                                                            <CheckChange Handler="if(this.checked){#{OtherOptionsMenuItem3}.setChecked(false);}" />
                                                        </Listeners>
                                                    </ext:CheckMenuItem>
                                                    <ext:MenuSeparator />
                                                    <ext:CheckMenuItem ID="OtherOptionsMenuItem5" runat="server" Text="仅查询非工程告警" HideOnClick="false">
                                                        <Listeners>
                                                            <CheckChange Handler="if(this.checked){#{OtherOptionsMenuItem6}.setChecked(false);}" />
                                                        </Listeners>
                                                    </ext:CheckMenuItem>
                                                    <ext:CheckMenuItem ID="OtherOptionsMenuItem6" runat="server" Text="仅查询工程告警" HideOnClick="false">
                                                        <Listeners>
                                                            <CheckChange Handler="if(this.checked){#{OtherOptionsMenuItem5}.setChecked(false);}" />
                                                        </Listeners>
                                                    </ext:CheckMenuItem>
                                                    <ext:MenuSeparator />
                                                    <ext:MenuItem ID="SaveMenuItem" runat="server" Text="打印/导出" Icon="Printer">
                                                        <DirectEvents>
                                                            <Click OnEvent="SaveBtn_Click" IsUpload="true">
                                                                <ExtraParams>
                                                                    <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{HisAlarmsGridPanel})" />
                                                                </ExtraParams>
                                                            </Click>
                                                        </DirectEvents>
                                                    </ext:MenuItem>
                                                </Items>
                                            </ext:Menu>
                                        </Menu>
                                        <DirectEvents>
                                            <Click OnEvent="QueryBtn_Click" Success="#{HisAlarmsGridPagingToolbar}.doLoad();">
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{HisAlarmsGridPanel}.body.up('div')" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:SplitButton>
                                    <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="5" />
                                </Items>
                            </ext:Toolbar>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store ID="HisAlarmsStore" runat="server" AutoLoad="false" OnRefreshData="OnHisAlarmsRefresh">
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
                <ColumnModel ID="HisAlarmsColumnModel" runat="server">
                    <Columns>
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
                        <ext:Column Header="告警级别" DataIndex="AlarmLevelName" Align="Center" />
                        <ext:Column Header="告警时间" DataIndex="StartTime" Align="Center" />
                        <ext:Column Header="结束时间" DataIndex="EndTime" Align="Center" />
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
                    <ext:RowSelectionModel ID="HisAlarmsRowSelectionModel" runat="server" SingleSelect="true">
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
                    <ext:GridView ID="HisAlarmsGridView" runat="server" ForceFit="false">
                        <GetRowClass Fn="HisAlarms.setGridRowClass" />
                    </ext:GridView>
                </View>
                <Listeners>
                    <RowContextMenu Fn="HisAlarms.showGridRowContextMenu" Scope="HisAlarms" />
                </Listeners>
                <LoadMask ShowMask="true" />
                <BottomBar>
                    <ext:PagingToolbar ID="HisAlarmsGridPagingToolbar" runat="server" PageSize="20" StoreID="HisAlarmsStore">
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
                                    <Select Handler="#{HisAlarmsGridPagingToolbar}.pageSize = parseInt(this.getValue()); #{HisAlarmsGridPagingToolbar}.doLoad();" />
                                </Listeners>
                            </ext:ComboBox>
                        </Items>
                    </ext:PagingToolbar>
                </BottomBar>
            </ext:GridPanel>
            <ext:GridPanel ID="SubAlarmsPanel" runat="server" Header="false" Height="100px" CollapseMode="Mini"
                Collapsible="true" Split="true" Collapsed="false" Region="South" Border="false">
                <Plugins>
                    <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh1"
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
                    <Collapse Handler="HisAlarms.collapseChecked();" Scope="HisAlarms" />
                    <Expand Handler="HisAlarms.expandChecked();" Scope="HisAlarms" />
                </Listeners>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    <ext:Menu ID="HisAlarmsGridRowMenu" runat="server">
        <Items>
            <ext:CheckMenuItem ID="AlarmsGridRowItem1" runat="server" Text="显示告警级别色" Checked="true"
                CheckHandler="function(){ #{HisAlarmsGridPagingToolbar}.doRefresh(); }" />
            <ext:CheckMenuItem ID="AlarmsGridRowItem2" runat="server" Text="显示告警子表" Checked="true"
                CheckHandler="HisAlarms.showSubPanel" Scope="HisAlarms" />
            <ext:MenuItem ID="AlarmsGridRowItem3" runat="server" Text="次告警列表" Icon="ApplicationViewDetail">
                <DirectEvents>
                    <Click OnEvent="ShowHisSAlmDetail">
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
            <ext:MenuItem ID="AlarmsGridRowItem4" runat="server" Text="打印/导出" Icon="Printer">
                <DirectEvents>
                    <Click OnEvent="SaveBtn_Click" IsUpload="true">
                        <ExtraParams>
                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{HisAlarmsGridPanel})" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    <ext:Hidden ID="LscHF" runat="server" />
    <ext:Hidden ID="Area2HF" runat="server" />
    <ext:Hidden ID="Area3HF" runat="server" />
    <ext:Hidden ID="StaHF" runat="server" />
    <ext:Hidden ID="DevHF" runat="server" />
    <ext:Hidden ID="NodeHF" runat="server" />
    </form>
</body>
</html>
