angular.module('item.grid', ['ui.grid']).

factory('ItemGridColumns', ['$state', function ($state) {
    
    return function () {
        var indexType = $state.current.data.type;

        switch (indexType) {
            case 'User':
                return {
                    FullName: { displayName: "Full Name", visible: true },
                    UserName: { displayName: "User Name", visible: true },
                    Frequency: { displayName: "Frequency", visible: true },
                    LastPromote: { displayName: "Last Promotion Date", visible: true }
                };
            case 'Record':
                return {
                    CardId: { displayName: "Request Id", visible: true, bindMode: 'ng-bind-html' },
                    PromoteDate: { displayName: "Promotion Date", visible: true },
                    Promoter: { displayName: "Promoter", visible: true },
                    Submitter: { displayName: "Submitter", visible: true },
                    Title: { displayName: "Card Title", visible: true }
                };
            case 'Card':
                return {
                    Id: { displayName: "Request ID", visible: true, bindMode: 'ng-bind-html' },
                    Submitter: { displayName: "Submitter", visible: true },
                    Title: { displayName: "Title", visible: true },
                    Priority: { displayName: "Priority", visible: true },
                    System: { displayName: "System", visible: true },
                    Type: { displayName: "Type", visible: true },
                    DueDate: { displayName: "Due Date", visible: true }
                };
            default:
                return;
        };
    };

}]).

factory('ItemTransformer', ['$filter', '$window', '$state', function ($filter, $window, $state) {
    
    var baseUrl = "http://laserfiche.leankit.com/Boards/View/";
    var boardId = "39048680"; // Mike: Test = 39987565, Live = 39048680
    var fullURL = baseUrl + boardId + "/";

    TransformData = function (data) {
        var indexType = $state.current.data.type;
        var transformedData = [];

        angular.forEach(data, function (item) {
            switch (indexType) {
                case 'User':
                    transformedData.push
                    ({
                        // Need to pass Id around for finding with dictionary
                        Id: { value: item.Id },
                        UserName: { value: item.Name },
                        FullName: { value: item.FullName },
                        Frequency: { value: item.Frequency },
                        LastPromote: { value: item.LastPromote, display: $filter('date')(item.LastPromote, 'MM/dd/yyyy') }
                    });
                    break;
                case 'Record':
                    transformedData.push
                    ({
                        // Need to pass Id around for finding with dictionary
                        Id: { value: item.Id },
                        CardId: { value: item.CardId, display: '<a href="' + fullURL + item.CardId + '">' + item.CardId + '<a>' },
                        PromoteDate: { value: item.PromoteDate, display: $filter('date')(item.PromoteDate, 'MM/dd/yyyy') },
                        Promoter: { value: item.Promoter },
                        Submitter: { value: item.Submitter },
                        Title: { value: item.Title }
                    });
                    break;
                case 'Card':
                    transformedData.push
                    ({
                        // Need to pass Id around for finding with dictionary
                        Id: { value: item.Id, display: '<a href="' + fullURL + item.Id + '">' + item.Id + '</a>' },
                        Submitter: { value: item.Submitter },
                        Title: { value: item.Title },
                        Priority: { value: item.PriorityText },
                        System: { value: item.Tags },
                        Type: { value: item.TypeName },
                        DueDate: { value: item.DueDate, display: $filter('date')(item.DueDate, 'MM/dd/yyyy') }
                    });
                    break;
            }
        });
        return transformedData;
    };

    return {
        TransformData: TransformData
    };
}]).

directive('itemGrid', ['$q', 'ItemGridColumns', 'ItemTransformer', '$filter',
function ($q, ItemGridColumns, ItemTransformer, $filter) {
    return {
        restrict: 'E',
        scope:
        {
            data: '=',
            onSelectItem: '=',
            initialSortBy: '=',
            preSort: '='
        },
        controller: ['$scope', '$element', '$attrs', function ($scope, $element, $attrs) {

            $scope.items = [];

            // When the search results change, update the grid with the data.
            $scope.$watch('data', function (data) {
                if (data) {
                    $scope.items = ItemTransformer.TransformData(data);
                }
            });

            // Copy the initial column list from the GridColumns dictionary.
            $scope.columns = angular.copy(ItemGridColumns());

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
        templateUrl: 'PartialViews/Grid/GridItem.html',
        replace: true
    };
}]);


