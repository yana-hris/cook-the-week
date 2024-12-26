$(document).ready(function () {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "positionClass": "toast-bottom-right", 
        "preventDuplicates": false,
        "onclick": null,
        "timeOut": "5000", // Duration before it disappears
        "extendedTimeOut": "3000", // Duration after hovering
        "showDuration": "300", // Smooth show animation (in milliseconds)
        "hideDuration": "500", // Smooth hide animation (in milliseconds)
        "showEasing": "swing", 
        "hideEasing": "linear",
        "showMethod": "fadeIn", 
        "hideMethod": "fadeOut" 
    };
    
});
