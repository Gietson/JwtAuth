// auth.service.js

(function () {
	'use strict';

	angular
		.module('app')
		.value('redirectToUrlAfterLogin', { url: '/' })
		.factory('Auth', function Auth($location, $rootScope, $http, $localStorage, $q, redirectToUrlAfterLogin, User) {
			var currentUser = {};
			if ($localStorage.token) {
				currentUser = $localStorage.currentUser;
			}

			return {
				login: function (user, callback) {
					var cb = callback || angular.noop;
					var deferred = $q.defer();

					$http.post('api/auth/local', {
						email: user.email,
						password: user.password
					})
						.then(function (response) {
							var data = response.data;
							$localStorage.token = data.token;
							currentUser = data.user;
							$localStorage.currentUser = currentUser;
							deferred.resolve(data);
							return cb();
						}, function (err) {
							this.logout();
							deferred.reject(err);
							return cb(err);
						}.bind(this));

					return deferred.promise;
				},
				logout: function () {
					delete $localStorage.token;
					currentUser = {};
				},
				redirectToAttemptedUrl: function () {
					$location.path(redirectToUrlAfterLogin.url);
				},
				createUser: function (user, callback) {
					var cb = callback || angular.noop;

					return User.save(user,
						function (data) {
							//$localStorage.token = data.token;
							//currentUser = User.get();
							return cb(user);
						},
						function (err) {
							this.logout();
							return cb(err);
						}.bind(this)).$promise;
				},
				getCurrentUser: function () {
					return currentUser;
				},
				isLoggedIn: function () {
					return currentUser.hasOwnProperty('role');
				},
				isLoggedInAsync: function (cb) {
					if (currentUser.hasOwnProperty('$promise')) {
						currentUser.$promise.then(function () {
							cb(true);
						}).catch(function () {
							cb(false);
						});
					} else if (currentUser.hasOwnProperty('role')) {
						cb(true);
					} else {
						cb(false);
					}
				},
				isAdmin: function () {
					return currentUser.role === 'admin';
				}
			};

		});

})();