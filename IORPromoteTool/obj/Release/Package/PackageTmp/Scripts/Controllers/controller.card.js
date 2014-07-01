angular.module('controller.card', ['ui.grid']).

controller('CardCtrl', ['$scope', '$state', 'CrudAPI', function ($scope, $state, CrudAPI) {

    /* Card Index*/
    var getCards = function () {
    CrudAPI('PromoteCardAPI').query({},
        function (cards) {
            $scope.cards = cards;
            $scope.cardsDict = {}
            angular.forEach(cards, function (card) {
                $scope.cardsDict[card.Id] = card;
            });
        },
        function (error) {
        });
    };

    getCards();

    /* Refresh Index */
    $scope.refresh = function () {
        getCards();
    };

    /* Select Item */
    var selectedItem;

    $scope.onSelectItem = function (item) {
        selectedItem = item;
    }

    /* Action Buttons */
    $scope.promoteMode = function () {

        // Navigate to edit mode if an item is selected
        if (selectedItem != undefined) {
            $state.transitionTo('card.promote', { id: selectedItem.Id });
        }
    }

}]);
