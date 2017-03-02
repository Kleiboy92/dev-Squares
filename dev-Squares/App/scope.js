(function () {
    'use strict';

    var squaresApp = angular.module('squaresApp', ['ui.bootstrap', 'ngResource', 'ngRoute' , 'ngDialog']);

    squaresApp.config(["ngDialogProvider", function (ngDialogProvider) {
        ngDialogProvider.setDefaults({
            className: "ngdialog-theme-default",
            plain: false,
            showClose: true,
            closeByDocument: true,
            closeByEscape: true,
            appendTo: false,
            preCloseCallback: function () {
                console.log("default pre-close callback");
            }
        });
    }]);
})();
