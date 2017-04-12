﻿angular.module('ui.loader', [])
	.service('uiLoad', ['$document', '$q', '$timeout', function ($document, $q, $timeout) {

	    var loaded = [];
	    var promise = false;
	    var deferred = $q.defer();

        this.load = function (srcs) {
			srcs = angular.isArray(srcs) ? srcs : srcs.split(/\s+/);
			var self = this;
			if (!promise) {
				promise = deferred.promise;
			}
			angular.forEach(srcs, function (src) {
				promise = promise.then(function () {
					return src.indexOf('.css') >= 0 ? self.loadCSS(src) : self.loadScript(src);
				});
			});
			deferred.resolve();
			return promise;
		}

	    this.loadScript = function (src) {
	        if (loaded[src]) return loaded[src].promise;

	        var deferred = $q.defer();
	        var script = $document[0].createElement('script');
	        script.src = src;
	        script.onload = function (e) {
	            $timeout(function () {
	                deferred.resolve(e);
	            });
	        };
	        script.onerror = function (e) {
	            $timeout(function () {
	                deferred.reject(e);
	            });
	        };
	        $document[0].body.appendChild(script);
	        loaded[src] = deferred;

	        return deferred.promise;
	    };

	    this.loadCSS = function (href) {
	        if (loaded[href]) return loaded[href].promise;

	        var deferred = $q.defer();
	        var style = $document[0].createElement('link');
	        style.rel = 'stylesheet';
	        style.type = 'text/css';
	        style.href = href;
	        style.onload = function (e) {
	            $timeout(function () {
	                deferred.resolve(e);
	            });
	        };
	        style.onerror = function (e) {
	            $timeout(function () {
	                deferred.reject(e);
	            });
	        };
	        $document[0].head.appendChild(style);
	        loaded[href] = deferred;

	        return deferred.promise;
	    };
	}]);
