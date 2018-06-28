var gulp = require('gulp');
var debug = require("gulp-debug");
var del = require("del");
var runSequence = require("run-sequence");

var config = require("../../gulpfile-config.js")();

gulp.task("clean", function (callback) {
  return runSequence(
    "clean:build",
    "clean:stage:website",
    "clean:stage:serialization",
    "clean:stage:packages",
    callback);
});

var deleteFiles = function (location) {
    if (location === undefined)
        return "undefined";

  return del(location);
};

gulp.task("clean:build", function () {
    return deleteFiles("./**/{code,tests}/{bin,obj}");
});

gulp.task("clean:stage:website", function () {
    return deleteFiles(config.stageWebsiteRoot);
});

gulp.task("clean:stage:serialization", function () {
  return deleteFiles(config.stageRoot + config.serializationFolder);
});

gulp.task("clean:stage:packages", function () {
  return deleteFiles(config.stageRoot + config.packagesFolder);
});

gulp.task("clean:testresults", function () {
  return deleteFiles("./TestResults");
});