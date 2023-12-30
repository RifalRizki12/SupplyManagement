//Login
$(document).ready(function () {
    $('#loginButton').click(function () {
        var email = $('#emailInput').val();
        var password = $('#passwordInput').val();

        var data = {
            Email: email,
            Password: password
        };

        $.ajax({
            url: '/Account/logins',
            type: 'POST',
            dataType: 'json',
            data: JSON.stringify(data),
            contentType: 'application/json',
            success: function (response) {
                if (response.redirectTo) {
                    // SweetAlert for login success
                    Swal.fire({
                        icon: 'success',
                        title: 'Login Berhasil',
                        text: 'Anda akan diarahkan ke halaman yang dituju.',
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    }).then(function () {
                        window.location.href = response.redirectTo;
                    });
                } else if (response.status === "Error") {
                    Swal.fire({
                        icon: 'error',
                        title: 'Login Gagal!',
                        text: response.message.error || response.message.message || response.message,
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    });
                }
            },
            error: function (response) {
                Swal.fire({
                    icon: 'error',
                    title: 'Error !!!',
                    text: 'Gagal Menghubungkan !!!',
                    showCloseButton: false,
                    focusConfirm: false,
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false,
                });
            }
        });
    });
});