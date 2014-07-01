// Laserfiche  - Copyright(c) 1993-2013 Compulink Management Center, Inc.

/**
 * @name ui.components.grid
 * @requires service.components.grid - the grid service which takes care of stuff like pagination.
 * @description
 * Grid directives.
 **/
angular.module('ui.grid', ['ui.grid.service', "ui.bindswitch"]).

/**
 * @ngdoc directive
 * @name grid.directive:gridContainer
 * @restrict E
 * @requires $filter - angular's filter service
 * @requires grid.Pagination - provides a Paginator class for paging variables and functions.
 * @description
 * A container for the grid. Contains the grid menu bar and the grid itself.
 *
 * @param {[dict[columnName] = value]} data The grid data.
 * @param {int} numItems Number of items to put in the grid.
 * @param {[Column]} columns The grid columns. (Column => filter(string): a filter to apply to the column values, name(string): the key of the column item, displayName(string): the title of the column)
 * @param {function} onSelectItem Callback for when item is selected.
 * @param {function} onColumnChange Callback for when column settings are changed.
 * @param {attr} hasColumnSelector If the grid has this attribute then the column selector is enable, if not then it is disabled.
 **/
directive('gridContainer', ['$filter', '$injector', 'Pagination', function ($filter, $injector, Pagination) {
    return {
        restrict: 'E',
        scope:
        {
            data: '=',
            numItems: '=',
            columns: '=',
            onSelectItem: '=',
            initialSortBy: '=',
            preSort: '='
        },
        controller: ['$scope', '$element', '$attrs', '$filter', function ($scope, $element, $attrs, $filter) {
            var self = this;

            updateData = function (data) {
                self.pageController.SetNumItems(data.length);
                self.pageController.UpdatePage(true);
                $scope.pageData = data.slice(self.pageController.startIndex, self.pageController.endIndex);
            };

            // Paginator. 
            self.pageController = new Pagination.Paginator();

            // Grid sort function. Hopefully we can optimize this with a partial sort or something if we do need to search on ALOT of data, but for now, we'll use angular's orderBy filter.
            self.Sort = function (predicate, reverse) {
                $scope.filteredData = $filter('orderBy')($scope.filteredData, predicate, reverse);
            };

            // When the search query change, update the grid with the data.
            $scope.$watch('query', function (query) {
                $scope.filteredData = $filter('filter')($scope.data, query);

                if (query == "") {
                    initialLoad = false; // Run initial load again when filter is empty
                }
            });

            // Initial sorting of grid
            var initialLoad = false;

            // Watch the filtered data. If it changes, then update the grid (always goes to page 1).
            $scope.$watch('filteredData', function (data) {
                
                if (data && data.length > 0) {
                    updateData(data);

                    // Only do default sort when query is empty and viewing the Users Grid
                    if (!initialLoad && $scope.preSort) {
                        self.Sort($scope.initialSortBy + '.value', false)
                        initialLoad = true;
                    }
                }
            });
            
            // Watch the data. If it changes, then update filtered data. This is to initialize filtered data.
            $scope.$watch('data', function (data) {
                if (data && data.length > 0) {
                    $scope.filteredData = data;
                }
            });

            // If the page changes, update the data displayed on the current page.
            $scope.$watch(function () { return self.pageController.currentIndex; }, function (index) {
                if ($scope.filteredData) {
                    $scope.pageData = $scope.filteredData.slice(self.pageController.startIndex, self.pageController.endIndex);
                }
            });

        }],
        template:
            '<div>' +
                '<grid-menu-bar></grid-menu-bar>' +
                '<grid></grid>' +
           '</div>',
        replace: true
    };
}]).

/**
 * @ngdoc directive
 * @name grid.directive:grid
 * @restrict E
 * @requires grid.directive:gridContainer
 * @description
 * A directive for a sortable grid.
 **/
directive('grid', ['$q', function ($q) {
    return {
        restrict: 'E',
        require: '^gridContainer',
        scope: true,
        link: function (scope, element, attrs, gridCtrl) {
            var selectedRow = null,
                predicate = '',
                reverse = '',
                current = null;

            /* Handle column click. Sort the column accordingly. */
            scope.onColumnClick = function (name, column) {
                if (predicate == name + '.value') {
                    reverse = !reverse;
                    gridCtrl.Sort(predicate, reverse)
                    column.arrowClass = reverse ? "col-descending" : 'col-ascending';
                }
                else {
                    if (current) current.arrowClass = '';
                    reverse = false;
                    current = column;
                    predicate = name + '.value';
                    gridCtrl.Sort(predicate, reverse);
                    column.arrowClass = 'col-ascending';
                }
            };

            /* Handle selecting a row. Highlight that row and un-highlight the previously selected row. */
            scope.onSelectRow = function (item) {
                if (selectedRow) selectedRow.selected = false;
                item.selected = true;
                selectedRow = item;

                scope.onSelectItem(item); // single click for select
            };

            /* Handle double clicking a row. */
            scope.onDblClickRow = function (item) {
                if (selectedRow) {
                    selectedRow.selected = false;
                    selectedRow = null;
                }
            };
        },
        templateUrl: 'PartialViews/Grid/Grid.html',
        replace: true
    };
}]).

/**
 * @ngdoc directive
 * @name grid.directive:gridMenuBar
 * @restrict E
 * @requires grid.directive:gridContainer
 * @description
 * The menu bar for the grid directive.
 **/
directive('gridMenuBar', function () {
    return {
        restrict: 'E',
        require: '^gridContainer',
        scope: false,
        link: function (scope, element, attrs, gridCtrl) {
            var pageController = scope.pageController = gridCtrl.pageController,
                setSearchIndicies = function () {
                    scope.searchIndicies = "(" + (pageController.startIndex + 1) + ' - ' + pageController.endIndex + ' of ' + pageController.numItems + " results)";
                };

            scope.$watch('pageController.currentIndex', function (index) {
                if (index == 0 && scope.prevTooltip) scope.prevTooltip.visible = false;
                if (index == scope.pageController.numPages - 1 && scope.nextTooltip) scope.nextTooltip.visible = false;
            });

            scope.$watch('pageController.endIndex', setSearchIndicies);
            scope.$watch('pageController.startIndex', setSearchIndicies);
            scope.$watch('pageController.numItems', setSearchIndicies);
        },
        templateUrl: 'PartialViews/Grid/GridMenu.html',
        replace: true
    };
});
