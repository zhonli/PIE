angular.module('pie')
    .controller('ProcessesActionCtrl',
        ['$scope', '$state', 'ProcessSource', 'toaster', function ($scope, $state, ProcessSource, toaster) {
            console.log(">>>>>> ProcessesActionCtrl");

            var def = function () {
                //$scope.hidRefresh = false;
                $scope.hidDetail = true;
                $scope.hidProgress = true;
                $scope.hidBlock = true;
                $scope.hidRun = true;
                $scope.hidCut = true;
                $scope.hidClose = true;
            };
            def();

            function showActionsBySelected() {
                if ($scope.curProcess) {
                    $scope.hidDetail = false;
                    $scope.hidProgress = false;
                }
            }

            function showActionsByStatus() {
                if (!$scope.curProcess)
                    return;
                switch ($scope.curProcess.Status) {
                    default:
                    case 'Running':
                        $scope.hidBlock = false;
                        break;
                    case 'Blocked':
                        $scope.hidRun = false;
                        break;
                    case 'Cutted':
                        break;
                    case 'Closed':
                        break;
                }
            }

            function showActionsByProgress() {
                if (!$scope.curProcess)
                    return;
                if ($scope.curProcess.Progress == 100)
                    $scope.hidClose = false;
                else
                    $scope.hidCut = false;
            }

            function quickViewCtrl() {
                if ($scope.app.settings.quickView == 'tpl/task.process.close.html') {
                    if (!$scope.curProcess){
                        $scope.app.settings.quickView = '';
                        return;
                    }
                    if ($scope.curProcess && $scope.curProcess.Progress != 100) {
                        $scope.app.settings.quickView = '';
                        return;
                    }
                }
            }

            $scope.$on('processSelected', function (event, process) {
                console.log(">>ProcessesActionCtrl.on.processSelected:: event, process");
                console.log(event);
                console.log(process);

                def();

                $scope.curProcess = process;
                showActionsBySelected();
                showActionsByStatus();
                showActionsByProgress();

                quickViewCtrl();

            });

            $scope.$on('processSelectedUpdated', function (event, process) {
                console.log(">>ProcessesActionCtrl.on.processSelectedUpdated:: event, process");
                console.log(event);
                console.log(process);

                def();

                $scope.curProcess = process;
                showActionsBySelected();
                showActionsByStatus();
                showActionsByProgress();

                quickViewCtrl();
            });

         
            $scope.detail = function () {
                console.log(">>ProcessesActionCtrl.detail");
                if (!$scope.curProcess)
                    return;
                $scope.app.settings.quickView = 'tpl/task.process.detail.html';

                $scope.$emit('openQViewEvent');
            };

            $scope.progress = function () {
                console.log(">>ProcessesActionCtrl.progress");
                if (!$scope.curProcess)
                    return;
                $scope.app.settings.quickView = 'tpl/task.process.progress.html';
                $scope.$emit('openQViewEvent');
            };

            $scope.block = function () {
                console.log(">>ProcessesActionCtrl.block");
                if (!$scope.curProcess)
                    return;
                $scope.app.settings.quickView = 'tpl/task.process.block.html';

                $scope.$emit('openQViewEvent');
            };

            $scope.run = function () {
                console.log(">>ProcessesActionCtrl.run");
                if (!$scope.curProcess)
                    return;
                $scope.app.settings.quickView = '';
                $scope.$emit('runProcessClickEvent');
            };

            $scope.getDataUri = function () {
                console.log(">>ProcessesActionCtrl.getDataUri");
                toaster.pop('info', 'Tips', 'The function is not implemented');
            };

            $scope.close = function () {

                console.log(">>ProcessesActionCtrl.close");
                if (!$scope.curProcess) {
                    toaster.pop('info', 'Tips', 'The selected process is not existed');
                    return;
                }
                if ($scope.curProcess.Progress != 100) {
                    toaster.pop('info', 'Tips', 'The progress of selected process is not 100%');
                    return;
                }


                $scope.app.settings.quickView = 'tpl/task.process.close.html';

                $scope.$emit('openQViewEvent');
            };

            $scope.abort = function () {
                console.log(">>ProcessesActionCtrl.abort");
                if (!$scope.curProcess)
                    return;
                $scope.app.settings.quickView = 'tpl/task.process.abort.html';
                $scope.$emit('openQViewEvent');
            };

        }]);