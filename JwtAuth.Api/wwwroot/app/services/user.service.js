// user.service.js

(function () {
	'use strict';

	angular
		.module('app')
		//.factory('User', function ($resource) {
		//	return $resource('/api/user/:username', {
		//		id: '@_id'
		//	},
		//		{
		//			get: {
		//				method: 'GET',
		//				params: {
		//					username: 'me'
		//				}
		//			}
		//		});
		//});
		.factory('User', function ($resource) {
			return $resource('/api/user/:username', {
				id: '@_id'
			},
				{
					get: {
						method: 'GET',
						params: {
							username: 'me'
						}
					}
				});
		});

})();