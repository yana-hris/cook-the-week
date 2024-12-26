$(document).ready(function () {
    
    adjustToastrPosition();

    $(window).resize(function () {
        adjustToastrPosition();
    });

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": true,
        "progressBar": true,
        "preventDuplicates": false,
        "onclick": null,
        "timeOut": "5000", 
        "extendedTimeOut": "3000", 
        "showDuration": "300", 
        "hideDuration": "500", 
        "showEasing": "swing", 
        "hideEasing": "linear",
        "showMethod": "fadeIn", 
        "hideMethod": "fadeOut" 
    };

    // Adjust toastr message position for smaller screens
    function adjustToastrPosition() {
        if (window.innerWidth <= 768) { 
            toastr.options.positionClass = "toast-top-center";
        } else {
            toastr.options.positionClass = "toast-bottom-right";
        }
    }

    
});
