<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AuthorViewSL.ascx.cs" Inherits="XBlogWF.Components.XBlogWF.Sublayouts.AuthorViewSL" %>
<%@ Register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<%@ Import Namespace="Sitecore.Globalization" %>

    <div id='<%=settingsItem.PrimaryCSSID%>' >
        <h1><sc:FieldRenderer runat="server" ID="frTitle" /></h1>
        <asp:ListView ID="lvAuthorList" OnItemDataBound="lvAuthorList_ItemDataBound"  runat="server" >
            <LayoutTemplate>
                <div class="container-wrapper">
                    <div class="container-fluid">
                        <div class="authorlist">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </div>
                    </div>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <div class="authorcol">
                    <div>
                        <asp:Literal ID="litProfileImage" runat="server" />
                        <h2><asp:HyperLink runat="server" ID="hlAuthor" /></h2>
                    </div>
                </div>
            </ItemTemplate>
        </asp:ListView>
        <asp:Panel ID="pnlAuthor" runat="server" Visible="false">
            <div class="container-wrapper">
                <div class="container-fluid">
                    <p><span class="authorImage"><sc:FieldRenderer ID="frProfileImage" runat="server" CssClass="authorImage" /></span>
                    <sc:FieldRenderer ID="frAuthorTitle" runat="server" Before="<strong>Title: </strong>" After="<br />" />
                    <sc:FieldRenderer ID="frLocation" runat="server" Before="<strong>Location: </strong>" After="<br />" />
                    <sc:FieldRenderer ID="frBio" runat="server" /></p>
                    <br />
                    <h3><asp:Literal ID="litSearchHeading" runat="server" /></h3>
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
                                <br class="clear" />
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
        </asp:Panel>
    </div>