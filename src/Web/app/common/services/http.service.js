(function () {
	'use strict';

	angular
        .module('module.app')
        .factory('httpService', httpService);

	httpService.$inject = ['$http', '$q'];

	function httpService($http, $q) {
		var service = {
			get: get,
			post: post,
			del: del
		}

		return service;

		function get(url, params) {
			var deferred = $q.defer();
			var options = { async: true };

			angular.extend(options, { params: params });

			$http.get(url, options)
                .success(function (data, status, headers, config) {
                	deferred.resolve(data);
                })
                .error(function (data, status, headers, config) {
                	deferred.reject(status);
                });

			return deferred.promise;
		}

		function post(url, data) {
			var deferred = $q.defer();

			$http.post(url, data, { async: true })
                .success(function (data, status, headers, config) {
                	deferred.resolve(data);
                })
                .error(function (data, status, headers, config) {
                	deferred.reject(data);
                });

			return deferred.promise;
		}

		function del(url, data) {
			var deferred = $q.defer();

			$http.delete(url, data, { async: true })
                .success(function (data, status, headers, config) {
                	deferred.resolve(data);
                })
                .error(function (data, status, headers, config) {
                	deferred.reject(status);
                });

			return deferred.promise;
		}
	}
})();