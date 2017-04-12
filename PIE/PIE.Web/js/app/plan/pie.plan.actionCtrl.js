angular.module('pie')
    .controller('PlanActionCtrl',
        ['$scope', '$state', 'BacklogSource', 'Plan', 'toaster', function ($scope, $state, BacklogSource, Plan, toaster) {
            console.log(">>>>>> PlanActionCtrl");
            $scope.curPlan = null;

            var def = function () {
                $scope.hideNewPlan = false;
                $scope.hideDetailPlan = true;
                $scope.hideDeletePlan = true;
                $scope.hideAssignPlan = true;
                $scope.hideMapVSOTask = true;
                $scope.hideExecutePlan = true;
                $scope.hideClosePlan = true;
            };
            def();

            function showActionsByStatus() {

                switch ($scope.curPlan.Status) {
                    default:
                    case 'Created':
                        $scope.hideDeletePlan = false;
                        $scope.hideAssignPlan = false;
                        $scope.hideExecutePlan = false;

                        break;
                    case 'Executing':
                        $scope.hideClosePlan = false;
                        break;
                    case 'Cancled':
                        break;
                    case 'Closed':
                        break;
                    case 'Obsoleted':
                        break;
                }
                
            }

            function showActionsByOwner() {
                $scope.hideDeletePlan = true;
                $scope.hideAssignPlan = true;
                if ($scope.curPlan.CreateBy == $scope.user.identity.name) {
                    $scope.hideDeletePlan = false;
                    $scope.hideAssignPlan = false;
                }
            }

            $scope.$on('planSelected', function (event, plan) {
                console.log(">>PlanOps.on.planSelected:: event, plan");
                console.log(event);
                console.log(plan);

                def();
                if (plan) {
                    $scope.hideDetailPlan = false;
                    $scope.hideMapVSOTask = false;
                }

                $scope.curPlan = plan;

                showActionsByStatus();
                showActionsByOwner();
            });

            $scope.newPlan = function () {
                $state.go('pie.plan.save', {
                    planId: null
                });
            };

            $scope.detailPlan = function () {
                $state.go('pie.plan.detail', {
                    planId: $scope.curPlan.ID
                });
            };

            $scope.deletePlan = function () {
                console.log(">>PlanActionCtrl.deletePlan");

                if ($scope.curPlan == null)
                    return;
                BacklogSource.deletePlan($scope.curPlan.ID, function () {
                    $scope.$emit('planDeletedEvent', $scope.curPlan);
                })

            };

            $scope.mapVSOTask = function () {
                $state.go('pie.plan.vsoMapping', {
                    planId: $scope.curPlan.ID
                });
            };

            $scope.assignPlan = function () {
                $state.go('pie.plan.assignment', {
                    planId: $scope.curPlan.ID
                });
            };

            $scope.goToExecutePlan = function () {
                if ($scope.actionWaiting) {
                    toaster.pop('info', 'Tips', 'The operation is handling, please waiting');
                    return;
                }
                if (!$scope.curPlan.TaskLink) {
                    toaster.pop('info', 'Tips', 'please confirm there is a vso task linked');
                    return;
                }

                if ($scope.curPlan.Type == 'Execution') {
                    $state.go('pie.plan.execute', {
                        planId: $scope.curPlan.ID
                    });
                }
                else {
                    $scope.actionWaiting = true;
                    Plan.executeVoid({ id: $scope.curPlan.ID }).$promise.then(function () {
                        $scope.curPlan.Status = 'Executing';
                        $scope.$emit('planExecutedEvent', $scope.curPlan);
                    }).finally(function () {
                        $scope.actionWaiting = false;
                    });
                }
            };

            $scope.closePlan = function () {
                console.log(">>PlanActionCtrl.closePlan");

                if ($scope.curPlan == null)
                    return;

                BacklogSource.closePlan($scope.curPlan, function () {
                    $scope.$emit('planClosedEvent', $scope.curPlan);
                })

            };

            $scope.getDataUri = function () {
                console.log(">>ProcessesActionCtrl.getDataUri");
                toaster.pop('info', 'Tips', 'The function is not implemented');
            };


            //save vso mapping

            $scope.saveMapping = function () {
                toaster.pop('info', 'Tips', 'The function is not linked');
            };

            //VSO task creation
            $scope.savePlan = function () {
                toaster.pop('info', 'Tips', 'The function is not linked');
            };

            $scope.addMore = function () {
                toaster.pop('info', 'Tips', 'The function is not implemented');
            };

            $scope.nextStep = function () {
                toaster.pop('info', 'Tips', 'The function is not linked');
            };

            // query module
            $scope.newQuery = function () {
                toaster.pop('info', 'Tips', 'The function is not implemented');
            };

            // plan execution
            $scope.executePlan = function () {
                toaster.pop('info', 'Tips', 'The function is not linked');
            };


        }]);