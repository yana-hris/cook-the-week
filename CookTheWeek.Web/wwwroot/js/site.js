function initialize() {
    debugger;
    $('#addIngredient').on('keyup', function () {
        let input = $(this).val();
        if (input.length > 1) {
            $.get('https://localhost:7279/api/recipeingredient/suggestions', { input: input }, function (data) {
                $('#ingredientsSuggestionList').empty();

                const parsedData = JSON.parse(data);

                $.each(parsedData, function (index, dataItem) {
                    const li = createNewItem(dataItem);
                    $('#ingredientsSuggestionList').append(li);
                })
            }, "json");
        } else {
            $('#ingredientsSuggestionList').empty();
        }
    });
    $('#removeRecipeIngredient').hide();


    $('#addRecipeIngredient').on('click', function (event) {
        event.preventDefault();
        event.stopPropagation();
        let ingredient = $('#addIngredient').val();
        if (ingredient.length > 2) {
            createNewIngredientLine(ingredient);
            $('#addIngredient').val('');
        }
    });

    function createNewItem(itemObject) {
        const li = document.createElement("li");
        li.textContent = itemObject.name;
        li.value = itemObject.id;
        return li;
    }


    function toggleIngredientBtns(event) {
        event.preventDefault();
        event.stopPropagation();

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


    $('div button').on('click', toggleIngredientBtns(e));
}


