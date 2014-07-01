angular.module('ui.datepickerCustom', ['ui.bootstrap.datepicker']).

config(function (datepickerConfig) {

    datepickerConfig.showWeeks = false;

}).

config(function (datepickerPopupConfig) {

    datepickerPopupConfig.closeOnDateSelection = true;
    datepickerPopupConfig.appendToBody = true;
    datepickerPopupConfig.closeText = "Close";
    datepickerPopupConfig.datepickerPopup; // = "MM/dd/yyyy";

});
