// Laserfiche  - Copyright(c) 1993-2013 Compulink Management Center, Inc.

/**
 * @name ui.bootstrap.progress
 **/
angular.module('ui.bootstrap.progress', []).

/**
 * @ngdoc directive
 * @name components.directive:progressIndicator
 * @requires $timeout Angular's timeout wrapper.
 * @restrict E
 * @description
 * A progress indicator UI element.
 * 
 * @param {string} cssClass The css class the progress should use (should at least include progress).
 * @param {string expression} text The text to display above the loading bar of the progress indicator.
 * @param {int} interval The time interval at which the progress loading bar updates (grows by 20%).
 *
 * @example
    <example module="ui.bootstrap.progress">
        <file name="index.html">
            <progress-indicator css-class="progress progress-striped active" text="'Loading'" interval="1000"></progressIndicator>
        </file>
    </example>
 **/
directive('progressIndicator', ['$timeout', function ($timeout)
{
    var widthPercentage = 20;
    return {
        restrict: 'E',
        replace: true,
        transclude: false,
        scope: 
        {
            text: '&',
            interval: '&',
        },
        link: function (scope, element, attrs)
        {
            /** 
             * @name updateInterval
             * @description 
             * Updates the inner length of the progress bar until it reaches 100%.
             **/
            var updateInterval = function ()
            {
                $timeout(function ()
                {
                    widthPercentage += 20;
                    scope.barStyle.width = widthPercentage + '%';
                    if (widthPercentage != 100)
                    {
                        updateInterval();
                    }

                }, scope.interval());
            };

            // Initial bar css style.
            scope.barStyle = { width: widthPercentage + '%' };

            // Start the update cycle.
            updateInterval();
        },
        template: function (element, attrs)
        {
            var template =
                '<div>' +
                    '<h4 ng-bind="text"></h4>' +
                    '<div class="' + attrs.cssClass + '">' +
                        '<div class="bar" ng-style="barStyle"></div>' +
                    '</div>' +
                '</div>';
            return template;
        }
    };
}]);