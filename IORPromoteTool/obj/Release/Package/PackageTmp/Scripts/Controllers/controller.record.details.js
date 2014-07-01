angular.module('controller.record.details', ['ui.grid']).

controller('RecordDetailsCtrl', ['$scope', '$state', 'CrudAPI', function ($scope, $state, CrudAPI) {

    /* Populate Record */
    var initialWatch = $scope.$watch('recordsDict', function (recordsDict) {
        if (recordsDict) {
            $scope.record = angular.copy($scope.recordsDict[$state.params.id]);
            initialWatch();
        }
    });

    $scope.$on('$stateChangeSuccess', function () {
        if ($scope.recordsDict) {
            $scope.record = angular.copy($scope.recordsDict[$state.params.id]);
        }
    });

    /* Action Buttons */
    $scope.returnToList = function () {
        $state.transitionTo('record');
    }
}]);
