$(document).ready(function () {
    $('#registerForm').on('submit', function (e) {
        e.preventDefault();

        var profilePictureFile = $('#profilePictureInput').prop('files')[0];

        // Validasi gambar profil
        if (profilePictureFile && !isValidImageFormat(profilePictureFile.name)) {
            Swal.fire({
                title: 'Format Gambar Profil Salah!',
                icon: 'info',
                html: 'Hanya format JPEG, JPG, PNG, dan GIF yang diperbolehkan.',
                showCloseButton: false,
                focusConfirm: false,
                customClass: {
                    confirmButton: 'btn btn-primary'
                },
                buttonsStyling: false,
            });
            return; // Berhenti jika format gambar salah
        }
        
        var formData = new FormData();

        // Tambahkan data teks ke formData
        formData.append('NameCompany', $('#name').val());
        formData.append('Email', $('#emailInput').val());
        formData.append('PhoneNumber', $('#phoneNumberInput').val());
        formData.append('AddressCompany', $('#addressCompanyInput').val());
        formData.append('Password', $('#passwordInput').val());
        formData.append('ConfirmPassword', $('#confirmPasswordInput').val());
        formData.append('FotoCompany', profilePictureFile);

        // Kirim data dengan metode AJAX ke server
        $.ajax({
            url: '/Account/RegisterCompany',
            type: 'POST',
            data: formData,
            processData: false,  // penting, jangan proses data
            contentType: false,  // penting, biarkan jQuery mengatur ini
            success: function (response) {

                if (response.status == "OK") {
                    Swal.fire({
                        icon: 'success',
                        title: 'Register Berhasil!',
                        text: 'Anda akan diarahkan ke halaman yang dituju !',
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    }).then(function () {
                        window.location.href = '/Account/Logins';
                    });
                }
                else if (response.status === "Error") {
                    Swal.fire({
                        icon: 'error',
                        title: 'Gagal Register !',
                        text: response.message.error || response.message.message,
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    });
                }
            },
            error: function (xhr, status, error) {
                Swal.fire({
                    icon: 'error',
                    title: 'Terjadi Kesalahan!',
                    text: 'Koneksi gagal atau server mengalami masalah. Silakan coba lagi nanti.',
                });
                console.error('Error:', error);
            }
        });
    });

    function isValidImageFormat(fileName) {
        var allowedExtensions = ["jpg", "jpeg", "png", "gif"];
        var fileExtension = fileName.split('.').pop().toLowerCase();

        return allowedExtensions.includes(fileExtension);
    }
});
