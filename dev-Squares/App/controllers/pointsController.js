(function () {
    'use strict';

    angular
        .module('squaresApp')
        .controller('pointsController', simpleCtrl);

    simpleCtrl.$inject = ['$scope', 'pageSvc', 'httpQueue', '$timeout', 'ngDialog'];

    function simpleCtrl($scope, pageSvc, httpQueue, $timeout, ngDialog) {

        $scope.info = {};
        $scope.results = [];
        $scope.alerts = [];
        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };

        $scope.max = 0;
        $scope.min = 0;
        $scope.size = 0;
        $scope.xValue = 0;
        $scope.yValue = 0;

        $scope.options = {
            maxSize: 5,
        };

        $scope.resultOptions = {
            maxSize: 5,
        };

        $scope.info = {
            totalItems: 0,
            totalPages: 1,
            currentPage: 0,
        };

        $scope.resultInfo = {
            totalItems: 0,
            totalPages: 1,
            currentPage: 0,
        };
        //dropdown
        $scope.status = {
            isopen1: false,
            isopen2: false
        };

        $scope.state = {
            message: "Calculate",
        };


        pageSvc.restrictions((res) => {
            $scope.max = res.data.max;
            $scope.min = res.data.min;
            $scope.size = res.data.size;
        }, (err) => {
            $scope.alerts.push({ msg: 'server dead!' });
        });

        $scope.pageSizes = [5, 10, 20, 50];

        $scope.clear = function(){
            pageSvc.clear((res) => {
                loadPoints();
            }, (err) => {
                $scope.alerts.push({ msg: 'server dead!' });
            });
        };

        $scope.addPoint = function (x, y) {
            pageSvc.addPoint(x, y, (res) => {
                $scope.alerts.push({ msg: res.data, type: 'success' });
                loadPoints();
            }, (err) => {
                $scope.alerts.push({
                    msg: err.data.exceptionMessage, type: 'danger'
                });
            });
        };

        $scope.removePoint = function (x, y) {
            pageSvc.removePoint(x, y, (res) => {
                $scope.alerts.push({ msg: res.data, type: 'success' });
                loadPoints();
            }, (err) => {
                $scope.alerts.push({
                    msg: err.data.exceptionMessage, type: 'danger'
                });
            });
        };

        $scope.setPage = function (pageNo) {
            $scope.info.currentPage = pageNo;
            loadPoints();
        };

        $scope.pageChanged = function () {
            $scope.setPage($scope.info.currentPage)
        };

        $scope.setSize = function (size) {
            $scope.options.maxSize = size;
            loadPoints();
        };

        $scope.download = function (size) {
            pageSvc.download();
        };

        $scope.solve = function () {
            pageSvc.solve((res) => {
                loadResults();
                $scope.state.message = 'working';
            }, (err) => {
                $scope.alerts.push({
                    msg: err.data.exceptionMessage, type: 'danger'
                });
            });
        };

        $scope.uploadFile = function (files) {
            var fd = new FormData();
            //Take the first selected file
            fd.append("file", files[0]);
            pageSvc.uploadList(fd, (res) => {
                res.data.forEach((el) => {
                    $scope.alerts.push({ msg: el.message, type: el.type });
                });
                loadPoints();
            }, (err) => {
                $scope.alerts.push({ msg: 'unable to send file' });
            });


        };

        

        $scope.toggleDropdown = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.status.isopen = !$scope.status.isopen;
        };


        function loadPoints() {
            pageSvc.getLists('GetPoints', $scope.options.maxSize, $scope.info.currentPage,
                (res) => {
                    $scope.points = res.data.results;
                    $scope.info.totalPages = res.data.totalPages,
                    $scope.info.totalItems = res.data.totalCount;

                }, (err) => {
                    $scope.info.currentPage -= 1;
                })
        };


        loadPoints();

        function loadResults() {
            httpQueue(
                pageSvc.getLists('GetResults', $scope.resultOptions.maxSize, $scope.resultInfo.currentPage,
                  (res) => {
                      $scope.results = res.data.results;
                      $scope.resultInfo.totalPages = res.data.totalPages,
                      $scope.resultInfo.totalItems = res.data.totalCount;
                      if (res.status === 201) {
                          $timeout(loadResults, 1000, true);
                      } else {
                          $scope.state.message = 'Calculate';
                      }
                  }, (err) => {
                      $scope.resultInfo.currentPage -= 1;
                  }));
        };
        loadResults();


        $scope.setResultPage = function (pageNo) {
            $scope.resultInfo.currentPage = pageNo;
            loadResults();
        };

        $scope.resultPageChanged = function () {
            $scope.setResultPage($scope.resultInfo.currentPage)
        };

        $scope.setResultSize = function (size) {
            $scope.resultOptions.maxSize = size;
            loadResults();
        };

        function getListNames() {
            pageSvc.getSaved((res) => {
                $scope.lists = res.data
            }, (err) => {
            })
        }

        getListNames();

        $scope.loadList = function () {
            ngDialog.open({
                template: '/app/views/load.html',
                className: 'ngdialog-theme-default',
                data: {
                    lists: $scope.lists,
                    listSelected: $scope.listSelected
                }
            });
        };

        $scope.listSelected = function (name) {
            ngDialog.close();

            pageSvc.load(name, (res) => {
                getListNames();
                loadPoints();
            }, (err) => {
            });
        };

        $scope.deleteList = function () {
            ngDialog.open({
                template: '/app/views/delete.html',
                className: 'ngdialog-theme-default',
                data: {
                    lists: $scope.lists,
                    deleteList: $scope.listDelete
                }
            });
        };

        $scope.listDelete = function (name) {
            ngDialog.close();

            pageSvc.delete(name, (res) => {
                getListNames();
            }, (err) => {
            });
        };

        $scope.saveList = function () {
            ngDialog.open({
                template: '/app/views/save.html',
                className: 'ngdialog-theme-default',
                data: {
                    save: $scope.save
                }
            });
        };

        $scope.save = function (name) {
            ngDialog.close();

            pageSvc.save(name, (res) => {
                getListNames();
            }, (err) => {
            });
        };
    }
})();
