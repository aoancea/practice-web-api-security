(function () {
	'use strict';

	angular
		.module('module.login')
		.controller("LoginController", LoginController);

	LoginController.$inject = ['$http', '$location', '$auth', 'toastr', 'satellizer.popup'];

	function LoginController($http, $location, $auth, toastr, popup) {

		var vm = this;
		vm.user = {};
		vm.login = login;
		vm.authenticate = authenticate;

		function login(user) {

			var data = "grant_type=password&username=" + user.email + "&password=" + user.password;

			$http.post("http://localhost:37227/token", data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } })
				.then(function (response) {

					localStorage.setItem("access_token", response.data.access_token);

					toastr.success('You have successfully signed in!');
				})
				.catch(function (error) {
					toastr.error(error.data.message, error.status);
				});
		};

		function authenticate(provider) {

			popup.open('http://localhost:37227/api/v1/account/externallogin?provider=Google', {}, '')
              .then(function (oauthData) {
              	if (defaults.responseType === 'token') {
              		return oauthData;
              	}
              	if (oauthData.state && oauthData.state !== $window.localStorage[stateName]) {
              		return $q.reject('OAuth 2.0 state parameter mismatch.');
              	}
              	return oauth2.exchangeForToken(oauthData, userData);
              })

			//$auth.authenticate(provider)
			//  .then(function () {
			//  	toastr.success('You have successfully signed in with ' + provider + '!');
			//  	$location.path('/');
			//  })
			//  .catch(function (error) {
			//  	if (error.error) {
			//  		// Popup error - invalid redirect_uri, pressed cancel button, etc.
			//  		toastr.error(error.error);
			//  	} else if (error.data) {
			//  		// HTTP response error from server
			//  		toastr.error(error.data.message, error.status);
			//  	} else {
			//  		toastr.error(error);
			//  	}
			//  });
		};
	}
})();