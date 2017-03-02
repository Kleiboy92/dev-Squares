module.exports = function (grunt) {
    grunt.loadNpmTasks('grunt-typescript');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-iisexpress');
    grunt.loadNpmTasks('grunt-open');
    grunt.loadNpmTasks('grunt-contrib-jasmine');
    grunt.loadNpmTasks('grunt-contrib-less');
    grunt.loadNpmTasks('grunt-concurrent');

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        jasmine : {
            'dev-Squares/Scripts/app.js',
            options : {
                vendor: [
                    'dev-Squares/Scripts/jquery-1.10.2.js',
                    'dev-Squares/Scripts/angular.js',
                    'dev-Squares/Scripts/angular-route.js',
                    'dev-Squares/Scripts/angular-mocks.js',
                    'dev-Squares/Scripts/restangular.js'
                ],
            }
        },
        iisexpress: {
            server: {
                options: {
                    port: 5000,
                    cmd: 'c:/program files (x86)/iis express/iisexpress.exe',
                    path: 'dev-Squares'
                }
            }
        },
        typescript: {
            base: {
                src: [
                    'dev-Squares/Scripts/typings/**/*.d.ts',
                    'dev-Squares/App/**/*.ts'],
                dest: 'dev-Squares/Scripts/app.js',
                options: {
                    module: 'amd',
                    target: 'es5',
                    sourceMap: true,
                    declaration: true
                }
            }
        },
        concurrent: {
            options: {
                logConcurrentOutput: true
            },
            dev: {
                tasks: ["watch:styles", "watch:scripts"]
            }
        },
        watch: {
            scripts: {
                files: [
                    'dev-Squares/App/**/*.ts'],
                //tasks: ['typescript:base', 'jasmine']
                tasks: ['typescript:base']
            },
            server: {
                files: [
                    'dev-Squares/bin/*.dll',
                    'dev-Squares/Scripts/**/*.js',
                    'dev-Squares/Scripts/*.js',
                    'dev-Squares/**/*.css',
                    'dev-Squares/Views/**/*.html',
                    'dev-Squares/Views/**/*.cshtml'],
                options: { livereload: true }
            }
        },
        open: {
            dev: {
                path: 'http://localhost:5000/'
            }
        }
    });

    grunt.registerTask('dev', ['concurrent:dev']);
    grunt.registerTask('server', ['iisexpress', 'open', 'watch:server']);
    grunt.registerTask('default', ['dev']);
}
