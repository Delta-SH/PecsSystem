<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SpeechSetting.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.SpeechSetting" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>语音配置</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <style type="text/css">
        .x-form-group .x-form-group-header-text {
            font: normal bold 12px/20px arial; 
        }
    </style>
    <script type="text/javascript" src="Resources/js/public.js?v=3.3.0.0"></script>
    <script type="text/javascript">
        //<![CDATA[
        Ext.onReady(function() {
            SpSetting.CreateTreeNodes();
        });
        //]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="SpSettingViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:Panel ID="SpCenterPanel" runat="server" Title="Lsc列表" Margins="10 0 10 10" Padding="5" Region="Center">
                <Items>
                    <ext:TreePanel ID="ColumnTreePanel" runat="server" RootVisible="false" AutoScroll="true"
                        ContainerScroll="true" Animate="true" EnableDD="true" Height="332" OnSubmit="SubmitNodes">
                        <Root>
                            <ext:TreeNode Text="ColumnNames" />
                        </Root>
                        <Listeners>
                            <Click Fn="SpSetting.OnSpTreeItemClick" Scope="SpSetting" />
                        </Listeners>
                    </ext:TreePanel>
                </Items>
                <BottomBar>
                    <ext:Toolbar ID="SpCenterToolbar" runat="server">
                        <Items>
                            <ext:Button ID="ResetBtn" runat="server" Text="重置" Icon="ArrowRotateAnticlockwise">
                                <Listeners>
                                    <Click Fn="SpSetting.CreateTreeNodes" Buffer="500" Scope="SpSetting" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill ID="SpCenterToolbarFill" runat="server" />
                            <ext:Button ID="SaveBtn" runat="server" Text="保存" Icon="Disk">
                                <Listeners>
                                    <Click Handler="#{ColumnTreePanel}.submitNodes();" Buffer="500" />
                                </Listeners>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </BottomBar>
            </ext:Panel>
            <ext:Panel ID="SpSettingEastPanel" runat="server" Title="属性选项" Margins="10 10 10 0"
                Padding="5" Width="312" Split="true" Collapsible="false" CollapseMode="Mini"
                Region="East">
                <Items>
                    <ext:Checkbox ID="DisConnectEnabled" runat="server" BoxLabel="服务器通信中断告警播报" Height="25"
                        StyleSpec="margin-left:5px;">
                        <Listeners>
                            <Check Fn="SpSetting.DisConnectCheck" Scope="SpSetting" />
                        </Listeners>
                    </ext:Checkbox>
                    <ext:Panel ID="ALPanel" runat="server" Title="告警级别播报" AutoHeight="true" FormGroup="true"
                        Padding="5">
                        <Items>
                            <ext:CheckboxGroup ID="ALItems" runat="server" ColumnsNumber="2">
                                <Items>
                                    <ext:Checkbox ID="AL1Enabled" runat="server" BoxLabel="一级告警" Height="25">
                                        <Listeners>
                                            <Check Fn="SpSetting.AL1Check" Scope="SpSetting" />
                                        </Listeners>
                                    </ext:Checkbox>
                                    <ext:Checkbox ID="AL2Enabled" runat="server" BoxLabel="二级告警" Height="25">
                                        <Listeners>
                                            <Check Fn="SpSetting.AL2Check" Scope="SpSetting" />
                                        </Listeners>
                                    </ext:Checkbox>
                                    <ext:Checkbox ID="AL3Enabled" runat="server" BoxLabel="三级告警" Height="25">
                                        <Listeners>
                                            <Check Fn="SpSetting.AL3Check" Scope="SpSetting" />
                                        </Listeners>
                                    </ext:Checkbox>
                                    <ext:Checkbox ID="AL4Enabled" runat="server" BoxLabel="四级告警">
                                        <Listeners>
                                            <Check Fn="SpSetting.AL4Check" Scope="SpSetting" />
                                        </Listeners>
                                    </ext:Checkbox>
                                </Items>
                            </ext:CheckboxGroup>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="FilterPanel" runat="server" Title="告警播报筛选" AutoHeight="true" FormGroup="true"
                        Padding="5">
                        <Items>
                            <ext:TextField ID="DevNameField" runat="server" FieldLabel="设备名称" LabelWidth="65"
                                Width="260">
                                <Listeners>
                                    <Change Fn="SpSetting.DevFilter" Scope="SpSetting" />
                                </Listeners>
                            </ext:TextField>
                            <ext:TextField ID="NodeNameField" runat="server" FieldLabel="测点名称" LabelWidth="65"
                                Width="260">
                                <Listeners>
                                    <Change Fn="SpSetting.NodeFilter" Scope="SpSetting" />
                                </Listeners>
                            </ext:TextField>
                            <ext:Checkbox ID="LoopEnabled" runat="server" BoxLabel="循环播报" LabelWidth="65" FieldLabel="&nbsp;"
                                LabelSeparator=" ">
                                <Listeners>
                                    <Check Fn="SpSetting.LoopCheck" Scope="SpSetting" />
                                </Listeners>
                            </ext:Checkbox>
                        </Items>
                    </ext:Panel>
                    <ext:Panel ID="SpeakerPanel" runat="server" Title="告警播报选项" AutoHeight="true" FormGroup="true"
                        Padding="5">
                        <Items>
                            <ext:CheckboxGroup ID="SpeakerItems" runat="server" ColumnsNumber="3">
                                <Items>
                                    <ext:Checkbox ID="SpArea2" runat="server" BoxLabel="所属地区">
                                        <Listeners>
                                            <Check Fn="SpSetting.Area2Check" Scope="SpSetting" />
                                        </Listeners>
                                    </ext:Checkbox>
                                    <ext:Checkbox ID="SpArea3" runat="server" BoxLabel="所属县市">
                                        <Listeners>
                                            <Check Fn="SpSetting.Area3Check" Scope="SpSetting" />
                                        </Listeners>
                                    </ext:Checkbox>
                                    <ext:Checkbox ID="SpStation" runat="server" BoxLabel="所属局站">
                                        <Listeners>
                                            <Check Fn="SpSetting.StationCheck" Scope="SpSetting" />
                                        </Listeners>
                                    </ext:Checkbox>
                                    <ext:Checkbox ID="SpDevice" runat="server" BoxLabel="所属设备">
                                        <Listeners>
                                            <Check Fn="SpSetting.DeviceCheck" Scope="SpSetting" />
                                        </Listeners>
                                    </ext:Checkbox>
                                    <ext:Checkbox ID="SpNode" runat="server" BoxLabel="测点名称">
                                        <Listeners>
                                            <Check Fn="SpSetting.NodeCheck" Scope="SpSetting" />
                                        </Listeners>
                                    </ext:Checkbox>
                                    <ext:Checkbox ID="SpALDesc" runat="server" BoxLabel="告警等级">
                                        <Listeners>
                                            <Check Fn="SpSetting.ALDescCheck" Scope="SpSetting" />
                                        </Listeners>
                                    </ext:Checkbox>
                                </Items>
                            </ext:CheckboxGroup>
                        </Items>
                    </ext:Panel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    </form>
</body>
</html>
