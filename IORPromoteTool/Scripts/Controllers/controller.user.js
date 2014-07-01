angular.module('controller.user', []).

controller('UserCtrl', ['$scope', '$state', 'CrudAPI', function ($scope, $state, CrudAPI) {

    /* Toggle Delete button */
    $scope.isDelete = $state.params.isDelete;
    
    /* Populate User */
    var initialWatch = $scope.$watch('itemsDict', function (itemsDict) {
        if (itemsDict) {
            $scope.item = angular.copy($scope.itemsDict[$state.params.id]);
            initialWatch();

            // Set Default Frequency if undefined
            if ($scope.item == undefined) {
                var date = new Date(new Date().setHours(17, 0, 0)); // Mike: Set default time to 17:00 so it will be converted correctly to the current date when saved into the DB as UTC.
                $scope.item = { Frequency: 5, LastPromote: date };
            }
        }
    });

    $scope.$on('$stateChangeSuccess', function () {
        if ($scope.itemsDict) {
            $scope.item = angular.copy($scope.itemsDict[$state.params.id]);
        }
    });

    /* Save */
    $scope.save = function (item) {
        $scope.disabledButton = !$scope.disabledButton;

        // Edit User
        if (item.Id != undefined) {
            CrudAPI('PromoteUserApi').update({ id: item.Id }, item,
                function (success) {
                    $state.transitionTo('user', {}, { reload: true });
                },
                function (error) {
                    $scope.formError = "Please correct form error, then re-submit";
                });
        }
        // Add User
        else {
            CrudAPI('PromoteUserApi').post(item,
                function (success) {
                    $state.transitionTo('user', {}, { reload: true });
                },
                function (error) {
                    $scope.formError = "Please correct form error, then re-submit";
                });
        }
    };

    /* Delete */
    $scope.delete = function (item) {
        $scope.disabledButton = !$scope.disabledButton;

        CrudAPI('PromoteUserApi').remove({ id: item.Id },
            function (success) {
                $state.transitionTo('user', {}, { reload: true });
            },
            function (error) {
                $scope.errorMessage = error;
            });
    };

    /* Cancel */
    $scope.returnToList = function () {
        $state.transitionTo('user');
    };

    /* Datepicker Button */
    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.opened = true;
    };

    /* Frequency Option List */
    CrudAPI('PromoteFrequencyApi').query({},
        function (options) {
            $scope.options = options;
        },
        function (error) {
            $scope.errorMessage = error;
        });
}]);

