angular.module('pie')
	.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
	    
	    $urlRouterProvider.otherwise('/pie/plan/backlogs/current');

	    $stateProvider
			.state('pie', {
			    url: '/pie',
			    templateUrl: 'tpl/app.html'
			})
            // plan
            .state('pie.plan', {
                url: '/plan',
                templateUrl: 'tpl/plan.html'
            })
            .state('pie.plan.backlogs', {
                url: '/backlogs/:bcId',
                templateUrl: 'tpl/plan.backlogs.html'
            })
            .state('pie.plan.listCreated', {
                url: '/listCreated',
                templateUrl: 'tpl/plan.query.html'
            })
			.state('pie.plan.save', {
			    url: '/save/:planId',
			    templateUrl: 'tpl/plan.save.html'
			})
            .state('pie.plan.detail', {
                url: '/detail/:planId',
                templateUrl: 'tpl/plan.detail.html'
            })
            .state('pie.plan.assignment', {
                url: '/assignment/:planId',
                templateUrl: 'tpl/plan.assignment.html'
            })
            .state('pie.plan.vsoMapping', {
                url: '/vsoMapping/:planId',
                templateUrl: 'tpl/plan.vsoMapping.html'
            })
            .state('pie.plan.execute', {
                url: '/execute/:planId',
                templateUrl: 'tpl/plan.execute.html'
            })

            // task
            .state('pie.task', {
                url: '/task',
                templateUrl: 'tpl/task.html'
            })
            .state('pie.task.processes', {
                url: '/processes',
                templateUrl: 'tpl/task.processes.html'
            })
            .state('pie.task.sanityCheck', {
                url: '/sanityCheck',
                templateUrl: 'tpl/task.sanityCheck.html'
            })

            // report
            .state('pie.report', {
                url: '/report',
                templateUrl: 'tpl/report.html'
            })
            .state('pie.report.daily', {
                url: '/daily/:dateRef',
                templateUrl: 'tpl/report.daily.html'
            })
            .state('pie.report.weekly', {
                url: '/weekly/:weekRef',
                templateUrl: 'tpl/report.weekly.html'
            })
            .state('pie.report.monthly', {
                url: '/monthly/:monthRef',
                templateUrl: 'tpl/report.monthly.html'
            })
            .state('pie.report.kpi', {
                url: '/kpi',
                templateUrl: 'tpl/report.kpi.html'
            });


	}]);