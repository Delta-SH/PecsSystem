<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RealTimeCurve.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.RealTimeCurve" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>实时曲线</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link rel="icon" type="image/x-icon" href="favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="favicon.ico" />
    <script type="text/javascript" src="Resources/amcharts/amcharts.js?v=3.3.0.0"></script>
    <script type="text/javascript" src="Resources/js/amcharts.js?v=3.3.0.0"></script>
    <script type="text/javascript">
        //<![CDATA[
        AmCharts.ready(function() {
            RTCurve.render();
        });
        Ext.onReady(function() {
            RTCurve.updateTitle();
        });
        //]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" IDMode="Explicit" DirectMethodNamespace="X" />
    <ext:Viewport ID="RealTimeCurveViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:Panel ID="ChartPanel" runat="server" AutoScroll="true" Region="Center" Title="实时曲线"
                Icon="ChartCurve">
                <TopBar>
                    <ext:Toolbar ID="RealTimeCurveToolbar" runat="server">
                        <Items>
                            <ext:Button ID="btnStart" runat="server" Text="启动刷新" Icon="ControlPlayBlue">
                                <Listeners>
                                    <Click Handler="this.disable();#{RealTimeCurveTaskManager}.startTask(0);#{btnStop}.enable()" />
                                </Listeners>
                            </ext:Button>
                            <ext:Button ID="btnStop" runat="server" Text="停止刷新" Icon="ControlStopBlue" Disabled="true">
                                <Listeners>
                                    <Click Handler="this.disable();#{RealTimeCurveTaskManager}.stopTask(0);#{btnStart}.enable();" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Content>
                    <div id="chartContainer">
                    </div>
                </Content>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    <ext:TaskManager ID="RealTimeCurveTaskManager" runat="server">
        <Tasks>
            <ext:Task AutoRun="false" Interval="5000">
                <Listeners>
                    <Update Fn="RTCurve.appendData" />
                </Listeners>
            </ext:Task>
        </Tasks>
    </ext:TaskManager>
    </form>
</body>
</html>
