$(document).ready(function () {
    var companyGuid;

    var guid = sessionStorage.getItem('companyGuid');

    // Mendapatkan data vendor dari server
    $.ajax({
        url: '/Vendor/' + guid,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            companyGuid = response.guid;

            $("#bidangUsaha").val(response.bidangUsaha);
            $("#jenisPerusahaan").val(response.jenisPerusahaan);
        },
        error: function (response) {
            Swal.fire({
                icon: 'error',
                title: 'Kesalahan',
                text: 'Terjadi kesalahan saat mencoba mendapatkan data vendor.'
            });
        }
    });

    $('#updateVendorForm').submit(function (event) {
        event.preventDefault();

        // Pastikan companyGuid telah di-set
        if (!companyGuid) {
            console.error("companyGuid belum di-set.");
            return;
        } else {
            updateVendorDetails(companyGuid);
        }
    });

    // Function untuk mengirim pembaruan data ke server
    function updateVendorDetails(guid) {
        // Ambil data dari elemen-elemen formulir
        console.log("GUID Update :" + guid);

        var bidangUsaha = $('#bidangUsaha').val();
        console.log(bidangUsaha);
        var jenisPerusahaan = $('#jenisPerusahaan').val();
        console.log(jenisPerusahaan);

        // Buat objek FormData dan tambahkan data
        var dataToUpdate = new FormData();
        dataToUpdate.append('guid', guid);
        dataToUpdate.append('bidangUsaha', bidangUsaha);
        dataToUpdate.append('jenisPerusahaan', jenisPerusahaan);

        // Mengirim pembaruan data vendor ke server
        $.ajax({
            url: '/Vendor/updateVendor',
            type: 'PUT',
            data: dataToUpdate,
            contentType: false,
            processData: false,
            success: function (response) {
                if (response.status === "OK") {
                    Swal.fire({
                        icon: 'success',
                        title: 'Pembaruan berhasil',
                        text: 'Data Vendor berhasil diperbarui.'
                    }).then(function () {
                        location.reload();
                    });
                } else if (response.status === "Error") {
                    Swal.fire({
                        icon: 'error',
                        title: 'Gagal Update!',
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
            error: function (response) {
                Swal.fire({
                    icon: 'error',
                    title: 'Pembaruan gagal',
                    text: 'Terjadi kesalahan saat mencoba update data Vendor.'
                });
            }
        });
    }
});
