(function () {
	'use strict';

	angular
		.module('module.app')
		.controller("LoginController", LoginController);

	LoginController.$inject = ['$http', '$location', 'toastr', '$window', 'windowService'];

	function LoginController($http, $location, toastr, $window, windowService) {

		var vm = this;
		vm.user = {};
		vm.login = login;
		vm.authenticate = authenticate;

		function login(user) {

			var data = "grant_type=password&username=" + user.email + "&password=" + user.password;

			$http.post("http://localhost:37227/token", data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
				.then(function (response) {

					localStorage.setItem("access_token", response.data.access_token);

					$window.location.href = '/';
				})
				.catch(function (error) {
					toastr.error(error.data.message, error.status);
				});
		};

		function authenticate(provider) {

			//windowService.open("http://www.google.com").then(function () {

			//});
		};
	}
})();