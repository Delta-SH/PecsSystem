﻿using Delta.PECS.WebCSC.BLL;
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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "energy004")]
    public partial class energy004 : PageBase {
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

        protected void QueryButtonClick(object sender, DirectEventArgs e) {
            try {
                AddDataToCache();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        protected void SaveButtonClick(object sender, DirectEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "energy-004-report");
                var data = HttpRuntime.Cache[cacheKey] as List<EnergyInfo04>;
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

                    var Period_Node = datas.CreateElement("Period");
                    Period_Node.InnerText = data[i].Period;
                    parent_Node.AppendChild(Period_Node);

                    var Value_Node = datas.CreateElement("Value");
                    Value_Node.InnerText = data[i].Value.ToString();
                    parent_Node.AppendChild(Value_Node);

                    var TValue_Node = datas.CreateElement("HValue");
                    TValue_Node.InnerText = data[i].HValue.ToString();
                    parent_Node.AppendChild(TValue_Node);

                    var CValue_Node = datas.CreateElement("CValue");
                    CValue_Node.InnerText = (data[i].Value - data[i].HValue).ToString();
                    parent_Node.AppendChild(CValue_Node);

                    var Rate_Node = datas.CreateElement("Rate");
                    Rate_Node.InnerText = String.Format("{0:P2}", data[i].HValue != 0 ? (data[i].Value - data[i].HValue) / data[i].HValue : 1);
                    parent_Node.AppendChild(Rate_Node);
                }

                var fileName = "energy004.xls";
                var sheetName = "energy004";
                var title = "动力环境监控中心系统 电量同址环比";
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
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "energy-004-report");
                var result = HttpRuntime.Cache[cacheKey] as List<EnergyInfo04>;
                if(result == null) { result = AddDataToCache(); }
                if(result != null && result.Count > 0) {
                    if(end > result.Count) { end = result.Count; }
                    for(int i = start; i < end; i++) {
                        data.Add(new {
                            Key = result[i].Key,
                            Current = result[i].Current,
                            Period = result[i].Period,
                            Value = result[i].Value,
                            HValue = result[i].HValue,
                            CValue = result[i].Value - result[i].HValue,
                            Rate = String.Format("{0:P2}", result[i].HValue != 0 ? (result[i].Value - result[i].HValue) / result[i].HValue : 1)
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "energy-004-report");
                var result = HttpRuntime.Cache[cacheKey] as List<EnergyInfo04>;
                if(result == null) { result = AddDataToCache(); }
                if(result != null && result.Count > 0) {
                    var bars = new List<string>();
                    for(int i = 0; i < result.Count; i++) {
                        if(result[i].Key == "TOTAL") continue;
                        bars.Add(string.Format("{{\"name\":\"{0}\",\"value\":{1},\"hvalue\":{2}}}", result[i].Current, result[i].Value, result[i].HValue));
                    }

                    return string.Format("{{Data:[{0}]}}", string.Join(",", bars.ToArray()));
                }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
                Ext.Net.ResourceManager.AjaxSuccess = false;
                Ext.Net.ResourceManager.AjaxErrorMessage = err.Message;
            }

            return string.Empty;
        }

        private List<EnergyInfo04> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "energy-004-report");
            HttpRuntime.Cache.Remove(cacheKey);

            if(String.IsNullOrEmpty(RangeField.RawValue.ToString())) { return new List<EnergyInfo04>(); }
            var period = (EnmPeriod)int.Parse(PeriodField.SelectedItem.Value);
            var startDate = DateTime.Parse(StartDate.RawText);
            var endDate = DateTime.Parse(EndDate.RawText).AddSeconds(86399);
            var tstartDate = startDate.AddMonths(-1);
            var tendDate = endDate.AddMonths(-1);
            var ranges = RangeField.RawValue.ToString();
            var auxSet = new string[] { "NHPAT", "NHPBT", "NHPCT", "NHPDT", "NHPET", "NHPFT" };

            var result = new List<EnergyInfo04>();
            if(ranges == "root") {
                #region root
                var nodeEntity = new BNode();
                foreach(var lsc in userData.LscUsers) {
                    var def = this.GetEntities(lsc.LscID.ToString(), lsc.LscName, period, startDate, endDate);
                    var nodes = nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Aic, null, auxSet, null);
                    if(nodes.Count > 0) {
                        var values = nodeEntity.GetElecValues(lsc.LscID, startDate, endDate, period);
                        var valWnodes = from node in nodes
                                        join value in values on node.NodeID equals value.NodeId
                                        select new { Start = value.Start, End = value.End, Value = value.Value };

                        def.Value = valWnodes.Sum(v => v.Value);

                        var hvalues = nodeEntity.GetElecValues(lsc.LscID, tstartDate, tendDate, period);
                        var hvalWnodes = from node in nodes
                                         join value in hvalues on node.NodeID equals value.NodeId
                                         select new { Start = value.Start, End = value.End, Value = value.Value };

                        def.HValue = hvalWnodes.Sum(v => v.Value);
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
                        var hvalues = new List<ElecValueInfo>();
                        var nodes = nodeEntity.GetNodes(lscUser.LscID, EnmNodeType.Aic, null, auxSet, null);
                        if(nodes.Count > 0) {
                            values = nodeEntity.GetElecValues(lscUser.LscID, startDate, endDate, period);
                            hvalues = nodeEntity.GetElecValues(lscUser.LscID, tstartDate, tendDate, period);
                        }

                        var valWnodes = from node in nodes
                                        join value in values on node.NodeID equals value.NodeId
                                        select new {
                                            Area = node.Area2ID,
                                            Start = value.Start,
                                            End = value.End,
                                            Value = value.Value
                                        };

                        var valWgrp = from vn in valWnodes
                                      group vn by new { vn.Area } into g
                                      select new {
                                          Area = g.Key.Area,
                                          Value = g.Sum(v => v.Value)
                                      };

                        var hvalWnodes = from node in nodes
                                         join value in hvalues on node.NodeID equals value.NodeId
                                         select new {
                                             Area = node.Area2ID,
                                             Start = value.Start,
                                             End = value.End,
                                             Value = value.Value
                                         };

                        var hvalWgrp = from vn in hvalWnodes
                                       group vn by new { vn.Area } into g
                                       select new {
                                           Area = g.Key.Area,
                                           Value = g.Sum(v => v.Value)
                                       };

                        var gnodes = lscUser.Group.GroupNodes.FindAll(gti => gti.NodeType == EnmNodeType.Area && gti.Remark.Equals("2"));
                        foreach(var gn in gnodes) {
                            var def = this.GetEntities(string.Format("{0}-{1}", gn.LscID, gn.NodeID), gn.NodeName, period, startDate, endDate);
                            if(nodes.Count > 0 && values.Count > 0) {
                                var details = valWgrp.Where(m => m.Area == gn.NodeID);
                                def.Value = details.Sum(v => v.Value);

                                var tdetails = hvalWgrp.Where(m => m.Area == gn.NodeID);
                                def.HValue = tdetails.Sum(v => v.Value);
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
                        var hvalues = new List<ElecValueInfo>();
                        var nodes = nodeEntity.GetNodes(lscUser.LscID, EnmNodeType.Aic, null, auxSet, null);
                        if(nodes.Count > 0) {
                            values = nodeEntity.GetElecValues(lscUser.LscID, startDate, endDate, period);
                            hvalues = nodeEntity.GetElecValues(lscUser.LscID, tstartDate, tendDate, period);
                        }

                        var valWnodes = from node in nodes
                                        join value in values on node.NodeID equals value.NodeId
                                        select new {
                                            Area = node.Area3ID,
                                            Start = value.Start,
                                            End = value.End,
                                            Value = value.Value
                                        };

                        var valWgrp = from vn in valWnodes
                                      group vn by new { vn.Area } into g
                                      select new {
                                          Area = g.Key.Area,
                                          Value = g.Sum(v => v.Value)
                                      };

                        var hvalWnodes = from node in nodes
                                         join value in hvalues on node.NodeID equals value.NodeId
                                         select new {
                                             Area = node.Area3ID,
                                             Start = value.Start,
                                             End = value.End,
                                             Value = value.Value
                                         };

                        var hvalWgrp = from vn in hvalWnodes
                                       group vn by new { vn.Area } into g
                                       select new {
                                           Area = g.Key.Area,
                                           Value = g.Sum(v => v.Value)
                                       };

                        var gnodes = lscUser.Group.GroupNodes.FindAll(gti => gti.NodeType == EnmNodeType.Area && gti.LastNodeID == nodeId);
                        foreach(var gn in gnodes) {
                            var def = this.GetEntities(string.Format("{0}-{1}", gn.LscID, gn.NodeID), gn.NodeName, period, startDate, endDate);
                            if(nodes.Count > 0 && values.Count > 0) {
                                var details = valWgrp.Where(m => m.Area == gn.NodeID);
                                def.Value = details.Sum(v => v.Value);

                                var hdetails = hvalWgrp.Where(m => m.Area == gn.NodeID);
                                def.HValue = hdetails.Sum(v => v.Value);
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
                        var hvalues = new List<ElecValueInfo>();
                        var nodes = nodeEntity.GetNodes(lscUser.LscID, EnmNodeType.Aic, null, auxSet, null);
                        if(nodes.Count > 0) {
                            values = nodeEntity.GetElecValues(lscUser.LscID, startDate, endDate, period);
                            hvalues = nodeEntity.GetElecValues(lscUser.LscID, tstartDate, tendDate, period);
                        }

                        var valWnodes = from node in nodes
                                        join value in values on node.NodeID equals value.NodeId
                                        select new {
                                            Station = node.StaID,
                                            Start = value.Start,
                                            End = value.End,
                                            Value = value.Value
                                        };

                        var valWgrp = from vn in valWnodes
                                      group vn by new { vn.Station } into g
                                      select new {
                                          Station = g.Key.Station,
                                          Value = g.Sum(v => v.Value)
                                      };

                        var hvalWnodes = from node in nodes
                                         join value in hvalues on node.NodeID equals value.NodeId
                                         select new {
                                             Station = node.StaID,
                                             Start = value.Start,
                                             End = value.End,
                                             Value = value.Value
                                         };

                        var hvalWgrp = from vn in hvalWnodes
                                       group vn by new { vn.Station } into g
                                       select new {
                                           Station = g.Key.Station,
                                           Value = g.Sum(v => v.Value)
                                       };

                        var gnodes = lscUser.Group.GroupNodes.FindAll(gti => gti.NodeType == EnmNodeType.Sta && gti.LastNodeID == nodeId);
                        foreach(var gn in gnodes) {
                            var def = this.GetEntities(string.Format("{0}-{1}", gn.LscID, gn.NodeID), gn.NodeName, period, startDate, endDate);
                            if(nodes.Count > 0 && values.Count > 0) {
                                var details = valWgrp.Where(m => m.Station == gn.NodeID);
                                def.Value = details.Sum(v => v.Value);

                                var hdetails = hvalWgrp.Where(m => m.Station == gn.NodeID);
                                def.HValue = hdetails.Sum(v => v.Value);
                            }
                            result.Add(def);
                        }
                    }
                    #endregion
                }
            }

            #region total
            if(result.Count > 0) {
                var def = this.GetEntities("TOTAL", "总计", period, startDate, endDate);
                def.Value = result.Sum(v => v.Value);
                def.HValue = result.Sum(v => v.HValue);
                result.Add(def);
            }
            #endregion

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return result;
        }

        private EnergyInfo04 GetEntities(string key, string current, EnmPeriod period, DateTime start, DateTime end) {
            //var dates = new List<DateTime>();
            //var tpdate = start;
            //while(tpdate <= end) {
            //    dates.Add(tpdate);
            //    tpdate = tpdate.AddDays(1);
            //}

            //var details = new List<EnergyDetailInfo03>();
            //if(period == EnmPeriod.Month) {
            //    dates = dates.GroupBy(d => new { d.Year, d.Month }).Select(g => new DateTime(g.Key.Year, g.Key.Month, 1)).ToList();
            //    foreach(var date in dates) {
            //        details.Add(new EnergyDetailInfo03 {
            //            Period = date.ToString("yyyy-MM"),
            //            Start = date,
            //            End = date.AddMonths(1).AddDays(-1),
            //            Value = 0,
            //            TValue = 0
            //        });
            //    }
            //} else if(period == EnmPeriod.Week) {
            //    dates = dates.GroupBy(d => d.Date.AddDays(-1 * (((int)d.DayOfWeek + 6) % 7))).Select(g => g.Key).ToList();
            //    var gc = new GregorianCalendar();
            //    foreach(var date in dates) {
            //        details.Add(new EnergyDetailInfo03 {
            //            Period = string.Format("第{0}周", gc.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday)),
            //            Start = date,
            //            End = date.AddDays(6),
            //            Value = 0,
            //            TValue = 0
            //        });
            //    }
            //} else {
            //    foreach(var date in dates) {
            //        details.Add(new EnergyDetailInfo03 {
            //            Period = WebUtility.GetDateString2(date),
            //            Start = date,
            //            End = date,
            //            Value = 0,
            //            TValue = 0
            //        });
            //    }
            //}

            return new EnergyInfo04 {
                Key = key,
                Current = current,
                Period = string.Format("{0} ~ {1}", WebUtility.GetDateString2(start), WebUtility.GetDateString2(end)),
                Value = 0,
                HValue = 0
            };
        }
    }
}