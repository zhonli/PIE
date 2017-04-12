angular.module('pie').factory('ReportSource', ['BCService', function (BCService) {
    console.log(">>>>ReportSource");

    var headers = { 'Content-Type': 'application/json', Accept: 'application/json' };
    // prepare the datasource for the data
    var reportSource =
    {
        datatype: 'json',
        url: serviceRoot + 'Plans',
        async: false,
        datafields:
        [
            { name: 'ID', type: 'int', map: 'ID' },
            { name: 'PID', type: 'int', map: 'TaskLink>TaskID' },
            { name: 'CID', type: 'string', map: 'ComponentID' },
            { name: 'CreatedBy', type: 'string', map: 'CreateBy' },
            { name: 'TaskType', type: 'string', map: 'Type' },
            { name: 'TaskName', type: 'string', map: 'Title' },
            { name: 'RCID', type: 'int', map: 'Process>ResultSummaries>0>ID' },
            { name: 'Release', type: 'string', map: 'IterationPath' },
            { name: 'AssignedTo', type: 'string', map: 'Process>Owner' },
            { name: 'StartDate', type: 'date' },
            { name: 'EndDate', type: 'date' },
            { name: 'ActualStartDate', type: 'date', map: 'Process>StartOn' },
            { name: 'ActualEndDate', type: 'date', map: 'Process>EndOn' },
            { name: 'Workload', type: 'float', map: 'Workhours' },
            { name: 'PlanProgress', type: 'float', map: 'Process>Progress' },
            { name: 'Progress', type: 'float', map: 'Process>Progress' },
            { name: 'PassRate', type: 'float', map: 'Process>PassRate' },
            { name: 'PlanStatus', type: 'string', map: 'Status' },
            { name: 'ProcessStatus', type: 'string', map: 'Status' },
            { name: 'Priority', type: 'int' },
            { name: 'Tags', type: 'string' },
            { name: 'SprintID', type: 'string', map: 'IterationID' },
            { name: 'SprintPath', type: 'string', map: 'IterationPath' },
            { name: 'Component', type: 'string' },
            { name: 'ComponentPath', type: 'string' }
        ],
        sortcolumn: 'StartDate',
        sortdirection: 'asc',
        filter: function (filters, recordsArray) {
            console.log(">>ReportSource.filter: filters, recordsArray");
            console.Log(filters);
            console.Log(recordsArray);
        }
    };

    // prepare the dataAdapter
    var newSource = function (filter) {

        return new $.jqx.dataAdapter(reportSource, {
            autoBind: true,
            autoSort: true,
            //uniqueDataFields: ['ID'],
            autoSortField: 'StartDate',
            contentType: 'application/json; charset=utf-8',
            beforeSend: function (jqXHR, settings) {
                var baseFilter = function () {
                    return ' and (date(EndDate) ge date({bcSDate}) and date(EndDate) le date({bcEDate}) )'.replace('{bcSDate}', BCService.curBC.startDate).replace('{bcEDate}', BCService.curBC.endDate);
                };
                //A pre-request callback function that can be used to modify the jqXHR
                console.log('>>ReportSource.dataAdapter.beforeSend: jqXHR, settings');
                settings.url = settings.url.split('?')[0] + "?$expand=Process($expand=ResultSummaries),TaskLink&$filter=(Status ne PIEM.Common.Model.PlanStatus'Obsoleted')" + baseFilter();
                if (filter)
                    settings.url += filter.calc();
                console.log(jqXHR);
                console.log(settings);
            },
            beforeLoadComplete: function (records) {
                console.log(">>ReportSource.dataAdapter.beforeLoadComplete: data")
            },
            downloadComplete: function (edata, textStatus, jqXHR) {
                console.log(">>ReportSource.dataAdapter.downloadComplete:: edata, textStatus, jqXHR");
            },
            loadComplete: function (records) {
                console.log(">>ReportSource.dataAdapter.loadComplete :: records")
            },
            loadError: function (jqXHR, status, error) {
                console.group(">>ReportSource.dataAdapter.loadError :: jqXHR, status, error");
            }
        });
    }

    return {
        newSource: newSource
    };
}]);