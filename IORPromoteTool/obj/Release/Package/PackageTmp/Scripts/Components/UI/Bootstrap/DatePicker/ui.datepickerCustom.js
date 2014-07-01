angular.module('ui.datepickerCustom', ['ui.bootstrap.datepicker']). //refactor datepicker name

config(function (datepickerConfig) {

    datepickerConfig.showWeeks = false;

}).

config(function (datepickerPopupConfig) {

    datepickerPopupConfig.closeOnDateSelection = true;

    datepickerPopupConfig.appendToBody = true;

    datepickerPopupConfig.closeText = "Close";

    //datepickerPopupConfig.datepickerPopup = "yyyy-MM-dd";

}).

directive('datepickerCustom', ['$scope', function ($scope) {

    //$scope.today = function () {
    //    $scope.dt = new Date();
    //};
    //$scope.today();

    //$scope.showWeeks = false;
    //$scope.toggleWeeks = function () {
    //    $scope.showWeeks = !$scope.showWeeks;
    //};

    //$scope.clear = function () {
    //    $scope.dt = null;
    //};

    // Disable weekend selection
    //$scope.disabled = function (date, mode) {
    //    return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    //};

    //$scope.toggleMin = function () {
    //    $scope.minDate = ($scope.minDate) ? null : new Date();
    //};
    //$scope.toggleMin();

    //$scope.open = function ($event) {
    //    $event.preventDefault();
    //    $event.stopPropagation();

    //    $scope.opened = true;
    //};

    //$scope.dateOptions = {
    //    'year-format': "'yy'",
    //    'starting-day': 1
    //};

    //$scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'shortDate'];
    //$scope.format = $scope.formats[0];

}]);

