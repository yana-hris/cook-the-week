function cookMeal(event, mealId) {
    event.preventDefault();
    event.stopPropagation();
    
    var data = {
        id: mealId
    }

    var antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();
    var containerDiv = $(event.currentTarget).closest('.cook-meal-container');

    let url = `/Meal/Cook`;

    $.ajax({
        url: url,
        method: 'POST',
        data: data,
        headers: {
            "RequestVerificationToken": antiForgeryToken // Include the token in the request headers
        },
        success: function (response) {
            toastr.success("Meal marked as cooked!");
            hideCookBtn(containerDiv);
        },
        error: function (xhr, status, error) {
            // Handle error response, e.g., display an error message
            console.error('Failed to mark meal as cooked:', error);
            toastr.error("Failed to mark meal as cooked.");
        }
    });
}

function hideCookBtn(button) {
    debugger;
    if (!button) {
        console.error("Event or currentTarget is null or undefined");
        return;
    }
    const $button = $(button);
    $button.hide();
}