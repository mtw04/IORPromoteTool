/**
 * @name ui.bootstrap.tooltip
 * @description
 * A directive for bootstrap tooltip.
 * This is a mix of AngularUI's bootstrap tooltip directive and my own code so that it does what i want.
 * @link http://angular-ui.github.com
 * @license MIT License, http://www.opensource.org/licenses/MIT
 **/
angular.module('ui.bootstrap.tooltip', ['ui.bootstrap.tooltip.service', 'components.service.callback']).

/**
 * @ngdoc directive
 * @name tooltip.directive:hoverTooltip
 * @restrict E
 * @description
 * A directive to put on an element that displays a tooltip on hover.
 * 
 * @param {string} hoverTooltip The name of the tooltip object that should be displayed when the user hovers over this element.
 * See {@link components.directive:tooltip tooltip}.
 *
 * @example
    <example module="ui.bootstrap.tooltip">
        <file name="index.html">
            <div ng-controller="Ctrl" ng-init="tooltipText='Tooltip!'">
                <input type="text" ng-model="tooltipText" /> <br />
                <span style="position: relative; display: inline-block; height: 30px;">
                    <button hover-tooltip="tooltipRight" class="btn btn-info">Right</button>
                    <tooltip placement="right" tooltip="tooltipRight">{{tooltipText}}</tooltip>
                </span>
                <span style="position: relative; display: inline-block; height: 30px;">
                    <button hover-tooltip="tooltipLeft" class="btn btn-warning">Left</button>
                    <tooltip placement="left" tooltip="tooltipLeft">{{tooltipText}}</tooltip>
                </span>
                <span style="position: relative; display: inline-block; height: 30px;">
                    <button hover-tooltip="tooltipTop" class="btn btn-danger">Top</button>
                    <tooltip placement="top" tooltip="tooltipTop">{{tooltipText}}</tooltip>
                </span>
                <span style="position: relative; display: inline-block; height: 30px;">
                    <button hover-tooltip="tooltipBottom" class="btn btn-primary">Bottom</button>
                    <tooltip placement="bottom" tooltip="tooltipBottom">{{tooltipText}}</tooltip>
                </span>
            </div>
        </file>
        <file name="script.js">
            function Ctrl($scope)
            {
                $scope.onMouseenter = function () { $scope.tooltipVisible = true;  $scope.tooltipDirty = true; }
                $scope.onMouseleave = function () { $scope.tooltipVisible = false; };
            }
        </file>
    </example>
 **/
directive('hoverTooltip', [function ()
{
    return {
        restrict: 'A',
        link: function (scope, element, attrs)
        {
            var tooltipName = attrs.hoverTooltip;

            element.bind('mouseenter', function ()
            {
                scope.$apply(function ()
                {
                    var tooltip = scope[tooltipName];
                    tooltip.visible = true;
                    tooltip.dirty = true;
                });
            });

            element.bind('mouseleave', function ()
            {
                scope.$apply(function ()
                {
                    var tooltip = scope[tooltipName];
                    tooltip.visible = false;

                });
            });
        }
    }
}]).

/**
 * @ngdoc directive
 * @name tooltip.directive:tooltip
 * @requires tooltip.TooltipPositionService
 * @restrict E
 * @requires components.DirectiveCallback Callback for when elements are rendered.
 * @description
 * A tooltip directive (not complete).
 * 
 * @param {string} placement The placement of the tooltip relative to its parent element.
 * @param {(visible: bool, dirty: bool)} tooltip The tooltip object.
 * @transclude The tooltip text.
 * 
 * @example See {@link tooltip.directive:hoverTooltip hoverTooltip}.
 **/
directive('tooltip', ['DirectiveCallback', 'TooltipPositionService',
function              (DirectiveCallback,   TooltipPositionService)
{
    return {
        restrict: 'E',
        replace: true,
        transclude: true,
        scope:
        {
            placement: '@',
            tooltip: '=',
        },
        link: function (scope, element, attrs)
        {
            if (!scope.tooltip) scope.tooltip = {};
            
            scope.$watch('tooltip.visible', function (value)
            {
                scope.visibility = value ? "visible" : "invisible";
            });

            scope.$watch('tooltip.dirty', function ()
            {
                if (scope.tooltip.dirty)
                {
                    scope.tooltip.dirty = false;
                    DirectiveCallback.PostTransform(function ()
                    {
                        // HACK: Do this two times to make sure that the widths don't change due to positioning.
                        element.css(TooltipPositionService.CalculateTooltipPosition(element, attrs.placement, false));
                        element.css(TooltipPositionService.CalculateTooltipPosition(element, attrs.placement, false));
                    });
                }
            });
        },
        template:
            '<div class="tooltip {{placement}}" ng-class="visibility">' +
                '<div class="tooltip-arrow"></div>' +
                '<div class="tooltip-inner" ng-transclude></div>' +
            '</div>'
    };
}]);