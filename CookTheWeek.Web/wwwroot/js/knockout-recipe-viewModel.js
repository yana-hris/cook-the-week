//Knockout Viewmodel definition, enabling two-way data binding (client-server side and vise-versa)
function RecipeViewModel(data, errorMessages, fractionOptions, validationConstants) {

    var self = this;

    self.Id = ko.observable(data.Id ? data.Id: '');

    self.Title = ko.observable(data.Title ? data.Title : '').extend({
        required: {
            message: errorMessages.TitleRequiredErrorMessage
        },
        minLength: {
            message: errorMessages.TitleMinLengthErrorMessage,
            params: validationConstants.TitleMinLength
        },
        maxLength: {
            message: errorMessages.TitleMaxLengthErrorMessage,
            params: validationConstants.TitleMaxLength
        },
        validatable: true
    });

    self.Description = ko.observable(data.Description ? data.Description : '').extend({
        minLength: {
            message: errorMessages.DescriptionRangeErrorMessage,
            params: validationConstants.DescriptionMinLength
        },
        maxLength: {
            message: errorMessages.DescriptionRangeErrorMessage,
            params: validationConstants.DescriptionMaxLength
        },
        validatable: true
    });

    self.Servings = ko.observable(data.Servings ? data.Servings : '').extend({
        required: {
            message: errorMessages.ServingsRequiredErrorMessage
        },
        min: {
            message: errorMessages.ServingsRangeErrorMessage,
            params: validationConstants.ServingsMinValue
        },
        max: {
            message: errorMessages.ServingsRangeErrorMessage,
            params: validationConstants.ServingsMaxValue
        },
        validatable: true
    });

    self.ImageUrl = ko.observable(data.ImageUrl ? data.ImageUrl : '').extend({
        required: {
            message: errorMessages.ImageRequiredErrorMessage
        },
        minLength: {
            message: errorMessages.ImageRangeErrorMessage,
            params: validationConstants.ImageUlrMinLength
        },
        maxLength: {
            message: errorMessages.ImageRangeErrorMessage,
            params: validationConstants.ImageUlrMaxLength
        },
        url: true,
        validatable: true
    });

    self.CookingTimeMinutes = ko.observable(data.CookingTimeMinutes ? data.CookingTimeMinutes : 0).extend({
        required: {
            message: errorMessages.CookingTimeRequiredErrorMessage
        },
        min: {
            message: errorMessages.CookingTimeRangeErrorMessage,
            params: validationConstants.CookingTimeMinValue
        },
        max: {
            message: errorMessages.CookingTimeRangeErrorMessage,
            params: validationConstants.CookingTimeMaxValue
        },
        validatable: true
    });

    self.RecipeCategoryId = ko.observable(data.RecipeCategoryId ? data.RecipeCategoryId : '').extend({
        required: {
            message: errorMessages.RecipeCategoryIdRequiredErrorMessage
        },
        validatable: true
    });

    function createStep(newStep) {

        let stepSelf = this;

        stepSelf.Description = ko.observable(newStep.Description ? newStep.Description : '').extend({
            required: {
                message: errorMessages.StepRequiredErrorMessage
            },
            minLength: {
                message: errorMessages.StepDescriptionRangeErrorMessage,
                params: validationConstants.StepDescriptionMinLength
            },
            maxLength: {
                message: errorMessages.StepDescriptionRangeErrorMessage,
                params: validationConstants.StepDescriptionMaxLength
            },
            validatable: true,
        });

        return stepSelf;
    };

    self.Steps = ko.observableArray(ko.utils.arrayMap(data.Steps, function (step) {
        return new createStep(step);
    }));

    self.RecipeIngredients = ko.observableArray(ko.utils.arrayMap(data.RecipeIngredients, function (ingredient) {
        return new createIngredient(ingredient);
    }));

    self.addStep = function () {
        let newStep = new createStep({
            Description: '',
        });

        self.Steps.push(newStep);
    };

    self.removeStep = function (step) {
        if (self.Steps().length > 1) {
            self.Steps.remove(step);
        } else {
            toastr.error(errorMessages.StepsRequiredErrorMessage);
        }
    };

    self.addIngredient = function () {
        let newIngredient = new createIngredient({
            Name: '',
            Qty: { QtyDecimal: null, QtyWhole: null, QtyFraction: '' },
            MeasureId: '',
            SpecificationId: ''
        });

        self.RecipeIngredients.push(newIngredient);
    };

    self.removeIngredient = function (ingredient) {

        if (self.RecipeIngredients().length > 1) {
            self.RecipeIngredients.remove(ingredient);
        } else {
            toastr.error(errorMessages.IngredientsRequiredErrorMessage);
        }
    };

    self.errors = ko.validation.group(self, { deep: true, observable: true });

    self.submitForm = function () {
        // Validate the entire ViewModel
        debugger
        const errorsArr = self.errors();
        self.errors.showAllMessages();

        // Additional validation for steps and ingredients
        const stepsValid = self.Steps().length > 0;
        const ingredientsValid = self.RecipeIngredients().length > 0;

        if (!stepsValid) {
            toastr.error(errorMessages.StepRequiredErrorMessage);
        }

        if (!ingredientsValid) {
            toastr.error(errorMessages.IngredientsRequiredErrorMessage);
        }

        if (errorsArr.length === 0 && stepsValid && ingredientsValid) {

            var jsonData = ko.toJSON(self);
            console.log('Submitting data:', jsonData); // Log data being sent

            // Perform AJAX POST request
            $.ajax({
                url: '/Recipe/Edit',
                type: 'POST',
                contentType: 'application/json',
                data: jsonData,
                success: function (response) {
                    if (response.success && response.redirectUrl) {
                        toastr.success('Recipe edited successfully!');
                        window.location.href = response.redirectUrl;
                    }
                },
                error: function (xhr) {
                    debugger
                    if (xhr.status === 400 && xhr.responseJSON && !xhr.responseJSON.success) {
                        // Display server-side validation errors
                        var errors = xhr.responseJSON.errors;
                        for (var key in errors) {
                            if (errors.hasOwnProperty(key)) {
                                var errorMessages = errors[key].errors;
                                if (errorMessages && errorMessages.length > 0) {
                                    ko.validation.addValidationMessage({
                                        message: errorMessages.join(', '),
                                        observable: self[key]
                                    });
                                }
                            }
                        }
                        self.errors.showAllMessages();
                    } else if (xhr.status === 400) {
                        // Replace the form content with the response HTML
                        $('#edit-recipe').html(xhr.responseText);
                        // Reapply bindings since the form content was replaced
                        ko.applyBindings(new RecipeViewModel(initialData), document.getElementById('edit-recipe'));

                    } else {
                        console.error('Error occurred while submitting form:', xhr.statusText);
                        alert('Error occurred while submitting form. Please try again later.');
                    }
                }
            });
        } else {
            self.errors.showAllMessages();
        }

        return false; // Prevent the default form submission
    }


    return self;

    function createQtyObservable(qty) {

        let self = this;

        self.QtyDecimal = ko.observable(qty.QtyDecimal);
        self.QtyWhole = ko.observable(qty.QtyWhole);
        self.QtyFraction = ko.observable(qty.QtyFraction);

        self.QtyDecimal.extend({
            required: {
                onlyIf: function () { return !self.QtyWhole() && !self.QtyFraction() },
                message: errorMessages.MissingFormInputErrorMessage
            },
            min: {
                message: errorMessages.InvalidDecimalRangeErrorMessage,
                params: 0.001
            },
            max: {
                message: errorMessages.InvalidDecimalRangeErrorMessage,
                params: 9999.99
            },
            validatable: true
        });

        self.QtyWhole.extend({
            required: {
                onlyIf: function () { return !self.QtyDecimal() && !self.QtyFraction() },
                message: errorMessages.MissingFormInputErrorMessage
            },
            min: {
                message: errorMessages.InvalidWholeQtyErrorMessage,
                params: 1
            },
            max: {
                message: errorMessages.InvalidWholeQtyErrorMessage,
                params: 9999
            },
            validatable: true
        });

        self.QtyFraction.extend({
            validation: {
                validator: function (val) {
                    return val ? fractionOptions.includes(val) : true;
                },
                message: errorMessages.InvalidFractionErrorMessage
            },
            validatable: true
        });

        return self;
    };

    function createIngredient(ingredient) {

        let ingredientSelf = this;
        
        ingredientSelf.Name = ko.observable(ingredient.Name ? ingredient.Name : '').extend({
            required: {
                message: errorMessages.RecipeIngredientNameRequiredErrorMessage
            },
            minLength: {
                message: errorMessages.RecipeIngredientNameRangeErrorMessage,
                params: validationConstants.RecipeIngredientNameMinLength
            },
            maxLength: {
                message: errorMessages.RecipeIngredientNameRangeErrorMessage,
                params: validationConstants.RecipeIngredientNameMaxLength
            },
            validatable: true
        });

        ingredientSelf.Qty = ko.observable(new createQtyObservable(ingredient.Qty ?
            {
                QtyDecimal: ingredient.Qty.QtyDecimal,
                QtyWhole: ingredient.Qty.QtyWhole,
                QtyFraction: ingredient.Qty.QtyFraction
            } :
            {
                QtyDecimal: '',
                QtyWhole: '',
                QtyFraction: ''
            }));

        ingredientSelf.MeasureId = ko.observable(ingredient.MeasureId ? ingredient.MeasureId : '').extend({
            required: {
                message: errorMessages.MeasureRequiredErrorMessage
            },
            validatable: true
        });

        ingredientSelf.SpecificationId = ko.observable(ingredient.SpecificationId ? ingredient.StecificationId : '').extend({ validatable: true });

        ingredientSelf.isUsingFractionsForIngredient = ko.computed(function () {

            let qty = ingredientSelf.Qty();
            if (qty) {
                return (!qty.QtyDecimal() && (qty.QtyFraction() || qty.QtyWhole()));
            }
            return false;
        }).extend({ validatable: true });

        return ingredientSelf;
    }

}
