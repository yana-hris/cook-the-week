﻿import { AppConfig } from './config.js';
const currentUserId = AppConfig.currentUserId;

const toggleFavourites = function (event, recipeId) {
    event.preventDefault();
    event.stopPropagation();
    // Change the state of the btn & icon
    toggleLikeButton(event);
    debugger;
    let url = '/FavouriteRecipe/ToggleFavourites'; 

    var data = {
        RecipeId: recipeId,
        UserId: currentUserId
    }

    var antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();

    $.ajax({
        url: url,
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        headers: {
            "RequestVerificationToken": antiForgeryToken // Include the token in the request headers
        },

        success: function (response) {
            debugger;
            toastr.success("Your preference is saved!");
        },
        error: function (xhr, status, error) {
            // Display generic error message
            toastr.error('Your preference not saved:', error);
            // In case of error, return the icon status to the previous state
            toggleLikeButton(event);
        }
    });
}

const toggleLikeButton = function (event) {
    const $button = $(event.currentTarget);

    $button.find('i').toggleClass('liked-icon not-liked-icon');

    // Check the current state and set the new title based on the state
    let isLiked = $button.find('i').hasClass('liked-icon');
    let newTitle = isLiked ? 'Remove from Favorites' : 'Add to Favorites';

    // Update the title and refresh the tooltip
    $button.find('i')
        .attr('title', newTitle)
        .tooltip('dispose') // Dispose of the current tooltip
        .tooltip(); // Re-initialize the tooltip
}

export const attachToggleFavouritesHandler = function () {
    
    $(document).on('click', '.add-to-favourites-button', function (event) {        
        const recipeId = $(this).data('recipe-id');
        toggleFavourites(event, recipeId);
    });    
}



 