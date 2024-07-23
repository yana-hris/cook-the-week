// Knockout external library Recipe Add ViewModel Definition for data-binding
function AddRecipeViewModel(data, serverErrors, errorMessages, qtyFractionOptions, validationConstants) {

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

        for (const propertyName in self.ServerErrors) {

            const errorsArr = self.ServerErrors[propertyName];
            console.log(propertyName, errorsArr.join("\r\n"));

            if (self.ServerErrors.hasOwnProperty(propertyName)) {
                var observable = self;
                var path = propertyName.split(/[.\[\]]+/).filter(Boolean);
                debugger;
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
                        console.log(observable.toString());

                    } else {

                        console.warn(`Path ${path[i]} not found in ViewModel`);
                        observable = null;
                        break;
                    }

                }
                if (observable && ko.isObservable(observable)) {
                    //Manually set observable`s error
                    observable.setError(errorsArr.join("\r\n"));
                }
            }
        }

        self.errors.showAllMessages();
    };

    // Initialize validation group
    self.errors = ko.validation.group(self, { deep: true, observable: true });

    self.submitForm = function () {
        
        const clientSideErrors = self.errors();
        self.errors.showAllMessages();

        //console.log(clientSideErrors);
        //console.log(self);

        // Additional validation for steps and ingredients
        const stepsValid = self.Steps().length > 0;

        const ingredientsValid = self.RecipeIngredients().length > 0;

        if (!stepsValid) {
            toastr.error(errorMessages.StepsRequiredErrorMessage);
        }

        if (!ingredientsValid) {
            toastr.error(errorMessages.IngredientsRequiredErrorMessage);
        }

        if (clientSideErrors.length === 0 && stepsValid && ingredientsValid) {
            
            return true; // Allow form submission
        } else {
            
            self.errors.showAllMessages();
            return false; // Prevent form submission
        }
    };

    return self;

    function createQtyObservable(qty) {

        let qtySelf = this;

        if (qty) {
            qtySelf.QtyDecimal = ko.observable(qty.QtyDecimal);
            qtySelf.QtyWhole = ko.observable(qty.QtyWhole);
            qtySelf.QtyFraction = ko.observable(qty.QtyFraction);
        } else {
            qtySelf.QtyDecimal = ko.observable('');
            qtySelf.QtyWhole = ko.observable('');
            qtySelf.QtyFraction = ko.observable('');
        }

        qtySelf.QtyDecimal.extend({
            required: {
                onlyIf: function () { return (!qtySelf.QtyWhole() && !qtySelf.QtyFraction()) },
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

        qtySelf.QtyWhole.extend({
            required: {
                onlyIf: function () {
                    return (!qtySelf.QtyDecimal() || isNaN(qtySelf.QtyDecimal())) && !qtySelf.QtyFraction();
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

        qtySelf.QtyFraction.extend({
            validation: {
                validator: function (val) {
                    return val ? fractionOptions.includes(val) : true;
                },
                message: errorMessages.InvalidFractionErrorMessage
            },
            validatable: true
        });

        return qtySelf;
    }

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
            validatable: true,

        });

        ingredientSelf.Qty = ko.observable(new createQtyObservable(ingredient.Qty ? ingredient.Qty : { QtyDecimal: '', QtyFraction: '', QtyWhole: '' }))
            .extend({ validatable: true })
            ;

        ingredientSelf.MeasureId = ko.observable(ingredient.MeasureId ? ingredient.MeasureId : '').extend({
            required: {
                message: errorMessages.MeasureRequiredErrorMessage
            },
            validatable: true
        });

        ingredientSelf.SpecificationId = ko.observable(ingredient.SpecificationId ? ingredient.SpecificationId : '')
            .extend({ validatable: true });

        ingredientSelf.isUsingFractionsForIngredient = ko.computed(function () {

            let qty = ingredientSelf.Qty();
            if (qty) {
                return (!qty.QtyDecimal() && (qty.QtyFraction() || qty.QtyWhole()));
            }
            return false;
        }).extend({ validatable: true });

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
            }
        }).extend({ validatable: true });

        return stepSelf;
    }
}
        