<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OGPostMeta.ascx.cs" Inherits="XBlogWF.Components.XBlogWF.Sublayouts.Callouts.OGPostMeta" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<%@ Import Namespace="Sitecore.Globalization" %>


<meta property="og:url" content="<%=ogURL%>" />
<meta property="og:title" content="<%=ogTitle%>" />
<meta property="og:description" content="<%=ogDescription%>" />
<meta property="og:type" content="article" />
<meta property="og:image" content="<%=ogAuthorImage%>" />