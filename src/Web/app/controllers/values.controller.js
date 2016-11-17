(function () {
	'use strict';

	angular
		.module('module.app')
		.controller("ValuesController", ValuesController);

	ValuesController.$inject = ['httpService'];

	function ValuesController(httpService) {

		var vm = this;
		vm.values = [];

		activate();

		function activate() {
			httpService.get("http://localhost:37227/api/v1/values").then(function (values) {
				vm.values = values;
			});
		}
	}
})();