﻿import { AppConfig }  from './config.js';

//document.addEventListener('DOMContentLoaded', () => {
//    console.log("Current View:", AppConfig.currentView);
//    console.log("Current User ID:", AppConfig.currentUserId);
//    console.log("Has Active Meal Plan:", AppConfig.hasActiveMealplan);

//    if (AppConfig.hasActiveMealplan) {
//        console.log("User has an active meal plan.");
//    }
//});

const currentUserId = AppConfig.currentUserId;
const hasActiveMealplan = AppConfig.hasActiveMealplan;

const emptyGuid = "00000000-0000-0000-0000-000000000000";
const buildBtnContainer = document.getElementById('build-btn-container'); 


const buildMealPlan = function (event) {
    event.preventDefault();
    event.stopPropagation();

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
            if (response.success) {
                window.location.href = response.redirectUrl;
            }                
        },
        error: function (xhr, status, error) {
            if (!hasActiveMealplan) {
                console.error('Error building the meal plan:', error);
                toastr.error("Meal plan creation failed! Try again later.");
            } else if (hasActiveMealplan) {
                console.error('Error adding meals to the active meal plan:', error);
                toastr.error("Adding meals to your active meal plan failed! Try again later.");
            }
            window.location.href = '/Recipe/All';
        }
    });
}

const attachHandlerOnBuildBtnAppearance = function () {

    if (!buildBtnContainer) {
        return;
    }

    let handlerAttached = false; //flag

    const isVisible = (btn) => {
        return $(btn).is(':visible');
    }

    const attachHandler = () => {
        if (!handlerAttached) {
            $(buildBtnContainer).on('click', function (event) {
                console.log('Btn clicked!');
                buildMealPlan(event);
            });
            handlerAttached = true;
        }
    };

    if (isVisible(buildBtnContainer)) {
        attachHandler();
    }

    // A MutationObserver to monitor visibility changes
    const observer = new MutationObserver(() => {
        if (isVisible(buildBtnContainer) && !handlerAttached) {
            console.log("Button is now visible. Attaching handler..");
            attachHandler();
            observer.disconnect(); // Stop observing
        }
    });

    observer.observe(buildBtnContainer, { attributes: true, aatributesFilter: ['style'] });
}
$(function () { 

    attachHandlerOnBuildBtnAppearance();

    const shouldRenderBuildBtn = viewWithBuildBtn() && userIsLoggedIn();

    if (shouldRenderBuildBtn) {

        updateBuildBtnVisibility(); // show the btn first
        updateRecipeBtns(); // Update buttons based on local storage

    } 
    delegateMealPlanEvents();
});

function delegateMealPlanEvents() {
    $(document).on('click', '.add-to-mealplan-container .mealplan-toggle-btn', function (event) {
        event.preventDefault();
        event.stopPropagation();

        debugger;
        const btn = $(this)[0];
        const recipeId = btn.dataset.recipeid;

        if (btn.classList.contains('plus')) {
            debugger;
            const added = addRecipeToMealPlan(recipeId);
            if (added) {
                setButtonTo(btn, 'minus');
                toastr.success(`Recipe successfully added to you meal plan`);
                updateBuildBtnVisibility();
            } else {
                toastr.error(`Recipe is already added to your meal plan`);
            }
        } else if (btn.classList.contains('minus')) {
            const removed = removeRecipeFromLocalStorage(recipeId);
            if (removed) {
                setButtonTo(btn, 'plus');
                updateBuildBtnVisibility();
                toastr.warning(`Recipe successfully removed from your meal plan`);
            } else {
                console.error(`Error: Recipe not found in meal plan`); 
            }
            
        }
    })
}

function userIsLoggedIn() {
    return currentUserId && currentUserId !== emptyGuid;
}
function updateRecipeBtns() {
    let userMealPlans = getUserLocalStorage() || [];
    const recipeButtons = document.querySelectorAll('.add-to-mealplan-container .mealplan-toggle-btn');

    if (userHasMealPlans()) {
        userMealPlans = JSON.parse(userMealPlans);
    }

    if (userMealPlans.length > 0) {
        recipeButtons.forEach(btn => {
            const recipeId = btn.getAttribute('data-recipeId');
            //const icon = btn.querySelector('i');

            if (userMealPlans.includes(recipeId)) {
                setButtonTo(btn, 'minus');
                
            } else {
                setButtonTo(btn, 'plus');
              
            }
        })
    } else {
        recipeButtons.forEach(btn => {
            if (btn.classList.contains('minus')) {
                setButtonTo(btn, 'plus');
               
            } 
        })
    }
}

const setButtonTo = function (btn, className) {
    const oppositeClass = className === 'minus' ? 'plus' : 'minus';

    btn.classList.remove(oppositeClass);
    btn.classList.add(className);

    const icon = btn.querySelector('i');
    icon.classList.remove(`fa-${oppositeClass}`);
    icon.classList.add(`fa-${className}`);
}

const viewWithBuildBtn = function () {
    return $('.has-build-btn').length > 0;
};

function userHasMealPlans() {
    
    const userMealPlans = JSON.parse(getUserLocalStorage());
    
    if (userMealPlans && userMealPlans.length) {
        return true;
    } else if (userMealPlans && !userMealPlans.length) {
        eraseUserLocalStorage();
    }

    return false;
    
}

function updateBuildBtnVisibility() {
    const buildBtnContainer = document.getElementById('build-btn-container');  
        
    if (currentUserId !== null && userHasMealPlans()) {

        buildBtnContainer.style.display = "block";        
    } else {     
        
        buildBtnContainer.style.display = "none";
    }
}

function getUserLocalStorage() {
    return localStorage.getItem(currentUserId);
}

const addRecipeToMealPlan = function(recipeId) {
    
    let userMealPlans = getUserLocalStorage();
    userMealPlans = userMealPlans ? JSON.parse(userMealPlans) : [];    

    if (userMealPlans.includes(recipeId)) {
        return false;
    } 

    // Add the recipe to the meal plan
    userMealPlans.push(recipeId);
    localStorage.setItem(currentUserId, JSON.stringify(userMealPlans));
    return true;
}

// Gets the antiforgery token
function getAntiForgeryToken() {
    return $('#antiForgeryForm input[name="__RequestVerificationToken"]').val();
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
        return false;
    }

    let userMealPlans = JSON.parse(getUserLocalStorage());

    const recipeIndex = userMealPlans.indexOf(recipeId);
    if (recipeIndex === -1) {
        return false; // Recipe not found
    } 

    userMealPlans.splice(recipeIndex, 1);

    if (userMealPlans.length === 0) { // Erase user local storage in case of no more meals
        eraseUserLocalStorage();
        return true;
    }

    localStorage.setItem(currentUserId, JSON.stringify(userMealPlans));
    return true;
}

export function eraseUserLocalStorage() {
    localStorage.removeItem(currentUserId);
}

export function reindexMealRows() {
    
    $('.meal-row').each(function (rowIndex) {

        const $row = $(this);
        
        $row.find('input[id^="Meals_"], select[id^="Meals_"]').each(function (index, element) {

            const $element = $(this);

            // Update 'id' attr
            const idParts = $element.attr('id').split('__');
            const newId = `Meals_${rowIndex}__${idParts[1]}`;
            $element.attr('id', newId);

            // Update 'name' attr
            const nameParts = $element.attr('name').split('.');
            const newName = `Meals[${rowIndex}].${nameParts[1]}`;
            //console.log("New name: " + newName);
            $element.attr('name', newName);

            // Update aria-describedby (if any)
            const describedBy = $element.attr('aria-describedby');
            if (describedBy) {
                const describedByParts = describedBy.split('__');
                const newDescribedBy = `Meals_${rowIndex}__${describedByParts[1]}`;
                $element.attr('aria-describedby', newDescribedBy);
            }
        });

        $row.find('span[data-valmsg-for^="Meals["]').each(function () {
            const $span = $(this);
            const valmsgForParts = $span.attr('data-valmsg-for').split('.');
            const newValmsgFor = `Meals[${rowIndex}].${valmsgForParts[1]}`;
            $span.attr('data-valmsg-for', newValmsgFor);

        });
    });
    
    $('form').validate().form();
}

export const attachToggleFiltersHandler = function() {
    $(document).on('click', '.filter-btn-container', function (e) {        
        toggleAdvancedSectionVisibility(e, this);
    });
}


const toggleAdvancedSectionVisibility = function (e, btn) {
    e.preventDefault();
    
    const filterSpan = btn.querySelector('.filter-link .btnText');
    const filtersDiv = document.querySelector('#tags');

    if (filtersDiv.classList.contains('hidden')) {
        filtersDiv.classList.remove('hidden');
        filterSpan.textContent = "Hide advanced filters";
        toggleIcons(btn);

    } else {
        filterSpan.textContent = "More advanced filters";
        toggleIcons(btn);
        filtersDiv.classList.add('hidden');
    }
}

const toggleIcons = function (btn) {
    if (btn) {
        const icon = btn.querySelector('a i');

        if (icon.classList.contains('fa-circle-chevron-up')) {
            icon.classList.remove('fa-circle-chevron-up');
            icon.classList.add('fa-circle-chevron-down');
        } else {
            icon.classList.remove('fa-circle-chevron-down');
            icon.classList.add('fa-circle-chevron-up');
        }
    }
    return;
}

  
