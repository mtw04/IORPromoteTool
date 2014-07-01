// Laserfiche  - Copyright(c) 1993-2013 Compulink Management Center, Inc.

/**
 * @name service.components.grid.paginator
 * @description service that provides pagination functions.
 **/
angular.module('ui.grid.service.paginator', [])

/**
 * @ngdoc service
 * @name grid.Pagination
 * @description
 * Contains a Paginator class that stores page variables and provides paging functions.
 *
 * <pre> var paginator = new Pagination.Paginator(); </pre>
 **/
.provider('Pagination', function ()
{
    var self = this,
        dependencies = {},

    /**
     * @name Paginator
     * @description
     * A paginator class that contains pagination variables and functions.
     **/
    Paginator = function ()
    {
        var gridItemsPerPage = 20; // set items per page

        //try
        //{
        //    var AccountApiService = dependencies.$injector.get("AccountApiService");
        //    gridItemsPerPage = AccountApiService.GetUserSetting('GridItemsPerPage');
        //} catch (ex) { }
       
        /**
         * @ngdoc property
         * @propertyOf grid.Pagination
         * @name grid.Pagination#currentIndex
         * @description The current page number. (programmer style).
         **/
        this.currentIndex = 0;

        /**
         * @ngdoc property
         * @propertyOf grid.Pagination
         * @name grid.Pagination#displayIndex
         * @description The display page number. (user style which is programmer style + 1).
         **/
        this.displayIndex = 1;

        /**
         * @ngdoc property
         * @propertyOf grid.Pagination
         * @name grid.Pagination#pageSize
         * @description The number of items displayed on one page.
         **/
        this.pageSize = gridItemsPerPage || 20;

        /**
         * @ngdoc property
         * @propertyOf grid.Pagination
         * @name grid.Pagination#startIndex
         * @description The item start index for the current page.
         **/
        this.startIndex;

        /**
         * @ngdoc property
         * @propertyOf grid.Pagination
         * @name grid.Pagination#endIndex
         * @description The item end index for the current page.
         **/
        this.endIndex;

        /**
         * @ngdoc property
         * @propertyOf grid.Pagination
         * @name grid.Pagination#numItems
         * @description The number of items to display in the grid.
         **/
        this.numItems;

        /**
         * @ngdoc property
         * @propertyOf grid.Pagination
         * @name grid.Pagination#numPages
         * @description The number of pages needed for the items in the grid.
         **/
        this.numPages;
    };

    Paginator.prototype = function ()
    {
        /**
         * @ngdoc function
         * @methodOf grid.Pagination
         * @name grid.Pagination#GoNextPage
         * @description
         * Goes to the next page. If there is no next page, it doesn't do anything.
         **/
        var GoNextPage = function ()
        {
            if (this.currentIndex < this.numPages - 1)
            {
                this.currentIndex += 1;
                updatePage.call(this);
            }
        },

        /**
         * @ngdoc function
         * @methodOf grid.Pagination
         * @name grid.Pagination#GoPrevPage
         * @description
         * Goes to the previous page. If there is no previous page, it doesn't do anything.
         **/
        GoPrevPage = function ()
        {
            if (this.currentIndex > 0)
            {
                this.currentIndex -= 1;
                updatePage.call(this);
            }
        },

        /**
         * @ngdoc function
         * @methodOf grid.Pagination
         * @name grid.Pagination#GoToPage
         * @description
         * Goes to the specified page.
         *
         * @param {int} index The index of the page to go to. If the page doesn't exist, it doesn't do anything.
         **/
        GoToPage = function (index)
        {
            if (index >= 0 && index < this.numPages)
            {
                this.currentIndex = index;
                updatePage.call(this);
            }
        },

        /**
         * @ngdoc function
         * @methodOf grid.Pagination
         * @name grid.Pagination#UpdatePage
         * @description
         * Updates the current index and/or display index. If the display index is set, this function should be called.
         * If the display index is within the paginiation range, then it updates the current index and updates the page.
         * If not, it sets the display index back to a valid value ( the previous value ).
         *
         * @param {bool} reset If true, reset the page number to 0. Otherwise, it syncs displayIndex and currentIndex.
         **/
        UpdatePage = function (reset)
        {
            // If reset, then set the page number back to 0.
            if (reset)
            {
                this.currentIndex = 0;
                updatePage.call(this);
            }

            // If the display index is in the pagination range, then change the currentIndex accordingly.
            else if (this.displayIndex > 0 && this.displayIndex <= this.numPages)
            {
                this.currentIndex = this.displayIndex - 1;
                updatePage.call(this);
            }

            // If the display index isn't in the pagination range, then set the display index back to the previous currentIndex.
            else
            {
                this.displayIndex = this.numItems == 0 ? this.currentIndex : this.currentIndex + 1;
            }
        },

        /**
         * @ngdoc function
         * @methodOf grid.Pagination
         * @name grid.Pagination#SetNumItems
         * @description
         * Sets the total number of items that are displayed in the grid.
         *
         * @param {int} numItems The total number of items that are displayed in the grid.
         **/
        SetNumItems = function (numItems)
        {
            this.numItems = numItems;
            this.numPages = calculateNumPages.call(this);
        },

        /**
         * @ngdoc function
         * @methodOf grid.Pagination
         * @name grid.Pagination#SetPageSize
         * @description
         * Sets the page size
         *
         * @param {int} pageSize The number of items displayed on one page.
         **/
        SetPageSize = function (pageSize)
        {
            this.pageSize = pageSize || 20;
            this.numPages = calculateNumPages.call(this);
            this.UpdatePage();
        },

        //#region Helper methods (private)

        /**
         * @name setStartIndex
         * @description
         * Calculates what the current start index of the page should be.
         **/
        setStartIndex = function ()
        {
            this.startIndex = this.pageSize * this.currentIndex;
        },

        /**
         * @name setEndIndex
         * @description
         * Calculates what the current end index of the page should be.
         **/
        setEndIndex = function ()
        {
            var end = this.startIndex + this.pageSize;
            this.endIndex = end > this.numItems - 1 ? this.numItems : end;
        },

        /**
         * @name calculateNumPages
         * @description
         * Calculates the total number of pages needed for the items.
         *
         * @returns (int) the total number of pages.
         **/
        calculateNumPages = function ()
        {
            return Math.floor((this.numItems - 1) / this.pageSize) + 1;
        },

        /**
         * @name updatePage
         * @description
         * Updates the properties for the page. (Sets the start and end index for the current page number, and also sets the display index).
         **/
        updatePage = function ()
        {
            setStartIndex.call(this);
            setEndIndex.call(this);
            this.displayIndex = this.numItems == 0 ? this.currentIndex : this.currentIndex + 1;
        };

        //#endregion

        return {
            GoNextPage: GoNextPage,
            GoPrevPage: GoPrevPage,
            GoToPage: GoToPage,
            UpdatePage: UpdatePage,
            SetNumItems: SetNumItems,
            SetPageSize: SetPageSize
        };
    }();

    this.$get = ['$injector', function ($injector)
    {
        dependencies.$injector = $injector;
        return {
            Paginator: Paginator
        }
    }];
});