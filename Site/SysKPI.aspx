<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysKPI.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.SysKPI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>系统考核指标</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <style type="text/css">
        .x-grouptabs-panel .x-grouptabs-expand { background-image: none; }
        .x-tab-panel-left .x-tab-panel-header ul.x-grouptabs-strip a.x-grouptabs-text { padding-left: 5px; }
        .x-grouptabs-panel { border-width: 10px; }
    </style>
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link rel="icon" type="image/x-icon" href="favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="favicon.ico" />
    <script type="text/javascript" src="Resources/js/public.js?v=3.3.0.0"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="PageViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GroupTabPanel ID="PageGroupTabPanel" runat="server" TabWidth="120" Padding="1" Region="Center">
                <Groups>
                    <ext:GroupTab ID="GroupTab1" runat="server">
                        <Items>
                            <ext:Panel ID="GroupContainer1" runat="server" Title="动环设备完好率" TabTip="动环设备完好率" Layout="FitLayout">
                                <Items>
                                    <ext:GridPanel ID="GroupGridPanel11" runat="server" Title="动环设备完好率＝{1－动环设备告警时长总和/(动环设备数量×统计时长)}×100%" Icon="ApplicationViewList" StripeRows="true" TrackMouseOver="true" AutoScroll="true">
                                        <TopBar>
                                            <ext:Toolbar ID="TopToolbars11" runat="server" Layout="ContainerLayout">
                                                <Items>
                                                    <ext:Toolbar ID="TopToolbar11" runat="server" Flat="true">
                                                        <Items>
                                                            <ext:ComboBox ID="LscsComboBox11" runat="server" Width="180" LabelWidth="50" FieldLabel="Lsc名称" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true" Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="LscsStore11" runat="server" AutoLoad="true" OnRefreshData="OnLscsRefresh">
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
                                                                            <Load Handler="if(this.getCount() > 0){ #{LscsComboBox11}.setValueAndFireSelect(this.getAt(0).get('Id')); }" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Handler="#{Area2ComboBox11}.clearValue();#{Area2ComboBox11}.store.reload();" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="Area2ComboBox11" runat="server" Width="180" LabelWidth="50" FieldLabel="地区名称"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="Area2Store11" runat="server" AutoLoad="false" OnRefreshData="OnArea2Refresh11">
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
                                                                            <Load Handler="#{Area2ComboBox11}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Handler="#{Area3ComboBox11}.clearValue(); #{Area3ComboBox11}.store.reload();" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="Area3ComboBox11" runat="server" Width="180" LabelWidth="50" FieldLabel="县市名称"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="Area3Store11" runat="server" AutoLoad="false" OnRefreshData="OnArea3Refresh11">
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
                                                                            <Load Handler="#{Area3ComboBox11}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="AlarmDevComboBox11" runat="server" Width="180" LabelWidth="50" FieldLabel="告警设备"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="AlarmDevStore11" runat="server" AutoLoad="true" OnRefreshData="OnAlarmDevRefresh">
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
                                                                            <Load Handler="#{AlarmDevComboBox11}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar ID="TopToolbar12" runat="server" Flat="true">
                                                        <Items>
                                                            <ext:TriggerField ID="FromDate11" runat="server" Width="180" LabelWidth="50" FieldLabel="开始时间">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Date" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Handler="WdatePicker({el:'FromDate11',isShowClear:false,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{ToDate11}.fireEvent('triggerclick')}});" />
                                                                </Listeners>
                                                            </ext:TriggerField>
                                                            <ext:TriggerField ID="ToDate11" runat="server" Width="180" LabelWidth="50" FieldLabel="结束时间">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Date" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Handler="WdatePicker({el:'ToDate11',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'FromDate11\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                                                </Listeners>
                                                            </ext:TriggerField>
                                                            <ext:NumberField ID="FaultLast11" runat="server" Width="180" LabelWidth="50" FieldLabel="故障时长" MinValue="0" DecimalPrecision="2" EmptyText="规定的故障时长(分钟)" />
                                                            <ext:ToolbarSpacer ID="ToolbarSpacer11" runat="server" Width="5" />
                                                            <ext:SplitButton ID="QueryBtn11" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                                                <Menu>
                                                                    <ext:Menu ID="QueryMenu11" runat="server">
                                                                        <Items>
                                                                            <ext:MenuItem ID="SettingMenuItem11" runat="server" Text="参考值设置" Icon="Cog">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="SettingMenuItem_Click" />
                                                                                </DirectEvents>
                                                                            </ext:MenuItem>
                                                                            <ext:MenuSeparator ID="QueryMenuSeparator11" runat="server" />
                                                                            <ext:MenuItem ID="SaveMenuItem11" runat="server" Text="打印/导出" Icon="Printer">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="SaveBtn11_Click" IsUpload="true">
                                                                                        <ExtraParams>
                                                                                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{GroupGridPanel11})" />
                                                                                        </ExtraParams>
                                                                                    </Click>
                                                                                </DirectEvents>
                                                                            </ext:MenuItem>
                                                                        </Items>
                                                                    </ext:Menu>
                                                                </Menu>
                                                                <DirectEvents>
                                                                    <Click OnEvent="QueryBtn_Click" Success="#{BottomPagingToolbar11}.doLoad();">
                                                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{GroupGridPanel11}.body.up('div')" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Cmd" Value="1x" />
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </DirectEvents>
                                                            </ext:SplitButton>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Items>
                                            </ext:Toolbar>
                                        </TopBar>
                                        <Store>
                                            <ext:Store ID="GridStore11" runat="server" AutoLoad="false" OnRefreshData="OnGridStoreRefresh11">
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
                                                            <ext:RecordField Name="DevCnt" Type="Int" />
                                                            <ext:RecordField Name="AlarmLast" Type="String" />
                                                            <ext:RecordField Name="TotalLast" Type="String" />
                                                            <ext:RecordField Name="DevRate" Type="String" />
                                                            <ext:RecordField Name="StandardValue" Type="String" />
                                                            <ext:RecordField Name="ExcellentValue" Type="String" />
                                                            <ext:RecordField Name="Completion" Type="String" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                                <BaseParams>
                                                    <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                                    <ext:Parameter Name="limit" Value="#{PageSizeComboBox11}.getValue()" Mode="Raw" />
                                                </BaseParams>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel ID="GridColumnModel11" runat="server">
                                            <Columns>
                                                <ext:Column Header="序号" DataIndex="ID" Align="Left" Locked="true"/>
                                                <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" Locked="true"/>
                                                <ext:Column Header="地区名称" DataIndex="Area2Name" Align="Left" Locked="true"/>
                                                <ext:Column Header="县市名称" DataIndex="Area3Name" Align="Left" Locked="true"/>
                                                <ext:Column Header="设备数量" DataIndex="DevCnt" Align="Left" />
                                                <ext:Column Header="动环设备告警总时长(H)" DataIndex="AlarmLast" Align="Left" />
                                                <ext:Column Header="动环设备×统计时长(H)" DataIndex="TotalLast" Align="Left" />
                                                <ext:Column Header="动环设备完好率" DataIndex="DevRate" Align="Left" />
                                                <ext:Column Header="达标值" DataIndex="StandardValue" Align="Left" />
                                                <ext:Column Header="优秀值" DataIndex="ExcellentValue" Align="Left" />
                                                <ext:Column Header="完成度" DataIndex="Completion" Align="Left" />
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:CellSelectionModel ID="GridRowSelectionModel11" runat="server" />
                                        </SelectionModel>
                                        <View>
                                            <ext:LockingGridView ID="LockingGridView11" runat="server" ForceFit="true"/>
                                        </View>
                                        <Listeners>
                                            <CellDblClick Fn="SysKPI.onGridCellDblClick11" Scope="SysKPI" />
                                        </Listeners>
                                        <LoadMask ShowMask="true" />
                                        <BottomBar>
                                            <ext:PagingToolbar ID="BottomPagingToolbar11" runat="server" PageSize="20" StoreID="GridStore11">
                                                <Items>
                                                    <ext:ComboBox ID="PageSizeComboBox11" runat="server" Width="150" LabelWidth="60"
                                                        FieldLabel="<%$ Resources: GlobalResource, ComboBoxPageSize %>">
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
                                                            <Select Handler="#{BottomPagingToolbar11}.pageSize = parseInt(this.getValue()); #{BottomPagingToolbar11}.doLoad();" />
                                                        </Listeners>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:PagingToolbar>
                                        </BottomBar>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:GroupTab>
                    <ext:GroupTab ID="GroupTab2" runat="server">
                        <Items>
                            <ext:Panel ID="GroupContainer2" runat="server" Title="故障修复及时率" TabTip="故障修复及时率" Layout="FitLayout">
                                <Items>
                                    <ext:GridPanel ID="GroupGridPanel21" runat="server" Title="动环设备故障修复及时率＝{1－（超出规定处理时长的动环设备故障次数/动环设备故障总次数)}×100%" Icon="ApplicationViewList" StripeRows="true" TrackMouseOver="true" AutoScroll="true">
                                        <TopBar>
                                            <ext:Toolbar ID="TopToolbars21" runat="server" Layout="ContainerLayout">
                                                <Items>
                                                    <ext:Toolbar ID="TopToolbar21" runat="server" Flat="true">
                                                        <Items>
                                                            <ext:ComboBox ID="LscsComboBox21" runat="server" Width="180" LabelWidth="50" FieldLabel="Lsc名称" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true" Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="LscsStore21" runat="server" AutoLoad="true" OnRefreshData="OnLscsRefresh">
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
                                                                            <Load Handler="if(this.getCount() > 0){ #{LscsComboBox21}.setValueAndFireSelect(this.getAt(0).get('Id')); }" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Handler="#{Area2ComboBox21}.clearValue();#{Area2ComboBox21}.store.reload();" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="Area2ComboBox21" runat="server" Width="180" LabelWidth="50" FieldLabel="地区名称"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="Area2Store21" runat="server" AutoLoad="false" OnRefreshData="OnArea2Refresh21">
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
                                                                            <Load Handler="#{Area2ComboBox21}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Handler="#{Area3ComboBox21}.clearValue(); #{Area3ComboBox21}.store.reload();" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="Area3ComboBox21" runat="server" Width="180" LabelWidth="50" FieldLabel="县市名称"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="Area3Store21" runat="server" AutoLoad="false" OnRefreshData="OnArea3Refresh21">
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
                                                                            <Load Handler="#{Area3ComboBox21}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="AlarmDevComboBox21" runat="server" Width="180" LabelWidth="50" FieldLabel="告警设备"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="AlarmDevStore21" runat="server" AutoLoad="true" OnRefreshData="OnAlarmDevRefresh">
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
                                                                            <Load Handler="#{AlarmDevComboBox21}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar ID="TopToolbar22" runat="server" Flat="true">
                                                        <Items>
                                                            <ext:TriggerField ID="FromDate21" runat="server" Width="180" LabelWidth="50" FieldLabel="开始时间">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Date" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Handler="WdatePicker({el:'FromDate21',isShowClear:false,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{ToDate21}.fireEvent('triggerclick')}});" />
                                                                </Listeners>
                                                            </ext:TriggerField>
                                                            <ext:TriggerField ID="ToDate21" runat="server" Width="180" LabelWidth="50" FieldLabel="结束时间">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Date" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Handler="WdatePicker({el:'ToDate21',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'FromDate21\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                                                </Listeners>
                                                            </ext:TriggerField>
                                                            <ext:NumberField ID="FaultLast21" runat="server" Width="180" LabelWidth="50" FieldLabel="处理时长" MinValue="0" DecimalPrecision="2" EmptyText="规定的处理时长(分钟)" />
                                                            <ext:ToolbarSpacer ID="ToolbarSpacer21" runat="server" Width="5" />
                                                            <ext:SplitButton ID="QueryBtn21" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                                                <Menu>
                                                                    <ext:Menu ID="QueryMenu21" runat="server">
                                                                        <Items>
                                                                            <ext:MenuItem ID="SettingMenuItem21" runat="server" Text="参考值设置" Icon="Cog">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="SettingMenuItem_Click" />
                                                                                </DirectEvents>
                                                                            </ext:MenuItem>
                                                                            <ext:MenuSeparator ID="QueryMenuSeparator21" runat="server" />
                                                                            <ext:MenuItem ID="SaveMenuItem21" runat="server" Text="打印/导出" Icon="Printer">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="SaveBtn21_Click" IsUpload="true">
                                                                                        <ExtraParams>
                                                                                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{GroupGridPanel21})" />
                                                                                        </ExtraParams>
                                                                                    </Click>
                                                                                </DirectEvents>
                                                                            </ext:MenuItem>
                                                                        </Items>
                                                                    </ext:Menu>
                                                                </Menu>
                                                                <DirectEvents>
                                                                    <Click OnEvent="QueryBtn_Click" Success="#{BottomPagingToolbar21}.doLoad();">
                                                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{GroupGridPanel21}.body.up('div')" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Cmd" Value="2x" />
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </DirectEvents>
                                                            </ext:SplitButton>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Items>
                                            </ext:Toolbar>
                                        </TopBar>
                                        <Store>
                                            <ext:Store ID="GridStore21" runat="server" AutoLoad="false" OnRefreshData="OnGridStoreRefresh21">
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
                                                            <ext:RecordField Name="AlarmCnt" Type="String" />
                                                            <ext:RecordField Name="TotalAlarmCnt" Type="String" />
                                                            <ext:RecordField Name="DevRepairRate" Type="String" />
                                                            <ext:RecordField Name="StandardValue" Type="String" />
                                                            <ext:RecordField Name="ExcellentValue" Type="String" />
                                                            <ext:RecordField Name="Completion" Type="String" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                                <BaseParams>
                                                    <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                                    <ext:Parameter Name="limit" Value="#{PageSizeComboBox21}.getValue()" Mode="Raw" />
                                                </BaseParams>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel ID="GridColumnModel21" runat="server">
                                            <Columns>
                                                <ext:Column Header="序号" DataIndex="ID" Align="Left" Locked="true"/>
                                                <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" Locked="true"/>
                                                <ext:Column Header="地区名称" DataIndex="Area2Name" Align="Left" Locked="true"/>
                                                <ext:Column Header="县市名称" DataIndex="Area3Name" Align="Left" Locked="true"/>
                                                <ext:Column Header="超出处理时长的动环设备故障次数" DataIndex="AlarmCnt" Align="Left" />
                                                <ext:Column Header="动环设备故障总次数" DataIndex="TotalAlarmCnt" Align="Left" />
                                                <ext:Column Header="动环设备故障修复及时率" DataIndex="DevRepairRate" Align="Left" />
                                                <ext:Column Header="达标值" DataIndex="StandardValue" Align="Left" />
                                                <ext:Column Header="优秀值" DataIndex="ExcellentValue" Align="Left" />
                                                <ext:Column Header="完成度" DataIndex="Completion" Align="Left" />
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:CellSelectionModel ID="GridRowSelectionModel21" runat="server" />
                                        </SelectionModel>
                                        <View>
                                            <ext:LockingGridView ID="LockingGridView21" runat="server" ForceFit="true"/>
                                        </View>
                                        <Listeners>
                                            <CellDblClick Fn="SysKPI.onGridCellDblClick21" Scope="SysKPI" />
                                        </Listeners>
                                        <LoadMask ShowMask="true" />
                                        <BottomBar>
                                            <ext:PagingToolbar ID="BottomPagingToolbar21" runat="server" PageSize="20" StoreID="GridStore21">
                                                <Items>
                                                    <ext:ComboBox ID="PageSizeComboBox21" runat="server" Width="150" LabelWidth="60"
                                                        FieldLabel="<%$ Resources: GlobalResource, ComboBoxPageSize %>">
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
                                                            <Select Handler="#{BottomPagingToolbar21}.pageSize = parseInt(this.getValue()); #{BottomPagingToolbar21}.doLoad();" />
                                                        </Listeners>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:PagingToolbar>
                                        </BottomBar>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:GroupTab>
                    <ext:GroupTab ID="GroupTab3" runat="server">
                        <Items>
                            <ext:Panel ID="GroupContainer3" runat="server" Title="告警关联成功率" TabTip="告警关联规则总体成功率" Layout="FitLayout">
                                <Items>
                                    <ext:GridPanel ID="GroupGridPanel31" runat="server" Title="告警关联规则总体成功率＝{（主次关联中关联成功的次告警数＋衍生关联中关联成功的原始告警数-衍生关联中的衍生告警数）/关联分析涉及告警数}×100%" Icon="ApplicationViewList" StripeRows="true" TrackMouseOver="true" AutoScroll="true">
                                        <TopBar>
                                            <ext:Toolbar ID="TopToolbars31" runat="server" Layout="ContainerLayout">
                                                <Items>
                                                    <ext:Toolbar ID="TopToolbar31" runat="server" Flat="true">
                                                        <Items>
                                                            <ext:ComboBox ID="LscsComboBox31" runat="server" Width="180" LabelWidth="50" FieldLabel="Lsc名称" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true" Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="LscsStore31" runat="server" AutoLoad="true" OnRefreshData="OnLscsRefresh">
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
                                                                            <Load Handler="if(this.getCount() > 0){ #{LscsComboBox31}.setValueAndFireSelect(this.getAt(0).get('Id')); }" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Handler="#{Area2ComboBox31}.clearValue();#{Area2ComboBox31}.store.reload();" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="Area2ComboBox31" runat="server" Width="180" LabelWidth="50" FieldLabel="地区名称"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="Area2Store31" runat="server" AutoLoad="false" OnRefreshData="OnArea2Refresh31">
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
                                                                            <Load Handler="#{Area2ComboBox31}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Handler="#{Area3ComboBox31}.clearValue(); #{Area3ComboBox31}.store.reload();" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="Area3ComboBox31" runat="server" Width="180" LabelWidth="50" FieldLabel="县市名称"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="Area3Store31" runat="server" AutoLoad="false" OnRefreshData="OnArea3Refresh31">
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
                                                                            <Load Handler="#{Area3ComboBox31}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar ID="TopToolbar32" runat="server" Flat="true">
                                                        <Items>
                                                            <ext:ComboBox ID="AlarmDevComboBox31" runat="server" Width="180" LabelWidth="50" FieldLabel="告警设备"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="AlarmDevStore31" runat="server" AutoLoad="true" OnRefreshData="OnAlarmDevRefresh">
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
                                                                            <Load Handler="#{AlarmDevComboBox31}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                            </ext:ComboBox>
                                                            <ext:TriggerField ID="FromDate31" runat="server" Width="180" LabelWidth="50" FieldLabel="开始时间">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Date" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Handler="WdatePicker({el:'FromDate31',isShowClear:false,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{ToDate31}.fireEvent('triggerclick')}});" />
                                                                </Listeners>
                                                            </ext:TriggerField>
                                                            <ext:TriggerField ID="ToDate31" runat="server" Width="180" LabelWidth="50" FieldLabel="结束时间">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Date" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Handler="WdatePicker({el:'ToDate31',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'FromDate31\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                                                </Listeners>
                                                            </ext:TriggerField>
                                                            <ext:ToolbarSpacer ID="ToolbarSpacer31" runat="server" Width="5" />
                                                            <ext:SplitButton ID="QueryBtn31" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                                                <Menu>
                                                                    <ext:Menu ID="QueryMenu31" runat="server">
                                                                        <Items>
                                                                            <ext:MenuItem ID="SettingMenuItem31" runat="server" Text="参考值设置" Icon="Cog">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="SettingMenuItem_Click" />
                                                                                </DirectEvents>
                                                                            </ext:MenuItem>
                                                                            <ext:MenuSeparator ID="QueryMenuSeparator31" runat="server" />
                                                                            <ext:MenuItem ID="SaveMenuItem31" runat="server" Text="打印/导出" Icon="Printer">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="SaveBtn31_Click" IsUpload="true">
                                                                                        <ExtraParams>
                                                                                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{GroupGridPanel31})" />
                                                                                        </ExtraParams>
                                                                                    </Click>
                                                                                </DirectEvents>
                                                                            </ext:MenuItem>
                                                                        </Items>
                                                                    </ext:Menu>
                                                                </Menu>
                                                                <DirectEvents>
                                                                    <Click OnEvent="QueryBtn_Click" Success="#{BottomPagingToolbar31}.doLoad();">
                                                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{GroupGridPanel31}.body.up('div')" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Cmd" Value="3x" />
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </DirectEvents>
                                                            </ext:SplitButton>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Items>
                                            </ext:Toolbar>
                                        </TopBar>
                                        <Store>
                                            <ext:Store ID="GridStore31" runat="server" AutoLoad="false" OnRefreshData="OnGridStoreRefresh31">
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
                                                            <ext:RecordField Name="SAlmCnt" Type="String" />
                                                            <ext:RecordField Name="OriginalBAlmCnt" Type="String" />
                                                            <ext:RecordField Name="BAlmCnt" Type="String" />
                                                            <ext:RecordField Name="TotalAlarmCnt" Type="String" />
                                                            <ext:RecordField Name="AlmSuccessRate" Type="String" />
                                                            <ext:RecordField Name="StandardValue" Type="String" />
                                                            <ext:RecordField Name="ExcellentValue" Type="String" />
                                                            <ext:RecordField Name="Completion" Type="String" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                                <BaseParams>
                                                    <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                                    <ext:Parameter Name="limit" Value="#{PageSizeComboBox31}.getValue()" Mode="Raw" />
                                                </BaseParams>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel ID="GridColumnModel31" runat="server">
                                            <Columns>
                                                <ext:Column Header="序号" DataIndex="ID" Align="Left" Locked="true"/>
                                                <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" Locked="true"/>
                                                <ext:Column Header="地区名称" DataIndex="Area2Name" Align="Left" Locked="true"/>
                                                <ext:Column Header="县市名称" DataIndex="Area3Name" Align="Left" Locked="true"/>
                                                <ext:Column Header="主次关联次告警数" DataIndex="SAlmCnt" Align="Left" />
                                                <ext:Column Header="衍生关联原始告警数" DataIndex="OriginalBAlmCnt" Align="Left" />
                                                <ext:Column Header="衍生关联衍生告警数" DataIndex="BAlmCnt" Align="Left" />
                                                <ext:Column Header="关联总告警数" DataIndex="TotalAlarmCnt" Align="Left" />
                                                <ext:Column Header="关联总体成功率" DataIndex="AlmSuccessRate" Align="Left" />
                                                <ext:Column Header="达标值" DataIndex="StandardValue" Align="Left" />
                                                <ext:Column Header="优秀值" DataIndex="ExcellentValue" Align="Left" />
                                                <ext:Column Header="完成度" DataIndex="Completion" Align="Left" />
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:CellSelectionModel ID="GridRowSelectionModel31" runat="server" />
                                        </SelectionModel>
                                        <View>
                                            <ext:LockingGridView ID="LockingGridView31" runat="server" ForceFit="true"/>
                                        </View>
                                        <Listeners>
                                            <CellDblClick Fn="SysKPI.onGridCellDblClick31" Scope="SysKPI" />
                                        </Listeners>
                                        <LoadMask ShowMask="true" />
                                        <BottomBar>
                                            <ext:PagingToolbar ID="BottomPagingToolbar31" runat="server" PageSize="20" StoreID="GridStore31">
                                                <Items>
                                                    <ext:ComboBox ID="PageSizeComboBox31" runat="server" Width="150" LabelWidth="60"
                                                        FieldLabel="<%$ Resources: GlobalResource, ComboBoxPageSize %>">
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
                                                            <Select Handler="#{BottomPagingToolbar31}.pageSize = parseInt(this.getValue()); #{BottomPagingToolbar31}.doLoad();" />
                                                        </Listeners>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:PagingToolbar>
                                        </BottomBar>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:GroupTab>
                    <ext:GroupTab ID="GroupTab4" runat="server">
                        <Items>
                            <ext:Panel ID="GroupContainer4" runat="server" Title="告警整体压缩率" TabTip="告警整体压缩率" Layout="FitLayout">
                                <Items>
                                    <ext:GridPanel ID="GroupGridPanel41" runat="server" Title="告警整体压缩率={(主次关联中的次告警数＋衍生关联中的原始告警数-衍生关联中的衍生告警数)/1~3级告警原始入库总数(含衍生告警)}×100%" Icon="ApplicationViewList" StripeRows="true" TrackMouseOver="true" AutoScroll="true">
                                        <TopBar>
                                            <ext:Toolbar ID="TopToolbars41" runat="server" Layout="ContainerLayout">
                                                <Items>
                                                    <ext:Toolbar ID="TopToolbar41" runat="server" Flat="true">
                                                        <Items>
                                                            <ext:ComboBox ID="LscsComboBox41" runat="server" Width="180" LabelWidth="50" FieldLabel="Lsc名称" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true" Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="LscsStore41" runat="server" AutoLoad="true" OnRefreshData="OnLscsRefresh">
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
                                                                            <Load Handler="if(this.getCount() > 0){ #{LscsComboBox41}.setValueAndFireSelect(this.getAt(0).get('Id')); }" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Handler="#{Area2ComboBox41}.clearValue();#{Area2ComboBox41}.store.reload();" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="Area2ComboBox41" runat="server" Width="180" LabelWidth="50" FieldLabel="地区名称"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="Area2Store41" runat="server" AutoLoad="false" OnRefreshData="OnArea2Refresh41">
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
                                                                            <Load Handler="#{Area2ComboBox41}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Handler="#{Area3ComboBox41}.clearValue(); #{Area3ComboBox41}.store.reload();" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="Area3ComboBox41" runat="server" Width="180" LabelWidth="50" FieldLabel="县市名称"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="Area3Store41" runat="server" AutoLoad="false" OnRefreshData="OnArea3Refresh41">
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
                                                                            <Load Handler="#{Area3ComboBox41}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar ID="TopToolbar42" runat="server" Flat="true">
                                                        <Items>
                                                            <ext:ComboBox ID="AlarmDevComboBox41" runat="server" Width="180" LabelWidth="50" FieldLabel="告警设备"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="AlarmDevStore41" runat="server" AutoLoad="true" OnRefreshData="OnAlarmDevRefresh">
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
                                                                            <Load Handler="#{AlarmDevComboBox41}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                            </ext:ComboBox>
                                                            <ext:TriggerField ID="FromDate41" runat="server" Width="180" LabelWidth="50" FieldLabel="开始时间">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Date" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Handler="WdatePicker({el:'FromDate41',isShowClear:false,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{ToDate41}.fireEvent('triggerclick')}});" />
                                                                </Listeners>
                                                            </ext:TriggerField>
                                                            <ext:TriggerField ID="ToDate41" runat="server" Width="180" LabelWidth="50" FieldLabel="结束时间">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Date" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Handler="WdatePicker({el:'ToDate41',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'FromDate41\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                                                </Listeners>
                                                            </ext:TriggerField>
                                                            <ext:ToolbarSpacer ID="ToolbarSpacer41" runat="server" Width="5" />
                                                            <ext:SplitButton ID="QueryBtn41" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                                                <Menu>
                                                                    <ext:Menu ID="QueryMenu41" runat="server">
                                                                        <Items>
                                                                            <ext:MenuItem ID="SettingMenuItem41" runat="server" Text="参考值设置" Icon="Cog">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="SettingMenuItem_Click" />
                                                                                </DirectEvents>
                                                                            </ext:MenuItem>
                                                                            <ext:MenuSeparator ID="QueryMenuSeparator41" runat="server" />
                                                                            <ext:MenuItem ID="SaveMenuItem41" runat="server" Text="打印/导出" Icon="Printer">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="SaveBtn41_Click" IsUpload="true">
                                                                                        <ExtraParams>
                                                                                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{GroupGridPanel41})" />
                                                                                        </ExtraParams>
                                                                                    </Click>
                                                                                </DirectEvents>
                                                                            </ext:MenuItem>
                                                                        </Items>
                                                                    </ext:Menu>
                                                                </Menu>
                                                                <DirectEvents>
                                                                    <Click OnEvent="QueryBtn_Click" Success="#{BottomPagingToolbar41}.doLoad();">
                                                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{GroupGridPanel41}.body.up('div')" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Cmd" Value="4x" />
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </DirectEvents>
                                                            </ext:SplitButton>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Items>
                                            </ext:Toolbar>
                                        </TopBar>
                                        <Store>
                                            <ext:Store ID="GridStore41" runat="server" AutoLoad="false" OnRefreshData="OnGridStoreRefresh41">
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
                                                            <ext:RecordField Name="SAlmCnt" Type="String" />
                                                            <ext:RecordField Name="OriginalBAlmCnt" Type="String" />
                                                            <ext:RecordField Name="BAlmCnt" Type="String" />
                                                            <ext:RecordField Name="TotalAlarmCnt" Type="String" />
                                                            <ext:RecordField Name="AlmCompressionRate" Type="String" />
                                                            <ext:RecordField Name="StandardValue" Type="String" />
                                                            <ext:RecordField Name="ExcellentValue" Type="String" />
                                                            <ext:RecordField Name="Completion" Type="String" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                                <BaseParams>
                                                    <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                                    <ext:Parameter Name="limit" Value="#{PageSizeComboBox41}.getValue()" Mode="Raw" />
                                                </BaseParams>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel ID="GridColumnModel41" runat="server">
                                            <Columns>
                                                <ext:Column Header="序号" DataIndex="ID" Align="Left" Locked="true"/>
                                                <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" Locked="true"/>
                                                <ext:Column Header="地区名称" DataIndex="Area2Name" Align="Left" Locked="true"/>
                                                <ext:Column Header="县市名称" DataIndex="Area3Name" Align="Left" Locked="true"/>
                                                <ext:Column Header="主次关联次告警数" DataIndex="SAlmCnt" Align="Left" />
                                                <ext:Column Header="衍生关联原始告警数" DataIndex="OriginalBAlmCnt" Align="Left" />
                                                <ext:Column Header="衍生关联衍生告警数" DataIndex="BAlmCnt" Align="Left" />
                                                <ext:Column Header="1~3级告警总数" DataIndex="TotalAlarmCnt" Align="Left" />
                                                <ext:Column Header="告警整体压缩率" DataIndex="AlmCompressionRate" Align="Left" />
                                                <ext:Column Header="达标值" DataIndex="StandardValue" Align="Left" />
                                                <ext:Column Header="优秀值" DataIndex="ExcellentValue" Align="Left" />
                                                <ext:Column Header="完成度" DataIndex="Completion" Align="Left" />
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:CellSelectionModel ID="GridRowSelectionModel41" runat="server" />
                                        </SelectionModel>
                                        <View>
                                            <ext:LockingGridView ID="LockingGridView41" runat="server" ForceFit="true"/>
                                        </View>
                                        <Listeners>
                                            <CellDblClick Fn="SysKPI.onGridCellDblClick41" Scope="SysKPI" />
                                        </Listeners>
                                        <LoadMask ShowMask="true" />
                                        <BottomBar>
                                            <ext:PagingToolbar ID="BottomPagingToolbar41" runat="server" PageSize="20" StoreID="GridStore41">
                                                <Items>
                                                    <ext:ComboBox ID="PageSizeComboBox41" runat="server" Width="150" LabelWidth="60"
                                                        FieldLabel="<%$ Resources: GlobalResource, ComboBoxPageSize %>">
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
                                                            <Select Handler="#{BottomPagingToolbar41}.pageSize = parseInt(this.getValue()); #{BottomPagingToolbar41}.doLoad();" />
                                                        </Listeners>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:PagingToolbar>
                                        </BottomBar>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:GroupTab>
                    <ext:GroupTab ID="GroupTab5" runat="server">
                        <Items>
                            <ext:Panel ID="GroupContainer5" runat="server" Title="告警确认及时率" TabTip="告警确认及时率" Layout="FitLayout">
                                <Items>
                                    <ext:GridPanel ID="GroupGridPanel51" runat="server" Title="告警确认及时率＝{1－（超出规定确认时长的告警条数/告警总条数)}×100%" Icon="ApplicationViewList" StripeRows="true" TrackMouseOver="true" AutoScroll="true">
                                        <TopBar>
                                            <ext:Toolbar ID="TopToolbars51" runat="server" Layout="ContainerLayout">
                                                <Items>
                                                    <ext:Toolbar ID="TopToolbar51" runat="server" Flat="true">
                                                        <Items>
                                                            <ext:ComboBox ID="LscsComboBox51" runat="server" Width="180" LabelWidth="50" FieldLabel="Lsc名称" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true" Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="LscsStore51" runat="server" AutoLoad="true" OnRefreshData="OnLscsRefresh">
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
                                                                            <Load Handler="if(this.getCount() > 0){ #{LscsComboBox51}.setValueAndFireSelect(this.getAt(0).get('Id')); }" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Handler="#{Area2ComboBox51}.clearValue();#{Area2ComboBox51}.store.reload();" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="Area2ComboBox51" runat="server" Width="180" LabelWidth="50" FieldLabel="地区名称"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="Area2Store51" runat="server" AutoLoad="false" OnRefreshData="OnArea2Refresh51">
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
                                                                            <Load Handler="#{Area2ComboBox51}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                                <Listeners>
                                                                    <Select Handler="#{Area3ComboBox51}.clearValue(); #{Area3ComboBox51}.store.reload();" />
                                                                </Listeners>
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="Area3ComboBox51" runat="server" Width="180" LabelWidth="50" FieldLabel="县市名称"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="Area3Store51" runat="server" AutoLoad="false" OnRefreshData="OnArea3Refresh51">
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
                                                                            <Load Handler="#{Area3ComboBox51}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                            </ext:ComboBox>
                                                            <ext:ComboBox ID="AlarmDevComboBox51" runat="server" Width="180" LabelWidth="50" FieldLabel="告警设备"
                                                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                                                Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="AlarmDevStore51" runat="server" AutoLoad="true" OnRefreshData="OnAlarmDevRefresh">
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
                                                                            <Load Handler="#{AlarmDevComboBox51}.setValueAndFireSelect(this.getAt(0).get('Id'));" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                            </ext:ComboBox>
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar ID="TopToolbar52" runat="server" Flat="true">
                                                        <Items>
                                                            <ext:TriggerField ID="FromDate51" runat="server" Width="180" LabelWidth="50" FieldLabel="开始时间">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Date" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Handler="WdatePicker({el:'FromDate51',isShowClear:false,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{ToDate51}.fireEvent('triggerclick')}});" />
                                                                </Listeners>
                                                            </ext:TriggerField>
                                                            <ext:TriggerField ID="ToDate51" runat="server" Width="180" LabelWidth="50" FieldLabel="结束时间">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Date" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Handler="WdatePicker({el:'ToDate51',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'FromDate51\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                                                </Listeners>
                                                            </ext:TriggerField>
                                                            <ext:NumberField ID="FaultLast51" runat="server" Width="180" LabelWidth="50" FieldLabel="确认时长" MinValue="0" DecimalPrecision="2" EmptyText="规定的确认时长(分钟)" />
                                                            <ext:ToolbarSpacer ID="ToolbarSpacer51" runat="server" Width="5" />
                                                            <ext:SplitButton ID="QueryBtn51" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                                                <Menu>
                                                                    <ext:Menu ID="QueryMenu51" runat="server">
                                                                        <Items>
                                                                            <ext:MenuItem ID="SettingMenuItem51" runat="server" Text="参考值设置" Icon="Cog">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="SettingMenuItem_Click" />
                                                                                </DirectEvents>
                                                                            </ext:MenuItem>
                                                                            <ext:MenuSeparator ID="QueryMenuSeparator51" runat="server" />
                                                                            <ext:MenuItem ID="SaveMenuItem51" runat="server" Text="打印/导出" Icon="Printer">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="SaveBtn51_Click" IsUpload="true">
                                                                                        <ExtraParams>
                                                                                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{GroupGridPanel51})" />
                                                                                        </ExtraParams>
                                                                                    </Click>
                                                                                </DirectEvents>
                                                                            </ext:MenuItem>
                                                                        </Items>
                                                                    </ext:Menu>
                                                                </Menu>
                                                                <DirectEvents>
                                                                    <Click OnEvent="QueryBtn_Click" Success="#{BottomPagingToolbar51}.doLoad();">
                                                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{GroupGridPanel51}.body.up('div')" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Cmd" Value="5x" />
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </DirectEvents>
                                                            </ext:SplitButton>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Items>
                                            </ext:Toolbar>
                                        </TopBar>
                                        <Store>
                                            <ext:Store ID="GridStore51" runat="server" AutoLoad="false" OnRefreshData="OnGridStoreRefresh51">
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
                                                            <ext:RecordField Name="TotalAlm" Type="String" />
                                                            <ext:RecordField Name="ConfirmAlm" Type="String" />
                                                            <ext:RecordField Name="ConfirmRate" Type="String" />
                                                            <ext:RecordField Name="StandardValue" Type="String" />
                                                            <ext:RecordField Name="ExcellentValue" Type="String" />
                                                            <ext:RecordField Name="Completion" Type="String" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                                <BaseParams>
                                                    <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                                    <ext:Parameter Name="limit" Value="#{PageSizeComboBox51}.getValue()" Mode="Raw" />
                                                </BaseParams>
                                            </ext:Store>
                                        </Store>
                                        <ColumnModel ID="GridColumnModel51" runat="server">
                                            <Columns>
                                                <ext:Column Header="序号" DataIndex="ID" Align="Left" Locked="true"/>
                                                <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" Locked="true"/>
                                                <ext:Column Header="地区名称" DataIndex="Area2Name" Align="Left" Locked="true"/>
                                                <ext:Column Header="县市名称" DataIndex="Area3Name" Align="Left" Locked="true"/>
                                                <ext:Column Header="总告警数" DataIndex="TotalAlm" Align="Left" />
                                                <ext:Column Header="超出确认时长告警数" DataIndex="ConfirmAlm" Align="Left" />
                                                <ext:Column Header="告警确认及时率" DataIndex="ConfirmRate" Align="Left" />
                                                <ext:Column Header="达标值" DataIndex="StandardValue" Align="Left" />
                                                <ext:Column Header="优秀值" DataIndex="ExcellentValue" Align="Left" />
                                                <ext:Column Header="完成度" DataIndex="Completion" Align="Left" />
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:CellSelectionModel ID="GridRowSelectionModel51" runat="server" />
                                        </SelectionModel>
                                        <View>
                                            <ext:LockingGridView ID="LockingGridView51" runat="server" ForceFit="true"/>
                                        </View>
                                        <Listeners>
                                            <CellDblClick Fn="SysKPI.onGridCellDblClick51" Scope="SysKPI" />
                                        </Listeners>
                                        <LoadMask ShowMask="true" />
                                        <BottomBar>
                                            <ext:PagingToolbar ID="BottomPagingToolbar51" runat="server" PageSize="20" StoreID="GridStore51">
                                                <Items>
                                                    <ext:ComboBox ID="PageSizeComboBox51" runat="server" Width="150" LabelWidth="60"
                                                        FieldLabel="<%$ Resources: GlobalResource, ComboBoxPageSize %>">
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
                                                            <Select Handler="#{BottomPagingToolbar51}.pageSize = parseInt(this.getValue()); #{BottomPagingToolbar51}.doLoad();" />
                                                        </Listeners>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:PagingToolbar>
                                        </BottomBar>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:GroupTab>
                    <ext:GroupTab ID="GroupTab6" runat="server">
                        <Items>
                            <ext:Panel ID="GroupContainer6" runat="server" Title="告警关联率" TabTip="告警关联率" Layout="FitLayout">
                                <Items>
                                    <ext:GridPanel ID="GroupGridPanel61" runat="server" Title="告警关联率＝{1－（工单总数/告警总数)}×100%" Icon="ApplicationViewList" StripeRows="true" TrackMouseOver="true" AutoScroll="true">
                                        <TopBar>
                                            <ext:Toolbar ID="TopToolbars61" runat="server" Layout="ContainerLayout">
                                                <Items>
                                                    <ext:Toolbar ID="TopToolbar61" runat="server" Flat="true">
                                                        <Items>
                                                            <ext:MultiCombo ID="AlmDevTypeMultiCombo61" runat="server" Width="180" SelectionMode="All" LabelWidth="50" FieldLabel="告警设备" EmptyText="select an item..." DisplayField="Name" ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="AlmDevTypeStore61" runat="server" AutoLoad="true" OnRefreshData="OnAlmDevTypeRefresh">
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
                                                                            <%--<Load Handler="#{AlmDevTypeMultiCombo61}.selectAll();" />--%>
                                                                            <Load Handler="#{AlmDevTypeMultiCombo61}.setValue('5,6,7,8,10,11,12,17,76,85,86,91');" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                            </ext:MultiCombo>
                                                            <ext:MultiCombo ID="AlarmNameMultiCombo61" runat="server" Width="360" SelectionMode="All" LabelWidth="50" FieldLabel="告警名称" EmptyText="select an item..." DisplayField="Name" ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true">
                                                                <Store>
                                                                    <ext:Store ID="AlarmNameStore61" runat="server" AutoLoad="true" OnRefreshData="OnAlarmNameRefresh">
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
                                                                            <%--<Load Handler="#{AlarmNameMultiCombo61}.selectAll();" />--%>
                                                                            <Load Handler="#{AlarmNameMultiCombo61}.setValue('5001,5003,5016,5022,6015,6035,6040,6041,6042,8020,8030,8034,8040,8044,8045,8046,8047,11001,11012,11026,85002,85004,85005,85006,85007,85008,86001,86006,86007,86023,91015,91035,91040,91041,91042,5015,11010,11020,6012,6011,7002,8012,10012,8011,5005,76011,17002,17001,17010,76010');" />
                                                                        </Listeners>
                                                                    </ext:Store>
                                                                </Store>
                                                            </ext:MultiCombo>
                                                            
                                                        </Items>
                                                    </ext:Toolbar>
                                                    <ext:Toolbar ID="TopToolbar62" runat="server" Flat="true">
                                                        <Items>
                                                            <ext:TriggerField ID="FromDate61" runat="server" Width="180" LabelWidth="50" FieldLabel="开始时间">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Date" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Handler="WdatePicker({el:'FromDate61',isShowClear:false,readOnly:true,isShowWeek:true,maxDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{ToDate61}.fireEvent('triggerclick')}});" />
                                                                </Listeners>
                                                            </ext:TriggerField>
                                                            <ext:TriggerField ID="ToDate61" runat="server" Width="180" LabelWidth="50" FieldLabel="结束时间">
                                                                <Triggers>
                                                                    <ext:FieldTrigger Icon="Date" />
                                                                </Triggers>
                                                                <Listeners>
                                                                    <TriggerClick Handler="WdatePicker({el:'ToDate61',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'FromDate61\')}',maxDate:'%y-%M-%d %H:%m:%s'});" />
                                                                </Listeners>
                                                            </ext:TriggerField>
                                                            <ext:NumberField ID="GongDanNumberField" runat="server" Width="180" LabelWidth="50" FieldLabel="工单总数" MinValue="0" AllowDecimals="false" />
                                                            <ext:ToolbarSpacer ID="ToolbarSpacer61" runat="server" Width="5" />
                                                            <ext:SplitButton ID="QueryBtn61" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                                                <Menu>
                                                                    <ext:Menu ID="QueryMenu61" runat="server">
                                                                        <Items>
                                                                            <ext:MenuItem ID="SaveMenuItem61" runat="server" Text="打印/导出" Icon="Printer">
                                                                                <DirectEvents>
                                                                                    <Click OnEvent="SaveBtn61_Click" IsUpload="true">
                                                                                        <ExtraParams>
                                                                                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{GroupGridPanel61})" />
                                                                                        </ExtraParams>
                                                                                    </Click>
                                                                                </DirectEvents>
                                                                            </ext:MenuItem>
                                                                        </Items>
                                                                    </ext:Menu>
                                                                </Menu>
                                                                <DirectEvents>
                                                                    <Click OnEvent="QueryBtn_Click" Success="#{BottomPagingToolbar61}.doLoad();">
                                                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{GroupGridPanel61}.body.up('div')" />
                                                                        <ExtraParams>
                                                                            <ext:Parameter Name="Cmd" Value="6x" />
                                                                        </ExtraParams>
                                                                    </Click>
                                                                </DirectEvents>
                                                            </ext:SplitButton>
                                                        </Items>
                                                    </ext:Toolbar>
                                                </Items>
                                            </ext:Toolbar>
                                        </TopBar>
                                        <Store>
                                            <ext:Store ID="GridStore61" runat="server" AutoLoad="false" OnRefreshData="OnGridStoreRefresh61">
                                                <Proxy>
                                                    <ext:PageProxy />
                                                </Proxy>
                                                <Reader>
                                                    <ext:JsonReader IDProperty="ID">
                                                        <Fields>
                                                            <ext:RecordField Name="ID" Type="Int" />
                                                            <ext:RecordField Name="LscName" Type="String" />
                                                            <ext:RecordField Name="TotalGongDan" Type="Int" />
                                                            <ext:RecordField Name="TotalAlm" Type="Int" />
                                                            <ext:RecordField Name="BeginTime" Type="String" />
                                                            <ext:RecordField Name="EndTime" Type="String" />
                                                            <ext:RecordField Name="Rate" Type="String" />
                                                        </Fields>
                                                    </ext:JsonReader>
                                                </Reader>
                                                <BaseParams>
                                                    <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                                    <ext:Parameter Name="limit" Value="#{PageSizeComboBox61}.getValue()" Mode="Raw" />
                                                </BaseParams>
                                            </ext:Store>
                                        </Store>
                                       <ColumnModel ID="GridColumnModel61" runat="server">
                                            <Columns>
                                                <ext:Column Header="序号" DataIndex="ID" Align="Left" />
                                                <ext:Column Header="省份" DataIndex="LscName" Align="Left" />
                                                <ext:Column Header="工单总数" DataIndex="TotalGongDan" Align="Left" />
                                                <ext:Column Header="告警总数" DataIndex="TotalAlm" Align="Left" />
                                                <ext:Column Header="开始时间" DataIndex="BeginTime" Align="Left" />
                                                <ext:Column Header="结束时间" DataIndex="EndTime" Align="Left" />
                                                <ext:Column Header="告警关联率" DataIndex="Rate" Align="Left" />
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:CellSelectionModel ID="GridRowSelectionModel61" runat="server" />
                                        </SelectionModel>
                                        <View>
                                            <ext:LockingGridView ID="LockingGridView61" runat="server" ForceFit="true"/>
                                        </View>
                                        <Listeners>
                                            <CellDblClick Fn="SysKPI.onGridCellDblClick61" Scope="SysKPI" />
                                        </Listeners>
                                        <LoadMask ShowMask="true" />
                                        <BottomBar>
                                            <ext:PagingToolbar ID="BottomPagingToolbar61" runat="server" PageSize="20" StoreID="GridStore61">
                                                <Items>
                                                    <ext:ComboBox ID="PageSizeComboBox61" runat="server" Width="150" LabelWidth="60" FieldLabel="<%$ Resources: GlobalResource, ComboBoxPageSize %>">
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
                                                            <Select Handler="#{BottomPagingToolbar61}.pageSize = parseInt(this.getValue()); #{BottomPagingToolbar61}.doLoad();" />
                                                        </Listeners>
                                                    </ext:ComboBox>
                                                </Items>
                                            </ext:PagingToolbar>
                                        </BottomBar>
                                    </ext:GridPanel>
                                </Items>
                            </ext:Panel>
                        </Items>
                    </ext:GroupTab>
                </Groups>
            </ext:GroupTabPanel>
        </Items>
    </ext:Viewport>
    <ext:Window ID="SettingWindow" runat="server" Icon="TableEdit" Title="参考值设置" Height="150" Width="300" Modal="true" InitCenter="true" Hidden="true">
        <Items>
            <ext:FormPanel ID="SettingFormPanel" runat="server" Border="false" LabelWidth="70" Height="120" Width="285" Padding="10" ButtonAlign="Right">
                <Items>
                    <ext:NumberField ID="StandardPercent" runat="server" AnchorHorizontal="95%" FieldLabel="达标值(%)" EmptyText="例:达标值为60%,则输入60" AllowBlank="false" MinValue="1" MaxValue="100" />
                    <ext:NumberField ID="ExcellentPercent" runat="server" AnchorHorizontal="95%" FieldLabel="优秀值(%)" EmptyText="例:优秀值为90%,则输入90" AllowBlank="false" MinValue="1" MaxValue="100" />
                </Items>
                <Buttons>
                    <ext:Button ID="DoButton" runat="server" Text="确定">
                        <DirectEvents>
                            <Click Before="return #{SettingFormPanel}.getForm().isValid();" OnEvent="DoButton_Click"/>
                        </DirectEvents>
                    </ext:Button>
                    <ext:Button ID="CancelButton" runat="server" Text="取消">
                        <Listeners>
                            <Click Handler="#{SettingWindow}.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    </form>
</body>
</html>
