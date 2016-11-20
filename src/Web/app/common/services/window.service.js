(function () {
	'use strict';

	angular
        .module('module.app')
        .factory('windowService', windowService);

	windowService.$inject = ['$q', '$window', '$interval'];

	function windowService($q, $window, $interval) {
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


			var polling = $interval(function () {
				try {
					if (popupWindow.document.domain === document.domain && (popupWindow.location.search || popupWindow.location.hash)) {
						var hashParams = popupWindow.location.hash.substring(1).replace(/\/$/, '');
						var queryParams = popupWindow.location.search.substring(1).replace(/\/$/, '');
						var hash = parseQueryString(hashParams);
						var qs = parseQueryString(queryParams);

						angular.extend(qs, hash);

						if (qs.error) {
							deferred.reject({ error: qs.error });
						} else {
							deferred.resolve(qs);
						}

						popupWindow.close();
						$interval.cancel(polling);
					}
				} catch (error) {
				}

				if (!popupWindow) {
					$interval.cancel(polling);
					deferred.reject({ data: 'Provider Popup Blocked' });
				} else if (popupWindow.closed) {
					$interval.cancel(polling);
					deferred.reject({ data: 'Authorization Failed' });
				}
			}, 35);

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

		function parseQueryString(keyValue) {
			var obj = {}, key, value;
			angular.forEach((keyValue || '').split('&'), function (keyValue) {
				if (keyValue) {
					value = keyValue.split('=');
					key = decodeURIComponent(value[0]);
					obj[key] = angular.isDefined(value[1]) ? decodeURIComponent(value[1]) : true;
				}
			});
			return obj;
		};
	}
})();