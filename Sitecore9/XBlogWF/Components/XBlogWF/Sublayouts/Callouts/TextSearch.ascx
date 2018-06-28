<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextSearch.ascx.cs" Inherits="XBlogWF.Components.XBlogWF.Sublayouts.Callouts.TextSearch" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>


<script type="text/javascript">
    function SubmitFrm() {
        var Searchtxt = document.getElementById("txtSearch").value;
        if (Searchtxt != "") {
            window.location = "<%=searchUrl%>" + Searchtxt;
        }
    }
</script>

<div id='<%=settingsItem.PrimaryCSSID%>-TextSearch' >
    <input name="txtSearch" type="text" id="txtSearch" class="field" />            
    <input type="submit" name="btnSearch" value="<%=settingsItem.SearchButtonValue%>" id="btnSearch" class="btn" onclick="SubmitFrm(); return false;" />
</div>