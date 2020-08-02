
function displayToastrMsg(tstrMessage, tstrTitle, tstrType) {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-center",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "15",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };

    switch (tstrType) {
    case "Success":
        toastr.success(tstrMessage, tstrTitle);
        break;
    case "Info":
        toastr.info(tstrMessage, tstrTitle);
        break;
    case "Warning":
        toastr.warning(tstrMessage, tstrTitle);
        break;
    case "Error":
        toastr.error(tstrMessage, tstrTitle);
        break;
    }
}