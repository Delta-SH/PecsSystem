using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Caching;
using System.Collections;
using System.Text;
using Ext.Net;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;

namespace Delta.PECS.WebCSC.Site {
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.Alias, Alias = "Speaker")]
    public partial class Speaker : IFramePageBase {
        protected void Page_Load(object sender, EventArgs e) {
            if (!Page.IsPostBack && !X.IsAjaxRequest) {
                var hc = Request.Cookies["ScanTime"];
                if (hc != null) {
                    hc.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(hc);
                }
            }
        }

        /// <summary>
        /// Play Handler
        /// </summary>
        /// <returns>count</returns>
        [DirectMethod(Timeout = 300000)]
        public string PlayHandler() {
            try {
                List<LscInfo> lscs = null;
                List<AlarmInfo> alarms = null;
                var audios = new List<string>();
                var userData = UserData;
                foreach (var lscUser in userData.LscUsers) {
                    if (lscUser.AlarmSoundFiterItem == null) { continue; }
                    if (lscUser.AlarmSoundFiterItem.SpDisconnect) {
                        if (lscs == null) {
                            var lscEntity = new BLsc();
                            lscs = lscEntity.GetLscs();
                        }

                        var lsc = lscs.Find(l => { return l.LscID == lscUser.LscID; });
                        if (lsc != null && !lsc.Connected) { audios.Add(String.Format("\"请注意，{0}服务器通信中断！\"", lsc.LscName)); }
                    }

                    bool isAdd = true;
                    if (lscUser.AlarmSoundFiterItem.AL1Enabled && isAdd) {
                        if (alarms == null) { alarms = WebUtility.AlarmSAlmFilter(WebUtility.GetUserAlarms(userData)); }
                        if (alarms != null && alarms.Count > 0) {
                            var agi = alarms.Find(a => { return a.LscID == lscUser.LscID && a.AlarmLevel == EnmAlarmLevel.Critical; });
                            if (agi != null) {
                                audios.Add(String.Format("\"请注意，{0}发生{1}！\"", agi.LscName, WebUtility.GetAlarmLevelName(agi.AlarmLevel)));
                                isAdd = false;
                            }
                        }
                    }

                    if (lscUser.AlarmSoundFiterItem.AL2Enabled && isAdd) {
                        if (alarms == null) { alarms = WebUtility.AlarmSAlmFilter(WebUtility.GetUserAlarms(userData)); }
                        if (alarms != null && alarms.Count > 0) {
                            var agi = alarms.Find(a => { return a.LscID == lscUser.LscID && a.AlarmLevel == EnmAlarmLevel.Major; });
                            if (agi != null) {
                                audios.Add(String.Format("\"请注意，{0}发生{1}！\"", agi.LscName, WebUtility.GetAlarmLevelName(agi.AlarmLevel)));
                                isAdd = false;
                            }
                        }
                    }

                    if (lscUser.AlarmSoundFiterItem.AL3Enabled && isAdd) {
                        if (alarms == null) { alarms = WebUtility.AlarmSAlmFilter(WebUtility.GetUserAlarms(userData)); }
                        if (alarms != null && alarms.Count > 0) {
                            var agi = alarms.Find(a => { return a.LscID == lscUser.LscID && a.AlarmLevel == EnmAlarmLevel.Minor; });
                            if (agi != null) {
                                audios.Add(String.Format("\"请注意，{0}发生{1}！\"", agi.LscName, WebUtility.GetAlarmLevelName(agi.AlarmLevel)));
                                isAdd = false;
                            }
                        }
                    }

                    if (lscUser.AlarmSoundFiterItem.AL4Enabled && isAdd) {
                        if (alarms == null) { alarms = WebUtility.AlarmSAlmFilter(WebUtility.GetUserAlarms(userData)); }
                        if (alarms != null && alarms.Count > 0) {
                            var agi = alarms.Find(a => { return a.LscID == lscUser.LscID && a.AlarmLevel == EnmAlarmLevel.Hint; });
                            if (agi != null) {
                                audios.Add(String.Format("\"请注意，{0}发生{1}！\"", agi.LscName, WebUtility.GetAlarmLevelName(agi.AlarmLevel)));
                                isAdd = false;
                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(lscUser.AlarmSoundFiterItem.SpDevFilter) || !String.IsNullOrEmpty(lscUser.AlarmSoundFiterItem.SpNodeFilter)) {
                        if (alarms == null) { alarms = WebUtility.AlarmSAlmFilter(WebUtility.GetUserAlarms(userData)); }
                        if (alarms != null && alarms.Count > 0) {
                            DateTime curScan, scanTime;
                            curScan = scanTime = alarms.Max(a => a.UpdateTime);

                            if (Request.Cookies["ScanTime"] != null) { scanTime = ConvertStringToDateTime(Request.Cookies["ScanTime"].Value); }
                            if (Request.Cookies["ScanTime"] == null || !lscUser.AlarmSoundFiterItem.SpLoop) {
                                var cookie = new HttpCookie("ScanTime", curScan.Ticks.ToString());
                                Response.Cookies.Add(cookie);
                            }

                            var agis = alarms.FindAll(a => {
                                return a.LscID == lscUser.LscID
                                    && a.UpdateTime > scanTime
                                    && (WebUtility.StringSplit(lscUser.AlarmSoundFiterItem.SpDevFilter).Contains(a.DevName)
                                    || WebUtility.StringSplit(lscUser.AlarmSoundFiterItem.SpNodeFilter).Contains(a.NodeName));
                            });
                            if (agis != null) {
                                foreach (var agi in agis) {
                                    audios.Add(String.Format("\"{0}{1}{2}{3}{4}{5}发生{6}！\"",
                                    agi.LscName,
                                    lscUser.AlarmSoundFiterItem.SpArea2 ? agi.Area2Name : String.Empty,
                                    lscUser.AlarmSoundFiterItem.SpArea3 ? agi.Area3Name : String.Empty,
                                    lscUser.AlarmSoundFiterItem.SpStation ? agi.StaName : String.Empty,
                                    lscUser.AlarmSoundFiterItem.SpDevice ? agi.DevName : String.Empty,
                                    lscUser.AlarmSoundFiterItem.SpNode ? agi.NodeName : String.Empty,
                                    lscUser.AlarmSoundFiterItem.SpALDesc ? WebUtility.GetAlarmLevelName(agi.AlarmLevel) : "告警"));
                                }
                            }
                        }
                    }
                }
                return String.Format("[{0}]", String.Join(",", audios.ToArray()));
            } catch (Exception err) {
                WebUtility.WriteLog(EnmSysLogLevel.Error, EnmSysLogType.Exception, err.ToString(), Page.User.Identity.Name);
            }
            return String.Empty;
        }

        /// <summary>
        /// Convert date ticks To DateTime
        /// </summary>
        /// <param name="date">date ticks</param>
        /// <returns>DateTime</returns>
        private DateTime ConvertStringToDateTime(string date) {
            try {
                var dateTick = Int64.Parse(date);
                return new DateTime(dateTick);
            } catch { return DateTime.Now; }
        }
    }
}