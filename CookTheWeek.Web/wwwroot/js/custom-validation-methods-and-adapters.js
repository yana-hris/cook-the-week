$.validator.addMethod("validateqty", function (value, element, params) {
    debugger;
    var fractionOptions = params.fractionoptions.split(',');
    var qtyDecimal = $(element).closest('.ingredient-row').find('input.qty-decimal').val();
    var qtyWhole = $(element).closest('.ingredient-row').find('input.qty-whole').val();
    var qtyFraction = $(element).closest('.ingredient-row').find('input.qty-fraction').val();

    if ((qtyDecimal && (qtyWhole || qtyFraction)) || (!qtyDecimal && (!qtyWhole && !qtyFraction))) {
        return false;
    }

    if (qtyDecimal && (parseFloat(qtyDecimal) < 0.001 || parseFloat(qtyDecimal) > 9999.99)) {
        return false;
    }

    if (qtyFraction && !fractionOptions.includes(qtyFraction)) {
        return false;
    }

    if (qtyWhole && (parseInt(qtyWhole) < 1 || parseInt(qtyWhole) > 9999)) {
        return false;
    }

    return true;
}, "");

$.validator.unobtrusive.adapters.add("validateqty", ["fractionoptions"], function (options) {
    options.rules["validateqty"] = {
        fractionoptions: options.params.fractionoptions
    };
    options.messages["validateqty"] = options.message;
});
$.validator.addMethod('rangebasedoncollectioncount', function (value, element, params) {
    // Get the collection property name
    var collectionPropertyName = params.collectionpropertyname;

    // Find the collection property in the form
    var collection = $('[name="' + collectionPropertyName + '"]');

    if (!collection.length) {
        return false;
    }

    // Get the collection count
    var collectionCount = collection.length;

    // Validate the value against the collection count
    var intValue = parseInt(value, 10);
    if (isNaN(intValue)) {
        return false;
    }

    return intValue >= 1 && intValue <= collectionCount;
});

$.validator.unobtrusive.adapters.add('rangebasedoncollectioncount', ['collectionpropertyname'], function (options) {
    options.rules['rangebasedoncollectioncount'] = {
        collectionpropertyname: options.params.collectionpropertyname
    };
    options.messages['rangebasedoncollectioncount'] = options.message;
});