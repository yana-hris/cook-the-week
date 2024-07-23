$(document).on('keyup', '.ingredientName', suggestWords);

function suggestWords(event) {
    let inputValue = $(this).val();
    let suggestionsContainer = this.parentNode.querySelector("div.suggestionsList");

    if (inputValue.length > 1) {
        $.ajax({
            url: 'https://localhost:7279/api/recipeingredient/suggestions',
            type: "get",
            data: {
                input: inputValue
            },
            dataType: 'json',
            success: function (response) {
                renderSuggestionResults(response, inputValue, suggestionsContainer, event.target);
            }
        });
    } else {
        $(suggestionsContainer).empty();
    }
}

function renderSuggestionResults(results, search, container, inputForm) {
    // delete unordered list from previous search result
    $(container).empty();

    // get properties from input field
    let form_font = window.getComputedStyle(inputForm, null).getPropertyValue('font-size');
    let form_width = inputForm.offsetWidth;
    
    container.style.width = form_width.toString() + 'px';

    if (results.length > 0) {
        // create ul and set classes
        let ul = document.createElement('UL');
        ul.classList.add('list-group', 'mt-2');

        // create list of results and append to ul
        results.map(function (item) {

            let a = document.createElement('A');
            a.classList.add('autocomplete-result', 'list-group-item', 'p-1'); // autocomplete used for init click event, other classes are from bootstrap
            a.style.fontSize = form_font;
            a.href = "#";
            a.setAttribute("id", item.id); // used for click-Event to fill the form
            
            a.innerHTML = colorResults(item.name, search);

            // add Eventlistener for chosen renderResults
            a.addEventListener("click", function (event) {
                event.preventDefault();
                event.stopPropagation();
                
                let ingredientName = item.name;

                $(inputForm).val(ingredientName).trigger('change');

                // clear results div after a suggestion is clicked
                $(container).empty();
                container.classList.add('invisible');

            });
            ul.append(a);
        });
        
        container.append(ul);
        container.classList.remove('invisible');

    }
    else {
        container.classList.add('invisible');

    }
}
// create colored marked search strings
function colorResults(string, search) {
    let splitted = string.toLowerCase().split(search.toLowerCase());

    let sp = []; 
    let start = 0; 

    splitted.map(function (element, index) {
        // empty string at the beginning
        if (element == false) {
            sp.push("<span class='text-success'>" + string.slice(start, start + search.length) + "</span>");
            start = start + search.length;
        }
        else if (index + 1 == splitted.length) {
            sp.push("<span>" + string.slice(start, start + element.length) + "</span>");
        }
        else {
            sp.push("<span>" + string.slice(start, start + element.length) + "</span>");
            start = start + element.length;
            sp.push("<span class='text-success'>" + string.slice(start, start + search.length) + "</span>");
            start = start + search.length;
        }
    });
    return sp.join('')
}



    