angular.module('pie')
    .controller('PlanCostCtrl',
        ['$scope', '$state', function ($scope, $state) {
            console.log(">>>>>> PlanCostCtrl");

            $scope.planCostSettings = {
                width: '100%',
                autoheight: true,
                source: $scope.aptPlans,
                pageable: false,
                sortable: true,
                showsortmenuitems: false,
                showsortcolumnbackground: false,
                showaggregates: true,
                showstatusbar: true,
                statusbarheight: 25,
                enabletooltips: true,
                selectionmode: 'singlerow',
                columns: [
                  { text: 'Plan ID', datafield: 'ID', columntype: 'textbox', width: 10, editable: false, hidden: true },
                  { text: 'Plan Name', columntype: 'textbox', datafield: 'Title', editable: false },
                  { text: 'Start Date', datafield: 'StartDate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false },
                  { text: 'End Date', datafield: 'EndDate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false },
                  { text: 'Owner', datafield: 'CreateBy', columntype: 'textbox', width: 120, editable: false },
                  { text: 'Status', datafield: 'Status', columntype: 'textbox', width: 80, editable: false },
                  { text: 'Priority', datafield: 'Priority', columntype: 'textbox', width: 60, editable: false },
                  {
                      text: 'Workload', datafield: 'Workload', columntype: 'textbox', width: 100, editable: false, aggregates: ['sum'],
                      aggregatesrenderer: function (aggregates, column, element) {
                          var renderstring = "<div style='float: left; width: 100%; height: 100%; '>";
                          $.each(aggregates, function (key, value) {
                              renderstring += '<div style="position: relative; margin: 6px; text-align: left; overflow: hidden;">SUM:' + value + '</div>';
                          });
                          renderstring += "</div>";
                          return renderstring;
                      }
                  },
                  { text: 'VSOTaskLink', datafield: 'TaskLink', columntype: 'textbox', width: 90, editable: false }
                ]
            };

            $scope.$on('rowdoubleclick', function (event, arguments) {
                console.log(">>PlanCostCtrl.jqxGrid.rowdoubleclick:: event, arguments");
                console.log(event);
                console.log(arguments);

                var args = arguments.args;
                var curCost = args.row.bounddata;

                //window.location.href = '/#/pie/plan/detail/{planId}'.replace("{planId}", curPlan.ID);
            });



        }]);