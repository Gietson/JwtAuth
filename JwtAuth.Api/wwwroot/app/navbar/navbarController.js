(function () {

	'use strict';

	angular
		.module('app')
		.controller('NavbarController', ['$scope', '$rootScope', 'Auth', '$location', function ($scope, $rootScope, Auth, $location) {

			$rootScope.isLoggedIn = Auth.isLoggedIn;
			$rootScope.isAdmin = Auth.isAdmin;
			$scope.getCurrentUser = Auth.getCurrentUser;

			$scope.logout = function () {
				Auth.logout();
				$location.path('/login');
			};

			$scope.isActive = function (route) {
				return route === $location.path();
			};

		}]);

})();