angular.module('pie')
    .controller('PlanBoardCtrlAdmin',
        ['$scope', '$state', function ($scope, $state) {
            console.log(">>>>>> PlanBoardCtrlAdmin");

            $scope.planBoardSettings = {
                width: '100%',
                autoheight: true,
                source: $scope.aptPlans,
                editable: true,
                pageable: false,
                sortable: true,
                showsortmenuitems: false,
                showsortcolumnbackground: false,
                enabletooltips: true,
                selectionmode: 'singlerow', //singlerow
                //pagermode: 'simple',
                editmode: 'click',
                columns: [
                  { text: 'Plan ID', datafield: 'ID', columntype: 'textbox', width: 10, editable: false, hidden: true },
                  { text: 'Plan Name', datafield: 'Title', columntype: 'textbox', editable: false },
                  { text: 'Start Date', datafield: 'StartDate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false },
                  { text: 'End Date', datafield: 'EndDate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false },
                  { text: 'Created', datafield: 'CreateTime', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false },
                  { text: 'Modified', datafield: 'LastModeifiedTime', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: true },
                  { text: 'Executed', datafield: 'ExecutedOn', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: true },
                  { text: 'Closed', datafield: 'ClosedOn', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: true },
                  { text: 'QA', datafield: 'QADate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false },
                  { text: 'SanityCheck ', datafield: 'SCDate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false }
                ]
            };

            $scope.$on('rowclick', function (event, arguments) {
                console.log(">>PlanBoardCtrl.jqxGrid.rowclick:: event, arguments");
                console.log(event);
                console.log(arguments);

                var args = arguments.args;
                var curPlan = args.row.bounddata;

                $scope.$emit('planSelectedEvent', curPlan);
            });

            $scope.$on('rowdoubleclick', function (event, arguments) {
                console.log(">>PlanBoardCtrl.jqxGrid.rowdoubleclick:: event, arguments");
                console.log(event);
                console.log(arguments);

                var args = arguments.args;
                var curPlan = args.row.bounddata;
                if (curPlan.CreateBy == alias) // && curPlan.Status == 'Created' 
                    window.location.href = '/#/pie/plan/save/{planId}'.replace("{planId}", curPlan.ID);
                else
                    window.location.href = '/#/pie/plan/detail/{planId}'.replace("{planId}", curPlan.ID);
            });

        }]);