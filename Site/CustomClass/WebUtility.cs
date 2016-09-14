using System;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data;
using System.Data.SqlTypes;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.DBUtility;
using Ext.Net;
using Excel = org.in2bits.MyXls;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;


namespace Delta.PECS.WebCSC.Site {
    /// <summary>
    /// Collection of utility methods for web tier
    /// </summary>
    public static class WebUtility {
        /// <summary>
        /// Default Check Code Cookie Name
        /// </summary>
        public const string DefaultCheckCodeName = "PECS.CSC.CheckCode";

        /// <summary>
        /// Default Super Token
        /// </summary>
        public const string DefaultSuperToken = "@10078";

        /// <summary>
        /// Form Timeout(Minitues)
        /// </summary>
        public const int FormTimeout = 360;

        /// <summary>
        /// Cache Timeout(Seconds)
        /// </summary>
        public const int CacheTimeout = 300;

        /// <summary>
        /// User Data
        /// </summary>
        public static readonly Dictionary<string, CscUserInfo> UserData = new Dictionary<string, CscUserInfo>();

        /// <summary>
        /// Int32 Default Value
        /// </summary>
        public static readonly int DefaultInt32 = Int32.MinValue;

        /// <summary>
        /// Int16 Default Value
        /// </summary>
        public static readonly short DefaultInt16 = Int16.MinValue;

        /// <summary>
        /// String Default Value
        /// </summary>
        public static readonly string DefaultString = String.Empty;

        /// <summary>
        /// Single Default Value
        /// </summary>
        public static readonly float DefaultFloat = Single.MinValue;

        /// <summary>
        /// Double Default Value
        /// </summary>
        public static readonly double DefaultDouble = Double.MinValue;

        /// <summary>
        /// DateTime Default Value
        /// </summary>
        public static readonly DateTime DefaultDateTime = DateTime.MinValue;

        /// <summary>
        /// Null Boolean Default Value
        /// </summary>
        public static readonly bool? DefaultNullBoolean = null;

        /// <summary>
        /// Boolean Default Value
        /// </summary>
        public static readonly bool DefaultBoolean = false;

        /// <summary>
        /// Byte Default Value
        /// </summary>
        public static readonly byte DefaultByte = Byte.MinValue;

        /// <summary>
        /// LscID Default Value
        /// </summary>
        public static readonly string DefaultLscID = "0&0";

        /// <summary>
        /// ItemID Default Value
        /// </summary>
        public static readonly string DefaultItemID = DefaultInt32.ToString();

        /// <summary>
        /// ItemName Default Value
        /// </summary>
        public static readonly string DefaultItemName = "--";

        /// <summary>
        /// Method to make sure that user's inputs are not malicious
        /// </summary>
        /// <param name="text">User's Input</param>
        /// <param name="maxLength">Maximum length of input</param>
        public static string InputText(string text, int maxLength) {
            text = text.Trim();
            if (String.IsNullOrEmpty(text)) { return String.Empty; }
            if (text.Length > maxLength) { text = text.Substring(0, maxLength); }
            text = Regex.Replace(text, "[\\s]{2,}", " ");	//two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	//<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", String.Empty);	//any other tags
            text = text.Replace("'", "''");
            return text;
        }

        /// <summary>
        /// Method to check whether input has other characters than numbers
        /// </summary>
        /// <param name="text">text</param>
        public static string CleanNonWord(string text) {
            return Regex.Replace(text, "\\W", "");
        }

        /// <summary>
        /// Method to check json input
        /// </summary>
        /// <param name="text">text</param>
        public static string JsonCharFilter(string text) {
            text = text.Replace("{", "");
            text = text.Replace("}", "");
            text = text.Replace("[", "");
            text = text.Replace("]", "");
            text = text.Replace(":", "");
            text = text.Replace("<", "");
            text = text.Replace(">", "");
            text = text.Replace("\\", "");
            text = text.Replace("\b", "");
            text = text.Replace("\t", "");
            text = text.Replace("\n", "");
            text = text.Replace("\f", "");
            text = text.Replace("\r", "");
            text = text.Replace("'", "");
            return text.Replace("\"", "");
        }

        /// <summary>
        /// Generating verification image
        /// </summary>
        /// <param name="codeLen">codeLen</param>
        public static byte[] CreateCodeImage(int codeLen) {
            var checkCode = GenerateCode(codeLen);
            if (String.IsNullOrEmpty(checkCode.Trim())) { return null; }
            var image = new Bitmap((int)Math.Ceiling((checkCode.Length * 14.5)), 30);
            var g = Graphics.FromImage(image);
            try {
                var random = new Random();
                g.Clear(Color.White);
                for (int i = 0; i < 25; i++) {
                    var x1 = random.Next(image.Width);
                    var x2 = random.Next(image.Width);
                    var y1 = random.Next(image.Height);
                    var y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                var font = new System.Drawing.Font("Arial", 14, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                var brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                for (int i = 0; i < 100; i++) {
                    var x = random.Next(image.Width);
                    var y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }

                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                var ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            } finally {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// Generating verification code
        /// </summary>
        /// <param name="codeLen">codeLen</param>
        private static string GenerateCode(int codeLen) {
            int number;
            char code;
            var checkCode = String.Empty;
            var random = new Random();
            for (int i = 0; i < codeLen; i++) {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));

                checkCode += code.ToString();
            }

            var hc = HttpContext.Current.Request.Cookies[DefaultCheckCodeName];
            if (hc != null) {
                hc.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(hc);
            }

            HttpContext.Current.Response.Cookies.Add(new HttpCookie(DefaultCheckCodeName, checkCode));
            return checkCode;
        }

        /// <summary>
        /// Generating QR Code
        /// </summary>
        /// <param name="text">text</param>
        /// <returns>QR Image</returns>
        public static byte[] CreateQRCode(string text) {
            var encoder = new QrEncoder(ErrorCorrectionLevel.L);
            QrCode qrCode; encoder.TryEncode(text, out qrCode);

            var gRenderer = new GraphicsRenderer(new FixedModuleSize(3, QuietZoneModules.Two), Brushes.Black, Brushes.White);
            var ms = new MemoryStream();
            gRenderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
            return ms.ToArray();
        }

        /// <summary>
        /// Method to get group tree icon
        /// </summary>
        /// <param name="status">status</param>
        public static string GetTreeIcon(EnmAlarmLevel status) {
            switch (status) {
                case EnmAlarmLevel.NoAlarm:
                    return "icon-tree-alarm0";
                case EnmAlarmLevel.Critical:
                    return "icon-tree-alarm1";
                case EnmAlarmLevel.Major:
                    return "icon-tree-alarm2";
                case EnmAlarmLevel.Minor:
                    return "icon-tree-alarm3";
                case EnmAlarmLevel.Hint:
                    return "icon-tree-alarm4";
                default:
                    return "icon-tree-alarm0";
            }
        }

        /// <summary>
        /// Method to show message
        /// </summary>
        /// <param name="msgType">msgType</param>
        /// <param name="msg">msg</param>
        public static void ShowMessage(EnmErrType msgType, string msg) {
            X.Msg.Show(new MessageBoxConfig {
                Title = GetMsgType(msgType),
                Message = msg,
                Buttons = MessageBox.Button.OK,
                Icon = GetMsgIcon(msgType)
            });
        }

        /// <summary>
        /// Method to show notify
        /// </summary>
        /// <param name="msgType">msgType</param>
        /// <param name="msg">msg</param>
        public static void ShowNotify(EnmErrType msgType, string msg) {
            X.Msg.Notify(GetMsgType(msgType), msg).Show();
        }

        /// <summary>
        /// Method to get message type
        /// </summary>
        /// <param name="msgType">msgType</param>
        private static string GetMsgType(EnmErrType msgType) {
            switch (msgType) {
                case EnmErrType.Error:
                    return "系统错误";
                case EnmErrType.Warning:
                    return "系统警告";
                case EnmErrType.Trace:
                    return "调试信息";
                case EnmErrType.Info:
                    return "系统提示";
                case EnmErrType.Unknow:
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get message icon
        /// </summary>
        /// <param name="msgType">msgType</param>
        private static MessageBox.Icon GetMsgIcon(EnmErrType msgType) {
            switch (msgType) {
                case EnmErrType.Error:
                    return MessageBox.Icon.ERROR;
                case EnmErrType.Warning:
                    return MessageBox.Icon.WARNING;
                case EnmErrType.Trace:
                    return MessageBox.Icon.QUESTION;
                case EnmErrType.Info:
                    return MessageBox.Icon.INFO;
                case EnmErrType.Unknow:
                    return MessageBox.Icon.NONE;
                default:
                    return MessageBox.Icon.NONE;
            }
        }

        /// <summary>
        /// Method to get node type name
        /// </summary>
        /// <param name="nodeType">nodeType</param>
        public static string GetNodeTypeName(EnmNodeType nodeType) {
            switch (nodeType) {
                case EnmNodeType.Null:
                    return "Null";
                case EnmNodeType.LSC:
                    return "LSC";
                case EnmNodeType.Area:
                    return "区域";
                case EnmNodeType.Sta:
                    return "局站";
                case EnmNodeType.Dev:
                    return "设备";
                case EnmNodeType.Dic:
                    return "遥信";
                case EnmNodeType.Aic:
                    return "遥测";
                case EnmNodeType.Doc:
                    return "遥控";
                case EnmNodeType.Aoc:
                    return "遥调";
                case EnmNodeType.Str:
                    return "Str";
                case EnmNodeType.Img:
                    return "Img";
                case EnmNodeType.Sic:
                    return "Sic";
                case EnmNodeType.SS:
                    return "SS";
                case EnmNodeType.RS:
                    return "RS";
                case EnmNodeType.RTU:
                    return "RTU";
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get user type name
        /// </summary>
        /// <param name="userType">userType</param>
        public static string GetUserTypeName(EnmUserType userType) {
            switch (userType) {
                case EnmUserType.CSC:
                    return "CSC客户";
                case EnmUserType.LSC:
                    return "LSC客户";
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get event type name
        /// </summary>
        /// <param name="eventType">eventType</param>
        public static string GetOpEventTypeName(int eventType) {
            switch (eventType) {
                case 0:
                    return "遥控";
                case 1:
                    return "遥调";
                case 2:
                    return "仿真开始";
                case 3:
                    return "仿真结束";
                case 4:
                    return "屏蔽开始";
                case 5:
                    return "屏蔽结束";
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get device type name
        /// </summary>
        /// <param name="devType">devType</param>
        public static string GetDevTypeName(EnmDevType devType) {
            try {
                switch (devType) {
                    case EnmDevType.GYPDSB:
                        return "高压配电设备";
                    case EnmDevType.DYPDSB:
                        return "低压配电设备";
                    case EnmDevType.CYFDJZ:
                        return "柴油发电机组";
                    case EnmDevType.YQFDJZ:
                        return "燃气发电机组";
                    case EnmDevType.UPS:
                        return "UPS";
                    case EnmDevType.NBQ:
                        return "逆变器";
                    case EnmDevType.ZLPDSB:
                        return "整流配电设备";
                    case EnmDevType.TYNPDSB:
                        return "太阳能供电设备";
                    case EnmDevType.DCDCBHSB:
                        return "DC-DC变换设备";
                    case EnmDevType.FLFDSB:
                        return "风力发电设备";
                    case EnmDevType.XDCZ:
                        return "蓄电池组";
                    case EnmDevType.JBKTSB:
                        return "局部空调设备";
                    case EnmDevType.JZKTSB:
                        return "集中空调设备";
                    case EnmDevType.MJ:
                        return "门禁";
                    case EnmDevType.HJSB:
                        return "环境设备";
                    case EnmDevType.FLSB:
                        return "防雷设备";
                    case EnmDevType.JKSB:
                        return "监控设备";
                    case EnmDevType.ZNDB:
                        return "智能电表";
                    case EnmDevType.ZNTFXT:
                        return "智能通风系统";
                    case EnmDevType.SPSB:
                        return "视频设备";
                    case EnmDevType.QTSB:
                    default:
                        return "Undefined";
                }
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Method to get alarm level name
        /// </summary>
        /// <param name="level">level</param>
        public static string GetAlarmLevelName(EnmAlarmLevel level) {
            switch (level) {
                case EnmAlarmLevel.NoAlarm:
                    return "全部告警";
                case EnmAlarmLevel.Critical:
                    return "一级告警";
                case EnmAlarmLevel.Major:
                    return "二级告警";
                case EnmAlarmLevel.Minor:
                    return "三级告警";
                case EnmAlarmLevel.Hint:
                    return "四级告警";
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get preAlarm level name
        /// </summary>
        /// <param name="level">level</param>
        public static string GetPreAlarmLevelName(EnmAlarmLevel level) {
            switch (level) {
                case EnmAlarmLevel.NoAlarm:
                    return "全部预警";
                case EnmAlarmLevel.Critical:
                    return "一级预警";
                case EnmAlarmLevel.Major:
                    return "二级预警";
                case EnmAlarmLevel.Minor:
                    return "三级预警";
                case EnmAlarmLevel.Hint:
                    return "四级预警";
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get confirm marking name
        /// </summary>
        /// <param name="confirmMarking">confirmMarking</param>
        public static string GetConfirmMarkingName(EnmConfirmMarking confirmMarking) {
            switch (confirmMarking) {
                case EnmConfirmMarking.NotConfirm:
                    return "未确认";
                case EnmConfirmMarking.NotDispatch:
                    return "未派修";
                case EnmConfirmMarking.Dispatched:
                    return "已派修";
                case EnmConfirmMarking.Processing:
                    return "处理中";
                case EnmConfirmMarking.Processed:
                    return "已处理";
                case EnmConfirmMarking.FileOff:
                    return "已归档";
                case EnmConfirmMarking.Cancel:
                    return "已作废";
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get service log type name
        /// </summary>
        /// <param name="logType">logType</param>
        public static string GetSvcLogTypeName(EnmSvcLogType logType) {
            switch (logType) {
                case EnmSvcLogType.Info:
                    return "普通";
                case EnmSvcLogType.Warning:
                    return "警告";
                case EnmSvcLogType.Error:
                    return "错误";
                case EnmSvcLogType.Trace:
                    return "跟踪";
                case EnmSvcLogType.Off:
                    return "无效";
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get system log level name
        /// </summary>
        /// <param name="logLevel">logLevel</param>
        public static string GetSysLogLevelName(EnmSysLogLevel logLevel) {
            switch (logLevel) {
                case EnmSysLogLevel.Debug:
                    return "调试";
                case EnmSysLogLevel.Info:
                    return "普通";
                case EnmSysLogLevel.Warn:
                    return "警告";
                case EnmSysLogLevel.Error:
                    return "错误";
                case EnmSysLogLevel.Fatal:
                    return "致命错误";
                case EnmSysLogLevel.Off:
                    return "无效";
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get system log type name
        /// </summary>
        /// <param name="logType">logType</param>
        public static string GetSysLogTypeName(EnmSysLogType logType) {
            switch (logType) {
                case EnmSysLogType.Login:
                    return "登录";
                case EnmSysLogType.Logout:
                    return "登出";
                case EnmSysLogType.Operating:
                    return "操作";
                case EnmSysLogType.Exception:
                    return "异常";
                case EnmSysLogType.Other:
                    return "其他";
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get database type name
        /// </summary>
        /// <param name="dbType">dbType</param>
        public static string GetDBTypeName(EnmDBType dbType) {
            switch (dbType) {
                case EnmDBType.SQLServer:
                    return "SQL Server";
                case EnmDBType.Oracle:
                    return "Oracle";
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get alarm filter type name
        /// </summary>
        /// <param name="type">type</param>
        public static string GetAlarmFilterTypeName(EnmAlarmFiterType type) {
            switch (type) {
                case EnmAlarmFiterType.AlarmDeviceID:
                    return "按告警设备类型";
                case EnmAlarmFiterType.AlarmLogID:
                    return "按告警逻辑分类";
                case EnmAlarmFiterType.AlarmID:
                    return "按告警标准名称";
                case EnmAlarmFiterType.AlarmDesc:
                    return "按告警描述";
                case EnmAlarmFiterType.NodeName:
                    return "按测点名称";
                case EnmAlarmFiterType.TimeShare:
                    return "按告警时长";
                case EnmAlarmFiterType.DevName:
                    return "按设备名称";
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get active values filter type name
        /// </summary>
        /// <param name="type">type</param>
        public static string GetActiveValuesFilterTypeName(EnmActiveValuesFiteType type) {
            switch (type) {
                case EnmActiveValuesFiteType.NodeName:
                    return "按测点名称";
                case EnmActiveValuesFiteType.NodeMarked:
                    return "按测点标记";
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get group tree name
        /// </summary>
        /// <param name="gti">gti</param>
        public static string GetGroupTreeName(GroupTreeInfo gti, string uid) {
            var remarks = ItemSplit(gti.Remark);
            switch (gti.NodeType) {
                case EnmNodeType.Area:
                    return gti.NodeName;
                case EnmNodeType.Sta:
                    if (remarks.Length == 3 ) {
                        var name = gti.NodeName;
                        var mid = remarks[0].Trim();
                        var staFeatures = remarks[1].Trim();
                        var staTypeName = remarks[2].Trim();

                        var values = GetGroupNodeDisplayValue(uid);
                        if((values & 1) > 0 && !String.IsNullOrEmpty(mid)) {
                            name = String.Format("{0}_{1}", mid, name);
                        }

                        if((values & 2) > 0 && !String.IsNullOrEmpty(staFeatures) && !staFeatures.Equals("NULL", StringComparison.CurrentCultureIgnoreCase)) {
                            name = String.Format("{0}_{1}", name, staFeatures);
                        }

                        if((values & 4) > 0 && !String.IsNullOrEmpty(staTypeName)) {
                            name = String.Format("{0}_{1}", name, staTypeName);
                        }

                        return name;
                    }
                    break;
                case EnmNodeType.Dev:
                    if (remarks.Length == 2) {
                        var name = gti.NodeName;
                        var productor = remarks[0].Trim();
                        var devTypeName = remarks[1].Trim();

                        if (!String.IsNullOrEmpty(productor)) {
                            name = String.Format("{0}({1})", name, productor);
                        }

                        return name;
                    }
                    break;
                default:
                    break;
            }
            return gti.NodeName;
        }

        /// <summary>
        /// Method to get group tree name
        /// </summary>
        /// <param name="gti">gti</param>
        public static string GetGroupTreeName(UDGroupTreeInfo gti, string uid) {
            var remarks = ItemSplit(gti.Remark);
            switch (gti.NodeType) {
                case EnmNodeType.Area:
                    return gti.NodeName;
                case EnmNodeType.Sta:
                    if (remarks.Length == 3) {
                        var name = gti.NodeName;
                        var mid = remarks[0].Trim();
                        var staFeatures = remarks[1].Trim();
                        var staTypeName = remarks[2].Trim();

                        var values = GetGroupNodeDisplayValue(uid);
                        if((values & 1) > 0 && !String.IsNullOrEmpty(mid)) {
                            name = String.Format("{0}_{1}", mid, name);
                        }

                        if((values & 2) > 0 && !String.IsNullOrEmpty(staFeatures) && !staFeatures.Equals("NULL", StringComparison.CurrentCultureIgnoreCase)) {
                            name = String.Format("{0}_{1}", name, staFeatures);
                        }

                        if((values & 4) > 0 && !String.IsNullOrEmpty(staTypeName)) {
                            name = String.Format("{0}_{1}", name, staTypeName);
                        }

                        return name;
                    }
                    break;
                case EnmNodeType.Dev:
                    if (remarks.Length == 2) {
                        var name = gti.NodeName;
                        var productor = remarks[0].Trim();
                        var devTypeName = remarks[1].Trim();

                        if (!String.IsNullOrEmpty(productor)) {
                            name = String.Format("{0}({1})", name, productor);
                        }

                        return name;
                    }
                    break;
                default:
                    break;
            }
            return gti.NodeName;
        }

        /// <summary>
        /// Method to get boolean name
        /// </summary>
        /// <param name="flag">flag</param>
        public static string GetBooleanName(bool flag) {
            return flag ? "是" : "否";
        }

        /// <summary>
        /// Method to get picture model name
        /// </summary>
        /// <param name="model"></param>
        public static string GetPicModelName(EnmPicModel model) {
            switch (model) {
                case EnmPicModel.CapturePic:
                    return "远程抓拍图片";
                case EnmPicModel.HistoryPic:
                    return "历史图片";
                default:
                    return "Undefined";
            }
        }

        /// <summary>
        /// Method to get completion name
        /// </summary>
        /// <param name="standard">standard</param>
        /// <param name="excellent">excellent</param>
        /// <param name="curValue">curValue</param>
        public static string GetCompletionName(SysParamInfo standard, SysParamInfo excellent, double curValue) {
            try {
                if (standard == null && excellent == null) { return String.Empty; }
                if (excellent != null) {
                    var excellentValue = Double.Parse(excellent.ParaDisplay);
                    if (curValue >= excellentValue) { return "优秀"; }
                }
                if (standard != null) {
                    var standardValue = Double.Parse(standard.ParaDisplay);
                    if (curValue >= standardValue) { return "达标"; }
                }
                return "未达标";
            } catch { throw; }
        }

        /// <summary>
        /// Method to get device ID
        /// </summary>
        /// <param name="nodeId">nodeId</param>
        public static int GetDevID(int nodeId) {
            return (int)(nodeId & 0xFFFFF800);
        }

        /// <summary>
        /// Method to get state color
        /// </summary>
        /// <param name="state">state</param>
        public static string GetStateColor(EnmState state) {
            switch (state) {
                case EnmState.NoAlarm:
                    return ColorTranslator.ToHtml(Color.Transparent);
                case EnmState.Opevent:
                    return ColorTranslator.ToHtml(Color.Transparent);
                case EnmState.Critical:
                    return ColorTranslator.ToHtml(Color.Red);
                case EnmState.Major:
                    return ColorTranslator.ToHtml(Color.Orange);
                case EnmState.Minor:
                    return ColorTranslator.ToHtml(Color.Yellow);
                case EnmState.Hint:
                    return ColorTranslator.ToHtml(Color.SkyBlue);
                case EnmState.Invalid:
                    return ColorTranslator.ToHtml(Color.Blue);
                default:
                    return ColorTranslator.ToHtml(Color.Transparent);
            }
        }

        /// <summary>
        /// Method to get level color
        /// </summary>
        /// <param name="level">level</param>
        public static string GetLevelColor(EnmAlarmLevel level) {
            switch (level) {
                case EnmAlarmLevel.NoAlarm:
                    return ColorTranslator.ToHtml(Color.Transparent);
                case EnmAlarmLevel.Critical:
                    return ColorTranslator.ToHtml(Color.Red);
                case EnmAlarmLevel.Major:
                    return ColorTranslator.ToHtml(Color.Orange);
                case EnmAlarmLevel.Minor:
                    return ColorTranslator.ToHtml(Color.Yellow);
                case EnmAlarmLevel.Hint:
                    return ColorTranslator.ToHtml(Color.SkyBlue);
                default:
                    return ColorTranslator.ToHtml(Color.Transparent);
            }
        }

        /// <summary>
        /// Method to get excel node color
        /// </summary>
        /// <param name="state">state</param>
        public static Excel.Color GetExcelStateColor(EnmState state) {
            switch (state) {
                case EnmState.NoAlarm:
                    return Excel.Colors.White;
                case EnmState.Opevent:
                    return Excel.Colors.White;
                case EnmState.Critical:
                    return Excel.Colors.Red;
                case EnmState.Major:
                    return Excel.Colors.Default34;
                case EnmState.Minor:
                    return Excel.Colors.Yellow;
                case EnmState.Hint:
                    return Excel.Colors.Default2C;
                case EnmState.Invalid:
                    return Excel.Colors.Blue;
                default:
                    return Excel.Colors.White;
            }
        }

        /// <summary>
        /// Method to get excel alarm color
        /// </summary>
        /// <param name="level">level</param>
        public static Excel.Color GetExcelAlarmColor(EnmAlarmLevel level) {
            switch (level) {
                case EnmAlarmLevel.NoAlarm:
                    return Excel.Colors.White;
                case EnmAlarmLevel.Critical:
                    return Excel.Colors.Red;
                case EnmAlarmLevel.Major:
                    return Excel.Colors.Default34;
                case EnmAlarmLevel.Minor:
                    return Excel.Colors.Yellow;
                case EnmAlarmLevel.Hint:
                    return Excel.Colors.Default2C;
                default:
                    return Excel.Colors.White;
            }
        }

        /// <summary>
        /// Method to get node value
        /// </summary>
        /// <param name="nodeType">node</param>
        public static string GetNodeValue(NodeInfo node) {
            switch (node.NodeType) {
                case EnmNodeType.Dic:
                case EnmNodeType.Doc:
                    return GetDesc(node.Remark, (int)node.Value);
                case EnmNodeType.Aic:
                case EnmNodeType.Aoc:
                    return String.Format("{0} {1}", node.Value.ToString("0.000"), node.Remark);
                default:
                    break;
            }
            return String.Empty;
        }

        /// <summary>
        /// Method to get description string
        /// </summary>
        /// <param name="Describe">Describe</param>
        /// <param name="value">value</param>
        public static string GetDesc(string describe, int value) {
            var describes = describe.Split(new char[] { '\t' });
            for (int i = 0; i < describes.Length; i++) {
                var subDescribes = describes[i].Split(new char[] { '&' });
                if (subDescribes.Length == 2 && subDescribes[0].Trim().Equals(value.ToString())) { return subDescribes[1].Trim(); }
            }
            return String.Empty;
        }

        /// <summary>
        /// Method to create LSC connection string
        /// </summary>
        /// <param name="lsc">lsc</param>
        public static string CreateLscConnectionString(LscInfo lsc) {
            if (lsc != null) { return SqlHelper.CreateConnectionString(false, lsc.DBServer, lsc.DBPort, lsc.DBName, lsc.DBUID, lsc.DBPwd, 120); }
            return String.Empty;
        }

        /// <summary>
        /// Method to create history LSC connection string
        /// </summary>
        /// <param name="lsc">lsc</param>
        public static string CreateHisLscConnectionString(LscInfo lsc) {
            if (lsc != null) { return SqlHelper.CreateConnectionString(false, lsc.HisDBServer, lsc.HisDBPort, lsc.HisDBName, lsc.HisDBUID, lsc.HisDBPwd, 120); }
            return String.Empty;
        }

        /// <summary>
        /// Method to get datetime string
        /// </summary>
        /// <param name="dt">DateTime</param>
        public static string GetDateString(DateTime dt) {
            if (dt == DefaultDateTime) { return String.Empty; }
            return dt.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Method to get datetime string
        /// </summary>
        /// <param name="dt">DateTime</param>
        public static string GetDateString2(DateTime dt) {
            if (dt == DefaultDateTime) { return String.Empty; }
            return dt.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Method to get datetime string
        /// </summary>
        /// <param name="dt">DateTime</param>
        /// <param name="format">format</param>
        public static string GetDateString(DateTime dt, string format) {
            if (dt == DefaultDateTime) { return String.Empty; }
            return dt.ToString(format);
        }

        /// <summary>
        /// Method to get string interval
        /// </summary>
        /// <param name="fromTime">fromTime</param>
        /// <param name="toTime">toTime</param>
        public static string GetDateTimeInterval(DateTime fromTime, DateTime toTime) {
            if (fromTime == DefaultDateTime) { return String.Empty; }
            if (toTime == DefaultDateTime) { toTime = DateTime.Now; }
            var ts = toTime.Subtract(fromTime);
            return String.Format("{0:0000}.{1:00}:{2:00}:{3:00}", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
        }

        /// <summary>
        /// Method to get string interval
        /// </summary>
        /// <param name="interval">interval</param>
        public static string GetDateTimeFromSec(double interval) {
            var ts = TimeSpan.FromSeconds(interval);
            return String.Format("{0:0000}. {1:00}: {2:00}: {3:00}", ts.Days, ts.Hours, ts.Minutes, ts.Seconds);
        }

        /// <summary>
        /// Method to get seconds interval
        /// </summary>
        /// <param name="date">date</param>
        public static double GetSecondFromDateTime(string date) {
            try {
                double seconds = 0; date = date.Replace(" ", "");
                var parts = date.Split(new char[] { '.' });
                if (parts.Length == 2) {
                    var day = Int32.Parse(parts[0]); seconds += day * 24 * 3600;
                    var times = parts[1].Split(new char[] { ':' });
                    if (times.Length == 3) {
                        int hrs = Int32.Parse(times[0]);
                        int mins = Int32.Parse(times[1]);
                        int sec = Int32.Parse(times[2]);
                        seconds += hrs * 3600 + mins * 60 + sec;
                    }
                }
                return seconds;
            } catch { throw; }
        }

        /// <summary>
        /// Method to export data to excel
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <param name="sheetName">sheetName</param>
        /// <param name="title">title</param>
        /// <param name="subtitle">subTitle</param>
        /// <param name="names">names</param>
        /// <param name="datas">datas</param>
        public static Excel.XlsDocument ExportDataToExcel(string fileName, string sheetName, string title, string subtitle, XmlDocument names, XmlDocument datas) {
            if (String.IsNullOrEmpty(fileName)) { fileName = "Excel.xls"; }
            if (String.IsNullOrEmpty(sheetName)) { sheetName = "Sheet1"; }
            if (String.IsNullOrEmpty(title)) { title = "No Title"; }
            if (String.IsNullOrEmpty(subtitle)) { subtitle = "No Subtitle"; }

            //获取列名集合
            var nameList = names.SelectNodes("/Names/Name");
            if (nameList.Count == 0) { return null; }

            //获取数据集合
            var dataList = datas.SelectNodes("/records/record");

            //创建Excel管理对象
            var xls = new Excel.XlsDocument();
            xls.FileName = fileName;
            xls.SummaryInformation.Author = "PECS Web";
            xls.SummaryInformation.Subject = title;
            xls.DocumentSummaryInformation.Company = "Delta GreenTech(China) Co., Ltd.";

            //Sheet标题样式  
            var titleXF = xls.NewXF();
            titleXF.HorizontalAlignment = Excel.HorizontalAlignments.Centered;
            titleXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            titleXF.UseBorder = true;
            titleXF.BottomLineStyle = 1;
            titleXF.BottomLineColor = Excel.Colors.Black;
            titleXF.RightLineStyle = 1;
            titleXF.RightLineColor = Excel.Colors.Black;
            titleXF.Font.Bold = true;
            titleXF.Font.Height = 15 * 20;

            //Sheet副标题样式  
            var subTitleXF = xls.NewXF();
            subTitleXF.HorizontalAlignment = Excel.HorizontalAlignments.Right;
            subTitleXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            subTitleXF.UseBorder = true;
            subTitleXF.RightLineStyle = 1;
            subTitleXF.RightLineColor = Excel.Colors.Black;
            subTitleXF.Font.Bold = false;
            subTitleXF.Font.Height = 10 * 20;

            //列标题样式  
            var columnTitleXF = xls.NewXF();
            columnTitleXF.HorizontalAlignment = Excel.HorizontalAlignments.Centered;
            columnTitleXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            columnTitleXF.UseBorder = true;
            columnTitleXF.TopLineStyle = 1;
            columnTitleXF.TopLineColor = Excel.Colors.Black;
            columnTitleXF.BottomLineStyle = 1;
            columnTitleXF.BottomLineColor = Excel.Colors.Black;
            columnTitleXF.RightLineStyle = 1;
            columnTitleXF.RightLineColor = Excel.Colors.Black;
            columnTitleXF.Pattern = 1;
            columnTitleXF.PatternBackgroundColor = Excel.Colors.Grey;
            columnTitleXF.PatternColor = Excel.Colors.Grey;
            columnTitleXF.Font.Bold = true;
            columnTitleXF.Font.Height = 11 * 20;
            columnTitleXF.Font.ColorIndex = 1;

            //数据单元格样式  
            var dataXF = xls.NewXF();
            dataXF.HorizontalAlignment = Excel.HorizontalAlignments.Left;
            dataXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            dataXF.UseBorder = true;
            dataXF.RightLineStyle = 1;
            dataXF.RightLineColor = Excel.Colors.Black;
            dataXF.BottomLineStyle = 1;
            dataXF.BottomLineColor = Excel.Colors.Black;
            dataXF.UseProtection = false;
            dataXF.TextWrapRight = true;
            dataXF.Font.Height = 10 * 20;

            //创建表单
            var sheet = xls.Workbook.Worksheets.Add(sheetName);

            //合并单元格
            var titleArea = new Excel.MergeArea(1, 1, 1, nameList.Count);
            sheet.AddMergeArea(titleArea);
            var subTitleArea = new Excel.MergeArea(2, 2, 1, nameList.Count);
            sheet.AddMergeArea(subTitleArea);

            //列设置  
            var col = new Excel.ColumnInfo(xls, sheet);
            col.ColumnIndexStart = 0;
            col.ColumnIndexEnd = (ushort)(nameList.Count - 1);
            col.Width = 15 * 256;
            sheet.AddColumnInfo(col);

            //行设置  
            //Excel.RowInfo rol = new Excel.RowInfo();
            //rol.RowHeight = 16 * 20;
            //rol.RowIndexStart = 3;
            //rol.RowIndexEnd = (ushort)dataList.Count;
            //sheet.AddRowInfo(rol);

            //开始填充数据到单元格  
            var cells = sheet.Cells;
            cells.Add(1, 1, title, titleXF);
            sheet.Rows[1].RowHeight = 40 * 20;
            cells.Add(2, 1, subtitle, subTitleXF);
            sheet.Rows[2].RowHeight = 20 * 20;
            for (int j = 1; j <= nameList.Count; j++) {
                if (j > 1) {
                    cells.Add(1, j, String.Empty, titleXF);
                    cells.Add(2, j, String.Empty, subTitleXF);
                }
                cells.Add(3, j, nameList[j - 1].Attributes["Header"].Value, columnTitleXF);
            }
            sheet.Rows[3].RowHeight = 20 * 20;
            for (int k = 0; k < dataList.Count; k++) {
                var columns = dataList[k].ChildNodes;
                for (int g = 0; g < nameList.Count; g++) {
                    foreach (XmlNode xn in columns) {
                        if (xn.Name.Equals(nameList[g].Attributes["DataIndex"].Value)) {
                            cells.Add(4 + k, g + 1, xn.InnerText, dataXF);
                            break;
                        }
                    }
                }
            }
            return xls;
        }

        /// <summary>
        /// Method to export alarms to excel
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <param name="sheetName">sheetName</param>
        /// <param name="title">title</param>
        /// <param name="subtitle">subTitle</param>
        /// <param name="names">names</param>
        /// <param name="datas">datas</param>
        /// <param name="showBackground">showBackground</param>
        public static Excel.XlsDocument ExportAlarmsToExcel(string fileName, string sheetName, string title, string subtitle, XmlDocument names, XmlDocument datas, bool showBackground) {
            if (String.IsNullOrEmpty(fileName)) { fileName = "Excel.xls"; }
            if (String.IsNullOrEmpty(sheetName)) { sheetName = "Sheet1"; }
            if (String.IsNullOrEmpty(title)) { title = "No Title"; }
            if (String.IsNullOrEmpty(subtitle)) { subtitle = "No SubTitle"; }

            //获取列名集合
            var nameList = names.SelectNodes("/Names/Name");
            if (nameList.Count == 0) { return null; }

            //获取数据集合
            var dataList = datas.SelectNodes("/records/record");

            //创建Excel管理对象
            var xls = new Excel.XlsDocument();
            xls.FileName = fileName;
            xls.SummaryInformation.Author = "PECS Web";
            xls.SummaryInformation.Subject = title;
            xls.DocumentSummaryInformation.Company = "Delta GreenTech(China) Co., Ltd.";

            //Sheet标题样式  
            var titleXF = xls.NewXF();
            titleXF.HorizontalAlignment = Excel.HorizontalAlignments.Centered;
            titleXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            titleXF.UseBorder = true;
            titleXF.BottomLineStyle = 1;
            titleXF.BottomLineColor = Excel.Colors.Black;
            titleXF.RightLineStyle = 1;
            titleXF.RightLineColor = Excel.Colors.Black;
            titleXF.Font.Bold = true;
            titleXF.Font.Height = 15 * 20;

            //Sheet副标题样式  
            var subTitleXF = xls.NewXF();
            subTitleXF.HorizontalAlignment = Excel.HorizontalAlignments.Right;
            subTitleXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            subTitleXF.UseBorder = true;
            subTitleXF.RightLineStyle = 1;
            subTitleXF.RightLineColor = Excel.Colors.Black;
            subTitleXF.Font.Bold = false;
            subTitleXF.Font.Height = 10 * 20;

            //列标题样式  
            var columnTitleXF = xls.NewXF();
            columnTitleXF.HorizontalAlignment = Excel.HorizontalAlignments.Centered;
            columnTitleXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            columnTitleXF.UseBorder = true;
            columnTitleXF.TopLineStyle = 1;
            columnTitleXF.TopLineColor = Excel.Colors.Black;
            columnTitleXF.BottomLineStyle = 1;
            columnTitleXF.BottomLineColor = Excel.Colors.Black;
            columnTitleXF.RightLineStyle = 1;
            columnTitleXF.RightLineColor = Excel.Colors.Black;
            columnTitleXF.Pattern = 1;
            columnTitleXF.PatternBackgroundColor = Excel.Colors.Grey;
            columnTitleXF.PatternColor = Excel.Colors.Grey;
            columnTitleXF.Font.Bold = true;
            columnTitleXF.Font.Height = 11 * 20;
            columnTitleXF.Font.ColorIndex = 1;

            //数据单元格样式  
            var dataXF = xls.NewXF();
            dataXF.HorizontalAlignment = Excel.HorizontalAlignments.Left;
            dataXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            dataXF.UseBorder = true;
            dataXF.RightLineStyle = 1;
            dataXF.RightLineColor = Excel.Colors.Black;
            dataXF.BottomLineStyle = 1;
            dataXF.BottomLineColor = Excel.Colors.Black;
            dataXF.UseProtection = false;
            dataXF.TextWrapRight = true;
            dataXF.Font.Height = 10 * 20;

            //创建表单
            var sheet = xls.Workbook.Worksheets.Add(sheetName);

            //合并单元格
            var titleArea = new Excel.MergeArea(1, 1, 1, nameList.Count);
            sheet.AddMergeArea(titleArea);
            var subTitleArea = new Excel.MergeArea(2, 2, 1, nameList.Count);
            sheet.AddMergeArea(subTitleArea);

            //列设置  
            var col = new Excel.ColumnInfo(xls, sheet);
            col.ColumnIndexStart = 0;
            col.ColumnIndexEnd = (ushort)(nameList.Count - 1);
            col.Width = 15 * 256;
            sheet.AddColumnInfo(col);

            //行设置  
            //Excel.RowInfo rol = new Excel.RowInfo();
            //rol.RowHeight = 16 * 20;
            //rol.RowIndexStart = 3;
            //rol.RowIndexEnd = (ushort)dataList.Count;
            //sheet.AddRowInfo(rol);

            //开始填充数据到单元格  
            var cells = sheet.Cells;
            cells.Add(1, 1, title, titleXF);
            sheet.Rows[1].RowHeight = 40 * 20;
            cells.Add(2, 1, subtitle, subTitleXF);
            sheet.Rows[2].RowHeight = 20 * 20;
            for (int j = 1; j <= nameList.Count; j++) {
                if (j > 1) {
                    cells.Add(1, j, String.Empty, titleXF);
                    cells.Add(2, j, String.Empty, subTitleXF);
                }
                cells.Add(3, j, nameList[j - 1].Attributes["Header"].Value, columnTitleXF);
            }
            sheet.Rows[3].RowHeight = 20 * 20;
            for (int k = 0; k < dataList.Count; k++) {
                var columns = dataList[k].ChildNodes;
                if (showBackground) {
                    foreach (XmlNode xn in columns) {
                        if (xn.Name.Equals("AlarmLevel")) {
                            var level = Int32.Parse(xn.InnerText);
                            var enmLevel = Enum.IsDefined(typeof(EnmAlarmLevel), level) ? (EnmAlarmLevel)level : EnmAlarmLevel.NoAlarm;
                            dataXF.Pattern = 1;
                            dataXF.PatternBackgroundColor = GetExcelAlarmColor(enmLevel);
                            dataXF.PatternColor = GetExcelAlarmColor(enmLevel);
                            break;
                        }
                    }
                }

                for (int g = 0; g < nameList.Count; g++) {
                    foreach (XmlNode xn in columns) {
                        if (xn.Name.Equals(nameList[g].Attributes["DataIndex"].Value)) {
                            cells.Add(4 + k, g + 1, xn.InnerText, dataXF);
                            break;
                        }
                    }
                }
            }
            return xls;
        }

        /// <summary>
        /// Method to export nodes to excel
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <param name="sheetName">sheetName</param>
        /// <param name="title">title</param>
        /// <param name="subtitle">subtitle</param>
        /// <param name="names">names</param>
        /// <param name="datas">datas</param>
        /// <param name="showBackground">showBackground</param>
        public static Excel.XlsDocument ExportNodesToExcel(string fileName, string sheetName, string title, string subtitle, XmlDocument names, XmlDocument datas, bool showBackground) {
            if (String.IsNullOrEmpty(fileName)) { fileName = "Excel.xls"; }
            if (String.IsNullOrEmpty(sheetName)) { sheetName = "Sheet1"; }
            if (String.IsNullOrEmpty(title)) { title = "No Title"; }
            if (String.IsNullOrEmpty(subtitle)) { subtitle = "No SubTitle"; }

            //获取列名集合
            var nameList = names.SelectNodes("/Names/Name");
            if (nameList.Count == 0) { return null; }

            //获取数据集合
            var dataList = datas.SelectNodes("/records/record");

            //创建Excel管理对象
            var xls = new Excel.XlsDocument();
            xls.FileName = fileName;
            xls.SummaryInformation.Author = "PECS Web";
            xls.SummaryInformation.Subject = title;
            xls.DocumentSummaryInformation.Company = "Delta GreenTech(China) Co., Ltd.";

            //Sheet标题样式  
            var titleXF = xls.NewXF();
            titleXF.HorizontalAlignment = Excel.HorizontalAlignments.Centered;
            titleXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            titleXF.UseBorder = true;
            titleXF.BottomLineStyle = 1;
            titleXF.BottomLineColor = Excel.Colors.Black;
            titleXF.RightLineStyle = 1;
            titleXF.RightLineColor = Excel.Colors.Black;
            titleXF.Font.Bold = true;
            titleXF.Font.Height = 15 * 20;

            //Sheet副标题样式  
            var subTitleXF = xls.NewXF();
            subTitleXF.HorizontalAlignment = Excel.HorizontalAlignments.Right;
            subTitleXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            subTitleXF.UseBorder = true;
            subTitleXF.RightLineStyle = 1;
            subTitleXF.RightLineColor = Excel.Colors.Black;
            subTitleXF.Font.Bold = false;
            subTitleXF.Font.Height = 10 * 20;

            //列标题样式  
            var columnTitleXF = xls.NewXF();
            columnTitleXF.HorizontalAlignment = Excel.HorizontalAlignments.Centered;
            columnTitleXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            columnTitleXF.UseBorder = true;
            columnTitleXF.TopLineStyle = 1;
            columnTitleXF.TopLineColor = Excel.Colors.Black;
            columnTitleXF.BottomLineStyle = 1;
            columnTitleXF.BottomLineColor = Excel.Colors.Black;
            columnTitleXF.RightLineStyle = 1;
            columnTitleXF.RightLineColor = Excel.Colors.Black;
            columnTitleXF.Pattern = 1;
            columnTitleXF.PatternBackgroundColor = Excel.Colors.Grey;
            columnTitleXF.PatternColor = Excel.Colors.Grey;
            columnTitleXF.Font.Bold = true;
            columnTitleXF.Font.Height = 11 * 20;
            columnTitleXF.Font.ColorIndex = 1;

            //数据单元格样式  
            var dataXF = xls.NewXF();
            dataXF.HorizontalAlignment = Excel.HorizontalAlignments.Left;
            dataXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            dataXF.UseBorder = true;
            dataXF.RightLineStyle = 1;
            dataXF.RightLineColor = Excel.Colors.Black;
            dataXF.BottomLineStyle = 1;
            dataXF.BottomLineColor = Excel.Colors.Black;
            dataXF.UseProtection = false;
            dataXF.TextWrapRight = true;
            dataXF.Font.Height = 10 * 20;

            //创建表单
            var sheet = xls.Workbook.Worksheets.Add(sheetName);

            //合并单元格
            var titleArea = new Excel.MergeArea(1, 1, 1, nameList.Count);
            sheet.AddMergeArea(titleArea);
            var subTitleArea = new Excel.MergeArea(2, 2, 1, nameList.Count);
            sheet.AddMergeArea(subTitleArea);

            //列设置  
            var col = new Excel.ColumnInfo(xls, sheet);
            col.ColumnIndexStart = 0;
            col.ColumnIndexEnd = (ushort)(nameList.Count - 1);
            col.Width = 15 * 256;
            sheet.AddColumnInfo(col);

            //行设置  
            //Excel.RowInfo rol = new Excel.RowInfo();
            //rol.RowHeight = 16 * 20;
            //rol.RowIndexStart = 3;
            //rol.RowIndexEnd = (ushort)dataList.Count;
            //sheet.AddRowInfo(rol);

            //开始填充数据到单元格  
            var cells = sheet.Cells;
            cells.Add(1, 1, title, titleXF);
            sheet.Rows[1].RowHeight = 40 * 20;
            cells.Add(2, 1, subtitle, subTitleXF);
            sheet.Rows[2].RowHeight = 20 * 20;
            for (int j = 1; j <= nameList.Count; j++) {
                if (j > 1) {
                    cells.Add(1, j, String.Empty, titleXF);
                    cells.Add(2, j, String.Empty, subTitleXF);
                }
                cells.Add(3, j, nameList[j - 1].Attributes["Header"].Value, columnTitleXF);
            }
            sheet.Rows[3].RowHeight = 20 * 20;
            for (int k = 0; k < dataList.Count; k++) {
                var columns = dataList[k].ChildNodes;
                if (showBackground) {
                    foreach (XmlNode xn in columns) {
                        if (xn.Name.Equals("Status")) {
                            var status = Int32.Parse(xn.InnerText);
                            var enmstate = Enum.IsDefined(typeof(EnmState), status) ? (EnmState)status : EnmState.NoAlarm;
                            dataXF.Pattern = 1;
                            dataXF.PatternBackgroundColor = GetExcelStateColor(enmstate);
                            dataXF.PatternColor = GetExcelStateColor(enmstate);
                            break;
                        }
                    }
                }

                for (int g = 0; g < nameList.Count; g++) {
                    foreach (XmlNode xn in columns) {
                        if (xn.Name.Equals(nameList[g].Attributes["DataIndex"].Value)) {
                            cells.Add(4 + k, g + 1, xn.InnerText, dataXF);
                            break;
                        }
                    }
                }
            }
            return xls;
        }

        /// <summary>
        /// Method to export data to excel
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <param name="sheetName">sheetName</param>
        /// <param name="title">title</param>
        /// <param name="subTitle">subtitle</param>
        /// <param name="datas">datas</param>
        public static Excel.XlsDocument ExportDataToExcel(string fileName, string sheetName, string title, string subtitle, DataTable datas) {
            if (String.IsNullOrEmpty(fileName)) { fileName = "Excel.xls"; }
            if (String.IsNullOrEmpty(sheetName)) { sheetName = "Sheet1"; }
            if (String.IsNullOrEmpty(title)) { title = "No Title"; }
            if (String.IsNullOrEmpty(subtitle)) { subtitle = "No Subtitle"; }

            //获取列名集合
            DataColumn colorColumn = null;
            var columns = new List<DataColumn>();
            for (var i = 0; i < datas.Columns.Count; i++) {
                if (datas.Columns[i].ColumnName.Equals("ColorColumn")) {
                    colorColumn = datas.Columns[i];
                    continue;
                }
                columns.Add(datas.Columns[i]);
            }
            if (columns.Count == 0) { return null; }

            //创建Excel管理对象
            var xls = new Excel.XlsDocument();
            xls.FileName = fileName;
            xls.SummaryInformation.Author = "PECS Web";
            xls.SummaryInformation.Subject = title;
            xls.DocumentSummaryInformation.Company = "Delta GreenTech(China) Co., Ltd.";

            //Sheet标题样式  
            var titleXF = xls.NewXF();
            titleXF.HorizontalAlignment = Excel.HorizontalAlignments.Centered;
            titleXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            titleXF.UseBorder = true;
            titleXF.BottomLineStyle = 1;
            titleXF.BottomLineColor = Excel.Colors.Black;
            titleXF.RightLineStyle = 1;
            titleXF.RightLineColor = Excel.Colors.Black;
            titleXF.Font.Bold = true;
            titleXF.Font.Height = 15 * 20;

            //Sheet副标题样式  
            var subTitleXF = xls.NewXF();
            subTitleXF.HorizontalAlignment = Excel.HorizontalAlignments.Right;
            subTitleXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            subTitleXF.UseBorder = true;
            subTitleXF.RightLineStyle = 1;
            subTitleXF.RightLineColor = Excel.Colors.Black;
            subTitleXF.Font.Bold = false;
            subTitleXF.Font.Height = 10 * 20;

            //列标题样式  
            var columnTitleXF = xls.NewXF();
            columnTitleXF.HorizontalAlignment = Excel.HorizontalAlignments.Centered;
            columnTitleXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            columnTitleXF.UseBorder = true;
            columnTitleXF.TopLineStyle = 1;
            columnTitleXF.TopLineColor = Excel.Colors.Black;
            columnTitleXF.BottomLineStyle = 1;
            columnTitleXF.BottomLineColor = Excel.Colors.Black;
            columnTitleXF.RightLineStyle = 1;
            columnTitleXF.RightLineColor = Excel.Colors.Black;
            columnTitleXF.Pattern = 1;
            columnTitleXF.PatternBackgroundColor = Excel.Colors.Grey;
            columnTitleXF.PatternColor = Excel.Colors.Grey;
            columnTitleXF.Font.Bold = true;
            columnTitleXF.Font.Height = 11 * 20;
            columnTitleXF.Font.ColorIndex = 1;

            //数据单元格样式  
            var dataXF = xls.NewXF();
            dataXF.HorizontalAlignment = Excel.HorizontalAlignments.Left;
            dataXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            dataXF.UseBorder = true;
            dataXF.RightLineStyle = 1;
            dataXF.RightLineColor = Excel.Colors.Black;
            dataXF.BottomLineStyle = 1;
            dataXF.BottomLineColor = Excel.Colors.Black;
            dataXF.UseProtection = false;
            dataXF.TextWrapRight = true;
            dataXF.Font.Height = 10 * 20;

            //日期单元格格式
            var dateXF = xls.NewXF();
            dateXF.HorizontalAlignment = Excel.HorizontalAlignments.Left;
            dateXF.VerticalAlignment = Excel.VerticalAlignments.Centered;
            dateXF.UseBorder = true;
            dateXF.RightLineStyle = 1;
            dateXF.RightLineColor = Excel.Colors.Black;
            dateXF.BottomLineStyle = 1;
            dateXF.BottomLineColor = Excel.Colors.Black;
            dateXF.UseProtection = false;
            dateXF.TextWrapRight = true;
            dateXF.Font.Height = 10 * 20;
            dateXF.Format = "yyyy-MM-dd HH:mm:ss";

            //创建表单
            var sheet = xls.Workbook.Worksheets.Add(sheetName);

            //合并单元格
            var titleArea = new Excel.MergeArea(1, 1, 1, columns.Count);
            sheet.AddMergeArea(titleArea);
            var subTitleArea = new Excel.MergeArea(2, 2, 1, columns.Count);
            sheet.AddMergeArea(subTitleArea);

            //列设置  
            var col = new Excel.ColumnInfo(xls, sheet);
            col.ColumnIndexStart = 0;
            col.ColumnIndexEnd = (ushort)(columns.Count - 1);
            col.Width = 15 * 256;
            sheet.AddColumnInfo(col);

            //开始填充数据到单元格  
            var cells = sheet.Cells;
            cells.Add(1, 1, title, titleXF);
            sheet.Rows[1].RowHeight = 40 * 20;
            cells.Add(2, 1, subtitle, subTitleXF);
            sheet.Rows[2].RowHeight = 20 * 20;
            for (int j = 1; j <= columns.Count; j++) {
                if (j > 1) {
                    cells.Add(1, j, String.Empty, titleXF);
                    cells.Add(2, j, String.Empty, subTitleXF);
                }
                cells.Add(3, j, columns[j - 1].ColumnName, columnTitleXF);
            }
            sheet.Rows[3].RowHeight = 20 * 20;
            for (int k = 0; k < datas.Rows.Count; k++) {
                if (colorColumn != null) {
                    var color = (Excel.Color)datas.Rows[k][colorColumn.ColumnName];
                    dataXF.Pattern = dateXF.Pattern = 1;
                    dataXF.PatternBackgroundColor = dateXF.PatternBackgroundColor = color;
                    dataXF.PatternColor = dateXF.PatternColor = color;
                }

                for (int g = 0; g < columns.Count; g++) {
                    var name = columns[g].ColumnName;
                    var cell = datas.Rows[k][name];
                    var type = cell.GetType();
                    if (type == typeof(DateTime)) {
                        cells.Add(4 + k, g + 1, cell, dateXF);
                    } else if (type == typeof(DBNull)) {
                        cells.Add(4 + k, g + 1, null, dataXF);
                    } else {
                        cells.Add(4 + k, g + 1, cell, dataXF);
                    }
                }
            }
            return xls;
        }

        /// <summary>
        /// Method to get random color
        /// </summary>
        public static Color GetRandomColor() {
            var RandomNum = new Random(Guid.NewGuid().GetHashCode());
            var int_Red = RandomNum.Next(256);
            var int_Green = RandomNum.Next(256);
            var int_Blue = (int_Red + int_Green > 400) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);
        }

        /// <summary>
        /// Method to filter SAlm alarms
        /// </summary>
        /// <param name="alarms">alarms</param>
        /// <param name="lscId">lscId</param>
        public static List<AlarmInfo> AlarmSAlmFilter(List<AlarmInfo> alarms, int lscId) {
            if (alarms == null) { return alarms; }
            alarms = alarms.FindAll(alarm => {
                return alarm.LscID == lscId && (!alarm.AuxSet.Contains("SAlm:") || alarm.AuxSet.Contains("SAlm:0"));
            });
            return alarms;
        }

        /// <summary>
        /// Method to filter SAlm alarms
        /// </summary>
        /// <param name="alarms">alarms</param>
        public static List<AlarmInfo> AlarmSAlmFilter(List<AlarmInfo> alarms) {
            if (alarms == null) { return alarms; }
            alarms = alarms.FindAll(alarm => {
                return (!alarm.AuxSet.Contains("SAlm:") || alarm.AuxSet.Contains("SAlm:0"));
            });
            return alarms;
        }

        /// <summary>
        /// Method to add database log information
        /// </summary>
        /// <param name="eventTime">eventTime</param>
        /// <param name="eventType">eventType</param>
        /// <param name="eventCode">eventCode</param>
        /// <param name="msg">msg</param>
        /// <param name="_operator">_operator</param>
        public static void WriteLog(EnmSysLogLevel level, EnmSysLogType type, string msg, string _operator) {
            try {
                var log = new SysLogInfo();
                log.EventID = DefaultInt32;
                log.EventTime = DateTime.Now;
                log.EventLevel = level;
                log.EventType = type;
                log.Message = msg;
                log.Url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
                log.ClientIP = GetClientIP();
                log.Operator = _operator;

                var logEntity = new BLog();
                logEntity.AddSysLogs(new List<SysLogInfo>() { log });
            } catch { }
        }

        /// <summary>
        /// Method to split string
        /// </summary>
        /// <param name="split">split</param>
        public static String[] StringSplit(String split) {
            if (String.IsNullOrEmpty(split)) { return new string[] { }; }
            return split.Split(new char[] { ';', '；', ',', '，' }, StringSplitOptions.None);
        }

        /// <summary>
        /// Method to split string
        /// </summary>
        /// <param name="split">split</param>
        public static String[] ItemSplit(String split) {
            if (String.IsNullOrEmpty(split)) { return new string[] { }; }
            return split.Split(new char[] { '&' }, StringSplitOptions.None);
        }

        /// <summary>
        /// Method to get CSC user alarms
        /// </summary>
        /// <param name="userData">userData</param>
        public static List<AlarmInfo> GetUserAlarms(CscUserInfo userData) {
            var alarms = new BAlarm().GetAlarms();
            var groups = new List<GroupTreeInfo>();
            foreach (var lu in userData.LscUsers) {
                groups.AddRange(lu.Group.GroupNodes.FindAll(g => g.NodeType == EnmNodeType.Dev));
            }

            return (from a in alarms
                    join g in groups on new { a.LscID, NodeID = GetDevID(a.NodeID) } equals new { g.LscID, g.NodeID }
                    select a).ToList();
        }

        /// <summary>
        /// Method to clear application caches
        /// </summary>
        public static void ClearApplicationCaches() {
            var enumerator = HttpRuntime.Cache.GetEnumerator();
            while (enumerator.MoveNext()) {
                HttpRuntime.Cache.Remove(enumerator.Key.ToString());
            }
            UserData.Clear();
        }

        /// <summary>
        /// Method to clear invalid caches
        /// </summary>
        public static void ClearInvalidCaches() {
            var keys = new List<string>(UserData.Keys);
            foreach (var key in keys) {
                if(!UserData.ContainsKey(key)) continue;
                if(DateTime.Now > UserData[key].ExpiredTime)
                    ClearUserCaches(key);
            }
        }

        /// <summary>
        /// Method to clear user caches
        /// </summary>
        /// <param name="cscUser">cscUser</param>
        public static void ClearUserCaches(string key) {
            var enumerator = HttpRuntime.Cache.GetEnumerator();
            while(enumerator.MoveNext()) {
                var cacheKey = enumerator.Key.ToString();
                if(cacheKey.StartsWith(key))
                    HttpRuntime.Cache.Remove(cacheKey);
            }

            if(UserData.ContainsKey(key))
                UserData.Remove(key);
        }

        /// <summary>
        /// Method to remove user cache
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="cacheKey">cacheKey</param>
        public static void RemoveUserCache(CscUserInfo userData, string cacheKey) {
            if (userData != null) {
                cacheKey = GetCacheKeyName(userData, cacheKey);
                HttpRuntime.Cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Method to get cache key name
        /// </summary>
        /// <param name="userData">userData</param>
        /// <param name="cacheKey">cacheKey</param>
        public static string GetCacheKeyName(CscUserInfo userData, string cacheKey) {
            if (userData != null) return String.Format("{0}-{1}", userData.Identifier, cacheKey);
            return String.Empty;
        }

        /// <summary>
        /// Method to get UI culture
        /// </summary>
        public static string GetUICulture() {
            var context = System.Web.HttpContext.Current;
            if (context.Request.Cookies["UICulture"] == null) { return "zh-cn"; }
            return context.Request.Cookies["UICulture"].Value.ToLower();
        }

        /// <summary>
        /// Method to get remote client IP
        /// </summary>
        public static string GetClientIP() {
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            else
                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }

        /// <summary>
        /// Method to get server IP
        /// </summary>
        public static string GetServerIP() {
            return HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString();
        }

        /// <summary>
        /// Method to check show maps page
        /// </summary>
        public static bool ShowNavMaps() {
            return WebConfigurationManager.AppSettings["ShowNavMaps"] != null && WebConfigurationManager.AppSettings["ShowNavMaps"].Trim().Equals("1");
        }

        /// <summary>
        /// Method to check show maps page
        /// </summary>
        public static bool FirstNavMaps() {
            return ShowNavMaps() && (WebConfigurationManager.AppSettings["FirstNavMaps"] != null && WebConfigurationManager.AppSettings["FirstNavMaps"].Trim().Equals("1"));
        }

        /// <summary>
        /// Method to get an new window
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        /// <param name="title">title</param>
        /// <param name="icon">icon</param>
        public static Window GetNewWindow(int width, int height, string title, Ext.Net.Icon icon) {
            var win = new Window();
            win.ID = "ReportWindow";
            win.Title = title;
            win.Icon = icon;
            win.Width = Unit.Pixel(width);
            win.Height = Unit.Pixel(height);
            win.Collapsible = false;
            win.Maximizable = false;
            win.Layout = "FitLayout";
            win.Modal = true;
            win.InitCenter = true;
            win.Maximized = false;
            win.CloseAction = CloseAction.Close;
            win.AutoLoad.NoCache = true;
            win.AutoLoad.Mode = LoadMode.IFrame;
            win.AutoLoad.ShowMask = true;
            return win;
        }

        /// <summary>
        /// Get Decrypt License.
        /// </summary>
        public static LicenseInfo GetDecryptLicense(String lcode, String mcode) {
            try {
                var keys = mcode.ToUpper().ToCharArray().Reverse().ToArray();
                if (keys.Length != 32) { return null; }
                var aesKey = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}", keys[3], keys[6], keys[9], keys[12], keys[13], keys[16], keys[19], keys[25], keys[29], keys[31], keys[7], keys[16], keys[12], keys[20], keys[2], keys[3]);
                var aesIV = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}", keys[5], keys[8], keys[10], keys[14], keys[20], keys[21], keys[24], keys[25], keys[29], keys[30], keys[2], keys[5], keys[6], keys[17], keys[22], keys[10]);

                var aes = new AesCryptoServiceProvider();
                aes.BlockSize = 128;
                aes.KeySize = 128;
                aes.IV = Encoding.UTF8.GetBytes(aesIV);
                aes.Key = Encoding.UTF8.GetBytes(aesKey);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                byte[] src = System.Convert.FromBase64String(lcode);
                using (ICryptoTransform decrypt = aes.CreateDecryptor()) {
                    byte[] dest = decrypt.TransformFinalBlock(src, 0, src.Length);
                    lcode = Encoding.Unicode.GetString(dest);

                    var licenses = lcode.Split(new char[] { '┋' });
                    if (licenses == null || licenses.Length != 4) { return null; }
                    return new LicenseInfo() {
                        Name = licenses[0],
                        Company = licenses[1],
                        MaxUsers = Int32.Parse(licenses[2]),
                        Expiration = Int64.Parse(licenses[3])
                    };
                }
            } catch { }
            return null;
        }

        /// <summary>
        /// whether contain target in source.
        /// </summary>
        /// <param name="target">target</param>
        /// <param name="source">source</param>
        /// <returns>true/false</returns>
        public static Boolean Contain(string target, string[] source) {
            if (target == null || source == null) { return false; }
            foreach (var s in source) {
                if (target.Contains(s)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// DBNull DateTime Handler
        /// </summary>
        /// <param name="val">val</param>
        public static object DBNullDateTimeChecker(DateTime val) {
            if (val == DefaultDateTime) { return DBNull.Value; }
            return val;
        }

        /// <summary>
        /// Gets the node names
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static int GetGroupNodeDisplayValue(string uid) {
            var values = 0;
            var parms = new BUser().GetSysParams(50000001);
            if(parms.Count > 0) {
                var param1 = parms.Find(p => p.ParaData == 0);
                if(param1 != null && param1.ParaDisplay != null && !string.IsNullOrEmpty(param1.ParaDisplay.Trim())) {
                    var displays = Newtonsoft.Json.JsonConvert.DeserializeObject<List<IDValuePair<string, int>>>(param1.ParaDisplay);
                    if(displays != null && displays.Count > 0) {
                        var current = displays.Find(d => d.ID.Equals(uid, StringComparison.CurrentCultureIgnoreCase));
                        if(current != null) {
                            values = current.Value;
                        }
                    }
                }
            }

            return values;
        }
    }
}