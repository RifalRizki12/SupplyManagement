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

    //GET DATA ALL VENDOR
    $('#tableVendor').DataTable({
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'copy',
                text: 'Copy',
                className: 'btn btn-dark btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                }
            },
            {
                extend: 'excel',
                text: 'Export to Excel',
                className: 'btn btn-success btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                },
            },
            {
                extend: 'pdf',
                text: 'Export to PDF',
                className: 'btn btn-danger btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                },
                customize: function (doc) {
                    doc.pageOrientation = 'landscape';
                    doc.pageSize = 'A3';
                }
            },
            {
                extend: 'print',
                text: 'Print',
                className: 'btn btn-info btn-sm',
                exportOptions: {
                    columns: ':visible:not(.no-export)'
                },
                customize: function (win) {
                    $(win.document.body).css('font-size', '12px');
                    $(win.document.body).find('table').addClass('compact').css('font-size', 'inherit');
                }
            },
            {
                extend: 'colvis',
                className: 'btn btn-primary btn-sm',
                postfixButtons: ['colvisRestore']
            }
        ],
        scrollX: true,
        columnDefs: [
            {
                visible: false
            }
        ],

        ajax: {
            url: '/Company/GetAllVendor',
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
        },
        columns: [
            {
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            {
                data: 'foto',
                render: function (data, type, row) {
                    if (type === 'display' && data) {
                        const baseURL = "https://localhost:7013/"; // Gantilah URL dasar sesuai dengan kebutuhan Anda
                        const photoURL = `${baseURL}FotoCompany/${data}`; // Gabungkan baseURL dengan path gambar
                        return `<img src="${photoURL}" alt="Company Photo" style="max-width: 100px; max-height: 100px;">`;
                    }
                    return 'N/A'; // Pesan jika URL gambar tidak tersedia
                }
            },
            { data: 'nameCompany' },
            { data: 'email' },
            { data: 'phoneNumber' },
            { data: 'address' },
            { data: 'bidangUsaha' },
            { data: 'jenisPerusahaan' },
            {
                data: 'statusVendor',
                render: function (data, type, row) {
                    var badgeClass = "bg-warning";

                    return `
                        <div class="text-center">
                            <span class="badge bg-glow ${badgeClass}">${data}</span>
                        </div>`;
                }
            },
        ]
    });

    $('.dt-buttons').removeClass('dt-buttons');
});
