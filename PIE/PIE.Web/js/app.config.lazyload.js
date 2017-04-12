angular.module('pie')
  .constant('JQ_CONFIG', {
      chosen: ['vendor/jquery/chosen/chosen.jquery.min.js',
               'vendor/jquery/chosen/chosen.css'],
      plot: ['vendor/jquery/charts/flot/jquery.flot.min.js',
             'vendor/jquery/charts/flot/jquery.flot.resize.js',
             'vendor/jquery/charts/flot/jquery.flot.tooltip.min.js',
             'vendor/jquery/charts/flot/jquery.flot.spline.js',
             'vendor/jquery/charts/flot/jquery.flot.orderBars.js',
             'vendor/jquery/charts/flot/jquery.flot.pie.min.js']
  })
  // oclazyload config
  .config(['$ocLazyLoadProvider', function ($ocLazyLoadProvider) {
      $ocLazyLoadProvider.config({
          debug: false,
          events: true,
          modules: [
          {
              name: 'toaster',
              files: [
                  'vendor/modules/angularjs-toaster/toaster.js',
                  'vendor/modules/angularjs-toaster/toaster.css'
              ]
          },
          {
              name: 'ui.select',
              files: [
                'vendor/modules/angular-ui-select/select.min.js',
                'vendor/modules/angular-ui-select/select.min.css'
              ]
          }]
      });
  }])
;