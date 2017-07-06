// loginController.js

(function () {

	'user strict';

	angular
		.module('app')
		.controller('loginController', loginController);

	function loginController(Auth) {
		var vm = this;
		vm.error = '';

		vm.login = function () {
			vm.loading = true;

			var data = { email: vm.email, password: vm.password };
			Auth.login(data)
				.then(function () {
					// Logged in, redirect to the page with requested a login
					vm.loading = false;
					Auth.redirectToAttemptedUrl();
				})
				.catch(function (err) {
					vm.error = err.message;
					vm.loading = false;
				});
		};
	}

})();