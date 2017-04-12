angular.module('pie')
    .controller('PlanExecution',
        ['$scope', '$state', '$q', 'Plan', 'TaskLink', 'TaskSource', function ($scope, $state, $q, Plan, TaskLink, TaskSource) {
            console.log(">>ExecutePlan:: planId");
            var planId = $state.params.planId;
            console.log(planId);

            $scope.plan = {};
            $scope.rc = {};
            $scope.rc.selections = [];

            $scope.actionWaiting = false;

            var initPlan = function () {
                var promise = Plan.get({ id: planId, $expand: 'TaskLink' }).$promise.then(function (plan) {
                    console.log(">>ExecutePlan:: Plan.get:: plan");
                    console.log(plan);

                    $scope.plan = plan;

                    if (plan.TaskLink && plan.TaskLink.TaskID) {
                        $scope.retriveRelatedInfo(plan.TaskLink.TaskID);
                    }
                });
                return promise;
            };

            var initWttSource = function () {

                var deferred = $q.defer();

                setTimeout(function () {
                    $scope.wttSources = [{ ID: 2, FriendlyName: '1Windows@ATLASIdentity.redmond.corp.microsoft.com/WTTIdentity' }];

                    if ($scope.wttSources && $scope.wttSources.length > 0)
                        $scope.selectedWttSource = $scope.wttSources[0];
                    return deferred.resolve();
                }, 100);

                return deferred.promise;
            };

            $scope.rcSelectorIsDisabled = function () {
                var disabled = true;
                if ($scope.wttMode == 'LinkRC' && $scope.taskRelatedRCs && $scope.selectedWttSource) {
                    disabled = false;
                }
                return disabled;
            }
            initPlan();
            initWttSource();

            var getRCs = function (taskId) {
                var deferred = $q.defer();

                var headers = {
                    'Content-Type': 'application/json', Accept: 'application/json'
                };
                var request = {
                    requestUri: serviceRoot + 'GetWttRCs(TaskID={TaskID})'.replace('{TaskID}', taskId),
                    method: 'GET',
                    headers: headers
                };

                odatajs.oData.request(
                    request,
                    function (data, response) {
                        deferred.resolve(data.value);
                    });

                return deferred.promise;
            };

            var getRC = function (rcId) {
                var deferred = $q.defer();

                var headers = {
                    'Content-Type': 'application/json', Accept: 'application/json'
                };
                var request = {
                    requestUri: serviceRoot + 'GetWttRC(RCID={rcId})'.replace('{rcId}', rcId),
                    method: 'GET',
                    headers: headers
                };

                odatajs.oData.request(
                    request,
                    function (data, response) {
                        deferred.resolve(data);
                    });

                return deferred.promise;
            };

            var executePlan = function () {
                var tran = { sourceId: $scope.selectedWttSource.ID, resultSummaries: JSON.stringify($scope.rc.selections) };
                return Plan.execute({ id: planId }, tran).$promise;
            };

            $scope.execute = function () {
                $scope.actionWaiting = true;
                executePlan().then(function () {
                    $scope.showNext = true;
                }).catch(function () {
                    $scope.showExeFailed = true;
                }).finally(function () {
                    $scope.actionWaiting = false;
                });
            };

            $scope.appendingRC = {};
            $scope.appendRC = function (rcId) {
                if (!rcId || ($scope.appendedRC && rcId == $scope.appendedRC.ID)) {
                    return;
                }
                $scope.appending = true;
                $scope.appendedRC = null;
                $scope.appendingError = '';

                getRC(rcId).then(function (retivedRC) {
                    if (retivedRC && retivedRC.Name) {
                        $scope.appendedRC = retivedRC;

                        var isExisted = false;
                        for (var i = 0; i < $scope.taskRelatedRCs.length; i++) {
                            if ($scope.taskRelatedRCs[i] == $scope.appendedRC.ID) {
                                isExisted = true;
                                $scope.taskRelatedRCs[i] = $scope.appendedRC;
                                break;
                            }
                        }
                        if (!isExisted)
                            $scope.taskRelatedRCs.push($scope.appendedRC);
                    }
                    else {
                        $scope.appendingError = "can't retrived rc with ID: " + rcId;
                    }
                }).catch(function (err) {
                    $scope.appendingError = 'existed exception when retriving rc';
                }).finally(function () {
                    $scope.appending = false;
                });
            };

            $scope.retriveRelatedInfo = function (taskId) {
                if (!taskId) {
                    console.error('taskId is null or undefined');
                    return;
                }

                $scope.rc.selections = [];
                //TODO: refresh rc selector.

                getRCs(taskId).then(function (relatedRCs) {
                    $scope.taskRelatedRCs = relatedRCs;
                    if ($scope.taskRelatedRCs.length > 0)
                        $scope.existedRCs = true;
                    else
                        $scope.existedRCs = false;
                }).catch(function (err) {

                }).finally(function () {

                });
            };

            $scope.continue = function () {
                $state.go('pie.task.processes', {});
            }

        }]);