angular.module('pie')
    .controller('PlanActionCtrlAdmin',
        ['$scope', '$state', 'BacklogSource', 'Plan', 'toaster', function ($scope, $state, BacklogSource, Plan, toaster) {
            console.log(">>>>>> PlanActionCtrlAdmin");
            $scope.curPlan = null;

            var def = function () {
                $scope.hideNewPlan = false;
                $scope.hideDetailPlan = true;
                $scope.hideDeletePlan = true;
                $scope.hideAssignPlan = true;
                $scope.hideExecutePlan = true;
                $scope.hideClosePlan = true;
            };
            def();


            function showActions(plan) {

                switch (plan.Status) {
                    default:
                    case 'Created':
                        $scope.hideDeletePlan = false;
                        $scope.hideAssignPlan = false;
                        $scope.hideExecutePlan = false;
                        break;
                    case 'Executing':
                        $scope.hideDeletePlan = false;
                        $scope.hideAssignPlan = false;
                        $scope.hideClosePlan = false;
                        break;
                    case 'Cancled':
                        break;
                    case 'Closed':
                        $scope.hideDeletePlan = false;
                        $scope.hideAssignPlan = false;
                        break;
                    case 'Obsoleted':
                        break;
                }

            }

            $scope.$on('planSelected', function (event, plan) {
                console.log(">>PlanOps.on.planSelected:: event, plan");
                console.log(event);
                console.log(plan);

                def();
                if (plan)
                    $scope.hideDetailPlan = false;

                $scope.curPlan = plan;

                showActions(plan);
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

            $scope.assignPlan = function () {
                $state.go('pie.plan.assignment', {
                    planId: $scope.curPlan.ID
                });
            };

            $scope.mapVSOTask = function () {
                $state.go('pie.plan.vsoMapping', {
                    planId: $scope.curPlan.ID
                });
            };

            $scope.executePlan = function () {
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

        }]);