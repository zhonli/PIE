angular.module('pie')
  .directive('uiToggleClass', ['$timeout', '$document', function ($timeout, $document) {
      return {
          restrict: 'AC',
          link: function (scope, el, attr) {
              
              el.on('click', function (e) {
                  e.preventDefault();
                  var classes = attr.uiToggleClass.split(','),
                      targets = (attr.target && attr.target.split(',')) || Array(el),
                      key = 0;
                  angular.forEach(classes, function (_class) {
                      var target = targets[(targets.length && key)];
                      $(target).toggleClass(_class);
                      key++;
                  });
                  $(el).toggleClass('active');
              });

              scope.$on('openQView', function (e) {
                  e.preventDefault();
                  var classes = attr.uiToggleClass.split(','),
                      targets = (attr.target && attr.target.split(',')) || Array(el),
                      key = 0;
                  if ($(el).attr('class').indexOf('active') !== -1)
                      return;
                  angular.forEach(classes, function (_class) {
                      var target = targets[(targets.length && key)];
                      $(target).toggleClass(_class);
                      key++;
                  });
                  $(el).toggleClass('active');
              });
          }
      };
  }]);