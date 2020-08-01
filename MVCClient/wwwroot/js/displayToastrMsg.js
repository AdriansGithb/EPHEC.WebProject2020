
function displayToastrMsg(tstrMessage,tstrTitle,tstrType) {
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