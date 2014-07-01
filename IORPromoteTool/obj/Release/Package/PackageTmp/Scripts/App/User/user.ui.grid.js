angular.module('user.ui.grid', ['ui.grid']).

constant('UserGridColumns',
{
    UserName: { displayName: "User Name", visible: true },
    FullName: { displayName: "Full Name", visible: true },
    Frequency: { displayName: "Frequency", visible: true },
    LastPromote: { displayName: "Last Promotion Date", visible: true }
}).

factory('UserTransformer', ['$filter', '$window', function ($filter, $window) {

    TransformData = function (data) {
        var transformedData = [];
        angular.forEach(data, function (user) {

            transformedData.push
            ({
                // Need to pass Id around for finding with dictionary
                Id: { value: user.Id },
                UserName: { value: user.Name },
                FullName: { value: user.FullName },
                Frequency: { value: user.Frequency },
                LastPromote: { value: user.LastPromote, display: $filter('date')(user.LastPromote, 'MM/dd/yyyy') }
            })
        });

        return transformedData;
    };

    return {
        TransformData: TransformData
    };
}]).

directive('userGrid', ['$q', 'UserGridColumns', 'UserTransformer',
function ($q, UserGridColumns, UserTransformer) {
    return {
        restrict: 'E',
        scope:
        {
            data: '=',
            onSelectItem: '='
        },
        controller: ['$scope', '$element', '$attrs', function ($scope, $element, $attrs) {

            $scope.users = [];

            // When the search results change, update the grid with the data.
            $scope.$watch('data', function (data) {
                if (data) {
                    $scope.users = UserTransformer.TransformData(data);
                }
            });

            // Copy the initial column list from the GridColumns dictionary.
            $scope.columns = angular.copy(UserGridColumns);

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
            '<div class="user-grid"><grid-container data="users" columns="columns" on-select-item="onSelect"' +
                                 'height="height">' + 
           '</grid-container></div>',
        replace: true
    };
}]);


