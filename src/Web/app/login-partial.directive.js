(function () {
	'use strict';

	angular
        .module('module.app')
        .directive('loginPartial', loginPartial);

	loginPartial.$inject = ['$window'];

	function loginPartial($window) {
		var directive = {
			replace: true,
			restrict: 'EA',
			scope: {
			},
			templateUrl: 'Templates/LoginPartial',
		};
		return directive;
	}
})();