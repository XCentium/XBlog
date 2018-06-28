var gulp = require('gulp');
var config = require("./gulpfile-config.js")();
var foreach = require("gulp-foreach");
var newer = require("gulp-newer");
var debug = require("gulp-debug");
var msbuild = require("gulp-msbuild");
var args = require("yargs").argv;

var publishFiles = function (roots, files, destination) {
  return gulp.src(roots).pipe(
    foreach(function (stream, file) {
      console.log("Publishing from " + file.path);
      gulp.src(file.path + files, { base: file.path })
        .pipe(newer(destination))
        .pipe(debug({ title: "Copying " }))
        .pipe(gulp.dest(destination));
      return stream;
    })
  );
}

var publishProjects = function (location, dest) {
  dest = dest || config.stageWebsiteRoot;
  var targets = ["Build"];
  var buildConfiguration = args.buildConfiguration || config.buildConfiguration;

  console.log("publish from " + location + " folder");
  console.log("publish to " + dest + " folder");
  var locations = [location + "/**/*.csproj", "!" + location + "/**/tests/*.csproj"];
  return gulp.src(locations)
    .pipe(foreach(function (stream, file) {
      return stream
        .pipe(debug({ title: "Building project:" }))
        .pipe(msbuild({
          targets: targets,
          configuration: buildConfiguration,
          logCommand: false,
          verbosity: config.buildVerbosity,
          stdout: true,
          errorOnFail: true,
          maxcpucount: config.buildMaxCpuCount,
          toolsVersion: config.buildToolsVersion,
          properties: {
            DeployOnBuild: "true",
            DeployDefaultTarget: "WebPublish",
            WebPublishMethod: "FileSystem",
            DeleteExistingFiles: "false",
            publishUrl: dest,
            _FindDependencies: "false"
          }
        }));
    }));
}


module.exports = function () {
  var common = {
    publishFiles: publishFiles,
    publishProjects: publishProjects
  }
  return common;
}


