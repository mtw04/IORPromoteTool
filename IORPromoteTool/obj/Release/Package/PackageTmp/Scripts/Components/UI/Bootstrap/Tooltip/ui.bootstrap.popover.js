/**
 * @name ui.bootstrap.popover
 * @description
 * A directive for bootstrap popover.
 * This is a mix of AngularUI's bootstrap popover directive and my own code so that it does what i want.
 * @link http://angular-ui.github.com
 * @license MIT License, http://www.opensource.org/licenses/MIT
 **/
angular.module('ui.bootstrap.popover', ['ui.bootstrap.tooltip.service', 'components.service.callback']).

/**
 * @ngdoc directive
 * @name tooltip.directive:popover
 * @restrict E
 * @requires tooltip.TooltipPositionService Service that helps calculate where the popover should be located. 
 * @requires components.DirectiveCallback Callback for when elements are rendered.
 * @description
 * A popover directive.
 *
 * @param {string} popoverTitle The popover title.
 * @param {string} titleClass Styling class for the title.
 * @param {string} placement The placement of the popover relative to its parent element.
 * @param {bool} visible If the popover should be visible.
 * @param {bool} dirty A dirty flag which is true when the popover needs to refresh it's sizing and positioning values.
 * @transclude The tooltip text.
 *
 * @example
    <example module="ui.bootstrap.popover">
        <file name="index.html">
            <div ng-controller="Ctrl">
                <span style="position: relative; display: inline-block; height: 30px;">
                    <button ng-click="togglePopover(popoverLeft)" class="btn btn-info">Left</button>
                    <popover popover-title="popoverLeft.title" title-class="popoverLeft.class" placement="left" visible="popoverLeft.visible" dirty="popoverLeft.dirty">Popover text!</tooltip>
                </span>
                 <span style="position: relative; display: inline-block; height: 30px;">
                    <button ng-click="togglePopover(popoverTop)" class="btn btn-info">Top</button>
                    <popover popover-title="popoverTop.title" title-class="popoverTop.class" placement="top" visible="popoverTop.visible" dirty="popoverTop.dirty">Popover text!</tooltip>
                </span>
                 <span style="position: relative; display: inline-block; height: 30px;">
                    <button ng-click="togglePopover(popoverBottom)" class="btn btn-danger">Bottom</button>
                    <popover popover-title="popoverBottom.title" title-class="popoverBottom.class" placement="bottom" visible="popoverBottom.visible" dirty="popoverBottom.dirty">Popover text!</tooltip>
                </span>
                <span style="position: relative; display: inline-block; height: 30px;">
                    <button ng-click="togglePopover(popoverRight)" class="btn btn-danger">Right</button>
                    <popover popover-title="popoverRight.title" title-class="popoverRight.class" placement="right" visible="popoverRight.visible" dirty="popoverRight.dirty">Popover text!</tooltip>
                </span>
            </div>
        </file>
        <file name="script.js">
            function Ctrl($scope)
            {
                $scope.popoverRight = { title: "Error Popover", class: "popover-error", dirty: false, visible: false };
                $scope.popoverLeft = { title: "Popover", class: "", dirty: false, visible: false };
                $scope.popoverTop = { title: "Popover", class: "", dirty: false, visible: false };
                $scope.popoverBottom = { title: "Error Popover", class: "popover-error", dirty: false, visible: false };

                $scope.togglePopover = function (popover)
                {
                    if (popover.visible)
                    {
                        popover.visible = false;
                    }
                    else
                    {
                        popover.visible = true;
                        popover.dirty = true;
                    }
                }
            }
        </file>
    </example>
 **/
directive('popover', ['DirectiveCallback', 'TooltipPositionService',
function              (DirectiveCallback,   TooltipPositionService)
{
    return {
        restrict: 'E',
        replace: true,
        transclude: true,
        scope:
        {
            popoverTitle: '=',
            titleClass: '=',
            placement: '@',
            visible: '=',
            dirty: '='
        },
        link: function (scope, element, attrs)
        {
            scope.visibility = "invisible";

            scope.$watch('visible', function (value)
            {
                scope.visibility = value ? "visible" : "invisible";
            });

            scope.$watch('dirty', function ()
            {
                if (scope.dirty)
                {
                    scope.dirty = false;
                    DirectiveCallback.PostTransform(function ()
                    {
                        // HACK: Do this two times to make sure that the widths don't change due to positioning.
                        element.css(TooltipPositionService.CalculateTooltipPosition(element, attrs.placement, true));
                        element.css(TooltipPositionService.CalculateTooltipPosition(element, attrs.placement, true));
                    });
                }
            });
        },
        template:
            '<div class="popover {{placement}}" ng-class="visibility">' +
                '<div class="arrow"></div>' +
                '<div class="popover-inner">' +
                    '<h3 class="popover-title" ng-show="popoverTitle" ng-class="titleClass">{{popoverTitle}}</h3>' +
                    '<div class="popover-content" ng-transclude></div>' +
                '</div>' +
            '</div>'
    };
}]);