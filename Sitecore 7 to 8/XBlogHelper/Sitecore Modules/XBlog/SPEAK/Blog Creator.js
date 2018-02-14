define(["sitecore"], function (Sitecore) {
    var BlogCreator = Sitecore.Definitions.App.extend({
        initialized: function () {
            //    this.ComboBox1.set("items", data);
            this.btnUpload.on("click", function () {
                if (this.UploadFiles.viewModel.totalFiles() < 1)
                    alert("Please select a file");
                else {
                    /*TODO Give user error or success message*/
                    if(this.RadioButton1.viewModel.isChecked()) {
                        alert("works");
                    }
                    alert("about to run job");
                    var json = this.UploadFiles.viewModel.upload();
                    alert("ran job");
                    alert(json);
                    obj = JSON.parse(json);
                    alert(obj);
                }
            }, this);
        }
    });
    return BlogCreator;
});