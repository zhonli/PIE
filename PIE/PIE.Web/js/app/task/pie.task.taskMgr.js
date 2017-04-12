angular.module('pie')
    .controller('TaskManager',
        ['$scope', '$state', 'ProcessSource', function ($scope, $state, ProcessSource) {
            console.log(">>>>>> TaskManager");
            $scope.ProcessSource = ProcessSource;
            $scope.aptProcesses = ProcessSource.newSource();
            $scope.curProcess = null;
            $scope.$on('processSelectedEvent', function (event, data) {
                $scope.curProcess = data;
                $scope.$broadcast('processSelected', data);
            });

            $scope.$on('progressUpdatedEvent', function (event) {
                $scope.$broadcast('progressUpdated');
            });

            $scope.$on('processClosedEvent', function (event, data) {
                $scope.$broadcast('processClosed');
            });

            $scope.$on('processBlockedEvent', function (event) {
                $scope.$broadcast('processBlocked');
            });

            $scope.$on('processCuttedEvent', function (event) {
                $scope.$broadcast('processCutted');
            });

        }]);