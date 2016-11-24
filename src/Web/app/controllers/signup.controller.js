(function () {
	'use strict';

	angular
        .module('module.app')
        .controller('SignupController', SignupController);

	SignupController.$inject = ['$location', 'httpService', 'toastr', '$window'];

	function SignupController($location, httpService, toastr, $window) {
		var vm = this;

		vm.user = {};
		vm.signup = signup;

		function signup(user) {

			httpService.post("http://localhost:37227/api/v1/account/register", user)
				.then(function (response) {
					$window.location.href = '/Login/Login';
				})
				.catch(function (data) {
					toastr.error(data.Message);
				});
		}
	}
})();