angular.module('pie')
    .controller('PlanAppCtrl',
        ['$scope', '$state', function ($scope, $state) {
            console.log(">>>>>> PlanAppCtrl");

            $scope.$on('searchedEvent', function (event, data) {
                $scope.$broadcast('searched', data);
            })

            
        }]);