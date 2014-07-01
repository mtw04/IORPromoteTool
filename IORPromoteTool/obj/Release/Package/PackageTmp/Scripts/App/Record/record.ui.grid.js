angular.module('record.ui.grid', ['ui.grid']).

constant('RecordGridColumns',
{
    CardId: { displayName: "Request Id", visible: true, bindMode: 'ng-bind-html' },
    PromoteDate: { displayName: "Promotion Date", visible: true },
    Promoter: { displayName: "Promoter", visible: true },
    Submitter: { displayName: "Submitter", visible: true },
    Title: { displayName: "Card Title", visible: true }
    //CurrentLane: { displayName: "Current Lane", visible: true } // Only include in Details and use API call?
}).

factory('RecordTransformer', ['$filter', '$window', function ($filter, $window) {

    TransformData = function (data) {
        var transformedData = [];
        angular.forEach(data, function (record) {

            var baseUrl = "http://laserfiche.leankit.com/Boards/View/";
            var boardId = "39987565";
            var fullURL = baseUrl + boardId + "/";

            transformedData.push
            ({
                // Need to pass Id around for finding with dictionary
                Id: { value: record.Id },
                CardId: { value: record.CardId, display: '<a href="' + fullURL + record.CardId + '">' + record.CardId + '<a>' },
                PromoteDate: { value: record.PromoteDate, display: $filter('date')(record.PromoteDate, 'MM/dd/yyyy') },
                Promoter: { value: record.Promoter },
                Submitter: { value: record.Submitter },
                Title: { value: record.Title }
            })
        });

        return transformedData;
    };

    return {
        TransformData: TransformData
    };
}]).

directive('recordGrid', ['$q', 'RecordGridColumns', 'RecordTransformer',
function ($q, RecordGridColumns, RecordTransformer) {
    return {
        restrict: 'E',
        scope:
        {
            data: '=',
            onSelectItem: '='
        },
        controller: ['$scope', '$element', '$attrs', function ($scope, $element, $attrs) {

            $scope.records = [];

            // When the search results change, update the grid with the data.
            $scope.$watch('data', function (data) {
                if (data) {
                    $scope.records = RecordTransformer.TransformData(data);
                }
            });

            // Copy the initial column list from the GridColumns dictionary.
            $scope.columns = angular.copy(RecordGridColumns);

            // Handling for selecting an item
            $scope.onSelect = function (item) {
                var processedItem = {};
                angular.forEach(item, function (value, key) {
                    processedItem[key] = value.value;
                });
                $scope.onSelectItem(processedItem);
            };

        }],
        template:
            '<div class="record-grid"><grid-container data="records" columns="columns" on-select-item="onSelect"' +
                                 'height="height">' + 
           '</grid-container></div>',
        replace: true
    };
}]);


