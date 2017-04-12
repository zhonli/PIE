var curSprintId = $("#hidCurSprint").val();
angular.module('pie')
    .controller('PlanDetail',
        ['$scope', '$state', 'Plan', 'TestCollateral', 'uiLoad', function ($scope, $state, Plan, TestCollateral, uiLoad) {
            var planId = $state.params.planId;

            $scope.isPlanUpdating = function () {
                if ($scope.plan.ID) {
                    return true;
                }
                else {
                    return false;
                }
            };
            $scope.isTestColUpdating = function () {
                if ($scope.testCol)
                    return true;
                else
                    return false;
            };


            $scope.isUpdating = function () {
                if ($scope.plan.Type == 'Execution')
                    return $scope.isPlanUpdating() && $scope.isTestColUpdating();
                else
                    return $scope.isPlanUpdating();
            }

            $scope.canContinue = function () {
                if ($scope.isUpdating() && $scope.plan.Status == 'Created')
                    return true;
                else
                    return false;
            };

            $scope.canUpdate = function () {
                if ($scope.plan.Status == 'Executing')
                    return false;
                else
                    return true;
            }

            if (planId) {
                Plan.get({ id: planId, $expand: 'TestCollateral' }, function (plan) {
                    console.log(">>UpdatePlan:: Plan.get:: plan");
                    console.log(plan);

                    if (plan) {
                        $scope.plan = plan;

                        if (plan.Tags)
                            $scope.Tags = plan.Tags.split(',');
                    }

                    if (plan.TestCollateral) {
                        $scope.testCollateral = angular.copy(plan.TestCollateral);
                        $scope.testCol = $scope.testCollateral;
                        delete plan.TestCollateral;
                    }
                });
            }

            uiLoad.loadScript('../js/app/plan/libs/pie.plan.assignment.list.js').
                then(function () {
                    assignment(planId);
                }).catch(function () {

                });

            $scope.continue = function () {
                function next() {
                    $state.go('pie.plan.assignment', {
                        planId: $scope.plan.ID
                    });
                }
                $scope.save(next);

            };
        }]);

