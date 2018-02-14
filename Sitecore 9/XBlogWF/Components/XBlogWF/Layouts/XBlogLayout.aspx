<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="XBlogLayout.aspx.cs" Inherits="XBlogWF.Components.XBlogWF.Layouts.XBlogLayout" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <sc:Placeholder ID="phHead" runat="server" Key="xbloghead"></sc:Placeholder>
    <title></title>
    <link rel="stylesheet" type="text/css" href="/Components/XBlog Assets/Style/XBlog.css" media="all" />

    <!-- RRSSB -->
    <link rel="stylesheet" href="/Components/XBlog Assets/Style/RRSSB/rrssb.css" />
    <script src="/Components/XBlog Assets/JS/RRSSB/vendor/modernizr-2.6.2-respond-1.1.0.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="XBlogMain">
            <sc:Placeholder ID="phMain" runat="server" Key="xblogmain"></sc:Placeholder>
        </div>
        <div class="XBlogRight">
            <sc:Placeholder ID="phRight" runat="server" Key="xblogright"></sc:Placeholder>
        </div>
    </form>

    <!-- RRSSB -->
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <script>window.jQuery || document.write('<script src="/Components/XBlog Assets/JS/RRSSB/vendor/jquery.1.10.2.min.js"><\/script>')</script>

    <script src="/Components/XBlog Assets/JS/RRSSB/rrssb.min.js"></script>

</body>
</html>
