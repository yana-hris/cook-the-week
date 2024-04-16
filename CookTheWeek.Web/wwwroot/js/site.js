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
    }   
    return;
};

function isSpecificView(view) {
    // Check if the current View is any of these   
    const specificViews = ["All Recipes", "Recipe Details", "My Recipes"];
    return specificViews.includes(view);
};

function userHasMealPlans(userId) {
    var mealPlans = getUserLocalStorage(userId);
    if (mealPlans !== null) {
        return true;
    }
    return false;
}
// Check if user has Local Storage Meal Plan:
function getUserLocalStorage(userId) {
    return localStorage.getItem(userId);
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
function addRecipeToMealPlan(event, userId, recipeId) {
    // Check if local storage has meal plans for this user
    event.preventDefault();
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
        toggleAddRemoveBtn(event,userId, recipeId);
        toastr.success(`Recipe "${recipeId}" added to meal plan`);
    } else {
        toastr.error(`Recipe is already added to your plan`);
    }

    // Save the updated meal plans back to local storage
    localStorage.setItem(userId, JSON.stringify(userMealPlans));
}

// Remove recipe from Meal Plan
function removeRecipeFromMealPlan(event, userId, recipeId) {
    // Get the user's meal plans from local storage
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
        toggleAddRemoveBtn(event, userId, recipeId);
        toasrt.success(`Recipe successfully removed from the meal plan`);

        // Save the updated meal plans back to local storage
        localStorage.setItem(userId, JSON.stringify(userMealPlans));
    } else {
        toastr.error(`This Recipe has not been added to your meal plan and cannot be removed`);
    }
}

// Show or Hide Build Meal Plan Btn
function showOrHideBuildMealPlanBtn(userId) {
    debugger;
    var buildBtn = document.getElementById("build-btn-container");

    if (userId !== null && userHasMealPlans(userId)) {
        buildBtn.removeAttribute("hidden");
    } else {
        buildBtn.setAttribute('hidden', '');
    }
}


function toggleAddRemoveBtn(event, userId, recipeId) {
    debugger;
    const btn = event.currentTarget;
    const icon = btn.querySelector('i');

    if (btn.classList.contains("plus")) {
        btn.classList.remove("plus");
        btn.classList.add("minus");

        btn.onclick = null;
        btn.onclick = function (event) {
            removeRecipeFromMealPlan(event, userId, recipeId);
        };

        icon.classList.remove("fa-plus");
        icon.classList.add("fa-minus");
    } else {
        btn.classList.remove("minus");
        btn.classList.add("plus");

        btn.onclick = null;
        btn.onclick = function (event) {
            addRecipeToMealPlan(event, userId, recipeId);
        };

        icon.classList.remove("fa-minus");
        icon.classList.add("fa-plus");
    }
}