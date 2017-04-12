angular.module('pie')
    .controller('PlanCreation',
        ['$scope', '$state', '$q', 'Plan', 'TestCollateral', 'toaster', function ($scope, $state, $q, Plan, TestCollateral, toaster) {
            console.log(">>>>PlanCreation");
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
            //ToDO: 
            $scope.isUpdating = function () {
                if ($scope.plan.Type == 'Execution')
                    return $scope.isPlanUpdating() && $scope.isTestColUpdating();
                else
                    return $scope.isPlanUpdating();
            };

            $scope.canContinue = function () {
                if ($scope.isUpdating() && $scope.plan.Status == 'Created')
                    return true;
                else
                    return false;
            };

            $scope.canUpdate = function () {
                //special right for owner.
                return true;
            };

            $scope.curComponent = null;

            var initPlan = function (planId) {
                Plan.get({ id: planId, $expand: 'TestCollateral' }).$promise.then(function (plan) {
                    console.log(">>PlanCreation:: Plan.get:: plan");
                    console.log(plan);

                    if (plan) {
                        if (plan.Tags)
                            $scope.Tags = plan.Tags.split(';');
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
                })
            }

            if (planId)
                initPlan(planId);

            var saveTestCollateral = function () {
                var deferred = $q.defer();
                if (!$scope.isTestColUpdating()) {
                    TestCollateral.save($scope.testCollateral).$promise.then(function (created) {
                        $scope.testCol = $scope.testCollateral;
                        deferred.resolve();
                    }).catch(function () {
                        deferred.reject();
                    })
                }
                else {
                    TestCollateral.update({ id: $scope.testCollateral.ID }, $scope.testCollateral).$promise.then(function () {
                        deferred.resolve();
                    }).catch(function () {
                        deferred.reject();
                    })
                }

                return deferred.promise;
            }

            var savePlan = function () {
                var deferred = $q.defer();

                if (!$scope.isPlanUpdating()) {
                    $scope.plan.CreateBy = alias;
                    Plan.save($scope.plan).$promise.then(function (created) {
                        console.log(">>PlanController.create.plan:: created");
                        console.log(created);

                        $scope.plan = created;
                        deferred.resolve(created);

                    }).catch(function () {
                        deferred.reject();
                    })
                }
                else {
                    $scope.plan.LastModifiedBy = alias;
                    Plan.update({ id: $scope.plan.ID }, $scope.plan).$promise.then(function (updated) {
                        console.log(">>PlanController.upate.plan :: updated");
                        console.log(updated);

                        $scope.plan = updated;

                        deferred.resolve(updated);
                    }).then(function () {
                        deferred.reject();
                    })
                }

                return deferred.promise;
            }

            var saveAll = function () {
                var deferred = $q.defer();
                
                if ($scope.Tags)
                    $scope.plan.Tags = $scope.Tags.join(";");

                console.log(">>PlanController.save:: $scope.plan");
                console.log($scope.plan);

                savePlan().then(function (data) {

                    if ($scope.plan.Type == 'NonExecution') {
                        deferred.resolve();
                        return;
                    }

                    console.log(">>PlanController.save:: $scope.testCollateral");
                    console.log($scope.testCollateral);

                    $scope.testCollateral.ID = data.ID;
                    //todo:remove
                    $scope.testCollateral.Workhours = $scope.plan.Workhours;

                    saveTestCollateral().then(function () {
                        deferred.resolve();
                    }).catch(function () {
                        deferred.reject();
                    });
                }).catch(function () {
                    deferred.reject();
                })

                return deferred.promise;
            };

            $scope.save = function () {
                if ($scope.waiting) {
                    toaster.pop('info', 'Tips', 'The operation is handling, please waiting');
                    return;
                }
                $scope.waiting = true;
                saveAll().then(function () {
                    toaster.pop('info', 'Tips', 'Saving plan is succeed');
                }).catch(function () {
                    toaster.pop('info', 'Tips', 'Saving plan is failed');
                }).finally(function () {
                    $scope.waiting = false;
                })
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
                saveAll().then(function () {
                    $state.go('pie.plan.assignment', {
                        planId: $scope.plan.ID
                    });
                });
            };
        }]);

