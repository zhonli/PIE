angular.module('pie')
    .controller('PlanExecution2',
        ['$scope', '$state', '$q', 'Plan', 'TaskLink', 'TaskSource', function ($scope, $state, $q, Plan, TaskLink, TaskSource) {
            console.log(">>ExecutePlan:: planId");
            var planId = $state.params.planId;
            console.log(planId);

            $scope.plan = {};
            $scope.task = {};
            $scope.taskLink = {};
            $scope.rc = {};
            //$scope.rc.selections = [];

            $scope.actionWaiting = false;

            var initPlan = function () {
                var promise = Plan.get({ id: planId, $expand: 'TaskLink' }).$promise.then(function (plan) {
                    console.log(">>ExecutePlan:: Plan.get:: plan");
                    console.log(plan);

                    $scope.plan = plan;

                    $scope.task.Title = plan.Title;
                    if (plan.TaskLink && plan.TaskLink.TaskID) {
                        $scope.taskLink = plan.TaskLink;
                        $scope.existedLink = true;
                        $scope.task.ID = plan.TaskLink.TaskID;
                        $scope.retriveRelatedInfo($scope.task.ID);
                    }
                });
                return promise;
            };

            var initVsoSource = function () {
                return TaskSource.get().$promise.then(function (response) {
                    $scope.sources = response.value;

                    if ($scope.sources && $scope.sources.length > 0)
                        $scope.selectedSource = $scope.sources[0];
                });
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

            initWttSource();
            $q.all([initPlan(), initVsoSource()])
            .then(function (result) {
                if (!$scope.taskLink.SourceID)
                    return;
                var match = _.find($scope.sources, function (src) {
                    src.ID == $scope.taskLink.SourceID;
                });
                if (match)
                    $scope.selectedSource = match;
            });

            var newTaskLink = function () {
                $scope.taskLink.ID = $scope.plan.ID;
                $scope.taskLink.TaskID = $scope.task.ID;
                $scope.taskLink.SourceID = $scope.selectedSource.ID;
                return TaskLink.save($scope.taskLink).$promise;
            };

            var updateTaskLink = function () {
                if (!$scope.taskLink.ID)
                    throw new Error('not found task Link ID in $scope.tasklink');
                $scope.taskLink.TaskID = $scope.task.ID;
                $scope.taskLink.SourceID = $scope.selectedSource.ID;
                return TaskLink.update({ id: $scope.taskLink.ID }, $scope.taskLink).$promise;
            };

            var newVsoTask = function () {
                var deferred = $q.defer();

                setTimeout(function () {
                    var retTask = { TaskID: 2341, Title: 'Demo A Task' };
                    deferred.resolve(retTask);
                }, 1000);

                return deferred.promise;
            };

            var getVsoTask = function (taskId) {
                var deferred = $q.defer();

                var headers = {
                    'Content-Type': 'application/json', Accept: 'application/json'
                };
                var request = {
                    requestUri: serviceRoot + 'GetVSOTask(TaskID={TaskID})'.replace('{TaskID}', taskId),
                    method: 'GET',
                    headers: headers
                };

                var resultSummaries = [];
                odatajs.oData.request(
                    request,
                    function (data, response) {
                        var vsoTask = { TaskID: data.Id, Title: data.Title };
                        deferred.resolve(vsoTask);
                    });

                return deferred.promise;
            };

            var linkToVsoTask = function () {
                if (!$scope.existedLink)
                    return newTaskLink();
                else
                    return updateTaskLink();
            }

            var publishToVsoTask = function () {
                newVsoTask().then(function () {
                    return linkToVsoTask();
                });
            };

            var executePlan = function () {
                return Plan.execute2({ id: planId }).$promise;
            };

            $scope.submit = function () {
                $scope.actionWaiting = true;
                $scope.showFailed = false;

                if ($scope.mode == 'LinkTask') {
                    linkToVsoTask().then(function () {
                        $scope.existedLink = true;
                    }).catch(function () {
                        $scope.showFailed = true;
                    }).finally(function () {
                        $scope.actionWaiting = false;
                    });
                }
                else if ($scope.mode == 'NewTask') {
                    publishToVsoTask().then(function () {
                        $scope.existedLink = true;
                    }).catch(function () {
                        $scope.showFailed = true;
                    }).finally(function () {
                        $scope.actionWaiting = false;
                    });
                }
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

            
            $scope.retriveRelatedInfo = function (taskId) {
                if (!taskId || ($scope.retrivedTask && taskId == $scope.retrivedTask.TaskID)) {
                    return;
                }

                $scope.retrivedTask = null;
                $scope.taskRelatedRCs = null;
                $scope.rc.selections = [];
                //TODO: refresh rc selector.

                $scope.retriving = true;
                getVsoTask(taskId).then(function (vsoTask) {
                    $scope.retrivedTask = vsoTask;
                }).catch(function (err) {

                }).finally(function(){
                    $scope.retriving = false;
                });

        
            };

            $scope.continue = function () {
                $state.go('pie.task.processes', {});
            }

        }]);