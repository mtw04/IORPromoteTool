angular.module('api.specific', ['ngResource']).

factory('SpecificAPI', ['$resource', function ($resource)
{
    return function (type) {
        //return $resource('specificApi/' + type + '/:action' + '/:id', { action: '@action', id: '@id' }, {
        return $resource('specificApi/' + type + '/:action' + '/:id', { action: '@action', id: '@id' }, {
            get: { method: 'GET'},
            query: { method: 'GET' },
            post: { method: 'POST' },
            update: { method: 'PUT' }, 
            remove: { method: 'DELETE' }
        });
    };
}]);