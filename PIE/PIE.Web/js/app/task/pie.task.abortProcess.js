﻿angular.module('pie')
    .controller('AbortProcess',
        ['$scope', '$state', '$timeout', '$interval', '$q', 'Transaction', function ($scope, $state, $timeout, $interval, $q, Transaction) {
            console.log(">>>>AbortProcess");
            $scope.state = {};
            $scope.curTransaction = {};

            $scope.interval = 20;
            $scope.intervalUnit = 'n';

            $scope.inUpdatingPhase = function () {
                var headers = {
                    'Content-Type': 'application/json', Accept: 'application/json'
                };
                var request = {
                    requestUri: serviceRoot + "Processes({ProcessID})/Transactions?$top=1&$filter=Name eq 'AbortProcess'&$orderby=ID desc".replace('{ProcessID}', $scope.curProcess.ID),
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
                                console.log(">>AbortProcess.is in Updating Phase, deff is " + deffValue);
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
                console.log(">>AbortProcess.processSelected::data");
                console.log(data);
                $scope.curProcess = data;
            });


            $scope.submit = function () {
                if (!$scope.curProcess) {
                    //TDDO: messsage Warning
                    return;
                }

                if ($scope.curProcess.Status != 'Running' && $scope.curProcess.Status != 'Blocked') {
                    //TDDO: messsage Warning
                    return;
                }

                $scope.inProgress = true;
                console.log(">>AbortProcess.submit");
                if ($scope.curProcess)
                    $scope.state.ProcessID = $scope.curProcess.ID;
                else {
                    console.log(">>AbortProcess.submit::return: curProcess is null");
                    return;
                }

                $scope.curTransaction.ProcessID = $scope.curProcess.ID;
                $scope.curTransaction.Name = 'AbortProcess';
                $scope.curTransaction.State = JSON.stringify({
                    ProcessID: $scope.curProcess.ID,
                    Comment: $scope.comment
                });

                console.log($scope.curTransaction);
                $scope.inUpdatingPhase().then(function (updating) {
                    if (updating) {
                        Transaction.update({ pid: $scope.curProcess.ID, tranid: $scope.curTransaction.ID }, $scope.curTransaction).$promise.then(function (data) {
                            $scope.curTransaction.ExecutedOn = data.ExecutedOn;

                            $scope.curProcess.Status = 'Cutted';
                            $scope.$emit('processCuttedEvent');
                            $scope.$emit('processSelectedUpdatedEvent', $scope.curProcess);
                            

                        }).catch(function (ex) {
                            console.log(ex);
                        }).finally(function () {
                            $scope.inProgress = false;
                        });
                    }
                    else {
                        Transaction.save({ id: $scope.curProcess.ID }, $scope.curTransaction).$promise.then(function (data) {
                            $scope.curTransaction.ID = data.ID;
                            $scope.curTransaction.ExecutedOn = data.ExecutedOn;

                            $scope.curProcess.Status = 'Cutted';
                            $scope.$emit('processCuttedEvent');
                            $scope.$emit('processSelectedUpdatedEvent', $scope.curProcess);
                            

                        }).catch(function (ex) {
                            console.log(ex);
                        }).finally(function () {
                            $scope.inProgress = false;
                        });
                    }
                })

            };

        }]);
