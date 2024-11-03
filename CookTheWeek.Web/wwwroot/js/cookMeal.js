function cookMeal(event, mealId) {
    event.preventDefault();
    event.stopPropagation();
    
    var data = {
        id: mealId
    }

    var antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();
    var containerDiv = $(event.currentTarget).closest('.button-container');

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
            changeBtnIcon(containerDiv);
        },
        error: function (xhr, status, error) {
            // Handle error response, e.g., display an error message
            console.error('Failed to mark meal as cooked:', error);
            toastr.error("Failed to mark meal as cooked.");
        }
    });
}

function changeBtnIcon(button) {
    
    if (!button) {
        console.error("Event or currentTarget is null or undefined");
        return;
    }
    const $button = $(button);
    $button.html(` <span class="cooked-icon" data-bs-toggle="tooltip" data-bs-placement="top" title="Cooked">
                       <i class="fa-solid fa-calendar-check"></i>
                   </span>`);
}