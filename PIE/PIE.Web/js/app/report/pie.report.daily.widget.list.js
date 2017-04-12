
angular.module('pie')
	.directive('rptList', function ($window) {
	    return {
	        restrict: 'AE',
	        replace: 'true',
	        scope: {
	            groupName: '@',
	            userFilter: '=filter',
	            source: '='
	        },
	        controller: ['$scope', '$filter', '$timeout', function ($scope, $filter, $timeout) {
	            $scope.alias = $filter('alias');
	            $scope.percentSign = $filter('percentSign');

	            $scope.completedWorkload = 0;
	            $scope.uncompletedWorkload = 0;

	            $scope.calcWorkload = function () {
	                if (!$scope.items || $scope.items.length == 0)
	                    return;

	                for (var i = 0; i < $scope.items.length; i++) {
	                    var item = $scope.items[i];

	                    var completed = 0;
	                    if (item.Process) {
	                        completed = item.Workhours * (item.Process.Progress / 100);
	                        $scope.completedWorkload += completed;
	                    }
	                    $scope.uncompletedWorkload += item.Workhours - completed;
	                }

	                $scope.$parent.completedWorkload += $scope.completedWorkload;
	                $scope.$parent.uncompletedWorkload += $scope.uncompletedWorkload;
	            };

	            $timeout(function () {
	                $scope.calcWorkload();
	            }, 2000)

	            $scope.color = function (item) {
	                if ((item.Process && item.Process.Status == 'Closed') || item.Status == 'Closed')
	                    return '#00B050'; //Green
	                else if (item.Status == 'Executing')
	                    return '#FFFF00'; //yellow 
	                else if (item.Status == 'Created' && new Date().DateDiff('h', item.StartDate) >= 0) {
	                    return '#00B0F0'; //lightBlue : going to execte
	                }
	                else {
	                    return 'red';
	                }
	            };

	        }],
	        templateUrl: 'tpl/report.daily.widget.list.html'
	    };
	});