function toggleMealState($button, mealId, url, successMessage, errorMessage) {
    debugger;
    const data = { id: mealId };
    const antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: url,
        method: 'POST',
        data: data,
        headers: {
            "RequestVerificationToken": antiForgeryToken
        },
        success: function () {
            toastr.success(successMessage);
            changeBtnIcon($button, mealId);
        },
        error: function (xhr, status, error) {
            console.error(errorMessage, error);
            toastr.error(errorMessage);
        }
    });
}


function cookMeal($button, mealId) {
    toggleMealState(
        $button,
        mealId,
        '/Meal/Cook',
        "Meal marked as cooked!",
        "Failed to mark meal as cooked."
    );
}


function unCookMeal($button, mealId) {
    toggleMealState(
        $button,
        mealId,
        '/Meal/Uncook',
        "Meal marked as uncooked!",
        "Failed to mark meal as uncooked."
    );
}

function changeBtnIcon($button, mealId) {
    const $icon = $button.find('i'); // Find the icon within the button

    // Check current state and toggle
    if ($button.hasClass('cook-meal-button')) {
        // Update to "Uncook" state
        $button
            .removeClass('cook-meal-button')
            .addClass('uncook-meal-button')
            .attr('title', 'Mark Uncooked') // Update tooltip text
            .tooltip('dispose').tooltip(); // Refresh tooltip

        // Update icon
        $icon.removeClass('fa-calendar-xmark').addClass('fa-calendar-check');

        // Update click event to call unCookMeal
        $button.off('click').on('click', function () {
            unCookMeal($button, mealId);
        });
    } else if ($button.hasClass('uncook-meal-button')) {
        // Update to "Cook" state
        $button
            .removeClass('uncook-meal-button')
            .addClass('cook-meal-button')
            .attr('title', 'Mark as Cooked') // Update tooltip text
            .tooltip('dispose').tooltip(); // Refresh tooltip

        // Update icon
        $icon.removeClass('fa-calendar-check').addClass('fa-calendar-xmark');

        // Update click event to call cookMeal
        $button.off('click').on('click', function () {
            cookMeal($button, mealId);
        });
    }
}
