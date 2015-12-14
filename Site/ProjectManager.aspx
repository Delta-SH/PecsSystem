<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProjectManager.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.ProjectManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>工程管理</title>
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
            <ext:GridPanel ID="MainGridPanel" runat="server" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Region="Center" Height="500px" Title="工程管理" Icon="ApplicationViewList">
                <TopBar>
                    <ext:Toolbar ID="TopToolbars" runat="server" Layout="ContainerLayout">
                        <Items>
                            <ext:Toolbar ID="TopToolbar1" runat="server" Flat="true">
                                <Items>
                                    <ext:ComboBox ID="LscsComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="Lsc名称" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true" Resizable="true">
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
                                    <ext:TriggerField ID="BeginDate" runat="server" Width="180" LabelWidth="50" FieldLabel="工程时间">
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
                            <ext:Toolbar ID="TopToolbar2" runat="server" Flat="true">
                                <Items>
                                    <ext:ComboBox ID="QueryTypeComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="筛选工程" ForceSelection="true" SelectOnFocus="true" Resizable="true">
                                        <Items>
                                            <ext:ListItem Text="按工程编号" Value="0" />
                                            <ext:ListItem Text="按工程名称" Value="1" />
                                        </Items>
                                        <SelectedItem Value="0" />
                                    </ext:ComboBox>
                                    <ext:TextField ID="QueryContentTextField" runat="server" Width="360" EmptyText="请输入筛选条件..." />
                                    <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5" />
                                    <ext:SplitButton ID="QueryBtn" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                        <Menu>
                                            <ext:Menu ID="QueryMenu1" runat="server">
                                                <Items>
                                                    <ext:MenuItem ID="AddMenuItem" runat="server" Text="新增工程" Icon="Add">
                                                        <Listeners>
                                                            <Click Fn="ProjectMgr.GridAddClick" Scope="ProjectMgr" />
                                                        </Listeners>
                                                    </ext:MenuItem>
                                                    <ext:MenuSeparator ID="MenuSeparator1" runat="server" />
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
                    <ext:Store ID="MainStore" runat="server" OnRefreshData="OnMainStoreRefresh" AutoSave="true">
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                        <Reader>
                            <ext:JsonReader IDProperty="ProjectId">
                                <Fields>
                                    <ext:RecordField Name="LscID" Type="Int" />
                                    <ext:RecordField Name="LscName" Type="String" />
                                    <ext:RecordField Name="ProjectId" Type="String" />
                                    <ext:RecordField Name="ProjectName" Type="String" />
                                    <ext:RecordField Name="BeginTime" Type="String" />
                                    <ext:RecordField Name="EndTime" Type="String" />
                                    <ext:RecordField Name="Responsible" Type="String" />
                                    <ext:RecordField Name="ContactPhone" Type="String" />
                                    <ext:RecordField Name="Company" Type="String" />
                                    <ext:RecordField Name="Comment" Type="String" />
                                    <ext:RecordField Name="Enabled" Type="Boolean" />
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
                <ColumnModel ID="ColumnModel" runat="server">
                    <Columns>
                        <ext:CommandColumn Header="操作" Align="Center" ButtonAlign="Center" Width="120">
                            <Commands>
                                <ext:GridCommand Icon="NoteEdit" Text="编辑" CommandName="Edit">
                                    <ToolTip Text="编辑行" />
                                </ext:GridCommand>
                                <ext:GridCommand Icon="Delete" Text="删除" CommandName="Del">
                                    <ToolTip Text="删除行" />
                                </ext:GridCommand>
                            </Commands>
                        </ext:CommandColumn>
                        <ext:Column Header="Lsc名称" DataIndex="LscName" Align="Left" />
                        <ext:Column Header="工程编号" DataIndex="ProjectId" Align="Left" />
                        <ext:Column Header="工程名称" DataIndex="ProjectName" Align="Left" />
                        <ext:Column Header="开始时间" DataIndex="BeginTime" Align="Left" />
                        <ext:Column Header="结束时间" DataIndex="EndTime" Align="Left" />
                        <ext:Column Header="负责人员" DataIndex="Responsible" Align="Left" />
                        <ext:Column Header="联系电话" DataIndex="ContactPhone" Align="Left" />
                        <ext:Column Header="施工公司" DataIndex="Company" Align="Left" />
                        <ext:Column Header="备注" DataIndex="Comment" Align="Left" />
                        <ext:CheckColumn Header="状态" DataIndex="Enabled" Align="Center" Width="60" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="RowSelectionModel" runat="server" SingleSelect="true" />
                </SelectionModel>
                <View>
                    <ext:GridView ID="GridView" runat="server" ForceFit="false" />
                </View>
                <Listeners>
                    <Command Fn="ProjectMgr.GridCmdClick" Scope="ProjectMgr"/>
                </Listeners>
                <BottomBar>
                    <ext:PagingToolbar ID="MainGridPagingToolbar" runat="server" PageSize="20" StoreID="MainStore">
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
    <ext:Window ID="ProjectWindow" runat="server" Width="550" Height="280" Modal="true" Hidden="true" Layout="FitLayout">
        <Items>
            <ext:FormPanel ID="ProjectFormPanel" runat="server" Header="false" Padding="10" ButtonAlign="Right" Layout="ColumnLayout" MonitorValid="true">
                <Items>
                    <ext:Panel ID="LeftPanel" runat="server" Border="false" Header="false" ColumnWidth=".5" Layout="FormLayout" LabelWidth="80">
                        <Defaults>
                            <ext:Parameter Name="MsgTarget" Value="side" />
                        </Defaults>
                        <Items>
                            <ext:ComboBox ID="WLscsComboBox" runat="server" FieldLabel="所属Lsc" AnchorHorizontal="92%" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true" Resizable="true">
                                <Store>
                                    <ext:Store ID="WLscsStore" runat="server" AutoLoad="true" OnRefreshData="OnWLscsRefresh">
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
                                            <Load Handler="if(this.getCount() > 0){ #{WLscsComboBox}.setValueAndFireSelect(this.getAt(0).get('Id')); }" />
                                        </Listeners>
                                    </ext:Store>
                                </Store>
                            </ext:ComboBox>
                            <ext:TextField ID="ProjNameField" runat="server" FieldLabel="工程名称" AnchorHorizontal="92%" AllowBlank="false" MaxLength="100" />
                            <ext:TriggerField ID="StartTimeField" runat="server" FieldLabel="开始时间" AnchorHorizontal="92%" AllowBlank="false">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Date" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="WdatePicker({el:'StartTimeField',isShowClear:false,readOnly:true,isShowWeek:true});" />
                                </Listeners>
                            </ext:TriggerField>
                            <ext:TextField ID="ResponsibleField" runat="server" FieldLabel="负责人员" AnchorHorizontal="92%" AllowBlank="false" MaxLength="50" />
                            <ext:TextField ID="CommentField" runat="server" FieldLabel="备注" AnchorHorizontal="92%" AllowBlank="true" MaxLength="512" />
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="RightPanel" runat="server" Border="false" Header="false" ColumnWidth=".5" Layout="FormLayout" LabelWidth="80">
                        <Defaults>
                            <ext:Parameter Name="MsgTarget" Value="side" />
                        </Defaults>
                        <Items>
                            <ext:TextField ID="ProjectIdField" runat="server" FieldLabel="工程编号" AnchorHorizontal="92%" AllowBlank="false" MaxLength="50" />
                            <ext:TextField ID="CompanyField" runat="server" FieldLabel="施工公司" AnchorHorizontal="92%" AllowBlank="true" MaxLength="100" />
                            <ext:TriggerField ID="EndTimeField" runat="server" FieldLabel="结束时间" AnchorHorizontal="92%" AllowBlank="false">
                                <Triggers>
                                    <ext:FieldTrigger Icon="Date" />
                                </Triggers>
                                <Listeners>
                                    <TriggerClick Handler="WdatePicker({el:'EndTimeField',isShowClear:false,readOnly:true,isShowWeek:true,minDate:'#F{$dp.$D(\'StartTimeField\')}'});" />
                                </Listeners>
                            </ext:TriggerField>
                            <ext:TextField ID="PhoneField" runat="server" FieldLabel="联系电话" AnchorHorizontal="92%" AllowBlank="false" MaxLength="20" />
                            <ext:Checkbox ID="EnabledCheckbox" runat="server" FieldLabel="启用状态" BoxLabel="勾选表示启用" AnchorHorizontal="95%" LabelAlign="Left" />
                        </Items>
                    </ext:Panel>
                    <ext:Hidden ID="OperationWindowHF" runat="server" />
                </Items>
                <Buttons>
                    <ext:Button ID="SaveButton" runat="server" Text="保存">
                        <Listeners>
                            <Click Fn="ProjectMgr.Save" Scope="ProjectMgr" />
                        </Listeners>
                    </ext:Button>
                    <ext:Button ID="CloseButton" runat="server" Text="关闭">
                        <Listeners>
                            <Click Handler="#{ProjectWindow}.hide();" />
                        </Listeners>
                    </ext:Button>
                </Buttons>
                <BottomBar>
                    <ext:StatusBar ID="TipsStatusBar" runat="server" Height="25" />
                </BottomBar>
            </ext:FormPanel>
        </Items>
    </ext:Window>
    </form>
</body>
</html>
