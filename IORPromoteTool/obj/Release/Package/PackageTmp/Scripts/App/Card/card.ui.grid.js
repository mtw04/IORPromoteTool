angular.module('card.ui.grid', ['ui.grid']).

constant('CardGridColumns',
{
    Id: { displayName: "Request ID", visible: true, bindMode: 'ng-bind-html' },
    Submitter: { displayName: "Submitter", visible: true },
    Title: { displayName: "Title", visible: true },
    Priority: { displayName: "Priority", visible: true },
    System: { displayName: "System", visible: true },
    Type: { displayName: "Type", visible: true },
    DueDate: { displayName: "Due Date", visible: true }
}).

factory('CardTransformer', ['$filter', '$window', function ($filter, $window) {

    TransformData = function (data) {
        var transformedData = [];
        angular.forEach(data, function (card) {

            var baseUrl = "http://laserfiche.leankit.com/Boards/View/";
            var boardId = "39987565";
            var fullURL = baseUrl + boardId + "/";

            transformedData.push
            ({
                // Need to pass Id around for finding with dictionary
                Id: { value: card.Id, display: '<a href="' + fullURL + card.Id + '">' + card.Id + '</a>' },
                Submitter: { value: card.Submitter },
                Title: { value: card.Title },
                Priority: { value: card.PriorityText },
                System: { value: card.Tags },
                Type: { value: card.TypeName },
                DueDate: { value: card.DueDate, display: $filter('date')(card.DueDate, 'MM/dd/yyyy') }
            })
        });

        return transformedData;
    };

    return {
        TransformData: TransformData
    };
}]).

directive('cardGrid', ['$q', 'CardGridColumns', 'CardTransformer',
function ($q, CardGridColumns, CardTransformer) {
    return {
        restrict: 'E',
        scope:
        {
            data: '=',
            onSelectItem: '='
        },
        controller: ['$scope', '$element', '$attrs', function ($scope, $element, $attrs) {

            $scope.cards = [];

            // When the search results change, update the grid with the data.
            $scope.$watch('data', function (data) {
                if (data) {
                    $scope.cards = CardTransformer.TransformData(data);
                }
            });

            // Copy the initial column list from the GridColumns dictionary.
            $scope.columns = angular.copy(CardGridColumns);

            // Handling for selecting an item
            $scope.onSelect = function (item) {
                var processedItem = {};
                angular.forEach(item, function (value, key) {
                    processedItem[key] = value.value;
                    //console.log(value);
                });
                $scope.onSelectItem(processedItem);
            };

        }],
        template:
            '<div class="card-grid"><grid-container data="cards" columns="columns" on-select-item="onSelect"' +
                                 'height="height">' + 
           '</grid-container></div>',
        replace: true
    };
}]);


