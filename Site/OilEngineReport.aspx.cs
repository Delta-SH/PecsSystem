using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Web.Configuration;
using System.Data;
using Ext.Net;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "OilEngineReport")]
    public partial class OilEngineReport : PageBase {
        public static readonly Int32 ZC = 0, GJ = 1;
        protected void Page_Load(object sender, EventArgs e) {
            if(!Page.IsPostBack && !X.IsAjaxRequest) {
                var dt = DateTime.Today.AddMonths(-1);
                var bt = new DateTime(dt.Year, dt.Month, 1);
                var et = bt.AddMonths(1).AddSeconds(-1);
                BeginFromDate.Text = WebUtility.GetDateString(bt);
                BeginToDate.Text = WebUtility.GetDateString(et);
            }
        }

        /// <summary>
        /// Lsc ComboBox Refresh
        /// </summary>
        protected void OnLscsRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultLscID,
                    Name = WebUtility.DefaultItemName
                });

                var lscIds = new Dictionary<int, int>();
                var lscUsers = UserData.LscUsers;
                foreach(var lscUser in lscUsers) {
                    lscIds.Add(lscUser.LscID, lscUser.Group.GroupID);
                }

                if(lscIds.Count > 0) {
                    var comboBoxEntity = new BComboBox();
                    var dict = comboBoxEntity.GetLscs(lscIds);
                    if(dict != null && dict.Count > 0) {
                        foreach(var key in dict) {
                            data.Add(new {
                                Id = key.Key,
                                Name = key.Value
                            });
                        }
                    }
                }

                LscsStore.DataSource = data;
                LscsStore.DataBind();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area2 ComboBox Refresh
        /// </summary>
        protected void OnArea2Refresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if(LscsComboBox.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if(ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var dict = comboboxEntity.GetArea2(lscId, WebUtility.DefaultInt32, groupId);
                        if(dict != null && dict.Count > 0) {
                            foreach(var key in dict) {
                                data.Add(new {
                                    Id = key.Key,
                                    Name = key.Value
                                });
                            }
                        }
                    }
                }

                Area2Store.DataSource = data;
                Area2Store.DataBind();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Area3 ComboBox Refresh
        /// </summary>
        protected void OnArea3Refresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                data.Add(new {
                    Id = WebUtility.DefaultItemID,
                    Name = WebUtility.DefaultItemName
                });

                if(LscsComboBox.SelectedIndex > 0) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if(ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area2Id = Area2ComboBox.SelectedIndex > 0 ? Int32.Parse(Area2ComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var dict = comboboxEntity.GetArea3(lscId, area2Id, groupId);
                        if(dict != null && dict.Count > 0) {
                            foreach(var key in dict) {
                                data.Add(new {
                                    Id = key.Key,
                                    Name = key.Value
                                });
                            }
                        }
                    }
                }

                Area3Store.DataSource = data;
                Area3Store.DataBind();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Device Type ComboBox Refresh
        /// </summary>
        protected void OnDevTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetDevTypes();
                if(dict != null && dict.Count > 0) {
                    foreach(var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                DevTypeStore.DataSource = data;
                DevTypeStore.DataBind();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Store Refresh
        /// </summary>
        protected void OnMainStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-oil-engine-report");
                var result = HttpRuntime.Cache[cacheKey] as List<OilEngineEntity>;
                if(result == null) { result = AddDataToCache(); }
                if(result != null && result.Count > 0) {
                    if(end > result.Count) { end = result.Count; }
                    for(int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = result[i].Station.LscID,
                            LscName = result[i].Station.LscName,
                            Area2ID = result[i].Station.Area2ID,
                            Area2Name = result[i].Station.Area2Name,
                            Area3ID = result[i].Station.Area3ID,
                            Area3Name = result[i].Station.Area3Name,
                            StaID = result[i].Station.StaID,
                            StaName = result[i].Station.StaName,
                            PowerOff = result[i].SNodes.Sum(s => s.Divs.Count(d => d.Value == GJ)),
                            OffInterval = result[i].SNodes.Sum(s => s.Interval),
                            PowerOn = result[i].YNodes.Sum(y => y.Divs.Count(d => d.Value == GJ)),
                            OnInterval = result[i].YNodes.Sum(y => y.Interval)
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

        /// <summary>
        /// Query Button Click
        /// </summary>
        protected void QueryBtn_Click(object sender, DirectEventArgs e) {
            try {
                AddDataToCache();
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save Button Click
        /// </summary>
        protected void SaveBtn_Click(object sender, DirectEventArgs e) {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-oil-engine-report");
                var result = HttpRuntime.Cache[cacheKey] as List<OilEngineEntity>;
                if(result == null) { result = AddDataToCache(); }
                if(result == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var datas = new DataTable();
                datas.Columns.Add(new DataColumn("序号", typeof(Int32)));
                datas.Columns.Add(new DataColumn("Lsc名称", typeof(String)));
                datas.Columns.Add(new DataColumn("地区名称", typeof(String)));
                datas.Columns.Add(new DataColumn("县市名称", typeof(String)));
                datas.Columns.Add(new DataColumn("局站名称", typeof(String)));
                datas.Columns.Add(new DataColumn("市电停电次数", typeof(Int32)));
                datas.Columns.Add(new DataColumn("市电停电时长(分钟)", typeof(Double)));
                datas.Columns.Add(new DataColumn("油机发电次数", typeof(Int32)));
                datas.Columns.Add(new DataColumn("油机发电时长(分钟)", typeof(Double)));

                DataRow row;
                for(int i = 0; i < result.Count; i++) {
                    row = datas.NewRow();
                    row["序号"] = i + 1;
                    row["Lsc名称"] = result[i].Station.LscName;
                    row["地区名称"] = result[i].Station.Area2Name;
                    row["县市名称"] = result[i].Station.Area3Name;
                    row["局站名称"] = result[i].Station.StaName;
                    row["市电停电次数"] = result[i].SNodes.Sum(s => s.Divs.Count(d => d.Value == GJ));
                    row["市电停电时长(分钟)"] = result[i].SNodes.Sum(s => s.Interval);
                    row["油机发电次数"] = result[i].YNodes.Sum(y => y.Divs.Count(d => d.Value == GJ));
                    row["油机发电时长(分钟)"] = result[i].YNodes.Sum(y => y.Interval);
                    datas.Rows.Add(row);
                }

                var fileName = "KPI-Report201.xls";
                var sheetName = "油机发电统计";
                var title = "动力环境监控中心系统 油机发电统计报表";
                var subtitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subtitle, datas);
                if(xls != null) { xls.Send(); }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add data to cache.
        /// </summary>
        private List<OilEngineEntity> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-oil-engine-report");
            HttpRuntime.Cache.Remove(cacheKey);

            var lscs = new List<LscUserInfo>();
            if(LscsComboBox.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if(ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if(lscUser == null) { return null; }
                lscs.Add(lscUser);
            } else {
                lscs.AddRange(userData.LscUsers);
            }

            var fromTime = DateTime.Parse(BeginFromDate.Text);
            var toTime = DateTime.Parse(BeginToDate.Text);

            var stexts = new List<String>();
            var sauxSets = new List<String>();
            var sfilterText = WebUtility.StringSplit(SNodeText.Text.Trim());
            foreach(var ft in sfilterText) {
                if(String.IsNullOrEmpty(ft.Trim())) { continue; }
                if(SFilterList.SelectedItem.Value.Equals("1"))
                    sauxSets.Add(ft.Trim());
                else
                    stexts.Add(ft.Trim());
            }

            var ytexts = new List<String>();
            var yauxSets = new List<String>();
            var yfilterText = WebUtility.StringSplit(YNodeText.Text.Trim());
            foreach(var ft in yfilterText) {
                if(String.IsNullOrEmpty(ft.Trim())) { continue; }
                if(YFilterList.SelectedItem.Value.Equals("1"))
                    yauxSets.Add(ft.Trim());
                else
                    ytexts.Add(ft.Trim());
            }

            var devTypes = new Dictionary<Int32, String>();
            foreach(var dt in DevTypeMultiCombo.SelectedItems) {
                devTypes[Int32.Parse(dt.Value)] = dt.Text;
            }
            if(devTypes.Count == 0) { return null; }

            var otherEntity = new BOther();
            var nodeEntity = new BNode();
            var result = new List<OilEngineEntity>();
            foreach(var lsc in lscs) {
                var stations = otherEntity.GetStations(lsc.LscID, lsc.Group.GroupID);

                var snodes = new List<NodeInfo>();
                var ynodes = new List<NodeInfo>();
                if(stexts.Count > 0 || sauxSets.Count > 0) {
                    snodes.AddRange(nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Dic, stexts.ToArray(), sauxSets.ToArray(), devTypes.Keys.ToArray()));
                }
                if(ytexts.Count > 0 || yauxSets.Count > 0) {
                    ynodes.AddRange(nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Dic, ytexts.ToArray(), yauxSets.ToArray(), devTypes.Keys.ToArray()));
                }

                if(Area2ComboBox.SelectedIndex > 0) {
                    var area2Id = Int32.Parse(Area2ComboBox.SelectedItem.Value);
                    stations = stations.FindAll(s => s.Area2ID == area2Id);
                    snodes = snodes.FindAll(n => n.Area2ID == area2Id);
                    ynodes = ynodes.FindAll(n => n.Area2ID == area2Id);
                }
                if(Area3ComboBox.SelectedIndex > 0) {
                    var area3Id = Int32.Parse(Area3ComboBox.SelectedItem.Value);
                    stations = stations.FindAll(s => s.Area3ID == area3Id);
                    snodes = snodes.FindAll(n => n.Area3ID == area3Id);
                    ynodes = ynodes.FindAll(n => n.Area3ID == area3Id);
                }

                var spairs = new List<OilEnginePair>();
                var sdiv = nodeEntity.GetHisDIV(lsc.LscID, snodes.Select(n => n.NodeID).ToArray(), fromTime, toTime);
                foreach(var node in snodes) {
                    var curNodes = sdiv.FindAll(d => d.NodeID == node.NodeID).OrderBy(d => d.UpdateTime).ToList();

                    var interval = 0d;
                    var startTime = DateTime.MinValue;
                    foreach(var cn in curNodes) {
                        if(cn.Value == GJ && startTime == DateTime.MinValue) {
                            startTime = cn.UpdateTime;
                        } else if(cn.Value == ZC && startTime != DateTime.MinValue) {
                            interval += cn.UpdateTime.Subtract(startTime).TotalMinutes;
                            startTime = DateTime.MinValue;
                        }
                    }

                    if(startTime != DateTime.MinValue) {
                        interval += toTime.Subtract(startTime).TotalMinutes;
                        startTime = DateTime.MinValue;
                    }

                    spairs.Add(new OilEnginePair {
                        Node = node,
                        Divs = curNodes,
                        Interval = interval
                    });
                }

                var ypairs = new List<OilEnginePair>();
                var ydiv = nodeEntity.GetHisDIV(lsc.LscID, ynodes.Select(n => n.NodeID).ToArray(), fromTime, toTime);
                foreach(var node in ynodes) {
                    var curNodes = ydiv.FindAll(d => d.NodeID == node.NodeID).OrderBy(d => d.UpdateTime).ToList();

                    var interval = 0d;
                    var startTime = DateTime.MinValue;
                    foreach(var cn in curNodes) {
                        if(cn.Value == GJ && startTime == DateTime.MinValue) {
                            startTime = cn.UpdateTime;
                        } else if(cn.Value == ZC && startTime != DateTime.MinValue) {
                            interval += cn.UpdateTime.Subtract(startTime).TotalMinutes;
                            startTime = DateTime.MinValue;
                        }
                    }

                    if(startTime != DateTime.MinValue) {
                        interval += toTime.Subtract(startTime).TotalMinutes;
                        startTime = DateTime.MinValue;
                    }

                    ypairs.Add(new OilEnginePair {
                        Node = node,
                        Divs = curNodes,
                        Interval = interval
                    });
                }

                var sresult = from s in spairs
                              group s by new { s.Node.LscID, s.Node.StaID } into g
                              select new {
                                  LscID = g.Key.LscID,
                                  StaID = g.Key.StaID,
                                  Nodes = g
                              };

                var yresult = from y in ypairs
                              group y by new { y.Node.LscID, y.Node.StaID } into g
                              select new {
                                  LscID = g.Key.LscID,
                                  StaID = g.Key.StaID,
                                  Nodes = g
                              };

                result.AddRange(from sta in stations
                                join sr in sresult on new { sta.LscID, sta.StaID } equals new { sr.LscID, sr.StaID } into lt1
                                from ss in lt1.DefaultIfEmpty()
                                join yr in yresult on new { sta.LscID, sta.StaID } equals new { yr.LscID, yr.StaID } into lt2
                                from ys in lt2.DefaultIfEmpty()
                                select new OilEngineEntity {
                                    Station = sta,
                                    SNodes = ss != null ? ss.Nodes.ToList() : new List<OilEnginePair>(),
                                    YNodes = ys != null ? ys.Nodes.ToList() : new List<OilEnginePair>()
                                });
            }

            int cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return result;
        }

        /// <summary>
        /// Show GridCell Detail
        /// </summary>
        /// <param name="title">title</param>
        /// <param name="lscId">lscId</param>
        /// <param name="staId">staId</param>
        [DirectMethod(Timeout = 300000)]
        public void ShowGridCellDetail(string title, string lscId, string staId, string typeId) {
            var userData = UserData;
            if(HttpRuntime.Cache[WebUtility.GetCacheKeyName(userData, "kpi-oil-engine-report")] == null) {
                AddDataToCache();
            }

            var win = WebUtility.GetNewWindow(800, 600, title, Icon.Printer);
            win.AutoLoad.Url = "~/DivWnd.aspx";
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Type", "kpi_oil_engine_report"));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("Title", title));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("LscID", lscId));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("StaID", staId));
            win.AutoLoad.Params.Add(new Ext.Net.Parameter("TypeID", typeId));
            win.Render();
            win.Show();
        }
    }

    #region Page Entity Information Class
    [Serializable]
    public class OilEngineEntity {
        public StationInfo Station { get; set; }
        public List<OilEnginePair> SNodes { get; set; }
        public List<OilEnginePair> YNodes { get; set; }
    }

    [Serializable]
    public class OilEnginePair {
        public NodeInfo Node { get; set; }
        public List<HisDIVInfo> Divs { get; set; }
        public Double Interval { get; set; }
    }
    #endregion
}