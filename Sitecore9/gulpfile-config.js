module.exports = function () {
  var projectRoot = __dirname;
    var instanceRoot = "C:\\Sites\\XBlog";
  var stageRoot = projectRoot + "\\stage";
  var sitecoreFilesRoot = projectRoot + "\\sitecore";

  var config = {
    sourceRoot: "./src",
    deploymentRoot: "./deployment",
    stageRoot: stageRoot,
    stageWebsiteRoot: stageRoot + "\\website",
    packagesFolder: "\\packages",
    serializationFolder: "\\serialization",
    websiteRoot: instanceRoot,
    websiteUrl: "http://xblog.sc",
    sitecoreFilesRoot: sitecoreFilesRoot,
    toolsRoot: "./tools",
    version: "1.0.0",
    solutionName: "XBlog",
    buildConfiguration: "Debug",
    buildToolsVersion: 15.0,
    buildMaxCpuCount: 0,
    buildVerbosity: "minimal",
    buildPlatform: "Any CPU",
    publishPlatform: "AnyCpu",
    runCleanBuilds: false
  };

  return config;
}