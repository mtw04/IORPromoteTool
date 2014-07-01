angular.module('controller.grid', ['ui.grid']).

controller('GridCtrl', ['$scope', '$state', 'CrudAPI', 'AccountAPI', function ($scope, $state, CrudAPI, AccountAPI) {

    /* Initial Sort */
    $scope.initialSortBy = "FullName";
    $scope.preSort = ($state.current.name == 'user');

    /* Admin Check */
    AccountAPI('AccountApi').get({},
        function (account) {
            $scope.isUserAdmin = account.UserAdmin;
            $scope.isRejectAdmin = account.RejectAdmin;
        });

    /* Index Type */
    $scope.indexType = $state.current.data.type;

    /* Parent State */
    $scope.$watch('$state.current.name', function (state) {
        if (state == 'user' || state == 'record' || state == 'card')
            $scope.parentState = true;
        else
            $scope.parentState = false;
    });

    /* Item Index */
    var getItems = function () {
        CrudAPI('Promote' + $scope.indexType + 'Api').query({},
            function (items) {
                $scope.items = items;
                $scope.itemsDict = {}
                angular.forEach(items, function (item) {
                    $scope.itemsDict[item.Id] = item;
                });
            },
            function (error) {
                $scope.errorMessage = error;
            });
    };
    getItems();
    
    /* Refresh Index */
    $scope.refresh = function () {
        getItems();
    };

    /* Select Item - local variable */
    var selectedItem;
    $scope.itemSelect;

    $scope.onSelectItem = function (item) {
        selectedItem = item;
        $scope.itemSelect = true;
    };

    /* Action Buttons */
    $scope.createMode = function () {
        $state.transitionTo('user.details');
    };

    $scope.editMode = function () {
        // Navigate to edit mode if an item is selected
        if (selectedItem != undefined) {
            $state.transitionTo('user.details', { id: selectedItem.Id });
        }
    };

    $scope.deleteMode = function () {
        var isDelete;
        // Navigate to delete mode if an item is selected
        if (selectedItem != undefined) {
            $state.transitionTo('user.details', { id: selectedItem.Id, isDelete: true });
        }
    };

    $scope.detailsMode = function () {
        // Navigate to details mode if an item is selected
        if (selectedItem != undefined) {
            $state.transitionTo('record.details', { id: selectedItem.Id });
        }
    };

    $scope.promoteMode = function () {
        // Navigate to promote mode if an item is selected
        if (selectedItem != undefined) {
            $state.transitionTo('card.details', { id: selectedItem.Id });
        }
    };

    $scope.rejectMode = function () {
        // Navigate to reject mode if an item is selected
        if (selectedItem != undefined) {
            $state.transitionTo('card.details', { id: selectedItem.Id, isDelete: true });
        }
    };

}]).

/* Directive to populate grid headers */
directive('gridHeader', function () {
    return {
        restrict: 'E',
        scope: 
        {
            indexType: '='
        },
        controller: ['$scope', '$element', '$attrs', function (scope, element, attrs) {
            scope.pageHeader = null;

            switch (scope.indexType) {
                case 'User':
                    scope.pageHeader = "Admin Console";
                    break;
                case 'Record':
                    scope.pageHeader = "Promotion History";
                    break;
                case 'Card':
                    scope.pageHeader = "Promotion Tool";
                    break;
            }
        }],
        template:
            '<div class="page-header">{{pageHeader}}</div>'
    };
}).

/* Directive for popuplating grid action buttons */
directive('gridActions', function () {
    return {
        restrict: 'E',
        scope: 
        {
            indexType: '=',
            itemSelect: '=',
            isRejectAdmin: '=',
            createMode: '&',
            editMode: '&',
            deleteMode: '&',
            detailsMode: '&',
            promoteMode: '&',
            rejectMode: '&'
        },
        templateUrl: 'PartialViews/Grid/GridAction.html'
    };
}).

/* Directive for setting focus on input elements */
directive('autoFocus', function ($timeout) {
    return {
        restrict: 'AE',
        link: function (scope, element) {
            $timeout(function () {
                element[0].focus();
            }, 0);
        }
    };
});