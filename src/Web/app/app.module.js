(function () {
	'use strict';

	angular.module('module.app', ['toastr']);

	angular
		.module("module.app")
		.config(config);

	config.$injector = ['$httpProvider'];

	function config($httpProvider) {
		$httpProvider.interceptors.push('httpAuthorizationInterceptor');
	};
})();