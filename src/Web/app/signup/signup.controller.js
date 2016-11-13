(function () {
    'use strict';

    angular
        .module('module.signup')
        .controller('SignupController', SignupController);

    SignupController.$inject = ['$location', '$http', 'toastr', '$window'];

    function SignupController($location, $http, toastr, $window) {
    	var vm = this;
    	vm.user = {};
    	vm.signup = signup;

    	function signup(user) {
    		var testUser = user;

    		$http.post("http://localhost:37227/api/v1/account/register", user)
				.then(function (response) {
					$window.location.href = '/Login/Login';
				})
				.catch(function (error) {
					toastr.error(error.data.message, error.status);
				});
    	}
    }
})();
