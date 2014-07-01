angular.module('controller.user.create', []).

controller('UserCreateCtrl', ['$scope', '$state', 'CrudAPI', function ($scope, $state, CrudAPI) {

    /* Save */
    $scope.save = function (user) {
        CrudAPI('PromoteUserAPI').post(user,
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