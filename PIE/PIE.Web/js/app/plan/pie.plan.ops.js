angular.module('pie')
    .controller('PlanOps',
        ['$scope', '$state', function ($scope, $state) {
            console.log(">>>>>> PlanOps");
            $scope.curPlan = null;

            function setUI(plan) {
                $scope.del.disabled = true;
                $scope.exe.disabled = true;
                $scope.close.disabled = true;
                switch (plan.Status) {
                    default:
                    case 'Created':
                        $scope.del.disabled = false;
                        $scope.exe.disabled = false;
                        break;
                    case 'Executing':
                        $scope.close.disabled = false;
                        break;
                    case 'Cancled':
                        $scope.del.disabled = false;
                        $scope.exe.disabled = false;
                        break;
                    case 'Closed':
                        break;
                    case 'Obsoleted':
                        $scope.del.disabled = false;
                        break;
                }

                $scope.del.refresh(['disabled']);
                $scope.exe.refresh(['disabled']);
                $scope.close.refresh(['disabled']);
            }

            $scope.$on('planSelected', function (event, plan) {
                console.log(">>PlanOps.on.planSelected:: event, plan");
                console.log(event);
                console.log(plan);

                $scope.curPlan = plan;
                setUI(plan);
            });

            $scope.$on('newPlan', function (event) {
                window.location.href = '/#/pie/plan/save/';
            });

            $scope.$on('delPlan', function (event) {
                console.log('>>PlanOps.delPlan');
                if ($scope.curPlan == null)
                    return;
                $scope.$emit('planDeleteEvent', $scope.curPlan);
            });

            $scope.$on('closePlan', function (event) {
                console.log('>>PlanOps.closePlan');
                if ($scope.curPlan == null)
                    return;
                $scope.$emit('planCloseEvent', $scope.curPlan);
            });

            $scope.$on('exePlan', function (event) {
                console.log('>>PlanOps.exePlan');
                if ($scope.curPlan == null)
                    return;
                window.location.href = '/#/pie/plan/execute/{planId}'.replace("{planId}", $scope.curPlan.ID);
            });

        }]);