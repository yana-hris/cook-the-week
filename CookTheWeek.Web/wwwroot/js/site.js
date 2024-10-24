﻿(function () {
    // JavaScript snippet handling Dark/Light mode switching
    const getStoredTheme = () => localStorage.getItem('theme');
    const setStoredTheme = theme => localStorage.setItem('theme', theme);
    const forcedTheme = document.documentElement.getAttribute('data-bss-forced-theme');

    const getPreferredTheme = () => {

        if (forcedTheme) return forcedTheme;

        const storedTheme = getStoredTheme();
        if (storedTheme) {
            return storedTheme;
        }

        const pageTheme = document.documentElement.getAttribute('data-bs-theme');

        if (pageTheme) {
            return pageTheme;
        }

        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    }

    const setTheme = theme => {
        if (theme === 'auto' && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            document.documentElement.setAttribute('data-bs-theme', 'dark');
        } else {
            document.documentElement.setAttribute('data-bs-theme', theme);
        }
    }

    setTheme(getPreferredTheme());

    const showActiveTheme = (theme, focus = false) => {
        const themeSwitchers = [].slice.call(document.querySelectorAll('.theme-switcher'));

        if (!themeSwitchers.length) return;

        document.querySelectorAll('[data-bs-theme-value]').forEach(element => {
            element.classList.remove('active');
            element.setAttribute('aria-pressed', 'false');
        });

        for (const themeSwitcher of themeSwitchers) {

            const btnToActivate = themeSwitcher.querySelector('[data-bs-theme-value="' + theme + '"]');

            if (btnToActivate) {
                btnToActivate.classList.add('active');
                btnToActivate.setAttribute('aria-pressed', 'true');
            }
        }
    }

    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
        const storedTheme = getStoredTheme();
        if (storedTheme !== 'light' && storedTheme !== 'dark') {
            setTheme(getPreferredTheme());
        }
    });

    window.addEventListener('DOMContentLoaded', () => {
        showActiveTheme(getPreferredTheme());

        document.querySelectorAll('[data-bs-theme-value]')
            .forEach(toggle => {
                toggle.addEventListener('click', (e) => {
                    e.preventDefault();
                    const theme = toggle.getAttribute('data-bs-theme-value');
                    setStoredTheme(theme);
                    setTheme(theme);
                    showActiveTheme(theme);
                })
            })
    });
})();


window.onload = function () {    
    var buildBtnShouldBeRendered = isSpecificView();    
    if (buildBtnShouldBeRendered) {
        if (currentUserId) {
            showOrHideBuildMealPlanBtn(); 
        }
        else {
            showOrHideBuildMealPlanBtn(null);
        }
    updateRecipeBtns(); 
    }
    return;
};

function updateRecipeBtns() {
    let userMealPlans = getUserLocalStorage() || [];

    const recipeButtons = document.querySelectorAll('.add-to-mealplan-container .btn');

    if (userHasMealPlans()) {
        userMealPlans = JSON.parse(userMealPlans);
    }

    if (userMealPlans.length > 0) {
        recipeButtons.forEach(btn => {
            const recipeId = btn.getAttribute('data-recipeId');
            const icon = btn.querySelector('i');

            if (userMealPlans.includes(recipeId)) {
                btn.classList.remove("plus");
                btn.classList.add("minus");

                btn.removeEventListener('click', addRecipeToMealPlan, true);
                btn.addEventListener('click', removeRecipeFromMealPlan, true);

                icon.classList.remove("fa-plus");
                icon.classList.add("fa-minus");
            } else {
                btn.classList.remove("minus");
                btn.classList.add("plus");

                btn.removeEventListener('click', removeRecipeFromMealPlan, true);
                btn.addEventListener('click', addRecipeToMealPlan, true);

                icon.classList.remove("fa-minus");
                icon.classList.add("fa-plus");
            }
        })
    } else {
        recipeButtons.forEach(btn => {
            if (btn.classList.contains('minus')) {
                const icon = btn.querySelector('i');

                btn.classList.remove("minus");
                btn.classList.add("plus");

                icon.classList.remove("fa-minus");
                icon.classList.add("fa-plus");

                btn.removeEventListener('click', removeRecipeFromMealPlan, true);
                btn.addEventListener('click', addRecipeToMealPlan, true);
            } else {
                btn.addEventListener('click', addRecipeToMealPlan, true);
            }
        })
    }
}
function isSpecificView() {
    // Check if the current View is any of these   
    const specificViews = ["All Recipes", "Recipe Details", "My Recipes"];
    return specificViews.includes(currentView);
};

function userHasMealPlans() {
    
    const userMealPlans = JSON.parse(getUserLocalStorage());
    
    if (userMealPlans && userMealPlans.length > 0) {
        return true;
    } else if (userMealPlans && userMealPlans.length === 0) {
        eraseUserLocalStorage();
    }

    return false;
    
}

function showOrHideBuildMealPlanBtn() {
    var buildBtnContainer = document.getElementById('build-btn-container');    
    
    if (currentUserId !== null && userHasMealPlans()) {
        // Show the btn and attach event-listener for creating mealplan
        buildBtnContainer.style.display = "block";
        buildBtnContainer.addEventListener('click', (event) => buildMealPlan(event));
    } else {
        // Remove event listener and toggle visibility
        buildBtnContainer.removeEventListener('click', (event) => buildMealPlan(event))
        buildBtnContainer.style.display = "none";
    }
}

function getUserLocalStorage() {
    return localStorage.getItem(currentUserId);
}

const addRecipeToMealPlan = function(event) {
    // Check if local storage has meal plans for this user
    event.preventDefault();
    event.stopPropagation();
    var recipeId = event.currentTarget.dataset.recipeid;
    let userMealPlans = getUserLocalStorage();

    // If the user doesn't have any meal plans yet, create an empty array
    if (!userMealPlans) {
        userMealPlans = [];
    } else {
        // Parse the existing meal plans
        userMealPlans = JSON.parse(userMealPlans);
    }

    if (userMealPlans.indexOf(recipeId) === -1) {
        // If the recipe doesn't exist, add it to the meal plan
        userMealPlans.push(recipeId);        
        toastr.success(`Recipe successfully added to you meal plan`);
        
    } else {
        toastr.error(`Recipe is already added to your meal plan`);
        return;
    }
    // Save the updated meal plans back to local storage
    localStorage.setItem(currentUserId, JSON.stringify(userMealPlans));
    toggleAddRemoveBtn(event.currentTarget);
    showOrHideBuildMealPlanBtn(currentUserId);
}

// Remove recipe from saved for meal plan (all, mine, details views) or from meal plan (view)


const removeRecipeFromMealPlan = function (event) {    
    event.preventDefault();
    event.stopPropagation();
    
    const recipeId = event.currentTarget.dataset.recipeid;

    removeRecipeFromLocalStorage(recipeId); 
    toggleAddRemoveBtn(event.currentTarget);
    showOrHideBuildMealPlanBtn();
}

function toggleAddRemoveBtn(btn) {
    const icon = btn.querySelector('i');    

    if (btn.classList.contains("plus")) {
        btn.classList.remove("plus");
        btn.classList.add("minus");

        btn.removeEventListener('click', addRecipeToMealPlan, true);
        btn.addEventListener('click', removeRecipeFromMealPlan, true);

        icon.classList.remove("fa-plus");
        icon.classList.add("fa-minus");

    } else {
        btn.classList.remove("minus");
        btn.classList.add("plus");

        btn.removeEventListener('click', removeRecipeFromMealPlan, true);
        btn.addEventListener('click', addRecipeToMealPlan, true);

        icon.classList.remove("fa-minus");
        icon.classList.add("fa-plus");  
    }          
}

// Gets the antiforgery token
function getAntiForgeryToken() {
    return $('#antiForgeryForm input[name="__RequestVerificationToken"]').val();
}

function buildMealPlan(event) {
    event.preventDefault();
    
    const userMealPlans = JSON.parse(getUserLocalStorage()) || [];

    if (userMealPlans.length === 0) {
        toastr.error("Error Building Meal Plan");
        return;
    }

    var model = {
        UserId: currentUserId,
        Meals: userMealPlans.map(recipeId => ({ RecipeId: recipeId }))
    };
    
    
    $.ajax({
        url: '/MealPlan/CreateMealPlanModel',
        type: 'POST',
        contentType: 'application/json',
        headers: {
            'RequestVerificationToken': getAntiForgeryToken()
        },
        data: JSON.stringify(model),
        success: function (response) {
            // Redirect to the Add view and use TempData to store the model
            window.location.href = '/MealPlan/Add';
        },
        error: function (xhr, status, error) {
            console.error('Error building the meal plan:', error);
            toastr.error("Meal plan creation failed! Try again later.");
            window.location.href = '/Recipe/All';
        }
    });
}

// Make Active Tab the tab with the first Error in Recipe Views
export function activateTabWithError(context) {
    // Find the first visible error element
    var firstErrorElement = $(context).find("span.text-danger").filter(function () {
        return $(this).is(":not(:empty)");
    }).first();

    if (firstErrorElement.length > 0) {
        // Get the tab pane that contains the first error
        var tabPane = firstErrorElement.closest(".tab-pane");

        if (tabPane.length > 0) {
            // Get the ID of the tab pane
            var tabPaneId = '#' + tabPane.attr("id");
            var buttonElement = `#recipe button[data-bs-target="${tabPaneId}"]`;

            const triggerEl = document.querySelector(`${buttonElement}`);
            var $buttonElement = $(triggerEl);
            $buttonElement.tab('show'); // Activate the tab                    
        }
    } 
}

export function removeRecipeFromLocalStorage(recipeId) {
    
    if (!userHasMealPlans()) {
        console.log('Error: User has no meal plans to remove recipes from');
        return;
    }

    let userMealPlans = JSON.parse(getUserLocalStorage());
    const recipeIndex = userMealPlans.indexOf(recipeId);

    if (recipeIndex !== -1) {
        userMealPlans.splice(recipeIndex, 1);
        toastr.warning(`Recipe successfully removed from your meal plan`);

        if (userMealPlans.length === 0) {
            eraseUserLocalStorage();
            return;
        }

    } else {
        toastr.error(`Error: Recipe not found in meal plan`);
        return;
    }

    localStorage.setItem(currentUserId, JSON.stringify(userMealPlans));
}

export function eraseUserLocalStorage() {
    localStorage.removeItem(currentUserId);
}

export function reindexMealRows() {
    
    $('.meal-row').each(function (rowIndex) {
        
        $(this).find('input[id^="Meals_"], select[id^="Meals_"]').each(function (index, element) {
            
            let idParts = element.id.split('__');
            console.log("Old id: " + idParts);
            let newId = 'Meals_' + rowIndex + '__' + idParts[1];
            console.log("New id: " + newId);
            $(element).attr('id', newId);

            let nameParts = element.name.split('.');
            console.log("Old name: " + nameParts);
            let newName = 'Meals[' + rowIndex + '].' + nameParts[1];
            console.log("New name: " + newName);
            $(element).attr('name', newName);
        });
    });

    $('span[data-valmsg-for^="Meals["]').each(function (index, element) {
        let $element = $(element);
        $element.attr('data-valmsg-for', $element.attr('data-valmsg-for').replace(/\[\d+\]/g, '[' + index + ']'));
    });

    $('form').validate().form();
}

  
