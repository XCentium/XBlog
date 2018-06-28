var gulp = require('gulp');
var foreach = require("gulp-foreach");
var newer = require("gulp-newer");
var debug = require("gulp-debug");
var runSequence = require("run-sequence");
var tap = require("gulp-tap");
var args = require("yargs").argv;

var config = require("../../gulpfile-config.js")();
var shared = require("../../gulpfile-global.js")();
var unicorn = require("../unicorn.js");

gulp.task("stage", function (callback) {
  return runSequence(
    "stage:files:platform",
    "stage:publish:foundation",
    "stage:publish:feature",
    "stage:publish:project",
    "stage:serialization",
    callback);
});

//////////////////////////////// Platform //////////////////////////////////////

gulp.task("stage:files:platform", function () {
  var root = args.sitecoreFilesRoot || config.sitecoreFilesRoot;
  var roots = [
    root + "/**/Website"
  ];
  var files = "/**/*";
  var dest = args.stageWebsiteRoot || config.stageWebsiteRoot;

  return shared.publishFiles(roots, files, dest);
});

/////////////////////////////// Publish Projects //////////////////////////////

gulp.task("stage:publish:foundation", function () {
  var source = config.sourceRoot + "/Foundation";
  var dest = args.stageWebsiteRoot || config.stageWebsiteRoot;

  return shared.publishProjects(source, dest);
});

gulp.task("stage:publish:feature", function () {
  var source = config.sourceRoot + "/Feature";
  var dest = args.stageWebsiteRoot || config.stageWebsiteRoot;

  return shared.publishProjects(source, dest);
});

gulp.task("stage:publish:project", function () {
  var source = config.sourceRoot + "/Project";
  var dest = args.stageWebsiteRoot || config.stageWebsiteRoot;

  return shared.publishProjects(source, dest);
});


///////////////// Stage Assemblies, Configs, Content, Views ///////////////////

gulp.task("stage:files:assemblies", function () {
  var root = config.sourceRoot;
  var roots = [
    root + "/**/code/bin",
    "!" + root + "/**/tests/bin"
  ];
  var files = ["/**/*.{Foundation,Feature,Project,Website}*.{dll,pdb}"];
  var dest = (args.stageWebsiteRoot || config.stageWebsiteRoot) + "\\bin";

  return shared.publishFiles(roots, files, dest);
});

gulp.task("stage:files:config", function () {
  var root = config.sourceRoot;
  var roots = [
    root + "/{Foundation,Feature,Project}/**/code/App_Config",
    "!" + root + "/{Foundation,Feature,Project}/**/obj/**/App_Config",
    "!" + root + "/**/Release/**/App_Config",
    "!" + root + "/**/tests/App_Config"
  ];
  var files = "/**/*.config";
  var dest = (args.stageWebsiteRoot || config.stageWebsiteRoot) + "\\App_Config";

  return shared.publishFiles(roots, files, dest);
});

gulp.task("stage:files:views", function () {
  var root = config.sourceRoot;
  var roots = [
    root + "/**/code/Views",
    "!" + root + "/**/obj/**/Views"
  ];
  var files = "/**/*.cshtml";
  var dest = (args.stageWebsiteRoot || config.stageWebsiteRoot) + "\\Views";

  return shared.publishFiles(roots, files, dest);
});

gulp.task("stage:files:content", function () {
  var root = config.sourceRoot;
  var roots = [
    root + "/**/code",
    "!" + root + "/**/code/obj",
    "!" + root + "/**/code/**/obj",
    "!" + root + "/**/code/obj/**",
    "!" + root + "/**/code/**/obj/**"
  ];

  var files = "/**/*.{css,js,eot,svg,ttf,woff,woff2}";
  var dest = (args.stageWebsiteRoot || config.stageWebsiteRoot);

  return shared.publishFiles(roots, files, dest);
});

///////////////////////////// Unicorn Files ///////////////////////////////////

var stageSerialization = function (roots, dest) {
  return gulp.src(roots)
    //.pipe(debug())
    .pipe(tap(function (file, t) {
      var itemPath = unicorn.getFullItemPath(file);
      return t;
    })
    .pipe(gulp.dest(dest)));
}

gulp.task("stage:serialization:platform", function () {
  var root = args.sitecoreFilesRoot || config.sitecoreFilesRoot;
  var roots = [
    root + "/**/serialization/**/*.yml"
    //"!" + root + "/**/serialization/*.Roles/**/*.yml",
    //"!" + root + "/**/serialization/*.Users/**/*.yml"
  ];
  var dest = (args.stageRoot || config.stageRoot) + config.serializationFolder;

  return stageSerialization(roots, dest);
});

gulp.task("stage:serialization:solution", function () {
  var root = config.sourceRoot;
  var roots = [
    root + "/**/serialization/**/*.yml",
    root + "/**/serialization.*/**/*.yml"
    //"!" + root + "/**/serialization/*.Roles/**/*.yml",
    //"!" + root + "/**/serialization/*.Users/**/*.yml"
  ];
  var dest = (args.stageRoot || config.stageRoot) + config.serializationFolder;

  return stageSerialization(roots, dest);
});

gulp.task("stage:serialization", function (callback) {
  return runSequence(
    "stage:serialization:platform",
    "stage:serialization:solution",
    callback);
});

/////////////////////////// Local Deployment /////////////////////////////////

gulp.task("stage:files:local", function () {
  var root = config.deploymentRoot;
  var roots = [
    root + "/Roles/Roles/Local/App_Config"
  ];
  var files = "/**/*.config";
  var dest = (args.stageWebsiteRoot || config.stageWebsiteRoot) + "\\App_Config";

  return shared.publishFiles(roots, files, dest);
});