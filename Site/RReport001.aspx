<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RReport001.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.RReport001" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>告警考核</title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link rel="icon" type="image/x-icon" href="favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="favicon.ico" />
    <script type="text/javascript" src="Resources/js/public.js?v=3.3.0.0"></script>
    <style type="text/css">
        .clear{display:block;overflow:hidden;clear:both;height:0;line-height:0;font-size:0;}
        .clearfix:after{content:".";display:block;height:0;clear:both;visibility:hidden;}
        .clearfix{display:inline-table;}/* Hides from IE-mac \*/
        *html .clearfix{height:1%;}
        .clearfix{display:block;}/* End hide from IE-mac */
        *+html .clearfix{min-height:1%;}
        
        .plus-tag 
        {
            display:block;
            padding:5px;
            height:80px; 
            width:708px;
            background:#fff; 
            border:1px solid #8DB2E3;
            overflow:hidden;
            overflow-x:hidden;
            overflow-y:auto;
        }
        
        .plus-tag a,.plus-tag a em
        {
            background:url(Resources/images/Shared/tag.png) no-repeat;
        }
        
        .plus-tag a
        {
            display:block;
            float:left;
            height:22px;
            line-height:22px;
            overflow:hidden;
            margin:0 10px 10px 0;
            padding: 0 10px 0 5px;
            white-space:nowrap;
            background-position:100% -22px;
        }
        
        .plus-tag a span
        {
            float:left;
            margin:3px 0 0 0;
        } 
        
        .plus-tag a em
        {
            display:block;
            float:left;
            margin:5px 0 0 8px;
            width:13px;
            height:13px;
            overflow:hidden;
            background-position:-165px -100px;
            cursor:pointer;
        }
        
        .plus-tag a:hover em
        {
            background-position:-168px -64px;
        }
    </style>
    <script type="text/javascript">
        Ext.onReady(function () {
            (function () {
                Ext.get("tagsInBar").on('click', function (e, el, eOpts) {
                    var parent = Ext.get(el).parent();
                    if (!Ext.isEmpty(parent)) {
                        parent.remove();
                    }
                }, this, { delegate: 'em.close' });
            })();
        });

        var ShowAlamParmWnd = function (e, el, eOpts) {
            var max = 10,
                tags = Ext.query(".plus-tag>a");

            if (tags.length >= max) {
                Ext.Msg.show({
                    title: 'Warning',
                    msg: String.format('最多允许创建{0}列!', max),
                    buttons: Ext.Msg.OK,
                    icon: Ext.MessageBox.WARNING
                });
                return false;
            }

            ParamWnd.show();
        };

        var addTagsClick = function (e, el, eOpts) {
            var source = ParamPropertyGrid.getSource(),
                colname = source['ColName'],
                almname = source['AlmName'],
                minvalue = source['AlmMinColName'],
                maxvalue = source['AlmMaxColName'];

            if (Ext.isEmpty(colname)) {
                Ext.Msg.show({
                    title: 'Warning',
                    msg: '列名称不能为空!',
                    buttons: Ext.Msg.OK,
                    icon: Ext.MessageBox.WARNING
                });
                return false;
            }

            if (Ext.isEmpty(almname)) {
                Ext.Msg.show({
                    title: 'Warning',
                    msg: '告警名称不能为空!',
                    buttons: Ext.Msg.OK,
                    icon: Ext.MessageBox.WARNING
                });
                return false;
            }

            if (Ext.isEmpty(minvalue)) {
                Ext.Msg.show({
                    title: 'Warning',
                    msg: '告警时长不能为空!',
                    buttons: Ext.Msg.OK,
                    icon: Ext.MessageBox.WARNING
                });
                return false;
            }

            if (Ext.isEmpty(maxvalue)) {
                Ext.Msg.show({
                    title: 'Warning',
                    msg: '告警时长不能为空!',
                    buttons: Ext.Msg.OK,
                    icon: Ext.MessageBox.WARNING
                });
                return false;
            }

            var data = String.format("{0}col:'{1}',alm:'{2}',min:{3},max:{4}{5}", '{', colname, almname, minvalue, maxvalue, '}');
            var html = String.format('<a data="{0}" title="{1}" href="javascript:void(0);"><span>{1}</span><em class="close"></em></a>', data, colname);
            Ext.DomHelper.append(Ext.get("tagsInBar"), html);
            ParamWnd.hide();
        };

        var getSyncColumns = function () {
            var tags = Ext.query(".plus-tag>a"),
                data = '[';

            Ext.each(tags, function (item, index, allItems) {
                if (index > 0) { data += ','; }
                data += item.getAttribute('data');
            });

            data += ']';
            return data;
        };

        var chkPost = function () {
            var data = getSyncColumns();
            if (Ext.isEmpty(data)) { return false; }

            var jdata = Ext.util.JSON.decode(data);
            if (jdata.length == 0) { return false; }

            return true;
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="x-hide-display">
        <div id="tagsInBar" class="plus-tag clearfix"></div>
    </div>
    <ext:ResourceManager ID="ResourceManager1" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <ext:Viewport ID="MainViewport" runat="server" Layout="BorderLayout">
        <Items>
            <ext:Panel ID="MainPanel" runat="server" Layout="FitLayout" Title="告警考核" Icon="ApplicationViewList" Region="Center">
                <TopBar>
                    <ext:Toolbar ID="TopToolbars" runat="server" Layout="MenuLayout">
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
                                                    <Load Handler="#{LscsComboBox}.setValueAndFireSelect(this.getAt(0).get('Id')); " />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                    <ext:ComboBox ID="DevTypeComboBox" runat="server" Width="180" LabelWidth="50" FieldLabel="设备类型" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true" Resizable="true">
                                        <Store>
                                            <ext:Store ID="DevTypeStore" runat="server" AutoLoad="true" OnRefreshData="OnDevTypeRefresh">
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
                                                    <Load Handler="#{DevTypeComboBox}.setValueAndFireSelect(this.getAt(0).get('Id')); " />
                                                </Listeners>
                                            </ext:Store>
                                        </Store>
                                    </ext:ComboBox>
                                    <ext:TriggerField ID="BeginFromDate" runat="server" Width="180" LabelWidth="50" FieldLabel="查询时间">
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
                                    <ext:ToolbarSpacer ID="ToolbarSpacer1" runat="server" Width="5" />
                                    <ext:SplitButton ID="QueryBtn" runat="server" Text="查询" Icon="Magnifier" StandOut="true">
                                        <Menu>
                                            <ext:Menu ID="QueryMenu" runat="server">
                                                <Items>
                                                    <ext:MenuItem ID="SaveMenuItem" runat="server" Text="打印/导出" Icon="Printer">
                                                        <%--<DirectEvents>
                                                            <Click OnEvent="SaveBtn_Click" IsUpload="true">
                                                                <ExtraParams>
                                                                    <ext:Parameter Name="ColumnNames" Mode="Raw" Value="getGridColumnNames(#{DevCntGridPanel})" />
                                                                </ExtraParams>
                                                            </Click>
                                                        </DirectEvents>--%>
                                                    </ext:MenuItem>
                                                </Items>
                                            </ext:Menu>
                                        </Menu>
                                        <DirectEvents>
                                            <Click OnEvent="QueryBtn_Click" Success="#{MainPagingToolbar}.doLoad();" Before="return chkPost();" ViewStateMode="Enabled">
                                                <ExtraParams>
                                                    <ext:Parameter Name="SyncColumns" Value="getSyncColumns()" Mode="Raw" />
                                                </ExtraParams>
                                                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{MainGridPanel}.body.up('div')" />
                                            </Click>
                                        </DirectEvents>
                                    </ext:SplitButton>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar ID="TopToolbar2" runat="server" Flat="true">
                                <Items>
                                    <ext:ToolbarHtmlElement Target="#{tagsInBar}" />
                                    <ext:ToolbarSpacer ID="ToolbarSpacer2" runat="server" Width="5" />
                                    <ext:Button ID="ColumnAddButton" runat="server" Icon="Add" StandOut="false" ToolTip="新增条件列">
                                        <Listeners>
                                            <Click Fn="ShowAlamParmWnd" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </Items>
                    </ext:Toolbar>
                </TopBar>
                <Items>
                    <ext:GridPanel ID="MainGridPanel" runat="server" StripeRows="true" TrackMouseOver="true" AutoScroll="true" Border="false">
                        <Store>
                            <ext:Store ID="MainStore" runat="server" AutoLoad="false" IgnoreExtraFields="false" OnRefreshData="OnMainStoreRefresh">
                                <Proxy>
                                    <ext:PageProxy />
                                </Proxy>
                                <Reader>
                                    <ext:JsonReader />
                                </Reader>
                                <BaseParams>
                                    <ext:Parameter Name="start" Value="0" Mode="Raw" />
                                    <ext:Parameter Name="limit" Value="#{PageSizeComboBox}.getValue()" Mode="Raw" />
                                </BaseParams>
                                <DirectEventConfig IsUpload="true" />
                            </ext:Store>
                        </Store>
                        <LoadMask ShowMask="true" />
                        <SelectionModel>
                            <ext:RowSelectionModel ID="MainRowSelectionModel" runat="server"/>
                        </SelectionModel>
                        <View>
                            <ext:GridView ID="MainGridView" runat="server" ForceFit="false" />
                        </View>
                        <BottomBar>
                            <ext:PagingToolbar ID="MainPagingToolbar" runat="server" PageSize="20" StoreID="MainStore">
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
                                            <Select Handler="#{MainPagingToolbar}.pageSize = parseInt(this.getValue()); #{MainPagingToolbar}.doLoad();" />
                                        </Listeners>
                                    </ext:ComboBox>
                                </Items>
                            </ext:PagingToolbar>
                        </BottomBar>
                    </ext:GridPanel>
                </Items>
            </ext:Panel>
        </Items>
    </ext:Viewport>
    <ext:Window ID="ParamWnd" runat="server" Title="告警筛选条件" Icon="TableColumnAdd" Width="350px" Height="300px" Modal="true" InitCenter="true" Hidden="true">
        <Items>
            <ext:BorderLayout ID="ParamBorderLayout" runat="server">
                <Center>
                    <ext:PropertyGrid ID="ParamPropertyGrid" runat="server" Border="false">
                        <Source>
                            <ext:PropertyGridParameter Name="ColName" Value="" DisplayName="列名称">
                                <Editor>
                                    <ext:TextField ID="ColShowName" runat="server" AllowBlank="false" MaxLength="15" />
                                </Editor>
                            </ext:PropertyGridParameter>
                            <ext:PropertyGridParameter Name="AlmName" Value="" DisplayName="告警名称">
                                <Editor>
                                    <ext:TriggerField ID="AlmNameTriggerField" runat="server">
                                        <Triggers>
                                            <ext:FieldTrigger Icon="SimpleEllipsis" Tag="almWndShow" />
                                        </Triggers>
                                        <Listeners>
                                            <TriggerClick Handler="#{ParamPropertyGrid}.stopEditing(); #{AlmNamesWnd}.show();" />
                                        </Listeners>
                                    </ext:TriggerField>
                                </Editor>
                            </ext:PropertyGridParameter>
                            <ext:PropertyGridParameter Name="AlmMinColName" Value="0" DisplayName="告警最小时长(秒)">
                                <Editor>
                                    <ext:SpinnerField ID="AlmMinInterval" runat="server" MinValue="0" AllowDecimals="false" IncrementValue="1" />
                                </Editor>
                            </ext:PropertyGridParameter>
                            <ext:PropertyGridParameter Name="AlmMaxColName" Value="0" DisplayName="告警最大时长(秒)">
                                <Editor>
                                    <ext:SpinnerField ID="AlmMaxInterval" runat="server" MinValue="0" AllowDecimals="false" IncrementValue="1" />
                                </Editor>
                            </ext:PropertyGridParameter>
                        </Source>
                        <View>
                            <ext:GridView ID="GridView1" runat="server" ForceFit="true" ScrollOffset="2" />
                        </View>
                        <Listeners>
                            <Render Handler="this.getStore().sortInfo = undefined;" />
                            <%--<AfterEdit Fn="onColumnPropertyChanged" />--%>
                        </Listeners>
                    </ext:PropertyGrid>
                </Center>
            </ext:BorderLayout>
        </Items>
        <Listeners>
            <BeforeShow Handler="
                #{ParamPropertyGrid}.setProperty('ColName', '');
                #{ParamPropertyGrid}.setProperty('AlmName', '');
                #{ParamPropertyGrid}.setProperty('AlmMinColName', 0);
                #{ParamPropertyGrid}.setProperty('AlmMaxColName', 0);
            " />
        </Listeners>
        <Buttons>
            <ext:Button ID="SaveBtn" runat="server" Text="保存">
                <Listeners>
                    <Click Fn="addTagsClick" />
                </Listeners>
            </ext:Button>
            <ext:Button ID="CancelBtn" runat="server" Text="关闭">
                <Listeners>
                    <Click Handler="#{ParamWnd}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    <ext:Window ID="AlmNamesWnd" runat="server" Title="告警名称筛选" Icon="TableColumnAdd" Width="350px" Height="155px" Modal="true" InitCenter="true" Hidden="true" Padding="5" BodyStyle="background:#fff;">
        <Items>
            <ext:FormPanel ID="AlmNamesFormPanel" runat="server" Border="false" Header="false" ColumnWidth="1" Layout="FormLayout" LabelWidth="80">
                <Defaults>
                    <ext:Parameter Name="MsgTarget" Value="side" />
                </Defaults>
                <Items>
                    <ext:ComboBox ID="AlarmDevComboBox" runat="server" FieldLabel="告警设备" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true" Resizable="true" AnchorHorizontal="92%">
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
                    <ext:ComboBox ID="AlarmLogicComboBox" runat="server" FieldLabel="告警逻辑" Editable="true" DisplayField="Name" ValueField="Id" TypeAhead="true" Mode="Local" ForceSelection="true" TriggerAction="All" EmptyText="Loading..." SelectOnFocus="true" Resizable="true" AnchorHorizontal="92%">
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
                            <Select Handler="#{AlarmNameMultiCombo}.clearValue(); #{AlarmNameMultiCombo}.store.reload();" />
                        </Listeners>
                    </ext:ComboBox>
                    <ext:MultiCombo ID="AlarmNameMultiCombo" runat="server" SelectionMode="All" FieldLabel="告警名称" EmptyText="请勾选至少一项" DisplayField="Name" ValueField="Id" ForceSelection="true" TriggerAction="All" Resizable="true" Delimiter=";" AnchorHorizontal="92%">
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
                            </ext:Store>
                        </Store>
                    </ext:MultiCombo>
                </Items>
            </ext:FormPanel>
        </Items>
        <Buttons>
            <ext:Button ID="SaveBtn2" runat="server" Text="保存">
                <Listeners>
                    <Click Handler="
                        var v = #{AlarmNameMultiCombo}.getRawValue();
                        if(!Ext.isEmpty(v)){
                            #{ParamPropertyGrid}.setProperty('AlmName', v);
                            #{AlmNamesWnd}.hide();
                        }
                    " />
                </Listeners>
            </ext:Button>
            <ext:Button ID="CancelBtn2" runat="server" Text="关闭">
                <Listeners>
                    <Click Handler="#{AlmNamesWnd}.hide();" />
                </Listeners>
            </ext:Button>
        </Buttons>
    </ext:Window>
    </form>
</body>
</html>
