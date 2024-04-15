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
    // Invoke your function here
    showOrHideBuildMealPlanBtn();
};
// Create or Add To MealPlan
function addRecipeToMealPlan(event, recipeId) {
    event.preventDefault();
    debugger;
    if (!isAddedToMealPlan(recipeId)) {
        var mealPlan = JSON.parse(localStorage.getItem('MealPlan')) || [];
        mealPlan.push({ id: recipeId });
        localStorage.setItem('MealPlan', JSON.stringify(mealPlan));
        showOrHideBuildMealPlanBtn();
    }
    else {
        toastr.warning("This recipe is already added to your Meal Plan!");
    }
}

// Check if a recipe is added to the MealPlan
function isAddedToMealPlan(recipeId) {
    var allRecipes = getRecipesFromLocalStorage();
    // Check if the specified recipeId exists in the list of added recipes
    var isAdded = allRecipes.some(recipe => recipe.id === recipeId);

    return isAdded;
}

// Function to get all recipes from local storage
function getRecipesFromLocalStorage() {
    return JSON.parse(localStorage.getItem('MealPlan')) || [];
}

// Function to remove a recipe from local storage
function removeRecipeFromLocalStorage(recipeId) {
    var mealPlan = JSON.parse(localStorage.getItem('MealPlan')) || [];
    var updatedMealPlan = mealPlan.filter(recipe => recipe.id !== recipeId);
    localStorage.setItem('MealPlan', JSON.stringify(updatedMealPlan));
    showOrHideBuildMealPlanBtn();
}

// Show btn
function showOrHideBuildMealPlanBtn() {
    var buildBtn = document.getElementById("build-btn-container");
    var recipes = getRecipesFromLocalStorage();
    debugger;
    if (recipes.length > 0) {
        buildBtn.removeAttribute("hidden");
    } else {
        buildBtn.setAttribute('hidden', '');
    }

    showOrHideWelcomeMassage();
}

function showOrHideWelcomeMassage() {
    var para = document.getElementById("welcome-message");
    var recipes = getRecipesFromLocalStorage();
    if (recipes.length > 0) {
        para.setAttribute("hidden", '');
    } else {
        buildBtn.removeAttribute('hidden');
    }
}


