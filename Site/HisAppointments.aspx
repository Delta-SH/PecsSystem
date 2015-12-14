<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HisAppointments.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.HisAppointments" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预约查询</title>
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
            X.HisAppointments.ShowDetailWindow(lscId, projectId);
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="HisAppointmentsViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="HisAppointmentsGridPanel" runat="server" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Region="Center" Height="500px" Title="历史预约" Icon="ApplicationViewList">
                <TopBar>
                    <ext:Toolbar ID="HisAppointmentsGridToolbars" runat="server" Layout="ContainerLayout">
                        <Items>
                            <ext:Toolbar ID="HisAppointmentsGridToolbar1" runat="server" Flat="true">
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
                                                    <Load Handler="if(this.getCount() > 0){ #{LscsComboBox}.setValueAndFireSelect(this.getAt(0).get('Id')); }" />
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
                            <ext:Toolbar ID="HisAppointmentsGridToolbar2" runat="server" Flat="true">
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
                                                    <ext:MenuItem ID="SaveMenuItem" runat="server" Text="打印/导出" Icon="Printer">
                                                        <DirectEvents>
                                                            <Click OnEvent="SaveBtn_Click" IsUpload="true">
                                                                <ExtraParams>
                                                                    <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{HisAppointmentsGridPanel})" />
                                                                </ExtraParams>
                                                            </Click>
                                                        </DirectEvents>
                                                    </ext:MenuItem>
                                                </Items>
                                            </ext:Menu>
                                        </Menu>
                                        <DirectEvents>
                                            <Click OnEvent="QueryBtn_Click" Success="#{HisAppointmentsGridPagingToolbar}.doLoad();">
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{HisAppointmentsGridPanel}.body.up('div')" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:SplitButton>
                                </Items>
                            </ext:Toolbar>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Store>
                    <ext:Store ID="HisAppointmentsGridStore" runat="server" AutoLoad="false" OnRefreshData="OnHisAppointmentsGridRefresh">
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
                                    <ext:RecordField Name="RecordTime" Type="String" />
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
                <ColumnModel ID="HisAppointmentsGridColumnModel" runat="server">
                    <Columns>
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
                        <ext:Column Header="归档时间" DataIndex="RecordTime" Align="Center" />
                    </Columns>
                </ColumnModel>
                <SelectionModel>
                    <ext:RowSelectionModel ID="HisAppointmentsGridRowSelectionModel" runat="server" SingleSelect="true" />
                </SelectionModel>
                <View>
                    <ext:GridView ID="HisAppointmentsGridView" runat="server" ForceFit="false" />
                </View>
                <BottomBar>
                    <ext:PagingToolbar ID="HisAppointmentsGridPagingToolbar" runat="server" PageSize="20"
                        StoreID="HisAppointmentsGridStore">
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
                                    <Select Handler="#{HisAppointmentsGridPagingToolbar}.pageSize = parseInt(this.getValue()); #{HisAppointmentsGridPagingToolbar}.doLoad();" />
                                </Listeners>
                            </ext:ComboBox>
                        </Items>
                    </ext:PagingToolbar>
                </BottomBar>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
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
