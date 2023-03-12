const gulp = require('gulp');
const sass = require('gulp-sass')(require('sass'));
const cleanCSS = require('gulp-clean-css');
const autoprefixerCSS = require('gulp-autoprefixer');
const renameGULP = require('gulp-rename');

const GUPLUglify = require('gulp-uglify');
const TypeScript = require('gulp-typescript');

const paths = {
    scss: "./Views/Resources/Styles/**/*.scss",
    css: "./wwwroot/css/",
    typescript: './Views/Resources/Scripts/**/*.ts',
    js: './wwwroot/js'
};

const tsProject = TypeScript.createProject('tsconfig.json');

gulp.task('sass', () => {
    return gulp.src(paths.scss)
        .pipe(sass().on('error' , sass.logError))
        .pipe(gulp.dest(paths.css))
        .pipe(autoprefixerCSS({
            overrideBrowserslist: ['last 2 versions'],
            cascade: false
        }))
        .pipe(cleanCSS())
        .pipe(renameGULP({
            suffix: '.min'
        }))
        .pipe(gulp.dest(paths.css));
});

gulp.task('typescript', () => {
    return gulp.src(paths.typescript)
        .pipe(tsProject())
        .pipe(gulp.dest(paths.js))
        .pipe(GUPLUglify())
        .pipe(renameGULP({ suffix: '.min' }))
        .pipe(gulp.dest(paths.js));
});

gulp.task('watch-typescript', () => {
    gulp.watch(paths.typescript, gulp.series('typescript'));
});

gulp.task('watch-sass', () => {
    gulp.watch(paths.scss, gulp.series('sass'));
});

gulp.task('default', gulp.series('sass', 'typescript'));
