angular.module('controller.user', ['ui.grid']).

controller('UserCtrl', ['$scope', '$state', 'CrudAPI', function ($scope, $state, CrudAPI) {

    /* User Index*/
    var getUsers = function () {
    CrudAPI('PromoteUserAPI').query({},
        function (users) {
            $scope.users = users;
            $scope.usersDict = {}
            angular.forEach(users, function (user) {
                $scope.usersDict[user.Id] = user;
            });
        },
        function (error) {
            $scope.errorMessage = error;
        });
    };

    getUsers();

    /* Refresh Index */
    $scope.refresh = function () {
        getUsers();
    };

    /* Select Item */
    var selectedItem;

    $scope.onSelectItem = function (item) {
        selectedItem = item;
    }

    /* Action Buttons */
    $scope.createMode = function () {
        $state.transitionTo('user.create');
    }

    $scope.editMode = function () {

        // Navigate to edit mode if an item is selected
        if (selectedItem != undefined) {
            $state.transitionTo('user.edit', { id: selectedItem.Id });
        }
    }

    $scope.deleteMode = function () {

        // Navigate to delete mode if an item is selected
        if (selectedItem != undefined) {
            $state.transitionTo('user.delete', { id: selectedItem.Id });
        }
    }

}]).

service('ApiService', function () {
});