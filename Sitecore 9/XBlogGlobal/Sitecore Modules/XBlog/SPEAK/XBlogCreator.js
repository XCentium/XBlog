define(["sitecore"], function (Sitecore) {
    var BlogCreator = Sitecore.Definitions.App.extend({
        initialized: function () {

            this.btnCreateBlog.on("click", function () {

                var uploadedFile = null;
                var type = null;
                var obj = null;
                var fileName = null;

                if (this.ufImport.viewModel.getFileNames().length > 0) {
                    var postFile = this.ufImport.viewModel.upload();
                    fileName = this.ufImport.viewModel.getFileNames();
                }

                if (this.rbMVC.attributes.isChecked) {
                    type = "MVC";
                }
                else if (this.rbWF.attributes.isChecked) {
                    type = "WF"
                }

                if (type == null) {
                    alert('Select a type');
                    return;
                }

                var sitecoreParentItemId = this.tvSitecore.viewModel.checkedItemIds._latestValue;

                if (sitecoreParentItemId == null) {
                    alert('Select a location for the blog');
                    return;
                }

                var blogName = this.tbBlogTitle.viewModel.text._latestValue;

                var xBlogData = {
                    blogName: blogName,
                    blogParent: sitecoreParentItemId,
                    blogType: type,
                    uploadFile: fileName
                };


                var imgLoader = this.imgLoader;
                var scLink = this.hbtnSitecore;
                this.btnCreateBlog.viewModel.hide();
                imgLoader.viewModel.show();

                $.ajax({
                    type: "POST",
                    url: "/api/sitecore/XBlog/Create",
                    content: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "xBlogData=" + JSON.stringify(xBlogData),
                    success: function (d) {
                        if (d == 'success') {
                            imgLoader.viewModel.hide();
                            scLink.viewModel.show();
                            alert('Success! your blog has been created.');
                        }
                        else {
                            imgLoader.viewModel.hide();
                            alert('Failed to create blog, check Sitecore logs for errors.');
                        }
                    },
                    error: function (xhr, textStatus, errorThrown) {
                        imgLoader.viewModel.hide();
                        alert('Failed to create blog, check Sitecore logs for errors.');
                    }
                });
            }, this);
        }
    });
    return BlogCreator;
});