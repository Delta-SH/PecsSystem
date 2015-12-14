<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder1" runat="server" Mode="Script" />
    <ext:ResourcePlaceHolder ID="ResourcePlaceHolder2" runat="server" Mode="Style" />
    <meta http-equiv="Content-Type" content="text/html; charset=gb2312" />
    <meta name="keywords" content="动力环境监控系统,动力监控,环境监控,视频监控,机房监控,节能,视讯,自动化产品" />
    <meta name="description" content="动力环境监控中心系统" />
    <meta name="title" content="动力环境监控中心系统" />
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link rel="icon" type="image/x-icon" href="favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="favicon.ico" />
    <style type="text/css">
    #ws-tips {
        background: none repeat scroll 0 0 #FFFFFF;
        border-color: #E3E3E3 #ECECEC #E0E0E0 #E3E3E3;
        border-image: none;
        border-style: solid;
        border-width: 1px;
        box-shadow: 1px 2px 1px rgba(0, 0, 0, 0.07);
        position: absolute;
        right: 15px;
        top: 15px;
        white-space: nowrap;
        height: 160px;
        width: 140px;
        z-index: 1;
    }
    #ws-tips img {
        position: absolute;
        top: 20px;
        left:20px;
        vertical-align: middle;
    }
    .ws-info {
        position: absolute;
        left: 10px;
        top: 125px;
        height: 30px;
        width: 120px;
        text-align: center;
    }
    .ws-label {
        color: #848585;
        font-family: '宋体';
        font-size: 12px;
    }
    .ws-label a 
    {
        color: #848585;
        text-decoration:none;
    }
    .ws-label a:hover 
    {
        color: #626262;
    }
    #ws-btn {
        background: url("Resources/images/Tips/T-Close.png") no-repeat scroll 0 0;
        height: 9px;
        position: absolute;
        right: 8px;
        top: 8px;
        width: 9px;
    }
    #ws-btn:focus {
        outline: 0 none;
    }
    </style>
    <script type="text/javascript">
        //<![CDATA[
        Ext.onReady(function() {
            Ext.select(".L-jsTips").hide();
            Ext.select(".L-ValidateCode").set({ src: String.format("CreateCode.ashx?_ct={0}", new Date().getTime()) }, false);
            Ext.select(".L-ValidateCode").on("click", function() { Ext.fly(this).set({ src: String.format("CreateCode.ashx?_ct={0}", new Date().getTime()) }, false) });
            Ext.select(".L-TextBoxLoginCss").on("focus", function() { Ext.fly(this).toggleClass("L-TextBoxLoginCss-On"); });
            Ext.select(".L-TextBoxLoginCss").on("blur", function() { Ext.fly(this).toggleClass("L-TextBoxLoginCss-On"); });
            Ext.select(".L-BtnLoginCss").on("mouseover", function() { Ext.fly(this).toggleClass("L-BtnLoginCss-On"); });
            Ext.select(".L-BtnLoginCss").on("mouseout", function() { Ext.fly(this).toggleClass("L-BtnLoginCss-On"); });
            Ext.select(".L-BtnLoginCss").on("mousedown", function() { Ext.fly(this).toggleClass("L-BtnLoginCss-Active"); });
            Ext.select(".L-BtnLoginCss").on("mouseup", function() { Ext.fly(this).toggleClass("L-BtnLoginCss-Active"); });

            Ext.fly("ws-qrcode").set({ src: String.format("CreateCode.ashx?QRCode=1&_ct={0}", new Date().getTime()) }, false);
            Ext.fly("ws-btn").on("click", function() { Ext.fly("ws-tips").remove(); });
        });
        //]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <ext:ResourceManager ID="ResourceManager1" CleanResourceUrl="false" runat="server" DirectMethodNamespace="X" IDMode="Explicit" />
    <div class="L-page">
        <div class="L-header">
            <div class="L-jsTips">
                <span class="words"><%= GetLocalResourceObject("Span.JSTips.InnerHtml") %></span>
            </div>
            <div id="FailureDiv" runat="server" class="L-ErrorTips" visible="false">
                <span id="FailureText" runat="server" class="words"></span>
            </div>
        </div>
        <div class="L-content">
            <div class="L-logintable" id="divTheme">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="bottom">
                            <div class="<%= GetLocalResourceObject("Div.LoginLogo.CssClass")%>">
                            </div>
                        </td>
                        <td rowspan="2" valign="bottom">
                            <div class="L-login">
                                <div class="L-loginHD">
                                    <em><%= GetLocalResourceObject("Em.LoginTitle.InnerHtml")%></em>
                                </div>
                                <div style="margin: 20px;">
                                    <div class="L-loginItem">
                                        <em><%= GetLocalResourceObject("Em.LoginUser.InnerHtml")%></em>
                                        <div>
                                            <asp:TextBox ID="UserName" runat="server" MaxLength="20" SkinID="TxtLoginSkin"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="UserNameRequired" ControlToValidate="UserName"
                                                ValidationGroup="Login" ErrorMessage="<img src='/Resources/images/Tips/T-Exclamation.png' alt='Required Field.'/>" meta:resourcekey="RequiredItem"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="L-loginItem">
                                        <em><%= GetLocalResourceObject("Em.LoginPassword.InnerHtml")%></em>
                                        <div>
                                            <asp:TextBox ID="Password" runat="server" MaxLength="20" TextMode="Password"
                                                SkinID="TxtLoginSkin"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="PasswordRequired" ControlToValidate="Password"
                                                ValidationGroup="Login" ErrorMessage="<img src='/Resources/images/Tips/T-Exclamation.png' alt='Required Field.'/>" meta:resourcekey="RequiredItem"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="L-loginItem">
                                        <em><%= GetLocalResourceObject("Em.LoginCode.InnerHtml")%></em>
                                        <div>
                                            <asp:TextBox ID="Verification" runat="server" MaxLength="5" SkinID="TxtLoginSkin" Style="width: 85px;"></asp:TextBox>
                                            <asp:RequiredFieldValidator runat="server" ID="VerificationRequired" ControlToValidate="Verification"
                                                ValidationGroup="Login" ErrorMessage="<img src='/Resources/images/Tips/T-Exclamation.png' alt='Required Field.'/>" meta:resourcekey="RequiredItem">
                                            </asp:RequiredFieldValidator>
                                            <img class="L-ValidateCode" src="CreateCode.ashx" alt="<%= GetLocalResourceObject("Img.Code.Alt") %>" title="<%= GetLocalResourceObject("Img.Code.Title") %>" />
                                        </div>
                                    </div>
                                    <div class="L-loginItem">
                                        <em><%= GetLocalResourceObject("Em.LoginLanguage.InnerHtml")%></em>
                                        <div>
                                            <asp:DropDownList ID="UICultureList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="UICultureList_SelectedIndexChanged" SkinID="ListLanSkin">
                                                <asp:ListItem Value="zh-CN" Text="中文" Selected="True" />
                                                <asp:ListItem Value="en-US" Text="English" />
                                            </asp:DropDownList>
                                            <ext:LinkButton ID="register" runat="server" meta:resourcekey="Register">
                                                <DirectEvents>
                                                    <Click OnEvent="RegisterBtn_Click">
                                                        <EventMask ShowMask="true" Target="Page" />
                                                    </Click> 
                                                </DirectEvents>
                                            </ext:LinkButton>
                                        </div>
                                    </div>
                                    <div class="L-loginItem">
                                        <em>&nbsp;</em>
                                        <asp:Button ID="LoginBtn" runat="server" meta:resourcekey="BtnLogin" ValidationGroup="Login" SkinID="BtnLoginSkin" OnClick="LoginBtn_Click" />
                                    </div>
                                    <div class="L-loginItem">
                                        <em>&nbsp;</em>
                                        <div class="L-loginVersion">
                                            <div>
                                                <%=String.Format("{0}{1}", GetLocalResourceObject("Div.Version.InnerHtml"), System.Web.Configuration.WebConfigurationManager.AppSettings["CurrentVersion"])%>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td valign="bottom">
                            <div class="L-theme">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" valign="bottom">
                            <div class="L-footer">
                                <span class="words"><%= String.Format(GetLocalResourceObject("Span.Copyright.InnerHtml").ToString(), DateTime.Today.Year)%></span>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="ws-tips">
            <a id="ws-btn" href="javascript:void(0);"></a>
            <img id="ws-qrcode" src="CreateCode.ashx?QRCode=1" alt=""/>
            <div class="ws-info">
                <div class="ws-label">
                    <a href="Download.aspx?QRCode=1" target="_blank">扫描二维码<br/>下载手机客户端</a>
                </div>
            </div>
        </div>
    </div>
    <ext:Window ID="RegisterWnd" runat="server" Title="产品注册授权" Icon="Cart" Width="480px" Height="385px" Modal="true" InitCenter="true" Hidden="true">
        <Items>
            <ext:BorderLayout ID="RegisterBorderLayout" runat="server">
                <Center>
                    <ext:FormPanel ID="RegisterFormPanel" runat="server" Border="false" ButtonAlign="Right" LabelWidth="80" Padding="15">
                        <Items>
                            <ext:TextField ID="MachineCode" runat="server" ReadOnly="true" FieldLabel="机器标识码" AnchorHorizontal="95%" AllowBlank="false" EmptyText="请启动服务程序获取机器标识码..." />
                            <ext:Label ID="TipLbl" runat="server" Icon="Information" Text="请将机器标识码提供给服务商以获取产品注册码" StyleSpec="color:#848585;" AnchorHorizontal="95%" />
                            <ext:Label ID="UserLimitLbl" runat="server" FieldLabel="用户数量" AnchorHorizontal="95%" />
                            <ext:Label ID="ExpiredLbl" runat="server" FieldLabel="有效日期" AnchorHorizontal="95%" />
                            <ext:Label ID="RemarkLbl" runat="server" FieldLabel="授权说明" AnchorHorizontal="95%" />
                            <ext:TextArea ID="LicenseCode" runat="server" FieldLabel="注册码" AnchorHorizontal="95%" Height="100" AllowBlank="false" EmptyText="请在此输入产品注册码..." />
                            <ext:FileUploadField ID="KeyFileUploadField" runat="server" AnchorHorizontal="95%" Height="25" Icon="Folder" ButtonOnly="true" ButtonText="导入授权文件..." >
                                <Listeners>
                                    <FileSelected Handler="#{UploadButton}.fireEvent('click');" />
                                </Listeners>
                            </ext:FileUploadField>
                        </Items>
                        <Buttons>
                            <ext:Button ID="SaveBtn" runat="server" Text="立即注册">
                                <DirectEvents>
                                    <Click OnEvent="SaveBtn_Click" Before="if(!#{RegisterFormPanel}.getForm().isValid()){return false;}">
                                        <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="RegisterFormPanel"/>
                                    </Click> 
                                </DirectEvents>
                            </ext:Button>
                            <ext:Button ID="CancelBtn" runat="server" Text="关闭">
                                <Listeners>
                                    <Click Handler="#{RegisterWnd}.hide();" />
                                </Listeners>
                            </ext:Button>
                        </Buttons>
                        <BottomBar>
                            <ext:StatusBar ID="ResultStatusBar" runat="server" Height="25"/>
                        </BottomBar>
                    </ext:FormPanel>
                </Center>
            </ext:BorderLayout>
        </Items>
    </ext:Window>
    <ext:Panel ID="CustomEl" runat="server" Border="false" Width="179" Height="113" Padding="5" Hidden="true">
        <Items>
            <ext:Label ID="CustomElTip" runat="server" StyleSpec="color:#555555;"/>
        </Items>
        <BottomBar>
            <ext:Toolbar ID="CustomElToolbar" runat="server">
                <Items>
                    <ext:ToolbarTextItem ID="BarLabel" runat="server" />
                    <ext:ToolbarFill ID="CustomElToolbarFill" runat="server" />
                    <ext:Button ID="CustomElRegister" runat="server" Icon="Cart" Text="立即注册">
                        <Listeners>
                            <Click Handler="#{register}.fireEvent('click');" />
                        </Listeners>
                    </ext:Button>
                </Items>
            </ext:Toolbar>
        </BottomBar>
    </ext:Panel>
    <ext:Button ID="UploadButton" runat="server" Text="上传文件" Hidden="true">
        <DirectEvents>
            <Click OnEvent="Upload_Click" IsUpload="true" Success="#{KeyFileUploadField}.reset();">
                <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="RegisterFormPanel"/>
            </Click> 
        </DirectEvents>
    </ext:Button>
    </form>
</body>
</html>
