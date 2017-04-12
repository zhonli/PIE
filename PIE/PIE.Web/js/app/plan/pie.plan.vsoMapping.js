angular.module('pie')
    .controller('VSOMapping',
        ['$scope', '$state', '$q', 'Plan', 'TaskLink', 'TaskSource', 'VSOWorkItem', 'VSOWorkItemLink', 'VSOUser', 'toaster', function ($scope, $state, $q, Plan, TaskLink, TaskSource, VSOWorkItem, VSOWorkItemLink, VSOUser,toaster) {
            console.log(">>VSOMapping:: planId");
            var planId = $state.params.planId;
            console.log(planId);

            $scope.plan = {};
            $scope.workItem = {};
            $scope.tfsIdentity = {};

            var defaultModel = {
                Type: "Task",
                TaskType: "Quality Task",
                Project: 'OS',
                AreaPath: 'OS\\Core\\DLUX\\Quality Engineering\\Experience Validation',
                IterationPath: 'OS\\1702',
                ProductFamily: 'Windows',
                Product: 'Internal',
                Release: 'RS2'
            }

            $scope.taskLink = {};

            $scope.actionWaiting = false;

            var initWorkItem = function (extendData) {
                $scope.workItem.Type = extendData.Type;
                $scope.workItem.TaskType = extendData.TaskType;
                initWorkItemByTaskGroup(extendData);
            };

            var initWorkItemByTaskGroup = function (extendData) {
                $scope.workItem.Project = extendData.Project;
                $scope.workItem.AreaPath = extendData.AreaPath;
                $scope.workItem.IterationPath = extendData.IterationPath;
                $scope.workItem.ProductFamily = extendData.ProductFamily;
                $scope.workItem.Product = extendData.Product;
                $scope.workItem.Release = extendData.Release;
            };

            var initWorkItemByPlan = function (plan) {
                $scope.workItem.Title = plan.Title;
                $scope.workItem.StartDate = plan.StartDate;
                $scope.workItem.DueDate = plan.EndDate;
                $scope.workItem.OriginalEstimate = plan.Workhours;
                $scope.workItem.Description = plan.Description;

                $scope.workItem.Tags = plan.Tags;
                if ($scope.workItem.Tags)
                    $scope.workItem.Tags += ';'
                $scope.workItem.Tags += "Pactera";

                if (plan.TaskLink && plan.TaskLink.TaskID) {
                    $scope.workItem.Id = plan.TaskLink.TaskID;
                }
            };

            var initWorkItemByTfsIdentity = function (tfsIdentity) {
                $scope.workItem.AssignedTo = tfsIdentity.DisplayName;
                $scope.workItem.CreatedBy = tfsIdentity.DisplayName;
            };

            var initPreferredEmail = function () {
                if (!$scope.user.identity.name) {
                    $scope.tfsIdentity.Email = "";
                    return;
                }
                var index = $scope.user.identity.name.indexOf('\\') + 1;
                var alias = $scope.user.identity.name.substring(index);
                $scope.tfsIdentity.Email = alias + "@microsoft.com";
            };

            initWorkItem(defaultModel);
            initPreferredEmail();

            var initPlan = function () {
                var promise = Plan.get({ id: planId, $expand: 'TaskLink' }).$promise.then(function (plan) {
                    console.log(">>VSOMapping:: Plan.get:: plan");
                    console.log(plan);

                    $scope.plan = plan;
                    initWorkItemByPlan(plan);

                    if (plan.TaskLink && plan.TaskLink.TaskID) {
                        $scope.taskLink = plan.TaskLink;
                        $scope.existedLink = true;

                        $scope.retriveTask(plan.TaskLink.TaskID);
                    }
                });
                return promise;
            };
            var initPlanByWorkItem = function () {

                $scope.plan.IterationPath = $scope.retrivedWorkItem.IterationPath;
                $scope.plan.IterationID = $scope.retrivedWorkItem.IterationId;

                delete $scope.plan.TaskLink;
            };

            var initVsoSource = function () {
                return TaskSource.get().$promise.then(function (response) {
                    $scope.sources = response.value;

                    if ($scope.sources && $scope.sources.length > 0) {
                        $scope.selectedSource = $scope.sources[0];
                        $scope.workItem.Project = $scope.selectedSource.Project;
                    }
                });
            };

            $q.all([initPlan(), initVsoSource()]).then(function (result) {
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
                $scope.taskLink.TaskID = $scope.workItem.Id;
                $scope.taskLink.SourceID = $scope.selectedSource.ID;
                return TaskLink.save($scope.taskLink).$promise;
            };

            var updateTaskLink = function () {
                if (!$scope.taskLink.ID)
                    throw new Error('not found task Link ID in $scope.tasklink');
                $scope.taskLink.TaskID = $scope.workItem.Id;
                $scope.taskLink.SourceID = $scope.selectedSource.ID;
                return TaskLink.update({ id: $scope.taskLink.ID }, $scope.taskLink).$promise;
            };

            var newVsoTask = function () {
                return VSOWorkItem.save({}, $scope.workItem).$promise;
            };
            //TODO: implement
            var linkVsoTaskGroup = function () {
                if (!$scope.workItem.Id) {
                    console.error("Can't find sourceId by $scope.workItem.Id");
                    return;
                }
                if (!$scope.taskGroupId){
                    console.error("Can't find targetId by $scope.taskGroupId");
                    return;
                }
                
                return VSOWorkItemLink.create('Parent', $scope.workItem.Id, $scope.taskGroupId);
            };

            var getVsoTask = function (taskId) {
                return VSOWorkItem.get({ workItemId: taskId }).$promise;
            };

            var getVsoUser = function (email) {
                return VSOUser.get({ email: email }).$promise;
            };

            var linkToVsoTask = function () {
                if (!$scope.existedLink)
                    return newTaskLink();
                else
                    return updateTaskLink();
            }

            $scope.sourceSelected = function () {
                $scope.workItem.Project = $scope.selectedSource.Project;
            };

            $scope.switchModel = function () {
                if ($scope.mode == 'NewTask') {
                    if (!$scope.retrivedTfsIdentity)
                        $scope.retriveTfsIdentity($scope.tfsIdentity.Email);
                }
            };
            //save
            $scope.submit = function () {
                $scope.actionWaiting = true;
                $scope.showFailed = false;

                if ($scope.mode == 'LinkTask') {
                    linkToVsoTask().then(function () {
                        $scope.existedLink = true;
                        toaster.pop('info', 'Tips', 'VSO Task is linked');
                    }).catch(function () {
                        $scope.showFailed = true;
                        toaster.pop('info', 'Tips', 'Linking VSO task is failed');
                    }).finally(function () {
                        $scope.actionWaiting = false;
                    });
                }
                else if ($scope.mode == 'NewTask') {
                    if ($scope.workItem.Id)
                        delete $scope.workItem.Id;
                    newVsoTask().then(function (data) {
                        toaster.pop('info', 'Tips', 'VSO task is Created');
                        $scope.workItem.Id = data.Id;

                        linkVsoTaskGroup();

                        //used to init link view
                        $scope.mode = 'LinkTask';
                        $scope.retrivedWorkItem = data;

                        linkToVsoTask().then(function () {
                            $scope.existedLink = true;
                            toaster.pop('info', 'Tips', 'VSO Task is linked');
                        }).catch(function () {
                            $scope.showFailed = true;
                            toaster.pop('info', 'Tips', 'Linking VSO task is failed');
                        }).finally(function () {
                            $scope.actionWaiting = false;
                        });

                    }).catch(function () {
                        $scope.showFailed = true;
                        toaster.pop('info', 'Tips', 'Creating VSO task is failed');
                    }).finally(function () {
                        $scope.actionWaiting = false;
                    });
                }
            };

            $scope.retriveTask = function (taskId) {

                if (!taskId || ($scope.retrivedWorkItem && taskId == $scope.retrivedWorkItem.Id)) {
                    return;
                }

                $scope.retrivedWorkItem = null;

                $scope.retriving = true;

                getVsoTask(taskId).then(function (data) {
                    $scope.retrivedWorkItem = data;
                    initPlanByWorkItem();
                    Plan.update({ id: $scope.plan.ID }, $scope.plan);
                }).catch(function (err) {

                }).finally(function () {
                    $scope.retriving = false;
                });

            };

            $scope.retriveTaskParent = function (taskParentId) {
                if (!taskParentId || ($scope.retrivedTaskParent && taskParentId == $scope.retrivedTaskParent.Id)) {
                    return;
                }

                $scope.taskGroupRetriving = true;
                getVsoTask(taskParentId).then(function (data) {
                    $scope.retrivedTaskParent = data;

                    initWorkItemByTaskGroup(data);

                }).catch(function (err) {

                }).finally(function () {
                    $scope.taskGroupRetriving = false;
                });
            };

            $scope.retriveTfsIdentity = function (email) {
                if (!email || ($scope.retrivedTfsIdentity && email == $scope.retrivedTfsIdentity.Email))
                    return;
                $scope.tfsIdentityRetriving = true;
                getVsoUser(email).then(function (data) {
                    $scope.retrivedTfsIdentity = data;
                    initWorkItemByTfsIdentity($scope.retrivedTfsIdentity);
                }).catch(function (err) {

                }).finally(function () {
                    $scope.tfsIdentityRetriving = false;
                });
            };

            $scope.taskGroupIDChanged = function () {
                console.log(">>VSOMapping:: taskGroupIDChanged");
                if (!$scope.taskGroupId)
                    $scope.retrivedTaskParent = null;
            }

            $scope.actionDisabled = function () {
                if ($scope.mode == 'NewTask') {
                    if (!($scope.workItem.Title && $scope.workItem.AreaPath && $scope.workItem.IterationPath))
                        return true;
                    if ($scope.existedLink)
                        return true;
                    if (!$scope.retrivedTfsIdentity)
                        return true;
                    if ($scope.retrivedTfsIdentity.Email != $scope.tfsIdentity.Email)
                        return true;
                    if ($scope.taskGroupRetriving)
                        return true;

                    if ($scope.taskGroupId) {
                        if (!$scope.retrivedTaskParent)
                            return true;
                        if ($scope.taskGroupId != $scope.retrivedTaskParent.Id)
                            return true;

                    }

                }
                else {
                    if ($scope.retriving)
                        return true;
                    if (!$scope.retrivedWorkItem)
                        return true;
                    if ($scope.workItem.Id != $scope.retrivedWorkItem.Id)
                        return true;
                }

                return false;
            }

            $scope.continue = function () {
                $state.go('pie.task.processes', {});
            }

        }]);