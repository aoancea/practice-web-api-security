(function () {
	'use strict';

	angular
        .module('module.app')
        .factory('windowService', windowService);

	windowService.$inject = ['$q', '$window'];

	function windowService($q, $window) {
		var service = {
			open: open,
		}

		return service;

		function open(url) {
			var deferred = $q.defer();

			var optionsString = buildWindowOptions();

			var popupWindow = $window.open(url, '_blank', optionsString);

			if (popupWindow && popupWindow.focus) {
				popupWindow.focus();
			}

			return deferred.promise;
		}

		function buildWindowOptions() {
			var width = 500;
			var height = 500;
			var options = angular.extend({
				width: width,
				height: height,
				left: $window.screenX + (($window.outerWidth - width) / 2),
				top: $window.screenY + (($window.outerHeight - height) / 2.5)
			}, {});

			var parts = [];
			angular.forEach(options, function (value, key) {
				parts.push(key + '=' + value);
			});

			return parts.join(',');
		};
	}
})();