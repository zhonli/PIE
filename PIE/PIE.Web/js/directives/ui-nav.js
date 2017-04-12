angular.module('pie')
  .directive('uiNav', ['$timeout', function ($timeout) {
      return {
          restrict: 'AC',
          link: function (scope, el, attr) {
              el.on('click', 'a', function (e) {
                  var _this = $(this);
                  _this.parent().siblings(".active").toggleClass('active');
                  _this.next().is('ul') && _this.parent().toggleClass('active') && e.preventDefault();
              });

          }
      };
  }]);