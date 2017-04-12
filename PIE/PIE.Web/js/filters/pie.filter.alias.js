angular.module('pie').filter('alias', function () {
    return function (input) {
        if (input) {
            var index = input.indexOf('\\');
            
            return input.slice(index + 1);
        }
    };
});