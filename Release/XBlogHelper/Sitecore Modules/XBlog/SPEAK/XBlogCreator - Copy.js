define(["sitecore", "jquery"], function (Sitecore, jQuery) {
    var AddEditPropertyPage = Sitecore.Definitions.App.extend({
 
        initialized: function ()
        {
				alert("asd");

            this.btnUpload.on("click", function () {
                if (this.UploadFiles.viewModel.totalFiles() < 1)
                    alert("Please select a file");
                else {

                    alert("about to run job");
                    var json = this.UploadFiles.viewModel.upload();
                    alert("ran job");
                    alert(json);
                    obj = JSON.parse(json);
                    alert(obj);
                }
            }, this);





            var type = null;


            /*TODO Give user error or success message*/
            if (this.rbMVC.viewModel.isChecked()) {
                type = "MVC";
            }
            else if (this.rbWF.viewModel.isChecked()) {
                type = "WF"
            }

            if (type == null) {
                alert('Select type');
                return;
            }
		
			$('*[data-sc-id="btnCreateBlog"]').on("click", function () {
			

				alert(_sc.Components[9].selectedItem);
				
				alert(Sitecore.app.valueOf("tbBlogTitle"));
			
				
				//alert($('*[data-sc-id="tbBlogTitle"]').val());
				
				var xBlogData = {
								blogName: $('*[data-sc-id="tbBlogTitle"]').val(),
								blogParent: "asdasda",
								blogType: "MVC"
							};


					BlogTitle		
				/*

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
							// TODO: Show error
						}
					});
				*/
			});
			
			

		
		

        }
 
    });
    return AddEditPropertyPage;
});