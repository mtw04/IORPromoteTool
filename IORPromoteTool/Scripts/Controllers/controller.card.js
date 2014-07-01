angular.module('controller.card', []).

controller('CardCtrl', ['$scope', '$state', 'CrudAPI', function ($scope, $state, CrudAPI) {

    /* Initialize Delete button */
    $scope.isDelete = $state.params.isDelete;

    /* Parent State */
    $scope.$watch('$state.current.name', function (state) {
        if (state == 'card.details')
            $scope.parentState = true;
        else
            $scope.parentState = false;
    });

    /* Populate Card */
    var initialWatch = $scope.$watch('itemsDict', function (itemsDict) {
        if (itemsDict) {
            $scope.item = angular.copy($scope.itemsDict[$state.params.id]);
            initialWatch();
        }
    });

    $scope.$on('$stateChangeSuccess', function () {
        if ($scope.itemsDict) {
            $scope.item = angular.copy($scope.itemsDict[$state.params.id]);
        }
    });

    /* Promote */
    $scope.promote = function (item) {
        $scope.disabledButton = !$scope.disabledButton;
        CrudAPI('PromoteCardApi').post(item,
            function (success) {
                $state.transitionTo('card.details.approve', { id: item.Id });
            },
            function (error) {
                $scope.errorMessage = error;
            });
    };

    /* Reject */
    $scope.reject = function (item) {
        $scope.disabledButton = !$scope.disabledButton;
        CrudAPI('RejectCardApi').post(item, 
            function (success) {
                $state.transitionTo('card.details.reject', { id: item.Id });
            },
            function (error) {
                $scope.formError = "Please correct form error, then re-submit";
            });
    };


    /* Cancel */
    $scope.returnToList = function () {
        $state.transitionTo('card', {}, { reload: true });
    };

}]);
