﻿(function () {
    var serviceRoot = $("#hidBaseUri").val();
    var alias = $("#hidIdentityName").val();
    var projectid = $("#hidCurProject").val();
    var curSprintId = $("#hidCurSprint").val();
    var selectedSprintId;
    var teamId = $("#hidCurTeam").val();
    var theme = 'bootstrap';

    var headers = { 'Content-Type': 'application/json', Accept: 'application/json' };

    var filter = function () {
        if (selectedSprintId) {
            return "$expand=TestCollateral,TaskLink&$filter=SprintID eq {SprintID}".replace('{SprintID}', selectedSprintId);
        }
        return "$expand=TestCollateral,TaskLink&$filter=1 eq 1 ";
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
            { name: 'CreateTime', type: 'date' },
            { name: 'LastModeifiedTime', type: 'date' },
            { name: 'ExecutedOn', type: 'date' },
            { name: 'ClosedOn', type: 'date' },
            { name: 'QADate', type: 'date' },
            { name: 'SCDate', type: 'date' },
            { name: 'Status', type: 'string' }
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
        /*
        loadServerData: function (serverdata, source, callback) {
            //A callback function which allows you to manually handle the ajax calls through the jqxDataAdapter.
            console.log('>>aptPlans.loadServerData: serverdata, source, callback');
            console.log(serverdata);
            console.log(source);
            console.log(callback);
        },
        */
        beforeSend: function (jqXHR, settings) {
            //A pre-request callback function that can be used to modify the jqXHR
            console.log('>>aptPlans.beforeSend: settings');
            console.log(settings);
            settings.url = settings.url.split('?')[0] + '?' + filter() + filterU;
            console.log(settings);
        },
        beforeLoadComplete: function (records) {
            console.log(">>aptPlans.beforeLoadComplete: records")
            console.log(records);
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
            for (var i = 0; i < length; i++) {
                var record = records[i];
                //console.log(record);
            }
            //console.groupEnd();
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
    var aptSprints = new $.jqx.dataAdapter(srcSprints, {
        beforeLoadComplete: function (records) {
            console.log(">>aptSprints.beforeLoadComplete: data")
            console.log(records);
            var data = [];
            for (var i = 0; i < records.length; i++) {
                var record = records[i];
                //console.log(record);
                if (record.ID >= curSprintId)
                    continue;
                data.push(record);
            }

            console.log(data);

            return data;
        }
    });

    $("#ddlSprints").jqxDropDownList({
        source: aptSprints,
        displayMember: "Path",
        valueMember: "ID",
        width: 156, //156
        height: 24,
        //checkboxes: true,
        animationType: 'none',
        theme: theme,
        //selectedIndex: sprintsCount - 1,
        //placeHolder: "Passed Sprint"
    });
    $("#ddlSprints").on('bindingComplete', function (event) {
        $("#ddlSprints").jqxDropDownList('selectedIndex', aptSprints.records.length - 1);
        bindGrid();
    });


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

    $("#txtPlanName").jqxInput({ placeHolder: "Plan Name", height: 24, width: 150, theme: theme });
    $("#txtOwner").jqxInput({ placeHolder: "Plan Owner", height: 24, width: 150, theme: theme });
    $("#txtStatus").jqxInput({ placeHolder: "Plan Status", height: 24, width: 150, theme: theme });
    $('#btnSearch').jqxButton({ width: 70, theme: theme });
    $('#btnClose').jqxButton({ width: 70, theme: theme });
    $('#btnDelete').jqxButton({ width: 70, theme: theme });
    $('#jqxTabs').jqxTabs({ width: '950', theme: theme });
    //$("#jqxTabs li").css("height", "auto");

    // prepare the jqxGrid
    var currentCellValue;

    var bindGrid = function () {
        $("#jqxgrid").jqxGrid({
            theme: theme,
            width: '100%',
            autoheight: true,
            source: aptPlans,
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
              { text: 'Created', datafield: 'CreateTime', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: true },
              { text: 'Modified', datafield: 'LastModeifiedTime', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: true },
              { text: 'Executed', datafield: 'ExecutedOn', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: true },
              { text: 'Closed', datafield: 'ClosedOn', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: true },
              { text: 'QA', datafield: 'QADate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false },
              { text: 'SanityCheck ', datafield: 'SCDate', columntype: 'datetimeinput', width: 80, cellsformat: 'd', editable: false },
            ]
        });
    }

    // methods
    function existedPlanName(title) {
        for (var i = 0; i < aptPlans.records.length; i++) {
            var itemName = aptPlans.records[i]["Title"];
            if (name == itemName)
                return true;
        }
        return false;
    };

    //
    function setUI(plan) {
        $('#btnClose').jqxButton({ disabled: true });
        $('#btnDelete').jqxButton({ disabled: true });
        switch (plan.Status) {
            default:
            case 'Created':
                $('#btnDelete').jqxButton({ disabled: false });
                break;
            case 'Executing':
                $('#btnClose').jqxButton({ disabled: false });
                break;
            case 'Cancled':
                break;
            case 'Closed':
                break;
            case 'Obsoleted':
                $('#btnDelete').jqxButton({ disabled: false });
                break;
        }
    }
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
        window.location.href = '/#/pie/plan/detail/{planId}'.replace("{planId}", curPlan.ID);
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
    });

    $("#btnDelete").bind("click", function () {
        var selectedrowindex = $("#jqxgrid").jqxGrid('getselectedrowindex');
        var rowId = $("#jqxgrid").jqxGrid('getrowid', selectedrowindex);
        var commit = $("#jqxgrid").jqxGrid('deleterow', rowId);
    });

    $('#animation').on('change', function (event) {
        var checked = event.args.checked;
        $('#jqxTabs').jqxTabs({ selectionTracker: checked });
    });

    $('#contentAnimation').on('change', function (event) {
        var checked = event.args.checked;
        //$('#jqxTabs').jqxTabs({ animationType: 'fade' });
        //$('#jqxTabs').jqxTabs({ animationType: 'none' });

    });

    $("body").keydown(function () {
        if (event.keyCode == "13") {
            $('#btnSearch').click();
        }
    });

})();