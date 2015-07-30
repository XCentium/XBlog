define(["sitecore"], function (Sitecore) {
    var BlogCreator = Sitecore.Definitions.App.extend({
        initialized: function () {


            $('*[data-sc-id="btnCreateBlog"]').on("click", function () {

                var uploadedFile = null;
                var type = null;
                var obj = null;
                var fileName = null;

                if (Sitecore.app.ufImport.viewModel.totalFiles._latestValue > 0) {
                    var postFile = Sitecore.app.ufImport.viewModel.upload();
                    fileName = Sitecore.app.ufImport.viewModel.getFileNames();
                }

                if (Sitecore.app.rbMVC.attributes.isChecked) {
                    type = "MVC";
                }
                else if (Sitecore.app.rbWF.attributes.isChecked) {
                    type = "WF"
                }

                if (type == null) {
                    alert('Select type');
                    return;
                }

                var sitecoreParentItemId = Sitecore.app.tvSitecore.viewModel.checkedItemIds._latestValue;
                var blogName = Sitecore.app.tbBlogTitle.viewModel.text._latestValue;

                var xBlogData = {
                    blogName: blogName,
                    blogParent: sitecoreParentItemId,
                    blogType: type,
                    uploadFile: fileName
                };

					$.ajax({
						type: "POST",
						url: "/api/sitecore/XBlog/Create",
						content: "application/json; charset=utf-8",
						dataType: "json",
						data: "xBlogData=" + JSON.stringify(xBlogData),
						success: function(d) {
							if (d.success == true)
								alert('success');
							else {}
						},
						error: function (xhr, textStatus, errorThrown) {
						    alert('Failed');
						}
					});
            });

        }
    });
    return BlogCreator;
});