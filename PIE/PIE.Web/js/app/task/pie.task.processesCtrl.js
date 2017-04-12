angular.module('pie')
    .controller('ProcessesCtrl',
        ['$scope', '$state', function ($scope, $state) {
            console.log(">>>>>> ProcessesCtrl");

            $scope.processesSettings = {
                width: '100%',
                autoheight: true,
                source: $scope.aptProcesses,
                pageable: false,
                sortable: true,
                showsortmenuitems: false,
                showsortcolumnbackground: false,
                showaggregates: true,
                showstatusbar: true,
                statusbarheight: 25,
                enabletooltips: true,
                selectionmode: 'singlerow',
                editmode: 'click',
                editable: true,
                columnsresize: true,
                columns: [
		          { text: 'ID', datafield: 'ID', columntype: 'textbox', width: 50, editable: false },
                  { text: 'Name', datafield: 'Name', columntype: 'textbox', editable: false },
                  { text: 'Status', datafield: 'Status', columntype: 'textbox', width: 70, editable: false },
                  { text: 'PID', datafield: 'PID', columntype: 'textbox', width: 90, editable: false },
                  { text: 'Priority', datafield: 'Priority', columntype: 'textbox', width: 60, editable: false },
                  { text: 'Progress', datafield: 'Progress', columntype: 'textbox', width: 60, editable: false, cellsformat: 'f0' },
                  { text: 'Completed', datafield: 'CompletedWorkhours', columntype: 'textbox', width: 80, editable: false },
                  { text: 'Actual SDate', datafield: 'StartOn', columntype: 'datetimeinput', width: 90, cellsformat: 'd', editable: true },
                  { text: 'Owner', datafield: 'Owner', columntype: 'textbox', width: 120, editable: false },
                  { text: 'RCIDs', datafield: 'RCIDs', columntype: 'textbox', width: 90, editable: false }
                ]
            };

            $scope.$on('rowclick', function (event, arguments) {
                console.log(">>ProcessesCtrl.jqxGrid.rowclick::curProcess");
                var args = arguments.args;
                var curProcess = args.row.bounddata;
                console.log(curProcess);
            });

            $scope.$on('rowselect', function (event, arguments) {
                console.log(">>ProcessesCtrl.jqxGrid.rowselect::curProcess");
                var args = arguments.args;
                $scope.selectedRowIndex = args.rowindex;
                var curProcess = args.row;
                console.log(curProcess);

                $scope.$emit('processSelectedEvent', curProcess);
            });

            $scope.$on('rowdoubleclick', function (event, arguments) {
                console.log(">>ProcessesCtrl.jqxGrid.rowdoubleclick");

                var args = arguments.args;
            });

            $scope.$on('bindingcomplete', function (event, arguments) {
                console.log(">>ProcessesCtrl.jqxGrid.bindingcomplete:: event, arguments");
                console.log(event);
                console.log(arguments);
            });

            $scope.$on('progressUpdated', function (event) {
                if (event.defaultPrevented)
                    return;
                event.preventDefault();
                console.log(">>ProcessesCtrl.on.progressUpdated:: event");
                console.log(event);

                if (!$scope.processesSettings.jqxGrid)
                    return;

                $scope.processesSettings.jqxGrid('updatebounddata', 'cells');

            });

            $scope.$on('processBlocked', function (event) {
                if (event.defaultPrevented)
                    return;
                event.preventDefault();
                console.log(">>ProcessesCtrl.on.processBlocked:: event");
                console.log(event);

                if (!$scope.processesSettings.jqxGrid)
                    return;

                $scope.processesSettings.jqxGrid('updatebounddata', 'cells');

            });

            $scope.$on('runProcessClick', function (event) {
                if (event.defaultPrevented)
                    return;
                event.preventDefault();
                console.log(">>ProcessesCtrl.on.runProcessClick:: event");
                console.log(event);

                if (!$scope.curProcess) {
                    //TDDO: messsage Warning
                    return;
                }

                if ($scope.curProcess.Status != 'Blocked') {
                    //TDDO: messsage Warning
                    return;
                }

                $scope.ProcessSource.runProcess($scope.curProcess, function () {
                    $scope.curProcess.Status = 'Running';
                    $scope.$emit('processSelectedUpdatedEvent', $scope.curProcess);
                    if (!$scope.processesSettings.jqxGrid)
                        return;
                    $scope.processesSettings.jqxGrid('updatebounddata', 'cells');

                });

            });

            $scope.$on('processAborted', function (event) {
                console.log(">>ProcessesCtrl.on.processAborted:: event");
                console.log(event);

                if (!$scope.processesSettings.jqxGrid)
                    return;
                $scope.processesSettings.jqxGrid('updatebounddata', 'cells');

            });

            $scope.$on('processClosed', function (event) {
                if (event.defaultPrevented)
                    return;
                event.preventDefault();

                console.log(">>ProcessesCtrl.on.ProcessClosed:: event");
                console.log(event);

                if (!$scope.processesSettings.jqxGrid)
                    return;
                $scope.curProcess = null;

                $scope.processesSettings.jqxGrid('updatebounddata', 'cells');

                $scope.$emit('processSelectedUpdatedEvent', $scope.curProcess);

            });

        }]);