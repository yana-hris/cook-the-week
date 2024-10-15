$(document).on('keyup', '.ingredientName', debounce(suggestWords, 300)); 

// Initial resize when the page loads
$(document).ready(function () {
    resizeSuggestionContainers(); // Ensure the suggestion windows are sized correctly
});

// Event listener to trigger resizing when the viewport is resized
$(window).on('resize', function () {
    resizeSuggestionContainers();  // Recalculate the width of all suggestion windows
});


function suggestWords(event) {
    let inputValue = $(this).val();
    let suggestionsContainer = this.parentNode.querySelector("div.suggestionsList");

    if (inputValue.length > 2) {
        $.ajax({
            url: '/RecipeIngredient/RenderSuggestions',
            type: 'GET',
            data: { input: inputValue },
            dataType: 'json',
            success: function (response) {
                renderSuggestionResults(response, inputValue, suggestionsContainer, event.target);
            },
            error: function (xhr, status, error) {
                console.error('Error fetching suggestions:', error);
                $(suggestionsContainer).html('<div class="text-danger">Error fetching suggestions.</div>');
            }
        });
    } else {
        $(suggestionsContainer).empty();
    }
}

function renderSuggestionResults(results, search, container, inputForm) {
    $(container).empty();  // Clear previous suggestions

    let formFont = window.getComputedStyle(inputForm, null).getPropertyValue('font-size');
    let formWidth = inputForm.offsetWidth;

    container.style.width = formWidth + 'px';

    if (results.length > 0) {
        let ul = document.createElement('UL');
        ul.classList.add('list-group');

        results.forEach(item => {
            let a = document.createElement('A');
            a.classList.add('autocomplete-result', 'list-group-item', 'p-1');
            a.style.fontSize = formFont;
            a.href = "#";
            a.setAttribute("id", item.id);
            a.innerHTML = colorResults(item.name, search);

            // Attach click event to fill the form
            a.addEventListener("click", event => {
                event.preventDefault();
                event.stopPropagation();

                let ingredientName = item.name;
                let ingredientId = item.id;

                // Get Knockout context and update observables
                let koContext = ko.dataFor(inputForm);
                if (koContext) {
                    koContext.Name(ingredientName);
                    koContext.IngredientId(ingredientId);
                }

                $(container).empty();  // Clear suggestions after selection
                container.classList.add('invisible');
            });

            ul.appendChild(a);
        });

        container.append(ul);
        container.classList.remove('invisible');
    } else {
        container.classList.add('invisible');
    }
}

// Debounce utility function to prevent too many requests
function debounce(func, wait) {
    let timeout;
    return function (...args) {
        const context = this;
        clearTimeout(timeout);
        timeout = setTimeout(() => func.apply(context, args), wait);
    };
}

// Highlight the matching part of the suggestions
function colorResults(string, search) {
    // Escape any special characters in the search string for regex
    const searchEscaped = search.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');

    // Create a case-insensitive regular expression to match the search term
    const regex = new RegExp(`(${searchEscaped})`, 'gi');

    // Replace matching parts of the string with a span that highlights it
    return string.replace(regex, '<span class="color-result">$1</span>');
}

// Function to resize suggestion containers based on input field width
function resizeSuggestionContainers() {
    // Select all input fields with class 'ingredientName'
    $('.ingredientName').each(function () {
        let inputField = $(this);  // This is the input field
        let suggestionsContainer = inputField.siblings('.suggestionsList');  // The suggestions list is a sibling

        // Adjust the width of the suggestion container to match the input field width
        let inputWidth = inputField.outerWidth();
        suggestionsContainer.css('width', inputWidth + 'px');
    });
}




