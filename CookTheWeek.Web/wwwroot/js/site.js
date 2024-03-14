
function initialize() {
    var triggerTabList = [].slice.call(document.querySelectorAll('#recipe button'));
    triggerTabList.forEach(function (triggerEl) {
        var tabTrigger = new bootstrap.Tab(triggerEl)

        triggerEl.addEventListener('click', function (event) {
            event.preventDefault()
            tabTrigger.show()
        })
    });
    resetForm();
    // attach event to add ingredient btn
    $('#addButton').on('click', (event) => {
        addIngredient(event);
    });

    // attach event on key-up ingredient input (word-suggestions functionality)
    $('#addIngredient').on('keyup', function () {
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
                a.style.fontSize = form_font;
                a.href = "#";
                a.setAttribute("id", item.id); // used for click-Event to fill the form

                // see function below - marked search string in results
                a.innerHTML = colorResults(item.name, search);

                // add Eventlistener for chosen renderResults
                a.addEventListener("click", function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    // get text from list item and set it into reffered form field
                    let ingredientName = a.innerText;
                    let ingredientId = $(a).attr('id');

                    inputForm.value = ingredientName;
                    $('#addButton a.btn').attr("id", ingredientId.toString());

                    // after a result is chose, make the div with results invisible -> or after changing input content again,
                    // all of childs of current div will be deleted [line 48,49]
                    container.classList.add('invisible');

                });
                ul.append(a);
            });

            // append ul to container and make container visible
            container.append(ul);
            container.classList.remove('invisible');

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

function resetForm() {
    $("form").each(function () {
        $(this).find(":input").each(function () {
            $(this).val("");
        });
        $(this).find("select").each(function () {
            $(this).val("");
        })
    });
}

function removeIngredient(event) {
    event.preventDefault();
    event.stopPropagation();
    const button = this.event.currentTarget;
    const row = button.parentNode.parentNode;
    $(row).remove();
}

function addIngredient(event) {
    event.preventDefault();
    event.stopPropagation();

    const name = $('#addIngredient').val();
    const ingredientId = $('#addButton a.btn').attr('id').toString();
    const qty = $('#qty').val();
    const measureName = $('#measure :selected').text();
    const measureId = $('#measure').val();
    const specName = $("#spec :selected").text();
    const specId = $("#spec").val();

    resetForm();
    // TODO: might use this for DTO in RecipeController (upon click of Add Recipe)
    // const ingredientViewModelInfo = {
    //     IngredientId: ingredientId,
    //     Name: name,
    //     Qty: qty,
    //     MeasureId: measureId,
    //     SpecificationId: specId
    // };

    const ingredientInfo = {
        id: ingredientId,
        name: name,
        qty: qty,
        measureName: measureName,
        specName: specName
    };

    createTableRow(ingredientInfo);
}

function createTableRow(ingredientInfo) {

    const tr = document.createElement("tr");
    const id = ingredientInfo.id;
    tr.setAttribute("id", id.toString());
    const nameTd = document.createElement('td');
    $(nameTd).text(ingredientInfo.name);

    const qtyTd = document.createElement('td');
    $(qtyTd).text(ingredientInfo.qty);

    const measureTd = document.createElement('td');
    $(measureTd).text(ingredientInfo.measureName);

    const specTd = document.createElement("td");
    $(specTd).text(ingredientInfo.specName);

    const btnTd = document.createElement("td");
    const btnContainer = document.createElement("div");
    btnContainer.classList.add("removeBtn");
    btnContainer.setAttribute("id", id.toString());
    btnContainer.innerHTML = '<ion-icon class="icons remove" name="remove-circle"></ion-icon><a href="#" class="btn"></a>';
    btnTd.appendChild(btnContainer);
    $(btnContainer).on('click', (event) => removeIngredient(event));

    tr.appendChild(nameTd);
    tr.appendChild(qtyTd);
    tr.appendChild(measureTd);
    tr.appendChild(specTd);
    tr.appendChild(btnTd);

    $(tr).appendTo($('#ingredientsList'));
}

