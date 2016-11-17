(function () {
	'use strict';

	angular
        .module('module.app')
        .directive('loginPartial', loginPartial);

	loginPartial.$inject = [];

	LoginPartialController.$inject = ['$window'];

	function loginPartial() {
		var directive = {
			restrict: 'E',
			controller: LoginPartialController,
			controllerAs: 'vm',
			templateUrl: 'Templates/LoginPartial',
		};
		return directive;
	}

	function LoginPartialController($window) {
		var vm = this;
		vm.logout = logout;

		function logout() {
			localStorage.removeItem('access_token');

			$window.location.reload();
		};
	}
})();