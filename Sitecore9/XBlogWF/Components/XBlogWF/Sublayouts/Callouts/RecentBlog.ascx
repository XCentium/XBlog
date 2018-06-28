<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RecentBlog.ascx.cs" Inherits="XBlogWF.Components.XBlogWF.Sublayouts.Callouts.RecentBlog" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<%@ Import Namespace="Sitecore.Globalization" %>

<div id='<%=settingsItem.PrimaryCSSID%>-RecentBlog' >
    <sc:FieldRenderer ID="frTitle" Before="<h3>" After="</h3>" runat="server" />
    <asp:ListView ID="lvBlogPosts" EnableViewState="false" OnItemDataBound="lvBlogPosts_ItemDataBound" runat="server">
        <LayoutTemplate>
            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
        </LayoutTemplate>
        <ItemTemplate>
            <div itemprop="headline">
                <asp:HyperLink runat="server" ID="hlTitle" />
            </div>
            <div class="set authorheading" itemprop="author">
                By
                <asp:Literal runat="server" ID="litAuthor" />
                On
                <sc:Date ID="dateBlogPostPublishDate" runat="server" Field="Publish Date" />
            </div>
            <br />
        </ItemTemplate>
        <EmptyDataTemplate>
        <%= Translate.Text("Sorry no matching results were found.") %>
    </EmptyDataTemplate>
    </asp:ListView>
</div>