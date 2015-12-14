<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HeaderControl.ascx.cs" Inherits="Delta.PECS.WebCSC.Site.HeaderControl" %>
<div class="controlHeader">
    <div class="<%= GetLocalResourceObject("Div.HeaderLogo.CssClass") %>">
        <span class="HeaderTip">
            <a href="javascript:void(0);"><%= String.Format("{0} {1}", Page.User.Identity.Name, GetLocalResourceObject("Span.HeaderTip.InnerHtml"))%></a>
            <%if(ShowNavMap()) {%>
            |<a href="NavMaps.aspx" target="_blank" title="<%= GetLocalResourceObject("A.NavMap.Title") %>"><%= GetLocalResourceObject("A.NavMap.InnerText")%></a>
            <%} %>
            |<a href="Logout.aspx"  target="_self" title="<%= GetLocalResourceObject("A.Logout.Title") %>"><%= GetLocalResourceObject("A.Logout.InnerText")%></a>
        </span>
    </div>
</div>
