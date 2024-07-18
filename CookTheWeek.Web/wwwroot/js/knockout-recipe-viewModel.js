//Knockout Viewmodel definition, enabling two-way data binding (client-server side and vise-versa)
function RecipeViewModel(data, errorMessages, qtyFractionOptions, validationConstants) {

    var self = this;

    const fractionOptions = Object.keys(qtyFractionOptions);

    self.Id = ko.observable(data.Id);

    self.Title = ko.observable(data.Title).extend({
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

    self.Description = ko.observable(data.Description).extend({
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

    self.Servings = ko.observable(data.Servings).extend({
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

    self.ImageUrl = ko.observable(data.ImageUrl).extend({
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

    self.CookingTimeMinutes = ko.observable(data.CookingTimeMinutes).extend({
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

    self.RecipeCategoryId = ko.observable(data.RecipeCategoryId).extend({
        required: {
            message: errorMessages.RecipeCategoryIdRequiredErrorMessage
        },
        validatable: true
    });

    function createStep(newStep) {

        let stepSelf = this;

        stepSelf.Description = ko.observable(newStep.Description).extend({
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

        stepSelf.toJSON = function () {
            return {
                Description: stepSelf.Description()
            };
        };

        return stepSelf;
    };

    self.Steps = ko.observableArray(ko.utils.arrayMap(data.Steps, function (step) {
        return createStep(step);
    }));

    self.RecipeIngredients = ko.observableArray(ko.utils.arrayMap(data.RecipeIngredients, function (ingredient) {
        return createIngredient(ingredient);
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
            Qty: { QtyDecimal: '', QtyWhole: '', QtyFraction: '' },
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
    self.toJSON = function () {
        return {
            Id: self.Id(),
            Title: self.Title(),
            Description: self.Description(),
            Servings: self.Servings(),
            ImageUrl: self.ImageUrl(),
            CookingTimeMinutes: self.CookingTimeMinutes(),
            RecipeCategoryId: self.RecipeCategoryId(),
            Steps: self.Steps().map(function (step) {
                return step.toJSON();
            }),
            RecipeIngredients: self.RecipeIngredients().map(function (ingredient) {
                return ingredient.toJSON();
            })
        };
    };
    self.submitForm = function () {
        debugger
        // Validate the entire ViewModel
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

            var jsonData = JSON.stringify(self.toJSON());
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
    };

    return self;

    function createQtyObservable(qty) {

        let qtyObservable = this;
        
        qtyObservable.QtyDecimal = ko.observable(qty.QtyDecimal);
        qtyObservable.QtyWhole = ko.observable(qty.QtyWhole);
        qtyObservable.QtyFraction = ko.observable(qty.QtyFraction);

        qtyObservable.QtyDecimal.extend({
            required: {
                onlyIf: function () { return !qtyObservable.QtyWhole() && !qtyObservable.QtyFraction() },
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

        qtyObservable.QtyWhole.extend({
            required: {
                onlyIf: function () { return !qtyObservable.QtyDecimal() && !qtyObservable.QtyFraction() },
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

        qtyObservable.QtyFraction.extend({
            validation: {
                validator: function (val) {
                    return val ? fractionOptions.includes(val) : true;
                },
                message: errorMessages.InvalidFractionErrorMessage
            },
            validatable: true
        });

        qtyObservable.toJSON = function () {
            return {
                QtyDecimal: qtyObservable.QtyDecimal(),
                QtyWhole: qtyObservable.QtyWhole(),
                QtyFraction: qtyObservable.QtyFraction()
            };
        };

        return qtyObservable;
    };

    function createIngredient(ingredient) {

        let ingredientSelf = this;
        
        ingredientSelf.Name = ko.observable(ingredient.Name).extend({
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

        ingredientSelf.Qty = ko.observable(createQtyObservable(ingredient.Qty));

        ingredientSelf.MeasureId = ko.observable(ingredient.MeasureId).extend({
            required: {
                message: errorMessages.MeasureRequiredErrorMessage
            },
            validatable: true
        });

        ingredientSelf.SpecificationId = ko.observable(ingredient.SpecificationId).extend({ validatable: true });

        let initialSwitchPosition = ingredientSelf.Qty() ? (!ingredientSelf.Qty().QtyDecimal() && (ingredientSelf.Qty().QtyFraction() || ingredientSelf.Qty().QtyWhole())) : false;
        
        ingredientSelf.isUsingFractionsForIngredient = ko.observable(initialSwitchPosition);
        ingredientSelf.switchQtyUnit = function () {
            debugger;
            const isDecimal = !ingredientSelf.isUsingFractionsForIngredient();

            if (isDecimal) {
                let decimalValue = parseFloat(ingredientSelf.Qty().QtyDecimal()) || 0;
                let { whole, fraction } = decimalToFraction(decimalValue);
                
                
                if (whole) {
                    ingredientSelf.Qty().QtyWhole(whole);
                } else {
                    ingredientSelf.Qty().QtyWhole('');
                }                
                ingredientSelf.Qty().QtyFraction(fraction);
                ingredientSelf.Qty().QtyDecimal('');

                ingredientSelf.isUsingFractionsForIngredient(true);
                
            } else {
                
                let wholeValue = parseInt(ingredientSelf.Qty().QtyWhole()) || 0;
                let fractionValue = ingredientSelf.Qty().QtyFraction() || '';
                let decimalValue = fractionToDecimal(wholeValue, fractionValue);


                if (decimalValue) {
                    ingredientSelf.Qty().QtyDecimal(decimalValue.toFixed(3));
                } else {
                    ingredientSelf.Qty().QtyDecimal('');
                }

                ingredientSelf.Qty().QtyWhole(''); 
                ingredientSelf.Qty().QtyFraction(''); 

                ingredientSelf.isUsingFractionsForIngredient(false);
            }

            // Helper function to convert decimal to whole and fraction
            function decimalToFraction(decimal) {
                let whole = Math.floor(decimal);
                let fraction = decimal - whole;

                if (fraction > 0) {
                    let closestFraction = '';
                    let minDifference = Number.MAX_VALUE;

                    for (const [key, value] of Object.entries(qtyFractionOptions)) {
                        const difference = Math.abs(value - fraction);
                        if (difference < minDifference) {
                            minDifference = difference;
                            closestFraction = key;
                        }
                    }

                    fraction = closestFraction;
                }
                else {
                    fraction = '';
                }

                return { whole, fraction };
            }

            // Helper function to convert whole and fraction to decimal
            function fractionToDecimal(whole, fraction) {
                let fractionValue = qtyFractionOptions[fraction] || 0;
                return parseFloat(whole) + fractionValue;
            }  
        };     
        ingredientSelf.toJSON = function () {
            return {
                Name: ingredientSelf.Name(),
                Qty: ingredientSelf.Qty().toJSON(),
                MeasureId: ingredientSelf.MeasureId(),
                SpecificationId: ingredientSelf.SpecificationId()
            };
        };

        return ingredientSelf;
    }   
}



