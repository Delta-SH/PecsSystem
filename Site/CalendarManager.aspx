<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalendarManager.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.CalendarManager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>日历管理</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link rel="icon" type="image/x-icon" href="favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="favicon.ico" />
    <script type="text/javascript" src="Resources/js/calendar.js?v=3.3.0.0"></script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IDMode="Explicit" InitScriptMode="Linked" Namespace="CompanyX" DirectMethodNamespace="X"/>
    <ext:Viewport ID="CalendarViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:Panel ID="MainPanel" runat="server" Title="日历管理" Layout="BorderLayout" Region="Center" Cls="app-center">
                <Items>
                    <ext:Panel ID="LeftNavPanel" runat="server" Width="180" Region="West" Border="false" Cls="app-west">
                        <Items>
                            <ext:DatePicker ID="CalendarDatePicker" runat="server" Cls="ext-cal-nav-picker">
                                <Listeners>
                                    <Select Fn="CompanyX.setStartDate" Scope="CompanyX" />
                                </Listeners>
                            </ext:DatePicker>
                        </Items>
                    </ext:Panel>
                    <ext:CalendarPanel ID="RightNavCalendarPanel" runat="server" Region="Center" ActiveIndex="2" Border="false">
                        <GroupStore ID="CalendarGroupStore" runat="server">
                            <Groups>
                                <ext:Group CalendarId="1" Title="重要" />
                                <ext:Group CalendarId="2" Title="普通" />
                                <ext:Group CalendarId="3" Title="其他" />
                            </Groups>
                        </GroupStore>
                        <EventStore ID="CalendarEventStore" runat="server" OnSubmitData="CalendarEventStore_SubmitData">
                            <Reader>
                                <ext:JsonReader IDProperty="EventId">
                                    <Fields>
                                        <ext:RecordField Name="EventId" Type="Int"/>
                                        <ext:RecordField Name="CalendarId" Type="Int"/>
                                        <ext:RecordField Name="Title" Type="String"/>
                                        <ext:RecordField Name="StartDate" Type="Date" />
                                        <ext:RecordField Name="EndDate" Type="Date" />
                                        <ext:RecordField Name="IsAllDay" Type="Boolean"/>
                                        <ext:RecordField Name="Location" Type="String"/>
                                        <ext:RecordField Name="Notes" Type="String"/>
                                        <ext:RecordField Name="Url" Type="String"/>
                                        <ext:RecordField Name="Reminder" Type="String"/>
                                        <ext:RecordField Name="IsNew" Type="Boolean"/>
                                    </Fields>
                                </ext:JsonReader>
                            </Reader>
                        </EventStore>
                        <DayView ID="CalendarDayView" runat="server" />
                        <WeekView ID="CalendarWeekView" runat="server" />
                        <MonthView ID="CalendarMonthView" runat="server" ShowHeader="true" ShowWeekLinks="true" ShowWeekNumbers="true" />
                        <EventEditForm ID="CalendarEditForm" runat="server" />
                        <Listeners>
                            <Render Fn="CompanyX.localize.editForm" Scope="CompanyX" />
                            <ViewChange Fn="CompanyX.viewChange" Scope="CompanyX" />
                            <EventClick Fn="CompanyX.record.show" Scope="CompanyX" />
                            <DayClick Fn="CompanyX.dayClick" Scope="CompanyX" />
                            <RangeSelect Fn="CompanyX.rangeSelect" Scope="CompanyX" />
                            <EventAdd Fn="CompanyX.record.addDetail" Scope="CompanyX" />
                            <EventUpdate Fn="CompanyX.record.updateDetail" Scope="CompanyX" />
                            <EventDelete Fn="CompanyX.record.removeDetail" Scope="CompanyX"/>
                            <EventMove Fn="CompanyX.record.move" Scope="CompanyX" />
                            <EventResize Fn="CompanyX.record.resize" Scope="CompanyX" />
                        </Listeners>
                    </ext:CalendarPanel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    <ext:EventEditWindow ID="CalendarEventEditWindow" runat="server" Hidden="true" GroupStoreID="CalendarGroupStore">
        <Listeners>
            <Render Fn="CompanyX.localize.editWin" Scope="CompanyX" />
            <EventAdd Fn="CompanyX.record.add" Scope="CompanyX" />
            <EventUpdate Fn="CompanyX.record.update" Scope="CompanyX" />
            <EventDelete Fn="CompanyX.record.remove" Scope="CompanyX" />
            <EditDetails Fn="CompanyX.record.edit" Scope="CompanyX" />
        </Listeners>
    </ext:EventEditWindow>
    <ext:TaskManager ID="CalendarTaskManager" runat="server" Interval="10000">
        <Tasks>
            <ext:Task AutoRun="true">
                <Listeners>
                    <Update Fn="CompanyX.doRemind" />
                </Listeners>
            </ext:Task>
        </Tasks>
    </ext:TaskManager>
    </form>
</body>
</html>
