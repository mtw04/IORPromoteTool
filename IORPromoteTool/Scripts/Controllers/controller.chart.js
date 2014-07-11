angular.module('controller.chart', ['googlechart'])

.controller('ChartCtrl', ['$scope', 'CrudAPI', 'SpecificAPI', function ($scope, CrudAPI, SpecificAPI) {

    /* Chart Object */
    // Chart object needs to be initialized first in order for the columns and rows to be populated by synchronous calls
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

    /* Get Chart Years */
    var getChartYears = function () {
        SpecificAPI('RequestSpecificApi').query({ action: "GetChartYears" },
            function (options) {
                $scope.options = options;
            }
            );
    };
    getChartYears();

    /* Watch Chart Years */
    var chartYearWatch = $scope.$watch('chartyear', function () {
        getChartMonths();
    });

    /* Get Chart Months */
    var getChartMonths = function () {
        SpecificAPI('RequestSpecificApi').query({ action: "GetChartMonths", year: $scope.chartyear || 0}, // Initialize to 0
             function (options) {
                 $scope.options = options;
             }
             );
    };

    /* Watch Chart Months */
    var chartMonthWatch = $scope.$watch('chartmonth', function () {
        setChartRows();
    });

    //$scope.chartyear = 0;
    //$scope.chartmonth = 0;

    /* Set Chart Rows */
    var setChartRows = function () {
        SpecificAPI('RequestSpecificApi').query({ action: "GetDepDict", year: $scope.chartyear || 0, month: $scope.chartmonth || 0}, // API call is asynchronous
            function (deps) {
                $scope.deps = deps;
                $scope.chartObject.data.rows.length = 0; //clear array

                angular.forEach(deps.Departments, function (value, key) {
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
