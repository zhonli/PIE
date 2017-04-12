(function () {
    var serviceRoot = $("#hidBaseUri").val();
    var alias = $("#hidIdentityName").val();
    var projectid = $("#hidCurProject").val();
    //var curSprintId = $("#hidCurSprint").val();
    var selectedSprintId;
    var teamId = $("#hidCurTeam").val();
    var theme = 'bootstrap';

    var headers = { 'Content-Type': 'application/json', Accept: 'application/json' };

    var filter = function () {
        if (selectedSprintId) {
            return "$expand=TestCollateral,TaskLink&$filter=SprintID eq {SprintID} and CreateBy eq '{CreateBy}'".replace('{SprintID}', selectedSprintId).replace("{CreateBy}", alias);
        }
        return "$expand=TestCollateral,TaskLink&$filter=(SprintID eq {SprintID} or SprintID eq null) and CreateBy eq '{CreateBy}'".replace('{SprintID}', curSprintId).replace("{CreateBy}", alias);
    };

    var filterU = '';
    // prepare the datasource for the data
    var source =
    {
        datatype: 'json',
        url: serviceRoot + 'Plans',
        datafields:
        [
            { name: 'ID', type: 'int' },
            { name: 'Title', type: 'string' },
            { name: 'StartDate', type: 'date' },
            { name: 'EndDate', type: 'date' },
            { name: 'Status', type: 'string' },
            { name: 'Priority', type: 'int' },
            { name: 'Feature', type: 'string' },
            { name: 'Description', type: 'string' },
            { name: 'Assignment', type: 'string' },
            { name: 'CreateBy', type: 'string' },
            { name: 'Workload', type: 'string', map: 'Workhours' },
            { name: 'TaskLink', type: 'string', map: 'TaskLink>TaskID' },
            { name: 'ProductFamily', type: 'string' },
            { name: 'TestCollateralID', type: 'string', map: 'TestCollateral>ID' },

            { name: 'CreateTime', type: 'date' },
            { name: 'LastModeifiedTime', type: 'date' },
            { name: 'ExecutedOn', type: 'date' },
            { name: 'ClosedOn', type: 'date' },
            { name: 'QADate', type: 'date' },
            { name: 'SCDate', type: 'date' }
        ],
        sortcolumn: 'StartDate',
        sortdirection: 'asc',

        filter: function (filters, recordsArray) {
            console.log(">>filter: filters, recordsArray");
            console.Log(filters);
            console.Log(recordsArray);
        },

        addrow: function (rowid, rowdata, position, commit) {
            console.log(">>addrow :: rowid, rowdata, position, commit");
            console.log(rowid);
            console.log(rowdata);
            console.log(position);
            console.log(commit);

            rowdata.SprintID = sprintId;
            rowdata.ProjectID = projectId;

            var request = {
                requestUri: serviceRoot + 'Plans',
                method: 'POST',
                headers: headers,
                data: rowdata
            };

            odatajs.oData.request(
                request,
                function (data, response) {
                    console.log(">>odatajs.oData.request - callback -succeed");
                    console.log(data);
                    console.log(response);

                    rowdata = data;
                    commit(true);
                    //TODO: when row added, the date cell format doesn't work, maybe this is a bug
                    //$('#jqxgrid').jqxGrid('updatebounddata', 'cells');
                },
                function (err) {
                    console.error(err);
                }
            );
        },

        updaterow: function (rowid, rowdata, commit) {
            console.log(">>updaterow :: rowid, rowdata");
            console.log(rowid);
            console.log(rowdata);

            var planId = rowdata.ID;
            var plan = {};
            $.extend(plan, rowdata);
            delete plan.uid;

            var request = {
                requestUri: serviceRoot + 'Plans(' + planId + ')',
                method: 'PATCH',
                headers: headers,
                data: plan
            };


            odatajs.oData.request(
                request,
                function (data, response) {
                    //var createeRes = response;
                    var updated = data;
                    $.extend(rowdata, updated);
                    commit(true);
                    //this.dataBind();
                    //TODO: when row added, the date cell format doesn't work, maybe this is a bug
                    //$('#jqxgrid').jqxGrid('updatebounddata', 'cells');
                    //$("#jqxgrid").jqxGrid('setcellvalue', 0, 'lastname', 'My Value');
                },
                function (err) {
                    console.error(err);
                }
            );
        },

        deleterow: function (rowid, commit) {
            console.log(">>deleterow :: planId, commit");
            console.log(rowid);
            console.log(commit);

            var rowdata = $("#jqxgrid").jqxGrid('getrowdatabyid', rowid);
            var planId = rowdata.ID;

            var request = {
                requestUri: serviceRoot + 'Plans(' + planId + ')',
                method: 'DELETE',
                headers: headers
            };

            odatajs.oData.request(
                request,
                function (data, response) {
                    console.log(">>odatajs.oData.request - delete - callback -succeed");
                    console.log(data);
                    console.log(response);

                    commit(true);
                    //TODO faild to delete.
                },
                function (err) {
                    console.error(err);
                }
            );
        },
    };

    // prepare the aptPlans
    var aptPlans = new $.jqx.dataAdapter(source, {
        contentType: 'application/json; charset=utf-8',

        beforeSend: function (jqXHR, settings) {
            //A pre-request callback function that can be used to modify the jqXHR
            console.log('>>aptPlans.beforeSend: settings');
            console.log(settings);
            settings.url = settings.url.split('?')[0] + '?' + filter() + filterU;
            console.log(settings);
        },
        beforeLoadComplete: function (records) {
            console.log(">>aptPlans.beforeLoadComplete: data")
            console.log(records);
            var data = [];
            for (var i = 0; i < records.length; i++) {
                var record = records[i];
                //console.log(record);
                record.name = record.Title;
                record.values = [];
                var valueItem = {};
                valueItem.id = record.ID;
                valueItem.from = record.StartDate;
                valueItem.to = record.EndDate;
                valueItem.label = record.Title;
                record.values.push(valueItem);
                data.push(record);
            }

            console.log(data);

            return data;
        },
        downloadComplete: function (edata, textStatus, jqXHR) {
            console.log(">>aptPlans.downloadComplete:: edata, textStatus, jqXHR");
            console.log(edata);
            console.log(textStatus);
            console.log(jqXHR);
        },
        loadComplete: function (records) {
            console.log(">>aptPlans.loadComplete :: records")
            console.log(records);
            var length = records.length;
            // loop through the records and display them in a table.

            //console.groupEnd();

            //bindGantt();
        },
        loadError: function (jqXHR, status, error) {
            console.group(">>aptPlans.loadError :: jqXHR, status, error");
            console.log(jqXHR);
            console.log(status);
            console.log(error);
        }
    });

    var srcSprints =
               {
                   datatype: "json",
                   datafields: [
                       { name: 'ID' },
                       { name: 'Path' }
                   ],
                   url: serviceRoot + 'Sprints',
                   sortcolumn: 'Path',
                   sortdirection: 'desc',
                   async: true
               };
    var aptSprints = new $.jqx.dataAdapter(srcSprints);

    $("#ddlSprints").jqxComboBox({
        //selectedIndex: 0,
        source: aptSprints,
        displayMember: "Path",
        valueMember: "ID",
        width: 110, //156
        height: 24,
        animationType: 'none',
        theme: theme,
        placeHolder: "Current Sprint"
    });
    //$("#ddlSprints").on('bindingComplete', function (event) {
    //    $("#ddlSprints").jqxDropDownList('selectedIndex', aptSprints.records.length - 1);
    //});
    function getProducts() {
        var products = [];
        products.push({ key: 'Analog', value: 'Analog' });
        products.push({ key: 'Apps', value: 'Apps' });
        products.push({ key: 'Content', value: 'Content' });
        products.push({ key: 'OSCore', value: 'OSCore' });
        products.push({ key: 'PTP', value: 'PC, Tablet, Phone' });
        products.push({ key: 'Server', value: 'Server' });
        products.push({ key: 'Store', value: 'Store' });
        products.push({ key: 'Studios', value: 'Studios' });
        products.push({ key: 'WSD', value: 'Windows Servicing and Delivery' });
        products.push({ key: 'XboxPlatform', value: 'Xbox Platform' });
        return products;
    }

    var srcProducts = {
        localdata: getProducts(),
        datatype: "array",
        datafields:
        [
            { name: 'key', type: 'string' },
            { name: 'value', type: 'string' }
        ]
    };
    var aptProducts = new $.jqx.dataAdapter(srcProducts);

    $("#ddlProducts").jqxComboBox({
        //selectedIndex: 0,
        source: aptProducts,
        displayMember: "value",
        valueMember: "key",
        dropDownWidth: 200,
        dropDownHeight: 250,
        width: 156, //156
        height: 24,
        animationType: 'none',
        theme: theme,
        placeHolder: "Enter Product"
    });

    $("#txtPlanName").jqxInput({ placeHolder: "Plan Name", height: 24, width: 140, theme: theme });
    $("#txtOwner").jqxInput({ placeHolder: "Plan Owner", height: 24, width: 140, theme: theme });
    $("#txtStatus").jqxInput({ placeHolder: "Plan Status", height: 24, width: 140, theme: theme });
    $('#btnSearch').jqxButton({ width: 70, theme: theme });
    $('#btnAdd').jqxButton({ width: 70, theme: theme });
    $('#btnDelete').jqxButton({ width: 70, theme: theme });
    $('#btnExecute').jqxButton({ width: 70, theme: theme });
    $('#btnClose').jqxButton({ width: 70, theme: theme });
    $('#jqxTabs').jqxTabs({ width: '950', theme: theme });
    //$("#jqxTabs li").css("height", "auto");

    // prepare the jqxGrid
    var currentCellValue;

    $("#jqxgrid").jqxGrid({
        theme: theme,
        width: '100%',
        autoheight: true,
        source: aptPlans,
        editable: false,
        pageable: false,
        sortable: true,
        showsortmenuitems: false,
        showsortcolumnbackground: false,
        showaggregates: true,
        showstatusbar: true,
        statusbarheight: 25,
        enabletooltips: true,
        selectionmode: 'singlerow',
        //pagermode: 'simple',
        editmode: 'dblclick',
        columns: [
          { text: 'Plan ID', datafield: 'ID', columntype: 'textbox', width: 10, editable: false, hidden: true },
          { text: 'Plan Name', columntype: 'textbox', datafield: 'Title', editable: false },
          { text: 'Start Date', datafield: 'StartDate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false },
          { text: 'End Date', datafield: 'EndDate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false },
          { text: 'Owner', datafield: 'CreateBy', columntype: 'textbox', width: 120, editable: false },
          { text: 'Status', datafield: 'Status', columntype: 'textbox', width: 80, editable: false },
          {
              text: 'Priority', datafield: 'Priority', columntype: 'textbox', width: 60, editable: false
          },
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
    });

    $("#gridBoard").jqxGrid({
        theme: theme,
        width: '100%',
        autoheight: true,
        source: aptPlans,
        editable: false,
        pageable: false,
        sortable: true,
        showsortmenuitems: false,
        showsortcolumnbackground: false,
        enabletooltips: true,
        selectionmode: 'singlerow', //singlerow
        //pagermode: 'simple',
        //editmode: 'click',
        columns: [
          { text: 'Plan ID', datafield: 'ID', columntype: 'textbox', width: 10, editable: false, hidden: true },
          { text: 'Plan Name', datafield: 'Title', columntype: 'textbox', editable: false },
          { text: 'Start Date', datafield: 'StartDate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false },
          { text: 'End Date', datafield: 'EndDate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false },
          { text: 'Created', datafield: 'CreateTime', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: true },
          { text: 'Modified', datafield: 'LastModeifiedTime', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: true },
          { text: 'Executed', datafield: 'ExecutedOn', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: true },
          { text: 'Closed', datafield: 'ClosedOn', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: true },
          { text: 'QA', datafield: 'QADate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false },
          { text: 'SanityCheck ', datafield: 'SCDate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false }
        ]
    });
    // methods
    function existedPlanName(title) {
        for (var i = 0; i < aptPlans.records.length; i++) {
            var itemName = aptPlans.records[i]["Title"];
            if (name == itemName)
                return true;
        }
        return false;
    };
    $("#jqxgrid").on("sort", function (event) {

        console.log(">>jqxgrid.sort :: jqxgrid");
        console.log($("#jqxgrid").jqxGrid('getrows'));
    });
    //bind the Events
    $("#jqxgrid").bind('cellbeginedit', function (event) {
        console.log(">>on cellbeginedit::event");
        console.log(event);

        var args = event.args;
        var columnDataField = args.datafield;
        var rowIndex = args.rowindex;
        currentCellValue = args.value;
    });

    $("#jqxgrid").bind('cellendedit', function (event) {
        console.log(">>on cellendedit::event");
        console.log(event);

        var args = event.args;
        var columnDataField = args.datafield;
        var rowIndex = args.rowindex;
        var cellValue = args.value;
        var oldValue = args.oldvalue;
    });

    $("#jqxgrid").bind("pagechanged", function (event) {
        console.log(">>on pagechanged::event");
        console.log(event);

        var args = event.args;
        console.log(args.pagenum);
        console.log(args.pagesize);

        // get page information.
        var paginginformation = $("#jqxgrid").jqxGrid('getpaginginformation');
        console.log(paginginformation.pagenum);
        console.log(paginginformation.pagesize);
        console.log(paginginformation.pagescount);
    });

    $("#jqxgrid").bind("pagesizechanged", function (event) {
        console.log(">>on pagesizechanged::event");
        console.log(event);

        var args = event.args;
        console.log(args.pagenum);
        console.log(args.pagesize);
        console.log(args.oldpagesize);

        // get page information.
        var paginginformation = $("#jqxgrid").jqxGrid('getpaginginformation');

        console.log(paginginformation.pagenum);
        console.log(paginginformation.pagesize);
        console.log(paginginformation.pagescount);
    });

    $('#ddlSprints').on('change', function (event) {
        var args = event.args;
        if (args) {
            // index represents the item's index.                          
            var index = args.index;
            var item = args.item;
            // get item's label and value.
            var label = item.label;
            var value = item.value;
            var type = args.type; // keyboard, mouse or null depending on how the item was selected.
            console.log(">>ddlSprints.change :: item.value, item.label");
            console.log(item.value);
            console.log(item.label);
            selectedSprintId = item.value;
        }
        else {
            console.log(">>ddlSprints.change :: args is null");
            selectedSprintId = null;
        }
        //$('#jqxgrid').jqxGrid('refreshdata');
        aptPlans.dataBind();
        bindGantt();
    });

    $('#jqxgrid').on('rowclick', function (event) {
        var args = event.args;
        var curPlan = args.row.bounddata;

        console.log(">>rowclick:: curPlan");
        console.log(curPlan);
        setUI(curPlan);
    });

    $('#jqxgrid').on('rowdoubleclick', function (event) {
        var args = event.args;
        var curPlan = args.row.bounddata;
        console.log(">>rowdoubleclick:: curPlan");
        console.log(curPlan);
        if (curPlan.Status == 'Created')
            window.location.href = '/#/pie/plan/save/{planId}'.replace("{planId}", curPlan.ID);
        else
            window.location.href = '/#/pie/plan/detail/{planId}'.replace("{planId}", curPlan.ID);
    });

    $("#jqxgrid").on("bindingcomplete", function (event) {
        bindGantt();
    });

    function setUI(plan) {
        $('#btnDelete').jqxButton({ disabled: true });
        $('#btnExecute').jqxButton({ disabled: true });
        $('#btnClose').jqxButton({ disabled: true });
        switch (plan.Status) {
            default:
            case 'Created':
                $('#btnDelete').jqxButton({ disabled: false });
                $('#btnExecute').jqxButton({ disabled: false });
                break;
            case 'Executing':
                $('#btnClose').jqxButton({ disabled: false });
                break;
            case 'Cancled':
                $('#btnDelete').jqxButton({ disabled: false });
                $('#btnExecute').jqxButton({ disabled: false });
                break;
            case 'Closed':
                break;
            case 'Obsoleted':
                $('#btnDelete').jqxButton({ disabled: false });
                break;
        }
    }

    $("#btnDelete").bind("click", function () {
        var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
        var rowId = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
        var commit = $("#jqxgrid").jqxGrid('deleterow', rowId);
    });

    $("#btnSearch").bind("click", function () {
        filterU = '';
        var titleS = $("#txtPlanName").val().trim();
        var ownerS = $("#txtOwner").val().trim();
        var statusS = $("#txtStatus").val().trim();
        var productS = $("#ddlProducts").val().trim();
        if (titleS)
            filterU += " and contains(Title,'titleS')".replace('titleS', titleS);
        if (ownerS)
            filterU += " and CreateBy eq 'ownerS'".replace('ownerS', ownerS);
        if (statusS)
            filterU += " and  Status eq PIEM.Common.Model.PlanStatus'statusS'".replace('statusS', statusS);
        if (productS)
            filterU += " and Product eq PIEM.Common.Model.Product'productS'".replace('productS', productS);
        console.log(filterU);

        aptPlans.dataBind();
        bindGantt();
    });

    $("#btnExecute").bind("click", function () {
        var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
        var rowId = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
        var rowdata = $("#jqxgrid").jqxGrid('getrowdatabyid', rowId);
        var planId = rowdata.ID;
        console.log(">>btnExecute.click:: planId");
        console.log(planId);
        window.location.href = '/#/pie/plan/execute/{planId}'.replace("{planId}", planId);
    });

    $("#btnClose").bind("click", function () {
        var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
        var rowId = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
        var rowdata = $("#jqxgrid").jqxGrid('getrowdatabyid', rowId);
        var planId = rowdata.ID;
        var planStatus = rowdata.Status;
        console.log(">>btnClose.click:: planId, planStatus");
        console.log(planId);
        console.log(planStatus);

        var headers = {
            'Content-Type': 'application/json', Accept: 'application/json'
        };
        var urlTemp = serviceRoot + 'Plans(:planId)/PieService.Close';
        var request = {
            requestUri: urlTemp.replace(':planId', planId),
            method: 'POST',
            headers: headers
        };

        if (planStatus == 'Executing') {
            odatajs.oData.request(
                request,
                function (data, response) {
                    console.log(">>odatajs.oData.request - get - callback -succeed");
                    console.log(data);
                    console.log(response);
                    aptPlans.dataBind();
                });
        }
    });


    $('#jqxTabs').on('tabclick', function (event) {
        console.log(">>jqxTabs.tabclick::event");
        console.log(event);
        var tabclicked = event.args.item;
        switch (tabclicked) {
            default:
            case 0:
                break;
            case 1:

                bindGantt();
                break;
        }
    });

    $("body").keydown(function () {
        if (event.keyCode == "13") {
            $('#btnSearch').click();
        }
    });

    //---------------------

    var bindGantt = function () {
        var data = $("#jqxgrid").jqxGrid('getrows');

        console.log(">>bindGantt:: data");
        console.log(data);

        if (data.length == 0) {
            $(".gantt").html('<span style="">No data to dispaly</span>');
            $(".gantt").css({ 'text-align': 'center', height: '24px' });

            return;
        }

        $(".gantt").gantt({
            source: data,
            itemsPerPage: data.length,
            //navigate: 'scroll',
            scale: 'days',
            maxScale: 'weeks',
            minScale: 'hours'
        });
    }

})();