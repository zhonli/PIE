angular.module('pie')
    .controller('TaskDetail',
        ['$scope', '$state', 'Plan', function ($scope, $state, Plan) {
            console.log(">>>>TaskDetail");
            function init(planId) {
                Plan.get({ id: planId, $expand: 'TestCollateral' }, function (plan) {
                    console.log(">>TaskDetail:: Plan.get:: plan");
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
            if($scope.curProcess)
                init($scope.curProcess.ID);

            $scope.$on('processSelected', function (event, data) {
                console.log(">>TaskDetail.processSelected::data");
                console.log(data);

                init(data.ID);
            });

        }]);

