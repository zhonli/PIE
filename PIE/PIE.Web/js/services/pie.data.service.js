angular.module('pie').factory('Plan', ['$resource', function ($resource) {

    return $resource(baseUri + 'Plans(:id)', null, {
        'update': {
            method: 'PATCH',
            url: baseUri + 'Plans(:id)'
        },
        'execute': {
            method: 'POST',
            url: baseUri + 'Plans(:id)/PieService.Execute',
            params: { id: '@id' }
        },
        'executeVoid': {
            method: 'POST',
            url: baseUri + 'Plans(:id)/PieService.ExecuteVoid',
            params: { id: '@id' }
        }
    });
}]);

angular.module('pie').factory('Process', ['$resource', function ($resource) {

    return $resource(baseUri + 'Processes', null, { query: { method: 'get', isArray: true } });
}]);

angular.module('pie').factory('Transaction', ['$resource', function ($resource) {

    return $resource(baseUri + 'Processes(:id)/Transactions', null, {
        query: {
            method: 'get',
            isArray: true
        },
        update: {
            method: 'PUT',
            url: baseUri + 'Processes(:pid)/Transactions(:tranid)',
            params: { pid: '@id', tranid: '@trId' }
        }
    });
}]);

angular.module('pie').factory('TestCollateral', ['$resource', function ($resource) {

    return $resource(baseUri + 'TestCollaterals(:id)', null, {
        'update': {
            method: 'PATCH',
            url: baseUri + 'TestCollaterals(:id)'
        }
    });
}]);

angular.module('pie').factory('TaskLink', ['$resource', function ($resource) {

    return $resource(baseUri + 'TaskLinks(:id)', null, {
        'update': {
            method: 'PATCH',
            url: baseUri + 'TaskLinks(:id)'
        }
    });
}]);

angular.module('pie').factory('TaskSource', ['$resource', function ($resource) {

    return $resource(baseUri + 'TaskSources', null, { query: { method: 'get', isArray: true } });
}]);

var baseTFSServerUri = 'http://localhost:10042/DefaultCollection/';
angular.module('pie').factory('VSOWorkItem', ['$resource', function ($resource) {   
    return $resource(baseTFSServerUri + 'WorkItems(:workItemId)?$format=json', null, {
        save: {
            method: 'POST',
            url: baseTFSServerUri + 'WorkItems?$format=json'
        },
        update: {
            method: 'PUT',
            url: baseTFSServerUri + 'WorkItems(:workItemId)?$format=json',
            params: { workItemId: '@id' }
        }
    });
}]);

angular.module('pie').factory('VSOWorkItemLink', ['$http', '$q', function ($http, $q) {

    var create = function (linkTypeEndName, sourceId, targetId) {
        var deferred = $q.defer();
        var data = 'linkTypeEndName={0}&sourceId={1}&targetId={2}'.replace('{0}', linkTypeEndName).replace('{1}', sourceId).replace('{2}', targetId);
        $http.post(baseTFSServerUri + 'CreateWorkItemLink', data)
             .success(function (data, status, headers, config) {

                 deferred.resolve();
             })
             .error(function (data, status, headers, config) {

                 deferred.reject();
             });

        return deferred.promise;
    };

    return {
        create: create
    };

}]);

angular.module('pie').factory('VSOUser', ['$resource', function ($resource) {
    return $resource(baseTFSServerUri + "Users(':email')?$format=json", null, null);
}]);

