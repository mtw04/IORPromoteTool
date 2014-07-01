angular.module('controller.main', []).

controller("MainCtrl", ['$scope', '$state', 
function ($scope, $state) {
	
    $scope.userNav = function () {
        $state.transitionTo('user');
    }

    $scope.recordNav = function () {
        $state.transitionTo('record');
    }

    $scope.faqNav = function () {
        $state.transitionTo('faq');
    }

    $scope.cardNav = function () {
        $state.transitionTo('card');
    }

}]);