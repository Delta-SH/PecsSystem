/*
* Common JavaScript Library v3.0.0
*
* Copyright 2013-2014, Steven
*
* Date: 2014/08/28
*/

Ext.ns("X");
Ext.Ajax.timeout = 300000;
Ext.net.DirectEvent.timeout = 300000;
Ext.data.Connection.override({ timeout: 300000 });
//Define enum objects
var PageSize = { width: parseInt(Ext.getBody().getViewSize().width, 10), height: parseInt(Ext.getBody().getViewSize().height, 10) };
var EnmNodeType = { Null: -3, LSC: -2, Area: -1, Sta: 0, Dev: 1, Dic: 2, Aic: 3, Doc: 4, Aoc: 5, Str: 6, Img: 7, Sic: 9, SS: 10, RS: 11, RTU: 12 };
var EnmAlarmLevel = { NoAlarm: 0, Critical: 1, Major: 2, Minor: 3, Hint: 4 };
var EnmState = { NoAlarm: 0, Critical: 1, Major: 2, Minor: 3, Hint: 4, Opevent: 5, Invalid: 6 };
var EnmAction = { Init: 0, Data: 1, Null: 2 };
//Define global variables
var curTreePanel = null;
var curNodesPanel = null;
var curNode = null;
var lastNodeType = null;
var ACSAction = EnmAction.Init;
var ACVAction = EnmAction.Init;
var navFilterSearch = function(el, e) {
    var tt = TreeTriggerField,
        text = tt.getRawValue();
    if (Ext.isEmpty(text, false)) { return false; }
    if (tt._filterData != null && tt._filterIndex != null) {
        var index = tt._filterIndex + 1;
        var nodes = tt._filterData;
        if (index >= nodes.length) { index = 0; }
        expandChildren(nodes[index]);
        tt._filterIndex = index;
    }
    else {
        X.Home.ConvertNodeNameToTreeID(text, {
            success: function(result) {
                if (!Ext.isEmpty(result, false)) {
                    var nodes = Ext.util.JSON.decode(result, true);
                    if (nodes) {
                        var len = nodes.length;
                        if (len > 0) {
                            expandChildren(nodes[0]);
                            tt._filterData = nodes;
                            tt._filterIndex = 0;
                        } else {
                            Ext.Msg.show({ title: LanguageSet.SystemTip, msg: String.format(LanguageSet.SearchTip, text), buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO, minWidth: 250 });
                        }
                    } else {
                        Ext.Msg.show({ title: LanguageSet.SystemTip, msg: String.format(LanguageSet.SearchTip, text), buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO, minWidth: 250 });
                    }
                } else {
                    Ext.Msg.show({ title: LanguageSet.SystemTip, msg: String.format(LanguageSet.SearchTip, text), buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO, minWidth: 250 });
                }
            }
        });
    }
};



var udFilterSearch = function (el, e) {
    var tt = UserDefindTriggerField,
        text = tt.getRawValue();
    if (Ext.isEmpty(text, false)) { return false; }
    if (tt._filterData != null && tt._filterIndex != null) {
        var index = tt._filterIndex + 1;
        var nodes = tt._filterData;
        if (index >= nodes.length) { index = 0; }
        expandChildren(nodes[index]);
        tt._filterIndex = index;
    }
    else {
        X.Home.ConvertUDNodeNameToTreeID(text, {
            success: function (result) {
                if (!Ext.isEmpty(result, false)) {
                    var nodes = Ext.util.JSON.decode(result, true);
                    if (nodes) {
                        var len = nodes.length;
                        if (len > 0) {
                            expandChildren(nodes[0]);
                            tt._filterData = nodes;
                            tt._filterIndex = 0;
                        } else {
                            Ext.Msg.show({ title: LanguageSet.SystemTip, msg: String.format(LanguageSet.SearchTip, text), buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO, minWidth: 250 });
                        }
                    } else {
                        Ext.Msg.show({ title: LanguageSet.SystemTip, msg: String.format(LanguageSet.SearchTip, text), buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO, minWidth: 250 });
                    }
                } else {
                    Ext.Msg.show({ title: LanguageSet.SystemTip, msg: String.format(LanguageSet.SearchTip, text), buttons: Ext.Msg.OK, icon: Ext.MessageBox.INFO, minWidth: 250 });
                }
            }
        });
    }
};

var treeFilterChange = function(el, newValue, oldValue) {
    delete el._filterData;
    delete el._filterIndex;
};

var onTreeItemClick = function (el, e) {
    curNode = el;
    if (typeof (e) != "undefined") {
        clearNodesGridStore();
        ACSAction = EnmAction.Init;
        ACVAction = EnmAction.Init;
    }

    switch (curNode.attributes.TreeNodeType) {
        case EnmNodeType.Null:
            if (lastNodeType != EnmNodeType.Null) {
                addNodesTab([]);
                lastNodeType = EnmNodeType.Null;
            }
            clearNodesGridStore();
            break;
        case EnmNodeType.LSC:
            if (lastNodeType != EnmNodeType.LSC) {
                addNodesTab([NodesGridTabPanel6, NodesGridTabPanel7]);
                lastNodeType = EnmNodeType.LSC;
            }
            loadNodesGridStore();
            break;
        case EnmNodeType.Area:
            if (lastNodeType != EnmNodeType.Area) {
                addNodesTab([NodesGridTabPanel6, NodesGridTabPanel7, NodesGridTabPanel8]);
                lastNodeType = EnmNodeType.Area;
            }
            loadNodesGridStore();
            break;
        case EnmNodeType.Sta:
            if (lastNodeType != EnmNodeType.Sta) {
                addNodesTab([NodesGridTabPanel6, NodesGridTabPanel8]);
                lastNodeType = EnmNodeType.Sta;
            }
            loadNodesGridStore();
            break;
        case EnmNodeType.Dev:
            if (lastNodeType != EnmNodeType.Dev) {
                addNodesTab([NodesGridTabPanel1, NodesGridTabPanel2, NodesGridTabPanel3, NodesGridTabPanel4, NodesGridTabPanel5, NodesGridTabPanel8]);
                lastNodeType = EnmNodeType.Dev;
            }
            loadNodesGridStore();
            break;
        default:
            break;
    }
    if (typeof (e) == "undefined") { return false; }
};

var clearNodesGridStore = function() {
    if (NodesGridStore1.data.length > 0) { NodesGridStore1.removeAll(); }
    if (NodesGridStore2.data.length > 0) { NodesGridStore2.removeAll(); }
    if (NodesGridStore3.data.length > 0) { NodesGridStore3.removeAll(); }
    if (NodesGridStore4.data.length > 0) { NodesGridStore4.removeAll(); }
    if (NodesGridStore5.data.length > 0) { NodesGridStore5.removeAll(); }
    if (NodesGridStore6.data.length > 0) { NodesGridStore6.removeAll(); }
    if (NodesGridStore7.data.length > 0) { NodesGridStore7.removeAll(); }
};

var clearNodesTab = function() {
    var el = NodesTabPanel;
    el.items.each(function(item) {
        el.closeTab(item);
    }, this);
};

var addNodesTab = function(targets) {
    var el = NodesTabPanel;
    el.activeLock = true;
    clearNodesTab();
    for (var i = 0; i < targets.length; i++) { el.addTab(targets[i]); }
    if (targets.length > 0) { el.setActiveTab(targets[0]); }
    el.activeLock = false;
};

var onNodesGridTabPanelActivate = function(el) {
    curNodesPanel = el;
    if (NodesTabPanel.activeLock) { return false; }
    if (el == NodesGridTabPanel7) { el.doLayout(); }
    if (curNode && curTreePanel) { curTreePanel.fireEvent("click", curNode); }
};

var loadNodesGridStore = function () {
    if (!curNode || !curNodesPanel) { return false; }
    var nodeType = curNode.attributes.TreeNodeType;
    switch (curNodesPanel) {
        case NodesGridTabPanel1:
            if (nodeType == EnmNodeType.Dev) { X.Home.GetNodes(curNode.id, EnmNodeType.Null); }
            break;
        case NodesGridTabPanel2:
            if (nodeType == EnmNodeType.Dev) { X.Home.GetNodes(curNode.id, EnmNodeType.Dic); }
            break;
        case NodesGridTabPanel3:
            if (nodeType == EnmNodeType.Dev) { X.Home.GetNodes(curNode.id, EnmNodeType.Doc); }
            break;
        case NodesGridTabPanel4:
            if (nodeType == EnmNodeType.Dev) { X.Home.GetNodes(curNode.id, EnmNodeType.Aic); }
            break;
        case NodesGridTabPanel5:
            if (nodeType == EnmNodeType.Dev) { X.Home.GetNodes(curNode.id, EnmNodeType.Aoc); }
            break;
        case NodesGridTabPanel6:
            if (nodeType == EnmNodeType.Sta || nodeType == EnmNodeType.Area || nodeType == EnmNodeType.LSC) {
                if (ACSAction == EnmAction.Init) {
                    X.Home.SetACSFilterGrid(curNode.id, nodeType, {
                        success: function (result) {
                            ACSAction = EnmAction.Data;
                            X.Home.SetACSFilterData(curNode.id, nodeType);
                        }
                    });
                } else if (ACSAction == EnmAction.Data) {
                    X.Home.SetACSFilterData(curNode.id, nodeType);
                }
            }
            break;
        case NodesGridTabPanel7:
            if (nodeType == EnmNodeType.Area || nodeType == EnmNodeType.LSC) {
                if (ACVAction == EnmAction.Init) {
                    X.Home.SetACVFilterGrid(curNode.id, nodeType, {
                        success: function (result) {
                            ACVAction = EnmAction.Data;
                            NodesGridTabPanel7.doLayout();
                            NodesGridTabPanel7PagingToolbar.doLoad();
                        }
                    });
                } else if (ACVAction == EnmAction.Data) {
                    NodesGridTabPanel7PagingToolbar.doRefresh();
                }
            }
            break;
        case NodesGridTabPanel8:
            break;
        default:
            break;
    }
};

var prepareCommand = function(grid, command, record, row) {
    switch (record.data.NodeType) {
        case EnmNodeType.Doc:
            if (command.command === "Set") {
                command.hidden = true;
                command.hideMode = "display";
            }
            break;
        case EnmNodeType.Aoc:
            if (command.command === "Ctrl") {
                command.hidden = true;
                command.hideMode = "display";
            }
            break;
        default:
            command.hidden = true;
            command.hideMode = "display";
            break;
    }

    if (command.command === "Cam") {
        command.hidden = Ext.isEmpty(record.data.CamUrl);
        command.hideMode = "display";
    }
};

var onNodesGridTabPanelCmdClick = function (command, record, rowIndex, colIndex) {
    if (command === "Cam") {
        window.open(record.data.CamUrl, '_blank')
    } else {
        X.Home.ShowControlWindow(command, record.data.LscID, record.data.NodeID, record.data.NodeType, record.data.NodeName);
    }
};

var activeTreeTab = function(el) {
    curTreePanel = el;
    clearNodesGridStore();
    curNode = curTreePanel.getSelectionModel().getSelectedNode();
    if (curNode) {
        ACSAction = EnmAction.Init;
        ACVAction = EnmAction.Init;
        el.fireEvent("click", curNode);
    }
    else {
        addNodesTab([]);
        lastNodeType = EnmNodeType.Null;
    }
};

var onUserDefindTreePanelMenuShow = function(el, e) {
    var depth = el.getDepth();
    if (depth > 0) {
        el.select();
        UserDefindTreePanelMenu.node = el;
        UserDefindTreePanelMenu.showAt(e.getXY());
        if (depth == 2) {
            UserDefindTreePanelMenuItem2.setDisabled(false);
            UserDefindTreePanelMenuItem3.setDisabled(false);
        }
        else {
            UserDefindTreePanelMenuItem2.setDisabled(true);
            UserDefindTreePanelMenuItem3.setDisabled(true);
        }
    }
};

var delUserDefindGroup = function(el, e) {
    var node = el.parentMenu.node;
    X.Home.DelUserDefindGroup(node.id, {
        success: function(result) {
            if (result) {
                node.remove(true);
                setUDGroupTreeIcons();
            }
        },
        eventMask: {
            showMask: true,
            target: "customtarget",
            customTarget: UserDefindTreePanel
        }
    });
};

var loadUserDefindGroups = function() {
    X.Home.InitUserDefindTreeNodes({
        success: function(result) {
            if (!Ext.isEmpty(result, false)) {
                var nodes = Ext.util.JSON.decode(result, true);
                if (nodes && nodes.length > 0) {
                    UserDefindTreePanel.initChildren(nodes);
                } else {
                    UserDefindTreePanel.getRootNode().removeChildren(); 
                }
                setUDGroupTreeIcons();
            }
        },
        eventMask: {
            showMask: true,
            target: "customtarget",
            customTarget: UserDefindTreePanel
        }
    });
};

var getCurNodeId = function() {
    if (curNode) { return curNode.id; }
    return "";
};

var getCurNodeType = function() {
    if (curNode) { return curNode.attributes.TreeNodeType; }
    return "";
};

var getCurNodeName = function() {
    if (curNode) { return curNode.text; }
    return "";
};

var getNodesExportTitle = function(el) {
    if (curNode && curNode.isLeaf()) { return curNode.text + el.title; }
    return "";
};

var onNodesExportClick = function(el, e) {
    if (!curNodesPanel) { return false; }
    if (curNodesPanel == NodesGridTabPanel1
    || curNodesPanel == NodesGridTabPanel2
    || curNodesPanel == NodesGridTabPanel3
    || curNodesPanel == NodesGridTabPanel4
    || curNodesPanel == NodesGridTabPanel5
    || curNodesPanel == NodesGridTabPanel6) {
        curNodesPanel.submitData(false); 
    }
    else if (curNodesPanel == NodesGridTabPanel7) {
        exportACVGridNodes(); 
    }
};

var onNodesGridRowContextMenuShow = function(el, rowIndex, e) {
    e.preventDefault();
    el.getSelectionModel().selectRow(rowIndex);
    NodesGridRowMenu.dataRecord = el.store.getAt(rowIndex);
    NodesGridRowMenu.showAt(e.getXY());
    var nodeType = el.store.getAt(rowIndex).data.NodeType;
    if (nodeType == EnmNodeType.Dic || nodeType == EnmNodeType.Aic) {
        NodesGridRowItem1.setDisabled(false);
        NodesGridRowItem2.setDisabled(false);
        NodesGridRowItem3.setDisabled(false);
    }
    else {
        NodesGridRowItem1.setDisabled(true);
        NodesGridRowItem2.setDisabled(true);
        NodesGridRowItem3.setDisabled(true);
    }
};

var onACSGridCellContextMenuShow = function(el, rowIndex, cellIndex, e) {
    e.preventDefault();
    el.getSelectionModel().select(rowIndex, cellIndex, false, false);
    ACSRowMenu.dataRecord = el.store.getAt(rowIndex);
    ACSRowMenu.showAt(e.getXY());
};

var onACVGridCellContextMenuShow = function(el, rowIndex, columnIndex, e) {
    e.preventDefault();
    el.getSelectionModel().select(rowIndex, columnIndex, false, false);
    var store = ACVRowStore,
        record = store.getAt(rowIndex),
        dataIndex = el.getColumnModel().getDataIndex(columnIndex),
        data = record.get(dataIndex);

    if (Ext.isEmpty(data, false)
    || dataIndex === "Data0"
    || dataIndex === "Data1"
    || dataIndex === "Data2") {
        ACVRowItem1.setDisabled(true);
        ACVRowItem2.setDisabled(true);
        ACVRowItem3.setDisabled(true);
    }
    else {
        var datas = data.split(";");
        if (datas.length != 2 && datas.length != 3) {
            ACVRowItem1.setDisabled(true);
            ACVRowItem2.setDisabled(true);
            ACVRowItem3.setDisabled(true);
        }
        else {
            ACVRowItem1.setDisabled(false);
            ACVRowItem2.setDisabled(false);
            ACVRowItem3.setDisabled(false);
            ACVRowMenu.dataRecord = new Ext.data.Record({ LscID: datas[0], NodeID: datas[1] });
        }
    }
    ACVRowMenu.showAt(e.getXY());
};

var onACSGridCellDblClick = function(el, rowIndex, columnIndex, e) {
    var column = el.getColumnModel().config[columnIndex];
    if (column.dataIndex === "Data0"
    || column.dataIndex === "Data1"
    || column.dataIndex === "Data2") { return false; }
    var record = el.store.getAt(rowIndex);
    if (record) {
        var title = String.format("{0} [{1}]{2}", record.data.Data1, column.header, LanguageSet.AlarmWndName);
        X.Home.ShowACSFilterDetail(title, rowIndex, column.dataIndex);
    }
};

var onACVGridCellDblClick = function (el, rowIndex, columnIndex, e) {
    var dataIndex = el.getColumnModel().getDataIndex(columnIndex);
    if (dataIndex === "Data0"
    || dataIndex === "Data1"
    || dataIndex === "Data2") { return false; }
    var record = ACVRowStore.getAt(rowIndex),
        data = record.get(dataIndex);

    if (Ext.isEmpty(data, false)) { return false; }
    var datas = data.split(";");
    if (datas.length != 2 && datas.length != 3) { return false; }

    /*select and scroll selected row into view*/
    var grid = NodesGridTabPanel1,
        ndid = datas[1];

    grid.view.on("refresh", function (v) {
        var store = v.grid.getStore();
        if (!Ext.isEmpty(store) && !Ext.isEmpty(ndid)) {
            var index = store.find("NodeID", ndid);
            if (index != -1) {
                v.grid.getSelectionModel().selectRow(index);
                v.focusRow(index);
            }
        }
    }, this, { single: true, delay: 500 });
    /*-----------------------------------------*/

    if (datas.length == 2) {
        X.Home.ConvertNodeIDToTreeID(datas[0], datas[1], {
            success: function (result) {
                if (!Ext.isEmpty(result, false)) {
                    var nodes = Ext.util.JSON.decode(result, true);
                    if (nodes) { expandChildren(nodes); }
                }
            }
        });
    } else {
        X.Home.ConvertUDNodeIDToTreeID(datas[0], datas[1], datas[2], {
            success: function (result) {
                if (!Ext.isEmpty(result, false)) {
                    var nodes = Ext.util.JSON.decode(result, true);
                    if (nodes) { expandChildren(nodes); }
                }
            }
        });
    }
};

var exportACVGridNodes = function(el, e) {
    X.Home.PreExportACVNodes(curNode.id, curNode.text, curNode.attributes.TreeNodeType, {
        success: function(result) {
            X.Home.ExportACVNodes(getGridColumnNames(NodesGridTabPanel7), curNode.text, { isUpload: true });
        },
        eventMask: {
            showMask: true,
            target: "customtarget",
            msg: LanguageSet.Exporting,
            customTarget: NodesGridTabPanel7
        }
    });
};

var getGridColumnNames = function(grid) {
    var xml = "<Names>";
    if (grid) {
        var columns = grid.getColumnModel().config;
        Ext.each(columns, function(c) {
            if (!c.hidden && !Ext.isEmpty(c.dataIndex, false)) {
                xml += '<Name DataIndex="' + c.dataIndex + '" Header="' + c.header + '" />';
            }
        });
    }

    xml += "</Names>";
    return xml;
};

var doProjName = function(value) {
    return !Ext.isEmpty(value, false) ? LanguageSet.BooleanTrue : LanguageSet.BooleanFalse;
};

var doMAlm = function(value) {
    return value === "MAlm" ? LanguageSet.BooleanTrue : LanguageSet.BooleanFalse;
};

var doConn = function(value) {
    return value === "Conn" ? LanguageSet.BooleanTrue : LanguageSet.BooleanFalse;
};

var setGridRowColor = function(record) {
    switch (record.data.AlarmLevel) {
        case EnmAlarmLevel.NoAlarm:
            return "grid-bg-NoAlarm";
        case EnmAlarmLevel.Critical:
            return "grid-bg-Critical";
        case EnmAlarmLevel.Major:
            return "grid-bg-Major";
        case EnmAlarmLevel.Minor:
            return "grid-bg-Minor";
        case EnmAlarmLevel.Hint:
            return "grid-bg-Hint";
        default:
            return "grid-bg-NoAlarm";
    }
};

var setNodesColor = function(record) {
    switch (record.data.Status) {
        case EnmState.NoAlarm:
            return "grid-bg-NoAlarm";
        case EnmState.Critical:
            return "grid-bg-Critical";
        case EnmState.Major:
            return "grid-bg-Major";
        case EnmState.Minor:
            return "grid-bg-Minor";
        case EnmState.Hint:
            return "grid-bg-Hint";
        case EnmState.Opevent:
            return "grid-bg-Opevent";
        case EnmState.Invalid:
            return "grid-bg-Invalid";
        default:
            return "grid-bg-NoAlarm";
    }
};

var setAlarmsColor = function(record) {
    if (!ViewMenuItem5.checked) { return false; }
    return setGridRowColor(record);
};

var setAlarmsInterpretTips = function(data, metadata, record, rowIndex, columnIndex, store) {
    metadata.attr = "ext:hide='false' ext:qtip='" + record.data.AlarmInterpret + "'";
    return record.data.AlarmInterpret;
};

var onAlarmsGridRowContextMenuShow = function(el, rowIndex, e) {
    e.preventDefault();
    el.getSelectionModel().selectRow(rowIndex);
    AlarmsGridRowMenu.dataRecord = el.store.getAt(rowIndex);
    if (AlarmsGridRowMenu.dataRecord.data.MAlm === "MAlm") {
        AlarmsGridRowItem3.setDisabled(false);
    }
    else {
        AlarmsGridRowItem3.setDisabled(true);
    }
    AlarmsGridRowMenu.showAt(e.getXY());
};

var onAlarmsFinderClick = function(el) {
    var lscId = el.parentMenu.dataRecord.data.LscID;
    var nodeId = el.parentMenu.dataRecord.data.NodeID;
    if (!lscId || !nodeId) { return false; }
    if (curTreePanel != NavTreePanel) {
        curNode = null;
        NavPanel.setActiveTab(NavTreePanel);
    }

    X.Home.ConvertNodeIDToTreeID(lscId, nodeId, {
        success: function(result) {
            if (!Ext.isEmpty(result, false)) {
                var nodes = Ext.util.JSON.decode(result, true);
                if (nodes) { expandChildren(nodes); }
            }
        }
    });
};

var onMenuAlarmsFinderClick = function() {
    if (!AlarmsGridPanel.hasSelection()) { return false; }
    var record = AlarmsGridPanel.getSelectionModel().getSelected();
    if (!record) { return false; }
    var lscId = record.data.LscID;
    var nodeId = record.data.NodeID;
    if (!lscId || !nodeId) { return false; }
    if (curTreePanel != NavTreePanel) {
        curNode = null;
        NavPanel.setActiveTab(NavTreePanel);
    }

    X.Home.ConvertNodeIDToTreeID(lscId, nodeId, {
        success: function(result) {
            if (!Ext.isEmpty(result, false)) {
                var nodes = Ext.util.JSON.decode(result, true);
                if (nodes) { expandChildren(nodes); }
            }
        }
    });
};

var expandChildren = function(ids, i) {
    i = i || 0;
    var node = curTreePanel.getNodeById(ids[i]);
    if (node) {
        if (ids.length - 1 == i) {
            node.ensureVisible(function() {
                node.select();
                curNode = node;
                doNodesGridLoad();
            });
        }
        else { node.expand(false, false, expandChildren.createDelegate(this, [ids, ++i])); }
    }
};

var doPageRefresh = function(option) {
    option = option || 15;
    if (option & 1 > 0) { doAlarmsGridPageLoad(); }
    if (option & 2 > 0) { doNodesGridLoad(); }
    if (option & 4 > 0) { setGroupTreeIcons(); }
    if (option & 8 > 0) { setUDGroupTreeIcons(); }
};

var doAlarmsGridLoad = function() {
    AlarmsGridPagingToolbar.doLoad();
};

var doAlarmsGridPageLoad = function() {
    AlarmsGridPagingToolbar.doRefresh();
};

var doNodesGridLoad = function() {
    if (curNode && curTreePanel) {
        curTreePanel.fireEvent("click", curNode); 
    }
};

var setGroupTreeIcons = function() {
    X.Home.GetTreeIcons({
        success: function(result) {
            if (!Ext.isEmpty(result, false)) {
                var nodes = Ext.util.JSON.decode(result, true);
                if (nodes) {
                    var tree = NavTreePanel;
                    var root = tree.getRootNode();
                    var cls = nodes["iconCls"];
                    if (root && cls) {
                        var r = nodes[root.id];
                        if (r) { root.setIconCls(r); } else { root.setIconCls(cls); }
                        if (root.hasChildNodes()) {
                            root.eachChild(function(c) {
                                c.cascade(function(n) {
                                    var t = nodes[n.id];
                                    if (t) { n.setIconCls(t); } else { n.setIconCls(cls); }
                                });
                            });
                        }
                    }
                }
            }
        }
    });
};

var setUDGroupTreeIcons = function() {
    X.Home.GetUDGTreeIcons({
        success: function(result) {
            if (!Ext.isEmpty(result, false)) {
                var nodes = Ext.util.JSON.decode(result, true);
                if (nodes) {
                    var tree = UserDefindTreePanel;
                    var root = tree.getRootNode();
                    var cls = nodes["iconCls"];
                    if (root && cls) {
                        var r = nodes[root.id];
                        if (r) { root.setIconCls(r); } else { root.setIconCls(cls); }
                        if (root.hasChildNodes()) {
                            root.eachChild(function(c) {
                                c.cascade(function(n) {
                                    var t = nodes[n.id];
                                    if (t) { n.setIconCls(t); } else { n.setIconCls(cls); }
                                });
                            });
                        }
                    }
                }
            }
        }
    });
};

var setConfirmAlarm = function() {
    if (AlarmsGridPanel.hasSelection()) {
        var row = AlarmsGridPanel.getSelectionModel().getSelected();
        X.Home.SetConfirmAlarm(row.data.LscID, row.data.SerialNO, row.data.ConfirmMarking);
    }
};

var setPageConfirmAlarms = function() {
    X.Home.SetPageConfirmAlarms(AlarmsGridPagingToolbar.cursor, AlarmsGridPagingToolbar.pageSize);
};

var setAllConfirmAlarms = function() {
    X.Home.SetAllConfirmAlarms();
};

var onLSCPanelCmdClick = function(command, record, rowIndex, colIndex) {
    X.LSCManager.ShowCmdWindow(command, record.data.LscID, record.data.LscName, record.data.LscIP, record.data.LscPort, record.data.LscUID, record.data.LscPwd, record.data.BeatInterval, record.data.BeatDelay, record.data.DBServer, record.data.DBPort, record.data.DBName, record.data.DBUID, record.data.DBPwd, record.data.HisDBServer, record.data.HisDBPort, record.data.HisDBName, record.data.HisDBUID, record.data.HisDBPwd, record.data.Enabled, {
        success: function(result) {
            FormPanel1.clearInvalid();
            FormPanel2.clearInvalid();
        }
    });
};

var onLSCPanelAddClick = function () {
    X.LSCManager.ShowCmdWindow("Add", 0, "", "", 7000, "", "", 20, 20, "", 1433, "", "", "", "", 1433, "", "", "", true, {
        success: function (result) {
            FormPanel1.clearInvalid();
            FormPanel2.clearInvalid();
        }
    });
};

var onLSCPanelRefreshClick = function() {
    LSCPagingToolbar.doRefresh();
};

var onLSCGridRowEditClick = function(el) {
    onLSCPanelCmdClick("Edit", el.parentMenu.dataRecord, 0, 0);
};

var onLSCGridRowDeleteClick = function(el) {
    onLSCPanelCmdClick("Del", el.parentMenu.dataRecord, 0, 0);
};

var onLSCGridRowSyncClick = function(el) {
    onLSCPanelCmdClick("Sync", el.parentMenu.dataRecord, 0, 0);
};

var onLSCGridRowContextMenuShow = function(el, rowIndex, e) {
    e.preventDefault();
    el.getSelectionModel().selectRow(rowIndex);
    LSCGridRowMenu.dataRecord = el.store.getAt(rowIndex);
    LSCGridRowMenu.showAt(e.getXY());
};

var setLSCStatus = function(value) {
    var template = "<img alt=\"{0}\" title=\"{0}\" src=\"{1}\" width=\"16\" height=\"16\" />";
    return String.format(template, value ? LanguageSet.Connected : LanguageSet.Disconnected, value ? "Resources/images/Tips/T-Connected.png" : "Resources/images/Tips/T-Disconnected.png");
};

var setNavPanelWidth = function(el) {
    el.setWidth(PageSize.width * 0.25);
};

var setAlarmPanelHeight = function(el) {
    el.setHeight((PageSize.height - 113) * 0.5);
};

var setCollapseChecked = function(el) {
    if (el.checked) { el.setChecked(false); }
};

var setExpandChecked = function(el) {
    if (!el.checked) { el.setChecked(true); }
};

var openNewPage = function(url) {
    var link = document.createElement("a");
    link.setAttribute("href", encodeURI(url));
    link.setAttribute("target", "_blank");
    link.setAttribute("id", "createNewPageLink");
    document.body.appendChild(link);
    link.click();
};

var newColsCnt;
var createColumnTreePanel = function(combo, records) {
    newColsCnt = 0;
    X.ACSFilterSetting.CreateTreeNodes({
        success: function(result) {
            if (!Ext.isEmpty(result, false)) {
                var nodes = Ext.util.JSON.decode(result, true);
                if (nodes && nodes.length > 0) {
                    ColumnTreePanel.initChildren(nodes);
                } else {
                    ColumnTreePanel.getRootNode().removeChildren(); 
                }
                ColumnPropertyGrid1.setVisible(false);
                ColumnPropertyGrid2.setVisible(false);
                ColumnPropertyGrid3.setVisible(false);
            }
        },
        eventMask: {
            showMask: true,
            target: "customtarget",
            customTarget: ColumnTreePanel
        }
    });
};

var addColumnTreeNode = function(el, e) {
    if (LscsComboBox.store.getCount() == 0) { return false; }
    X.ACSFilterSetting.AddTreeNode(++newColsCnt, {
        success: function(result) {
            if (!Ext.isEmpty(result, false)) {
                var nodes = Ext.util.JSON.decode(result, true);
                if (nodes && nodes.length > 0) {
                    var tree = ColumnTreePanel;
                    tree.getRootNode().appendChild(nodes);
                    var node = tree.getNodeById(nodes[0].id);
                    if (node) { tree.fireEvent("click", node); }
                }
            }
        }
    });
};

var removeColumnTreeNode = function(el, e) {
    var tree = ColumnTreePanel;
    var node = tree.getSelectionModel().getSelectedNode();
    if (node) {
        tree.removeNode(node);
        var len = tree.getRootNode().childNodes.length;
        if (len > 0) {
            tree.fireEvent("click", tree.getRootNode().childNodes[len - 1]);
        }
        else {
            ColumnPropertyGrid1.setVisible(false);
            ColumnPropertyGrid2.setVisible(false);
            ColumnPropertyGrid3.setVisible(false);
        }
    }
};

var resetColumnTreeNode = function(el, e) {
    createColumnTreePanel();
};

var onColumnTreeItemClick = function(el, e) {
    X.ACSFilterSetting.SetPropertyGrid(el.text, el.attributes.filterType, el.attributes.filterItem, el.attributes.isNew, {
        eventMask: {
            showMask: true,
            target: "customtarget",
            customTarget: TCEastPanel
        }
    });
};

var onColumnPropertyChanged = function(e) {
    var tree = ColumnTreePanel;
    var el = e.grid,
    rec = e.record.data.name,
    newValue = e.value,
    node = tree.getSelectionModel().getSelectedNode();
    if (node && el) {
        switch (rec) {
            case "ColName":
                node.setText(newValue);
                break;
            case "FilterType":
                node.attributes.filterType = newValue;
                node.attributes.filterItem = "";
                tree.fireEvent("click", node);
                break;
            case "FilterItem":
                node.attributes.filterItem = newValue;
                break;
            case "FilterItem1":
                var v2 = el.getSource()["FilterItem2"];
                v2 = Ext.isEmpty(v2, false) ? 0 : v2;
                X.ACSFilterSetting.GetDateTimeString(newValue, v2, {
                    success: function(result) {
                        if (!Ext.isEmpty(result, false)) { node.attributes.filterItem = result; }
                    }
                });
                break;
            case "FilterItem2":
                var v1 = el.getSource()["FilterItem1"];
                v1 = Ext.isEmpty(v1, false) ? 0 : v1;
                X.ACSFilterSetting.GetDateTimeString(v1, newValue, {
                    success: function(result) {
                        if (!Ext.isEmpty(result, false)) { node.attributes.filterItem = result; }
                    }
                });
                break;
            default:
                break;
        }
    }
};

var createACVColumnTreePanel = function(combo, records) {
    newColsCnt = 0;
    X.ACVFilterSetting.CreateTreeNodes({
        success: function(result) {
            if (!Ext.isEmpty(result, false)) {
                var nodes = Ext.util.JSON.decode(result, true);
                if (nodes && nodes.length > 0) {
                    ColumnTreePanel.initChildren(nodes);
                } else {
                    ColumnTreePanel.getRootNode().removeChildren(); 
                }
                ColumnPropertyGrid.setVisible(false);
            }
        },
        eventMask: {
            showMask: true,
            target: "customtarget",
            customTarget: ColumnTreePanel
        }
    });
};

var onACVColumnTreeItemClick = function(el, e) {
    ColumnPropertyGrid.setProperty("ColName", el.text);
    ColumnPropertyGrid.setProperty("DevName", el.attributes.devName);
    ColumnPropertyGrid.setProperty("NodeType", el.attributes.vNodeType);
    ColumnPropertyGrid.setProperty("FilterType", el.attributes.filterType);
    ColumnPropertyGrid.setProperty("FilterItem", el.attributes.filterItem);
    ColumnPropertyGrid.setVisible(true);
};

var addACVColumnTreeNode = function(el, e) {
    if (LscsComboBox.store.getCount() == 0) { return false; }
    X.ACVFilterSetting.AddTreeNode(++newColsCnt, {
        success: function(result) {
            if (!Ext.isEmpty(result, false)) {
                var nodes = Ext.util.JSON.decode(result, true);
                if (nodes && nodes.length > 0) {
                    var tree = ColumnTreePanel;
                    tree.getRootNode().appendChild(nodes);
                    var node = tree.getNodeById(nodes[0].id);
                    if (node) { tree.fireEvent("click", node); }
                }
            }
        }
    });
};

var removeACVColumnTreeNode = function(el, e) {
    var tree = ColumnTreePanel;
    var node = tree.getSelectionModel().getSelectedNode();
    if (node) {
        tree.removeNode(node);
        var len = tree.getRootNode().childNodes.length;
        if (len > 0) {
            tree.fireEvent("click", tree.getRootNode().childNodes[len - 1]);
        } else {
            ColumnPropertyGrid.setVisible(false); 
        }
    }
};

var resetACVColumnTreeNode = function(el, e) {
    createACVColumnTreePanel();
};

var onACVColumnPropertyChanged = function(e) {
    var rec = e.record.data.name,
    newValue = e.value,
    node = ColumnTreePanel.getSelectionModel().getSelectedNode();
    if (node) {
        switch (rec) {
            case "ColName":
                node.setText(newValue);
                break;
            case "DevName":
                node.attributes.devName = newValue;
                break;
            case "NodeType":
                node.attributes.vNodeType = newValue;
                break;
            case "FilterType":
                node.attributes.filterType = newValue;
                break;
            case "FilterItem":
                node.attributes.filterItem = newValue;
                break;
            default:
                break;
        }
    }
};

var HisAlarms = {
    setGridRowClass: function(record) {
        if (!AlarmsGridRowItem1.checked) { return false; }
        return setGridRowColor(record);
    },
    showGridRowContextMenu: function(el, rowIndex, e) {
        e.preventDefault();
        el.getSelectionModel().selectRow(rowIndex);
        HisAlarmsGridRowMenu.dataRecord = el.store.getAt(rowIndex);
        if (HisAlarmsGridRowMenu.dataRecord.data.MAlm === "MAlm") {
            AlarmsGridRowItem3.setDisabled(false);
        } else {
            AlarmsGridRowItem3.setDisabled(true);
        }
        HisAlarmsGridRowMenu.showAt(e.getXY());
    },
    collapseChecked: function() {
        AlarmsGridRowItem2.setChecked(false);
    },
    expandChecked: function() {
        AlarmsGridRowItem2.setChecked(true);
    },
    showSubPanel: function(el, e) {
        if (el.checked) {
            SubAlarmsPanel.expand();
        } else {
            SubAlarmsPanel.collapse();
        }
    }
};

var AlarmWnd = {
    setGridRowClass: function(record) {
        if (!AlarmWndGridRowItem2.checked) { return false; }
        return setGridRowColor(record);
    },
    showGridRowContextMenu: function(el, rowIndex, e) {
        e.preventDefault();
        el.getSelectionModel().selectRow(rowIndex);
        AlarmWndGridRowMenu.dataRecord = el.store.getAt(rowIndex);
        AlarmWndGridRowMenu.showAt(e.getXY());
    },
    collapseChecked: function() {
        AlarmWndGridRowItem3.setChecked(false);
    },
    expandChecked: function() {
        AlarmWndGridRowItem3.setChecked(true);
    },
    showSubPanel: function(el, e) {
        if (el.checked) {
            AlarmWndSubPanel.expand();
        } else {
            AlarmWndSubPanel.collapse();
        }
    }
};

var HisCategory = {
    onGridContextMenuShow: function(el, rowIndex, cellIndex, e) {
        e.preventDefault();
        el.getSelectionModel().select(rowIndex, cellIndex, false, false);
        HisCategoryContextMenu.dataRecord = el.store.getAt(rowIndex);
        HisCategoryContextMenu.showAt(e.getXY());
    },
    onGridCellDblClick: function(grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled === "0") { return false; }

        var record = grid.store.getAt(rowIndex);
        if (record) {
            rowIndex = record.data.Data0 - 1;
            var title = String.format("{0} {1}{2}", record.data.Data2, column.header, LanguageSet.AlarmWndName);
            X.HisAlarmsCategory.ShowGridCellDetail(title, rowIndex, column.dataIndex);
        }
    }
};

var SysLogs = {
    setMessageTips: function(data, metadata, record, rowIndex, columnIndex, store) {
        metadata.attr = "ext:hide='false' ext:qtip='" + record.data.Message + "'";
        return record.data.Message;
    }
};

var ServiceLogs = {
    setMessageTips: function(data, metadata, record, rowIndex, columnIndex, store) {
        metadata.attr = "ext:hide='false' ext:qtip='" + record.data.Message + "'";
        return record.data.Message;
    }
};

var AppointMgr = {
    onGridCmdClick: function (command, record, rowIndex, colIndex) {
        this.ShowCmdWindow(command, record.data.LscID, record.data.Id);
    },
    onGridAddClick: function () {
        this.ShowCmdWindow("Add", 0, 0);
    },
    ShowCmdWindow: function (command, lscId, id) {
        X.AppointmentManager.ShowCmdWindow(command, lscId, id, {
            success: function (result) {
                AppointmentFormPanel.clearInvalid(); 
            }
        });
    },
    initStaTreePanel: function () {
        var tree = StaIncludedField.component;
        if (tree.rendered) {
            X.AppointmentManager.InitStaTreePanel({
                success: function (result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length != 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: 'customtarget',
                    customTarget: tree
                }
            });
        }
    },
    initDevTreePanel: function () {
        var tree = DevIncludedField.component;
        if (tree.rendered) {
            X.AppointmentManager.InitDevTreePanel({
                success: function (result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length > 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: "customtarget",
                    customTarget: tree
                }
            });
        }
    },
    LscChanged: function (combo, record, index) {
        StaIncludedField.setValue("", "", false);
        DevIncludedField.setValue("", "", false);
        AppointMgr.initStaTreePanel();
        AppointMgr.initDevTreePanel();

        BookingUserNameTextField.setValue(record.data.LscUserName);
        BookingUserPhoneTextField.setValue(record.data.LscUserMobilePhone);
        LscCheckbox.setValue(false);

        WProjectComboBox.clearValue();
        WProjectComboBox.store.reload();
    },
    ProjectLoaded: function (store, records, options) {
        if (store.getCount() == 0) return false;
        var value = JsonValueHF.getValue();
        if (!Ext.isEmpty(value, false)) {
            var json = Ext.util.JSON.decode(value, true);
            if (!Ext.isEmpty(json, false)) {
                WProjectComboBox.setValueAndFireSelect(json.ProjectId);
            } else {
                WProjectComboBox.setValueAndFireSelect(store.getAt(0).get('Id'));
            }
        } else {
            WProjectComboBox.setValueAndFireSelect(store.getAt(0).get('Id'));
        }
    },
    ProjectChanged: function (combo, record, index) {
        var value = JsonValueHF.getValue();
        if (!Ext.isEmpty(value, false)) {
            var json = Ext.util.JSON.decode(value, true);
            if (!Ext.isEmpty(json, false)) {
                if (!Ext.isEmpty(json.StaIncluded, false)) {
                    StaIncludedField.setValue(json.StaIncluded, json.StaIncluded);
                }
                if (!Ext.isEmpty(json.DevIncluded, false)) {
                    DevIncludedField.setValue(json.DevIncluded, json.DevIncluded);
                }
                StartTimeTextField.setValue(Ext.util.Format.date(json.StartTime, 'Y-m-d H:i:s'));
                EndTimeTextField.setValue(Ext.util.Format.date(json.EndTime, 'Y-m-d H:i:s'));
                BookingUserNameTextField.setValue(json.BookingUserName);
                BookingUserPhoneTextField.setValue(json.BookingUserPhone);
                LscCheckbox.setValue(json.LscIncluded > 0);
            }
        }
    },
    Save: function (el, e) {
        if (!AppointmentFormPanel.getForm().isValid()) return false;
        if (Ext.isEmpty(StaIncludedField.getValue(), false)
            && Ext.isEmpty(DevIncludedField.getValue(), false) && !LscCheckbox.getValue()) {
            AppointmentStatusBar.setStatus({ text: LanguageSet.InvalidForm, iconCls: 'icon-exclamation' });
            return false;
        }

        AppointmentStatusBar.setStatus({ text: LanguageSet.Saving, iconCls: 'icon-loading' });
        X.AppointmentManager.Save({
            success: function (result) {
                if (!Ext.isEmpty(result, false)) {
                    var msg = Ext.util.JSON.decode(result, true);
                    if (msg) {
                        if (msg.Status == 200) {
                            AppointmentStatusBar.setStatus({ text: msg.Msg, iconCls: 'icon-tick' });
                            AppointmentGridPagingToolbar.doLoad();
                        } else {
                            AppointmentStatusBar.setStatus({ text: msg.Msg, iconCls: 'icon-exclamation' });
                        }
                    }
                }
            }
        });
    }
};

var SpSetting = {
    CurNode: null,
    Lock: false,
    OnSpTreeItemClick: function(el, e) {
        this.CurNode = el;
        this.Lock = true;
        DisConnectEnabled.setValue(el.attributes.SpDisconnect === "True" ? true : false);
        AL1Enabled.setValue(el.attributes.AL1Enabled === "True" ? true : false);
        AL2Enabled.setValue(el.attributes.AL2Enabled === "True" ? true : false);
        AL3Enabled.setValue(el.attributes.AL3Enabled === "True" ? true : false);
        AL4Enabled.setValue(el.attributes.AL4Enabled === "True" ? true : false);
        DevNameField.setValue(el.attributes.SpDevFilter);
        NodeNameField.setValue(el.attributes.SpNodeFilter);
        LoopEnabled.setValue(el.attributes.SpLoop === "True" ? true : false);
        SpArea2.setValue(el.attributes.SpArea2 === "True" ? true : false);
        SpArea3.setValue(el.attributes.SpArea3 === "True" ? true : false);
        SpStation.setValue(el.attributes.SpStation === "True" ? true : false);
        SpDevice.setValue(el.attributes.SpDevice === "True" ? true : false);
        SpNode.setValue(el.attributes.SpNode === "True" ? true : false);
        SpALDesc.setValue(el.attributes.SpALDesc === "True" ? true : false);
        this.Lock = false;
    },
    CreateTreeNodes: function(el, e) {
        X.SpeechSetting.CreateTreeNodes({
            success: function(result) {
                var tree = ColumnTreePanel;
                if (!Ext.isEmpty(result, false)) {
                    var nodes = Ext.util.JSON.decode(result, true);
                    if (nodes && nodes.length > 0) {
                        tree.initChildren(nodes);
                        tree.fireEvent("click", tree.getRootNode().childNodes[0]);
                    } else { tree.getRootNode().removeChildren(); }
                }
            },
            eventMask: {
                showMask: true,
                target: "customtarget",
                customTarget: ColumnTreePanel
            }
        });
    },
    DisConnectCheck: function(el) {
        if (this.CurNode && !this.Lock) { this.CurNode.attributes.SpDisconnect = el.checked ? "True" : "False"; }
    },
    AL1Check: function(el) {
        if (this.CurNode && !this.Lock) { this.CurNode.attributes.AL1Enabled = el.checked ? "True" : "False"; }
    },
    AL2Check: function(el) {
        if (this.CurNode && !this.Lock) { this.CurNode.attributes.AL2Enabled = el.checked ? "True" : "False"; }
    },
    AL3Check: function(el) {
        if (this.CurNode && !this.Lock) { this.CurNode.attributes.AL3Enabled = el.checked ? "True" : "False"; }
    },
    AL4Check: function(el) {
        if (this.CurNode && !this.Lock) { this.CurNode.attributes.AL4Enabled = el.checked ? "True" : "False"; }
    },
    DevFilter: function(el, newValue, oldValue) {
        if (this.CurNode && !this.Lock) { this.CurNode.attributes.SpDevFilter = newValue; }
    },
    NodeFilter: function(el, newValue, oldValue) {
        if (this.CurNode && !this.Lock) { this.CurNode.attributes.SpNodeFilter = newValue; }
    },
    LoopCheck: function(el) {
        if (this.CurNode && !this.Lock) { this.CurNode.attributes.SpLoop = el.checked ? "True" : "False"; }
    },
    Area2Check: function(el) {
        if (this.CurNode && !this.Lock) { this.CurNode.attributes.SpArea2 = el.checked ? "True" : "False"; }
    },
    Area3Check: function(el) {
        if (this.CurNode && !this.Lock) { this.CurNode.attributes.SpArea3 = el.checked ? "True" : "False"; }
    },
    StationCheck: function(el) {
        if (this.CurNode && !this.Lock) { this.CurNode.attributes.SpStation = el.checked ? "True" : "False"; }
    },
    DeviceCheck: function(el) {
        if (this.CurNode && !this.Lock) { this.CurNode.attributes.SpDevice = el.checked ? "True" : "False"; }
    },
    NodeCheck: function(el) {
        if (this.CurNode && !this.Lock) { this.CurNode.attributes.SpNode = el.checked ? "True" : "False"; }
    },
    ALDescCheck: function(el) {
        if (this.CurNode && !this.Lock) { this.CurNode.attributes.SpALDesc = el.checked ? "True" : "False"; }
    }
};

var UDGSetting = {
    checkChange: true,
    checkChangeLock: false,
    setUDGroupName: function(el) {
        var node = el.getRootNode();
        if (node) { UDGroupName.setValue(node.text); }
    },
    setUDGroupTreeName: function(el, e) {
        var text = el.getRawValue();
        if (!Ext.isEmpty(text, false)) {
            var node = UDGSettingWestPanel.getRootNode();
            if (node) { node.setText(text); }
        }
    },
    groupNodesCheckChange: function(el, checked) {
        if (UDGSetting.checkChangeLock) { return false; }
        UDGSetting.checkChangeLock = true;
        el.select();
        if (el.hasChildNodes()) {
            el.expand(false);
            if (selectAll.checked) {
                el.eachChild(function(c) {
                    c.cascade(function(n) {
                        if (n.ui.rendered) {
                            n.ui.toggleCheck(checked);
                        } else {
                            n.attributes.checked = checked;
                        }
                    });
                });
            }
        }
        UDGSetting.checkChangeLock = false;
    },
    previewUDGroupTree: function(el, e) {
        var pn = UDGSettingCenterPanel.getRootNode();
        var tn = UDGSettingWestPanel.getRootNode();
        tn.removeAll(true);
        UDGSetting.previewUDGroupTreeCallback(pn, tn);
    },
    previewUDGroupTreeCallback: function(pn, tn) {
        if (pn && pn.hasChildNodes()) {
            pn.eachChild(function(c) {
                var tp = tn;
                if (Ext.isDefined(c.attributes.checked) && c.attributes.checked) {
                    var n = c.clone();
                    n.attributes.id = c.id;
                    delete n.attributes.checked;
                    tp = tp.appendChild(new Ext.tree.TreeNode(n.attributes));
                }
                UDGSetting.previewUDGroupTreeCallback(c, tp);
            });
        }
    },
    setGroupNodesFilter: function(el, e) {
        var lscId = LscIDHidden.getRawValue();
        if (!Ext.isEmpty(lscId, false)) {
            X.UserDefindGroupSetting.BuiltGroupNodesTree(lscId, {
                success: function(result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length > 0) {
                            UDGSettingCenterPanel.initChildren(nodes);
                        } else {
                            UDGSettingCenterPanel.getRootNode().removeChildren(); 
                        }
                        PreviewBtn.fireEvent("click");
                    }
                },
                eventMask: {
                    showMask: true,
                    target: "customtarget",
                    customTarget: UDGSettingCenterPanel
                }
            });
        }
    },
    dragUDGGroupNode: function(dropEvent) {
        if (dropEvent.point === "append") { return false; }
        if (dropEvent.dropNode.getDepth() != dropEvent.target.getDepth()) { return false; }
    }
};

var SysKPI = {
    onGridCellDblClick11: function (el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        var dataIndex = column.dataIndex;
        if (dataIndex != "DevCnt" && dataIndex != "AlarmLast" && dataIndex != "TotalLast") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            var title = String.format("{0} {1}", column.header, LanguageSet.AlarmWndName);
            rowIndex = record.data.ID - 1;
            X.SysKPI.ShowGridCellDetail("1x", title, rowIndex, dataIndex);
        }
    },
    onGridCellDblClick21: function (el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        var dataIndex = column.dataIndex;
        if (dataIndex != "AlarmCnt" && dataIndex != "TotalAlarmCnt") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            var title = String.format("{0} {1}", column.header, LanguageSet.AlarmWndName);
            rowIndex = record.data.ID - 1;
            X.SysKPI.ShowGridCellDetail("2x", title, rowIndex, dataIndex);
        }
    },
    onGridCellDblClick31: function (el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        var dataIndex = column.dataIndex;
        if (dataIndex != "SAlmCnt" && dataIndex != "OriginalBAlmCnt" && dataIndex != "BAlmCnt" && dataIndex != "TotalAlarmCnt") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            var title = String.format("{0} {1}", column.header, LanguageSet.AlarmWndName);
            rowIndex = record.data.ID - 1;
            X.SysKPI.ShowGridCellDetail("3x", title, rowIndex, dataIndex);
        }
    },
    onGridCellDblClick41: function (el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        var dataIndex = column.dataIndex;
        if (dataIndex != "SAlmCnt" && dataIndex != "OriginalBAlmCnt" && dataIndex != "BAlmCnt" && dataIndex != "TotalAlarmCnt") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            var title = String.format("{0} {1}", column.header, LanguageSet.AlarmWndName);
            rowIndex = record.data.ID - 1;
            X.SysKPI.ShowGridCellDetail("4x", title, rowIndex, dataIndex);
        }
    },
    onGridCellDblClick51: function (el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        var dataIndex = column.dataIndex;
        if (dataIndex != "TotalAlm" && dataIndex != "ConfirmAlm") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            var title = String.format("{0} {1}", column.header, LanguageSet.AlarmWndName);
            rowIndex = record.data.ID - 1;
            X.SysKPI.ShowGridCellDetail("5x", title, rowIndex, dataIndex);
        }
    },
    onGridCellDblClick61: function (el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        var dataIndex = column.dataIndex;
        if (dataIndex != "TotalAlm") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            var title = String.format("{0} {1}", column.header, LanguageSet.AlarmWndName);
            rowIndex = record.data.ID - 1;
            X.SysKPI.ShowGridCellDetail("6x", title, rowIndex, dataIndex);
        }
    }
};

var StaCnt = {
    getValues: function(tree) {
        var ids = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function(node) {
            ids.push(node.id);
        });
        return ids.join(";");
    },
    getTexts: function(tree) {
        var texts = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function(node) {
            texts.push(node.text);
        });
        return texts.join(";");
    },
    syncValue: function(value) {
        var tree = this.component;
        if (tree.rendered) {
            X.StaCount.InitStaCountTreeNodes({
                success: function(result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length > 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: "customtarget",
                    customTarget: tree
                }
            });
        }
    },
    checkChanged: function(el, checked) {
        if (this.checkChangeLock) { return false; }
        this.checkChangeLock = true;
        el.select();
        if (el.hasChildNodes()) {
            el.expand(false);
            el.eachChild(function(c) {
                c.cascade(function(n) {
                    if (n.ui.rendered) {
                        n.ui.toggleCheck(checked);
                    } else {
                        n.attributes.checked = checked;
                    }
                });
            });
        }
        this.checkChangeLock = false;
        this.dropDownField.setValue(StaCnt.getValues(this), StaCnt.getTexts(this), false);
    },
    onGridCellDblClick: function(el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled === "0") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            rowIndex = record.data.Data0 - 1;
            var title = LanguageSet.StaWndName;
            if (column.titleStyle === "2") {
                title = String.format("{0} [{1}]{2}", record.data.Data1, column.header, LanguageSet.StaWndName);
            }
            else if (column.titleStyle === "3") {
                title = String.format("{0} {1} [{2}]{3}", record.data.Data1, record.data.Data2, column.header, LanguageSet.StaWndName);
            }
            else if (column.titleStyle === "4") {
                title = String.format("{0} {1} {2} [{3}]{4}", record.data.Data1, record.data.Data2, record.data.Data3, column.header, LanguageSet.StaWndName);
            }

            X.StaCount.ShowGridCellDetail(title, rowIndex, column.dataIndex);
        }
    }
};

var StaWnd = {
    showGridRowContextMenu: function(el, rowIndex, e) {
        e.preventDefault();
        el.getSelectionModel().selectRow(rowIndex);
        StaWndGridRowMenu.dataRecord = el.store.getAt(rowIndex);
        StaWndGridRowMenu.showAt(e.getXY());
    }
};

var DevCnt = {
    getValues: function(tree) {
        var ids = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function(node) {
            ids.push(node.id);
        });
        return ids.join(";");
    },
    getTexts: function(tree) {
        var texts = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function(node) {
            texts.push(node.text);
        });
        return texts.join(";");
    },
    syncValue: function(value) {
        var tree = this.component;
        if (tree.rendered) {
            X.DevCount.InitDevCountTreeNodes({
                success: function(result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length > 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: "customtarget",
                    customTarget: tree
                }
            });
        }
    },
    checkChanged: function(el, checked) {
        if (this.checkChangeLock) { return false; }
        this.checkChangeLock = true;
        el.select();
        if (el.hasChildNodes()) {
            el.expand(false);
            el.eachChild(function(c) {
                c.cascade(function(n) {
                    if (n.ui.rendered) {
                        n.ui.toggleCheck(checked);
                    } else {
                        n.attributes.checked = checked;
                    }
                });
            });
        }
        this.checkChangeLock = false;
        this.dropDownField.setValue(DevCnt.getValues(this), DevCnt.getTexts(this), false);
    },
    onGridCellDblClick: function(el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled === "0") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            rowIndex = record.data.Data0 - 1;
            var title = LanguageSet.DevWndName;
            if (column.titleStyle === "2") {
                title = String.format("{0} [{1}]{2}", record.data.Data1, column.header, LanguageSet.DevWndName);
            }
            else if (column.titleStyle === "3") {
                title = String.format("{0} {1} [{2}]{3}", record.data.Data1, record.data.Data2, column.header, LanguageSet.DevWndName);
            }
            else if (column.titleStyle === "4") {
                title = String.format("{0} {1} {2} [{3}]{4}", record.data.Data1, record.data.Data2, record.data.Data3, column.header, LanguageSet.DevWndName);
            }
            else if (column.titleStyle === "5") {
                title = String.format("{0} {1} {2} {3} [{4}]{5}", record.data.Data1, record.data.Data2, record.data.Data3, record.data.Data4, column.header, LanguageSet.DevWndName);
            }
            else if (column.titleStyle === "6") {
                title = String.format("{0} {1} {2} {3} {4} [{5}]{6}", record.data.Data1, record.data.Data2, record.data.Data3, record.data.Data4, record.data.Data5, column.header, LanguageSet.DevWndName);
            }

            X.DevCount.ShowGridCellDetail(title, rowIndex, column.dataIndex);
        }
    }
};

var DevWnd = {
    showGridRowContextMenu: function(el, rowIndex, e) {
        e.preventDefault();
        el.getSelectionModel().selectRow(rowIndex);
        DevWndGridRowMenu.dataRecord = el.store.getAt(rowIndex);
        DevWndGridRowMenu.showAt(e.getXY());
    }
};

var TrendMgr = {
    GetGridRowClass: function(record) {
        if (!TrendGridRowMenuItem3.checked) { return false; }
        return setGridRowColor(record);
    },
    GetCountGridRowClass: function(record) {
        if (!TrendCountGridRowMenuItem1.checked) { return false; }
        return setGridRowColor(record);
    },
    GetHisGridRowClass: function(record) {
        if (!TrendHisGridRowMenuItem1.checked) { return false; }
        return setGridRowColor(record);
    },
    ShowGridRowMenu: function(el, rowIndex, e) {
        e.preventDefault();
        el.getSelectionModel().selectRow(rowIndex);
        TrendGridRowMenu.dataRecord = el.store.getAt(rowIndex);
        TrendGridRowMenu.showAt(e.getXY());
    },
    ShowCountGridRowMenu: function(el, rowIndex, e) {
        e.preventDefault();
        el.getSelectionModel().selectRow(rowIndex);
        TrendCountGridRowMenu.dataRecord = el.store.getAt(rowIndex);
        TrendCountGridRowMenu.showAt(e.getXY());
    },
    ShowHisGridRowMenu: function(el, rowIndex, e) {
        e.preventDefault();
        el.getSelectionModel().selectRow(rowIndex);
        TrendHisGridRowMenu.dataRecord = el.store.getAt(rowIndex);
        TrendHisGridRowMenu.showAt(e.getXY());
    },
    LoadPageTrendAlarms: function() {
        if (TrendGridToolbars.loaded1
        && TrendGridToolbars.loaded2
        && TrendGridToolbars.loaded3) {
            TrendGridPagingToolbar.doRefresh();
        }
    }
};

var LoadMgr = {
    GetGridRowClass: function(record) {
        if (!LoadGridRowMenuItem3.checked) { return false; }
        return setGridRowColor(record);
    },
    GetHisGridRowClass: function(record) {
        if (!LoadHisGridRowMenuItem1.checked) { return false; }
        return setGridRowColor(record);
    },
    ShowGridRowMenu: function(el, rowIndex, e) {
        e.preventDefault();
        el.getSelectionModel().selectRow(rowIndex);
        LoadGridRowMenu.dataRecord = el.store.getAt(rowIndex);
        LoadGridRowMenu.showAt(e.getXY());
    },
    ShowHisGridRowMenu: function(el, rowIndex, e) {
        e.preventDefault();
        el.getSelectionModel().selectRow(rowIndex);
        LoadHisGridRowMenu.dataRecord = el.store.getAt(rowIndex);
        LoadHisGridRowMenu.showAt(e.getXY());
    },
    LoadPageLoadAlarms: function() {
        if (LoadGridToolbars.loaded1
        && LoadGridToolbars.loaded2
        && LoadGridToolbars.loaded3) {
            LoadGridPagingToolbar.doRefresh();
        }
    }
};

var FreMgr = {
    GetGridRowClass: function(record) {
        if (!FreGridRowMenuItem3.checked) { return false; }
        return setGridRowColor(record);
    },
    GetHisGridRowClass: function(record) {
        if (!FreHisGridRowMenuItem1.checked) { return false; }
        return setGridRowColor(record);
    },
    ShowGridRowMenu: function(el, rowIndex, e) {
        e.preventDefault();
        el.getSelectionModel().selectRow(rowIndex);
        FreGridRowMenu.dataRecord = el.store.getAt(rowIndex);
        FreGridRowMenu.showAt(e.getXY());
    },
    ShowHisGridRowMenu: function(el, rowIndex, e) {
        e.preventDefault();
        el.getSelectionModel().selectRow(rowIndex);
        FreHisGridRowMenu.dataRecord = el.store.getAt(rowIndex);
        FreHisGridRowMenu.showAt(e.getXY());
    },
    LoadPageFreAlarms: function() {
        if (FreGridToolbars.loaded1
        && FreGridToolbars.loaded2
        && FreGridToolbars.loaded3) {
            FreGridPagingToolbar.doRefresh();
        }
    }
};

var BatCR = {
    onGridCellDblClick: function(grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (record) {
                X.BatCountReport.ShowGridCellDetail('', record.data.LscID, record.data.DevID, record.data.DevIndex);
            }
        }
    }
};

var BatWnd = {
    showGridRowContextMenu: function(el, rowIndex, e) {
        e.preventDefault();
        el.getSelectionModel().selectRow(rowIndex);
        BatWndGridRowMenu.dataRecord = el.store.getAt(rowIndex);
        BatWndGridRowMenu.showAt(e.getXY());
    }
};

var DivWnd = {
    showGridRowContextMenu: function (el, rowIndex, e) {
        e.preventDefault();
        el.getSelectionModel().selectRow(rowIndex);
        WndGridRowMenu.dataRecord = el.store.getAt(rowIndex);
        WndGridRowMenu.showAt(e.getXY());
    }
};

var KPIReport104 = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                X.report104.ShowGridCellDetail(LanguageSet.AlarmWndName, record.data.LscID, record.data.Area3ID, record.data.BuildingID);
            }
        }
    }
};

var KPIReport105 = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                X.report105.ShowGridCellDetail(LanguageSet.AlarmWndName, record.data.LscID, record.data.Area3ID, record.data.BuildingID);
            }
        }
    }
};

var KPIReport106 = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                X.report106.ShowGridCellDetail(LanguageSet.AlarmWndName, record.data.LscID, record.data.Area3ID, record.data.BuildingID);
            }
        }
    }
};

var KPIReport107 = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                X.report107.ShowGridCellDetail(LanguageSet.AlarmWndName, record.data.LscID, record.data.Area3ID, record.data.BuildingID);
            }
        }
    }
};

var KPIReport108 = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                X.report108.ShowGridCellDetail(LanguageSet.AlarmWndName, record.data.LscID, record.data.Area3ID, record.data.BuildingID);
            }
        }
    }
};

var KPIReport109 = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                rowIndex = record.data.ID - 1;
                X.report109.ShowGridCellDetail(LanguageSet.StaWndName, record.data.LscID, column.dataIndex);
            }
        }
    }
};

var KPIReport110 = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                rowIndex = record.data.ID - 1;
                X.report110.ShowGridCellDetail(LanguageSet.StaWndName, rowIndex, column.dataIndex);
            }
        }
    }
};

var KPIReport111 = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                X.report111.ShowGridCellDetail(LanguageSet.AlarmWndName, record.data.LscID);
            }
        }
    }
};

var KPIReport112 = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                rowIndex = record.data.ID - 1;
                X.report112.ShowGridCellDetail(LanguageSet.StaWndName, record.data.LscID, column.dataIndex);
            }
        }
    }
};

var KPIReport113 = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                rowIndex = record.data.ID - 1;
                X.report113.ShowGridCellDetail(LanguageSet.DevWndName, record.data.LscID, column.dataIndex);
            }
        }
    }
};

var KPIReport114 = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                rowIndex = record.data.ID - 1;
                X.report114.ShowGridCellDetail(LanguageSet.DevWndName, record.data.LscID, column.dataIndex);
            }
        }
    }
};

var KPIReport115 = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                rowIndex = record.data.ID - 1;
                X.report115.ShowGridCellDetail(LanguageSet.StaWndName, record.data.LscID, column.dataIndex);
            }
        }
    }
};

var KPIReport116 = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                X.report116.ShowGridCellDetail(LanguageSet.AlarmWndName, record.data.LscID);
            }
        }
    }
};

var KPIReport117 = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                rowIndex = record.data.ID - 1;
                X.report117.ShowGridCellDetail(LanguageSet.StaWndName, record.data.LscID, column.dataIndex);
            }
        }
    }
};

var ProjectMgr = {
    GridCmdClick: function (command, record, rowIndex, colIndex) {
        this.ShowCmdWindow(command, record.data.LscID, record.data.ProjectId);
    },
    GridAddClick: function () {
        this.ShowCmdWindow("Add", 0, 0);
    },
    ShowCmdWindow: function (command, lscId, projectId) {
        X.ProjectManager.ShowCmdWindow(command, lscId, projectId, {
            success: function (result) { ProjectFormPanel.clearInvalid(); }
        });
    },
    Save: function (el, e) {
        if (!ProjectFormPanel.getForm().isValid())
            return false;

        TipsStatusBar.setStatus({ text: LanguageSet.Saving, iconCls: 'icon-loading' });
        X.ProjectManager.Save({
            success: function (result) {
                if (!Ext.isEmpty(result, false)) {
                    var msg = Ext.util.JSON.decode(result, true);
                    if (msg) {
                        if (msg.Status == 200) {
                            TipsStatusBar.setStatus({ text: msg.Msg, iconCls: 'icon-tick' });
                            MainGridPagingToolbar.doLoad();
                        } else {
                            TipsStatusBar.setStatus({ text: msg.Msg, iconCls: 'icon-exclamation' });
                        }
                    }
                }
            }
        });
    }
};

var ImportantAlm = {
    getValues: function (tree) {
        var ids = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function (node) {
            ids.push(node.id);
        });
        return ids.join(";");
    },
    getTexts: function (tree) {
        var texts = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function (node) {
            texts.push(node.text);
        });
        return texts.join(";");
    },
    syncValue: function (value) {
        var tree = this.component;
        if (tree.rendered) {
            X.ImportantAlmReport.InitCountItemTreeNodes({
                success: function (result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length > 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: "customtarget",
                    customTarget: tree
                }
            });
        }
    },
    checkChanged: function (el, checked) {
        if (this.checkChangeLock) { return false; }
        this.checkChangeLock = true;
        el.select();
        if (el.hasChildNodes()) {
            el.expand(false);
            el.eachChild(function (c) {
                c.cascade(function (n) {
                    if (n.ui.rendered) {
                        n.ui.toggleCheck(checked);
                    } else {
                        n.attributes.checked = checked;
                    }
                });
            });
        }
        this.checkChangeLock = false;
        this.dropDownField.setValue(ImportantAlm.getValues(this), ImportantAlm.getTexts(this), false);
    },
    onGridCellDblClick: function (el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled === "0") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            rowIndex = record.data.Data0 - 1;
            var title = LanguageSet.AlarmWndName;
            X.ImportantAlmReport.ShowGridCellDetail(title, rowIndex, column.dataIndex);
        }
    }
};

var HisAlmFre = {
    getValues: function (tree) {
        var ids = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function (node) {
            ids.push(node.id);
        });
        return ids.join(";");
    },
    getTexts: function (tree) {
        var texts = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function (node) {
            texts.push(node.text);
        });
        return texts.join(";");
    },
    syncValue: function (value) {
        var tree = this.component;
        if (tree.rendered) {
            X.HisAlmFreReport.InitCountItemTreeNodes({
                success: function (result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length > 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: "customtarget",
                    customTarget: tree
                }
            });
        }
    },
    checkChanged: function (el, checked) {
        if (this.checkChangeLock) { return false; }
        this.checkChangeLock = true;
        el.select();
        if (el.hasChildNodes()) {
            el.expand(false);
            el.eachChild(function (c) {
                c.cascade(function (n) {
                    if (n.ui.rendered) {
                        n.ui.toggleCheck(checked);
                    } else {
                        n.attributes.checked = checked;
                    }
                });
            });
        }
        this.checkChangeLock = false;
        this.dropDownField.setValue(HisAlmFre.getValues(this), HisAlmFre.getTexts(this), false);
    },
    onGridCellDblClick: function (el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled === "0") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            rowIndex = record.data.Data0 - 1;
            var title = LanguageSet.AlarmWndName;
            X.HisAlmFreReport.ShowGridCellDetail(title, rowIndex, column.dataIndex);
        }
    }
};

var AlarmMinInterval = {
    getValues: function (tree) {
        var ids = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function (node) {
            ids.push(node.id);
        });
        return ids.join(";");
    },
    getTexts: function (tree) {
        var texts = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function (node) {
            texts.push(node.text);
        });
        return texts.join(";");
    },
    syncValue: function (value) {
        var tree = this.component;
        if (tree.rendered) {
            X.HisAlarmMinIntervalReport.InitCountItemTreeNodes({
                success: function (result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length > 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: "customtarget",
                    customTarget: tree
                }
            });
        }
    },
    checkChanged: function (el, checked) {
        if (this.checkChangeLock) { return false; }
        this.checkChangeLock = true;
        el.select();
        if (el.hasChildNodes()) {
            el.expand(false);
            el.eachChild(function (c) {
                c.cascade(function (n) {
                    if (n.ui.rendered) {
                        n.ui.toggleCheck(checked);
                    } else {
                        n.attributes.checked = checked;
                    }
                });
            });
        }
        this.checkChangeLock = false;
        this.dropDownField.setValue(AlarmMinInterval.getValues(this), AlarmMinInterval.getTexts(this), false);
    },
    onGridCellDblClick: function (el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled === "0") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            rowIndex = record.data.Data0 - 1;
            var title = LanguageSet.AlarmWndName;
            if (column.titleStyle === "2") {
                title = String.format("{0} {1}", record.data.Data1, title);
            }
            else if (column.titleStyle === "3") {
                title = String.format("{0} {1} {2}", record.data.Data1, record.data.Data2, title);
            }
            else if (column.titleStyle === "4") {
                title = String.format("{0} {1} {2} {3}", record.data.Data1, record.data.Data2, record.data.Data3, title);
            }
            else if (column.titleStyle === "5") {
                title = String.format("{0} {1} {2} {3} {4}", record.data.Data1, record.data.Data2, record.data.Data3, record.data.Data4, title);
            }

            X.HisAlarmMinIntervalReport.ShowGridCellDetail(title, rowIndex, column.dataIndex);
        }
    }
};

var AlarmMaxInterval = {
    getValues: function (tree) {
        var ids = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function (node) {
            ids.push(node.id);
        });
        return ids.join(";");
    },
    getTexts: function (tree) {
        var texts = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function (node) {
            texts.push(node.text);
        });
        return texts.join(";");
    },
    syncValue: function (value) {
        var tree = this.component;
        if (tree.rendered) {
            X.HisAlarmMaxIntervalReport.InitCountItemTreeNodes({
                success: function (result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length > 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: "customtarget",
                    customTarget: tree
                }
            });
        }
    },
    checkChanged: function (el, checked) {
        if (this.checkChangeLock) { return false; }
        this.checkChangeLock = true;
        el.select();
        if (el.hasChildNodes()) {
            el.expand(false);
            el.eachChild(function (c) {
                c.cascade(function (n) {
                    if (n.ui.rendered) {
                        n.ui.toggleCheck(checked);
                    } else {
                        n.attributes.checked = checked;
                    }
                });
            });
        }
        this.checkChangeLock = false;
        this.dropDownField.setValue(AlarmMaxInterval.getValues(this), AlarmMaxInterval.getTexts(this), false);
    },
    onGridCellDblClick: function (el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled === "0") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            rowIndex = record.data.Data0 - 1;
            var title = LanguageSet.AlarmWndName;
            if (column.titleStyle === "2") {
                title = String.format("{0} {1}", record.data.Data1, title);
            }
            else if (column.titleStyle === "3") {
                title = String.format("{0} {1} {2}", record.data.Data1, record.data.Data2, title);
            }
            else if (column.titleStyle === "4") {
                title = String.format("{0} {1} {2} {3}", record.data.Data1, record.data.Data2, record.data.Data3, title);
            }
            else if (column.titleStyle === "5") {
                title = String.format("{0} {1} {2} {3} {4}", record.data.Data1, record.data.Data2, record.data.Data3, record.data.Data4, title);
            }

            X.HisAlarmMaxIntervalReport.ShowGridCellDetail(title, rowIndex, column.dataIndex);
        }
    }
};

var MajorIncident = {
    getValues: function (tree) {
        var ids = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function (node) {
            ids.push(node.id);
        });
        return ids.join(";");
    },
    getTexts: function (tree) {
        var texts = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function (node) {
            texts.push(node.text);
        });
        return texts.join(";");
    },
    syncValue: function (value) {
        var tree = this.component;
        if (tree.rendered) {
            X.MajorIncidentReport.InitCountItemTreeNodes({
                success: function (result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length > 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: "customtarget",
                    customTarget: tree
                }
            });
        }
    },
    checkChanged: function (el, checked) {
        if (this.checkChangeLock) { return false; }
        this.checkChangeLock = true;
        el.select();
        if (el.hasChildNodes()) {
            el.expand(false);
            el.eachChild(function (c) {
                c.cascade(function (n) {
                    if (n.ui.rendered) {
                        n.ui.toggleCheck(checked);
                    } else {
                        n.attributes.checked = checked;
                    }
                });
            });
        }
        this.checkChangeLock = false;
        this.dropDownField.setValue(MajorIncident.getValues(this), MajorIncident.getTexts(this), false);
    },
    onGridCellDblClick: function (el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled === "0") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            rowIndex = record.data.Data0 - 1;
            var title = LanguageSet.AlarmWndName;
            X.MajorIncidentReport.ShowGridCellDetail(title, rowIndex, column.dataIndex);
        }
    },
    initAlarmNameTreePanel: function () {
        var tree = AlarmNameDropDownField.component;
        if (tree.rendered) {
            X.MajorIncidentReport.InitAlarmNameTreePanel({
                success: function (result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length != 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: 'customtarget',
                    customTarget: tree
                }
            });
        }
    }
};

var DevAlm = {
    getValues: function (tree) {
        var ids = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function (node) {
            ids.push(node.id);
        });
        return ids.join(";");
    },
    getTexts: function (tree) {
        var texts = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function (node) {
            texts.push(node.text);
        });
        return texts.join(";");
    },
    syncValue: function (value) {
        var tree = this.component;
        if (tree.rendered) {
            X.DevAlmReport.InitCountItemTreeNodes({
                success: function (result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length > 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: "customtarget",
                    customTarget: tree
                }
            });
        }
    },
    checkChanged: function (el, checked) {
        if (this.checkChangeLock) { return false; }
        this.checkChangeLock = true;
        el.select();
        if (el.hasChildNodes()) {
            el.expand(false);
            el.eachChild(function (c) {
                c.cascade(function (n) {
                    if (n.ui.rendered) {
                        n.ui.toggleCheck(checked);
                    } else {
                        n.attributes.checked = checked;
                    }
                });
            });
        }
        this.checkChangeLock = false;
        this.dropDownField.setValue(DevAlm.getValues(this), DevAlm.getTexts(this), false);
    },
    onGridCellDblClick: function (el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled === "0") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            rowIndex = record.data.Data0 - 1;
            if (column.dblClickEnabled === "1") {
                X.DevAlmReport.ShowAlarmDetail(LanguageSet.AlarmWndName, rowIndex, column.dataIndex);
            }
            else if (column.dblClickEnabled === "2") {
                X.DevAlmReport.ShowDevDetail(LanguageSet.DevWndName, rowIndex, column.dataIndex);
            }
        }
    },
    initAlarmNameTreePanel: function () {
        var tree = AlarmNameDropDownField.component;
        if (tree.rendered) {
            X.DevAlmReport.InitAlarmNameTreePanel({
                success: function (result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length != 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: 'customtarget',
                    customTarget: tree
                }
            });
        }
    }
};

var AlmStandard = {
    getValues: function (tree) {
        var ids = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function (node) {
            ids.push(node.id);
        });
        return ids.join(";");
    },
    getTexts: function (tree) {
        var texts = [], selNodes = tree.getChecked();
        Ext.each(selNodes, function (node) {
            texts.push(node.text);
        });
        return texts.join(";");
    },
    syncValue: function (value) {
        var tree = this.component;
        if (tree.rendered) {
            X.AlmStandardizationReport.InitCountItemTreeNodes({
                success: function (result) {
                    if (!Ext.isEmpty(result, false)) {
                        var nodes = Ext.util.JSON.decode(result, true);
                        if (nodes && nodes.length > 0) {
                            tree.initChildren(nodes);
                        } else {
                            tree.getRootNode().removeChildren();
                        }
                    }
                },
                eventMask: {
                    showMask: true,
                    target: "customtarget",
                    customTarget: tree
                }
            });
        }
    },
    checkChanged: function (el, checked) {
        if (this.checkChangeLock) { return false; }
        this.checkChangeLock = true;
        el.select();
        if (el.hasChildNodes()) {
            el.expand(false);
            el.eachChild(function (c) {
                c.cascade(function (n) {
                    if (n.ui.rendered) {
                        n.ui.toggleCheck(checked);
                    } else {
                        n.attributes.checked = checked;
                    }
                });
            });
        }
        this.checkChangeLock = false;
        this.dropDownField.setValue(AlmStandard.getValues(this), AlmStandard.getTexts(this), false);
    },
    onGridCellDblClick: function (el, rowIndex, columnIndex, e) {
        var column = el.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled === "0") { return false; }

        var record = el.store.getAt(rowIndex);
        if (record) {
            rowIndex = record.data.Data0 - 1;
            var title = LanguageSet.AlarmWndName;
            X.AlmStandardizationReport.ShowGridCellDetail(title, rowIndex, column.dataIndex);
        }
    }
};

var OilEngineReport = {
    GridCellDblClick: function (grid, rowIndex, columnIndex, e) {
        var column = grid.getColumnModel().config[columnIndex];
        if (column.dblClickEnabled && column.wndType && column.dblClickEnabled === "1") {
            var record = grid.store.getAt(rowIndex);
            if (!Ext.isEmpty(record)) {
                X.OilEngineReport.ShowGridCellDetail(LanguageSet.DivWndName, record.data.LscID, record.data.StaID, column.wndType);
            }
        }
    }
};