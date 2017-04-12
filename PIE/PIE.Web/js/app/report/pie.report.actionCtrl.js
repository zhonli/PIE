angular.module('pie')
    .controller('ReportActionCtrl', ['$scope', '$state', 'toaster', function ($scope, $state, toaster) {
        console.log('>>>>ReportActionCtrl');

        $scope.reDesginReport = function () {
            toaster.pop('info', 'Tips', 'The function is not implemented');
        };

        $scope.emailReport = function () {
            toaster.pop('info', 'Tips', 'The function is not implemented');
        };

        $scope.previousReport = function () {
            toaster.pop('info', 'Tips', 'The function is not implemented');
        };

        $scope.nextReport = function () {
            toaster.pop('info', 'Tips', 'The function is not implemented');
        }

    }]);