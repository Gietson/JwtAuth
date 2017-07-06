// app.js

(function () {
	'use strict';

	angular
		.module('app', [
			'ui.router',
			'ngStorage',
			'ngResource'
		])
		.config(config)
		.run(run);

	function config($stateProvider, $urlRouterProvider, $locationProvider, $httpProvider) {

		$locationProvider.html5Mode(true).hashPrefix('!');

		if (window.location.href.indexOf('#_=_') > 0) {
			window.location = window.location.href.replace(/#.*/, '');
		}

		$httpProvider.interceptors.push(authInterceptor);

		// app routes
		$stateProvider
			.state('main', {
				url: '/',
				templateUrl: 'app/main/main.html',
				controller: 'mainController',
				controllerAs: 'vm'
			})
			.state('login', {
				url: '/login',
				templateUrl: 'app/login/login.html',
				controller: 'loginController',
				controllerAs: 'vm'
			})
			.state('register', {
				url: '/register',
				templateUrl: 'app/register/register.html',
				controller: 'registerController',
				controllerAs: 'vm'
			})
			.state('admin', {
				url: '/admin',
				templateUrl: 'app/admin/admin.html',
				controller: 'adminController',
				controllerAs: 'vm'
			});
		$urlRouterProvider.otherwise("/");
	}

	function run($rootScope, $http, $location, $localStorage) {
		// Redirect to login if route requires auth and you're not logged in
		$rootScope.$on('$stateChangeStart', function (event, next) {
			Auth.isLoggedInAsync(function (loggedIn) {
				if (next.authenticate && !loggedIn) {
					event.preventDefault();
					Auth.saveAttemptUrl();
					$state.go('/login');
				}
			});
		});

		$rootScope.$on('$stateChangeSuccess', function (evt, toState) {
			window.document.title = toState.title + ' - GIE';
		});
	}

	function authInterceptor($rootScope, $q, $localStorage, $location) {

		return {
			// Add authorization token to headers
			request: function (config) {
				config.headers = config.headers || {};
				if ($localStorage.token) {
					config.headers.Authorization = 'Bearer ' + $localStorage.token;
				}
				return config;
			},

			// Intercept 401s and redirect you to login
			responseError: function (response) {
				if (response.status === 401) {
					$location.path('/login');
					// remove any stale tokens
					delete $localStorage.token;
					return $q.reject(response);
				}
				else {
					return $q.reject(response);
				}
			}
		};
	};


})();