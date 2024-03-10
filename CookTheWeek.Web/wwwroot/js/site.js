$(initialize());
function initialize() {
    $(document).ready(function () {
        resetForm($('.ingredient-container'));
    });
    $('div.addButton').on('click', function (event) {
        addIngredient(event);
    });
    $('div.removeBtn').on('click', function (event) {
        removeIngredient(event);
    });

    $('.addIngredient').on('keyup', function () {
        let inputValue = $(this).val();
        $(this).next().empty();
        let contextForm = this;
        let ingredientContainer = this.parentNode.querySelector("div");

        if (inputValue.length > 1) {
            $.ajax({
                url: 'https://localhost:7279/api/recipeingredient/suggestions',
                type: "get",
                data: {
                    input: inputValue
                },
                dataType: 'json',
                success: function (response) {
                    renderSuggestionResults(response, inputValue, ingredientContainer, contextForm);
                }
            });
        } else {
            $(this).next().empty();
        }
    });

    // render the result list in input drop-down list
    function renderSuggestionResults(results, search, container, inputForm) {
        // delete unordered list from previous search result
        $(container).empty();

        // get properties from input field
        let form_font = window.getComputedStyle(inputForm, null).getPropertyValue('font-size');
        let form_width = inputForm.offsetWidth;

        //set result list to same width less borders
        container.style.width = form_width.toString() + 'px';

        if (results.length > 0) {
            // create ul and set classes
            let ul = document.createElement('UL');
            ul.classList.add('list-group', 'mt-2');

            // create list of results and append to ul            
            results.map(function (item) {
                let a = document.createElement('A');
                a.classList.add('autocomplete-result', 'list-group-item', 'p-1'); // autocomplete used for init click event, other classes are from bootstrap
                a.setAttribute("reference", item.id); // used for click-Event to fill the form
                a.style.fontSize = form_font;
                a.href = "#";

                // see function below - marked search string in results
                a.innerHTML = colorResults(item.name, search);

                // add Eventlistener for search renderResults
                a.addEventListener("click", function (event) {
                    event.preventDefault();
                    event.stopPropagation();

                    // get text from list item and set it into reffered form field
                    let ingredientName = a.innerText;
                    let ingredientId = a.getAttribute('reference');
                    // TODO: save the ID in serviceViewModel?!!
                    inputForm.value = ingredientName;

                    // after choosen a result make div with results invisible -> after changing input content again,
                    // all of childs of current div will be deleted [line 48,49]
                    container.classList.add('invisible');

                });
                ul.append(a);
            });

            // append ul to container and make container visible
            container.append(ul);
            container.classList.remove('invisible');
            //choose_result(); // add Eventlistener to every result in ul
        }
        else {
            container.classList.add('invisible');

        }
    }

    // create span's with colored marked search strings
    function colorResults(string, search) {
        let splitted = string.toLowerCase().split(search.toLowerCase());

        let sp = []; // array of all spans, created in folling loop
        let start = 0; //start for slicing

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
}

function resetForm(ingredientContainer) {
    $(ingredientContainer).find('input').each(function () {
        $(this).val("");
    });
    $(ingredientContainer).find('select').each(function () {
        $(this).val("");
    });
    $(ingredientContainer).find('.addButton').show();
    $(ingredientContainer).find('.removeBtn').hide();
}
function addIngredient(event) {
    event.preventDefault();
    event.stopPropagation();

    const addBtnDiv = this.event.currentTarget;
    const removeBtnDiv = addBtnDiv.parentNode.querySelector("div.removeBtn");

    // Toggle add/remove btns
    $(addBtnDiv).hide();
    $(removeBtnDiv).show();

    // Clone the form before disabled 
    let newItem = $('.ingredient-container').last().clone(true);

    // Disable all input fields in current form
    const formContainer = addBtnDiv.parentNode.parentNode;
    Array.from(formContainer.querySelectorAll("input"))
        .forEach(input => {
            input.disabled = true;
        });
    Array.from(formContainer.querySelectorAll("select"))
        .forEach(select => {
            select.disabled = true;
        });

    // Reset all fields of the cloned form
    resetForm(newItem);
    // Attach to DOM the cloned and reset form
    $(newItem).appendTo('#ingredients');
}

function removeIngredient(event) {
    event.preventDefault();
    event.stopPropagation();

    const removeBtnDiv = this.event.currentTarget;
    const currentIngredientContainer = removeBtnDiv.parentNode.parentNode;
    const allIngredientsContainer = currentIngredientContainer.parentNode;

    if ($(allIngredientsContainer).children().length > 1) {
        //just remove the current line and do nothing else
        $(currentIngredientContainer).remove();
    } else {
        // reset the form and btns
        $(currentIngredientContainer).find('input').each(function () {
            $(this).val("");
        });
        $(currentIngredientContainer).find('.addButton').show();
        $(removeBtnDiv).hide();
    }
}

