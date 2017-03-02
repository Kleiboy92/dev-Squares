(function () {
    'use strict';

    angular
        .module('squaresApp')
        .factory('pageSvc', ['$http', ($http) => {

            return {

                getLists: function (action, pageSize, pageNumber, succ, err) {
                    return $http.get('/api/Points/' + action + '/' + pageSize + '/' + pageNumber).then(succ, err);
                },

                getSaved: function (succ, err) {
                    return $http.get('api/Points/Lists').then(succ, err);
                },

                save: function (name, succ, err) {
                    return $http.get('api/Points/Save/' + name).then(succ, err);
                },

                load: function (name, succ, err) {
                    return $http.get('api/Points/Load/' + name).then(succ, err);
                },

                delete: function (name, succ, err) {
                    return $http.get('api/Points/Delete/' + name).then(succ, err);
                },

                uploadList: function (fd, succ, err) {
                    $http.post('/api/Points/Upload', fd, {
                        headers: { 'Content-Type': undefined },
                    }).then(succ, err);
                },

                restrictions: function (succ, err) {
                    $http.get('/api/Points/Restrictions').then(succ, err);
                },

                clear: function(succ, err){
                    $http.get('/api/Points/Clear').then(succ, err);
                },

                download: function(){
                    window.location.href = '/api/Points/Download/';
                },

                addPoint: function(x, y, succ, err){
                    $http.get('/api/Points/Add/'+ x+ '/' +y).then(succ, err);
                },

                removePoint: function (x, y, succ, err) {
                    $http.get('/api/Points/Remove/' + x + '/' + y).then(succ, err);
                },

                solve: function (succ, err) {
                    $http.get('/api/Points/Solve').then(succ, err);
                },

                updateList: function(id, data, succ, err){
                    $http.put('/api/lists/' + id, data).then(succ, err);
                },

                deleteList: function(id, succ, err){
                    $http.delete('/api/lists/' + id).then(succ, err);
                }
            };

        }]);
})();
