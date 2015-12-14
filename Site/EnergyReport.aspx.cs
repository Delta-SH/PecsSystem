using System;
using System.Collections.Generic;
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
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "EnergyReport")]
    public partial class EnergyReport : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                var datetime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                FromDate.Text = WebUtility.GetDateString(datetime.AddMonths(-1));
                ToDate.Text = WebUtility.GetDateString(datetime.AddSeconds(-1));
                CountItemField.Text = WebConfigurationManager.AppSettings["DefaultCountItem"];
            }
        }

        /// <summary>
        /// Lsc ComboBox Refresh
        /// </summary>
        protected void OnLscsRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var data = new List<object>();
                var lscIds = new Dictionary<int, int>();
                var lscUsers = UserData.LscUsers;
                foreach (var lscUser in lscUsers) {
                    lscIds.Add(lscUser.LscID, lscUser.Group.GroupID);
                }

                if (lscIds.Count > 0) {
                    var comboBoxEntity = new BComboBox();
                    var dict = comboBoxEntity.GetLscs(lscIds);
                    if (dict != null && dict.Count > 0) {
                        foreach (var key in dict) {
                            data.Add(new {
                                Id = key.Key,
                                Name = key.Value
                            });
                        }
                    }
                }

                LscsStore.DataSource = data;
                LscsStore.DataBind();
            } catch (Exception err) {
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

                if (LscsComboBox.SelectedItem != null) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var dict = comboboxEntity.GetArea2(lscId, WebUtility.DefaultInt32, groupId);
                        if (dict != null && dict.Count > 0) {
                            foreach (var key in dict) {
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
            } catch (Exception err) {
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

                if (LscsComboBox.SelectedItem != null) {
                    var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                    if (ids.Length == 2) {
                        var lscId = Int32.Parse(ids[0]);
                        var groupId = Int32.Parse(ids[1]);
                        var comboboxEntity = new BComboBox();
                        var area2Id = Area2ComboBox.SelectedIndex > 0 ? Int32.Parse(Area2ComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var dict = comboboxEntity.GetArea3(lscId, area2Id, groupId);
                        if (dict != null && dict.Count > 0) {
                            foreach (var key in dict) {
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
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Grid Store Refresh
        /// </summary>
        protected void OnGridStoreRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "energy-records");
                var records = HttpRuntime.Cache[cacheKey] as List<EnergyInfo>;
                if (records == null) { records = AddDataToCache(); }
                if (records != null && records.Count > 0) {
                    if (end > records.Count) { end = records.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscName = records[i].LscName,
                            Area1Name = records[i].Area1Name,
                            Area2Name = records[i].Area2Name,
                            Area3Name = records[i].Area3Name,
                            StaName = records[i].StaName,
                            StaTypeName = records[i].StaTypeName,
                            SumValue = records[i].SumValue,
                            YoYValue = records[i].YoYValue,
                            YoY = String.Format("{0:P2}", records[i].YoYValue > 0 ? (records[i].SumValue - records[i].YoYValue) / records[i].YoYValue : 1),
                            QoQValue = records[i].QoQValue,
                            QoQ = String.Format("{0:P2}", records[i].QoQValue > 0 ? (records[i].SumValue - records[i].QoQValue) / records[i].QoQValue : 1)
                        });
                    }
                }

                e.Total = (records != null ? records.Count : 0);
                GridStore.DataSource = data;
                GridStore.DataBind();
            } catch (Exception err) {
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
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Save Button Click
        /// </summary>
        protected void SaveBtn_Click(object sender, DirectEventArgs e) {
            try {
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "energy-records");
                var records = HttpRuntime.Cache[cacheKey] as List<EnergyInfo>;
                if (records == null) { records = AddDataToCache(); }
                if (records == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < records.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = records[i].LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = records[i].Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = records[i].Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = records[i].Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var StaName_Node = datas.CreateElement("StaName");
                    StaName_Node.InnerText = records[i].StaName;
                    parent_Node.AppendChild(StaName_Node);

                    var StaTypeName_Node = datas.CreateElement("StaTypeName");
                    StaTypeName_Node.InnerText = records[i].StaTypeName;
                    parent_Node.AppendChild(StaTypeName_Node);

                    var SumValue_Node = datas.CreateElement("SumValue");
                    SumValue_Node.InnerText = records[i].SumValue.ToString();
                    parent_Node.AppendChild(SumValue_Node);

                    var YoYValue_Node = datas.CreateElement("YoYValue");
                    YoYValue_Node.InnerText = records[i].YoYValue.ToString();
                    parent_Node.AppendChild(YoYValue_Node);

                    var YoY_Node = datas.CreateElement("YoY");
                    YoY_Node.InnerText = String.Format("{0:P2}", records[i].YoYValue > 0 ? (records[i].SumValue - records[i].YoYValue) / records[i].YoYValue : 1);
                    parent_Node.AppendChild(YoY_Node);

                    var QoQValue_Node = datas.CreateElement("QoQValue");
                    QoQValue_Node.InnerText = records[i].QoQValue.ToString();
                    parent_Node.AppendChild(QoQValue_Node);

                    var QoQ_Node = datas.CreateElement("QoQ");
                    QoQ_Node.InnerText = String.Format("{0:P2}", records[i].QoQValue > 0 ? (records[i].SumValue - records[i].QoQValue) / records[i].QoQValue : 1);
                    parent_Node.AppendChild(QoQ_Node);
                }

                var fileName = "EnergyReport.xls";
                var sheetName = "EnergyReport";
                var title = "动力环境监控中心系统 能耗管理报表";
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, names, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Data To Cache
        /// </summary>
        private List<EnergyInfo> AddDataToCache() {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "energy-records");
                HttpRuntime.Cache.Remove(cacheKey);

                if (LscsComboBox.SelectedItem == null) { return null; }
                if (String.IsNullOrEmpty(CountItemField.Text.Trim())) { return null; }
                var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
                if (ids.Length != 2) { return null; }
                var lscId = Int32.Parse(ids[0]);
                var groupId = Int32.Parse(ids[1]);
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) { return null; }

                var nodeEntity = new BNode();
                var fromTime = DateTime.Parse(FromDate.Text);
                var toTime = DateTime.Parse(ToDate.Text);
                var countItem = CountItemField.Text.Trim();
                var area2Name = WebUtility.DefaultString;
                var area3Name = WebUtility.DefaultString;
                if (Area2ComboBox.SelectedIndex > 0) { area2Name = Area2ComboBox.SelectedItem.Text; }
                if (Area3ComboBox.SelectedIndex > 0) { area3Name = Area3ComboBox.SelectedItem.Text; }
                var nodes = nodeEntity.GetNodes(lscId, EnmNodeType.Aic, WebUtility.DefaultString, area2Name, area3Name, WebUtility.DefaultString, WebUtility.DefaultString, WebUtility.DefaultString, WebUtility.DefaultString, countItem);
                var gNodes = from node in nodes
                             join gnode in lscUser.Group.GroupNodes on node.DevID equals gnode.NodeID
                             select node;

                var AINodes1 = nodeEntity.GetAIStatic(lscId, WebUtility.DefaultInt32, fromTime, toTime);
                var nodesCount1 = from node in gNodes
                                  join ai in AINodes1 on node.NodeID equals ai.NodeID into nodesGroup
                                  select new {
                                      LscID = node.LscID,
                                      LscName = node.LscName,
                                      Area1ID = node.Area1ID,
                                      Area1Name = node.Area1Name,
                                      Area2ID = node.Area2ID,
                                      Area2Name = node.Area2Name,
                                      Area3ID = node.Area3ID,
                                      Area3Name = node.Area3Name,
                                      StaID = node.StaID,
                                      StaName = node.StaName,
                                      StaTypeName = node.StaTypeName,
                                      SumValue = nodesGroup.Any() ? nodesGroup.Max(v => v.MaxValue) - nodesGroup.Min(v => v.MaxValue) : 0
                                  };
                var staCount1 = from node in nodesCount1
                                group node by new { LscID = node.LscID, StaID = node.StaID } into staGroup
                                select new {
                                    LscID = staGroup.Key.LscID,
                                    LscName = staGroup.Max(v => v.LscName),
                                    Area1ID = staGroup.Max(v => v.Area1ID),
                                    Area1Name = staGroup.Max(v => v.Area1Name),
                                    Area2ID = staGroup.Max(v => v.Area2ID),
                                    Area2Name = staGroup.Max(v => v.Area2Name),
                                    Area3ID = staGroup.Max(v => v.Area3ID),
                                    Area3Name = staGroup.Max(v => v.Area3Name),
                                    StaID = staGroup.Key.StaID,
                                    StaName = staGroup.Max(v => v.StaName),
                                    StaTypeName = staGroup.Max(v => v.StaTypeName),
                                    SumValue = staGroup.Sum(v => v.SumValue)
                                };

                var AINodes2 = nodeEntity.GetAIStatic(lscId, WebUtility.DefaultInt32, fromTime.AddYears(-1), toTime.AddYears(-1));
                var nodesCount2 = from node in gNodes
                                  join ai in AINodes2 on new { LscID = node.LscID, NodeID = node.NodeID } equals new { LscID = ai.LscID, NodeID = ai.NodeID } into nodesGroup
                                  select new {
                                      LscID = node.LscID,
                                      LscName = node.LscName,
                                      Area1ID = node.Area1ID,
                                      Area1Name = node.Area1Name,
                                      Area2ID = node.Area2ID,
                                      Area2Name = node.Area2Name,
                                      Area3ID = node.Area3ID,
                                      Area3Name = node.Area3Name,
                                      StaID = node.StaID,
                                      StaName = node.StaName,
                                      StaTypeName = node.StaTypeName,
                                      SumValue = nodesGroup.Any() ? nodesGroup.Max(v => v.MaxValue) - nodesGroup.Min(v => v.MaxValue) : 0
                                  };
                var staCount2 = from node in nodesCount2
                                group node by new { LscID = node.LscID, StaID = node.StaID } into staGroup
                                select new {
                                    LscID = staGroup.Key.LscID,
                                    LscName = staGroup.Max(v => v.LscName),
                                    Area1ID = staGroup.Max(v => v.Area1ID),
                                    Area1Name = staGroup.Max(v => v.Area1Name),
                                    Area2ID = staGroup.Max(v => v.Area2ID),
                                    Area2Name = staGroup.Max(v => v.Area2Name),
                                    Area3ID = staGroup.Max(v => v.Area3ID),
                                    Area3Name = staGroup.Max(v => v.Area3Name),
                                    StaID = staGroup.Key.StaID,
                                    StaName = staGroup.Max(v => v.StaName),
                                    StaTypeName = staGroup.Max(v => v.StaTypeName),
                                    SumValue = staGroup.Sum(v => v.SumValue)
                                };

                var AINodes3 = nodeEntity.GetAIStatic(lscId, WebUtility.DefaultInt32, fromTime.AddMonths(-1), toTime.AddMonths(-1));
                var nodesCount3 = from node in gNodes
                                  join ai in AINodes3 on new { LscID = node.LscID, NodeID = node.NodeID } equals new { LscID = ai.LscID, NodeID = ai.NodeID } into nodesGroup
                                  select new {
                                      LscID = node.LscID,
                                      LscName = node.LscName,
                                      Area1ID = node.Area1ID,
                                      Area1Name = node.Area1Name,
                                      Area2ID = node.Area2ID,
                                      Area2Name = node.Area2Name,
                                      Area3ID = node.Area3ID,
                                      Area3Name = node.Area3Name,
                                      StaID = node.StaID,
                                      StaName = node.StaName,
                                      StaTypeName = node.StaTypeName,
                                      SumValue = nodesGroup.Any() ? nodesGroup.Max(v => v.MaxValue) - nodesGroup.Min(v => v.MaxValue) : 0
                                  };
                var staCount3 = from node in nodesCount3
                                group node by new { LscID = node.LscID, StaID = node.StaID } into staGroup
                                select new {
                                    LscID = staGroup.Key.LscID,
                                    LscName = staGroup.Max(v => v.LscName),
                                    Area1ID = staGroup.Max(v => v.Area1ID),
                                    Area1Name = staGroup.Max(v => v.Area1Name),
                                    Area2ID = staGroup.Max(v => v.Area2ID),
                                    Area2Name = staGroup.Max(v => v.Area2Name),
                                    Area3ID = staGroup.Max(v => v.Area3ID),
                                    Area3Name = staGroup.Max(v => v.Area3Name),
                                    StaID = staGroup.Key.StaID,
                                    StaName = staGroup.Max(v => v.StaName),
                                    StaTypeName = staGroup.Max(v => v.StaTypeName),
                                    SumValue = staGroup.Sum(v => v.SumValue)
                                };

                var records = (from rec1 in staCount1
                               join rec2 in staCount2 on new { LscID = rec1.LscID, StaID = rec1.StaID } equals new { LscID = rec2.LscID, StaID = rec2.StaID } into defaultRecords2
                               from defaultRec2 in defaultRecords2.DefaultIfEmpty()
                               join rec3 in staCount3 on new { LscID = rec1.LscID, StaID = rec1.StaID } equals new { LscID = rec3.LscID, StaID = rec3.StaID } into defaultRecords3
                               from defaultRec3 in defaultRecords3.DefaultIfEmpty()
                               select new EnergyInfo {
                                   LscID = rec1.LscID,
                                   LscName = rec1.LscName,
                                   Area1Name = rec1.Area1Name,
                                   Area2Name = rec1.Area2Name,
                                   Area3Name = rec1.Area3Name,
                                   StaName = rec1.StaName,
                                   StaTypeName = rec1.StaTypeName,
                                   SumValue = rec1.SumValue,
                                   YoYValue = (defaultRec2 == null ? 0 : defaultRec2.SumValue),
                                   QoQValue = (defaultRec3 == null ? 0 : defaultRec3.SumValue)
                               }).ToList();

                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, records, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
                return records;
            } catch { throw; }
        }
    }
}