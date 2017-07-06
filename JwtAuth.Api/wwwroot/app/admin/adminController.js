// adminController.js

(function () {

	'user strict';

	angular
		.module('app')
		.controller('adminController', adminController);

	function adminController(User) {
		var vm = this;
		//vm.error = '';

		vm.users = {};

		 User.query(function (data) {
			 vm.users = data;
		}, function (err) {
			console.log('users err= ' + JSON.stringify(err));
		});

	}

})();