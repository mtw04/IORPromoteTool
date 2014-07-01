// Laserfiche  - Copyright(c) 1993-2013 Compulink Management Center, Inc.

/**
 * @name service.components.callback
 * @description
 * Contains events for directives.
 **/
angular.module('components.service.callback', []).

/**
 * @ngdoc service
 * @name components.DirectiveCallback
 * @requires $timeout Angulars timeout service.
 * @description
 * [Source: Blog article.](http://lorenzmerdian.blogspot.com/2013/03/how-to-handle-dom-updates-in-angularjs.html)
 *
 * This is pretty much a HACK that gives a callback for directives that is called after the DOM elements are manipulated or when they are finished rendering.
 * It takes advantage of javascript's single threaded nature and adds the callback functions to the end of the queue. 
 **/
provider('DirectiveCallback', function ()
{
    this.$get = ['$timeout', function ($timeout)
    {
        return {

            /**
             * @ngdoc function
             * @methodOf components.DirectiveCallback
             * @name components.DirectiveCallback#PostTransform
             * @description PostTransform occurs after the DOM has been manipulated by directives.
             **/
            PostTransform: function (fn)
            {
                return $timeout(fn, 0);
            },

            /**
             * @ngdoc function
             * @methodOf components.DirectiveCallback
             * @name components.DirectiveCallback#PostRender
             * @description PostRender will occur after the browser has finished the render and layout of the elements. 
             **/
            PostRender: function (fn)
            {
                return $timeout(function () { $timeout(fn, 0); });
            }
        };
    }];
});
    