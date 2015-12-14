/*
* Map Device JavaScript Library v3.0.0
*
* Copyright 2013-2014, Steven
*
* Date: 2014/08/28
*/

var NavDev = {
    NG1: null,
    NG2: null,
    NG3: null,
    NG4: null,
    NG5: null,
    NTree: null,
    CurTreeNode: null,
    CurNodesPanel: null,
    EnmNodeType: { Null: -3, LSC: -2, Area: -1, Sta: 0, Dev: 1, Dic: 2, Aic: 3, Doc: 4, Aoc: 5, Str: 6, Img: 7, Sic: 9, SS: 10, RS: 11, RTU: 12 },
    EnmState: { NoAlarm: 0, Critical: 1, Major: 2, Minor: 3, Hint: 4, Opevent: 5, Invalid: 6 },
    InitClass: function() {
        this.NG1 = NodesGridTabPanel1;
        this.NG2 = NodesGridTabPanel2;
        this.NG3 = NodesGridTabPanel3;
        this.NG4 = NodesGridTabPanel4;
        this.NG5 = NodesGridTabPanel5;
        this.NTree = NavTreePanel;
    },
    OnTreeItemClick: function(el, e) {
        this.CurTreeNode = el;
        if (typeof (e) != "undefined") {
            this.ClearNodesGridStore();
        }

        if (!this.CurTreeNode.attributes.TreeNodeType) { return false; }
        switch (this.CurTreeNode.attributes.TreeNodeType) {
            case this.EnmNodeType.Sta:
                break;
            case this.EnmNodeType.Dev:
                this.LoadNodesGrid();
                break;
            default:
                break;
        }
        if (typeof (e) == "undefined") { return false; }
    },
    ClearNodesGridStore: function() {
        this.NG1.getStore().removeAll();
        this.NG2.getStore().removeAll();
        this.NG3.getStore().removeAll();
        this.NG4.getStore().removeAll();
        this.NG5.getStore().removeAll();
    },
    LoadNodesGrid: function() {
        if (!this.CurTreeNode || !this.CurNodesPanel) { return false; }
        var nodeType = this.CurTreeNode.attributes.TreeNodeType;
        if (nodeType != this.EnmNodeType.Dev) { return false; }
        switch (this.CurNodesPanel) {
            case this.NG1:
                X.NavDevWnd.GetAllNodes(this.CurTreeNode.id);
                break;
            case this.NG2:
                X.NavDevWnd.GetDINodes(this.CurTreeNode.id);
                break;
            case this.NG3:
                X.NavDevWnd.GetDONodes(this.CurTreeNode.id);
                break;
            case this.NG4:
                X.NavDevWnd.GetAINodes(this.CurTreeNode.id);
                break;
            case this.NG5:
                X.NavDevWnd.GetAONodes(this.CurTreeNode.id);
                break;
            default:
                break;
        }
    },
    OnNodesGridTabPanelActivate: function(el) {
        this.CurNodesPanel = el;
        this.ReLoadNodesGrid();
    },
    OnFilterTextFieldClick: function(el, e) {
        var field = DevFilterTextField,
            text = field.getRawValue(),
            tree = this.NTree;

        tree.clearFilter();
        if (Ext.isEmpty(text, false)) {
            return false;
        }

        var re = new RegExp(".*" + text + ".*", "i");
        tree.filterBy(function(node) {
            var match = re.test(node.text.replace(/<span>&nbsp;<\/span>/g, "")),
                pn = node.parentNode;

            if (match && node.isLeaf()) {
                pn.hasMatchNode = true;
            }

            if (pn != null && pn.fixed) {
                if (node.isLeaf() === false) {
                    node.fixed = true;
                }
                return true;
            }

            if (node.isLeaf() === false) {
                node.fixed = match;
                return match;
            }

            return (pn != null && pn.fixed) || match;
        }, { expandNodes: false });

        tree.getRootNode().cascade(function(node) {
            if (node.isRoot) {
                return;
            }

            if (node.isLeaf() === false) {
                node.expand(false, false);
            }

            delete node.fixed;
            delete node.hasMatchNode;
        }, tree);
    },
    SetGridNodesRowColor: function(record) {
        switch (record.data.Status) {
            case NavDev.EnmState.NoAlarm:
                return "grid-bg-NoAlarm";
            case NavDev.EnmState.Critical:
                return "grid-bg-Critical";
            case NavDev.EnmState.Major:
                return "grid-bg-Major";
            case NavDev.EnmState.Minor:
                return "grid-bg-Minor";
            case NavDev.EnmState.Hint:
                return "grid-bg-Hint";
            case NavDev.EnmState.Opevent:
                return "grid-bg-Opevent";
            case NavDev.EnmState.Invalid:
                return "grid-bg-Invalid";
            default:
                return "grid-bg-NoAlarm";
        }
    },
    PrepareCommand: function(grid, command, record, row) {
        switch (record.data.NodeType) {
            case NavDev.EnmNodeType.Doc:
                if (command.command === "Set") {
                    command.hidden = true;
                    command.hideMode = "display";
                }
                break;
            case NavDev.EnmNodeType.Aoc:
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
    },
    OnNodesGridTabPanelCmdClick: function(command, record, rowIndex, colIndex) {
        X.NavDevWnd.ShowControlWindow(command, record.data.LscID, record.data.NodeID, record.data.NodeType, record.data.NodeName);
    },
    ReLoadNodesGrid: function() {
        if (this.CurTreeNode && this.CurNodesPanel) {
            this.NTree.fireEvent("click", this.CurTreeNode);
        }
    },
    OnNodesGridRowContextMenuShow: function(el, rowIndex, e) {
        e.preventDefault();
        el.getSelectionModel().selectRow(rowIndex);
        NodesGridRowMenu.dataRecord = el.store.getAt(rowIndex);
        NodesGridRowMenu.showAt(e.getXY());
    }
};