
angular.module('pie')
    .controller('PlanCreationAdmin',
        ['$scope', '$state', 'Plan', 'TestCollateral', function ($scope, $state, Plan, TestCollateral) {
            console.log(">>>>PlanCreationAdmin");
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
                //special right for owner.
                return true;
            }

            $scope.curComponent = null;

            if (planId) {
                Plan.get({ id: planId, $expand: 'TestCollateral' }, function (plan) {
                    console.log(">>PlanCreation:: Plan.get:: plan");
                    console.log(plan);

                    if (plan) {
                        if (plan.Tags)
                            $scope.Tags = plan.Tags.split(',');
                        $scope.plan = plan;
                        if (plan.ComponentID)
                            $scope.curComponent = { id: plan.ComponentID, name: plan.Component };

                        console.log(">>PlanCreation:: Plan.loaded");
                    }

                    if (plan.TestCollateral) {
                        $scope.testCollateral = angular.copy(plan.TestCollateral);
                        $scope.testCol = $scope.testCollateral;
                        delete plan.TestCollateral;
                    }
                });
            }


            $scope.save = function (callback) {

                $scope.plan.CreateBy = alias;
                if($scope.Tags)
                    $scope.plan.Tags = $scope.Tags.join();

                console.log(">>PlanController.save:: $scope.plan");
                console.log($scope.plan);

                $scope.waiting = false;
                (function savePlan() {
                    $scope.waiting = true;
                    var saveTestCollateral = function () {
                        if (!$scope.isTestColUpdating()) {
                            TestCollateral.save($scope.testCollateral,
                                function (created) {
                                    $scope.testCol = $scope.testCollateral;
                                    $scope.waiting = false;
                                    if (callback)
                                        callback();
                                },
                                function (httpResponse) {
                                    console.log(">>PlanController.create.plan :: TestCollateral.save :: error callback:: httpResponse");
                                    console.log(httpResponse);
                                    $scope.waiting = false;
                                });
                        }
                        else {
                            TestCollateral.update({ id: $scope.testCollateral.ID }, $scope.testCollateral,
                                    function (updated) {
                                        $scope.waiting = false;
                                        if (callback)
                                            callback();

                                    }, function (httpResponse) {
                                        console.log(">>PlanController.update.plan :: TestCollateral.save :: error callback:: httpResponse");
                                        console.log(httpResponse);
                                        $scope.waiting = false;
                                    });
                        }
                    }

                    if (!$scope.isPlanUpdating()) {
                        //$scope.plan.SprintID = curSprintId;
                        Plan.save($scope.plan,
                            function (created) {
                                console.log(">>PlanController.create.plan:: created");
                                console.log(created);
                                $scope.plan.ID = created.ID;
                                $scope.plan.Status = created.Status;

                                //bindGant();

                                if ($scope.plan.Type == 'NonExecution') {
                                    $scope.waiting = false;
                                    if (callback)
                                        callback();
                                    return;
                                }

                                console.log(">>PlanController.save:: $scope.testCollateral");
                                console.log($scope.testCollateral);

                                $scope.testCollateral.ID = created.ID;
                                //todo:remove
                                $scope.testCollateral.Workhours = $scope.plan.Workhours;

                                saveTestCollateral();

                            }, function (httpResponse) {
                                console.log(">>PlanController.create.plan :: error callback:: httpResponse");
                                console.log(httpResponse);
                                $scope.waiting = false;
                            });
                    }
                    else {
                        Plan.update({ id: $scope.plan.ID }, $scope.plan,
                            function (updated) {
                                console.log(">>PlanController.upate.plan :: updated");
                                console.log(updated);

                                //bindGant();

                                if ($scope.plan.Type == 'NonExecution') {
                                    $scope.waiting = false;
                                    if (callback)
                                        callback();
                                    return;
                                }
                                console.log(">>PlanController.save:: $scope.testCollateral");
                                console.log($scope.testCollateral);

                                $scope.testCollateral.ID = updated.ID;
                                //todo:remove
                                $scope.testCollateral.Workhours = $scope.plan.Workhours;
                                saveTestCollateral();

                            }, function (httpResponse) {
                                console.log(">>PlanController.update.plan :: error callback:: httpResponse");
                                console.log(httpResponse);
                                $scope.waiting = false;
                            });
                    }
                })();
            };


            $scope.$on('productSelected', function (event, data) {
                console.log(">>PlanCreation.productSelected :: event, data");
                console.log(event);
                console.log(data);

                $scope.plan.ComponentID = data.cid;
                $scope.plan.Component = data.product;
                $scope.plan.ComponentPath = data.path;
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

