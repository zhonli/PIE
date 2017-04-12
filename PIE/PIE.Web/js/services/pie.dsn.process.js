angular.module('pie').factory('ProcessSource', ['$q', function ($q) {
    console.log(">>>>ProcessSource");
    var headers = { 'Content-Type': 'application/json', Accept: 'application/json' };
    // prepare the datasource for the data
    var processSource =
    {
        datatype: 'json',
        url: serviceRoot + 'Processes',
        datafields:
        [
            { name: 'ID', type: 'int' },
            { name: 'PID', type: 'int', map: 'Plan>TaskLink>TaskID' },
            { name: 'Name', type: 'string' },
            { name: 'RCIDs', type: 'string', map: 'ResultSummaries>0>RSID' },
            { name: 'Status', type: 'string' },
            { name: 'StartOn', type: 'date' },
            { name: 'EndOn', type: 'date' },
            { name: 'Owner', type: 'string' },
            { name: 'Progress', type: 'float' },
            { name: 'CompletedWorkhours', type: 'float' },
            { name: 'ActualWorkhours', type: 'float' },
            { name: 'PassRate', type: 'float' },
            { name: 'Type', type: 'string', map: 'Plan>Type' },
            { name: 'Priority', type: 'int', map: 'Plan>Priority' },
            { name: 'ComponentID', type: 'string', map: 'Plan>ComponentID' },
            { name: 'Component', type: 'string', map: 'Plan>Component' },
            { name: 'ComponentPath', type: 'string', map: 'Plan>ComponentPath' }
        ],
        sortcolumn: 'StartOn',
        sortdirection: 'asc',

        filter: function (filters, recordsArray) {
            console.log(">>processSource.filter: filters, recordsArray");
            console.Log(filters);
            console.Log(recordsArray);
        },

        addrow: function (rowid, rowdata, position, commit) {
            console.log(">>processSource.addrow :: rowid, rowdata, position, commit");
            console.log(rowid);
            console.log(rowdata);
            console.log(position);
            console.log(commit);

            rowdata.SprintID = selectedSprintId;
            rowdata.ProjectID = projectId;

            var request = {
                requestUri: serviceRoot + 'Processes',
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
            console.log(">>processSource.updaterow :: rowid, rowdata");
            console.log(rowid);
            console.log(rowdata);

            var processId = rowdata.ID;
            var process = {};
            $.extend(process, rowdata);
            delete process.uid;
            delete process.PID;
            delete process.RCIDs;
            delete process.Type;
            delete process.Priority;
            delete process.ComponentID;
            delete process.Component;
            delete process.ComponentPath;

            var request = {
                requestUri: serviceRoot + 'Processes(' + processId + ')',
                method: 'PATCH',
                headers: headers,
                data: process
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
        // can't get plan ID by this api, to use deletePlan api.
        deleterow: function (rowid, commit) {
            console.log(">>processSource.deleterow :: rowid, commit");
            console.log(rowid);
            console.log(commit);

            commit(true);
        },

    };

    var deleteProcess = function (processId, succeed) {

        var request = {
            requestUri: serviceRoot + 'Processes(' + processId + ')',
            method: 'DELETE',
            headers: headers
        };

        odatajs.oData.request(
            request,
            function (data, response) {
                if (succeed)
                    succeed();
                //TODO faild to delete.
            },
            function (err) {
                console.error(err);
            }
        );
    };

    var runProcess = function (process, succeed) {
        var urlTemp = serviceRoot + 'Processes(:processId)/PieService.Run';
        var request = {
            requestUri: urlTemp.replace(':processId', process.ID),
            method: 'POST',
            headers: headers
        };

        if (process.Status != 'Blocked') {
            return;
        }

        odatajs.oData.request(
                request,
                function (data, response) {
                    if (succeed)
                        succeed();
                });
    };

    var closeProcess = function (process) {
        var deferred = $q.defer();
        var urlTemp = serviceRoot + 'Processes(:processId)/PieService.Close';
        var request = {
            requestUri: urlTemp.replace(':processId', process.ID),
            method: 'POST',
            headers: headers
        };

        if (process.Status != 'Running' && process.Status != 'Blocked') {
            return;
        }

        odatajs.oData.request(
                request,
                function (data, response) {
                    deferred.resolve(data);                   
                },function (error) {
                    deferred.reject();                   
                });

        return deferred.promise;
    };

    // prepare the dataAdapter
    var newProcessSource = function (filter) {

        return new $.jqx.dataAdapter(processSource, {
            contentType: 'application/json; charset=utf-8',

            beforeSend: function (jqXHR, settings) {
                //A pre-request callback function that can be used to modify the jqXHR
                console.log('>>processSource.dataAdapter.beforeSend: jqXHR, settings');
                settings.url = settings.url.split('?')[0] + "?$filter=(Status eq PIEM.Common.Model.ProcessStatus'Running' or Status eq PIEM.Common.Model.ProcessStatus'Blocked')&$expand=ResultSummaries,Plan($expand=TaskLink)";
                if (filter)
                    settings.url += filter.calc();
                //filter.dataUri = settings.url;
                console.log(jqXHR);
                console.log(settings);
            },
            beforeLoadComplete: function (records) {
                console.log(">>processSource.dataAdapter.beforeLoadComplete: data")
                console.log(records);
            },
            downloadComplete: function (edata, textStatus, jqXHR) {
                console.log(">>processSource.dataAdapter.downloadComplete:: edata, textStatus, jqXHR");
                console.log(edata);

            },
            loadComplete: function (records) {
                console.log(">>processSource.dataAdapter.loadComplete :: records")
                console.log(records);
            },
            loadError: function (jqXHR, status, error) {
                console.group(">>processSource.dataAdapter.loadError :: jqXHR, status, error");
            }
        });
    }

    return {
        newSource: newProcessSource,
        deleteProcess: deleteProcess,
        runProcess: runProcess,
        closeProcess: closeProcess
    };
}]);