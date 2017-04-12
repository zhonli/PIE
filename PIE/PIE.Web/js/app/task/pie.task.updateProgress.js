angular.module('pie')
    .controller('UpdateProgress',
        ['$scope', '$state', '$q', '$timeout', '$interval', '$localStorage', 'toaster', 'Transaction', function ($scope, $state, $q, $timeout, $interval, $localStorage, toaster, Transaction) {
            console.log(">>>>UpdateProgress");
            $scope.state = {};
            $scope.progressSerial = [[0, 90]];

            $scope.curTransaction = {};
            $scope.inProgress = false;
            $scope.interval = 1;
            $scope.intervalUnit = 'd';          

            $scope.inUpdatingPhase = function () {
                var headers = {
                    'Content-Type': 'application/json', Accept: 'application/json'
                };
                var request = {
                    requestUri: serviceRoot + "Processes({ProcessID})/Transactions?$top=1&$filter=Name eq 'UpdateProgress'&$orderby=ID desc".replace('{ProcessID}', $scope.curProcess.ID),
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
                                console.log(">>UpdateProgress.is in Updating Phase, deff is " + deffValue);
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

            var getResultSummaries = function () {
                var headers = {
                    'Content-Type': 'application/json', Accept: 'application/json'
                };
                var request = {
                    requestUri: serviceRoot + "ResultStats?$expand=ResultSummary&$filter=ResultSummary/ProcessID eq {ProcessID}".replace('{ProcessID}', $scope.curProcess.ID),
                    method: 'GET',
                    headers: headers
                };
                var deferred = $q.defer();
                odatajs.oData.request(
                    request,
                    function (data, response) {
                        if (data && data.value) {
                            var resultStats = data.value;

                            return deferred.resolve(resultStats);
                        }

                    }, function () {

                    });

                return deferred.promise;
            }

            $scope.$on('processSelected', function (event, data) {
                console.log(">>UpdateProgress.processSelected::data");
                console.log(data);
                $scope.curProcess = data;

                if ($scope.curProcess.Type == 'Execution')
                    $scope.calcCompletedWorkhours();

                if (!$scope.curProcess)
                    return;
                if ($scope.curProcess.Type == 'NonExecution')
                    initProgressSerial($scope.curProcess.ID);
                else
                    initExecutionProgressSerial($scope.curProcess.ID);
            });
            var numExecutedOn = function () {
                var num = Number($scope.curProcess.StartOn.Format('yyyyMMdd'));
                return num;
            }
            var initProgressSerial = function (pid) {

                Transaction.get({ id: pid, $filter: "Name eq 'UpdateProgress'" }).$promise.then(function (data) {
                    var trans = data.value;

                    if (trans.length == 0) {
                        $scope.progressSerial = [[numExecutedOn(), 90]];
                        return;
                    }
                    var serial = [];

                    var zeroItem = [0, 0];
                    serial.push(zeroItem);

                    angular.forEach(trans, function (tr, iterator) {
                        var item = [];
                        var state = JSON.parse(tr.State);
                        if (state) {
                            item.push(Number(StringToDate(tr.ExecutedOn).Format('yyyyMMdd')) - numExecutedOn());
                            item.push(state.Progress);
                            serial.push(item);
                        }
                    });

                    console.log("UpdateProgress.initProgressSerial::get::succeed::serial");
                    console.log(serial);
                    $scope.progressSerial = serial;
                }).catch(function (ex) {
                    console.log(ex);
                })
            };
            var initExecutionProgressSerial = function (pid) {
                getResultSummaries(pid).then(function (resultStats) {
                    if (resultStats.length == 0) {
                        $scope.progressSerial = [[numExecutedOn(), 90]];
                        return;
                    }

                    var serial = [];
                    var zeroItem = [0, 0];
                    serial.push(zeroItem);

                    var statsHistory = _.groupBy(resultStats, function (rs) {
                        return Number(StringToDate(rs.StatsOn).Format('yyyyMMdd')) - numExecutedOn();
                    });

                    for (key in statsHistory) {
                        var item = [];
                        var siblingStats = statsHistory[key];

                        var completedResults = 0;
                        var cancelledResults = 0;
                        var totalResults = 0;
                        for (var i = 0; i < siblingStats.length; i++) {
                            completedResults += siblingStats[i].CompletedResults;
                            cancelledResults += siblingStats[i].CancelledResults;
                            totalResults += siblingStats[i].TotalResults;
                        }
                        var progress = 0;
                        if (totalResults > 0) {
                            progress = (completedResults + cancelledResults) / totalResults;
                            progress = (progress * 100).toFixed(2);
                        }
                        item.push(Number(key));
                        item.push(progress);
                        serial.push(item);
                    }

                    console.log("UpdateProgress.initExecutionProgressSerial::get::succeed::serial");
                    console.log(serial);
                    $scope.progressSerial = serial;
                })
            };

            if ($scope.curProcess) {
                if ($scope.curProcess.Type == 'NonExecution')
                    initProgressSerial($scope.curProcess.ID);
                else
                    initExecutionProgressSerial($scope.curProcess.ID);
            }

            var UpdateProgressSerial = function (transaction) {
                var item = [];
                item.push(Number(StringToDate(transaction.ExecutedOn).Format('yyyyMMdd')) - numExecutedOn());
                var state = JSON.parse(transaction.State);
                item.push(state.Progress);

                $scope.progressSerial.pop();
                $scope.progressSerial.push(item);
            };

            var AppendProgressSerial = function (transaction) {
                var item = [];
                item.push(Number(StringToDate(transaction.ExecutedOn).Format('yyyyMMdd')) - numExecutedOn());
                var state = JSON.parse(transaction.State);
                item.push(state.Progress);

                if ($scope.progressSerial.length == 1) {
                    var zeroItem = [0, 0];
                    $scope.progressSerial.pop();
                    $scope.progressSerial.push(zeroItem);
                }
                $scope.progressSerial.push(item);
            };

            $scope.submit = function () {
                
                console.log(">>UpdateProgress.submit");

                if (!$scope.curProcess) {
                    toaster.pop('info', 'Tips', "The selected process doesn't exist");
                    return;
                }

                $scope.state.ProcessID = $scope.curProcess.ID;

                $scope.curTransaction.ProcessID = $scope.curProcess.ID;
                $scope.curTransaction.Name = 'UpdateProgress';
                $scope.curTransaction.State = JSON.stringify({
                    ProcessID: $scope.curProcess.ID,
                    Progress: $scope.calcProgress(),
                    CompletedWorkhours: $scope.curProcess.CompletedWorkhours,
                    ActualWorkhours: $scope.curProcess.ActualWorkhours,
                    Comment: $scope.comment
                });
                $scope.inProgress = true;
                console.log($scope.curTransaction);
                $scope.inUpdatingPhase().then(function (updating) {
                    if (updating) {
                        Transaction.update({ pid: $scope.curProcess.ID, tranid: $scope.curTransaction.ID }, $scope.curTransaction).$promise.then(function (data) {
                            //TDOO: return data 
                            if (data.ExecutedOn)
                                $scope.curTransaction.ExecutedOn = data.ExecutedOn;
                            else
                                $scope.curTransaction.ExecutedOn = (new Date()).Format('yyyy-MM-dd');

                            UpdateProgressSerial(data);
                            $scope.$emit('processSelectedUpdatedEvent', $scope.curProcess);
                            $scope.$emit('progressUpdatedEvent', $scope.curTransaction);

                            toaster.pop('info', 'Tips', 'current operation succeed.');
                        }).catch(function (ex) {
                            console.log(ex);
                            toaster.pop('warning', 'Tips', 'current operation failed.');
                        }).finally(function () {
                            $scope.inProgress = false;
                        });
                    }
                    else {
                        Transaction.save({ id: $scope.curProcess.ID }, $scope.curTransaction).$promise.then(function (data) {
                            $scope.curTransaction.ID = data.ID;
                            $scope.curTransaction.ExecutedOn = data.ExecutedOn;

                            AppendProgressSerial(data);
                            
                            $scope.$emit('progressUpdatedEvent', $scope.curTransaction);
                            $scope.$emit('processSelectedUpdatedEvent', $scope.curProcess);

                            toaster.pop('info', 'Tips', 'current operation succeed.');
                        }).catch(function (ex) {
                            console.log(ex);
                            toaster.pop('warning', 'Tips', 'current operation failed.');
                        }).finally(function () {
                            $scope.inProgress = false;
                        });
                    }
                })

            };

            $scope.calcProgress = function () {
                if (!$scope.curProcess.ActualWorkhours)
                    return;
                var progress = ($scope.curProcess.CompletedWorkhours / $scope.curProcess.ActualWorkhours) * 100;

                $scope.curProcess.Progress = progress;
                return progress;
            }

            $scope.calcCompletedWorkhours = function () {
                $scope.curProcess.CompletedWorkhours = $scope.curProcess.ActualWorkhours * $scope.curProcess.Progress / 100;
            }

            $scope.calcActualWorkhours = function () {
                if (!$scope.curProcess.Progress)
                    return;
                $scope.curProcess.ActualWorkhours = $scope.curProcess.CompletedWorkhours * 100 / $scope.curProcess.Progress;
            }

        }]);
