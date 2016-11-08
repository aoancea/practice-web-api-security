(function () {
	'use strict';

	angular
		.module('module.login')
		.controller("LoginController", LoginController);

	LoginController.$inject = ['$location', '$auth', 'toastr'];

	function LoginController($location, $auth, toastr) {

		var vm = this;
		vm.user = {};
		vm.login = login;
		vm.authenticate = authenticate;

		function login(user) {
			$auth.login(user)
			  .then(function () {
			  	toastr.success('You have successfully signed in!');
			  	$location.path('/');
			  })
			  .catch(function (error) {
			  	toastr.error(error.data.message, error.status);
			  });
		};

		function authenticate(provider) {
			$auth.authenticate(provider)
			  .then(function () {
			  	toastr.success('You have successfully signed in with ' + provider + '!');
			  	$location.path('/');
			  })
			  .catch(function (error) {
			  	if (error.error) {
			  		// Popup error - invalid redirect_uri, pressed cancel button, etc.
			  		toastr.error(error.error);
			  	} else if (error.data) {
			  		// HTTP response error from server
			  		toastr.error(error.data.message, error.status);
			  	} else {
			  		toastr.error(error);
			  	}
			  });
		};
	}
})();