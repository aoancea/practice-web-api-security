(function () {
	'use strict';

	angular.module('module.app', []);

	angular
		.module("module.app")
		.config(config);

	config.$injector = ['$httpProvider'];

	function config($httpProvider) {
		$httpProvider.interceptors.push('httpAuthorizationInterceptor');
	};
})();