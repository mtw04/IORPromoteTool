/**
 * @name ui.bootstrap.tooltip.service
 * @description
 * This is a mix of AngularUI's bootstrap tooltip service and my own code so that it does what i want.
 **/
angular.module('ui.bootstrap.tooltip.service', []).

/**
 * @ngdoc service
 * @name tooltip.TooltipPositionService
 * @description 
 * Calculates the position of where tooltips/popovers should be. 
 **/
factory('TooltipPositionService', function ()
{
    /**
     * @name getPosition
     * @description 
     * Get the position properties of the DOM element. 
     *
     * @param {Element} element The element we want to get the position properties of.
     * @returns {(top: float, left: float, width: float, height: float)} The position properties of the element. 
     **/
    var getPosition = function (element)
    {
        return {
            width: element.prop('offsetWidth'),
            height: element.prop('offsetHeight')
        };
    },

    /** 
     * @ngdoc method
     * @methodOf tooltip.TooltipPositionService
     * @name tooltip.TooltipPositionService#CalculateTooltipPosition
     * @description 
     * Calculates where the tooltip/popover should be positioned. 
     * 
     * @param {Element} element The tooltip or popover element. The element should have a relative parent container. 
     * @param {string} placement How the tooltip should be positioned relative to its parent. Can be top, bottom, left, or right.
     * @param {bool} isPopover If the element is a popover. This changes the positioning by a few pixels for some of the positions.
     * @returns {css style} A css style that can be applied to the tooltip element (contains top left right and bottom properties).
     **/
    CalculateTooltipPosition = function (element, placement, isPopover)
    {
        var tooltipPosition = getPosition(element),
            parentElement = angular.element(element.parent()[0]),
            parentPosition = getPosition(parentElement),
            position;

        switch (placement)
        {
            case 'right':
                position =
                {
                    top: ((parentPosition.height - tooltipPosition.height) / 2) + 'px',
                    right: -(tooltipPosition.width + (isPopover ? 12 : 0)) + 'px', // relative to the parent's right so its just negative tooltip width.
                    left: 'auto'
                };
                break;
            case 'bottom':
                position =
                {
                    top: parentPosition.height + 'px', // relative to parent's top so it's just parent height.
                    left: ((parentPosition.width - tooltipPosition.width) / 2) + 'px', // centered, so half the difference of the parent and tooltip.
                    right: 'auto'
                };
                break;
            case 'left':
                position =
                {
                    top: ((parentPosition.height - tooltipPosition.height) / 2) + 'px',
                    left: -(tooltipPosition.width) + 'px', // relative to the parent's left so its just negative tooltip width.
                    right: 'auto'
                };
                break;
            default:
                position =
                {
                    bottom: ((isPopover ? 12 : 0) + parentPosition.height) + 'px', // relative to parent's bottom so it's just parent height.
                    left: ((parentPosition.width - tooltipPosition.width) / 2) + 'px', // centered, so half the difference of the parent and tooltip.
                    right: 'auto',
                    top: 'auto'
                };
                break;
        }

        return position;
    };

    return {
        CalculateTooltipPosition: CalculateTooltipPosition
    }
});