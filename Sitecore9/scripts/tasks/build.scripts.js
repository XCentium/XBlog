var gulp = require('gulp');
var sourcemaps = require('gulp-sourcemaps');
var babel = require('gulp-babel');
var concat = require('gulp-concat');
var foreach = require("gulp-foreach");
var debug = require("gulp-debug");
var path = require('path');
var newer = require("gulp-newer");
var replace = require("gulp-replace");
var runSequence = require("run-sequence");

var config = require("../../gulpfile-config.js")();
var shared = require("../../gulpfile-global.js")();

gulp.task("build:scripts", function (callback) {
    return runSequence(
        "build:scripts:foundation",
        "build:scripts:feature",
        "build:scripts:project",
        callback);
});

var compileScripts = function (root) {
    var files = root + "/scripts/src/**/*.js";

    return gulp.src(files).pipe(
        foreach(function (stream, file) {
            var dest = file.path
                .replace("\\scripts\\src\\", "\\scripts\\dist\\")
                .replace(path.basename(file.path), "");

            return gulp.src(file.path)
                .pipe(babel({
                    presets: ['es2015']
                }))
                .pipe(newer(dest))
                .pipe(debug({ title: "Babel" }))
                .pipe(gulp.dest(dest));
        })
    );
}

var compileScript = function (file) {

    var dest = file.path
        .replace("\\scripts\\src\\", "\\scripts\\dist\\")
        .replace(path.basename(file.path), "");

    return gulp.src(file.path)
        .pipe(babel({
            presets: ['es2015']
        }))
        .pipe(newer(dest))
        .pipe(debug({ title: "Babel" }))
        .pipe(gulp.dest(dest));
}

gulp.task("build:scripts:foundation", function () {

    var foundationFilters = config.sourceRoot + "/Foundation/**/code/";

    return compileScripts(foundationFilters);
});

gulp.task("build:scripts:feature", function () {

    var featureFilters = config.sourceRoot + "/Feature/**/code/";

    return compileScripts(featureFilters);
});

gulp.task("build:scripts:project", function () {

    var projectFilters = config.sourceRoot + "/Project/**/code/";

    return compileScripts(projectFilters);
});

gulp.task("watch:build:scripts", function() {
    var root = config.sourceRoot;
    var roots = [
        root + "/Foundation/**/code/",
        root + "/Feature/**/code/",
        root + "/Project/**/code/",
        "!" + root + "/**/obj/**"
    ];
    var files = "/scripts/src/**/*.js";

    gulp.src(roots, { base: root }).pipe(
        foreach(function(stream, rootFolder) {
            console.log(rootFolder.path + files);
            gulp.watch(rootFolder.path + files,
                function(event) {
                    if (event.type === "changed") {
                        compileScript(event.path);
                    }
                });
            return stream;
        })
    );
});
