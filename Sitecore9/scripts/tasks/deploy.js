var gulp = require('gulp');
var foreach = require("gulp-foreach");
var newer = require("gulp-newer");
var debug = require("gulp-debug");
var runSequence = require("run-sequence");

var config = require("../../gulpfile-config.js")();
var shared = require("../../gulpfile-global.js")();
var unicorn = require("../unicorn.js");

gulp.task("deploy", function (callback) {
  return runSequence(
    "deploy:files",
    "deploy:items",
    callback);
});

gulp.task("deploy:files", function () {
  var roots = [
    config.stageRoot + "/website/**",
    "!" + config.stageRoot + "/website/**/ConnectionStrings.config",
    "!" + config.stageRoot + "/website/packages.config"
  ];

  return gulp.src(roots)
      .pipe(newer(config.websiteRoot))
      .pipe(debug({ title: "Copying" }))
      .pipe(gulp.dest(config.websiteRoot));
});


gulp.task("deploy:items", function (callback) {
  var options = {};
  options.siteHostName = config.websiteUrl;
  // options.authenticationConfigFile = config.websiteRoot + "/App_config/Include/Unicorn/Unicorn.UI.config";
  options.authenticationConfigFile = config.websiteRoot + "/App_config/Include/Unicorn/Unicorn.zSharedSecret.config";

  unicorn(function () { return callback() }, options);
});
