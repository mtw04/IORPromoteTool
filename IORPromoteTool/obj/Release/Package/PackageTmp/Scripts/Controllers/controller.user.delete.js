angular.module('controller.user.delete', []).

controller('UserDeleteCtrl', ['$scope', '$state', 'CrudAPI', function ($scope, $state, CrudAPI) {

    /* Toggle Delete button */
    $scope.isDeleteRequested = !!$state.current.isDeleteRequested;

    /* Populate User */
    var initialWatch = $scope.$watch('usersDict', function (usersDict) {
        if (usersDict) {
            $scope.user = angular.copy($scope.usersDict[$state.params.id]);
            initialWatch();
        }
    });

    $scope.$on('$stateChangeSuccess', function () {
        if ($scope.usersDict) {
            $scope.user = angular.copy($scope.usersDict[$state.params.id]);
        }
    });

    /* Delete */
    $scope.delete = function (user) {
        CrudAPI('PromoteUserAPI').remove({ id: user.Id },
            function (success) {
                $state.transitionTo('user', {}, { reload: true });
            },
            function (error) {
                $scope.errorMessage = error;
            });
    }

    /* Back to Index */
    $scope.returnToList = function () {
        $state.transitionTo('user');
    }

}]);

//factory('ConvertService', ['Scope', function ($scope) {
//    var self = this;

//    /* Populate User */
//    var initialWatch = $scope.$watch('usersDict', function (usersDict) {
//        if (usersDict) {
//            $scope.user = angular.copy($scope.usersDict[$state.params.id]);
//            initialWatch();
//        }
//    });
//}]);
