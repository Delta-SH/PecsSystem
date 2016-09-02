using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "energy005")]
    public partial class energy005 : PageBase {
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
                    } else if(gti.NodeType == EnmNodeType.Sta){
                        var node = new Ext.Net.TreeNode();
                        node.Text = gti.NodeName;
                        node.NodeID = String.Format("{0}&{1}&{2}&{3}", group.LscID, group.GroupID, gti.NodeID, (Int32)gti.NodeType);
                        node.Icon = Icon.Building;
                        node.Checked = ThreeStateBool.False;
                        pnode.Nodes.Add(node);
                        RangeNodesLoaded(node, gti.NodeID, group);
                    }
                }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        protected void QueryButtonClick(object sender, DirectEventArgs e) {
            try {
                var columns = new List<string>();
                var grid = MainGridPanel;
                var store = MainStore;
                store.RemoveFields();
                grid.ColumnModel.Columns.Clear();

                var details = WebUtility.StringSplit(RangeField.Text.Trim());
                if(details.Length >= 2) {
                    columns.Add("时段");
                    columns.Add(string.Format("{0}(A)", details[0]));
                    columns.Add(string.Format("{0}(B)", details[1]));
                    columns.Add(string.Format("{0}电量", details[0]));
                    columns.Add(string.Format("{0}电量", details[1]));
                    columns.Add("对比值(A-B)");
                    columns.Add("对比幅度(A-B)/B");

                    for(var i = 0; i < columns.Count; i++) {
                        var dataIndex = String.Format("Data{0}", i);
                        store.AddField(new RecordField(dataIndex, RecordFieldType.String), false);

                        var column = new Column();
                        column.Header = columns[i];
                        column.DataIndex = dataIndex;
                        column.Align = Alignment.Center;
                        column.CustomConfig.Add(new ConfigItem("DblClickEnabled", i == 3 || i == 4 ? "1" : "0", ParameterMode.Value));
                        grid.ColumnModel.Columns.Add(column);
                    }
                }

                store.ClearMeta();
                grid.Reconfigure();
                AddDataToCache();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        protected void SaveButtonClick(object sender, DirectEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "energy-005-report");
                var data = HttpRuntime.Cache[cacheKey] as List<EnergyInfo05>;
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

                    var Data0_Node = datas.CreateElement("Data0");
                    Data0_Node.InnerText = data[i].Period;
                    parent_Node.AppendChild(Data0_Node);

                    var Data1_Node = datas.CreateElement("Data1");
                    Data1_Node.InnerText = data[i].A;
                    parent_Node.AppendChild(Data1_Node);

                    var Data2_Node = datas.CreateElement("Data2");
                    Data2_Node.InnerText = data[i].B;
                    parent_Node.AppendChild(Data2_Node);

                    var Data3_Node = datas.CreateElement("Data3");
                    Data3_Node.InnerText = data[i].AValue.ToString();
                    parent_Node.AppendChild(Data3_Node);

                    var Data4_Node = datas.CreateElement("Data4");
                    Data4_Node.InnerText = data[i].BValue.ToString();
                    parent_Node.AppendChild(Data4_Node);

                    var Data5_Node = datas.CreateElement("Data5");
                    Data5_Node.InnerText = (data[i].AValue - data[i].BValue).ToString();
                    parent_Node.AppendChild(Data5_Node);

                    var Data6_Node = datas.CreateElement("Data6");
                    Data6_Node.InnerText = String.Format("{0:P2}", data[i].BValue != 0 ? (data[i].AValue - data[i].BValue) / data[i].BValue : 1);
                    parent_Node.AppendChild(Data6_Node);
                }

                var fileName = "energy005.xls";
                var sheetName = "energy005";
                var title = "动力环境监控中心系统 机房电量对比列表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if(xls != null) { xls.Send(); }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        protected void MainStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var length = MainGridPanel.ColumnModel.Columns.Count;
                var data = CreateCustomizeTable(0);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "energy-005-report");
                var result = HttpRuntime.Cache[cacheKey] as List<EnergyInfo05>;
                if(result == null) { result = AddDataToCache(); }
                if(result != null && result.Count > 0) {
                    if(end > result.Count) { end = result.Count; }
                    data = CreateCustomizeTable(7);
                    for(int i = start; i < end; i++) {
                        var current = result[i];
                        var row = data.NewRow();
                        row[0] = current.Period;
                        row[1] = current.A;
                        row[2] = current.B;
                        row[3] = current.AValue;
                        row[4] = current.BValue;
                        row[5] = current.AValue - current.BValue;
                        row[6] = String.Format("{0:P2}", result[i].BValue != 0 ? (result[i].AValue - result[i].BValue) / result[i].BValue : 1);
                        data.Rows.Add(row);
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "energy-005-report");
                var result = HttpRuntime.Cache[cacheKey] as List<EnergyInfo05>;
                if(result == null) { result = AddDataToCache(); }
                if(result != null && result.Count > 0) {
                    var bars = new List<string>();
                    var series = new List<string>();
                    series.Add(string.Format("{{\"name\":\"{0}\",type: \"bar\",\"data\":[{1}]}}", result[0].A, string.Join(",", result.Select(v => v.AValue.ToString()).ToArray())));
                    series.Add(string.Format("{{\"name\":\"{0}\",type: \"bar\",\"data\":[{1}]}}", result[0].B, string.Join(",", result.Select(v => v.BValue.ToString()).ToArray())));

                    var xaxis = result.Select(t => t.Period);
                    var cpaxis = new List<string>();
                    foreach(var x in xaxis) cpaxis.Add(string.Format("\"{0}\"", x));
                    return string.Format("{{xAxis:[{0}],Series:[{1}]}}", string.Join(",", cpaxis.ToArray()), string.Join(",", series.ToArray()));
                }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
                Ext.Net.ResourceManager.AjaxSuccess = false;
                Ext.Net.ResourceManager.AjaxErrorMessage = err.Message;
            }

            return string.Empty;
        }

        private List<EnergyInfo05> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "energy-005-report");
            HttpRuntime.Cache.Remove(cacheKey);

            if(String.IsNullOrEmpty(RangeField.RawValue.ToString())) { return new List<EnergyInfo05>(); }
            var period = (EnmPeriod)int.Parse(PeriodField.SelectedItem.Value);
            var startDate = DateTime.Parse(StartDate.RawText);
            var endDate = DateTime.Parse(EndDate.RawText).AddSeconds(86399);
            var ranges = RangeField.RawValue.ToString();
            var auxSet = new string[] { "NHPT" };

            var ids = WebUtility.StringSplit(ranges);
            if(ids.Length < 2) return new List<EnergyInfo05>();

            var result = GetEntities(period, startDate, endDate);
            if(!string.IsNullOrEmpty(ids[0])) {
                var keys = WebUtility.ItemSplit(ids[0]);
                if(keys.Length != 4) return new List<EnergyInfo05>();
                var lscId = int.Parse(keys[0]);
                var groupId = int.Parse(keys[1]);
                var nodeId = int.Parse(keys[2]);
                var nodeType = int.Parse(keys[3]);
                var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                if(enmNodeType != EnmNodeType.Sta) return new List<EnergyInfo05>();
                var lscUser = userData.LscUsers.Find(l => l.LscID == lscId);
                if(lscUser == null) return new List<EnergyInfo05>();
                var station = lscUser.Group.GroupNodes.Find(g => g.NodeType == EnmNodeType.Sta && g.NodeID == nodeId);
                if(station == null) return new List<EnergyInfo05>();

                var nodeEntity = new BNode();
                var values = new List<ElecValueInfo>();
                var nodes = nodeEntity.GetNodes(lscId, EnmNodeType.Aic, null, auxSet, null).FindAll(n => n.StaID == nodeId);
                if(nodes.Count > 0)
                    values = nodeEntity.GetElecValues(lscId, startDate, endDate, period);

                var valWnodes = from node in nodes
                                join value in values on node.NodeID equals value.NodeId
                                select new {
                                    Start = value.Start,
                                    End = value.End,
                                    Value = value.Value
                                };

                var valWgrp = (from vn in valWnodes
                              group vn by new { vn.Start, vn.End } into g
                              select new {
                                  Start = g.Key.Start,
                                  End = g.Key.End,
                                  Value = g.Sum(v => v.Value)
                              }).ToList();

                for(var i = 0; i < result.Count; i++) {
                    result[i].A = station.NodeName;
                    var current = valWgrp.Find(v => v.Start == result[i].Start && v.End == result[i].End);
                    if(current != null){
                        result[i].AValue = current.Value;
                    }
                }
            }

            if(!string.IsNullOrEmpty(ids[1])) {
                var keys = WebUtility.ItemSplit(ids[1]);
                if(keys.Length != 4) return new List<EnergyInfo05>();
                var lscId = int.Parse(keys[0]);
                var groupId = int.Parse(keys[1]);
                var nodeId = int.Parse(keys[2]);
                var nodeType = int.Parse(keys[3]);
                var enmNodeType = Enum.IsDefined(typeof(EnmNodeType), nodeType) ? (EnmNodeType)nodeType : EnmNodeType.Null;
                if(enmNodeType != EnmNodeType.Sta) return new List<EnergyInfo05>();
                var lscUser = userData.LscUsers.Find(l => l.LscID == lscId);
                if(lscUser == null) return new List<EnergyInfo05>();
                var station = lscUser.Group.GroupNodes.Find(g => g.NodeType == EnmNodeType.Sta && g.NodeID == nodeId);
                if(station == null) return new List<EnergyInfo05>();

                var nodeEntity = new BNode();
                var values = new List<ElecValueInfo>();
                var nodes = nodeEntity.GetNodes(lscId, EnmNodeType.Aic, null, auxSet, null).FindAll(n => n.StaID == nodeId);
                if(nodes.Count > 0)
                    values = nodeEntity.GetElecValues(lscId, startDate, endDate, period);

                var valWnodes = from node in nodes
                                join value in values on node.NodeID equals value.NodeId
                                select new {
                                    Start = value.Start,
                                    End = value.End,
                                    Value = value.Value
                                };

                var valWgrp = (from vn in valWnodes
                               group vn by new { vn.Start, vn.End } into g
                               select new {
                                   Start = g.Key.Start,
                                   End = g.Key.End,
                                   Value = g.Sum(v => v.Value)
                               }).ToList();

                for(var i = 0; i < result.Count; i++) {
                    result[i].B = station.NodeName;
                    var current = valWgrp.Find(v => v.Start == result[i].Start && v.End == result[i].End);
                    if(current != null) {
                        result[i].BValue = current.Value;
                    }
                }
            }

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return result;
        }

        private DataTable CreateCustomizeTable(int length) {
            var dt = new DataTable("dtable");
            for(int i = 0; i < length; i++) {
                var column = new DataColumn();
                column.DataType = typeof(String);
                column.ColumnName = String.Format("Data{0}", i);
                column.DefaultValue = String.Empty;
                dt.Columns.Add(column);
            }
            return dt;
        }

        private List<EnergyInfo05> GetEntities(EnmPeriod period, DateTime start, DateTime end) {
            var dates = new List<DateTime>();
            var tpdate = start;
            while(tpdate <= end) {
                dates.Add(tpdate);
                tpdate = tpdate.AddDays(1);
            }

            var details = new List<EnergyInfo05>();
            if(period == EnmPeriod.Month) {
                dates = dates.GroupBy(d => new { d.Year, d.Month }).Select(g => new DateTime(g.Key.Year, g.Key.Month, 1)).ToList();
                foreach(var date in dates) {
                    details.Add(new EnergyInfo05 {
                        Period = date.ToString("yyyy-MM"),
                        Start = date,
                        End = date.AddMonths(1).AddDays(-1),
                        A = "",
                        B = "",
                        AValue = 0,
                        BValue = 0
                    });
                }
            } else if(period == EnmPeriod.Week) {
                dates = dates.GroupBy(d => d.Date.AddDays(-1 * (((int)d.DayOfWeek + 6) % 7))).Select(g => g.Key).ToList();
                var gc = new GregorianCalendar();
                foreach(var date in dates) {
                    details.Add(new EnergyInfo05 {
                        Period = string.Format("第{0}周", gc.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday)),
                        Start = date,
                        End = date.AddDays(6),
                        A = "",
                        B = "",
                        AValue = 0,
                        BValue = 0
                    });
                }
            } else {
                foreach(var date in dates) {
                    details.Add(new EnergyInfo05 {
                        Period = WebUtility.GetDateString2(date),
                        Start = date,
                        End = date,
                        A = "",
                        B = "",
                        AValue = 0,
                        BValue = 0
                    });
                }
            }

            return details;
        }
    }
}