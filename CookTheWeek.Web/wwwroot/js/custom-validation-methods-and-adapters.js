$.validator.addMethod("validateqty", function (value, element, params) {
    var $element = $(element);
    var isUsingFractions = $element.closest('.qty-row').find('.show-fraction-checkbox').is(':checked');
    var qtyDecimal = $element.closest('.ingredient-row').find('input.qty-decimal').val();
    var qtyWhole = $element.closest('.ingredient-row').find('input.qty-whole').val();
    var qtyFraction = $element.closest('.ingredient-row').find('input.qty-fraction').val();

    if (isUsingFractions) {
        if ((!qtyWhole && !qtyFraction)) {
            this.settings.messages[element.name].validateqty = errorMessages.MissingFractionalOrWholeInputMessage;
            return false;
        } else if (qtyFraction && !fractionOptions.includes(qtyFraction)) {
            this.settings.messages[element.name].validateqty = errorMessages.InvalidFractionErrorMessage;
            return false;
        }
        if (qtyWhole && (qtyWhole < 1 || qtyWhole > 9999)) {
            this.settings.messages[element.name].validateqty = errorMessages.InvalidWholeQtyErrorMessage;
            return false;
        }
    } else {
        if (!qtyDecimal || qtyDecimal < 0.001 || qtyDecimal > 9999.99) {
            this.settings.messages[element.name].validateqty = errorMessages.InvalidDecimalRangeErrorMessage;
            return false;
        }
    }

    if (!qtyDecimal && !qtyWhole && !qtyFraction) {
        this.settings.messages[element.name].validateqty = errorMessages.MissingFormInputErrorMessage;
        return false;
    }

    return true;
}, "");

$.validator.unobtrusive.adapters.add("validateqty", [], function (options) {    
        options.rules["validateqty"] = {};
        options.messages["validateqty"] = options.message;
 });


