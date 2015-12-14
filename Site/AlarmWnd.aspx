<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AlarmWnd.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.AlarmWnd" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>告警详单</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <script type="text/javascript" src="Resources/js/public.js?v=3.3.0.0"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="AlarmWndViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:GridPanel ID="AlarmWndGridPanel" runat="server" Icon="ApplicationViewColumns" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Border="false" Region="Center">
                <Plugins>
                    <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh1" runat="server" />
                </Plugins>
                <Store>
                    <ext:Store ID="AlarmWndStore" runat="server" AutoLoad="true" OnRefreshData="OnAlarmWndRefresh" OnSubmitData="OnAlarmWnd_Submit">
                        <Proxy>
                            <ext:PageProxy />
                        </Proxy>
                        <Reader>
                            <ext:JsonReader IDProperty="ID">
                                <Fields>
                                    <ext:RecordField Name="ID" Type="Int" />
                                    <ext:RecordField Name="LscId" Type="Int" />
                                    <ext:RecordField Name="LscName" Type="String" />
                                    <ext:RecordField Name="SerialNO" Type="Int" />
                                    <ext:RecordField Name="Area1Name" Type="String" />
                                    <ext:RecordField Name="Area2Name" Type="String" />
                                    <ext:RecordField Name="Area3Name" Type="String" />
                                    <ext:RecordField Name="Area4Name" Type="String" />
                                    <ext:RecordField Name="StaName" Type="String" />
                                    <ext:RecordField Name="DevName" Type="String" />
                                    <ext:RecordField Name="DevDesc" Type="String" />
                                    <ext:RecordField Name="NodeId" Type="Int" />
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
                        <WriteBaseParams>
                            <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{AlarmWndGridPanel})" />
                        </WriteBaseParams>
                        <DirectEventConfig IsUpload="true" />
                    </ext:Store>
                </Store>
                <ColumnModel ID="AlarmWndColumnModel" runat="server">
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
                    <ext:RowSelectionModel ID="AlarmWndRowSelectionModel" runat="server" SingleSelect="true">
                        <DirectEvents>
                            <RowSelect OnEvent="OnAlarmWndSubPanelRefresh">
                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{AlarmWndSubPanel}" />
                                <ExtraParams>
                                    <ext:Parameter Name="LscID" Value="record.data.LscId" Mode="Raw" />
                                    <ext:Parameter Name="SerialNO" Value="record.data.SerialNO" Mode="Raw" />
                                    <ext:Parameter Name="AlarmID" Value="record.data.AlarmID" Mode="Raw" />
                                    <ext:Parameter Name="ProjName" Value="record.data.ProjName" Mode="Raw" />
                                </ExtraParams>
                            </RowSelect>
                        </DirectEvents>
                    </ext:RowSelectionModel>
                </SelectionModel>
                <LoadMask ShowMask="true" />
                <View>
                    <ext:GridView ID="AlarmWndGridView" runat="server" ForceFit="false">
                        <GetRowClass Fn="AlarmWnd.setGridRowClass" />
                    </ext:GridView>
                </View>
                <Listeners>
                    <RowContextMenu Fn="AlarmWnd.showGridRowContextMenu" Scope="AlarmWnd" />
                </Listeners>
                <BottomBar>
                    <ext:PagingToolbar ID="AlarmWndGridPagingToolbar" runat="server" PageSize="20" StoreID="AlarmWndStore">
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
                                    <Select Handler="#{AlarmWndGridPagingToolbar}.pageSize = parseInt(this.getValue()); #{AlarmWndGridPagingToolbar}.doLoad();" />
                                </Listeners>
                            </ext:ComboBox>
                        </Items>
                    </ext:PagingToolbar>
                </BottomBar>
            </ext:GridPanel>
            <ext:GridPanel ID="AlarmWndSubPanel" runat="server" Header="false" Height="100px" CollapseMode="Mini" Collapsible="true" Split="true" AutoScroll="true" Border="false" Region="South">
                <Plugins>
                    <ext:GridPanelMaintainScrollPositionOnRefresh ID="GridPanelMaintainScrollPositionOnRefresh2" runat="server" />
                </Plugins>
                <Store>
                    <ext:Store ID="AlarmWndSubStore" runat="server" AutoLoad="false">
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
                <ColumnModel ID="AlarmWndSubColumnModel" runat="server">
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
                    <ext:RowSelectionModel ID="AlarmWndSubRowSelectionModel" runat="server" SingleSelect="true" />
                </SelectionModel>
                <View>
                    <ext:GridView ID="AlarmWndSubGridView" runat="server" ForceFit="false" />
                </View>
                <Listeners>
                    <Collapse Fn="AlarmWnd.collapseChecked" Scope="AlarmWnd" />
                    <Expand Fn="AlarmWnd.expandChecked" Scope="AlarmWnd" />
                </Listeners>
            </ext:GridPanel>
        </Items>
    </ext:Viewport>
    <ext:Menu ID="AlarmWndGridRowMenu" runat="server">
        <Items>
            <ext:MenuItem ID="AlarmWndGridRowItem1" runat="server" Text="告警确认" Icon="PageLightning">
                <DirectEvents>
                    <Click OnEvent="OnConfirmAlarmClick">
                        <ExtraParams>
                            <ext:Parameter Name="LscId" Value="this.parentMenu.dataRecord.data.LscId" Mode="Raw" />
                            <ext:Parameter Name="SerialNO" Value="this.parentMenu.dataRecord.data.SerialNO" Mode="Raw" />
                            <ext:Parameter Name="ConfirmMarking" Value="this.parentMenu.dataRecord.data.ConfirmMarking" Mode="Raw" />
                        </ExtraParams>
                    </Click>
                </DirectEvents>
            </ext:MenuItem>
            <ext:MenuSeparator ID="AlarmWndGridRowSeparator1" runat="server" />
            <ext:CheckMenuItem ID="AlarmWndGridRowItem2" runat="server" Text="显示告警级别色" Checked="true" CheckHandler="function(){ #{AlarmWndGridPagingToolbar}.doRefresh(); }" />
            <ext:CheckMenuItem ID="AlarmWndGridRowItem3" runat="server" Text="显示告警子表" Checked="true" CheckHandler="AlarmWnd.showSubPanel" Scope="AlarmWnd" />
            <ext:MenuSeparator ID="AlarmWndGridRowSeparator2" runat="server" />
            <ext:MenuItem ID="AlarmWndGridRowItem4" runat="server" Text="刷新列表" Icon="TableRefresh">
                <Listeners>
                    <Click Handler="#{AlarmWndGridPagingToolbar}.doRefresh();" />
                </Listeners>
            </ext:MenuItem>
            <ext:MenuSeparator ID="AlarmWndGridRowSeparator3" runat="server" />
            <ext:MenuItem ID="AlarmWndGridRowItem5" runat="server" Text="打印/导出" Icon="Printer">
                <Listeners>
                    <Click Handler="#{AlarmWndGridPanel}.submitData(false);" />
                </Listeners>
            </ext:MenuItem>
        </Items>
    </ext:Menu>
    </form>
</body>
</html>
