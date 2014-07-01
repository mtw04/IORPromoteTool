angular.module('main', [
    'ngResource',
    'ui.router',
    'ui',
    'card.ui.grid',
    'user.ui.grid',
    'record.ui.grid', // refactor
    'controller',
    'api.crud'
]).

config(['$stateProvider', '$urlRouterProvider',
function ($stateProvider, $urlRouterProvider) {

    /* Redirects */
    $urlRouterProvider.
        when('', '/CardIndex').
        when('/', '/CardIndex').
        otherwise("");

    /* States */
    $stateProvider.
        state('card', {
            url: '/CardIndex',
            templateUrl: 'PartialViews/Card/CardIndex.html',
            controller: 'CardCtrl',
            type: 'Card'
        }).

        state('card.promote', {
            url: '/CardPromote/:id',
            templateUrl: 'PartialViews/Card/CardDetails.html',
            controller: 'CardPromoteCtrl'
        }).

        state('card.approve', {
            url: '/CardApprove/:id',
            templateUrl: 'PartialViews/Card/CardApprove.html',
            controller: 'CardResultCtrl'
        }).

        state('user', {
            url: '/UserIndex',
            templateUrl: 'PartialViews/User/UserIndex.html',
            controller: 'UserCtrl',
            type: 'User'
        }).

        state('user.edit', {
            url: '/UserEdit/:id',
            templateUrl: 'PartialViews/User/UserDetails.html',
            controller: 'UserEditCtrl'

        }).

        state('user.delete', {
            url: '/UserDelete/:id',
            templateUrl: 'PartialViews/User/UserDetails.html',
            controller: 'UserDeleteCtrl',
            isDeleteRequested: true
        }).

        state('user.create', {
            url: '/UserCreate',
            templateUrl: 'PartialViews/User/UserDetails.html',
            controller: 'UserCreateCtrl'
        }).

        state('record', {
            url: '/RecordIndex',
            templateUrl: 'PartialViews/Record/RecordIndex.html',
            controller: 'RecordCtrl',
            type: 'Record'
        }).

        state('record.details', {
            url: '/RecordDetails/:id',
            templateUrl: 'PartialViews/Record/RecordDetails.html',
            controller: 'RecordDetailsCtrl'

        }).

        state('faq', {
            url: '/FAQ',
            templateUrl: 'PartialViews/FAQ.html'
        });

}]).

/* Module run, mostly for setting some rootScope variables. */
run(['$rootScope', '$state', '$stateParams',
function ($rootScope, $state, $stateParams) {
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
}]);


