module.exports = function () {
    var instanceRoot = "C:\\inetpub\\wwwroot\\xb.local";
    var config = {
        websiteRoot: instanceRoot + "\\",
        sitecoreLibraries: instanceRoot + "\\bin",
        licensePath: instanceRoot + "\\Data\\license.xml",
        solutionName: "XBlog",
        buildConfiguration: "Debug",
        runCleanBuilds: false
    };
    return config;
};