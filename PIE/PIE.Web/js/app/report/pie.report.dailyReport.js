angular.module('pie')
    .controller('DailyReport',
        ['$scope', '$state', '$q', '$timeout', 'ReportSource', 'BCService', function ($scope, $state, $q, $timeout, ReportSource, BCService) {
            console.log(">>>>>> DailyReport Ctrl");

            var filter = {
                userDef: '',
                calc: function () {
                    return this.userDef;
                }
            };

            var getData = function () {
                var baseFilter = function () {
                    return ' and (date(EndDate) ge date({bcSDate}) and date(EndDate) le date({bcEDate}) )'.replace('{bcSDate}', BCService.curBC.startDate).replace('{bcEDate}', BCService.curBC.endDate);
                };
                var headers = {
                    'Content-Type': 'application/json', Accept: 'application/json'
                };
                var request = {
                    requestUri: serviceRoot + "Plans?$expand=Process($expand=ResultSummaries($expand=ResultStats)),TaskLink&$filter=(Status ne PIEM.Common.Model.PlanStatus'Obsoleted') and CreateBy ne 'FAREAST\\v-jigao'" + baseFilter() + filter.calc(),
                    method: 'GET',
                    headers: headers
                };

                var deferred = $q.defer();
                odatajs.oData.request(
                    request,
                    function (data, response) {
                        return deferred.resolve(data.value);
                    }, function () {

                    });

                return deferred.promise;
            }

            var init = function () {
                getData().then(function (data) {

                    var allItems = data;
                    var totalWorkload = 0;
                    var completedWorkload = 0;

                    var closedItems = [];
                    var noneClosedItems = [];

                    angular.forEach(allItems, function (item) {
                        if (item.TaskLink && item.TaskLink.TaskID) {
                            totalWorkload += item.Workhours;
                            if (item.Status == 'Closed') {
                                closedItems.push(item);
                                completedWorkload += item.Workhours;
                            }
                            else {
                                noneClosedItems.push(item);
                            }
                        }
                    });
                    console.log(">>DailyReport.init.getData.succeed::closedItems,noneClosedItems");
                    $scope.closedItems = closedItems;
                    $scope.noneClosedItems = noneClosedItems;

                    console.log($scope.closedItems);
                    console.log($scope.noneClosedItems);

                    $scope.totalWorkload = totalWorkload;
                    $scope.completedWorkload = 0;
                    $scope.uncompletedWorkload = 0;
                });
            }

            $scope.$on('filterChangedEvent', function (event, newFilter) {
                newFilter = newFilter || '';
                console.log(">>DailyReport.filterChanged:: event, newFilter");

                filter.userDef = ' ';
                filter.userDef += newFilter;
                init();
            });

            

        }]);