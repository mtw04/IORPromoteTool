angular.module('api.account', ['ngResource']).

factory('AccountAPI', ['$resource', function($resource)
{
    return function (type) {
        return $resource('api/' + type, {
            get: { method: 'GET' }
        });
    };
}]);