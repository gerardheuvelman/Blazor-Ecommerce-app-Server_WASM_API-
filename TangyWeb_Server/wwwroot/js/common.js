window.ShowToastr = (type, message) => {
    if (type === "success") {
        toastr.success(message, 'Operation Successful!', { timeOut: 5000 });
    }
    else if (type === "error") {
        toastr.error(message, 'Operation Failed.', { timeOut: 5000 });
    }
} 

window.SwalFire = (type, message) => {
    if (type === "success") {
        Swal.fire(message, message, 'success');
    }
    else if (type === "error") {
        Swal.fire(message, message, 'error');
    }
} 