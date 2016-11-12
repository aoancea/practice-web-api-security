(function () {
	'use strict';

	angular
        .module('module.app')
        .factory('httpAuthorizationInterceptor', httpAuthorizationInterceptor);

	httpAuthorizationInterceptor.$inject = [];

	function httpAuthorizationInterceptor() {
		var service = {
			request: request,
			response: response
		}

		return service;

		function request(config) {

			var access_token = localStorage.getItem("access_token");

			if (access_token) {
				config.headers.Authorization = 'Bearer ' + access_token;
			}

			return config;
		}

		function response(config) {

			return config;
		}
	}

})();