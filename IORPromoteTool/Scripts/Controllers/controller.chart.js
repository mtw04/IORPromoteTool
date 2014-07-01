angular.module('controller.chart', ['googlechart'])

.controller('ChartCtrl', ['$scope', 'CrudAPI', 'SpecificAPI', function ($scope, CrudAPI, SpecificAPI) {

    /* Chart Object */
    $scope.chartObject = {
        "type": "PieChart",
        "displayed": false,
        "data": {
            "cols": [],
            "rows": [],
        },
        "options": {
            "title": "Requests per Department",
            "width": "900",
            "height": "900",
        },
        "formatters": {},
        "view": {}
    }

    /* Item Index */
    var getItems = function () {
        CrudAPI('RequestApi').query({},
            function (items) {
                $scope.items = items;
            },
            function (error) {
                $scope.errorMessage = error;
            });
    };
    getItems();

    /* Set Chart Rows */
    var setChartRows = function () {
        SpecificAPI('RequestSpecificApi').query({ action: "GetDepDict", year: 2013, month: 4 }, // API call is asynchronous
            function (items) {
                $scope.items = items;
                angular.forEach(items.Departments, function (value, key) {
                    var department = key;
                    var depCount = value;

                    $scope.chartObject.data.rows.push({
                        "c": [
                                {"v": key},
                                {"v": value},
                            ]
                    });
                });
            },
            function (error) {
                $scope.errorMessage = error;
            });
    };
    setChartRows();
    
    /* Set Chart Columns */
    var setChartCols = function () {
        $scope.chartObject.data.cols.push(
            {
                "id": "department",
                "label": "Department",
                "type": "string"
            },
            {
                "id": "departmentCount",
                "label": "Department Count",
                "type": "number"
            }
        );
    };
    setChartCols();

}]);
