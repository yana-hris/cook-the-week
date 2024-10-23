function cookMeal(event, mealId) {
    event.preventDefault();
    event.stopPropagation();

    debugger;
    var data = {
        mealId: mealId
    }

    var antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();

    let url = `/Meal/Cook/${mealId}`;

    $.ajax({
        url: url,
        method: 'GET',
        contentType: 'application/json',
        //data: JSON.stringify(data),
        headers: {
            "RequestVerificationToken": antiForgeryToken // Include the token in the request headers
        },
        success: function (response) {
            toastr.success("Meal marked as cooked!");
            hideCookBtn(event);
        },
        error: function (xhr, status, error) {
            // Handle error response, e.g., display an error message
            console.error('Failed to mark meal as cooked:', error);
            toastr.error("Failed to mark meal as cooked.");
        }
    });
}

function hideCookBtn(e) {
    const $button = $(e.currentTarget);
    $button.hide();
}