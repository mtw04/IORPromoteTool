angular.module('main', [
    'ngResource',
    'controller',
    'api',
    'item.grid',
    'ui.router',
    'components'
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
            templateUrl: 'PartialViews/Grid/GridIndex.html',
            controller: 'GridCtrl',
            data: {
                type: "Card"
            }
        }).

        state('card.details', {
            url: '/CardDetails/:id?isDelete',
            templateUrl: 'PartialViews/Card/CardDetails.html',
            controller: 'CardCtrl'
        }).

        state('card.details.approve', {
            url: '/CardApprove',
            templateUrl: 'PartialViews/Card/CardApprove.html',
            controller: 'CardCtrl'
        }).

        state('card.details.reject', {
            url: '/CardReject',
            templateUrl: 'PartialViews/Card/CardReject.html',
            controller: 'CardCtrl'
        }).

        state('user', {
            url: '/UserIndex',
            templateUrl: 'PartialViews/Grid/GridIndex.html',
            controller: 'GridCtrl',
            data: {
                type: "User"
            }
        }).

        state('user.details', {
            url: '/UserDetails/:id?isDelete',
            templateUrl: 'PartialViews/User/UserDetails.html',
            controller: 'UserCtrl',
            //params: ['isDelete', 'id']
        }).

        state('record', {
            url: '/RecordIndex',
            templateUrl: 'PartialViews/Grid/GridIndex.html',
            controller: 'GridCtrl',
            data: {
                type: "Record"
            }
        }).

        state('record.details', {
            url: '/RecordDetails/:id',
            templateUrl: 'PartialViews/Record/RecordDetails.html',
            controller: 'RecordCtrl'
        }).

        state('faq', {
            url: '/FAQ',
            templateUrl: 'PartialViews/FAQ/FAQ.html'
        }).

        state('chart', {
            url: '/Chart',
            templateUrl: 'PartialViews/Chart/Chart.html',
            controller: 'ChartCtrl'
        });
}]).

/* Module run, mostly for setting some rootScope variables. */
run(['$rootScope', '$state', '$stateParams',
function ($rootScope, $state, $stateParams) {
    $rootScope.$state = $state;
    $rootScope.$stateParams = $stateParams;
}]);


