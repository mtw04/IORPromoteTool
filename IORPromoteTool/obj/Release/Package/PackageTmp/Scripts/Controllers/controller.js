// Laserfiche  - Copyright(c) 1993-2013 Compulink Management Center, Inc.

/**
 * @name controller
 * @requires 
 *           controller.mainctrl - the main controller for the application.
 * @description
 * Application controllers.
 **/
angular.module('controller', ['controller.main',
                              'controller.card',
                              'controller.card.promote',
                              'controller.card.result',
                              'controller.record',
                              'controller.record.details',
                              'controller.user',
                              'controller.user.create',
                              'controller.user.edit',
                              'controller.user.delete'
]);
