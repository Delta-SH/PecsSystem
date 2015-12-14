using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Caching;
using System.Reflection;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;
using Excel = org.in2bits.MyXls;

namespace Delta.PECS.WebCSC.Site
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "AlarmWndClass")]
    public partial class AlarmWnd : IFramePageBase
    {
        protected void Page_Load(object sender, EventArgs e) {
            try {
                var title = Request.QueryString["Title"];
                if (!String.IsNullOrEmpty(title)) { Page.Header.Title = title; }
                if (!X.IsAjaxRequest && !Page.IsPostBack) {
                    var userData = UserData;
                    if (!userData.Super) {
                        if (userData.MaxOpLevel <= EnmUserLevel.Ordinary) {
                            AlarmWndGridRowMenu.Visible = false;
                        }

                        if (userData.MaxOpLevel < EnmUserLevel.Attendant) {
                            AlarmWndGridRowItem1.Visible = false;
                            AlarmWndGridRowSeparator1.Visible = false;
                        }
                    }

                    AddDataToCache(); 
                }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Refresh Alarm Window
        /// </summary>
        protected void OnAlarmWndRefresh(object sender, StoreRefreshDataEventArgs e) {
            try {
                var start = Int32.Parse(e.Parameters["start"]);
                var limit = Int32.Parse(e.Parameters["limit"]);
                var end = start + limit;
                var data = new List<object>(limit);

                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "alarm-wnd-data");
                var alarms = HttpRuntime.Cache[cacheKey] as List<AlarmInfo>;
                if (alarms == null) { alarms = AddDataToCache(); }
                if (alarms != null && alarms.Count > 0) {
                    if (end > alarms.Count) { end = alarms.Count; }
                    for (int i = start; i < end; i++) {
                        data.Add(new {
                            ID = i + 1,
                            LscId = alarms[i].LscID,
                            LscName = alarms[i].LscName,
                            SerialNO = alarms[i].SerialNO,
                            Area1Name = alarms[i].Area1Name,
                            Area2Name = alarms[i].Area2Name,
                            Area3Name = alarms[i].Area3Name,
                            Area4Name = alarms[i].Area4Name,
                            StaName = alarms[i].StaName,
                            DevName = alarms[i].DevName,
                            DevDesc = alarms[i].DevDesc,
                            NodeId = alarms[i].NodeID,
                            NodeType = (int)alarms[i].NodeType,
                            NodeName = alarms[i].NodeName,
                            AlarmID = alarms[i].AlarmID,
                            AlarmDesc = alarms[i].AlarmDesc,
                            AlarmDevice = alarms[i].AlarmDeviceType,
                            AlarmLogic = alarms[i].AlarmLogType,
                            AlarmName = alarms[i].AlarmName,
                            AlarmClass = alarms[i].AlarmClass,
                            AlarmLevel = (int)alarms[i].AlarmLevel,
                            AlarmLevelName = WebUtility.GetAlarmLevelName(alarms[i].AlarmLevel),
                            NMAlarmID = alarms[i].NMAlarmID,
                            StartTime = WebUtility.GetDateString(alarms[i].StartTime),
                            EndTime = WebUtility.GetDateString(alarms[i].EndTime),
                            ConfirmMarking = (int)alarms[i].ConfirmMarking,
                            ConfirmMarkingName = WebUtility.GetConfirmMarkingName(alarms[i].ConfirmMarking),
                            ConfirmTime = WebUtility.GetDateString(alarms[i].ConfirmTime),
                            ConfirmName = alarms[i].ConfirmName,
                            TimeInterval = WebUtility.GetDateTimeInterval(alarms[i].StartTime, alarms[i].EndTime),
                            ProjName = alarms[i].ProjName,
                            MAlm = alarms[i].EndTime != WebUtility.DefaultDateTime ? alarms[i].AuxSet : alarms[i].AuxAlarmDesc,
                            Conn = alarms[i].EndTime != WebUtility.DefaultDateTime ? alarms[i].AuxSet : alarms[i].AuxAlarmDesc,
                            TurnCount = alarms[i].TurnCount
                        });
                    }
                }

                e.Total = (alarms != null ? alarms.Count : 0);
                AlarmWndStore.DataSource = data;
                AlarmWndStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Refresh Alarm Window SubPanel
        /// </summary>
        protected void OnAlarmWndSubPanelRefresh(object sender, DirectEventArgs e) {
            try {
                var data = new List<object>();
                var lscId = Int32.Parse(e.ExtraParams["LscID"]);
                var serialNO = e.ExtraParams["SerialNO"];
                var alarmId = Int32.Parse(e.ExtraParams["AlarmID"]);
                var projName = e.ExtraParams["ProjName"];

                var userData = UserData;
                var alarmDetail = userData.StandardProtocol.Find(sp => { return sp.AlarmID == alarmId; });
                if (alarmDetail != null) {
                    data.Add(new {
                        SerialNO = serialNO,
                        AlarmID = alarmDetail.AlarmID,
                        NMAlarmID = alarmDetail.NMAlarmID,
                        AlarmLogType = alarmDetail.AlarmLogType,
                        SubAlarmLogType = alarmDetail.SubAlarmLogType,
                        DevEffect = alarmDetail.DevEffect,
                        OperEffect = alarmDetail.OperEffect,
                        AlarmInterpret = alarmDetail.AlarmInterpret,
                        OtherInfo = String.Empty,
                        Correlation = "否",
                        ProjName = projName
                    });
                }

                AlarmWndSubStore.DataSource = data;
                AlarmWndSubStore.DataBind();
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
            }
        }

        /// <summary>
        /// Confirm Alarm Click
        /// </summary>
        protected void OnConfirmAlarmClick(object sender, DirectEventArgs e) {
            try {
                var lscId = Int32.Parse(e.ExtraParams["LscId"]);
                var serialNO = Int32.Parse(e.ExtraParams["SerialNO"]);
                var confirmMarking = Int32.Parse(e.ExtraParams["ConfirmMarking"]);
                var enmConfirmMarking = Enum.IsDefined(typeof(EnmConfirmMarking), confirmMarking) ? (EnmConfirmMarking)confirmMarking : EnmConfirmMarking.NotConfirm;
                if (enmConfirmMarking != EnmConfirmMarking.NotConfirm) { return; }

                var userData = UserData;
                var lscUser = userData.LscUsers.Find(lu => { return lu.LscID == lscId; });
                if (lscUser == null) {
                    string warningMsg = "无法获取用户信息，告警确认失败。";
                    WebUtility.WriteLog(EnmSysLogLevel.Warn, EnmSysLogType.Operating, warningMsg, Page.User.Identity.Name);
                    WebUtility.ShowMessage(EnmErrType.Warning, warningMsg);
                    return;
                }

                var orderEntity = new BOrder();
                var order = new OrderInfo();
                order.LscID = lscId;
                order.TargetID = serialNO;
                order.TargetType = EnmNodeType.Null;
                order.OrderType = EnmActType.ConfirmAlarm;
                order.RelValue1 = ((int)EnmConfirmMarking.NotDispatch).ToString();
                order.RelValue2 = lscUser.UserName;
                order.RelValue3 = WebUtility.DefaultString;
                order.RelValue4 = WebUtility.DefaultString;
                order.RelValue5 = WebUtility.DefaultString;
                order.UpdateTime = DateTime.Now;
                orderEntity.AddOrder(order);
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Alarm Window Submit
        /// </summary>
        protected void OnAlarmWnd_Submit(object sender, StoreSubmitDataEventArgs e) {
            try {
                var userData = UserData;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "alarm-wnd-data");
                var alarms = HttpRuntime.Cache[cacheKey] as List<AlarmInfo>;
                if (alarms == null) { alarms = AddDataToCache(); }
                if (alarms == null) {
                    WebUtility.ShowMessage(EnmErrType.Warning, "获取数据时发生错误，导出失败！");
                    return;
                }

                var datas = new DataTable();
                datas.Columns.Add(new DataColumn("Lsc名称", typeof(String)));
                datas.Columns.Add(new DataColumn("地区名称", typeof(String)));
                datas.Columns.Add(new DataColumn("县市名称", typeof(String)));
                datas.Columns.Add(new DataColumn("局站名称", typeof(String)));
                datas.Columns.Add(new DataColumn("设备名称", typeof(String)));
                datas.Columns.Add(new DataColumn("设备描述", typeof(String)));
                datas.Columns.Add(new DataColumn("测点名称", typeof(String)));
                datas.Columns.Add(new DataColumn("告警描述", typeof(String)));
                datas.Columns.Add(new DataColumn("告警设备类型", typeof(String)));
                datas.Columns.Add(new DataColumn("告警逻辑分类", typeof(String)));
                datas.Columns.Add(new DataColumn("告警标准名称", typeof(String)));
                datas.Columns.Add(new DataColumn("告警类别", typeof(String)));
                datas.Columns.Add(new DataColumn("告警级别", typeof(String)));
                datas.Columns.Add(new DataColumn("告警时间", typeof(DateTime)));
                datas.Columns.Add(new DataColumn("结束时间", typeof(DateTime)));
                datas.Columns.Add(new DataColumn("处理标识", typeof(String)));
                datas.Columns.Add(new DataColumn("处理时间", typeof(DateTime)));
                datas.Columns.Add(new DataColumn("处理人员", typeof(String)));
                datas.Columns.Add(new DataColumn("告警历时", typeof(String)));
                datas.Columns.Add(new DataColumn("动环监控告警ID", typeof(String)));
                datas.Columns.Add(new DataColumn("工程预约", typeof(String)));
                datas.Columns.Add(new DataColumn("主告警", typeof(String)));
                datas.Columns.Add(new DataColumn("关联告警", typeof(String)));
                datas.Columns.Add(new DataColumn("翻转次数", typeof(Int32)));
                datas.Columns.Add(new DataColumn("ColorColumn", typeof(Excel.Color)));

                DataRow row;
                for (int i = 0; i < alarms.Count; i++) {
                    row = datas.NewRow();
                    row["Lsc名称"] = alarms[i].LscName;
                    row["地区名称"] = alarms[i].Area2Name;
                    row["县市名称"] = alarms[i].Area3Name;
                    row["局站名称"] = alarms[i].StaName;
                    row["设备名称"] = alarms[i].DevName;
                    row["设备描述"] = alarms[i].DevDesc;
                    row["测点名称"] = alarms[i].NodeName;
                    row["告警描述"] = alarms[i].AlarmDesc;
                    row["告警设备类型"] = alarms[i].AlarmDeviceType;
                    row["告警逻辑分类"] = alarms[i].AlarmLogType;
                    row["告警标准名称"] = alarms[i].AlarmName;
                    row["告警类别"] = alarms[i].AlarmClass;
                    row["告警级别"] = WebUtility.GetAlarmLevelName(alarms[i].AlarmLevel);
                    row["告警时间"] = WebUtility.DBNullDateTimeChecker(alarms[i].StartTime);
                    row["结束时间"] = WebUtility.DBNullDateTimeChecker(alarms[i].EndTime);
                    row["处理标识"] = WebUtility.GetConfirmMarkingName(alarms[i].ConfirmMarking);
                    row["处理时间"] = WebUtility.DBNullDateTimeChecker(alarms[i].ConfirmTime);
                    row["处理人员"] = alarms[i].ConfirmName;
                    row["告警历时"] = WebUtility.GetDateTimeInterval(alarms[i].StartTime, alarms[i].EndTime);
                    row["动环监控告警ID"] = alarms[i].NMAlarmID;
                    row["工程预约"] = WebUtility.GetBooleanName(!String.IsNullOrEmpty(alarms[i].ProjName));
                    row["主告警"] = WebUtility.GetBooleanName(alarms[i].AuxSet.Equals("MAlm"));
                    row["关联告警"] = WebUtility.GetBooleanName(alarms[i].AuxSet.Equals("Conn"));
                    row["翻转次数"] = alarms[i].TurnCount;
                    row["ColorColumn"] = WebUtility.GetExcelAlarmColor(alarms[i].AlarmLevel);
                    datas.Rows.Add(row);
                }

                var fileName = "AlarmDetails.xls";
                var sheetName = "告警详单";
                var title = String.Format("动力环境监控中心系统 {0}", Page.Header.Title);
                var subTitle = String.Format("值班员:{0}  日期:{1}", Page.User.Identity.Name, WebUtility.GetDateString(DateTime.Now));
                var xls = WebUtility.ExportDataToExcel(fileName, sheetName, title, subTitle, datas);
                if (xls != null) { xls.Send(); }
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
                WebUtility.ShowMessage(EnmErrType.Error, err.Message);
            }
        }

        /// <summary>
        /// Add Data To Cache
        /// </summary>
        private List<AlarmInfo> AddDataToCache() {
            var userData = UserData;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "alarm-wnd-data");
            HttpRuntime.Cache.Remove(cacheKey);

            List<AlarmInfo> alarms = null;
            if (String.IsNullOrEmpty(Request.QueryString["Type"])) { return alarms; }
            switch (Request.QueryString["Type"].ToLower()) {
                case "home_acs":
                    alarms = GetASCFilterData(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "map_sta":
                    alarms = GetMapStaData(userData, Request.QueryString["LscID"], Request.QueryString["StaName"], Request.QueryString["Level"]);
                    break;
                case "map_total":
                    alarms = GetMapTotalData(userData, Request.QueryString["Level"]);
                    break;
                case "alarm_salm":
                    alarms = GetSAlmData(userData, Request.QueryString["LscID"], Request.QueryString["SerialNO"]);
                    break;
                case "hisalarm_salm":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetHisSAlmData(userData, Request.QueryString["LscID"], Request.QueryString["SerialNO"]);
                    break;
                case "major_incident":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetMajorIncidents(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "important_alarm":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetImportantAlarms(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "hisalarm_frequency":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetHisAlarmFrequency(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "hisalarm_min_interval":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetHisAlarmMinInterval(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "hisalarm_max_interval":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetHisAlarmMaxInterval(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "hisalarm_category":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetHisAlarmCategory(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "dev_alarm_count":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetDevAlarmCount(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "alarm_standardization":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetAlarmStandardization(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "syskpi_1x":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetSysKPIData_1x(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "syskpi_2x":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetSysKPIData_2x(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "syskpi_3x":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetSysKPIData_3x(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "syskpi_4x":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetSysKPIData_4x(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "syskpi_5x":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetSysKPIData_5x(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "syskpi_6x":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetSysKPIData_6x(userData, Request.QueryString["RowIndex"], Request.QueryString["DataIndex"]);
                    break;
                case "kpi_report_104":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetKPIReport104Data(userData, Request.QueryString["LscID"], Request.QueryString["Area3ID"], Request.QueryString["BuildingID"]);
                    break;
                case "kpi_report_105":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetKPIReport105Data(userData, Request.QueryString["LscID"], Request.QueryString["Area3ID"], Request.QueryString["BuildingID"]);
                    break;
                case "kpi_report_106":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetKPIReport106Data(userData, Request.QueryString["LscID"], Request.QueryString["Area3ID"], Request.QueryString["BuildingID"]);
                    break;
                case "kpi_report_107":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetKPIReport107Data(userData, Request.QueryString["LscID"], Request.QueryString["Area3ID"], Request.QueryString["BuildingID"]);
                    break;
                case "kpi_report_108":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetKPIReport108Data(userData, Request.QueryString["LscID"], Request.QueryString["Area3ID"], Request.QueryString["BuildingID"]);
                    break;
                case "kpi_report_111":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetKPIReport111Data(userData, Request.QueryString["LscID"]);
                    break;
                case "kpi_report_116":
                    AlarmWndGridRowItem1.Visible = false;
                    AlarmWndGridRowSeparator1.Visible = false;
                    alarms = GetKPIReport116Data(userData, Request.QueryString["LscID"]);
                    break;
                default:
                    break;
            }

            if (alarms != null) {
                alarms = alarms.OrderByDescending(alarm => alarm.StartTime).ToList();
                var cacheDuration = Int32.Parse(WebConfigurationManager.AppSettings["DefaultCacheDuration"]);
                HttpRuntime.Cache.Insert(cacheKey, alarms, null, Cache.NoAbsoluteExpiration, TimeSpan.FromSeconds(cacheDuration), CacheItemPriority.Default, null);
            }
            return alarms;
        }

        /// <summary>
        /// Get ASC Filter Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<AlarmInfo> GetASCFilterData(CscUserInfo userData, string rowIndex, string dataIndex) {
            try {
                if (String.IsNullOrEmpty(rowIndex) || String.IsNullOrEmpty(dataIndex)) { return null; }
                var row = Int32.Parse(rowIndex);
                var column = dataIndex;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "acs-filter-data");
                var data = HttpRuntime.Cache[cacheKey] as DataTable;
                if (data == null || data.Rows.Count <= row) { return null; }
                var cell2 = data.Rows[row][2];
                var cellx = data.Rows[row][column];
                if (cell2 == null || cellx == null) { return null; }
                var lscId = Int32.Parse(cell2.ToString());
                var ids = WebUtility.StringSplit(cellx.ToString());
                if (ids.Length == 0) { return null; }
                var series = (new List<string>(ids)).ConvertAll<int>(id => Int32.Parse(id));
                var alarms = WebUtility.AlarmSAlmFilter(WebUtility.GetUserAlarms(userData), lscId);
                return (from alarm in alarms
                        join s in series on alarm.SerialNO equals s
                        select alarm).ToList();
            } catch { throw; }
        }

        /// <summary>
        /// Get Map Station Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="staName">staName</param>
        /// <param name="level">level</param>
        private List<AlarmInfo> GetMapStaData(CscUserInfo userData, string lscId, string staName, string level) {
            try {
                if (String.IsNullOrEmpty(lscId) || String.IsNullOrEmpty(staName) || String.IsNullOrEmpty(level)) { return null; }
                var ld = Int32.Parse(lscId);
                var ll = Int32.Parse(level);
                var alarms = WebUtility.AlarmSAlmFilter(WebUtility.GetUserAlarms(userData), ld);
                return alarms.FindAll(alarm => { return alarm.StaName.Equals(staName) && (ll == 0 || ll == (int)alarm.AlarmLevel); });
            } catch { throw; }
        }

        /// <summary>
        /// Get Map Total Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="level">level</param>
        private List<AlarmInfo> GetMapTotalData(CscUserInfo userData, string level) {
            try {
                var alarms = WebUtility.AlarmSAlmFilter(WebUtility.GetUserAlarms(userData));
                if (!String.IsNullOrEmpty(level)) {
                    var ll = Int32.Parse(level);
                    if (ll != 0) { alarms = alarms.FindAll(alarm => { return ll == (int)alarm.AlarmLevel; }); }
                }
                return alarms;
            } catch { throw; }
        }

        /// <summary>
        /// Get SAlm Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="serialNO">serialNO</param>
        private List<AlarmInfo> GetSAlmData(CscUserInfo userData, string lscId, string serialNO) {
            try {
                if (String.IsNullOrEmpty(lscId) || String.IsNullOrEmpty(serialNO)) { return null; }
                var ld = Int32.Parse(lscId);
                var sn = Int32.Parse(serialNO);
                var filter = String.Format("SAlm:{0}", serialNO);
                return WebUtility.GetUserAlarms(userData).FindAll(agi => { return agi.LscID == ld && agi.AuxSet.Contains(filter); });
            } catch { throw; }
        }

        /// <summary>
        /// Get History SAlm Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="serialNO">serialNO</param>
        private List<AlarmInfo> GetHisSAlmData(CscUserInfo userData, string lscId, string serialNO) {
            try {
                if (String.IsNullOrEmpty(lscId) || String.IsNullOrEmpty(serialNO)) { return null; }
                var ld = Int32.Parse(lscId);
                var sn = Int32.Parse(serialNO);
                var filter = String.Format("SAlm:{0}", serialNO);

                var cacheKey = WebUtility.GetCacheKeyName(userData, "his-alarms");
                var alarms = HttpRuntime.Cache[cacheKey] as List<AlarmInfo>;
                if (alarms == null) { return null; }
                return alarms.FindAll(agi => { return agi.LscID == ld && agi.AuxAlarmDesc.Contains(filter); });
            } catch { throw; }
        }

        /// <summary>
        /// Get Major Incident Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<AlarmInfo> GetMajorIncidents(CscUserInfo userData, string rowIndex, string dataIndex) {
            var row = Int32.Parse(rowIndex);
            var column = dataIndex;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "major-incident-source2");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<AlarmInfo>>;
            if (data == null) { return null; }
            var key = String.Format("{0}-{1}", rowIndex, dataIndex);
            if (!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get Important Alarm Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<AlarmInfo> GetImportantAlarms(CscUserInfo userData, string rowIndex, string dataIndex) {
            var row = Int32.Parse(rowIndex);
            var column = dataIndex;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "important-alarm-source2");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<AlarmInfo>>;
            if (data == null) { return null; }
            var key = String.Format("{0}-{1}", rowIndex, dataIndex);
            if (!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get History Alarm Frequency Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="key">key</param>
        private List<AlarmInfo> GetHisAlarmFrequency(CscUserInfo userData, string rowIndex, string dataIndex) {
            var row = Int32.Parse(rowIndex);
            var column = dataIndex;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "his-alarm-frequency-report2");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<AlarmInfo>>;
            if (data == null) { return null; }
            var key = String.Format("{0}-{1}", rowIndex, dataIndex);
            if (!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get History Alarm Interval Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<AlarmInfo> GetHisAlarmMinInterval(CscUserInfo userData, string rowIndex, string dataIndex) {
            var row = Int32.Parse(rowIndex);
            var column = dataIndex;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "his-alarm-min-interval-source2");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<AlarmInfo>>;
            if (data == null) { return null; }
            var key = String.Format("{0}-{1}", rowIndex, dataIndex);
            if (!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get History Alarm Interval Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<AlarmInfo> GetHisAlarmMaxInterval(CscUserInfo userData, string rowIndex, string dataIndex) {
            var row = Int32.Parse(rowIndex);
            var column = dataIndex;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "his-alarm-max-interval-source2");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<AlarmInfo>>;
            if (data == null) { return null; }
            var key = String.Format("{0}-{1}", rowIndex, dataIndex);
            if (!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get History Alarm Category Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<AlarmInfo> GetHisAlarmCategory(CscUserInfo userData, string rowIndex, string dataIndex) {
            try {
                var row = Int32.Parse(rowIndex);
                var column = dataIndex;
                var cacheKey = WebUtility.GetCacheKeyName(userData, "his-alarm-category-source2");
                var data = HttpRuntime.Cache[cacheKey] as DataTable;
                if (data == null || data.Rows.Count <= row) { return null; }
                var cellx = data.Rows[row][column];
                if (cellx == null || cellx == DBNull.Value) { return null; }
                return (List<AlarmInfo>)cellx;
            } catch { throw; }
        }

        /// <summary>
        /// Get History Alarm Category Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<AlarmInfo> GetDevAlarmCount(CscUserInfo userData, string rowIndex, string dataIndex) {
            var row = Int32.Parse(rowIndex);
            var column = dataIndex;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "dev-alarm-source2");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<AlarmInfo>>;
            if (data == null) { return null; }
            var key = String.Format("{0}-{1}", rowIndex, dataIndex);
            if (!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get Alarm Standardization Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<AlarmInfo> GetAlarmStandardization(CscUserInfo userData, string rowIndex, string dataIndex) {
            var row = Int32.Parse(rowIndex);
            var column = dataIndex;
            var cacheKey = WebUtility.GetCacheKeyName(userData, "alarm-standardization-source2");
            var data = HttpRuntime.Cache[cacheKey] as Dictionary<String, List<AlarmInfo>>;
            if (data == null) { return null; }
            var key = String.Format("{0}-{1}", rowIndex, dataIndex);
            if (!data.ContainsKey(key)) { return null; }
            return data[key];
        }

        /// <summary>
        /// Get SysKPI 1x Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<AlarmInfo> GetSysKPIData_1x(CscUserInfo userData, string rowIndex, string dataIndex) {
            try {
                if (String.IsNullOrEmpty(rowIndex) || String.IsNullOrEmpty(dataIndex)) { return null; }
                var row = Int32.Parse(rowIndex);
                var cacheKey = WebUtility.GetCacheKeyName(userData, "normal-device-rate");
                var data = HttpRuntime.Cache[cacheKey] as List<NormalDevRateInfo>;

                if (data == null || data.Count <= row) { return null; }
                return data[row].CntAlarms;
            } catch { throw; }
        }

        /// <summary>
        /// Get SysKPI 2x Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<AlarmInfo> GetSysKPIData_2x(CscUserInfo userData, string rowIndex, string dataIndex) {
            try {
                if (String.IsNullOrEmpty(rowIndex) || String.IsNullOrEmpty(dataIndex)) { return null; }
                var row = Int32.Parse(rowIndex);
                var cacheKey = WebUtility.GetCacheKeyName(userData, "device-repair-rate");
                var data = HttpRuntime.Cache[cacheKey] as List<DevRepairRate>;

                if (data == null || data.Count <= row) { return null; }
                switch (dataIndex) {
                    case "AlarmCnt":
                        return data[row].CntAlarms;
                    case "TotalAlarmCnt":
                        return data[row].TotalAlarms;
                    default:
                        return null;
                }
            } catch { throw; }
        }

        /// <summary>
        /// Get SysKPI 3x Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<AlarmInfo> GetSysKPIData_3x(CscUserInfo userData, string rowIndex, string dataIndex) {
            try {
                if (String.IsNullOrEmpty(rowIndex) || String.IsNullOrEmpty(dataIndex)) { return null; }
                var row = Int32.Parse(rowIndex);
                var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-success-rate");
                var data = HttpRuntime.Cache[cacheKey] as List<AlmKPIRate>;

                if (data == null || data.Count <= row) { return null; }
                switch (dataIndex) {
                    case "SAlmCnt":
                        return data[row].SAlm;
                    case "OriginalBAlmCnt":
                        return data[row].OriginalBAlm;
                    case "BAlmCnt":
                        return data[row].BAlm;
                    case "TotalAlarmCnt":
                        return data[row].TotalAlarm;
                    default:
                        return null;
                }
            } catch { throw; }
        }

        /// <summary>
        /// Get SysKPI 4x Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<AlarmInfo> GetSysKPIData_4x(CscUserInfo userData, string rowIndex, string dataIndex) {
            try {
                if (String.IsNullOrEmpty(rowIndex) || String.IsNullOrEmpty(dataIndex)) { return null; }
                var row = Int32.Parse(rowIndex);
                var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-compression-rate");
                var data = HttpRuntime.Cache[cacheKey] as List<AlmKPIRate>;

                if (data == null || data.Count <= row) { return null; }
                switch (dataIndex) {
                    case "SAlmCnt":
                        return data[row].SAlm;
                    case "OriginalBAlmCnt":
                        return data[row].OriginalBAlm;
                    case "BAlmCnt":
                        return data[row].BAlm;
                    case "TotalAlarmCnt":
                        return data[row].TotalAlarm;
                    default:
                        return null;
                }
            } catch { throw; }
        }

        /// <summary>
        /// Get SysKPI 5x Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<AlarmInfo> GetSysKPIData_5x(CscUserInfo userData, string rowIndex, string dataIndex) {
            try {
                if (String.IsNullOrEmpty(rowIndex) || String.IsNullOrEmpty(dataIndex)) { return null; }
                var row = Int32.Parse(rowIndex);
                var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-confirm-rate");
                var data = HttpRuntime.Cache[cacheKey] as List<AlmKPIRate>;

                if (data == null || data.Count <= row) { return null; }
                switch (dataIndex) {
                    case "TotalAlm":
                        return data[row].TotalAlarm;
                    case "ConfirmAlm":
                        return data[row].BAlm;
                    default:
                        return null;
                }
            } catch { throw; }
        }

        /// <summary>
        /// Get SysKPI 6x Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="rowIndex">rowIndex</param>
        /// <param name="dataIndex">dataIndex</param>
        private List<AlarmInfo> GetSysKPIData_6x(CscUserInfo userData, string rowIndex, string dataIndex) {
            if(String.IsNullOrEmpty(rowIndex) || String.IsNullOrEmpty(dataIndex)) { return null; }
            var row = Int32.Parse(rowIndex);
            var cacheKey = WebUtility.GetCacheKeyName(userData, "alm-related-rate");
            var data = HttpRuntime.Cache[cacheKey] as List<NormalDevRateInfo>;

            if(data == null || data.Count <= row) { return null; }
            switch(dataIndex) {
                case "TotalAlm":
                    return data[row].CntAlarms;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Get KPI Report104 Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="buildingId">buildingId</param>
        private List<AlarmInfo> GetKPIReport104Data(CscUserInfo userData, string lscId, string area3Id, string buildingId) {
            int _lscId, _area3Id, _buildingId;
            if (!Int32.TryParse(lscId, out _lscId)) { return null; }
            if (!Int32.TryParse(buildingId, out _buildingId)) { return null; }
            if (!Int32.TryParse(area3Id, out _area3Id)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-104");
            var data = HttpRuntime.Cache[cacheKey] as List<Report104Entity>;
            if (data == null || data.Count <= 0) { return null; }
            var target = data.Find(alarm => alarm.Station.LscID == _lscId && alarm.Station.Area3ID == _area3Id && alarm.Station.BuildingID == _buildingId);
            if (target == null) { return null; }
            return target.Alarms;
        }

        /// <summary>
        /// Get KPI Report105 Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="buildingId">buildingId</param>
        private List<AlarmInfo> GetKPIReport105Data(CscUserInfo userData, string lscId, string area3Id, string buildingId) {
            int _lscId, _area3Id, _buildingId;
            if (!Int32.TryParse(lscId, out _lscId)) { return null; }
            if (!Int32.TryParse(buildingId, out _buildingId)) { return null; }
            if (!Int32.TryParse(area3Id, out _area3Id)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-105");
            var data = HttpRuntime.Cache[cacheKey] as List<Report105Entity>;
            if (data == null || data.Count <= 0) { return null; }
            var target = data.Find(alarm => alarm.Station.LscID == _lscId && alarm.Station.Area3ID == _area3Id && alarm.Station.BuildingID == _buildingId);
            if (target == null) { return null; }
            return target.Alarms;
        }

        /// <summary>
        /// Get KPI Report106 Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="buildingId">buildingId</param>
        private List<AlarmInfo> GetKPIReport106Data(CscUserInfo userData, string lscId, string area3Id, string buildingId) {
            int _lscId, _area3Id, _buildingId;
            if (!Int32.TryParse(lscId, out _lscId)) { return null; }
            if (!Int32.TryParse(buildingId, out _buildingId)) { return null; }
            if (!Int32.TryParse(area3Id, out _area3Id)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-106");
            var data = HttpRuntime.Cache[cacheKey] as List<Report106Entity>;
            if (data == null || data.Count <= 0) { return null; }
            var target = data.Find(alarm => alarm.Station.LscID == _lscId && alarm.Station.Area3ID == _area3Id && alarm.Station.BuildingID == _buildingId);
            if (target == null) { return null; }
            return target.Alarms;
        }

        /// <summary>
        /// Get KPI Report107 Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="buildingId">buildingId</param>
        private List<AlarmInfo> GetKPIReport107Data(CscUserInfo userData, string lscId, string area3Id, string buildingId) {
            int _lscId, _area3Id, _buildingId;
            if (!Int32.TryParse(lscId, out _lscId)) { return null; }
            if (!Int32.TryParse(buildingId, out _buildingId)) { return null; }
            if (!Int32.TryParse(area3Id, out _area3Id)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-107");
            var data = HttpRuntime.Cache[cacheKey] as List<Report107Entity>;
            if (data == null || data.Count <= 0) { return null; }
            var target = data.Find(alarm => alarm.Station.LscID == _lscId && alarm.Station.Area3ID == _area3Id && alarm.Station.BuildingID == _buildingId);
            if (target == null) { return null; }
            return target.Alarms;
        }

        /// <summary>
        /// Get KPI Report108 Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="buildingId">buildingId</param>
        private List<AlarmInfo> GetKPIReport108Data(CscUserInfo userData, string lscId, string area3Id, string buildingId) {
            int _lscId, _area3Id, _buildingId;
            if (!Int32.TryParse(lscId, out _lscId)) { return null; }
            if (!Int32.TryParse(buildingId, out _buildingId)) { return null; }
            if (!Int32.TryParse(area3Id, out _area3Id)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-108");
            var data = HttpRuntime.Cache[cacheKey] as List<Report108Entity>;
            if (data == null || data.Count <= 0) { return null; }
            var target = data.Find(alarm => alarm.Station.LscID == _lscId && alarm.Station.Area3ID == _area3Id && alarm.Station.BuildingID == _buildingId);
            if (target == null) { return null; }
            return target.Alarms;
        }

        /// <summary>
        /// Get KPI Report111 Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="buildingId">buildingId</param>
        private List<AlarmInfo> GetKPIReport111Data(CscUserInfo userData, string lscId) {
            int _lscId;
            if(!Int32.TryParse(lscId, out _lscId)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-111");
            var data = HttpRuntime.Cache[cacheKey] as List<Report111Entity>;
            if(data == null || data.Count <= 0) { return null; }
            var target = data.Find(alarm => alarm.LscID == _lscId);
            if(target == null) { return null; }
            return target.Alarms;
        }

        /// <summary>
        /// Get KPI Report011 Data
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="lscId">lscId</param>
        /// <param name="buildingId">buildingId</param>
        private List<AlarmInfo> GetKPIReport116Data(CscUserInfo userData, string lscId) {
            int _lscId;
            if(!Int32.TryParse(lscId, out _lscId)) { return null; }
            var cacheKey = WebUtility.GetCacheKeyName(userData, "kpi-report-116");
            var data = HttpRuntime.Cache[cacheKey] as List<Report116Entity>;
            if(data == null || data.Count <= 0) { return null; }
            var target = data.Find(alarm => alarm.LscID == _lscId);
            if(target == null) { return null; }
            return target.Alarms;
        }
    }
}