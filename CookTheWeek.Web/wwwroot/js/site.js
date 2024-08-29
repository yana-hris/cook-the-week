(function () {
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
    var userId = currentUserId;
    var buildBtnShouldBeRendered = isSpecificView(currentView);
    
    if (buildBtnShouldBeRendered) {
        if (userId) {
            showOrHideBuildMealPlanBtn(userId); 
        }
        else {
            showOrHideBuildMealPlanBtn(null);
        }
    updateRecipeBtns(); 
    }
    return;
};

function updateRecipeBtns() {
    var userId = currentUserId;
    let userMealPlans = getUserLocalStorage(userId) || [];

    const recipeButtons = document.querySelectorAll('.add-to-mealplan-container .btn');

    if (userHasMealPlans(userId)) {
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
function isSpecificView(view) {
    // Check if the current View is any of these   
    const specificViews = ["All Recipes", "Recipe Details", "My Recipes"];
    return specificViews.includes(view);
};

function isIndexView(view) {
    return view === "Home Page";
}

function userHasMealPlans(userId) {
    const userMealPlans = getUserLocalStorage(userId);

    // Check if user meal plans exist
    if (userMealPlans) {
        // Parse the existing meal plans
        const mealPlans = JSON.parse(userMealPlans);

        // Check if the meal plans array is not empty
        if (mealPlans.length > 0) {
            // User has meal plans stored in local storage
            return true;
        }
    }

    // User does not have any meal plans stored in local storage
    return false;
}

// Show or Hide Build Meal Plan Btn
function showOrHideBuildMealPlanBtn(userId) {
    var buildBtnContainer = document.getElementById('build-btn-container');    

    if (userId !== null && userHasMealPlans(userId)) {
        // Show the btn and attach event-listener for creating mealplan
        buildBtnContainer.style.display = "block";
        buildBtnContainer.addEventListener('click', (event) => buildMealPlan(event));
    } else {
        // Remove event listener and toggle visibility
        buildBtnContainer.removeEventListener('click', (event) => buildMealPlan(event))
        buildBtnContainer.style.display = "none";
    }
}
// Check if user has Local Storage Meal Plan:
function getUserLocalStorage(userId) {
    return localStorage.getItem(userId);
}

function eraseUserLocalStorage(userId) {
    localStorage.removeItem(userId);
}

// Check if a Recipe is already added to User`s Meal Plan
function isRecipeAddedToMealPlan(userId, recipeId) {
    var userMealPlans = getUserLocalStorage(userId);

    if (!userMealPlans) {
        return false;
    }
    // Parse the user's meal plans
    const mealPlans = JSON.parse(userMealPlans);

    // Check if the recipe ID exists in the user's meal plans
    return mealPlans.includes(recipeId);
}

// Create or Add To MealPlan
const addRecipeToMealPlan = function(event) {
    // Check if local storage has meal plans for this user
    event.preventDefault();
    event.stopPropagation();
    var recipeId = event.currentTarget.dataset.recipeid;
    var userId = currentUserId;
    let userMealPlans = getUserLocalStorage(userId);

    // If the user doesn't have any meal plans yet, create an empty array
    if (!userMealPlans) {
        userMealPlans = [];
    } else {
        // Parse the existing meal plans
        userMealPlans = JSON.parse(userMealPlans);
    }

    // Check if the recipe is already in the meal plan
    const existingRecipeIndex = userMealPlans.findIndex(item => item === recipeId);

    if (existingRecipeIndex === -1) {
        // If the recipe doesn't exist, add it to the meal plan
        userMealPlans.push(recipeId);        
        toastr.success(`Recipe successfully added to meal plan`);
        
    } else {
        toastr.error(`Recipe is already added to your plan`);
        return;
    }
    // Save the updated meal plans back to local storage
    localStorage.setItem(userId, JSON.stringify(userMealPlans));
    toggleAddRemoveBtn(event.currentTarget);
    showOrHideBuildMealPlanBtn(userId);
}

// Remove recipe from Meal Plan
const removeRecipeFromMealPlan = function(event) {
    // Get the user's meal plans from local storage
    event.preventDefault();
    event.stopPropagation();
    var recipeId = event.currentTarget.dataset.recipeid;
    var userId = currentUserId;
    let userMealPlans = getUserLocalStorage(userId);

    // If the user doesn't have any meal plans yet, return
    if (!userMealPlans) {
        toastr.error(`No meal plan yet`);
        return;
    }
    // Parse the user's meal plans
    userMealPlans = JSON.parse(userMealPlans);

    // Check if the recipe ID exists in the user's meal plans
    const recipeIndex = userMealPlans.indexOf(recipeId);

    if (recipeIndex !== -1) {
        // Remove the recipe from the user's meal plans
        userMealPlans.splice(recipeIndex, 1);        
        toastr.success(`Recipe successfully removed from the meal plan`);
        
    } else {
        toastr.error(`This Recipe has not been added to your meal plan and cannot be removed`);
        return;
    }
    localStorage.setItem(userId, JSON.stringify(userMealPlans));
    toggleAddRemoveBtn(event.currentTarget);
    showOrHideBuildMealPlanBtn(userId);
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

function buildMealPlan(event) {
    event.preventDefault();

    const userId = currentUserId;
    var userMealPlanList = JSON.parse(getUserLocalStorage(userId)) || [];

    if (userMealPlanList.length === 0) {
        toastr.error("Error Building Meal Plan!");
        return;
    }

    var model = {
        UserId: userId,
        Meals: userMealPlanList.map(recipeId => ({ RecipeId: recipeId }))
    };
    
    
    $.ajax({
        url: '/MealPlan/CreateMealPlanModel',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(model),        
        success: function (response, status, xhr) {
            // Check if a redirect is happening
            if (xhr.getResponseHeader('X-Redirect') != null) {
                window.location.href = xhr.getResponseHeader('X-Redirect');
            } else {
                console.log('Success:', response);
            }
        },
        error: function (xhr, status, error) {
            // Handle error
            console.error('Post to CreateMealPlanModel Failed!');
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

  
