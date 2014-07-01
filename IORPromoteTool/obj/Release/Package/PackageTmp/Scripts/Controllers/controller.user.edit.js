angular.module('controller.user.edit', ['ui.datepickerCustom']).

controller('UserEditCtrl', ['$scope', '$state', 'CrudAPI', function ($scope, $state, CrudAPI) {

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

    /* Save */
    $scope.save = function (user) {
        CrudAPI('PromoteUserAPI').update({ id: user.Id }, user,
            function (success) {
                $state.transitionTo('user', {}, { reload: true });
            },
            function (error) {
                $scope.formError = "Please correct form error, then re-submit";
            });
    }

    /* Cancel */
    $scope.returnToList = function () {
        $state.transitionTo('user');
    }

}]);

