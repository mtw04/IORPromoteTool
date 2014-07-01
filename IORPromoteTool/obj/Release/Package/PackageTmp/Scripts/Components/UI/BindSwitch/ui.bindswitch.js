// Laserfiche  - Copyright(c) 1993-2013 Compulink Management Center, Inc.

/**
 * @name ui.bindswitch.
 **/
angular.module('ui.bindswitch', ['ngSanitize'])

/**
 * @ngdoc directive
 * @name components.directive:bindSwitch
 * @requires $sanitize Angular's sanitize service for binding safe HTML.
 * @description
 * A directive that can use ng-bind, ng-bind-html, ng-bind-html-unsafe depending on the value specified.
 * 
 * @param {string} bindSwitch The value of the binding.
 * @param {string} bindSwitchValue The type of binding to use (ng-bind-html, ng-bind-html-unsafe, or ng-bind)
 *
 * @example
    <example module="ui.bindswitch">
        <file name="index.html">
            <div ng-controller="Ctrl">
                ng-bind<div class="bs-callout bs-callout-block" bind-switch="name" bind-switch-value="ng-bind"></div>
                ng-bind-html<div class="bs-callout bs-callout-block" bind-switch="name" bind-switch-value="ng-bind-html"></div>
                ng-bind-html-unsafe<div class="bs-callout bs-callout-block" bind-switch="name" bind-switch-value="ng-bind-html-unsafe"></div>
            </div>
        </file>
        <file name="script.js">
            function Ctrl($scope)
            {
                $scope.name = '<p style="color:blue">an html <em onmouseover="this.textContent=\'PWN3D!\'">click here</em>snippet</p>';
            }
        </file>
    </example>
 **/
.directive("bindSwitch", ['$sanitize', function ($sanitize)
{
    return function (scope, element, attr)
    {
        element.addClass('ng-binding').data('$binding', attr.bindSwitch);

        /** 
         * @name update
         * @description 
         * Execute angular's bind functions depending on what the bindSwitchValue is.
         **/
        function update()
        {
            var value = scope.$eval(attr.bindSwitch);
            var bindOption = attr.bindSwitchValue;

            if (bindOption == "ng-bind-html")
            {
                value = $sanitize(value);
                element.html(value || '');
            }

            else if (bindOption == "ng-bind-html-unsafe")
            {
                element.html(value || '');
            }

            else if (bindOption == "ng-bind")
            {
                element.text(value == undefined ? '' : value);
            }
        }

        // Update if either of the attributes change.
        scope.$watch(attr.bindSwitch, update);
        attr.$observe('bindSwitchValue', update);
    };
}]);