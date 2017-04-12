angular.module('pie')
    .controller('PlanQueryCtrl',
        ['$scope', '$state', 'BacklogSource', function ($scope, $state, BacklogSource) {
            console.log(">>>>>> PlanViewsCtrl :: bcdate");

            var pageFilter = function () {
                return "and CreateBy eq '{CreateBy}'".replace("{CreateBy}", alias);
            }

            var filter = {
                userDef: '',
                calc: function () {
                    return pageFilter() + this.userDef;
                }
            };

            $scope.aptPlans = BacklogSource.newSource(filter);

            $scope.$on('tabclick', function (event, arguments) {
                console.log(">>jqxTabs.tabclick::event");
                console.log(event);
                console.log(arguments);
            });

            $scope.$on('searched', function (event, search) {
                console.log(">>PlanViewsCtrl.on.filtered:: event, search");
                console.log(event);
                console.log(search);
                filter.userDef = '';
                filter.userDef += search;
                $scope.aptPlans.dataBind();
            });

        }]);