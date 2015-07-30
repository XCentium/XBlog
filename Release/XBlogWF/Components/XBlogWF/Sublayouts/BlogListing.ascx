<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BlogListing.ascx.cs" Inherits="XBlogWF.Components.XBlogWF.Sublayouts.BlogListing" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<%@ Import Namespace="Sitecore.Globalization" %>

<div id='<%=settingsItem.PrimaryCSSID%>'>
    <h1>
        <sc:Text runat="server" ID="scTitle" Field="Blog Name" />
    </h1>
    <h3>
        <asp:Literal ID="litSearchHeading" runat="server" /></h3>
        <div class="container-wrapper">
                <div class="container-fluid">
                <asp:ListView ID="lvBlogPosts" EnableViewState="false" OnItemDataBound="lvBlogPosts_ItemDataBound" runat="server">
                    <LayoutTemplate>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <div itemprop="headline">
                            <h2>
                                <asp:HyperLink runat="server" ID="hlTitle" /></h2>
                        </div>
                        <div class="set authorheading" itemprop="author">
                            <asp:Literal runat="server" ID="litProfileImage" />
                            by
                            <asp:Literal runat="server" ID="litAuthor" /><br />
                            <sc:Date ID="dateBlogPostPublishDate" runat="server" Field="Publish Date" />
                        </div>
                        <p class="clear">
                            <asp:Literal runat="server" ID="scBlogPostSummary" />
                        </p>
                        <div itemprop="articleSection">
                            <asp:Literal runat="server" ID="litCategories" />
                        </div>
                        <div itemprop="articleSection">
                            <asp:Literal runat="server" ID="litTags" />
                        </div>
                        <div class="readmore">
                            <asp:HyperLink runat="server" ID="hlReadMore" />
                        </div>
                        <br />
                    </ItemTemplate>
                    <EmptyDataTemplate>
                    <%= Translate.Text("Sorry no matching results were found.") %>
                </EmptyDataTemplate>
                </asp:ListView>
                <div class="pagination"><asp:Literal ID="litPagination" runat="server" /></div>
        </div>
    </div>
</div>
