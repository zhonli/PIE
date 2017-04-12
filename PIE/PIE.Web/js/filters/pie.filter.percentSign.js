angular.module('pie').filter('percentSign', function () {
    return function (input) {
        if (input == 0)
            return '0';
        if (input) {
            var input = input.toFixed(0);
            
            return input + '%';
        }
    };
});