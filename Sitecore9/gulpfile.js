var gulp = require("gulp");
var runSequence = require("run-sequence");
var requireDir = require('require-dir');

var config = require("./gulpfile-config.js")();
var tasks = requireDir("./scripts/tasks");

module.exports.config = config;

gulp.task("default", function (callback) {
  config.runCleanBuilds = true;
  return runSequence(
    "clean",
    "build",
    //"test:solution",
    "stage",
    "build:transform",
    "deploy",
    callback);
});

gulp.task("_default:local", function (callback) {
  config.runCleanBuilds = true;
  return runSequence(
    "clean:stage:website",
    "clean:stage:serialization",
    "build:nuget",
    "stage",
    "stage:files:local",
    "build:transform",
    "deploy",
    callback);
});

gulp.task("_default:local:files", function (callback) {
    config.runCleanBuilds = true;
    return runSequence(
        "clean:stage:website",
		"build:nuget",
		"stage",
		"stage:files:local",
		"build:transform",
        "deploy:files",
        callback);
});
