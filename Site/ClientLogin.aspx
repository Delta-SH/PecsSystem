<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientLogin.aspx.cs" Inherits="Delta.PECS.WebCSC.Site.ClientLogin" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>CASP认证登录</title>
    <link rel="shortcut icon" type="image/x-icon" href="favicon.ico" />
    <link rel="icon" type="image/x-icon" href="favicon.ico" />
    <link rel="bookmark" type="image/x-icon" href="favicon.ico" />
    <style type="text/css">
        body {
            text-align:center;
        }
        .errtips {
	        background:#EEEEEE;
	        color:#cc0000;
	        width:400px;
	        margin-left:auto;
	        margin-right:auto;
	        margin-top:50px;
	        -moz-border-radius:5px; 
	        -webkit-border-radius:5px; 
	        border-radius:5px;
        }
        .title {
             font-size:14px;
             font-weight:bold;
             text-align:center;
             padding-top:20px;
             padding-bottom:10px;
             padding-left:10px;
             padding-right:10px;
        }
        .content {
            height:80px;
            font-size:12px;
            text-align:center;
            padding-top:0;
            padding-bottom:0;
            padding-left:10px;
            padding-right:10px;
        }
        .advice {
            height:30px;
            line-height:30px;
            color:#a2a2a2;
        }
        .advice a {
            color:#a2a2a2;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ErrorTips" runat="server" class="errtips" visible="false">
        <div class="title">CASP认证登录失败</div>
        <div class="content">
            <span id="FailureText" runat="server" />
        </div>
        <div class="advice">
            友情提示：<a href="Login.aspx" target="_self">您可以跳转到登录界面，手动登录系统»</a>
        </div>
    </div>
    </form>
</body>
</html>
