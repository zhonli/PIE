angular.module('pie')
    .controller('PlanBacklogsCtrl',
        ['$scope', '$state', 'BCService', 'BacklogSource', function ($scope, $state, BCService, BacklogSource) {
            console.log(">>>>>> PlanBacklogsCtrl :: bcId");
            
            console.log($state.params.bcId);
            var bc = BCService.getBCByRef($state.params.bcId);

            var pageFilter = function () {
                return 'and (date(EndDate) ge date({bcSDate}) and date(EndDate) lt date({bcEDate}) )'.replace('{bcSDate}', bc.startDate).replace('{bcEDate}', bc.endDate);
            };

            $scope.filter = {
                userDef: '',
                dataUri: '',
                calc: function () {
                    return pageFilter() + this.userDef;
                }
            };

            $scope.aptPlans = BacklogSource.newSource($scope.filter);

            $scope.$on('tabclick', function (event, arguments) {
                console.log(">>jqxTabs.tabclick::event");
                console.log(event);
                console.log(arguments);
            });

            $scope.$on('searched', function (event, search) {
                console.log(">>PlanBacklogsCtrl.on.filtered:: event, search");
                console.log(event);
                console.log(search);
                $scope.filter.userDef = '';
                $scope.filter.userDef += search;
                $scope.aptPlans.dataBind();
                console.log($scope.filter);
            });

        }]);