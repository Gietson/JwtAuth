// registerController.js

(function () {

	'user strict';

	angular
		.module('app')
		.controller('registerController', registerController);

	function registerController(Auth) {
		var vm = this;
		vm.error = '';

		vm.register = function () {
			vm.loading = true;

			var user = { email: vm.email, username: vm.username, password: vm.password };
			Auth.createUser(user)
				.then(function () {
					// Logged in, redirect to the page with requested a login
					vm.loading = false;
					Auth.redirectToAttemptedUrl();
				})
				.catch(function (err) {
					console.log('register err: ' + err);
					vm.error = err.message;
					vm.loading = false;
				});
		};
	}

})();