function toggleMealState($button, mealId, url, successMessage, errorMessage) {
    return new Promise((resolve, reject) => {
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
                resolve(true); // Resolve on success
            },
            error: function (xhr, status, error) {
                console.error(errorMessage, error);
                toastr.error(errorMessage);
                resolve(false); // Resolve as false on error
            }
        });
    });
}

function cookMeal($button, mealId) {
    
    toggleMealState(
        $button,
        mealId,
        '/Meal/Cook',
        "Meal marked as cooked!",
        "Failed to mark meal as cooked."
    ).then(success => {
        if (success && $button.hasClass('cook-meal-button')) {
            $button.tooltip('dispose');

            $button.removeClass('cook-meal-button')
                .addClass('uncook-meal-button')
                .attr('data-bs-original-title', 'Not Cooked?');

            $button.tooltip();

            
            // Update click event
            $button.off('click').on('click', function () {
                unCookMeal($button, mealId);
            });

            adjustDeleteMealBtn(mealId, true);

            const $mealRow = $button.closest('.meal-row');
            if ($mealRow.length > 0) {
                adjustSelectMenus($mealRow, true);
                adjustInfoBtn($mealRow, true);
            }
        }
    })
}

function unCookMeal($button, mealId) {
    toggleMealState(
        $button,
        mealId,
        '/Meal/Uncook',
        "Meal marked as uncooked!",
        "Failed to mark meal as uncooked."
    ).then(success => {
        if (success && $button.hasClass('uncook-meal-button')) {
            $button.tooltip('dispose');

            $button.removeClass('uncook-meal-button')
                .addClass('cook-meal-button')
                .attr('data-bs-original-title', 'Mark Cooked');

            $button.tooltip();

            $button.off('click').on('click', function () {
                cookMeal($button, mealId);
            });

            adjustDeleteMealBtn(mealId, false);

            const $mealRow = $button.closest('.meal-row');
            if ($mealRow.length > 0) {
                adjustSelectMenus($mealRow, false);
                adjustInfoBtn($mealRow, false);
            }
        }
    });
}


function adjustDeleteMealBtn(mealId, disable) {
    const $deleteButton = $(`.delete-meal-button[data-meal-id="${mealId}"]`);

    debugger;
    if (!$deleteButton.length === 0) {
        console.warn(`Delete button for meal ID ${mealId} not found.`);
        return;
    }

    if (disable) {
        $deleteButton.addClass('disabled').attr('disabled', true); // Add class and disable the button
    } else {
        $deleteButton.removeClass('disabled').removeAttr('disabled'); // Remove class and enable the button
    }
}

function adjustSelectMenus($mealRow, disable) {
    const $servingsSelect = $mealRow.find('select[name$=".Servings"]');
    const $dateSelect = $mealRow.find('select[name$=".Date"]');
   
    if ($servingsSelect.length === 0 || $dateSelect.length === 0) {
        console.log('Select menus are not found.');
        return;
    }

    if (disable) {
        $servingsSelect.addClass('disabled');
        $dateSelect.addClass('disabled');
        
    } else {
        $servingsSelect.removeClass('disabled');
        $dateSelect.removeClass('disabled');
    }
    
}

function adjustInfoBtn($mealRow, show) {
    const $infoBtn = $mealRow.find('.info-btn');
    if ($infoBtn.length > 0) {
        $infoBtn.css('display', show ? 'block' : 'none');
    }
}
