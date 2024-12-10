import { activateTabWithError } from './site.js';
// Knockout external library Recipe Add ViewModel Definition for data-binding
export function AddRecipeViewModel(data, serverErrors, errorMessages, qtyFractionOptions, validationConstants) {

    var self = this;

    const fractionOptions = Object.keys(qtyFractionOptions);

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


    self.Description = ko.observable(data.Description === undefined || data.Description === null || data.Description === '' ? null : data.Description).extend({
        required: false,
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

    self.AvailableTags = ko.observableArray(ko.utils.arrayMap(data.AvailableTags || [], function (tag) {
        return {
            Id: tag.Id,
            Name: tag.Name
        };
    }));

    self.SelectedTagIds = ko.observableArray(data.SelectedTagIds || []).extend({
        validatable: true
    });

    self.DifficultyLevels = ko.observableArray(ko.utils.arrayMap(data.DifficultyLevels || [], function (level) {
        return {
            Id: level.Id,
            Name: level.Name
        };
    })
    );

    self.DifficultyLevelId = ko.observable(data.DifficultyLevelId || null).extend({
        validatable: true
    });

    self.RecipeCategoryId = ko.observable(data.RecipeCategoryId ? data.RecipeCategoryId : '').extend({
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

    self.addStep = function () {
        let newStep = new createStep(
            { Descrption: '' });

        self.Steps.push(newStep);
    };

    self.removeStep = function (step) {
        if (self.Steps().length > 1) {
            self.Steps.remove(step);
        } else {
            toastr.error(errorMessages.StepsRequiredErrorMessage);
        }
        self.errors();
    };

    self.addIngredient = function () {
        let newIngredient = new createIngredient({
            IngredientId: '',
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

    // Initialize the server errors in the view model
    self.ServerErrors = serverErrors;

    // Function to apply server-side errors to the client-side model
    self.applyServerErrors = function () {
        debugger;
        for (const propertyName in self.ServerErrors) {

            const errorsArr = self.ServerErrors[propertyName];
            console.log(propertyName, errorsArr.join("\r\n"));

            if (self.ServerErrors.hasOwnProperty(propertyName)) {
                var observable = self;
                var path = propertyName.split(/[.\[\]]+/).filter(Boolean);
                
                for (var i = 0; i < path.length; i++) {

                    // retrieve the value of the observable wanted if it exists
                    if (ko.isObservable(observable)) {
                        observable = observable();
                    }

                    // check if the value is a number and the observable - an array
                    if (!isNaN(parseInt(path[i], 10)) && Array.isArray(observable)) {

                        observable = observable[parseInt(path[i], 10)];

                    } else if (observable[path[i]]) {

                        observable = observable[path[i]];
                        console.log(observable);

                    } else {

                        console.warn(`Path ${path[i]} not found in ViewModel`);
                        observable = null;
                        break;
                    }

                }
                if (observable && ko.isObservable(observable)) {
                    //Manually set observable`s error
                    observable.setError(errorsArr[0]);
                }
            }
        }

        self.errors.showAllMessages();
        activateTabWithError("#add-recipe");
    };

    // Initialize validation group
    self.errors = ko.validation.group(self, { deep: true, live: true });

    self.submitForm = function () {
        debugger;
        const clientSideErrors = self.errors();
        self.errors.showAllMessages();
       

        console.log(clientSideErrors);
        console.log(self);

        // Additional validation for steps and ingredients
        const stepsValid = self.Steps().length > 0;
        const ingredientsValid = self.RecipeIngredients().length > 0;

        if (!stepsValid) {
            toastr.error(errorMessages.StepsRequiredErrorMessage);
        }

        if (!ingredientsValid) {
            toastr.error(errorMessages.IngredientsRequiredErrorMessage);
        }

        // If there are errors, find the first one and activate the corresponding tab
        if (clientSideErrors.length > 0 || !stepsValid || !ingredientsValid) {
            activateTabWithError("#add-recipe");
            return false; // Prevent form submission
        } else {
            
            // Update the history state without reloading the page
            history.replaceState("", "", "/Recipe/All");
            return true; // Allow form submission
        }
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
                onlyIf: function () {
                    return !qtyObservable.QtyDecimal() && !qtyObservable.QtyFraction();
                },
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
    }

    function createIngredient(ingredient) {

        let ingredientSelf = this;
        
        ingredientSelf.IngredientId = ko.observable(ingredient.IngredientId ? ingredient.IngredientId : '').extend({
            required: {
                message: errorMessages.RecipeIngredientIdRequiredErrorMessage
            },
            validatable: true
        });

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
            validatable: true,

        });

        ingredientSelf.Qty = ko.observable(new createQtyObservable(ingredient.Qty ? ingredient.Qty : { QtyDecimal: '', QtyFraction: '', QtyWhole: '' }));

        ingredientSelf.MeasureId = ko.observable(ingredient.MeasureId ? ingredient.MeasureId : '').extend({
            required: {
                message: errorMessages.MeasureRequiredErrorMessage
            },
            validatable: true
        });

        ingredientSelf.SpecificationId = ko.observable(ingredient.SpecificationId ? ingredient.SpecificationId : '')
            .extend({ validatable: true });

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
            validatable: true
        });

        return stepSelf;
    }
}
        