angular.module('pie')
    .controller('ReportFilter', ['$scope', '$state', function ($scope, $state) {
        console.log('>>>>ReportFilter');

        function initFilterStr(data) {
            if (data) {
                var filter = ' and (';
                for (var i = 0; i < data.length; i++) {
                    filter += "startswith(ComponentPath, '{ComponentPath}')".replace('{ComponentPath}', data[i]);
                    if (i != data.length - 1)
                        filter += ' or ';
                }
                filter += ')';
            }

            $scope.$emit('filterChangedEvent', filter);
        }

        $scope.tabs = [
          { title: 'Desktop & Mobile', content: 'tpl/report.daily.tpl.html', filter: ['CID0010136', 'CID0010037', 'CID0010042', 'CID0010002', 'CID0010170'], active: true, disabled: false },
          { title: 'WSSC', content: '', filter: ['CID0010336'], disabled: false },
          { title: 'Servicing', content: '', filter: ['CID0010405'], disabled: false }
        ];


        initFilterStr($scope.tabs[0].filter);

        function EmptyContent() {
            for (var i = 0; i < $scope.tabs.length; i++) {
                $scope.tabs[i].content = '';
            }
        }

        $scope.switch = function (index) {
            console.log(">>ReportFilter.switch::index");
            var tab = $scope.tabs[index];
            EmptyContent();
            initFilterStr(tab.filter);
            tab.content = 'tpl/report.daily.tpl.html';
        };

    }]);