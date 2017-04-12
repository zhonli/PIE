angular.module('pie').factory('BacklogSource', function () {
    console.log(">>>>BacklogSource");
    var headers = { 'Content-Type': 'application/json', Accept: 'application/json' };
    // prepare the datasource for the data
    var backlogSource =
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
            { name: 'Workhours', type: 'string', map: 'Workhours' },
            { name: 'TaskLink', type: 'string', map: 'TaskLink>TaskID' },
            { name: 'ComponentID', type: 'string' },
            { name: 'Component', type: 'string' },
            { name: 'ComponentPath', type: 'string' },
            { name: 'Type', type: 'string' },
            { name: 'CreateTime', type: 'date' },
            { name: 'LastModeifiedTime', type: 'date' },
            { name: 'ExecutedOn', type: 'date' },
            { name: 'ClosedOn', type: 'date' },
            { name: 'QADate', type: 'date' },
            { name: 'SCDate', type: 'date' },
        ],
        sortcolumn: 'StartDate',
        sortdirection: 'asc',

        filter: function (filters, recordsArray) {
            console.log(">>BacklogSource.filter: filters, recordsArray");
            //console.Log(filters);
            //console.Log(recordsArray);
        },

        addrow: function (rowid, rowdata, position, commit) {
            console.log(">>BacklogSource.addrow :: rowid, rowdata, position, commit");
            console.log(rowid);
            console.log(rowdata);
            console.log(position);
            console.log(commit);

            rowdata.SprintID = selectedSprintId;
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
            console.log(">>BacklogSource.updaterow :: rowid, rowdata");

            var planId = rowdata.ID;
            var plan = {};
            $.extend(plan, rowdata);
            delete plan.uid;
            delete plan.TaskLink;
            delete plan.name;
            delete plan.values;

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

            var taskLink = {};
            taskLink.ID = rowdata.ID;
            taskLink.TaskID = rowdata.TaskLink;

            var taskLinkRequest = {
                requestUri: serviceRoot + 'TaskLinks(' + taskLink.ID + ')',
                method: 'PATCH',
                headers: headers,
                data: taskLink
            };

            odatajs.oData.request(
                taskLinkRequest,
                function (data, response) {
                    var updated = data;
                    $.extend(rowdata, updated);
                    commit(true);
                },
                function (err) {
                    console.error(err);
                }
            );
        },
        // can't get plan ID by this api, to use deletePlan api.
        deleterow: function (rowid, commit) {
            console.log(">>BacklogSource.deleterow :: rowid, commit");
            console.log(rowid);
            console.log(commit);

            commit(true);
        },

    };

    var deletePlan = function (planId, succeed) {

        var request = {
            requestUri: serviceRoot + 'Plans(' + planId + ')',
            method: 'DELETE',
            headers: headers
        };

        odatajs.oData.request(
            request,
            function (data, response) {
                if(succeed)
                    succeed();
                //TODO faild to delete.
            },
            function (err) {
                console.error(err);
            }
        );
    };

    var closePlan = function (plan, succeed) {
        var urlTemp = serviceRoot + 'Plans(:planId)/PieService.Close';
        var request = {
            requestUri: urlTemp.replace(':planId', plan.ID),
            method: 'POST',
            headers: headers
        };

        if (plan.Status != 'Executing') {
            //TODO: messge to warning.
            return;
        }

        odatajs.oData.request(
                request,
                function (data, response) {
                    if(succeed)
                        succeed();
                });
    };

    // prepare the dataAdapter
    var newSource = function (filter) {

        return new $.jqx.dataAdapter(backlogSource, {
            contentType: 'application/json; charset=utf-8',
            beforeSend: function (jqXHR, settings) {
                //A pre-request callback function that can be used to modify the jqXHR
                console.log('>>BacklogSource.dataAdapter.beforeSend: jqXHR, settings');
                settings.url = settings.url.split('?')[0] + "?$expand=TestCollateral,TaskLink&$filter=(Status ne PIEM.Common.Model.PlanStatus'Obsoleted')";
                if(filter) {
                    settings.url += filter.calc();
                    filter.dataUri = settings.url;
                }
                console.log(jqXHR);
                console.log(settings);
            },
            beforeLoadComplete: function (records) {
                console.log(">>BacklogSource.dataAdapter.beforeLoadComplete: records")
                //console.log(records);
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

                //console.log(data);
                return data;
            },
            downloadComplete: function (edata, textStatus, jqXHR) {
                console.log(">>BacklogSource.dataAdapter.downloadComplete:: edata, textStatus, jqXHR");
            },
            loadComplete: function (records) {
                console.log(">>BacklogSource.dataAdapter.loadComplete :: records")
            },
            loadError: function (jqXHR, status, error) {
                console.group(">>BacklogSource.dataAdapter.loadError :: jqXHR, status, error");
            }
        });
    }

    return {
        newSource: newSource,
        deletePlan: deletePlan,
        closePlan: closePlan
    };
});