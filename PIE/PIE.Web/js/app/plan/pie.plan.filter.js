angular.module('pie')
    .controller('PlanFilter',
        ['$scope', '$state', function ($scope, $state) {
            console.log(">>>>>> PlanFilter :: bcdate");
            var bcdate = $state.params.bcdate;
            console.log(bcdate);

            $scope.products = [
                { value: 'CID0010136', label: 'Analog' },
                { value: 'CID0010037', label: 'Apps' },
                { value: 'CID0010165', label: 'Content' },
                { value: 'CID0010042', label: 'OSCore' },
                { value: 'CID0010002', label: 'PC, Tablet, Phone' },
                { value: 'CID0010336', label: 'Server' },
                { value: 'CID0010170', label: 'Store' },
                { value: 'CID0010875', label: 'Studios' },
                { value: 'CID0010405', label: 'Windows Servicing and Delivery' },
                { value: 'CID0010743', label: 'Xbox Platform' }
            ];

            var getResources = function (name) {
                var resouceSource =
                {
                    datatype: "json",
                    datafields: [
                        { name: 'Domain', type: 'string' },
                        { name: 'Alias', type: 'string' }
                    ],
                    url: serviceRoot + "Resources",
                    async: false
                };

                var fields = new Array();
                fields.push(name);
                var resourceDataAdapter = new $.jqx.dataAdapter(resouceSource, {
                    autoBind: true,
                    autoSort: true,
                    uniqueDataFields: fields,
                    autoSortField: name
                });
                resources = resourceDataAdapter.records;
                console.log(">>PlanFilter.getResources:: resources");
                console.log(resources);
                return resourceDataAdapter.records;
            }

            $scope.resources = getResources('Alias');

            $scope.status = ['Created', 'Executing', 'Closed'];

            $scope.$on("click", function (event) {
                console.log("search.button.click :: event");
                console.log(event);
                console.log($scope.filter);

                var filterU = '';
                if ($scope.filter.planTitle)
                    filterU += " and contains(Title,'titleS')".replace('titleS', $scope.filter.planTitle);
                if ($scope.filter.planOwner && $scope.filter.planOwner.value)
                    filterU += " and endswith(CreateBy,'ownerS')".replace('ownerS', $scope.filter.planOwner.value);
                if ($scope.filter.planStatus)
                    filterU += " and  Status eq PIEM.Common.Model.PlanStatus'statusS'".replace('statusS', $scope.filter.planStatus);
                if ($scope.filter.product && $scope.filter.product.value)
                    filterU += " and startswith(ComponentPath,'{rootComponentID}')".replace('{rootComponentID}', $scope.filter.product.value);
                console.log(filterU);

                $scope.$emit('searchedEvent', filterU);
            });

        }]);