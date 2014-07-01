angular.module('controller.card.result', []).

controller('CardResultCtrl', ['$scope', '$state', 'CrudAPI', function ($scope, $state, CrudAPI) {

    /* Populate Card */
    var initialWatch = $scope.$watch('cardsDict', function (cardsDict) {
        if (cardsDict) {
            $scope.card = angular.copy($scope.cardsDict[$state.params.id]);
            initialWatch();
        }
    });

    $scope.$on('$stateChangeSuccess', function () {
        if ($scope.cardsDict) {
            $scope.card = angular.copy($scope.cardsDict[$state.params.id]);
        }
    });

    /* Back */
    $scope.returnToList = function () {
        $state.transitionTo('card');
    }

}]);