<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppointmentManager.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.AppointmentManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预约管理</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link rel="icon" type="image/x-icon" href="favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="favicon.ico" />
    <script type="text/javascript" src="Resources/js/public.js?v=3.3.0.0"></script>
    <script type="text/javascript">
        var ProjectRenderer = function (value, meta, record) {
            return String.format("<a href='javascript:void(0)' style='cursor:pointer;' onclick=\"javascript:ShowDetailWindow({0},'{1}')\">{2}</a>", record.data.LscID, record.data.ProjectId, record.data.ProjectName);
        };

        var ShowDetailWindow = function (lscId, projectId) {
            X.AppointmentManager.ShowDetailWindow(lscId, projectId);
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="AppointmentViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="AppointmentGridPanel" runat="server" StripeRows="true" TrackMouseOver="true"
                AutoScroll="true" Region="Center" Height="500px" Title="预约管理" Icon="ApplicationViewList">
                <TopBar>
                    <ext:Toolbar ID="AppointmentGridToolbars" runat="server" Layout="ContainerLayout">
                        <Items>
                            <ext:Toolbar ID="AppointmentGridToolbar1" runat="server" Flat="true">
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
                                                    <Load Handler="if(this.getCount() > 0){ #{LscsComboBox}.setValue(this.getAt(0).get('Id')); }" />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                    <ext:TriggerField ID="BeginDate" runat="server" Width="180" LabelWidth="50" FieldLabel="预约时间">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Date" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="WdatePicker({el:'BeginDate',isShowClear:false,readOnly:true,isShowWeek:true,onpicked:function(){#{EndDate}.fireEvent('triggerclick')}});" />
                                        </Listeners>
                                    </ext:TriggerField>
                                    <ext:TriggerField ID="EndDate" runat="server" Width="180" LabelWidth="50" FieldLabel="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" LabelSeparator="-">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="Date" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="WdatePicker({el:'EndDate',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'BeginDate\')}'});" />
                                        </Listeners>
                                    </ext:TriggerField>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar ID="AppointmentGridToolbar2" runat="server" Flat="true">
                                <Items>
                                    <ext:ComboBox ID="QueryTypeComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="筛选预约" ForceSelection="true" SelectOnFocus="true" Resizable="true">
                                        <Items>
                                            <ext:ListItem Text="按工程编号" Value="0" />
                                            <ext:ListItem Text="按工程名称" Value="1" />
                                            <ext:ListItem Text="按Lsc编号" Value="2" />
                                            <ext:ListItem Text="按局站编号" Value="3" />
                                            <ext:ListItem Text="按设备编号" Value="4" />
                                        </Items>
                                        <SelectedItem Value="0" />
                                    </ext:ComboBox>
                                    <ext:TextField ID="QueryContentTextField" runat="server" Width="360" EmptyText="请输入筛选条件..." />
                                    <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5" />
                                    <ext:SplitButton ID="QueryBtn" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                        <Menu>
                                            <ext:Menu ID="QueryMenu1" runat="server">
                                                <Items>
                                                    <ext:MenuItem ID="AddMenuItem" runat="server" Text="新增预约" Icon="Add">
                                                        <Listeners>
                                                            <Click Fn="AppointMgr.onGridAddClick" Scope="AppointMgr" />
                                                        </Listeners>
                                                    </ext:MenuItem>
                                                    <ext:MenuSeparator ID="MenuSeparator1" runat="server" />
                                                    <ext:MenuItem ID="SaveMenuItem" runat="server" Text="打印/导出" Icon="Printer">
                                                        <DirectEvents>
                                                            <Click OnEvent="SaveBtn_Click" IsUpload="true">
                                                                <ExtraParams>
                                                                    <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{AppointmentGridPanel})" />
                                                                </ExtraParams>
                                                            </Click>
                                                        </DirectEvents>
                                                    </ext:MenuItem>
                                                </Items>
                                            </ext:Menu>
                                        </Menu>
                                        <DirectEvents>
                                            <Click OnEvent="QueryBtn_Click" Success="#{AppointmentGridPagingToolbar}.doLoad();">
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{AppointmentGridPanel}.body.up('div')" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:SplitButton>
                                </Items>
                            </ext:Toolbar>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store ID="AppointmentGridStore" runat="server" AutoLoad="false" OnRefreshData="OnAppointmentGridRefresh">
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                        <Reader>
                            <ext:JsonReader IDProperty="Index">
                                <Fields>
                                    <ext:RecordField Name="Index" Type="Int" />
                                    <ext:RecordField Name="LscID" Type="Int" />
                                    <ext:RecordField Name="LscName" Type="String" />
                                    <ext:RecordField Name="Id" Type="Int" />
                                    <ext:RecordField Name="StartTime" Type="String" />
                                    <ext:RecordField Name="EndTime" Type="String" />
                                    <ext:RecordField Name="LscIncluded" Type="String" />
                                    <ext:RecordField Name="StaIncluded" Type="String" />
                                    <ext:RecordField Name="DevIncluded" Type="String" />
                                    <ext:RecordField Name="ProjectId" Type="String" />
                                    <ext:RecordField Name="ProjectName" Type="String" />
                                    <ext:RecordField Name="Creater" Type="String" />
                                    <ext:RecordField Name="ContactPhone" Type="String" />
                                    <ext:RecordField Name="CreatedTime" Type="String" />
                                </Fields>
                            </ext:JsonReader>
                        </Reader>
                        <BaseParams>
                            <ext:Parameter Name="start" Value="0" Mode="Raw" />
                            <ext:Parameter Name="limit" Value="#{PageSizeComboBox}.getValue()" Mode="Raw" />
                        </BaseParams>
                    </ext:Store>
                </Store>
                <LoadMask ShowMask="true" />
                <ColumnModel ID="AppointmentGridColumnModel" runat="server">
                    <Columns>
                        <ext:CommandColumn Header="操作" Align="Center" ButtonAlign="Center">
                            <Commands>
                                <ext:GridCommand Icon="NoteEdit" Text="编辑" CommandName="Edit">
                                    <ToolTip Text="编辑预约信息" />
                                </ext:GridCommand>
                            </Commands>
                        </ext:CommandColumn>
                        <ext:Column Header="序号" DataIndex="Index" Align="Left" />
                        <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" />
                        <ext:Column Header="工程名称" DataIndex="ProjectName" Align="Left">
                            <Renderer Fn="ProjectRenderer" />
                        </ext:Column>
                        <ext:Column Header="预约Lsc" DataIndex="LscIncluded" Align="Left" />
                        <ext:Column Header="预约局站" DataIndex="StaIncluded" Align="Left" />
                        <ext:Column Header="预约设备" DataIndex="DevIncluded" Align="Left" />
                        <ext:Column Header="开始时间" DataIndex="StartTime" Align="Center" />
                        <ext:Column Header="结束时间" DataIndex="EndTime" Align="Center" />
                        <ext:Column Header="创建人员" DataIndex="Creater" Align="Left" />
                        <ext:Column Header="联系电话" DataIndex="ContactPhone" Align="Left" />
                        <ext:Column Header="创建时间" DataIndex="CreatedTime" Align="Center" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="AppointmentGridRowSelectionModel" runat="server" SingleSelect="true" />
                </SelectionModel>
                <View>
                    <ext:GridView ID="AppointmentGridView" runat="server" ForceFit="false" />
                </View>
                <Listeners>
                    <Command Fn="AppointMgr.onGridCmdClick" Scope="AppointMgr" />
                </Listeners>
                <BottomBar>
                    <ext:PagingToolbar ID="AppointmentGridPagingToolbar" runat="server" PageSize="20" StoreID="AppointmentGridStore">
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
                                    <Select Handler="#{AppointmentGridPagingToolbar}.pageSize = parseInt(this.getValue()); #{AppointmentGridPagingToolbar}.doLoad();" />
                                </Listeners>
                            </ext:ComboBox>
                        </Items>
                    </ext:PagingToolbar>
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    <ext:Window ID="AppointmentWindow" runat="server" Width="600" Height="300" Modal="true" Hidden="true" Layout="FitLayout">
        <Items>
            <ext:FormPanel ID="AppointmentFormPanel" runat="server" Header="false" Padding="10" ButtonAlign="Right" Layout="ColumnLayout" MonitorValid="true">
                <Items>
                    <ext:Panel ID="LeftPanel" runat="server" Border="false" Header="false" ColumnWidth=".5" Layout="FormLayout" LabelWidth="80">
                        <Defaults>
                            <ext:Parameter Name="MsgTarget" Value="side" />
                        </Defaults>
                        <Items>
                            <ext:ComboBox ID="WLscsComboBox" runat="server" FieldLabel="Lsc名称" AnchorHorizontal="95%"
                                Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                                ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true"
                                Resizable="true">
                                <Store>
                                    <ext:Store ID="WLscsStore" runat="server" AutoLoad="true" OnRefreshData="OnWLscsRefresh">
                                        <Proxy>
                                            <ext:PageProxy />
                                        </Proxy>
                                        <Reader>
                                            <ext:JsonReader IDProperty="Id">
                                                <Fields>
                                                    <ext:RecordField Name="Id" Type="Int" />
                                                    <ext:RecordField Name="Name" Type="String" />
                                                    <ext:RecordField Name="LscUserID" Type="Int" />
                                                    <ext:RecordField Name="LscUserName" Type="String" />
                                                    <ext:RecordField Name="LscUserMobilePhone" Type="String" />
                                                </Fields>
                                            </ext:JsonReader>
                                        </Reader>
                                        <Listeners>
                                            <Load Handler="if(this.getCount() > 0){ #{WLscsComboBox}.setValueAndFireSelect(this.getAt(0).get('Id')); }" />
                                        </Listeners>
                                    </ext:Store>
                                </Store>
                                <Listeners>
                                    <Select Fn="AppointMgr.LscChanged" Scope="AppointMgr" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:DropDownField ID="StaIncludedField" runat="server" FieldLabel="预约局站" Editable="false" TriggerIcon="Combo" Mode="ValueText" AnchorHorizontal="95%">
                                <Component>
                                    <ext:TreePanel ID="StaTreePanel" runat="server" Header="false" Lines="true" AutoScroll="true" Animate="true" EnableDD="true" ContainerScroll="true" RootVisible="false" Height="300">
                                        <Root>
                                            <ext:TreeNode />
                                        </Root>
                                        <Loader>
                                            <ext:PageTreeLoader OnNodeLoad="StaIncludedLoaded" />
                                        </Loader>
                                        <Listeners>
                                            <CheckChange Handler="
                                            var values = this.dropDownField.getValue().split(';');
                                            for(var i in values) {
                                                if(Ext.isEmpty(values[i], false)) {
                                                    values.splice(i, 1);
                                                }
                                            }
                                            
                                            if(checked) {
                                                if(values.indexOf(node.id) === -1){
                                                    values.push(node.id);
                                                }
                                            } else {
                                                values.remove(node.id);
                                            }
                                            
                                            var nv = values.join(';');
                                            this.dropDownField.setValue(nv, nv, false);" />
                                        </Listeners>
                                        <SelectionModel>
                                            <ext:MultiSelectionModel ID="MultiSelectionModel1" runat="server" />
                                        </SelectionModel>
                                    </ext:TreePanel>
                                </Component>
                                <Listeners>
                                    <Expand Fn="AppointMgr.initStaTreePanel" Scope="AppointMgr" Single="true" />
                                </Listeners>
                            </ext:DropDownField>
                            <ext:TriggerField ID="StartTimeTextField" runat="server" AnchorHorizontal="95%" FieldLabel="开始时间" AllowBlank="false">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Date" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="WdatePicker({el:'StartTimeTextField',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'%y-%M-%d %H:%m:%s',onpicked:function(){#{EndTimeTextField}.fireEvent('triggerclick')}});" />
                                </Listeners>
                            </ext:TriggerField>
                            <ext:TextField ID="BookingUserNameTextField" runat="server" FieldLabel="创建人员" AnchorHorizontal="95%" MaxLength="40" AllowBlank="false" Disabled="true" />
                            <ext:Checkbox ID="LscCheckbox" runat="server" FieldLabel="预约Lsc" BoxLabel="(勾选表示预约整个Lsc)" AnchorHorizontal="95%" />
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="RightPanel" runat="server" Border="false" Header="false" ColumnWidth=".5" Layout="FormLayout" LabelWidth="80">
                        <Defaults>
                            <ext:Parameter Name="MsgTarget" Value="side" />
                        </Defaults>
                        <Items>
                            <ext:ComboBox ID="WProjectComboBox" runat="server" FieldLabel="预约工程" AnchorHorizontal="95%" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="select an item..." AllowBlank="false" SelectOnFocus="true" Resizable="true">
                                <Store>
                                    <ext:Store ID="WProjectStore" runat="server" AutoLoad="true" OnRefreshData="OnWProjectRefresh">
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
                                            <Load Fn="AppointMgr.ProjectLoaded" Scope="AppointMgr" />
                                        </Listeners>
                                    </ext:Store>
                                </Store>
                                <Listeners>
                                    <Select Fn="AppointMgr.ProjectChanged" Scope="AppointMgr" />
                                </Listeners>
                            </ext:ComboBox>
                            <ext:DropDownField ID="DevIncludedField" runat="server" FieldLabel="预约设备" Editable="false" TriggerIcon="Combo" Mode="ValueText" AnchorHorizontal="95%">
                                <Component>
                                    <ext:TreePanel ID="DevTreePanel" runat="server" Header="false" Lines="true" AutoScroll="true"
                                        Animate="true" EnableDD="true" ContainerScroll="true" RootVisible="false" Height="300">
                                        <Root>
                                            <ext:TreeNode />
                                        </Root>
                                        <Loader>
                                            <ext:PageTreeLoader OnNodeLoad="DevIncludedLoaded" />
                                        </Loader>
                                        <Listeners>
                                            <CheckChange Handler="
                                            var values = this.dropDownField.getValue().split(';');
                                            for(var i in values) {
                                                if(Ext.isEmpty(values[i], false)) {
                                                    values.splice(i, 1);
                                                }
                                            }
                                            
                                            if(checked) {
                                                if(values.indexOf(node.id) === -1){
                                                    values.push(node.id);
                                                }
                                            } else {
                                                values.remove(node.id);
                                            }
                                            
                                            var nv = values.join(';');
                                            this.dropDownField.setValue(nv, nv, false);" />
                                        </Listeners>
                                        <SelectionModel>
                                            <ext:MultiSelectionModel ID="MultiSelectionModel2" runat="server" />
                                        </SelectionModel>
                                    </ext:TreePanel>
                                </Component>
                                <Listeners>
                                    <Expand Fn="AppointMgr.initDevTreePanel" Scope="AppointMgr" Single="true" />
                                </Listeners>
                            </ext:DropDownField>
                            <ext:TriggerField ID="EndTimeTextField" runat="server" AnchorHorizontal="95%" FieldLabel="结束时间" AllowBlank="false">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Date" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="WdatePicker({el:'EndTimeTextField',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'StartTimeTextField\')}'});" />
                                </Listeners>
                            </ext:TriggerField>
                            <ext:TextField ID="BookingUserPhoneTextField" runat="server" FieldLabel="联系电话" AnchorHorizontal="95%" MaxLength="20" AllowBlank="true" />
                            <ext:Checkbox ID="ProjStatusBox" runat="server" FieldLabel="其他选项" BoxLabel="停用工程预约" AnchorHorizontal="95%" />
                        </Items>
                    </ext:Panel>
                </Items>
                <Buttons>
                    <ext:Button ID="SaveBtn" runat="server" Text="保存">
                        <Listeners>
                            <Click Fn="AppointMgr.Save" Scope="AppointMgr" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="CancelBtn" runat="server" Text="关闭">
                        <Listeners>
                            <Click Handler="#{AppointmentWindow}.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <BottomBar>
                    <ext:StatusBar ID="AppointmentStatusBar" runat="server" Height="25" />
                </BottomBar>
            </ext:FormPanel>
            <ext:Hidden ID="JsonValueHF" runat="server" />
        </Items>
    </ext:Window>
    <ext:Window ID="DetailWindow" runat="server" Width="400" Height="325" Modal="true" Hidden="true" Title="工程详细信息" Layout="FitLayout">
        <Items>
            <ext:Panel ID="DetailPanel" runat="server" Header="false" Padding="10" ButtonAlign="Right" Layout="FormLayout" LabelWidth="80">
                <Items>
                    <ext:DisplayField ID="ProjectIdField" runat="server" FieldLabel="工程编号"/>
                    <ext:DisplayField ID="ProjNameField" runat="server" FieldLabel="工程名称" />
                    <ext:DisplayField ID="StartTimeField" runat="server" FieldLabel="开始时间" />
                    <ext:DisplayField ID="EndTimeField" runat="server" FieldLabel="结束时间" />
                    <ext:DisplayField ID="ResponsibleField" runat="server" FieldLabel="负责人员" />
                    <ext:DisplayField ID="PhoneField" runat="server" FieldLabel="联系电话" />
                    <ext:DisplayField ID="CompanyField" runat="server" FieldLabel="施工公司" />
                    <ext:DisplayField ID="CommentField" runat="server" FieldLabel="备注"/>
                    <ext:DisplayField ID="EnabledField" runat="server" FieldLabel="启用状态" />
                </Items>
                <Buttons>
                    <ext:Button ID="DetailCancelBtn" runat="server" Text="关闭">
                        <Listeners>
                            <Click Handler="#{DetailWindow}.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
            </ext:Panel>
        </Items>
    </ext:Window>
    </form>
</body>
</html>
