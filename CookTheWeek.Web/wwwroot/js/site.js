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
    debugger
    var userId = currentUserId;
    let userMealPlans = getUserLocalStorage(userId) || [];
    const recipeButtons = document.querySelectorAll('.add-to-mealplan-button');

    if (userHasMealPlans) {
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
            btn.addEventListener('click', addRecipeToMealPlan, true);
        })
    }
}
function isSpecificView(view) {
    // Check if the current View is any of these   
    const specificViews = ["All Recipes", "Recipe Details", "My Recipes"];
    return specificViews.includes(view);
};

function userHasMealPlans(userId) {
    var mealPlans = getUserLocalStorage(userId);
    if (mealPlans !== null) {

        var mealsPlansAsObj = JSON.parse(mealPlans);
        if (mealsPlansAsObj.length > 0) {
            return true;
        }        
    }
    return false;
}

// Show or Hide Build Meal Plan Btn
function showOrHideBuildMealPlanBtn(userId) {
    var buildBtn = document.getElementById("build-btn-container");

    if (userId !== null && userHasMealPlans(userId)) {
        buildBtn.removeAttribute("hidden");
    } else {
        buildBtn.setAttribute('hidden', '');
    }
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
const addRecipeToMealPlan = function(event) {
    // Check if local storage has meal plans for this user
    debugger;
    event.preventDefault();
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
    debugger;
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
    debugger;    
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

function buildMealPlan() {
    
    const userId = currentUserId;
    var userMealPlanList = JSON.parse(getUserLocalStorage(userId)) || [];
    
    if (userMealPlanList.length === 0) {
        toastr.error("Error Building Meal Plan!");
        return;
    }

    var data = {
        UserId: userId,
        Meals: userMealPlanList.map(recipeId => ({ RecipeId: recipeId }))
    };

    console.log(data);


    // Make an HTTP POST request to your controller
    $.ajax({
        url: 'https://localhost:7279/api/mealplan/buildMealPlan',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function (data) {
            // Handle the successful response from the Web API
            console.log('Received view model data:', data);
            // Make a subsequent AJAX request to the MVC controller endpoint
            $.ajax({
                url: 'https://localhost:7170/MealPlan/Add', // URL of the MVC controller action
                method: 'POST', // or 'GET', depending on your MVC controller action
                contentType: 'application/json',               
                data: JSON.stringify(data), // Pass the received JSON data directly
                success: function (response) {
                    // Handle the successful response from the MVC controller
                   
                    console.log('Successfully sent model data to MVC controller');
                    // Optionally, do something with the response from the MVC controller
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    // Handle the error from the MVC controller request

                    console.error('Error sending model data to MVC controller:', errorThrown);
                }
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            // Handle the error from the Web API request
            console.error('Error building meal plan:', errorThrown);
        }
    });
}