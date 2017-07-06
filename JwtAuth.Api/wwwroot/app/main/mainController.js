// homeController.js

(function () {

	'user strict';

	angular
		.module('app')
		.controller('mainController', mainController);

	function mainController($http) {
		var vm = this;

		vm.values = [];

		$http.get('api/values')
			.then(function (response) {
				vm.values = response.data;
			}, function (err) {
				//console.log('[mainController] err=' + JSON.stringify(err));
			});

	}

})();