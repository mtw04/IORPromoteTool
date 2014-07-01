/**
 * @name ui.bootstrap.tabs
 * @description
 * A directive for bootstrap tabs.
 **/
angular.module('ui.bootstrap.tabs', []).

/**
 * @ngdoc directive
 * @name components.directive:tabs
 * @restrict E
 * @description
 * The main directive for tabs.
 **/
directive('tabs', function ()
{
    return {
        restrict: 'E',
        transclude: true,
        scope:
        {
            direction: "@"
        },
        controller: ['$scope', '$element', function ($scope, $element)
        {
            var panes = $scope.panes = [];

            /* Pane select handler. */
            $scope.select = function (pane)
            {
                angular.forEach(panes, function (pane)
                {
                    pane.selected = false;
                });

                pane.selected = true;
            };

            /* Add a pane to the tab control. This should be called for every added pane. */
            this.addPane = function (pane)
            {
                if (panes.length == 0) $scope.select(pane);
                panes.push(pane);
            };
        }],
        template:
            '<div class="tabbable tabs-{{direction}}">' +
                '<ul class="nav nav-tabs">' +
                    '<li ng-repeat="pane in panes" ng-class="{active:pane.selected}">' +
                        '<a href="" ng-click="select(pane)">{{pane.tabTitle}}</a>' +
                    '</li>' +
                '</ul>' +
                '<div class="tab-content" ng-transclude></div>' +
            '</div>',
        replace: true
    };
}).

/**
 * @ngdoc directive
 * @name components.directive:pane
 * @restrict E
 * @description
 * A directive for a pane in a tabs control.
 **/
directive('pane', function ()
{
    return {
        require: '^tabs',
        restrict: 'E',
        transclude: true,
        scope: { tabTitle: '@' },
        link: function (scope, element, attrs, tabsCtrl)
        {
            tabsCtrl.addPane(scope);
        },
        template:
            '<div class="tab-pane" ng-class="{active: selected}" ng-transclude>' +
            '</div>',
        replace: true
    };
});