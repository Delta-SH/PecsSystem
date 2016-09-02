using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "energy001")]
    public partial class energy001 : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if(!X.IsAjaxRequest) {
                ResourceManager.GetInstance().RegisterIcon(Icon.World);
                ResourceManager.GetInstance().RegisterIcon(Icon.House);
                ResourceManager.GetInstance().RegisterIcon(Icon.Building);

                var dt = DateTime.Today.AddMonths(-1);
                var bt = new DateTime(dt.Year, dt.Month, 1);
                var et = bt.AddMonths(1).AddSeconds(-1);
                StartDate.Text = WebUtility.GetDateString2(bt);
                EndDate.Text = WebUtility.GetDateString2(et);
            }
        }

        [DirectMethod(Timeout = 300000)]
        public string InitRangeNodes() {
            try {
                var root = new Ext.Net.TreeNode();
                root.Text = "全部";
                root.NodeID = "root";
                root.Icon = Icon.World;
                root.Expanded = true;
                root.SingleClickExpand = true;
                RangeTreePanel.Root.Clear();
                RangeTreePanel.Root.Add(root);

                var userData = UserData;
                foreach(var lscUser in userData.LscUsers) {
                    if(lscUser.Group == null) { continue; }
                    var node = new Ext.Net.TreeNode();
                    node.Text = lscUser.LscName;
                    node.NodeID = String.Format("{0}&{1}&{2}&{3}&{4}", lscUser.Group.LscID, lscUser.Group.GroupID, 0, (Int32)EnmNodeType.LSC, String.Empty);
                    node.Icon = Icon.House;
                    root.Nodes.Add(node);

                    RangeNodesLoaded(node, 0, lscUser.Group);
                }
                return RangeTreePanel.Root.ToJson();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
            return String.Empty;
        }

        private void RangeNodesLoaded(Ext.Net.TreeNode pnode, int pId, GroupInfo group) {
            try {
                var groupNodes = group.GroupNodes.FindAll(gti => { return gti.LastNodeID == pId; });
                foreach(var gti in groupNodes) {
                    if(gti.NodeType == EnmNodeType.Area) {
                        var node = new Ext.Net.TreeNode();
                        node.Text = gti.NodeName;
                        node.NodeID = String.Format("{0}&{1}&{2}&{3}&{4}", group.LscID, group.GroupID, gti.NodeID, (Int32)gti.NodeType, gti.Remark);
                        node.Icon = Icon.Building;
                        pnode.Nodes.Add(node);
                        RangeNodesLoaded(node, gti.NodeID, group);
                    }
                }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        protected void QueryButton_Click(object sender, DirectEventArgs e) {
            try {
                AddDataToCache();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        protected void SaveButton_Click(object sender, DirectEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "energy-001-report");
                var data = HttpRuntime.Cache[cacheKey] as List<EnergyInfo01>;
                if(data == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for(int i = 0; i < data.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var Current_Node = datas.CreateElement("Current");
                    Current_Node.InnerText = data[i].Current;
                    parent_Node.AppendChild(Current_Node);

                    var NHPAT_Node = datas.CreateElement("NHPAT");
                    NHPAT_Node.InnerText = data[i].NHPAT.ToString();
                    parent_Node.AppendChild(NHPAT_Node);

                    var NHPBT_Node = datas.CreateElement("NHPBT");
                    NHPBT_Node.InnerText = data[i].NHPBT.ToString();
                    parent_Node.AppendChild(NHPBT_Node);

                    var NHPCT_Node = datas.CreateElement("NHPCT");
                    NHPCT_Node.InnerText = data[i].NHPCT.ToString();
                    parent_Node.AppendChild(NHPCT_Node);

                    var NHPDT_Node = datas.CreateElement("NHPDT");
                    NHPDT_Node.InnerText = data[i].NHPDT.ToString();
                    parent_Node.AppendChild(NHPDT_Node);

                    var NHPET_Node = datas.CreateElement("NHPET");
                    NHPET_Node.InnerText = data[i].NHPET.ToString();
                    parent_Node.AppendChild(NHPET_Node);

                    var NHPFT_Node = datas.CreateElement("NHPFT");
                    NHPFT_Node.InnerText = data[i].NHPFT.ToString();
                    parent_Node.AppendChild(NHPFT_Node);

                    var NHPT_Node = datas.CreateElement("NHPT");
                    NHPT_Node.InnerText = data[i].NHPT.ToString();
                    parent_Node.AppendChild(NHPT_Node);

                    var NHPATRT_Node = datas.CreateElement("NHPATRT");
                    NHPATRT_Node.InnerText = String.Format("{0:P2}", data[i].NHPT != 0 ? data[i].NHPAT / data[i].NHPT : 0);
                    parent_Node.AppendChild(NHPATRT_Node);

                    var NHPBTRT_Node = datas.CreateElement("NHPBTRT");
                    NHPBTRT_Node.InnerText = String.Format("{0:P2}", data[i].NHPT != 0 ? data[i].NHPBT / data[i].NHPT : 0);
                    parent_Node.AppendChild(NHPBTRT_Node);

                    var NHPCTRT_Node = datas.CreateElement("NHPCTRT");
                    NHPCTRT_Node.InnerText = String.Format("{0:P2}", data[i].NHPT != 0 ? data[i].NHPCT / data[i].NHPT : 0);
                    parent_Node.AppendChild(NHPCTRT_Node);

                    var NHPDTRT_Node = datas.CreateElement("NHPDTRT");
                    NHPDTRT_Node.InnerText = String.Format("{0:P2}", data[i].NHPT != 0 ? data[i].NHPDT / data[i].NHPT : 0);
                    parent_Node.AppendChild(NHPDTRT_Node);

                    var NHPETRT_Node = datas.CreateElement("NHPETRT");
                    NHPETRT_Node.InnerText = String.Format("{0:P2}", data[i].NHPT != 0 ? data[i].NHPET / data[i].NHPT : 0);
                    parent_Node.AppendChild(NHPETRT_Node);

                    var NHPFTRT_Node = datas.CreateElement("NHPFTRT");
                    NHPFTRT_Node.InnerText = String.Format("{0:P2}", data[i].NHPT != 0 ? data[i].NHPFT / data[i].NHPT : 0);
                    parent_Node.AppendChild(NHPFTRT_Node);
                }

                var fileName = "energy001.xls";
                var sheetName = "energy001";
                var title = "动力环境监控中心系统 电量分类统计";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if(xls != null) { xls.Send(); }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        protected void MainStore_Refresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "energy-001-report");
                var result = HttpRuntime.Cache[cacheKey] as List<EnergyInfo01>;
                if(result == null) { result = AddDataToCache(); }
                if(result != null && result.Count > 0) {
                    if(end > result.Count) { end = result.Count; }
                    for(int i = start; i < end; i++) {
                        data.Add(new {
                            Key = result[i].Key,
                            Current = result[i].Current,
                            NHPAT = result[i].NHPAT,
                            NHPBT = result[i].NHPBT,
                            NHPCT = result[i].NHPCT,
                            NHPDT = result[i].NHPDT,
                            NHPET = result[i].NHPET,
                            NHPFT = result[i].NHPFT,
                            NHPT = result[i].NHPT,
                            NHPATRT = String.Format("{0:P2}", result[i].NHPT != 0 ? result[i].NHPAT / result[i].NHPT : 0),
                            NHPBTRT = String.Format("{0:P2}", result[i].NHPT != 0 ? result[i].NHPBT / result[i].NHPT : 0),
                            NHPCTRT = String.Format("{0:P2}", result[i].NHPT != 0 ? result[i].NHPCT / result[i].NHPT : 0),
                            NHPDTRT = String.Format("{0:P2}", result[i].NHPT != 0 ? result[i].NHPDT / result[i].NHPT : 0),
                            NHPETRT = String.Format("{0:P2}", result[i].NHPT != 0 ? result[i].NHPET / result[i].NHPT : 0),
                            NHPFTRT = String.Format("{0:P2}", result[i].NHPT != 0 ? result[i].NHPFT / result[i].NHPT : 0),
                        });
                    }
                }

                e.Total = (result != null ? result.Count : 0);
                MainStore.DataSource = data;
                MainStore.DataBind();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        [DirectMethod(Timeout = 300000)]
        public string GetCharts() {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "energy-001-report");
                var result = HttpRuntime.Cache[cacheKey] as List<EnergyInfo01>;
                if(result == null) { result = AddDataToCache(); }
                if(result != null && result.Count > 0) {
                    var pies = new List<string>();
                    var bars = new List<string>();
                    for(int i = 0; i < result.Count; i++) {
                        if(result[i].Key == "TOTAL") continue;
                        pies.Add(string.Format("{{\"name\":\"{0}\",\"value\":{1}}}", result[i].Current, result[i].NHPT));
                        bars.Add(string.Format("{{\"name\":\"{0}\",\"values\":[{{\"name\":\"开关电源\",\"value\":{1}}},{{\"name\":\"UPS\",\"value\":{2}}},{{\"name\":\"空调\",\"value\":{3}}},{{\"name\":\"照明\",\"value\":{4}}},{{\"name\":\"办公\",\"value\":{5}}},{{\"name\":\"综合\",\"value\":{6}}}]}}", result[i].Current, result[i].NHPAT, result[i].NHPBT, result[i].NHPCT, result[i].NHPDT, result[i].NHPET, result[i].NHPFT));
                    }

                    return string.Format("{{Pie:[{0}],Bar:[{1}]}}", string.Join(",", pies.ToArray()), string.Join(",", bars.ToArray()));
                }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
                Ext.Net.ResourceManager.AjaxSuccess = false;
                Ext.Net.ResourceManager.AjaxErrorMessage = err.Message;
            }

            return string.Empty;
        }

        [DirectMethod(Timeout = 300000)]
        public void ShowCellDetail(string key, string auxset, string title) {
            var userData = UserData;
            if(HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "energy-001-report")] == null) {
                AddDataToCache();
            }

            var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
            win.AutoLoad.Url = "~/Energy/Details/Detail001.aspx";
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "001"));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Key", key));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("AuxSet", auxset));
            win.Render();
            win.Show();
        }

        private List<EnergyInfo01> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "energy-001-report");
            HttpRuntime.Cache.Remove(cacheKey);

            if(String.IsNullOrEmpty(RangeField.RawValue.ToString())) { return new List<EnergyInfo01>(); }
            var period = (EnmPeriod)int.Parse(PeriodField.SelectedItem.Value);
            var startDate = DateTime.Parse(StartDate.RawText);
            var endDate = DateTime.Parse(EndDate.RawText).AddSeconds(86399);
            var ranges = RangeField.RawValue.ToString();
            var auxSet = new string[] { "NHPAT", "NHPBT", "NHPCT", "NHPDT", "NHPET", "NHPFT" };

            var result = new List<EnergyInfo01>();
            if(ranges == "root") {
                #region root
                var nodeEntity = new BNode();
                foreach(var lsc in userData.LscUsers) {
                    var def = this.GetEmptyEnergy(lsc.LscID.ToString(), lsc.LscName, period, startDate, endDate);
                    var nodes = nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Aic, null, auxSet, null);
                    if(nodes.Count > 0) {
                        var values = nodeEntity.GetElecValues(lsc.LscID, startDate, endDate, period);
                        var valWnodes = from node in nodes
                                        join value in values on node.NodeID equals value.NodeId
                                        select new {
                                            Start = value.Start,
                                            End = value.End,
                                            Value = value.Value,
                                            AuxSet = GetAuxset(auxSet, node.AuxSet)
                                        };

                        var valWgrp = from vn in valWnodes
                                      group vn by new { vn.Start, vn.End, vn.AuxSet } into g
                                      select new {
                                          Start = g.Key.Start,
                                          End = g.Key.End,
                                          Value = g.Sum(v => v.Value),
                                          AuxSet = g.Key.AuxSet
                                      };

                        var NHPATDetail = valWgrp.Where(m => m.AuxSet == "NHPAT");
                        def.NHPATDetail = (from dnd in def.NHPATDetail
                                           join nd in NHPATDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                           from defnd in lt.DefaultIfEmpty()
                                           select new EnergyDetailInfo01 {
                                               Start = dnd.Start,
                                               End = dnd.End,
                                               Value = defnd != null ? defnd.Value : dnd.Value
                                           }).ToList();

                        var NHPBTDetail = valWgrp.Where(m => m.AuxSet == "NHPBT");
                        def.NHPBTDetail = (from dnd in def.NHPBTDetail
                                           join nd in NHPBTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                           from defnd in lt.DefaultIfEmpty()
                                           select new EnergyDetailInfo01 {
                                               Start = dnd.Start,
                                               End = dnd.End,
                                               Value = defnd != null ? defnd.Value : dnd.Value
                                           }).ToList();

                        var NHPCTDetail = valWgrp.Where(m => m.AuxSet == "NHPCT");
                        def.NHPCTDetail = (from dnd in def.NHPCTDetail
                                           join nd in NHPCTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                           from defnd in lt.DefaultIfEmpty()
                                           select new EnergyDetailInfo01 {
                                               Start = dnd.Start,
                                               End = dnd.End,
                                               Value = defnd != null ? defnd.Value : dnd.Value
                                           }).ToList();

                        var NHPDTDetail = valWgrp.Where(m => m.AuxSet == "NHPDT");
                        def.NHPDTDetail = (from dnd in def.NHPDTDetail
                                           join nd in NHPDTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                           from defnd in lt.DefaultIfEmpty()
                                           select new EnergyDetailInfo01 {
                                               Start = dnd.Start,
                                               End = dnd.End,
                                               Value = defnd != null ? defnd.Value : dnd.Value
                                           }).ToList();

                        var NHPETDetail = valWgrp.Where(m => m.AuxSet == "NHPET");
                        def.NHPETDetail = (from dnd in def.NHPETDetail
                                           join nd in NHPETDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                           from defnd in lt.DefaultIfEmpty()
                                           select new EnergyDetailInfo01 {
                                               Start = dnd.Start,
                                               End = dnd.End,
                                               Value = defnd != null ? defnd.Value : dnd.Value
                                           }).ToList();

                        var NHPFTDetail = valWgrp.Where(m => m.AuxSet == "NHPFT");
                        def.NHPFTDetail = (from dnd in def.NHPFTDetail
                                           join nd in NHPFTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                           from defnd in lt.DefaultIfEmpty()
                                           select new EnergyDetailInfo01 {
                                               Start = dnd.Start,
                                               End = dnd.End,
                                               Value = defnd != null ? defnd.Value : dnd.Value
                                           }).ToList();
                        
                        
                        def.NHPAT = def.NHPATDetail.Sum(v => v.Value);
                        def.NHPBT = def.NHPBTDetail.Sum(v => v.Value);
                        def.NHPCT = def.NHPCTDetail.Sum(v => v.Value);
                        def.NHPDT = def.NHPDTDetail.Sum(v => v.Value);
                        def.NHPET = def.NHPETDetail.Sum(v => v.Value);
                        def.NHPFT = def.NHPFTDetail.Sum(v => v.Value);

                        var total = new List<EnergyDetailInfo01>();
                        total.AddRange(def.NHPATDetail);
                        total.AddRange(def.NHPBTDetail);
                        total.AddRange(def.NHPCTDetail);
                        total.AddRange(def.NHPDTDetail);
                        total.AddRange(def.NHPETDetail);
                        total.AddRange(def.NHPFTDetail);
                        def.NHPTDetail = (from tt in total
                                          group tt by new { tt.Start, tt.End } into g
                                          select new EnergyDetailInfo01 {
                                              Start = g.Key.Start,
                                              End = g.Key.End,
                                              Value = g.Sum(v => v.Value)
                                          }).ToList();

                        def.NHPT = def.NHPTDetail.Sum(v => v.Value);
                    }

                    result.Add(def);
                }
                #endregion
            } else {
                var keys = WebUtility.ItemSplit(ranges);
                if(keys.Length != 5) { return result; }

                var lscId = int.Parse(keys[0]);
                var groupId = int.Parse(keys[1]);
                var nodeId = int.Parse(keys[2]);
                var nodeType = int.Parse(keys[3]);
                var remark = keys[4];
                var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                if(enmNodeType == EnmNodeType.LSC) {
                    #region lsc
                    var lscUser = userData.LscUsers.Find(l => l.LscID == lscId);
                    if(lscUser != null) {
                        var nodeEntity = new BNode();
                        var values = new List<ElecValueInfo>();
                        var nodes = nodeEntity.GetNodes(lscUser.LscID, EnmNodeType.Aic, null, auxSet, null);
                        if(nodes.Count > 0)
                            values = nodeEntity.GetElecValues(lscUser.LscID, startDate, endDate, period);

                        var valWnodes = from node in nodes
                                        join value in values on node.NodeID equals value.NodeId
                                        select new {
                                            Area = node.Area2ID,
                                            Start = value.Start,
                                            End = value.End,
                                            Value = value.Value,
                                            AuxSet = GetAuxset(auxSet, node.AuxSet)
                                        };

                        var valWgrp = from vn in valWnodes
                                      group vn by new { vn.Area, vn.Start, vn.End, vn.AuxSet } into g
                                      select new {
                                          Area = g.Key.Area,
                                          Start = g.Key.Start,
                                          End = g.Key.End,
                                          Value = g.Sum(v => v.Value),
                                          AuxSet = g.Key.AuxSet
                                      };

                        var gnodes = lscUser.Group.GroupNodes.FindAll(gti => gti.NodeType == EnmNodeType.Area && gti.Remark.Equals("2"));
                        foreach(var gn in gnodes) {
                            var def = this.GetEmptyEnergy(string.Format("{0}-{1}", gn.LscID, gn.NodeID), gn.NodeName, period, startDate, endDate);
                            if(nodes.Count > 0 && values.Count > 0) {
                                var NHPATDetail = valWgrp.Where(m => m.Area == gn.NodeID && m.AuxSet == "NHPAT");
                                def.NHPATDetail = (from dnd in def.NHPATDetail
                                                   join nd in NHPATDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPBTDetail = valWgrp.Where(m => m.Area == gn.NodeID && m.AuxSet == "NHPBT");
                                def.NHPBTDetail = (from dnd in def.NHPBTDetail
                                                   join nd in NHPBTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPCTDetail = valWgrp.Where(m => m.Area == gn.NodeID && m.AuxSet == "NHPCT");
                                def.NHPCTDetail = (from dnd in def.NHPCTDetail
                                                   join nd in NHPCTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPDTDetail = valWgrp.Where(m => m.Area == gn.NodeID && m.AuxSet == "NHPDT");
                                def.NHPDTDetail = (from dnd in def.NHPDTDetail
                                                   join nd in NHPDTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPETDetail = valWgrp.Where(m => m.Area == gn.NodeID && m.AuxSet == "NHPET");
                                def.NHPETDetail = (from dnd in def.NHPETDetail
                                                   join nd in NHPETDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPFTDetail = valWgrp.Where(m => m.Area == gn.NodeID && m.AuxSet == "NHPFT");
                                def.NHPFTDetail = (from dnd in def.NHPFTDetail
                                                   join nd in NHPFTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                def.NHPAT = def.NHPATDetail.Sum(v => v.Value);
                                def.NHPBT = def.NHPBTDetail.Sum(v => v.Value);
                                def.NHPCT = def.NHPCTDetail.Sum(v => v.Value);
                                def.NHPDT = def.NHPDTDetail.Sum(v => v.Value);
                                def.NHPET = def.NHPETDetail.Sum(v => v.Value);
                                def.NHPFT = def.NHPFTDetail.Sum(v => v.Value);

                                var total = new List<EnergyDetailInfo01>();
                                total.AddRange(def.NHPATDetail);
                                total.AddRange(def.NHPBTDetail);
                                total.AddRange(def.NHPCTDetail);
                                total.AddRange(def.NHPDTDetail);
                                total.AddRange(def.NHPETDetail);
                                total.AddRange(def.NHPFTDetail);
                                def.NHPTDetail = (from tt in total
                                                  group tt by new { tt.Start, tt.End } into g
                                                  select new EnergyDetailInfo01 {
                                                      Start = g.Key.Start,
                                                      End = g.Key.End,
                                                      Value = g.Sum(v => v.Value)
                                                  }).ToList();
                                def.NHPT = def.NHPTDetail.Sum(v => v.Value);
                            }
                            result.Add(def);
                        }
                    }
                    #endregion
                } else if(enmNodeType == EnmNodeType.Area && remark.Equals("2")) {
                    #region area2
                    var lscUser = userData.LscUsers.Find(l => l.LscID == lscId);
                    if(lscUser != null) {
                        var nodeEntity = new BNode();
                        var values = new List<ElecValueInfo>();
                        var nodes = nodeEntity.GetNodes(lscUser.LscID, EnmNodeType.Aic, null, auxSet, null);
                        if(nodes.Count > 0)
                            values = nodeEntity.GetElecValues(lscUser.LscID, startDate, endDate, period);

                        var valWnodes = from node in nodes
                                        join value in values on node.NodeID equals value.NodeId
                                        select new {
                                            Area = node.Area3ID,
                                            Start = value.Start,
                                            End = value.End,
                                            Value = value.Value,
                                            AuxSet = GetAuxset(auxSet, node.AuxSet)
                                        };

                        var valWgrp = from vn in valWnodes
                                      group vn by new { vn.Area, vn.Start, vn.End, vn.AuxSet } into g
                                      select new {
                                          Area = g.Key.Area,
                                          Start = g.Key.Start,
                                          End = g.Key.End,
                                          Value = g.Sum(v => v.Value),
                                          AuxSet = g.Key.AuxSet
                                      };

                        var gnodes = lscUser.Group.GroupNodes.FindAll(gti => gti.NodeType == EnmNodeType.Area && gti.LastNodeID == nodeId);
                        foreach(var gn in gnodes) {
                            var def = this.GetEmptyEnergy(string.Format("{0}-{1}", gn.LscID, gn.NodeID), gn.NodeName, period, startDate, endDate);
                            if(nodes.Count > 0 && values.Count > 0) {
                                var NHPATDetail = valWgrp.Where(m => m.Area == gn.NodeID && m.AuxSet == "NHPAT");
                                def.NHPATDetail = (from dnd in def.NHPATDetail
                                                   join nd in NHPATDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPBTDetail = valWgrp.Where(m => m.Area == gn.NodeID && m.AuxSet == "NHPBT");
                                def.NHPBTDetail = (from dnd in def.NHPBTDetail
                                                   join nd in NHPBTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPCTDetail = valWgrp.Where(m => m.Area == gn.NodeID && m.AuxSet == "NHPCT");
                                def.NHPCTDetail = (from dnd in def.NHPCTDetail
                                                   join nd in NHPCTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPDTDetail = valWgrp.Where(m => m.Area == gn.NodeID && m.AuxSet == "NHPDT");
                                def.NHPDTDetail = (from dnd in def.NHPDTDetail
                                                   join nd in NHPDTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPETDetail = valWgrp.Where(m => m.Area == gn.NodeID && m.AuxSet == "NHPET");
                                def.NHPETDetail = (from dnd in def.NHPETDetail
                                                   join nd in NHPETDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPFTDetail = valWgrp.Where(m => m.Area == gn.NodeID && m.AuxSet == "NHPFT");
                                def.NHPFTDetail = (from dnd in def.NHPFTDetail
                                                   join nd in NHPFTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                def.NHPAT = def.NHPATDetail.Sum(v => v.Value);
                                def.NHPBT = def.NHPBTDetail.Sum(v => v.Value);
                                def.NHPCT = def.NHPCTDetail.Sum(v => v.Value);
                                def.NHPDT = def.NHPDTDetail.Sum(v => v.Value);
                                def.NHPET = def.NHPETDetail.Sum(v => v.Value);
                                def.NHPFT = def.NHPFTDetail.Sum(v => v.Value);

                                var total = new List<EnergyDetailInfo01>();
                                total.AddRange(def.NHPATDetail);
                                total.AddRange(def.NHPBTDetail);
                                total.AddRange(def.NHPCTDetail);
                                total.AddRange(def.NHPDTDetail);
                                total.AddRange(def.NHPETDetail);
                                total.AddRange(def.NHPFTDetail);
                                def.NHPTDetail = (from tt in total
                                                  group tt by new { tt.Start, tt.End } into g
                                                  select new EnergyDetailInfo01 {
                                                      Start = g.Key.Start,
                                                      End = g.Key.End,
                                                      Value = g.Sum(v => v.Value)
                                                  }).ToList();
                                def.NHPT = def.NHPTDetail.Sum(v => v.Value);
                            }
                            result.Add(def);
                        }
                    }
                    #endregion
                } else if(enmNodeType == EnmNodeType.Area && remark.Equals("3")) {
                    #region area3
                    var lscUser = userData.LscUsers.Find(l => l.LscID == lscId);
                    if(lscUser != null) {
                        var nodeEntity = new BNode();
                        var values = new List<ElecValueInfo>();
                        var nodes = nodeEntity.GetNodes(lscUser.LscID, EnmNodeType.Aic, null, auxSet, null);
                        if(nodes.Count > 0)
                            values = nodeEntity.GetElecValues(lscUser.LscID, startDate, endDate, period);

                        var valWnodes = from node in nodes
                                        join value in values on node.NodeID equals value.NodeId
                                        select new {
                                            Station = node.StaID,
                                            Start = value.Start,
                                            End = value.End,
                                            Value = value.Value,
                                            AuxSet = GetAuxset(auxSet, node.AuxSet)
                                        };

                        var valWgrp = from vn in valWnodes
                                      group vn by new { vn.Station, vn.Start, vn.End, vn.AuxSet } into g
                                      select new {
                                          Station = g.Key.Station,
                                          Start = g.Key.Start,
                                          End = g.Key.End,
                                          Value = g.Sum(v => v.Value),
                                          AuxSet = g.Key.AuxSet
                                      };

                        var gnodes = lscUser.Group.GroupNodes.FindAll(gti => gti.NodeType == EnmNodeType.Sta && gti.LastNodeID == nodeId);
                        foreach(var gn in gnodes) {
                            var def = this.GetEmptyEnergy(string.Format("{0}-{1}", gn.LscID, gn.NodeID), gn.NodeName, period, startDate, endDate);
                            if(nodes.Count > 0 && values.Count > 0) {
                                var NHPATDetail = valWgrp.Where(m => m.Station == gn.NodeID && m.AuxSet == "NHPAT");
                                def.NHPATDetail = (from dnd in def.NHPATDetail
                                                   join nd in NHPATDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPBTDetail = valWgrp.Where(m => m.Station == gn.NodeID && m.AuxSet == "NHPBT");
                                def.NHPBTDetail = (from dnd in def.NHPBTDetail
                                                   join nd in NHPBTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPCTDetail = valWgrp.Where(m => m.Station == gn.NodeID && m.AuxSet == "NHPCT");
                                def.NHPCTDetail = (from dnd in def.NHPCTDetail
                                                   join nd in NHPCTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPDTDetail = valWgrp.Where(m => m.Station == gn.NodeID && m.AuxSet == "NHPDT");
                                def.NHPDTDetail = (from dnd in def.NHPDTDetail
                                                   join nd in NHPDTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPETDetail = valWgrp.Where(m => m.Station == gn.NodeID && m.AuxSet == "NHPET");
                                def.NHPETDetail = (from dnd in def.NHPETDetail
                                                   join nd in NHPETDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                var NHPFTDetail = valWgrp.Where(m => m.Station == gn.NodeID && m.AuxSet == "NHPFT");
                                def.NHPFTDetail = (from dnd in def.NHPFTDetail
                                                   join nd in NHPFTDetail on new { dnd.Start, dnd.End } equals new { nd.Start, nd.End } into lt
                                                   from defnd in lt.DefaultIfEmpty()
                                                   select new EnergyDetailInfo01 {
                                                       Start = dnd.Start,
                                                       End = dnd.End,
                                                       Value = defnd != null ? defnd.Value : dnd.Value
                                                   }).ToList();

                                def.NHPAT = def.NHPATDetail.Sum(v => v.Value);
                                def.NHPBT = def.NHPBTDetail.Sum(v => v.Value);
                                def.NHPCT = def.NHPCTDetail.Sum(v => v.Value);
                                def.NHPDT = def.NHPDTDetail.Sum(v => v.Value);
                                def.NHPET = def.NHPETDetail.Sum(v => v.Value);
                                def.NHPFT = def.NHPFTDetail.Sum(v => v.Value);

                                var total = new List<EnergyDetailInfo01>();
                                total.AddRange(def.NHPATDetail);
                                total.AddRange(def.NHPBTDetail);
                                total.AddRange(def.NHPCTDetail);
                                total.AddRange(def.NHPDTDetail);
                                total.AddRange(def.NHPETDetail);
                                total.AddRange(def.NHPFTDetail);
                                def.NHPTDetail = (from tt in total
                                                  group tt by new { tt.Start, tt.End } into g
                                                  select new EnergyDetailInfo01 {
                                                      Start = g.Key.Start,
                                                      End = g.Key.End,
                                                      Value = g.Sum(v => v.Value)
                                                  }).ToList();
                                def.NHPT = def.NHPTDetail.Sum(v => v.Value);
                            }
                            result.Add(def);
                        }
                    }
                    #endregion
                }
            }

            #region total
            if(result.Count > 0) {
                var def = new EnergyInfo01 {
                    Key = "TOTAL",
                    Current = "总计",
                    NHPATDetail = null,
                    NHPAT = result.Sum(r => r.NHPAT),
                    NHPBTDetail = null,
                    NHPBT = result.Sum(r => r.NHPBT),
                    NHPCTDetail = null,
                    NHPCT = result.Sum(r => r.NHPCT),
                    NHPDTDetail = null,
                    NHPDT = result.Sum(r => r.NHPDT),
                    NHPETDetail = null,
                    NHPET = result.Sum(r => r.NHPET),
                    NHPFTDetail = null,
                    NHPFT = result.Sum(r => r.NHPFT),
                    NHPTDetail = null,
                    NHPT = result.Sum(r => r.NHPT)
                };

                var NHPATDetail = result.SelectMany(r => r.NHPATDetail);
                def.NHPATDetail = (from detail in NHPATDetail
                                   group detail by new { detail.Start, detail.End } into g
                                   select new EnergyDetailInfo01 {
                                       Start = g.Key.Start,
                                       End = g.Key.End,
                                       Value = g.Sum(v => v.Value)
                                   }).ToList();

                var NHPBTDetail = result.SelectMany(r => r.NHPBTDetail);
                def.NHPBTDetail = (from detail in NHPBTDetail
                                   group detail by new { detail.Start, detail.End } into g
                                   select new EnergyDetailInfo01 {
                                       Start = g.Key.Start,
                                       End = g.Key.End,
                                       Value = g.Sum(v => v.Value)
                                   }).ToList();

                var NHPCTDetail = result.SelectMany(r => r.NHPCTDetail);
                def.NHPCTDetail = (from detail in NHPCTDetail
                                   group detail by new { detail.Start, detail.End } into g
                                   select new EnergyDetailInfo01 {
                                       Start = g.Key.Start,
                                       End = g.Key.End,
                                       Value = g.Sum(v => v.Value)
                                   }).ToList();

                var NHPDTDetail = result.SelectMany(r => r.NHPDTDetail);
                def.NHPDTDetail = (from detail in NHPDTDetail
                                   group detail by new { detail.Start, detail.End } into g
                                   select new EnergyDetailInfo01 {
                                       Start = g.Key.Start,
                                       End = g.Key.End,
                                       Value = g.Sum(v => v.Value)
                                   }).ToList();

                var NHPETDetail = result.SelectMany(r => r.NHPETDetail);
                def.NHPETDetail = (from detail in NHPETDetail
                                   group detail by new { detail.Start, detail.End } into g
                                   select new EnergyDetailInfo01 {
                                       Start = g.Key.Start,
                                       End = g.Key.End,
                                       Value = g.Sum(v => v.Value)
                                   }).ToList();

                var NHPFTDetail = result.SelectMany(r => r.NHPFTDetail);
                def.NHPFTDetail = (from detail in NHPFTDetail
                                   group detail by new { detail.Start, detail.End } into g
                                   select new EnergyDetailInfo01 {
                                       Start = g.Key.Start,
                                       End = g.Key.End,
                                       Value = g.Sum(v => v.Value)
                                   }).ToList();

                var NHPTDetail = result.SelectMany(r => r.NHPTDetail);
                def.NHPTDetail = (from detail in NHPTDetail
                                  group detail by new { detail.Start, detail.End } into g
                                  select new EnergyDetailInfo01 {
                                      Start = g.Key.Start,
                                      End = g.Key.End,
                                      Value = g.Sum(v => v.Value)
                                  }).ToList();

                result.Add(def);
            }
            #endregion

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return result;
        }

        private string GetAuxset(string[] auxsets, string auxset) {
            if(auxset == null) 
                return string.Empty;

            if(auxsets == null)
                return string.Empty;

            foreach(var aux in auxsets) {
                if(auxset.ToUpper().Contains(aux))
                    return aux;
            }

            return string.Empty;
        }

        private EnergyInfo01 GetEmptyEnergy(string key, string current, EnmPeriod period, DateTime start, DateTime end) {
            var dates = new List<DateTime>();
            var tpdate = start;
            while(tpdate <= end) {
                dates.Add(tpdate);
                tpdate = tpdate.AddDays(1);
            }

            var intervals = new List<IDValuePair<DateTime, DateTime>>();
            if(period == EnmPeriod.Month) {
                dates = dates.GroupBy(d => new { d.Year, d.Month }).Select(g => new DateTime(g.Key.Year, g.Key.Month, 1)).ToList();
                foreach(var date in dates) {
                    intervals.Add(new IDValuePair<DateTime, DateTime> {
                        ID = date,
                        Value = date.AddMonths(1).AddDays(-1)
                    });
                }
            } else if(period == EnmPeriod.Week) {
                dates = dates.GroupBy(d => d.Date.AddDays(-1 * (((int)d.DayOfWeek + 6) % 7))).Select(g => g.Key).ToList();
                foreach(var date in dates) {
                    intervals.Add(new IDValuePair<DateTime, DateTime> {
                        ID = date,
                        Value = date.AddDays(6)
                    });
                }
            } else {
                foreach(var date in dates) {
                    intervals.Add(new IDValuePair<DateTime, DateTime> {
                        ID = date,
                        Value = date
                    });
                }
            }

            var result = new EnergyInfo01 {
                Key = key,
                Current = current,
                NHPAT = 0,
                NHPATDetail = new List<EnergyDetailInfo01>(),
                NHPBT = 0,
                NHPBTDetail = new List<EnergyDetailInfo01>(),
                NHPCT = 0,
                NHPCTDetail = new List<EnergyDetailInfo01>(),
                NHPDT = 0,
                NHPDTDetail = new List<EnergyDetailInfo01>(),
                NHPET = 0,
                NHPETDetail = new List<EnergyDetailInfo01>(),
                NHPFT = 0,
                NHPFTDetail = new List<EnergyDetailInfo01>(),
                NHPT = 0,
                NHPTDetail = new List<EnergyDetailInfo01>()
            };

            foreach(var interval in intervals) {
                result.NHPATDetail.Add(new EnergyDetailInfo01 { Start = interval.ID, End = interval.Value, Value = 0 });
                result.NHPBTDetail.Add(new EnergyDetailInfo01 { Start = interval.ID, End = interval.Value, Value = 0 });
                result.NHPCTDetail.Add(new EnergyDetailInfo01 { Start = interval.ID, End = interval.Value, Value = 0 });
                result.NHPDTDetail.Add(new EnergyDetailInfo01 { Start = interval.ID, End = interval.Value, Value = 0 });
                result.NHPETDetail.Add(new EnergyDetailInfo01 { Start = interval.ID, End = interval.Value, Value = 0 });
                result.NHPFTDetail.Add(new EnergyDetailInfo01 { Start = interval.ID, End = interval.Value, Value = 0 });
                result.NHPTDetail.Add(new EnergyDetailInfo01 { Start = interval.ID, End = interval.Value, Value = 0 });
            }

            return result;
        }
    }
}