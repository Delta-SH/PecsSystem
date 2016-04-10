using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Caching;
using System.Xml;
using Ext.Net;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "report013")]
    public partial class report013 : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack && !X.IsAjaxRequest) {
                BeginDate.Text = WebUtility.GetDateString(new DateTime(DateTime.Now.Year, DateTime.Today.Month, 1));
                EndDate.Text = WebUtility.GetDateString(DateTime.Now);
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
        /// Station Type ComboBox Refresh
        /// </summary>
        protected void OnStaTypeRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                var comboboxEntity = new BComboBox();
                var dict = comboboxEntity.GetStaTypes();
                if(dict != null && dict.Count > 0) {
                    foreach(var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                StaTypeStore.DataSource = data;
                StaTypeStore.DataBind();
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

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-013");
                var result = HttpRuntime.Cache[cacheKey] as List<Report013Entity>;
                if(result == null) { result = AddDataToCache(); }
                if(result != null && result.Count > 0) {
                    if(end > result.Count) { end = result.Count; }
                    for(int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscID = result[i].LscID,
                            LscName = result[i].LscName,
                            CorrectCount = result[i].CorrectCount,
                            AllCount = result[i].AllCount,
                            Rate = String.Format("{0:P2}", result[i].AllCount > 0 ? (double)result[i].CorrectCount / (double)result[i].AllCount : 1)
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "kpi-report-013");
                var result = HttpRuntime.Cache[cacheKey] as List<Report013Entity>;
                if(result == null) { result = AddDataToCache(); }
                if(result == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for(int i = 0; i < result.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscID_Node = datas.CreateElement("LscID");
                    LscID_Node.InnerText = result[i].LscID.ToString();
                    parent_Node.AppendChild(LscID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = result[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("CorrectCount");
                    Area1Name_Node.InnerText = result[i].CorrectCount.ToString();
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("AllCount");
                    Area2Name_Node.InnerText = result[i].AllCount.ToString();
                    parent_Node.AppendChild(Area2Name_Node);
                }

                var fileName = "KPI-Report013.xls";
                var sheetName = "蓄电池后备时长合格率";
                var title = "动力环境监控中心系统 蓄电池后备时长合格率报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportNodesToExcel(fileName, sheetName, title, subTitle, names, datas, false);
                if(xls != null) { xls.Send(); }
            } catch(Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add data to cache.
        /// </summary>
        private List<Report013Entity> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-013");
            HttpRuntime.Cache.Remove(cacheKey);

            var lscs = new List<LscUserInfo>();
            if(LscsComboBox.SelectedIndex > 0) {
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if(ids.Length == 2) {
                    var lscId = Int32.Parse(ids[0]);
                    var groupId = Int32.Parse(ids[1]);
                    var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                    if(lscUser != null) {
                        lscs.Add(lscUser);
                    }
                }
            } else {
                lscs.AddRange(userData.LscUsers);
            }
            if(lscs.Count == 0) { return null; }

            var staTypes = new Dictionary<Int32, String>();
            foreach(var dt in StaTypeMultiCombo.SelectedItems) {
                staTypes[Int32.Parse(dt.Value)] = dt.Text;
            }
            if(staTypes.Count == 0) { return null; }

            var fromTime = DateTime.Parse(BeginDate.Text);
            var toTime = DateTime.Parse(EndDate.Text);

            var texts = new List<String>();
            var auxSets = new List<String>();
            var filterText = WebUtility.StringSplit(NodeText.Text.Trim());
            foreach(var ft in filterText) {
                if(String.IsNullOrEmpty(ft.Trim())) { continue; }
                if(FilterTypeList.SelectedItem.Value.Equals("1"))
                    auxSets.Add(ft.Trim());
                else
                    texts.Add(ft.Trim());
            }

            var nodeEntity = new BNode();
            var otherEntity = new BOther();
            var source1 = new List<Report013Entity>();
            foreach(var lsc in lscs) {
                var bats = otherEntity.GetBatStatic(lsc.LscID, lsc.LscName, null, null, fromTime, null, null, toTime, 0, Int32.MaxValue);
                var fdev = from bat in bats
                           group bat by bat.DevID into g
                           select new {
                               DevID = g.Key,
                               Interval = g.Max(t => t.LastTime) * 24 * 60
                           };

                var rdev = from fv in fdev
                           where fv.Interval >= 15
                           select fv.DevID;

                if(rdev.Any()) {
                    var nodes = nodeEntity.GetNodes(lsc.LscID, EnmNodeType.Aic, texts.ToArray(), auxSets.ToArray(), null);
                    var values = nodeEntity.GetMaxHisAIV(lsc.LscID, fromTime, toTime);
                    var ndWavl = from node in nodes
                                 join val in values on node.NodeID equals val.NodeID into tp
                                 from nv in tp.DefaultIfEmpty()
                                 select new {
                                     Node = node,
                                     Value = nv != null ? nv.Value : 0
                                 };

                    var ndWre = from na in ndWavl
                                join id in rdev on na.Node.DevID equals id
                                select na;

                    var staWall = from nr in ndWre
                                  group nr by nr.Node.StaID into g
                                  select new {
                                      StaID = g.Key,
                                      Value = g.Min(v => v.Value)
                                  };

                    var staWhg = from sw in staWall
                                 where sw.Value >= 47
                                 select sw;

                    source1.Add(new Report013Entity {
                        LscID = lsc.LscID,
                        LscName = lsc.LscName,
                        CorrectCount = staWhg.Count(),
                        AllCount = staWall.Count()
                    });
                } else {
                    source1.Add(new Report013Entity {
                        LscID = lsc.LscID,
                        LscName = lsc.LscName,
                        CorrectCount = 0,
                        AllCount = 0
                    });
                }
            }

            if(source1.Count > 0) {
                source1.Add(new Report013Entity {
                    LscID = -100,
                    LscName = "汇总(平均值)",
                    CorrectCount = (Int32)source1.Average(r => r.CorrectCount),
                    AllCount = (Int32)source1.Average(r => r.AllCount)
                });
            }

            int cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, source1, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return source1;
        }
    }

    #region Page Entity Information Class
    [Serializable]
    public class Report013Entity {
        public Int32 LscID { get; set; }
        public String LscName { get; set; }
        public Int32 CorrectCount { get; set; }
        public Int32 AllCount { get; set; }
    }
    #endregion
}