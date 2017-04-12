angular.module('pie')
    .controller('PlanAssignment',
        ['$scope', '$state', 'Plan', 'uiLoad', function ($scope, $state, Plan, uiLoad) {
            console.log(">>>>>>AssignResources ctrl :: planId");
            var planId = $state.params.planId;
            console.log(planId);
            $scope.curPlanId = planId;

            Plan.get({ id: planId }, function (plan) {
                console.log(">>ExecutePlan:: Plan.get:: plan");
                console.log(plan);
            });

            uiLoad.loadScript('../js/app/plan/libs/pie.plan.assignment.ladu.js').
                then(function () {
                    assignment(planId);
                }).catch(function () {

                });

            $scope.continue = function () {
                $state.go('pie.plan.execute', { planId: planId });
            };
        }]);