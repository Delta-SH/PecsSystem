<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NavMaps.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.NavMaps" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>动力环境监控中心系统 地图导航</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link rel="icon" type="image/x-icon" href="favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="favicon.ico" />
    <link rel="stylesheet" href="Resources/css/baidu.map.css?v=3.3.0.0" />
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=1.4"></script>
    <script type="text/javascript" src="Resources/js/baidu.map.js?v=3.3.0.0"></script>
    <script type="text/javascript">
        //<![CDATA[
        Ext.form.ComboBox.override({
            onFocus: function() {
                Ext.form.TriggerField.superclass.onFocus.call(this);
                if (!this.mimicing) {
                    this.wrap.addClass(this.wrapFocusClass);
                    this.mimicing = true;
                    this.doc.on('mouseup', this.mimicBlur, this, { delay: 10 });
                    if (this.monitorTab) { this.on('specialkey', this.checkTab, this); }
                }
            },
            triggerBlur: function() {
                this.mimicing = false;
                this.doc.un('mouseup', this.mimicBlur, this);
                if (this.monitorTab && this.el) { this.un('specialkey', this.checkTab, this); }
                Ext.form.TriggerField.superclass.onBlur.call(this);
                if (this.wrap) { this.wrap.removeClass(this.wrapFocusClass); }
            }
        });
        Ext.onReady(function() {
            if (typeof (BMap) == 'undefined') { TipWnd.show(); }
            NavMap.render();
        });
        //]]>
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Window ID="TipWnd" runat="server" Title="系统提示" Icon="Lightbulb" Height="180px"
        Width="600px" BodyStyle="background-color:#fff;padding:5px;" Closable="false"
        Modal="true" InitCenter="true" Hidden="true">
        <Content>
            <p style="font-size: 15px; font-weight: bold; color: #cc0000; text-align: center; padding: 10px 0;">
                未连接到网络，无法使用地图导航功能。
            </p>
            <p style="font-size: 13px; padding-bottom: 5px;">
                1) 地图导航功能提供的是一种在线服务，需要访问的客户端连接到互联网后才可以正常使用。</p>
            <p style="font-size: 13px; padding-bottom: 5px;">
                2) 请确保您的客户端已成功连接到互联网后，刷新页面重试。</p>
            <p style="font-size: 13px; padding-bottom: 5px;">
                3) 您也可以忽略本功能，选择以下操作：</p>
            <p style="font-size: 13px; padding-top: 10px; text-align: center;">
                <a href="Logout.aspx" target="_self">安全退出系统</a> 
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <a href="Home.aspx" target="_self">进入系统主页</a>
            </p>
        </Content>
    </ext:Window>
    <ext:Viewport ID="NavMapViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:TreePanel ID="NavTreePanel" runat="server" Header="false" Width="260" Lines="true"
                CollapseFirst="false" Collapsed ="true" ContainerScroll="true" AutoScroll="true" Split="true" CollapseMode="Mini"
                Region="West">
                <TopBar>
                    <ext:Toolbar ID="FindTreeNodeToolBar" runat="server">
                        <Items>
                            <ext:TextField ID="TreeTriggerField" runat="server" Width="180" EmptyText="输入局站名称...">
                                <Listeners>
                                    <Change Fn="NavMap.mapFilterChange" Scope="NavMap" />
                                </Listeners>
                            </ext:TextField>
                            <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5" />
                            <ext:Button ID="btnTreeSearch" runat="server" Icon="Magnifier" ToolTip="搜索局站">
                                <Listeners>
                                    <Click Fn="NavMap.mapFilterSearch" Scope="NavMap" />
                                </Listeners>
                            </ext:Button>
                            <ext:ToolbarFill ID="TreeToolbarFill" runat="server" />
                            <ext:Button ID="TreeMenuButton" runat="server" Icon="Cog" ToolTip="选项">
                                <Menu>
                                    <ext:Menu ID="TreeMenu" runat="server">
                                        <Items>
                                            <ext:MenuItem ID="TreeMenuItem1" runat="server" Text="展开所有" IconCls="icon-expand-all">
                                                <Listeners>
                                                    <Click Handler="#{NavTreePanel}.expandAll();" />
                                                </Listeners>
                                            </ext:MenuItem>
                                            <ext:MenuSeparator ID="TreeMenuSeparator1" runat="server" />
                                            <ext:MenuItem ID="TreeMenuItem2" runat="server" Text="折叠所有" IconCls="icon-collapse-all">
                                                <Listeners>
                                                    <Click Handler="#{NavTreePanel}.collapseAll();" />
                                                </Listeners>
                                            </ext:MenuItem>
                                        </Items>
                                    </ext:Menu>
                                </Menu>
                            </ext:Button>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Root>
                    <ext:TreeNode Text="全国城市列表" Icon="World" SingleClickExpand="true" Expanded="true">
                    </ext:TreeNode>
                </Root>
                <Listeners>
                    <AfterRender Fn="NavMap.buildCityCenterTree" Scope="NavMap" />
                    <Click Fn="NavMap.onNavTreeItemClick" Scope="NavMap" />
                </Listeners>
            </ext:TreePanel>
            <ext:Panel ID="MapPanel" runat="server" Title="欢迎使用地图导航" Icon="Map" EnableTabScroll="true"
                Region="Center">
                <Content>
                    <div id="mapContainer">
                    </div>
                    <div style="width: 100%; height: 25px; line-height: 25px; background: #DFE8F6; border-top: solid 1px #99BBE8;">
                        <div style="float: left; text-align: left;">
                            <div class="cursor-alarm-cnt" title="总计告警">
                                <a id="div-alarm-cnt" href="javascript:void(0)" onclick="NavMap.showTotalWnd(EnmAlarmLevel.NoAlarm)">
                                    (0)</a>
                            </div>
                            <div class="cursor-alarm-1" title="一级告警">
                                <a id="div-alarm-1" href="javascript:void(0)" onclick="NavMap.showTotalWnd(EnmAlarmLevel.Critical)">
                                    (0)</a>
                            </div>
                            <div class="cursor-alarm-2" title="二级告警">
                                <a id="div-alarm-2" href="javascript:void(0)" onclick="NavMap.showTotalWnd(EnmAlarmLevel.Major)">
                                    (0)</a>
                            </div>
                            <div class="cursor-alarm-3" title="三级告警">
                                <a id="div-alarm-3" href="javascript:void(0)" onclick="NavMap.showTotalWnd(EnmAlarmLevel.Minor)">
                                    (0)</a>
                            </div>
                            <div class="cursor-alarm-4" title="四级告警">
                                <a id="div-alarm-4" href="javascript:void(0)" onclick="NavMap.showTotalWnd(EnmAlarmLevel.Hint)">
                                    (0)</a>
                            </div>
                        </div>
                        <div style="float: right; text-align: right;">
                            如果在线地图无法加载或暂不需要该功能，您可以直接跳转到<a href="Home.aspx" target="_self">系统主页</a>或<a href="Logout.aspx"
                                target="_self">退出系统</a>。
                        </div>
                    </div>
                </Content>
                <Listeners>
                    <AfterLayout Fn="NavMap.setMapPanelSize" Scope="NavMap" />
                </Listeners>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    <ext:TaskManager ID="NavMapsTaskManager" runat="server">
        <Tasks>
            <ext:Task AutoRun="false" Interval="5000">
                <Listeners>
                    <Update Handler="NavMap.updateViewMarkers();" />
                </Listeners>
            </ext:Task>
        </Tasks>
    </ext:TaskManager>
    <div id="infoWnd" class="infoWndBox">
        <div id="infoWnd_title">
            <div id="infoWnd_title_container" class="infoWnd_title" title="未命名">
                <span id="infoWnd_title_container_name">未命名</span>
                <a id="A1" href="javascript:void(0);" onclick="javascript:NavMap.infoWnd.showDevWnd();return false">详细»</a>
                |
                <a id="infoWnd_title_container_detail" href="javascript:void(0);" onclick="javascript:NavMap.infoWnd.showDetailWnd(EnmAlarmLevel.NoAlarm);return false">告警»</a>
            </div>
        </div>
        <div id="infoWnd_content">
            <div id="infoWnd_content_container" class="infoWnd_content">
            </div>
            <div id="infoWnd_content_control" class="infoWnd_ctrl">
                <span id="infoWnd_content_control_delete" class="delete" title="删除标记"></span><span
                    id="infoWnd_content_control_edit" class="edit" title="修改标记"></span>
            </div>
        </div>
    </div>
    <div id="editWnd" class="editWndBox">
        <div id="editWnd_title">
            <div id="editWnd_title_container" class="editWnd_title" title="修改标记">
                修改标记
            </div>
        </div>
        <div id="editWnd_content" class="editWnd_content">
            <div class="item">
                <ext:ComboBox ID="LscsComboBox" runat="server" Width="250" LabelWidth="40" FieldLabel="Lsc"
                    Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                    ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true">
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
                    <Listeners>
                        <Select Handler="#{StaComboBox}.clearValue(); #{StaComboBox}.store.reload();" />
                    </Listeners>
                </ext:ComboBox>
            </div>
            <div class="item">
                <ext:ComboBox ID="StaComboBox" runat="server" Width="250" LabelWidth="40" FieldLabel="局站"
                    Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local"
                    ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true">
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
                                <Load Handler="if(this.getCount() > 0){if(this._staId){#{StaComboBox}.setValue(this._staId);} else{#{StaComboBox}.setValue(this.getAt(0).get('Id'));} this._staId = null;}" />
                            </Listeners>
                        </ext:Store>
                    </Store>
                </ext:ComboBox>
            </div>
            <div class="item">
                <ext:TextField ID="us_editWnd_lng" runat="server" Width="250" LabelWidth="40" FieldLabel="经度">
                </ext:TextField>
            </div>
            <div class="item">
                <ext:TextField ID="us_editWnd_lat" runat="server" Width="250" LabelWidth="40" FieldLabel="纬度">
                </ext:TextField>
            </div>
            <div class="item">
                <ext:TextField ID="us_editWnd_add" runat="server" Width="250" LabelWidth="40" FieldLabel="地址">
                </ext:TextField>
            </div>
            <div class="bottom">
                <input id="btnSave" class="iw_bt" type="button" onmouseup="this.className='iw_bt_over'"
                    onmousedown="this.className='iw_bt_down'" onmouseout="this.className='iw_bt'"
                    onmouseover="this.className='iw_bt_over'" value="保存">
                <input id="btnCancel" class="iw_bt" type="button" onmouseup="this.className='iw_bt_over'"
                    onmousedown="this.className='iw_bt_down'" onmouseout="this.className='iw_bt'"
                    onmouseover="this.className='iw_bt_over'" value="取消">
            </div>
        </div>
    </div>
    </form>
</body>
</html>
