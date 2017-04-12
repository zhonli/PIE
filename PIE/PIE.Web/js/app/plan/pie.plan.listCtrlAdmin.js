angular.module('pie')
    .controller('PlanListCtrlAdmin',
        ['$scope', '$state', function ($scope, $state) {
            console.log(">>>>>> PlanListCtrlAdmin");

            $scope.planListSettings = {
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
                editable: true,
                editmode: 'click',
                columns: [
                  { text: 'ID', datafield: 'ID', columntype: 'textbox', width: 50, editable: false},
                  { text: 'Plan Name', columntype: 'textbox', datafield: 'Title', editable: false },
                  { text: 'Plan SDate', datafield: 'StartDate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false },
                  { text: 'Plan EDate', datafield: 'EndDate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false},
                  { text: 'Owner', datafield: 'CreateBy', columntype: 'textbox', width: 120, editable: false },
                  { text: 'Status', datafield: 'Status', columntype: 'textbox', width: 80, editable: false },
                  { text: 'Priority', datafield: 'Priority', columntype: 'textbox', width: 60, editable: false },
                  {
                      text: 'Plan Workload', datafield: 'Workhours', columntype: 'textbox', width: 100, editable: false, aggregates: ['sum'],
                      aggregatesrenderer: function (aggregates, column, element) {
                          var renderstring = "<div style='float: left; width: 100%; height: 100%; '>";
                          $.each(aggregates, function (key, value) {
                              renderstring += '<div style="position: relative; margin: 6px; text-align: left; overflow: hidden;">SUM:' + value + '</div>';
                          });
                          renderstring += "</div>";
                          return renderstring;
                      }
                  },
                  { text: 'CID', datafield: 'ComponentID', columntype: 'textbox', width: 80, editable: false },
                  { text: 'VSOTaskLink', datafield: 'TaskLink', columntype: 'textbox', width: 90, editable: true }           
                ]
            };

            $scope.$on('rowclick', function (event, arguments) {
                console.log(">>PlanListCtrlAdmin.jqxGrid.rowclick:: event, arguments");
                console.log(event);
                console.log(arguments);

                var args = arguments.args;
                var curPlan = args.row.bounddata;

                $scope.$emit('planSelectedEvent', curPlan);
            });

            $scope.$on('rowdoubleclick', function (event, arguments) {
                console.log(">>PlanListCtrlAdmin.jqxGrid.rowdoubleclick:: event, arguments");
                console.log(event);
                console.log(arguments);

                var args = arguments.args;
                var curPlan = args.row.bounddata;
                if (curPlan.CreateBy == alias) // && curPlan.Status == 'Created' //special right for owner. 
                    window.location.href = '/#/pie/plan/save/{planId}'.replace("{planId}", curPlan.ID);
                else
                    window.location.href = '/#/pie/plan/detail/{planId}'.replace("{planId}", curPlan.ID);
            });

            $scope.$on('bindingcomplete', function (event, arguments) {
                console.log(">>PlanListCtrlAdmin.jqxGrid.bindingcomplete:: event, arguments");
                console.log(event);
                console.log(arguments);
                /*
                element - the widget's HTML Element
                id - the widget's id
                name - the widget's name
                scope - the widget's scope
                instance - the widget's instnace
                */

                //bindGantt();
            });

            $scope.$on('planDeleted', function (event, plan) {
                console.log(">>PlanListCtrlAdmin.on.planDeleted:: event, plan");
                console.log(event);
                console.log(plan);
                if ($scope.planListSettings)
                    $scope.planListSettings.apply('deleterow', plan.uid);
            });

            $scope.$on('planClosed', function (event, plan) {
                console.log(">>PlanListCtrlAdmin.on.planClosed:: event, plan");
                console.log(event);
                console.log(plan);

                if ($scope.aptPlans)
                    $scope.aptPlans.dataBind();
            });

            $scope.$on('planExecuted', function (event, plan) {
                console.log(">>PlanListCtrl.on.planExecuted:: event, plan");
                console.log(event);
                console.log(plan);

                if ($scope.aptPlans)
                    $scope.aptPlans.dataBind();
            });
        }]);