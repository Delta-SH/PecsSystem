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
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "DevLoadReport")]
    public partial class DevLoadReport : PageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack && !X.IsAjaxRequest) {
                BeginDate.Text = WebUtility.GetDateString(DateTime.Today.AddMonths(-1));
                EndDate.Text = WebUtility.GetDateString(DateTime.Now);
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
        /// Station ComboBox Refresh
        /// </summary>
        protected void OnStaRefresh(object sender, StoreRefreshDataEventArgs e) {
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
                        var area3Id = Area3ComboBox.SelectedIndex > 0 ? Int32.Parse(Area3ComboBox.SelectedItem.Value) : WebUtility.DefaultInt32;
                        var dict = comboboxEntity.GetStas(lscId, area3Id, groupId);
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

                StaStore.DataSource = data;
                StaStore.DataBind();
            } catch (Exception err) {
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
                if (dict != null && dict.Count > 0) {
                    foreach (var key in dict) {
                        data.Add(new {
                            Id = key.Key,
                            Name = key.Value
                        });
                    }
                }

                DevTypeStore.DataSource = data;
                DevTypeStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Main Grid Store Refresh
        /// </summary>
        protected void OnMainGridRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var cacheKey = WebUtility.GetCacheKeyName(UserData, "dev-load-report");
                var nodes = HttpRuntime.Cache[cacheKey] as List<DevLoadPageEntity>;
                if (nodes == null) { nodes = AddDataToCache(); }
                if (nodes != null && nodes.Count > 0) {
                    if (end > nodes.Count) { end = nodes.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscName = nodes[i].Device.LscName,
                            Area1Name = nodes[i].Device.Area1Name,
                            Area2Name = nodes[i].Device.Area2Name,
                            Area3Name = nodes[i].Device.Area3Name,
                            StaName = nodes[i].Device.StaName,
                            DevTypeName = nodes[i].Device.DevTypeName,
                            DevName = nodes[i].Device.DevName,
                            SingleRatedCapacity = nodes[i].Device.SingleRatedCapacity * nodes[i].Device.ModuleCount,
                            DevDesignCapacity = nodes[i].Device.DevDesignCapacity,
                            TotalRatedCapacity = nodes[i].Device.TotalRatedCapacity,
                            RedundantCapacity = nodes[i].Device.RedundantCapacity,
                            LoadRate = nodes[i].Device.DevDesignCapacity > 0 ? String.Format("{0:P2}", nodes[i].Value / nodes[i].Device.DevDesignCapacity) : "",
                            LoadLevel = nodes[i].Device.DevDesignCapacity > 0 ? GetLoadLevel(nodes[i].SubDev, nodes[i].Value / nodes[i].Device.DevDesignCapacity) : ""
                        });
                    }
                }

                e.Total = (nodes != null ? nodes.Count : 0);
                MainGridStore.DataSource = data;
                MainGridStore.DataBind();
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
                var cacheKey = WebUtility.GetCacheKeyName(UserData, "dev-load-report");
                var nodes = HttpRuntime.Cache[cacheKey] as List<DevLoadPageEntity>;
                if (nodes == null) { nodes = AddDataToCache(); }
                if (nodes == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var colNames = e.ExtraParams["ColumnNames"];
                var names = new XmlDocument();
                names.LoadXml(colNames);
                var datas = new XmlDocument();
                var root = datas.CreateElement("records");
                datas.AppendChild(root);
                for (int i = 0; i < nodes.Count; i++) {
                    var parent_Node = datas.CreateElement("record");
                    root.AppendChild(parent_Node);

                    var ID_Node = datas.CreateElement("ID");
                    ID_Node.InnerText = (i + 1).ToString();
                    parent_Node.AppendChild(ID_Node);

                    var LscName_Node = datas.CreateElement("LscName");
                    LscName_Node.InnerText = nodes[i].Device.LscName;
                    parent_Node.AppendChild(LscName_Node);

                    var Area1Name_Node = datas.CreateElement("Area1Name");
                    Area1Name_Node.InnerText = nodes[i].Device.Area1Name;
                    parent_Node.AppendChild(Area1Name_Node);

                    var Area2Name_Node = datas.CreateElement("Area2Name");
                    Area2Name_Node.InnerText = nodes[i].Device.Area2Name;
                    parent_Node.AppendChild(Area2Name_Node);

                    var Area3Name_Node = datas.CreateElement("Area3Name");
                    Area3Name_Node.InnerText = nodes[i].Device.Area3Name;
                    parent_Node.AppendChild(Area3Name_Node);

                    var StaName_Node = datas.CreateElement("StaName");
                    StaName_Node.InnerText = nodes[i].Device.StaName;
                    parent_Node.AppendChild(StaName_Node);

                    var DevTypeName_Node = datas.CreateElement("DevTypeName");
                    DevTypeName_Node.InnerText = nodes[i].Device.DevTypeName;
                    parent_Node.AppendChild(DevTypeName_Node);

                    var DevName_Node = datas.CreateElement("DevName");
                    DevName_Node.InnerText = nodes[i].Device.DevName;
                    parent_Node.AppendChild(DevName_Node);

                    var SingleRatedCapacity_Node = datas.CreateElement("SingleRatedCapacity");
                    SingleRatedCapacity_Node.InnerText = (nodes[i].Device.SingleRatedCapacity * nodes[i].Device.ModuleCount).ToString();
                    parent_Node.AppendChild(SingleRatedCapacity_Node);

                    var DevDesignCapacity_Node = datas.CreateElement("DevDesignCapacity");
                    DevDesignCapacity_Node.InnerText = nodes[i].Device.DevDesignCapacity.ToString();
                    parent_Node.AppendChild(DevDesignCapacity_Node);

                    var TotalRatedCapacity_Node = datas.CreateElement("TotalRatedCapacity");
                    TotalRatedCapacity_Node.InnerText = nodes[i].Device.TotalRatedCapacity.ToString();
                    parent_Node.AppendChild(TotalRatedCapacity_Node);

                    var RedundantCapacity_Node = datas.CreateElement("RedundantCapacity");
                    RedundantCapacity_Node.InnerText = nodes[i].Device.RedundantCapacity.ToString();
                    parent_Node.AppendChild(RedundantCapacity_Node);

                    var LoadRate_Node = datas.CreateElement("LoadRate");
                    LoadRate_Node.InnerText = nodes[i].Device.DevDesignCapacity > 0 ? String.Format("{0:P2}", nodes[i].Value / nodes[i].Device.DevDesignCapacity) : "";
                    parent_Node.AppendChild(LoadRate_Node);

                    var LoadLevel_Node = datas.CreateElement("LoadLevel");
                    LoadLevel_Node.InnerText = nodes[i].Device.DevDesignCapacity > 0 ? GetLoadLevel(nodes[i].SubDev, nodes[i].Value / nodes[i].Device.DevDesignCapacity) : "";
                    parent_Node.AppendChild(LoadLevel_Node);
                }

                var fileName = "DevLoadReport.xls";
                var sheetName = "DevLoadReport";
                var title = "动力环境监控中心系统 设备运行负载报表";
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
        private List<DevLoadPageEntity> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "dev-load-report");
            HttpRuntime.Cache.Remove(cacheKey);

            if (LscsComboBox.SelectedItem.Value == "" ) { return null; }
            var ids = WebUtility.ItemSplit(LscsComboBox.SelectedItem.Value);
            if (ids.Length != 2) { return null; }
            var lscId = Int32.Parse(ids[0]);
            var groupId = Int32.Parse(ids[1]);
            var lsc = new BLsc().GetLsc(lscId);
            if (lsc == null) {
                WebUtility.ShowMessage(EnmErrType.Warning, "无法获取Lsc信息");
                return null;
            }

            var fromTime = DateTime.Parse(BeginDate.Text);
            var toTime = DateTime.Parse(EndDate.Text);

            var devTypes = new Dictionary<Int32, String>();
            foreach (var dt in DevTypeMultiCombo.SelectedItems) {
                devTypes[Int32.Parse(dt.Value)] = dt.Text;
            }
            if (devTypes.Count == 0) { return null; }

            var devices = new BOther().GetDevices(lsc.LscID, groupId);
            if (Area2ComboBox.SelectedIndex > 0) {
                var area2Id = Int32.Parse(Area2ComboBox.SelectedItem.Value);
                devices = devices.FindAll(s => s.Area2ID == area2Id);
            }
            if (Area3ComboBox.SelectedIndex > 0) {
                var area3Id = Int32.Parse(Area3ComboBox.SelectedItem.Value);
                devices = devices.FindAll(s => s.Area3ID == area3Id);
            }
            if (StaComboBox.SelectedIndex > 0) {
                var staId = Int32.Parse(StaComboBox.SelectedItem.Value);
                devices = devices.FindAll(s => s.StaID == staId);
            }
            if (devTypes.Count > 0) {
                devices = devices.FindAll(s => devTypes.ContainsKey(s.DevTypeID));
            }

            var connectionString = WebUtility.CreateLscConnectionString(lsc);
            var sdevs = new BOther().GetSubDev(lsc.LscID, lsc.LscName, connectionString);
            var values = new BNode().GetMaxHisAIV(lsc.LscID, fromTime, toTime);

            var temp1 = from sd in sdevs
                        join v in values on sd.AicID equals v.NodeID
                        select new {
                            SubDev = sd,
                            v.Value
                        };

            var result = (from dev in devices
                         join t1 in temp1 on dev.DevID equals t1.SubDev.DevID into lt
                         from dt in lt.DefaultIfEmpty()
                         select new DevLoadPageEntity {
                             Device = dev,
                             SubDev = dt != null ? dt.SubDev : null,
                             Value = dt != null ? dt.Value : 0
                         }).ToList();
                        

            var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
            HttpRuntime.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            return result;
        }

        /// <summary>
        /// Gets load level names.
        /// </summary>
        private string GetLoadLevel(SubDevInfo sdi, float rate) {
            if (sdi == null) { return ""; }
            if (sdi.AlarmLevel != 0) { return sdi.AlarmLevel.ToString(); }
            if (rate <= sdi.EventValue4) { return "4"; }
            if (rate <= sdi.EventValue3) { return "3"; }
            if (rate <= sdi.EventValue2) { return "2"; }
            return "1";
        }
    }

    public class DevLoadPageEntity {
        /// <summary>
        /// Device
        /// </summary>
        public DeviceInfo Device { get; set; }

        /// <summary>
        /// SubDev
        /// </summary>
        public SubDevInfo SubDev { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public float Value { get; set; }
    }
}