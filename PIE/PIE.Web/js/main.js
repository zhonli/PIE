var baseUri = $("#hidBaseUri").val();
//Todo remove serviceRoot val
var serviceRoot = $("#hidBaseUri").val();
var alias = $("#hidIdentityName").val();

console.log(">>pie.pm.main:: alias");
console.log(alias);

angular.module('pie')
  .run(['$rootScope', '$templateCache', function ($rootScope, $templateCache) {

      console.log('>>pie.run');

      $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams, error) {
          console.log('>>pie.ui.router.$routeChangeStart');

      });

      $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams, error) {
          console.log('>>pie.ui.router.$stateChangeSuccess');
          
      });

      $rootScope.$on('$stateChangeError', function (event, toState, toParams, fromState, fromParams, error) {
          console.log('>>pie.ui.router.$stateChangeError');
      });

  }]);

angular.module('pie')
  .controller('PieCtrl', ['$scope', '$window',
    function ($scope, $window) {
        console.log(">>PieCtrl");

        // config
        $scope.app = {
            name: 'PIE',
            settings: {
                appActions: '',
                quickView: '',
                asideFolded: false
            }
        };
        $scope.curTheme = 'bootstrap';
        $scope.user = {};
        $scope.user.identity = {};
        $scope.user.identity.name = alias;

        //default plan backlogs actions
        $scope.appActions = 'tpl/plan.backlogs.actions.html';

        //TODO: removed to toolbar service
        $scope.$on('planSelectedEvent', function (event, data) {
            $scope.$broadcast('planSelected', data);
        });

        $scope.$on('processSelectedEvent', function (event, data) {
            $scope.$broadcast('processSelected', data);
        });

        $scope.$on('processSelectedUpdatedEvent', function (event, data) {
            $scope.$broadcast('processSelectedUpdated', data);
        });

        $scope.$on('planDeletedEvent', function (event, data) {
            $scope.$broadcast('planDeleted', data);
        });
        
        $scope.$on('planExecutedEvent', function (event, data) {
            $scope.$broadcast('planExecuted', data);
        });
        $scope.$on('openQViewEvent', function (event, data) {
            $scope.$broadcast('openQView');
        });

        $scope.$on('runProcessClickEvent', function (event, data) {
            $scope.$broadcast('runProcessClick');
        });


        $scope.$watch('app.settings', function () {
            
        }, true);

    }]);








