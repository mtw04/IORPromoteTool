// Laserfiche  - Copyright(c) 1993-2013 Compulink Management Center, Inc.

/**
 * @name ui
 * @requires ui.canvas - directives for rendering workflow canvas components,
 *           ui.bootstap - directives that use the bootstrap CSS,
 *           ui.directives - AngularUI directives (http://angular-ui.github.com/),
 *           ui.error - a directive for displaying errors.
 * @description
 * Contains UI directives.
 **/
angular.module("ui", ["ui.bootstrap",
                      "ui.grid",
                      "ui.datepickerCustom"
]);

/**
 * @name ui.bootstrap
 * @requires ui.bootstrap.tabs - directive for rendering bootstrap tabs.
 * @description
 * Our Bootstrap directives.
 **/
angular.module("ui.bootstrap", ["ui.bootstrap.tabs", "ui.bootstrap.popover", "ui.bootstrap.tooltip", "ui.bootstrap.progress", "ui.bootstrap.datepicker"]);
