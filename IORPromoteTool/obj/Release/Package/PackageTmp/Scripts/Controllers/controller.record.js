angular.module('controller.record', ['ui.grid']).

controller('RecordCtrl', ['$scope', '$state', 'CrudAPI', function ($scope, $state, CrudAPI) {

    /* User Index*/
    var getRecords = function () {
    CrudAPI('PromoteRecordAPI').query({},
        function (records) {
            $scope.records = records;
            $scope.recordsDict = {}
            angular.forEach(records, function (record) {
                $scope.recordsDict[record.Id] = record;
            });
        },
        function (error) {
            $scope.errorMessage = error;
        });
    };

    getRecords();

    /* Refresh Index */
    $scope.refresh = function () {
        getRecords();
    };

    /* Select Item */
    var selectedItem;

    $scope.onSelectItem = function (item) {
        selectedItem = item;
    }

    /* Action Buttons */
    $scope.detailsMode = function () {

        // Navigate to edit mode if an item is selected
        if (selectedItem != undefined) {
            $state.transitionTo('record.details', { id: selectedItem.Id });
        }
    }

}]);
