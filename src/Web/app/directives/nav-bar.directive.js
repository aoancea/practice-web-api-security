(function () {
	'use strict';

	angular
        .module('module.app')
        .directive('navBar', navBar);

	navBar.$inject = [];

	function navBar() {
		var directive = {
			restrict: 'E',
			templateUrl: 'Templates/NavBar',
		};

		return directive;
	}
})();