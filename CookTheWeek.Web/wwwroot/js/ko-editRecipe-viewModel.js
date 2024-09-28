import { activateTabWithError } from './site.js';
//Knockout Edit Recipe Viewmodel definition, enabling two-way data binding (client-server side and vise-versa)
export function EditRecipeViewModel(data, errorMessages, qtyFractionOptions, validationConstants, returnUrl) {

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
    self.Steps = ko.observableArray(ko.utils.arrayMap(data.Steps, function (step) {
        return new createStep(step);
    }));

    self.RecipeIngredients = ko.observableArray(ko.utils.arrayMap(data.RecipeIngredients, function (ingredient) {
        return new createIngredient(ingredient);
    }));

    self.StepsErrors = ko.validation.group(self.Steps, {
        deep: true,
        live: true
    });

    self.RecipeIngredientErrors = ko.validation.group(self.RecipeIngredients, {
        deep: true,
        live: true
    });

    self.addStep = function () {
        
        let newStep = new createStep({
            Description: ''
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

    self.errors = ko.validation.group(self, { deep: true, live: true });
    self.toJSON = function () {
        return {
            Id: self.Id(),
            Title: self.Title(),
            Description: self.Description(),
            Servings: self.Servings(),
            ImageUrl: self.ImageUrl(),
            CookingTimeMinutes: self.CookingTimeMinutes(),
            RecipeCategoryId: self.RecipeCategoryId(),
            Steps: ko.utils.arrayMap(self.Steps(), function (step) {
                return {
                    Description: step.Description()
                };
            }),
            RecipeIngredients: ko.utils.arrayMap(self.RecipeIngredients(), function (ingredient) {
                return {
                    Name: ingredient.Name(),
                    Qty: {
                        QtyDecimal: ingredient.Qty().QtyDecimal(),
                        QtyWhole: ingredient.Qty().QtyWhole(),
                        QtyFraction: ingredient.Qty().QtyFraction()
                    },
                    MeasureId: ingredient.MeasureId(),
                    SpecificationId: ingredient.SpecificationId()
                };
            })
        };
    };
    self.submitForm = function () {
        
        var isValid = true;

        if (self.StepsErrors().length !== 0) {
            self.StepsErrors.showAllMessages();
            isValid = false;
        }

        if (!self.Steps.isValid()) {
            self.Steps.notifySubscribers();
            isValid = false;
        }


        if (self.RecipeIngredientErrors().length !== 0) {
            self.RecipeIngredientErrors.showAllMessages();
            isValid = false;
        }

        if (!self.RecipeIngredients.isValid()) {
            self.RecipeIngredients.notifySubscribers();
            isValid = false;
        }
        // Validate the entire ViewModel
       
        const otherModelErrors = self.errors();

        // Additional validation for steps and ingredients
        const stepsValid = self.Steps().length > 0;
        const ingredientsValid = self.RecipeIngredients().length > 0;

        if (!stepsValid) {
            toastr.error(errorMessages.StepRequiredErrorMessage);
        }

        if (!ingredientsValid) {
            toastr.error(errorMessages.IngredientsRequiredErrorMessage);
        }

        if (otherModelErrors.length !== 0 || !stepsValid || !ingredientsValid) {
            isValid = false;
        }

        if (isValid) {

            var jsonData = JSON.stringify(self.toJSON());
            debugger;
            console.log('Submitting data:', jsonData);
            
            $.ajax({
                url: '/Recipe/Edit?returnUrl=' + encodeURIComponent(returnUrl), 
                type: 'POST',
                contentType: 'application/json',
                data: jsonData, 
                success: function (response) {
                    if (response.success && response.redirectUrl) { 
                        setTimeout(function () {
                            toastr.success('Recipe edited successfully!');
                        }, 2000);
                        // Replace the current history state with the return URL
                        history.replaceState(null, null, returnUrl);
                        window.location.href = response.redirectUrl;
                    } 
                },
                error: (response) => {
                    if (response.status === 400 && response.responseJSON && !response.responseJSON.success) {
                        // Display server-side validation errors
                        if (response.responseJSON.errors) {
                            processErrors(response.responseJSON.errors);
                        }

                        // Handle redirection if provided in case of 400 errors
                        if (response.responseJSON.redirectUrl) {
                            window.location.href = response.responseJSON.redirectUrl;
                        }
                    } else if (response.status === 500) {
                        toastr.error('An internal server error occurred. Please try again later.');
                        window.location.href = '/Home/InternalServerError';
                    } else {
                        console.error('Error occurred while submitting form:', response.statusText);
                        toastr.error('Error occurred while submitting form. Please try again later.');
                    }
                }
            });
        } else {
            self.errors.showAllMessages();
            activateTabWithError("#edit-recipe");
        }

        return false; // Prevent the default form submission
    };
    
    return self;

    // Helper function for processing ajax JSON errors in case of response BadRequest and Errors dictionary sent in json format
    function processErrors(errors) {
        for (var key in errors) {
            if (errors.hasOwnProperty(key)) {
                var errorMessages = errors[key].errors;
                var firstErrorMessage = errorMessages[0].errorMessage;

                if (errorMessages && errorMessages.length > 0) {
                    // Convert property name to match the Knockout view model's format
                    const propertyName = key.charAt(0).toUpperCase() + key.slice(1);

                    var observable = this; // Adjust the context if needed
                    var path = propertyName.split(/[.\[\]]+/).filter(Boolean);

                    // Traverse the path to the correct observable
                    for (var i = 0; i < path.length; i++) {
                        if (ko.isObservable(observable)) {
                            observable = observable();
                        }
                        if (!isNaN(parseInt(path[i], 10)) && Array.isArray(observable)) {
                            observable = observable[parseInt(path[i], 10)];
                        } else if (observable[path[i]]) {
                            observable = observable[path[i]];
                        } else {
                            console.warn(`Path ${path[i]} not found in ViewModel`);
                            observable = null;
                            break;
                        }
                    }

                    // Set the error on the observable
                    if (observable && ko.isObservable(observable)) {
                        observable.setError(firstErrorMessage);
                    }
                }
            }
        }
        this.errors.showAllMessages(); // Display all validation messages
    }

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

        ingredientSelf.Qty = ko.observable(new createQtyObservable(ingredient.Qty));

        ingredientSelf.MeasureId = ko.observable(ingredient.MeasureId).extend({
            required: {
                message: errorMessages.MeasureRequiredErrorMessage
            },
            validatable: true
        });

        ingredientSelf.SpecificationId = ko.observable(ingredient.SpecificationId).extend({ validatable: true });

        let switchChecked = Boolean(ingredientSelf.Qty() ? (!ingredientSelf.Qty().QtyDecimal() && (ingredientSelf.Qty().QtyFraction() || ingredientSelf.Qty().QtyWhole()) ? true : false) : false);
        
        ingredientSelf.isUsingFractionsForIngredient = ko.observable(switchChecked);

        ingredientSelf.isChecked = ko.observable(switchChecked);

        ingredientSelf.showFractionsDiv = ko.observable(switchChecked);
        ingredientSelf.switchQtyUnit = function () {

            const isFractional = ingredientSelf.isUsingFractionsForIngredient();

            if (!isFractional) {
                let decimalValue = parseFloat(ingredientSelf.Qty().QtyDecimal()) || 0;
                let { whole, fraction } = decimalToFraction(decimalValue);

                ingredientSelf.Qty().QtyDecimal('');
                ingredientSelf.showFractionsDiv(true);
                
                if (whole) {
                    ingredientSelf.Qty().QtyWhole(whole);
                } else {
                    ingredientSelf.Qty().QtyWhole('');
                }                
                ingredientSelf.Qty().QtyFraction(fraction);
                
            } else {
                
                let wholeValue = parseInt(ingredientSelf.Qty().QtyWhole()) || 0;
                let fractionValue = ingredientSelf.Qty().QtyFraction() || '';
                let decimalValue = fractionToDecimal(wholeValue, fractionValue);

                ingredientSelf.Qty().QtyWhole('');
                ingredientSelf.Qty().QtyFraction('');
                ingredientSelf.showFractionsDiv(false);

                if (decimalValue) {
                    ingredientSelf.Qty().QtyDecimal(decimalValue.toFixed(3));
                } else {
                    ingredientSelf.Qty().QtyDecimal('');
                }
                
            }
            ingredientSelf.isUsingFractionsForIngredient(!isFractional);
            return true;

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
        
        return ingredientSelf;
    }   

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
                Description: stepSelf.Description() || null
            };
        };

        return stepSelf;
       
    };
    
}



