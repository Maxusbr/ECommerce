jQuery.validator.addMethod('numericlessthan', function (value, element, params) {
    var otherValue = $(params.element).val();

    return isNaN(value) && isNaN(otherValue) || (params.allowequality === 'True' ? parseFloat(value) <= parseFloat(otherValue) : parseFloat(value) < parseFloat(otherValue));
}, '');

jQuery.validator.unobtrusive.adapters.add('numericlessthan', ['other', 'allowequality'], function (options) {
    var prefix = options.element.name.substr(0, options.element.name.lastIndexOf('.') + 1),
    other = options.params.other,
    fullOtherName = appendModelPrefix(other, prefix),
    element = $(options.form).find(':input[id=' + fullOtherName + ']')[0];

    options.rules['numericlessthan'] = { allowequality: options.params.allowequality, element: element };
    if (options.message) {
        options.messages['numericlessthan'] = options.message;
    }
});

jQuery.validator.addMethod('numericgreaterthan', function (value, element, params) {
    var otherValue = $(params.element).val();

    return isNaN(value) && isNaN(otherValue) || (params.allowequality === 'True' ? parseFloat(value) >= parseFloat(otherValue) : parseFloat(value) > parseFloat(otherValue));
}, '');

jQuery.validator.unobtrusive.adapters.add('numericgreaterthan', ['other', 'allowequality'], function (options) {
    var prefix = options.element.name.substr(0, options.element.name.lastIndexOf('.') + 1),
    other = options.params.other,
    fullOtherName = appendModelPrefix(other, prefix),
    element = $(options.form).find(':input[name=' + fullOtherName + ']')[0];

    options.rules['numericgreaterthan'] = { allowequality: options.params.allowequality, element: element };
    if (options.message) {
        options.messages['numericgreaterthan'] = options.message;
    }
});


function appendModelPrefix(value, prefix) {
    if (value.indexOf('*.') === 0) {
    	value = value.replace('*.', prefix);
    	value = value.replace('[', '_');
    	value = value.replace('.', '_');
    	value = value.replace(']', '_');
    }
    return value;
}