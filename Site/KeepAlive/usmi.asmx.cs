using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using Delta.PECS.WebCSC.BLL;
using Delta.PECS.WebCSC.Model;
using Delta.PECS.WebCSC.DBUtility;

namespace Delta.PECS.WebCSC.Site {
    [WebService(Namespace = "http://delta.com.cn/pecs/", Description = "操作从账号接口服务")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class usmi : System.Web.Services.WebService {
        #region 添加从帐号接口
        [WebMethod(Description = "添加从帐号接口")]
        public String addUserInfo(String user) {
            var result = new XmlDocument();
            var xdt = result.CreateXmlDeclaration("1.0", "UTF-8", null);
            var xmle1 = result.CreateElement("results");
            result.AppendChild(xdt);
            result.AppendChild(xmle1);

            var messages = new List<String>();
            var users_success = new List<KGUser>();
            var users_failure = new List<KGUser>();

            #region 解析数据
            try {
                if (user == null || String.IsNullOrEmpty(user.Trim())) {
                    var xmle2 = result.CreateElement("errorMsg");
                    xmle2.InnerText = "参数不能为空";
                    xmle1.AppendChild(xmle2);
                    return result.OuterXml;
                }

                var xml = new XmlDocument(); xml.LoadXml(user);
                var root = xml.SelectSingleNode("accounts");
                if (root == null) {
                    var xmle2 = result.CreateElement("errorMsg");
                    xmle2.InnerText = "参数解析错误，未找到accounts节点。";
                    xmle1.AppendChild(xmle2);
                    return result.OuterXml;
                }

                var accounts = root.ChildNodes;
                if (accounts.Count == 0) {
                    var xmle2 = result.CreateElement("errorMsg");
                    xmle2.InnerText = "参数解析错误，未找到account节点。";
                    xmle1.AppendChild(xmle2);
                    return result.OuterXml;
                }

                foreach (XmlNode account in accounts) {
                    var nodes = account.ChildNodes;
                    if (nodes != null && nodes.Count > 0) {
                        var utp = new KGUser();
                        try {
                            foreach (XmlNode nd in nodes) {
                                switch (nd.Name.Trim().ToLower()) {
                                    case "accid":
                                        utp.UID = nd.InnerText.Trim();
                                        break;
                                    case "userpasswordmd5":
                                        utp.PWD = nd.InnerText.Trim();
                                        break;
                                    case "name":
                                        utp.UserName = nd.InnerText.Trim();
                                        break;
                                    case "description":
                                        utp.Remark = nd.InnerText.Trim();
                                        break;
                                    case "email":
                                        utp.Email = nd.InnerText.Trim();
                                        break;
                                    case "gender":
                                        utp.Sex = nd.InnerText.Trim().Equals("女") ? 1 : 0;
                                        break;
                                    case "telephonenumber":
                                        utp.TelePhone = nd.InnerText.Trim();
                                        break;
                                    case "mobile":
                                        utp.MobilePhone = nd.InnerText.Trim();
                                        break;
                                    case "starttime":
                                        utp.StartTime = DateTime.Parse(nd.InnerText.Trim());
                                        break;
                                    case "endtime":
                                        utp.EndTime = DateTime.Parse(nd.InnerText.Trim());
                                        break;
                                    case "idcardnumber":
                                        utp.CardID = nd.InnerText.Trim();
                                        break;
                                    case "employeenumber":
                                        utp.EmployeeID = nd.InnerText.Trim();
                                        break;
                                    default:
                                        break;
                                }
                            }

                            users_success.Add(utp);
                        } catch {
                            users_failure.Add(utp);
                            messages.Add(String.Format("用户{0}解析错误", utp.UID == null ? "null" : utp.UID));
                        }
                    }
                }
            } catch (Exception err) {
                var xmle2 = result.CreateElement("errorMsg");
                xmle2.InnerText = err.Message;
                xmle1.AppendChild(xmle2);
                return result.OuterXml;
            }
            #endregion

            #region 添加数据
            try {
                if (users_success.Count == 0) {
                    if (users_failure.Count > 0) {
                        var xmle2 = result.CreateElement("result");
                        xmle2.SetAttribute("returncode", "1301");
                        foreach (var u in users_failure) {
                            var xmle3 = result.CreateElement("accId");
                            xmle3.InnerText = u.UID;
                            xmle2.AppendChild(xmle3);
                        }
                        xmle1.AppendChild(xmle2);
                    }

                    var msg = result.CreateElement("errorMsg");
                    msg.InnerText = "无效的用户信息";
                    xmle1.AppendChild(msg);
                    return result.OuterXml;
                }

                var lscs = new BLsc().GetLscs();
                if (lscs.Count == 0) {
                    var xmle2 = result.CreateElement("result");
                    xmle2.SetAttribute("returncode", "1301");
                    foreach (var u in users_failure) {
                        var xmle3 = result.CreateElement("accId");
                        xmle3.InnerText = u.UID;
                        xmle2.AppendChild(xmle3);
                    }
                    xmle1.AppendChild(xmle2);

                    var msg = result.CreateElement("errorMsg");
                    msg.InnerText = "无效的Lsc信息";
                    xmle1.AppendChild(msg);
                    return result.OuterXml;
                }

                var dic = new Dictionary<String, KGUser>();
                foreach (var lsc in lscs) {
                    var connectionString = WebUtility.CreateLscConnectionString(lsc);
                    foreach (var us in users_success) {
                        try {
                            SaveUser(connectionString, lsc.LscID, us);
                        } catch {
                            dic[us.UID] = us;
                            messages.Add(String.Format("添加用户{0}失败[{1},{2}]", us.UID, lsc.LscID, lsc.LscName));
                        }
                    }
                }

                users_success.RemoveAll(u => dic.ContainsKey(u.UID));
                users_failure.AddRange(dic.Select(u => u.Value));
                if (users_success.Count > 0) {
                    var xmle2 = result.CreateElement("result");
                    xmle2.SetAttribute("returncode", "1300");
                    foreach (var u in users_success) {
                        var xmle3 = result.CreateElement("accId");
                        xmle3.InnerText = u.UID;
                        xmle2.AppendChild(xmle3);
                    }
                    xmle1.AppendChild(xmle2);
                }

                if (users_failure.Count > 0) {
                    var xmle2 = result.CreateElement("result");
                    xmle2.SetAttribute("returncode", "1301");
                    foreach (var u in users_success) {
                        var xmle3 = result.CreateElement("accId");
                        xmle3.InnerText = u.UID;
                        xmle2.AppendChild(xmle3);
                    }
                    xmle1.AppendChild(xmle2);
                }

                if (messages.Count > 0) {
                    var xmle2 = result.CreateElement("errorMsg");
                    xmle2.InnerText = String.Join(";", messages.ToArray());
                    xmle1.AppendChild(xmle2);
                }

                return result.OuterXml;
            } catch (Exception err) {
                users_failure.AddRange(users_success);
                if (users_failure.Count > 0) {
                    var xmle2 = result.CreateElement("result");
                    xmle2.SetAttribute("returncode", "1301");
                    foreach (var u in users_success) {
                        var xmle3 = result.CreateElement("accId");
                        xmle3.InnerText = u.UID;
                        xmle2.AppendChild(xmle3);
                    }
                    xmle1.AppendChild(xmle2);
                }

                var msg = result.CreateElement("errorMsg");
                msg.InnerText = err.Message;
                xmle1.AppendChild(msg);
                return result.OuterXml;
            }
            #endregion
        }
        #endregion

        #region 更新从帐号接口
        [WebMethod(Description = "更新从帐号接口")]
        public String modifyUserInfo(String user) {
            var result = new XmlDocument();
            var xdt = result.CreateXmlDeclaration("1.0", "UTF-8", null);
            var xmle1 = result.CreateElement("results");
            result.AppendChild(xdt);
            result.AppendChild(xmle1);

            var messages = new List<String>();
            var users_success = new List<KGUser>();
            var users_failure = new List<KGUser>();

            #region 解析数据
            try {
                if (user == null || String.IsNullOrEmpty(user.Trim())) {
                    var xmle2 = result.CreateElement("errorMsg");
                    xmle2.InnerText = "参数不能为空";
                    xmle1.AppendChild(xmle2);
                    return result.OuterXml;
                }

                var xml = new XmlDocument(); xml.LoadXml(user);
                var root = xml.SelectSingleNode("accounts");
                if (root == null) {
                    var xmle2 = result.CreateElement("errorMsg");
                    xmle2.InnerText = "参数解析错误，未找到accounts节点。";
                    xmle1.AppendChild(xmle2);
                    return result.OuterXml;
                }

                var accounts = root.ChildNodes;
                if (accounts.Count == 0) {
                    var xmle2 = result.CreateElement("errorMsg");
                    xmle2.InnerText = "参数解析错误，未找到account节点。";
                    xmle1.AppendChild(xmle2);
                    return result.OuterXml;
                }

                foreach (XmlNode account in accounts) {
                    var nodes = account.ChildNodes;
                    if (nodes != null && nodes.Count > 0) {
                        var utp = new KGUser();
                        try {
                            foreach (XmlNode nd in nodes) {
                                switch (nd.Name.Trim().ToLower()) {
                                    case "accid":
                                        utp.UID = nd.InnerText.Trim();
                                        break;
                                    case "userpasswordmd5":
                                        utp.PWD = nd.InnerText.Trim();
                                        break;
                                    case "name":
                                        utp.UserName = nd.InnerText.Trim();
                                        break;
                                    case "description":
                                        utp.Remark = nd.InnerText.Trim();
                                        break;
                                    case "email":
                                        utp.Email = nd.InnerText.Trim();
                                        break;
                                    case "gender":
                                        utp.Sex = nd.InnerText.Trim().Equals("女") ? 1 : 0;
                                        break;
                                    case "telephonenumber":
                                        utp.TelePhone = nd.InnerText.Trim();
                                        break;
                                    case "mobile":
                                        utp.MobilePhone = nd.InnerText.Trim();
                                        break;
                                    case "starttime":
                                        utp.StartTime = DateTime.Parse(nd.InnerText.Trim());
                                        break;
                                    case "endtime":
                                        utp.EndTime = DateTime.Parse(nd.InnerText.Trim());
                                        break;
                                    case "idcardnumber":
                                        utp.CardID = nd.InnerText.Trim();
                                        break;
                                    case "employeenumber":
                                        utp.EmployeeID = nd.InnerText.Trim();
                                        break;
                                    default:
                                        break;
                                }
                            }

                            users_success.Add(utp);
                        } catch {
                            users_failure.Add(utp);
                            messages.Add(String.Format("用户{0}解析错误", utp.UID == null ? "null" : utp.UID));
                        }
                    }
                }
            } catch (Exception err) {
                var xmle2 = result.CreateElement("errorMsg");
                xmle2.InnerText = err.Message;
                xmle1.AppendChild(xmle2);
                return result.OuterXml;
            }
            #endregion

            #region 更新数据
            try {
                if (users_success.Count == 0) {
                    if (users_failure.Count > 0) {
                        var xmle2 = result.CreateElement("result");
                        xmle2.SetAttribute("returncode", "1303");
                        foreach (var u in users_failure) {
                            var xmle3 = result.CreateElement("accId");
                            xmle3.InnerText = u.UID;
                            xmle2.AppendChild(xmle3);
                        }
                        xmle1.AppendChild(xmle2);
                    }

                    var msg = result.CreateElement("errorMsg");
                    msg.InnerText = "无效的用户信息";
                    xmle1.AppendChild(msg);
                    return result.OuterXml;
                }

                var lscs = new BLsc().GetLscs();
                if (lscs.Count == 0) {
                    var xmle2 = result.CreateElement("result");
                    xmle2.SetAttribute("returncode", "1303");
                    foreach (var u in users_failure) {
                        var xmle3 = result.CreateElement("accId");
                        xmle3.InnerText = u.UID;
                        xmle2.AppendChild(xmle3);
                    }
                    xmle1.AppendChild(xmle2);

                    var msg = result.CreateElement("errorMsg");
                    msg.InnerText = "无效的Lsc信息";
                    xmle1.AppendChild(msg);
                    return result.OuterXml;
                }

                var dic = new Dictionary<String, KGUser>();
                foreach (var lsc in lscs) {
                    var connectionString = WebUtility.CreateLscConnectionString(lsc);
                    foreach (var us in users_success) {
                        try {
                            SaveUser(connectionString, lsc.LscID, us);
                        } catch {
                            dic[us.UID] = us;
                            messages.Add(String.Format("更新用户{0}失败[{1},{2}]", us.UID, lsc.LscID, lsc.LscName));
                        }
                    }
                }

                users_success.RemoveAll(u => dic.ContainsKey(u.UID));
                users_failure.AddRange(dic.Select(u => u.Value));
                if (users_success.Count > 0) {
                    var xmle2 = result.CreateElement("result");
                    xmle2.SetAttribute("returncode", "1302");
                    foreach (var u in users_success) {
                        var xmle3 = result.CreateElement("accId");
                        xmle3.InnerText = u.UID;
                        xmle2.AppendChild(xmle3);
                    }
                    xmle1.AppendChild(xmle2);
                }

                if (users_failure.Count > 0) {
                    var xmle2 = result.CreateElement("result");
                    xmle2.SetAttribute("returncode", "1303");
                    foreach (var u in users_success) {
                        var xmle3 = result.CreateElement("accId");
                        xmle3.InnerText = u.UID;
                        xmle2.AppendChild(xmle3);
                    }
                    xmle1.AppendChild(xmle2);
                }

                if (messages.Count > 0) {
                    var xmle2 = result.CreateElement("errorMsg");
                    xmle2.InnerText = String.Join(";", messages.ToArray());
                    xmle1.AppendChild(xmle2);
                }

                return result.OuterXml;
            } catch (Exception err) {
                users_failure.AddRange(users_success);
                if (users_failure.Count > 0) {
                    var xmle2 = result.CreateElement("result");
                    xmle2.SetAttribute("returncode", "1303");
                    foreach (var u in users_success) {
                        var xmle3 = result.CreateElement("accId");
                        xmle3.InnerText = u.UID;
                        xmle2.AppendChild(xmle3);
                    }
                    xmle1.AppendChild(xmle2);
                }

                var msg = result.CreateElement("errorMsg");
                msg.InnerText = err.Message;
                xmle1.AppendChild(msg);
                return result.OuterXml;
            }
            #endregion
        }
        #endregion

        #region 删除从帐号接口
        [WebMethod(Description = "删除从帐号接口")]
        public String delUser(String user) {
            var result = new XmlDocument();
            var xdt = result.CreateXmlDeclaration("1.0", "UTF-8", null);
            var xmle1 = result.CreateElement("results");
            result.AppendChild(xdt);
            result.AppendChild(xmle1);

            var messages = new List<String>();
            var users_success = new List<String>();
            var users_failure = new List<String>();

            #region 解析数据
            try {
                if (user == null || String.IsNullOrEmpty(user.Trim())) {
                    var xmle2 = result.CreateElement("errorMsg");
                    xmle2.InnerText = "参数不能为空";
                    xmle1.AppendChild(xmle2);
                    return result.OuterXml;
                }

                var xml = new XmlDocument(); xml.LoadXml(user);
                var root = xml.SelectSingleNode("accounts");
                if (root == null) {
                    var xmle2 = result.CreateElement("errorMsg");
                    xmle2.InnerText = "参数解析错误，未找到accounts节点。";
                    xmle1.AppendChild(xmle2);
                    return result.OuterXml;
                }

                var accounts = root.ChildNodes;
                if (accounts.Count == 0) {
                    var xmle2 = result.CreateElement("errorMsg");
                    xmle2.InnerText = "参数解析错误，未找到accId节点。";
                    xmle1.AppendChild(xmle2);
                    return result.OuterXml;
                }

                foreach (XmlNode accId in accounts) {
                    switch (accId.Name.Trim().ToLower()) {
                        case "accid":
                            users_success.Add(accId.InnerText.Trim());
                            break;
                        default:
                            break;
                    }
                }
            } catch (Exception err) {
                var xmle2 = result.CreateElement("errorMsg");
                xmle2.InnerText = err.Message;
                xmle1.AppendChild(xmle2);
                return result.OuterXml;
            }
            #endregion

            #region 更新数据
            try {
                if (users_success.Count == 0) {
                    if (users_failure.Count > 0) {
                        var xmle2 = result.CreateElement("result");
                        xmle2.SetAttribute("returncode", "1305");
                        foreach (var u in users_failure) {
                            var xmle3 = result.CreateElement("accId");
                            xmle3.InnerText = u;
                            xmle2.AppendChild(xmle3);
                        }
                        xmle1.AppendChild(xmle2);
                    }

                    var msg = result.CreateElement("errorMsg");
                    msg.InnerText = "无效的用户ID";
                    xmle1.AppendChild(msg);
                    return result.OuterXml;
                }

                var lscs = new BLsc().GetLscs();
                if (lscs.Count == 0) {
                    var xmle2 = result.CreateElement("result");
                    xmle2.SetAttribute("returncode", "1305");
                    foreach (var u in users_failure) {
                        var xmle3 = result.CreateElement("accId");
                        xmle3.InnerText = u;
                        xmle2.AppendChild(xmle3);
                    }
                    xmle1.AppendChild(xmle2);

                    var msg = result.CreateElement("errorMsg");
                    msg.InnerText = "无效的Lsc信息";
                    xmle1.AppendChild(msg);
                    return result.OuterXml;
                }

                var dic = new Dictionary<String, String>();
                foreach (var lsc in lscs) {
                    var connectionString = WebUtility.CreateLscConnectionString(lsc);
                    foreach (var us in users_success) {
                        try {
                            DeleteUser(connectionString, lsc.LscID, us);
                        } catch {
                            dic[us] = us;
                            messages.Add(String.Format("删除用户{0}失败[{1},{2}]", us, lsc.LscID, lsc.LscName));
                        }
                    }
                }

                users_success.RemoveAll(u => dic.ContainsKey(u));
                users_failure.AddRange(dic.Select(u => u.Value));
                if (users_success.Count > 0) {
                    var xmle2 = result.CreateElement("result");
                    xmle2.SetAttribute("returncode", "1304");
                    foreach (var u in users_success) {
                        var xmle3 = result.CreateElement("accId");
                        xmle3.InnerText = u;
                        xmle2.AppendChild(xmle3);
                    }
                    xmle1.AppendChild(xmle2);
                }

                if (users_failure.Count > 0) {
                    var xmle2 = result.CreateElement("result");
                    xmle2.SetAttribute("returncode", "1305");
                    foreach (var u in users_success) {
                        var xmle3 = result.CreateElement("accId");
                        xmle3.InnerText = u;
                        xmle2.AppendChild(xmle3);
                    }
                    xmle1.AppendChild(xmle2);
                }

                if (messages.Count > 0) {
                    var xmle2 = result.CreateElement("errorMsg");
                    xmle2.InnerText = String.Join(";", messages.ToArray());
                    xmle1.AppendChild(xmle2);
                }

                return result.OuterXml;
            } catch (Exception err) {
                users_failure.AddRange(users_success);
                if (users_failure.Count > 0) {
                    var xmle2 = result.CreateElement("result");
                    xmle2.SetAttribute("returncode", "1305");
                    foreach (var u in users_success) {
                        var xmle3 = result.CreateElement("accId");
                        xmle3.InnerText = u;
                        xmle2.AppendChild(xmle3);
                    }
                    xmle1.AppendChild(xmle2);
                }

                var msg = result.CreateElement("errorMsg");
                msg.InnerText = err.Message;
                xmle1.AppendChild(msg);
                return result.OuterXml;
            }
            #endregion
        }
        #endregion

        #region 查询从帐号接口
        [WebMethod(Description = "查询从帐号接口")]
        public String findUser(String uId) {
            try {
                var result = new XmlDocument();
                var xdt = result.CreateXmlDeclaration("1.0", "UTF-8", null);
                var xmle1 = result.CreateElement("accounts");
                result.AppendChild(xdt);
                result.AppendChild(xmle1);

                var users = GetUsers(uId, 1, 1);
                foreach (var u in users) {
                    var xmlel2 = result.CreateElement("account");
                    xmle1.AppendChild(xmlel2);

                    var accId = result.CreateElement("accId");
                    accId.InnerText = u.UID;
                    xmlel2.AppendChild(accId);

                    var userPasswordMD5 = result.CreateElement("userPasswordMD5");
                    userPasswordMD5.InnerText = u.PWD;
                    xmlel2.AppendChild(userPasswordMD5);

                    var userPasswordSHA1 = result.CreateElement("userPasswordSHA1");
                    userPasswordSHA1.InnerText = String.Empty;
                    xmlel2.AppendChild(userPasswordSHA1);

                    var name = result.CreateElement("name");
                    name.InnerText = u.UserName;
                    xmlel2.AppendChild(name);

                    var sn = result.CreateElement("sn");
                    sn.InnerText = String.Empty;
                    xmlel2.AppendChild(sn);

                    var description = result.CreateElement("description");
                    description.InnerText = u.Remark;
                    xmlel2.AppendChild(description);

                    var email = result.CreateElement("email");
                    email.InnerText = u.Email;
                    xmlel2.AppendChild(email);

                    var gender = result.CreateElement("gender");
                    gender.InnerText = u.Sex == 1 ? "女" : "男";
                    xmlel2.AppendChild(gender);

                    var telephoneNumber = result.CreateElement("telephoneNumber");
                    telephoneNumber.InnerText = u.TelePhone;
                    xmlel2.AppendChild(telephoneNumber);

                    var mobile = result.CreateElement("mobile");
                    mobile.InnerText = u.MobilePhone;
                    xmlel2.AppendChild(mobile);

                    var startTime = result.CreateElement("startTime");
                    startTime.InnerText = u.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    xmlel2.AppendChild(startTime);

                    var endTime = result.CreateElement("endTime");
                    endTime.InnerText = u.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    xmlel2.AppendChild(endTime);

                    var idCardNumber = result.CreateElement("idCardNumber");
                    idCardNumber.InnerText = u.CardID;
                    xmlel2.AppendChild(idCardNumber);

                    var employeeNumber = result.CreateElement("employeeNumber");
                    employeeNumber.InnerText = u.EmployeeID;
                    xmlel2.AppendChild(employeeNumber);

                    var o = result.CreateElement("o");
                    o.InnerText = String.Empty;
                    xmlel2.AppendChild(o);

                    var employeeType = result.CreateElement("employeeType");
                    employeeType.InnerText = String.Empty;
                    xmlel2.AppendChild(employeeType);

                    var supporterCorpName = result.CreateElement("supporterCorpName");
                    supporterCorpName.InnerText = String.Empty;
                    xmlel2.AppendChild(supporterCorpName);
                }

                return result.OuterXml;
            } catch {
                var result = new XmlDocument();
                var xdt = result.CreateXmlDeclaration("1.0", "UTF-8", null);
                var xmle1 = result.CreateElement("accounts");
                result.AppendChild(xdt);
                result.AppendChild(xmle1);
                return result.OuterXml;
            }
        }
        #endregion

        #region 查询所有从帐号接口
        [WebMethod(Description = "查询所有从帐号接口")]
        public String queryUsers() {
            try {
                var result = new XmlDocument();
                var xdt = result.CreateXmlDeclaration("1.0", "UTF-8", null);
                var xmle1 = result.CreateElement("accounts");
                result.AppendChild(xdt);
                result.AppendChild(xmle1);

                var users = GetUsers(null, 1, Int32.MaxValue);
                foreach (var u in users) {
                    var xmlel2 = result.CreateElement("account");
                    xmle1.AppendChild(xmlel2);

                    var accId = result.CreateElement("accId");
                    accId.InnerText = u.UID;
                    xmlel2.AppendChild(accId);

                    var userPasswordMD5 = result.CreateElement("userPasswordMD5");
                    userPasswordMD5.InnerText = u.PWD;
                    xmlel2.AppendChild(userPasswordMD5);

                    var userPasswordSHA1 = result.CreateElement("userPasswordSHA1");
                    userPasswordSHA1.InnerText = String.Empty;
                    xmlel2.AppendChild(userPasswordSHA1);

                    var name = result.CreateElement("name");
                    name.InnerText = u.UserName;
                    xmlel2.AppendChild(name);

                    var sn = result.CreateElement("sn");
                    sn.InnerText = String.Empty;
                    xmlel2.AppendChild(sn);

                    var description = result.CreateElement("description");
                    description.InnerText = u.Remark;
                    xmlel2.AppendChild(description);

                    var email = result.CreateElement("email");
                    email.InnerText = u.Email;
                    xmlel2.AppendChild(email);

                    var gender = result.CreateElement("gender");
                    gender.InnerText = u.Sex == 1 ? "女" : "男";
                    xmlel2.AppendChild(gender);

                    var telephoneNumber = result.CreateElement("telephoneNumber");
                    telephoneNumber.InnerText = u.TelePhone;
                    xmlel2.AppendChild(telephoneNumber);

                    var mobile = result.CreateElement("mobile");
                    mobile.InnerText = u.MobilePhone;
                    xmlel2.AppendChild(mobile);

                    var startTime = result.CreateElement("startTime");
                    startTime.InnerText = u.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    xmlel2.AppendChild(startTime);

                    var endTime = result.CreateElement("endTime");
                    endTime.InnerText = u.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    xmlel2.AppendChild(endTime);

                    var idCardNumber = result.CreateElement("idCardNumber");
                    idCardNumber.InnerText = u.CardID;
                    xmlel2.AppendChild(idCardNumber);

                    var employeeNumber = result.CreateElement("employeeNumber");
                    employeeNumber.InnerText = u.EmployeeID;
                    xmlel2.AppendChild(employeeNumber);

                    var o = result.CreateElement("o");
                    o.InnerText = String.Empty;
                    xmlel2.AppendChild(o);

                    var employeeType = result.CreateElement("employeeType");
                    employeeType.InnerText = String.Empty;
                    xmlel2.AppendChild(employeeType);

                    var supporterCorpName = result.CreateElement("supporterCorpName");
                    supporterCorpName.InnerText = String.Empty;
                    xmlel2.AppendChild(supporterCorpName);
                }

                return result.OuterXml;
            } catch {
                var result = new XmlDocument();
                var xdt = result.CreateXmlDeclaration("1.0", "UTF-8", null);
                var xmle1 = result.CreateElement("accounts");
                result.AppendChild(xdt);
                result.AppendChild(xmle1);
                return result.OuterXml;
            }
        }
        #endregion

        #region 查询从帐号总数接口
        [WebMethod(Description = "查询从帐号总数接口")]
        public String getUserAmount() {
            try {
                return GetUsersCnt().ToString();
            } catch {
                return "-1";
            }
        }
        #endregion

        #region 分页查询从帐号接口
        [WebMethod(Description = "分页查询从帐号接口")]
        public String queryUsersByPage(String pageSize, String pageNum) {
            try {
                var startIndex = (Int32.Parse(pageNum) - 1) * Int32.Parse(pageSize) + 1;
                var endIndex = startIndex + Int32.Parse(pageSize) - 1;

                var result = new XmlDocument();
                var xdt = result.CreateXmlDeclaration("1.0", "UTF-8", null);
                var xmle1 = result.CreateElement("accounts");
                result.AppendChild(xdt);
                result.AppendChild(xmle1);

                var users = GetUsers(null, startIndex, endIndex);
                foreach (var u in users) {
                    var xmlel2 = result.CreateElement("account");
                    xmle1.AppendChild(xmlel2);

                    var accId = result.CreateElement("accId");
                    accId.InnerText = u.UID;
                    xmlel2.AppendChild(accId);

                    var userPasswordMD5 = result.CreateElement("userPasswordMD5");
                    userPasswordMD5.InnerText = u.PWD;
                    xmlel2.AppendChild(userPasswordMD5);

                    var userPasswordSHA1 = result.CreateElement("userPasswordSHA1");
                    userPasswordSHA1.InnerText = String.Empty;
                    xmlel2.AppendChild(userPasswordSHA1);

                    var name = result.CreateElement("name");
                    name.InnerText = u.UserName;
                    xmlel2.AppendChild(name);

                    var sn = result.CreateElement("sn");
                    sn.InnerText = String.Empty;
                    xmlel2.AppendChild(sn);

                    var description = result.CreateElement("description");
                    description.InnerText = u.Remark;
                    xmlel2.AppendChild(description);

                    var email = result.CreateElement("email");
                    email.InnerText = u.Email;
                    xmlel2.AppendChild(email);

                    var gender = result.CreateElement("gender");
                    gender.InnerText = u.Sex == 1 ? "女" : "男";
                    xmlel2.AppendChild(gender);

                    var telephoneNumber = result.CreateElement("telephoneNumber");
                    telephoneNumber.InnerText = u.TelePhone;
                    xmlel2.AppendChild(telephoneNumber);

                    var mobile = result.CreateElement("mobile");
                    mobile.InnerText = u.MobilePhone;
                    xmlel2.AppendChild(mobile);

                    var startTime = result.CreateElement("startTime");
                    startTime.InnerText = u.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                    xmlel2.AppendChild(startTime);

                    var endTime = result.CreateElement("endTime");
                    endTime.InnerText = u.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    xmlel2.AppendChild(endTime);

                    var idCardNumber = result.CreateElement("idCardNumber");
                    idCardNumber.InnerText = u.CardID;
                    xmlel2.AppendChild(idCardNumber);

                    var employeeNumber = result.CreateElement("employeeNumber");
                    employeeNumber.InnerText = u.EmployeeID;
                    xmlel2.AppendChild(employeeNumber);

                    var o = result.CreateElement("o");
                    o.InnerText = String.Empty;
                    xmlel2.AppendChild(o);

                    var employeeType = result.CreateElement("employeeType");
                    employeeType.InnerText = String.Empty;
                    xmlel2.AppendChild(employeeType);

                    var supporterCorpName = result.CreateElement("supporterCorpName");
                    supporterCorpName.InnerText = String.Empty;
                    xmlel2.AppendChild(supporterCorpName);
                }

                return result.OuterXml;
            } catch {
                var result = new XmlDocument();
                var xdt = result.CreateXmlDeclaration("1.0", "UTF-8", null);
                var xmle1 = result.CreateElement("accounts");
                result.AppendChild(xdt);
                result.AppendChild(xmle1);
                return result.OuterXml;
            }
        }
        #endregion

        #region 私有方法
        /// <summary>
        /// Get Users.
        /// </summary>
        private List<KGUser> GetUsers(String uId, Int32 startIndex, Int32 endIndex) {
            var sql = @";WITH Temp AS
                        (
	                        SELECT ROW_NUMBER() OVER(PARTITION BY [UID] ORDER BY [UID]) AS [ID],* FROM [dbo].[TU_User] 
	                        WHERE (@UID IS NULL OR [UID] = @UID)
                        ),
                        PG AS
                        (
	                        SELECT ROW_NUMBER() OVER(ORDER BY [UID]) AS [NO],* FROM Temp WHERE [ID] = 1
                        )
                        SELECT [UserID],[UserName],[UID],[PWD],[EmpNO],[OpLevel],[Sex],[TelePhone],[MobilePhone],[Email],
                        [Address],[PostalCode],[Remark],[OnlineATime],[LimitTime] FROM PG
                        WHERE [NO] BETWEEN @StartIndex AND @EndIndex;";

            var users = new List<KGUser>();
            SqlParameter[] parms = { new SqlParameter("@UID", SqlDbType.VarChar,20),
                                     new SqlParameter("@StartIndex", SqlDbType.Int),
                                     new SqlParameter("@EndIndex", SqlDbType.Int) };

            if (uId != null) { parms[0].Value = uId; } else { parms[0].Value = DBNull.Value; }
            parms[1].Value = startIndex; parms[2].Value = endIndex;
            using (var rdr = SqlHelper.ExecuteReader(SqlHelper.ConnectionStringLocalTransaction, CommandType.Text, sql, parms)) {
                while (rdr.Read()) {
                    var user = new KGUser();
                    user.UserID = rdr["UserID"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["UserID"]);
                    user.UserName = rdr["UserName"] == DBNull.Value ? String.Empty : Convert.ToString(rdr["UserName"]);
                    user.UID = rdr["UID"] == DBNull.Value ? String.Empty : Convert.ToString(rdr["UID"]);
                    user.PWD = rdr["PWD"] == DBNull.Value ? String.Empty : Convert.ToString(rdr["PWD"]);
                    user.EmployeeID = rdr["EmpNO"] == DBNull.Value ? String.Empty : Convert.ToString(rdr["EmpNO"]);
                    user.CardID = String.Empty;
                    user.Sex = rdr["Sex"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["Sex"]);
                    user.TelePhone = rdr["TelePhone"] == DBNull.Value ? String.Empty : Convert.ToString(rdr["TelePhone"]);
                    user.MobilePhone = rdr["MobilePhone"] == DBNull.Value ? String.Empty : Convert.ToString(rdr["MobilePhone"]);
                    user.Email = rdr["Email"] == DBNull.Value ? String.Empty : Convert.ToString(rdr["Email"]);
                    user.Remark = rdr["Remark"] == DBNull.Value ? String.Empty : Convert.ToString(rdr["Remark"]);
                    user.StartTime = rdr["OnlineATime"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(rdr["OnlineATime"]);
                    user.EndTime = rdr["LimitTime"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(rdr["LimitTime"]);

                    users.Add(user);
                }
            }

            return users;
        }

        /// <summary>
        /// Get Users Count.
        /// </summary>
        private Int32 GetUsersCnt() {
            var sql = @";WITH Temp AS
                        (
	                        SELECT ROW_NUMBER() OVER(PARTITION BY [UID] ORDER BY [UID]) AS [ID],* FROM [dbo].[TU_User]
                        )
                        SELECT COUNT(1) AS Cnt FROM Temp WHERE [ID] = 1;";

            using (var conn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                var cnt = SqlHelper.ExecuteScalar(conn, CommandType.Text, sql, null);
                return Convert.ToInt32(cnt);
            }
        }

        /// <summary>
        /// Save User By Interface.
        /// </summary>
        private void SaveUser(string connectionString, Int32 lscId, KGUser user) {
            SqlParameter[] parms = { new SqlParameter("@UserID", SqlDbType.Int),
                                     new SqlParameter("@UserName", SqlDbType.VarChar,40),
                                     new SqlParameter("@Sex", SqlDbType.Int),
                                     new SqlParameter("@CardID", SqlDbType.VarChar,20),
                                     new SqlParameter("@Email", SqlDbType.VarChar,250),
                                     new SqlParameter("@TelePhone", SqlDbType.VarChar,20),
                                     new SqlParameter("@MobilePhone", SqlDbType.VarChar,20),
                                     new SqlParameter("@EmpNO", SqlDbType.VarChar,20),
                                     new SqlParameter("@UID", SqlDbType.VarChar,20),
                                     new SqlParameter("@PWD", SqlDbType.VarChar,20),
                                     new SqlParameter("@StartTime", SqlDbType.DateTime),
                                     new SqlParameter("@EndTime", SqlDbType.DateTime),
                                     new SqlParameter("@Remark", SqlDbType.VarChar,250) };

            parms[0].Value = 0;
            parms[1].Value = user.UserName;
            parms[2].Value = user.Sex;
            parms[3].Value = user.CardID;
            parms[4].Value = user.Email;
            parms[5].Value = user.TelePhone;
            parms[6].Value = user.MobilePhone;
            parms[7].Value = user.EmployeeID;
            parms[8].Value = user.UID;
            parms[9].Value = user.PWD;
            parms[10].Value = user.StartTime;
            parms[11].Value = user.EndTime;
            parms[12].Value = user.Remark;

            var lsc_sql = @"IF EXISTS(SELECT 1 FROM [dbo].[TU_User] WHERE [UID] = @UID)
                            BEGIN
	                            UPDATE [dbo].[TU_User] SET [UserName] = @UserName,[EmpNO] = @EmpNO,[Sex] = @Sex,[PWD] = @PWD,[OpLevel] = 3,[TelePhone] = @TelePhone,[MobilePhone] = @MobilePhone,[Email] = @Email,[Remark] = @Remark,[OnlineATime] = @StartTime,[LimitTime] = @EndTime,[Enabled] = 1 
	                            WHERE [UID] = @UID;
                            END
                            ELSE
                            BEGIN
	                            INSERT INTO [dbo].[TU_User]([GroupID],[Enabled],[UID],[PWD],[UserName],[EmpNO],[OpLevel],[Sex],[DeptID],[DutyID],[TelePhone],[MobilePhone],[Email],[Address],[PostalCode],[Remark],[OnlineATime],[LimitTime])
	                            VALUES(-1,1,@UID,@PWD,@UserName,@EmpNO,3,@Sex,1,1,@TelePhone,@MobilePhone,@Email,'','',@Remark,@StartTime,@EndTime);	
                            END
                            SELECT [UserID],[GroupID],[Enabled],[UID],[PWD],[UserName],[EmpNO],[OpLevel],[Sex],[DeptID],[DutyID],[TelePhone],[MobilePhone],[Email],[Address],[PostalCode],[Remark],[OnlineATime],[SendMSG],[SMSLevel],[SMSFilter],[LimitTime],[VoiceLevel],[VoiceFilter],[VoiceType],[SendVoice],[EOMSUserName],[EOMSUserPWD],[IsAutoTaskObj],[LastTaskUserID],[AlarmSoundFiterItem],[AlarmStaticFiterItem],[ActiveValuesFiterItem] FROM [dbo].[TU_User]
                            WHERE [UID] = @UID;";
            var csc_sql = @"DELETE FROM [dbo].[TU_User] WHERE [LscID] = @LscID AND [UserID] = @UserID;INSERT INTO [dbo].[TU_User]([LscID],[UserID],[GroupID],[Enabled],[UID],[PWD],[UserName],[EmpNO],[OpLevel],[Sex],[DeptID],[DutyID],[TelePhone],[MobilePhone],[Email],[Address],[PostalCode],[Remark],[OnlineATime],[SendMSG],[SMSLevel],[SMSFilter],[LimitTime],[VoiceLevel],[VoiceFilter],[VoiceType],[SendVoice],[EOMSUserName],[EOMSUserPWD],[IsAutoTaskObj],[LastTaskUserID],[AlarmSoundFiterItem],[AlarmStaticFiterItem],[ActiveValuesFiterItem]) VALUES(@LscID,@UserID,@GroupID,@Enabled,@UID,@PWD,@UserName,@EmpNO,@OpLevel,@Sex,@DeptID,@DutyID,@TelePhone,@MobilePhone,@Email,@Address,@PostalCode,@Remark,@OnlineATime,@SendMSG,@SMSLevel,@SMSFilter,@LimitTime,@VoiceLevel,@VoiceFilter,@VoiceType,@SendVoice,@EOMSUserName,@EOMSUserPWD,@IsAutoTaskObj,@LastTaskUserID,@AlarmSoundFiterItem,@AlarmStaticFiterItem,@ActiveValuesFiterItem);";

            SqlHelper.TestConnection(connectionString);
            using (var lconn = new SqlConnection(connectionString)) {
                lconn.Open();
                var ltrans = lconn.BeginTransaction(IsolationLevel.ReadCommitted);
                try {
                    using (var rdr = SqlHelper.ExecuteReader(ltrans, CommandType.Text, lsc_sql, parms)) {
                        if (rdr.Read()) {
                            #region csc
                            using (var cconn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                                cconn.Open();
                                var ctrans = cconn.BeginTransaction(IsolationLevel.ReadCommitted);
                                try {
                                    #region paramters
                                    parms = new SqlParameter[] {    new SqlParameter("@LscID", SqlDbType.Int),
                                                                    new SqlParameter("@UserID", SqlDbType.Int),
                                                                    new SqlParameter("@GroupID", SqlDbType.Int),
                                                                    new SqlParameter("@Enabled", SqlDbType.Bit),
                                                                    new SqlParameter("@UID", SqlDbType.VarChar,20),
                                                                    new SqlParameter("@PWD", SqlDbType.VarChar,20),
                                                                    new SqlParameter("@UserName", SqlDbType.VarChar,40),
                                                                    new SqlParameter("@EmpNO", SqlDbType.VarChar,20),
                                                                    new SqlParameter("@OpLevel", SqlDbType.Int),
                                                                    new SqlParameter("@Sex", SqlDbType.Int),
                                                                    new SqlParameter("@DeptID", SqlDbType.Int),
                                                                    new SqlParameter("@DutyID", SqlDbType.Int),
                                                                    new SqlParameter("@TelePhone", SqlDbType.VarChar,20),
                                                                    new SqlParameter("@MobilePhone", SqlDbType.VarChar,20),
                                                                    new SqlParameter("@Email", SqlDbType.VarChar,250),
                                                                    new SqlParameter("@Address", SqlDbType.VarChar,250),
                                                                    new SqlParameter("@PostalCode", SqlDbType.VarChar,6),
                                                                    new SqlParameter("@Remark", SqlDbType.VarChar,250),
                                                                    new SqlParameter("@OnlineATime", SqlDbType.DateTime),
                                                                    new SqlParameter("@SendMSG", SqlDbType.Bit),
                                                                    new SqlParameter("@SMSLevel", SqlDbType.Binary,4),
                                                                    new SqlParameter("@SMSFilter", SqlDbType.VarChar,100),
                                                                    new SqlParameter("@LimitTime", SqlDbType.DateTime),
                                                                    new SqlParameter("@VoiceLevel", SqlDbType.Binary,4),
                                                                    new SqlParameter("@VoiceFilter", SqlDbType.VarChar,100),
                                                                    new SqlParameter("@VoiceType", SqlDbType.Int),
                                                                    new SqlParameter("@SendVoice", SqlDbType.Bit),
                                                                    new SqlParameter("@EOMSUserName", SqlDbType.VarChar,20),
                                                                    new SqlParameter("@EOMSUserPWD", SqlDbType.VarChar,20),
                                                                    new SqlParameter("@IsAutoTaskObj", SqlDbType.Bit),
                                                                    new SqlParameter("@LastTaskUserID", SqlDbType.Int),
                                                                    new SqlParameter("@AlarmSoundFiterItem", SqlDbType.Image),
                                                                    new SqlParameter("@AlarmStaticFiterItem", SqlDbType.Image),
                                                                    new SqlParameter("@ActiveValuesFiterItem", SqlDbType.Image) };

                                    parms[0].Value = lscId;
                                    parms[1].Value = rdr["UserID"];
                                    parms[2].Value = rdr["GroupID"];
                                    parms[3].Value = rdr["Enabled"];
                                    parms[4].Value = rdr["UID"];
                                    parms[5].Value = rdr["PWD"];
                                    parms[6].Value = rdr["UserName"];
                                    parms[7].Value = rdr["EmpNO"];
                                    parms[8].Value = rdr["OpLevel"];
                                    parms[9].Value = rdr["Sex"];
                                    parms[10].Value = rdr["DeptID"];
                                    parms[11].Value = rdr["DutyID"];
                                    parms[12].Value = rdr["TelePhone"];
                                    parms[13].Value = rdr["MobilePhone"];
                                    parms[14].Value = rdr["Email"];
                                    parms[15].Value = rdr["Address"];
                                    parms[16].Value = rdr["PostalCode"];
                                    parms[17].Value = rdr["Remark"];
                                    parms[18].Value = rdr["OnlineATime"];
                                    parms[19].Value = rdr["SendMSG"];
                                    parms[20].Value = rdr["SMSLevel"];
                                    parms[21].Value = rdr["SMSFilter"];
                                    parms[22].Value = rdr["LimitTime"];
                                    parms[23].Value = rdr["VoiceLevel"];
                                    parms[24].Value = rdr["VoiceFilter"];
                                    parms[25].Value = rdr["VoiceType"];
                                    parms[26].Value = rdr["SendVoice"];
                                    parms[27].Value = rdr["EOMSUserName"];
                                    parms[28].Value = rdr["EOMSUserPWD"];
                                    parms[29].Value = rdr["IsAutoTaskObj"];
                                    parms[30].Value = rdr["LastTaskUserID"];
                                    parms[31].Value = rdr["AlarmSoundFiterItem"];
                                    parms[32].Value = rdr["AlarmStaticFiterItem"];
                                    parms[33].Value = rdr["ActiveValuesFiterItem"];
                                    #endregion

                                    SqlHelper.ExecuteNonQuery(ctrans, CommandType.Text, csc_sql, parms);
                                    ctrans.Commit();
                                } catch {
                                    ctrans.Rollback();
                                    throw;
                                }
                            }
                            #endregion
                        }
                    }
                    ltrans.Commit();
                } catch {
                    ltrans.Rollback();
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete User By Interface.
        /// </summary>
        private void DeleteUser(string connectionString, Int32 lscId, String uId) {
            SqlParameter[] parms = { new SqlParameter("@UID", SqlDbType.VarChar, 20) };
            parms[0].Value = uId;

            var lsc_sql = @"DELETE FROM [dbo].[TU_User] WHERE [UID] = @UID;";
            var csc_sql = @"DELETE FROM [dbo].[TU_User] WHERE [LscID] = @LscID AND [UID] = @UID;";

            SqlHelper.TestConnection(connectionString);
            using (var lconn = new SqlConnection(connectionString)) {
                lconn.Open();
                var ltrans = lconn.BeginTransaction(IsolationLevel.ReadCommitted);
                try {
                    SqlHelper.ExecuteNonQuery(ltrans, CommandType.Text, lsc_sql, parms);

                    #region csc
                    using (var cconn = new SqlConnection(SqlHelper.ConnectionStringLocalTransaction)) {
                        cconn.Open();
                        var ctrans = cconn.BeginTransaction(IsolationLevel.ReadCommitted);
                        try {
                            parms = new SqlParameter[] { new SqlParameter("@LscID", SqlDbType.Int), new SqlParameter("@UID", SqlDbType.VarChar, 20) };
                            parms[0].Value = lscId; parms[1].Value = uId;

                            SqlHelper.ExecuteNonQuery(ctrans, CommandType.Text, csc_sql, parms);
                            ctrans.Commit();
                        } catch {
                            ctrans.Rollback();
                            throw;
                        }
                    }
                    #endregion
                    ltrans.Commit();
                } catch {
                    ltrans.Rollback();
                    throw;
                }
            }
        }
        #endregion
    }

    #region 内部类
    /// <summary>
    /// KG User
    /// </summary>
    public class KGUser {
        /// <summary>
        /// UserID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// UserName
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Sex
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// CardID
        /// </summary>
        public string CardID { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// TelePhone
        /// </summary>
        public string TelePhone { get; set; }

        /// <summary>
        /// MobilePhone
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// EmployeeID
        /// </summary>
        public string EmployeeID { get; set; }

        /// <summary>
        /// UID
        /// </summary>
        public string UID { get; set; }

        /// <summary>
        /// PWD
        /// </summary>
        public string PWD { get; set; }

        /// <summary>
        /// StartTime
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// EndTime
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Remark
        /// </summary>
        public string Remark { get; set; }
    }
    #endregion
}
