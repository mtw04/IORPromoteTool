angular.module('controller.card.promote', []).

controller('CardPromoteCtrl', ['$scope', '$state', 'CrudAPI', function ($scope, $state, CrudAPI) {

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

    /* Promote */
    $scope.promote = function (card) {
        CrudAPI('PromoteCardAPI').post(card,
            function (success) {
                $state.transitionTo('card.approve', {}, { reload: true });
            },
            function (error) {
                $scope.errorMessage = error;
            });
    }

    /* Cancel */
    $scope.returnToList = function () {
        $state.transitionTo('card');
    }

}]);