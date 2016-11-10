(function () {
	'use strict';

	angular.module('module.app', ['satellizer', 'toastr']);

	angular
		.module("module.app")
		.config(config);

	config.$injector = ['$authProvider'];

	function config($authProvider) {
		$authProvider.loginUrl = 'http://localhost:37227/token';

		$authProvider.google({
			authorizationEndpoint: 'http://localhost:37227/api/v1/account/externallogin?provider=Google'
		});
	};
})();