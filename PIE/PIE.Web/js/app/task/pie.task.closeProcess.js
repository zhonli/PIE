angular.module('pie')
    .controller('CloseProcess',
        ['$scope', '$state', '$timeout', '$interval', '$q', 'toaster', 'Transaction', function ($scope, $state, $timeout, $interval, $q, toaster, Transaction) {
            console.log(">>>>CloseProcess");
            $scope.state = {};
            $scope.curTransaction = {};

            
            $scope.interval = 1;
            $scope.intervalUnit = 'd';
            
            $scope.inUpdatingPhase = function () {
                var headers = {
                    'Content-Type': 'application/json', Accept: 'application/json'
                };
                var request = {
                    requestUri: serviceRoot + "Processes({ProcessID})/Transactions?$top=1&$filter=Name eq 'CloseProcess'&$orderby=ID desc".replace('{ProcessID}', $scope.curProcess.ID),
                    method: 'GET',
                    headers: headers
                };

                var deferred = $q.defer();
                odatajs.oData.request(
                    request,
                    function (data, response) {
                        var updating = false;
                        if (data && data.value && data.value[0] && data.value[0].ExecutedOn) {
                            var lastExecutedOn = StringToDate(data.value[0].ExecutedOn);
                            var deffValue = lastExecutedOn.DateDiff($scope.intervalUnit, new Date());

                            if (deffValue <= $scope.interval) {
                                console.log(">>CloseProcess.is in Updating Phase, deff is " + deffValue);
                                updating = true;
                                $scope.curTransaction.ID = data.value[0].ID;
                            }
                        }
                        return deferred.resolve(updating);
                    }, function () {
                        return deferred.resolve(false);
                    });

                return deferred.promise;
            }
            

            $scope.$on('processSelected', function (event, data) {
                console.log(">>CloseProcess.processSelected::data");
                console.log(data);
                $scope.curProcess = data;
            });


            $scope.submit = function () {
                if ($scope.actionWaiting) {
                    toaster.pop('info', 'Tips', 'The operation is handling, Please waiting');
                    return;
                }
                
                if (!$scope.curProcess) {
                    toaster.pop('info', 'Tips', "The selected process doesn't exist");
                    return;
                }

                if ($scope.curProcess.Progress != 100) {
                    toaster.pop('info', 'Tips', 'The progress of current process is not 100%');
                    return;
                }

                if ($scope.curProcess.Status != 'Running' && $scope.curProcess.Status != 'Blocked') {
                    toaster.pop('info', 'Tips', 'current process is not running or blocked');
                    return;
                }
                
                
                console.log(">>CloseProcess.submit");
                $scope.state.ProcessID = $scope.curProcess.ID;

                $scope.curTransaction.ProcessID = $scope.curProcess.ID;
                $scope.curTransaction.Name = 'CloseProcess';
                $scope.curTransaction.State = JSON.stringify({
                    ProcessID: $scope.curProcess.ID,
                    ActualWorkhours: $scope.curProcess.ActualWorkhours,
                    EndOn: $scope.curProcess.EndOn,
                    Comment: $scope.comment
                });
                $scope.actionWaiting = true;
                console.log($scope.curTransaction);
                $scope.inUpdatingPhase().then(function (updating) {
                    if (updating) {
                        Transaction.update({ pid: $scope.curProcess.ID, tranid: $scope.curTransaction.ID }, $scope.curTransaction).$promise.then(function (data) {
                            $scope.curTransaction.ExecutedOn = data.ExecutedOn;

                            $scope.curProcess.Status = 'Closed';
                            $scope.$emit('processClosedEvent');
                            //why $scope.curProcess is not null
                            //$scope.$emit('processSelectedUpdatedEvent', $scope.curProcess);
                            
                            toaster.pop('info', 'Tips', 'current operation succeed.');
                        }).catch(function (ex) {
                            console.log(ex);
                            toaster.pop('warning', 'Tips', 'current operation failed.');
                        }).finally(function () {
                            $scope.actionWaiting = false;
                        });
                    }
                    else {
                        Transaction.save({ id: $scope.curProcess.ID }, $scope.curTransaction).$promise.then(function (data) {
                            $scope.curTransaction.ID = data.ID;
                            $scope.curTransaction.ExecutedOn = data.ExecutedOn;

                            $scope.curProcess.Status = 'Closed';
                            $scope.$emit('processClosedEvent');
                            //why $scope.curProcess is not null
                            //$scope.$emit('processSelectedUpdatedEvent', $scope.curProcess);
                            
                            toaster.pop('info', 'Tips', 'current operation succeed.');
                        }).catch(function (ex) {
                            console.log(ex);
                            toaster.pop('warning', 'Tips', 'current operation failed.');
                        }).finally(function () {
                            $scope.actionWaiting = false;
                        });
                    }
                })

            };

        }]);
