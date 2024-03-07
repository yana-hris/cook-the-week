$(document).ready(initilize);

function initilize() {
    $('div button').on('click', toggleIngredientBtns(e));

    $('#addIngredient').on('keyup', function () {
        let input = $(this).val();
        if (input.length > 1) {
            $.ajax({
                url: '@Url.Action("GetSuggestions", "RecipeIngredientController")',
                type: 'GET',
                data: { input: input },
                dataType: 'json',
                success: function (data) {
                    const parsedArray = JSON.parse(data);
                    displaySuggestions(parsedArray);
                },
                error: function () {
                    alert('Error occurred while retrieving suggestions!');
                }
            });

        } else {
            $('#ingredientsSuggestionList').empty();
        }
    });
    $('#removeRecipeIngredient').hide();

    $('#addRecipeIngredient').on('click', function (event) {
        event.preventDefault();
        let ingredient = $('#addIngredient').val();
        if (ingredient.length > 2) {
            createNewIngredientLine(ingredient);
            $('#addIngredient').val('');
        }
    });
}

function displaySuggestions(suggestions) {
    $('#ingredientsSuggestionList').empty();
    $.each(suggestions, function (index, suggestion) {
        let li = createNewItem(suggestion);
        $('#ingredientsSuggestionList').append(li);
    });
}

function createNewItem(itemText) {
    const li = document.createElement("li");
    li.textContent = itemText;
    return li;
}

function toggleIngredientBtns(event) {
    event.preventDefault();
    let parentNode = $(this).parent();

    if (parentNode.classList.contains("addButton")) {
        const addBtn = this.event.target;
        const removeBtn = parentNode.parent().querySelector('div.removeBtn button');

        $(addBtn).hide();
        $(removeBtn).show();

    } else if (parentNode.classList.contains("removeBtn")) {
        const addBtn = parentNode.parent().querySelector('div.addBtn button');
        const removeBtn = this.event.target;

        $(addBtn).show();
        $(removeBtn).hide();
    }

}

function createNewIngredientLine(ingredient) {
    const ingredientLine = document.createElement("div"); // <div class="form-group d-flex col-5">
    ingredientLine.classList.add("form-group", "d-flex", "col-5");

    const btnContainer = document.createElement("div"); //<div class="removeBtn">
    btnContainer.classList.add("removeBtn");
    const removeBtn = document.createElement("button");
    removeBtn.classList.add("btn", "btn-danger", "text-light"); //<button class="btn btn-danger text-light"> -</button>
    removeBtn.value = "-";

    btnContainer.append(removeBtn);

    const inputFiled = document.createElement("input");
    inputFiled.classList.Add("form-control me-2");
    inputFiled.disabled = true;
    inputFiled.textContent = ingredient;
    inputFiled.style = "min-width: 12rem";

    ingredientLine.append(inputFiled);
    ingredientLine.append(btnContainer);

    $(ingredientLine).insertBefore('.input-form');
}