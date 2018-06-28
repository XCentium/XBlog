var gulp = require('gulp');
var msbuild = require("gulp-msbuild");
var runSequence = require("run-sequence");
var nugetRestore = require("gulp-nuget-restore");
var foreach = require("gulp-foreach");
var debug = require("gulp-debug");
var path = require("path");
var util = require("gulp-util");
var args = require("yargs").argv;

var config = require("../../gulpfile-config.js")();

gulp.task("build", function (callback) {
  return runSequence(
    "build:nuget",
    "build:solution",
    callback);
});

gulp.task("build:nuget", function () {
  var solution = "./" + config.solutionName + ".sln";
  return gulp.src(solution).pipe(nugetRestore());
});


gulp.task("build:solution", function () {
  var targets = ["Build"];
  var buildConfiguration = args.buildConfiguration || config.buildConfiguration;

  if (config.runCleanBuilds) {
    targets = ["Clean", "Build"];
  }
  var solution = "./" + config.solutionName + ".sln";
  return gulp.src(solution)
      .pipe(msbuild({
        targets: targets,
        configuration: buildConfiguration,
        logCommand: false,
        verbosity: config.buildVerbosity,
        stdout: true,
        errorOnFail: true,
        maxcpucount: config.buildMaxCpuCount,
        toolsVersion: config.buildToolsVersion
      }));
});

gulp.task("build:transform", function () {
  var buildConfiguration = args.buildConfiguration || config.buildConfiguration;
  var transformFileName = "web." + buildConfiguration + ".config";

  var sitecoreFilters = (args.sitecoreFilesRoot || config.sitecoreFilesRoot) + "/**/files/Website/" + transformFileName;
  var foundationFilters = config.sourceRoot + "/Foundation/**/code/" + transformFileName;
  var featureFilters = config.sourceRoot + "/Feature/**/code/" + transformFileName;
  var projectFilters = config.sourceRoot + "/Project/**/code/" + transformFileName;
  var ignoreFilters = "!" + config.sourceRoot + "/**/{obj,bin}/" + transformFileName;

  var webConfigToDest = args.stageWebsiteRoot || config.stageWebsiteRoot;

  //// use stage:files:platform
  //var webConfigSource = config.sitecoreFilesRoot + "/web.config";

  //gulp.src(webConfigSource)
  //  .pipe(debug({ title: "Copying file: " }))
  //  .pipe(gulp.dest(webConfigToDest));

  var layerPathFilters = [sitecoreFilters, foundationFilters, featureFilters, projectFilters, ignoreFilters];
  return gulp.src(layerPathFilters)
    .pipe(foreach(function (stream, file) {
      util.log("Applying configuration transform: " + file.path);
      return gulp.src("./scripts/msbuild/applytransform.targets")
        .pipe(msbuild({
          targets: ["ApplyTransform"],
          configuration: buildConfiguration,
          logCommand: false,
          verbosity: config.buildVerbosity,
          stdout: true,
          errorOnFail: true,
          maxcpucount: config.buildMaxCpuCount,
          toolsVersion: config.buildToolsVersion,
          properties: {
            WebConfigToTransform: webConfigToDest + "/web.config",
            TransformFile: file.path
          }
        }));
    }));
});


