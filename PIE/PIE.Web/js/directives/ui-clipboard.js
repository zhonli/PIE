angular.module('pie')
   .directive('uiClipboard', ['clipboard', function (clipboard) {
    return {
        restrict: 'A',
        scope: {
            onCopied: '&',
            onError: '&',
            copyText: '='
        },
        link: function (scope, element, attrs) {
            element.bind('click', function (event) {
                try {
                    clipboard.copyText(scope.copyText, element);
                    if (angular.isFunction(scope.onCopied)) {
                        scope.$evalAsync(scope.onCopied());
                    }
                } catch (err) {
                    if (angular.isFunction(scope.onError)) {
                        scope.$evalAsync(scope.onError({ err: err }));
                    }
                }
            });
        }
    }
}])