angular.module('controller.record', []).

controller('RecordCtrl', ['$scope', '$state', 'CrudAPI', function ($scope, $state, CrudAPI) {

    /* Populate Record */
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

    /* Action Buttons */
    $scope.returnToList = function () {
        $state.transitionTo('record');
    };
}]);
