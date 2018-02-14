<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AuthorViewList.ascx.cs" Inherits="XBlogWF.Components.XBlogWF.Sublayouts.Callouts.AuthorViewList" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>

<div id='<%=settingsItem.PrimaryCSSID%>-AuthorList' >
    <sc:FieldRenderer ID="frTitle" Before="<h3>" After="</h3>" runat="server" />
    <asp:ListView ID="lvAuthorList" OnItemDataBound="lvAuthorList_ItemDataBound"  runat="server" >
        <LayoutTemplate>
            <div>
                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
            </div>
        </LayoutTemplate>
        <ItemTemplate>
                <div class="authorlistimage"><asp:Literal ID="litProfileImage" runat="server" /></div>
                <div class="authorlistlink"><asp:HyperLink ID="hlAuthor" runat="server" /></div>
            <br class="clear" />
        </ItemTemplate>
    </asp:ListView>
    <asp:Literal ID="ltlAuthorViewAllLink" runat="server" />

</div>