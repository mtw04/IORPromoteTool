angular.module('api.crud', ['ngResource']).

factory('CrudAPI', ['$resource', function ($resource)
{
    return function (type) {
        return $resource('api/' + type + '/:id', { id: '@id' }, {
            get: { method: 'GET'},
            query: { method: 'GET', isArray: true },
            post: { method: 'POST' },
            update: { method: 'PUT' }, 
            remove: { method: 'DELETE' }
        });
    };
}]);