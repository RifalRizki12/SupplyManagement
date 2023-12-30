$(document).ready(function () {
    $('#tableCompany').DataTable({
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
            url: '/Company/GetCompanyData',
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
            {
                data: 'statusAccount',
                render: function (data, type, row) {
                    var statusText = data;
                    var badgeClass = "bg-success";

                    if (data === "Approved") {
                        statusText = "Approved";
                    } else if (data === "Requested") {
                        statusText = "Requested";
                        badgeClass = "bg-dark";
                    } else if (data === "Rejected") {
                        statusText = "Reject";
                        badgeClass = "bg-danger";
                    } else if (data === "NonAktif") {
                        statusText = "NonAktif";
                        badgeClass = "bg-secondary";
                    }

                    return `
                        <div class="text-center">
                            <span class="badge bg-glow ${badgeClass}">${statusText}</span>
                        </div>`;
                }
            },
            { data: 'phoneNumber' },
            { data: 'address' },
            {
                data: null,
                render: function (data, type, row) {

                    return `<div class="btn-group">
                            <button type="button" class="btn btn-primary waves-effect waves-light btn-sm">Actions</button>
                            <button type="button" class="btn btn-primary dropdown-toggle dropdown-toggle-split waves-effect waves-light" data-bs-toggle="dropdown" aria-expanded="false">
                              <span class="visually-hidden">Toggle Dropdown</span>
                            </button>
                            <ul class="dropdown-menu" style="">
                                 <a class="dropdown-item" data-guid="${data.companyGuid}" data-status="1">Approve</a>
                                 <a class="dropdown-item" data-guid="${data.companyGuid}" data-status="2">Reject</a>
                                 <a class="dropdown-item" data-guid="${data.companyGuid}" data-status="4">Non Active</a>
                            </ul>
                          </div><br/><br/>
                          `;
                }
            },
        ]
    });

    //Action tombol dropdown
    $('#tableCompany').on('click', '.dropdown-item', function () {
        var guid = $(this).data('guid');
        var status = $(this).data('status');
        updateAccountStatus(guid, status);
    });

    // function update Account status
    function updateAccountStatus(guid, status) {
        $.ajax({
            url: '/Account/GuidAccount/' + guid,
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
            success: function (data) {
                if (data) {
                    var dataToUpdate = {
                        guid: data.guid,
                        expiredTime: data.expiredTime,
                        isUsed: data.isUsed,
                        otp: data.otp,
                        password: data.password,
                        roleGuid: data.roleGuid,
                        status: status,
                    };

                    $.ajax({
                        url: '/Account/UpdateAccount/' + guid,
                        type: 'PUT',
                        data: JSON.stringify(dataToUpdate),
                        contentType: 'application/json',

                        success: function (response) {
                            $('#tableClient').DataTable().ajax.reload();
                            Swal.fire({
                                icon: 'success',
                                title: 'Pembaruan berhasil',
                                text: 'Status akun klien telah diubah !',
                                showCloseButton: false,
                                focusConfirm: false,
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                },
                                buttonsStyling: false,
                            });
                        },
                        error: function () {
                            Swal.fire({
                                icon: 'error',
                                title: 'Pembaruan Gagal',
                                text: 'Terjadi kesalahan saat mencoba mengubah status akun klien !',
                                showCloseButton: false,
                                focusConfirm: false,
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                },
                                buttonsStyling: false,
                            });
                        }
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Data tidak ditemukan',
                        text: 'Data akun klien tidak ditemukan !',
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Kesalahan',
                    text: 'Terjadi kesalahan saat mencoba mendapatkan data akun klien.',
                    showCloseButton: false,
                    focusConfirm: false,
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false,
                });
            }
        });
    }

    //GetCompanyAdmin
    $('#tableVendorAdmin').DataTable({
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
            url: '/Company/GetCompanyApproveAdmin',
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
            {
                data: 'statusAccount',
                render: function (data, type, row) {
                    var statusText = data;
                    var badgeClass = "bg-success";

                    if (data === "Approved") {
                        statusText = "Approved";
                    } else if (data === "Requested") {
                        statusText = "Requested";
                        badgeClass = "bg-dark";
                    } else if (data === "Rejected") {
                        statusText = "Reject";
                        badgeClass = "bg-danger";
                    } else if (data === "NonAktif") {
                        statusText = "NonAktif";
                        badgeClass = "bg-secondary";
                    }

                    return `
                        <div class="text-center">
                            <span class="badge bg-glow ${badgeClass}">${statusText}</span>
                        </div>`;
                }
            },
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
            {
                data: null,
                render: function (data, type, row) {

                    return `<div class="btn-group"><br>
                            <button type="button" class="btn btn-primary waves-effect waves-light btn-sm">Actions</button>
                            <button type="button" class="btn btn-primary dropdown-toggle dropdown-toggle-split waves-effect waves-light" data-bs-toggle="dropdown" aria-expanded="false">
                              <span class="visually-hidden">Toggle Dropdown</span>
                            </button>
                            <ul class="dropdown-menu" style="">
                                 <a class="dropdown-item" data-guid="${data.companyGuid}" data-status="2">Approve</a>
                                 <a class="dropdown-item" data-guid="${data.companyGuid}" data-status="4">Reject</a>
                            </ul>
                          </div><br/><br/>
                          `;
                }
            },
        ]
    });
    $('#tableVendorAdmin').on('click', '.dropdown-item', function () {
        var guid = $(this).data('guid');
        var status = $(this).data('status');
        console.log("Guid:" + guid);
        console.log("status:" + status);
        updateVendorStatus(guid, status);
    });
    function updateVendorStatus(guid, status) {
        $.ajax({
            url: '/Vendor/' + guid,
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
            success: function (data) {
                if (data) {
                    var dataToUpdate = {
                        guid: data.guid,
                        bidangUsaha: data.bidangUsaha,
                        jenisPerusahaan: data.jenisPerusahaan,
                        statusVendor: status,
                    };

                    $.ajax({
                        url: '/Vendor/PutVendorByAdmin/' + guid,
                        type: 'PUT',
                        data: JSON.stringify(dataToUpdate),
                        contentType: 'application/json',

                        success: function (response) {
                            $('#tableVendorAdmin').DataTable().ajax.reload();
                            Swal.fire({
                                icon: 'success',
                                title: 'Pembaruan berhasil',
                                text: 'Status akun klien telah diubah !',
                                showCloseButton: false,
                                focusConfirm: false,
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                },
                                buttonsStyling: false,
                            });
                        },
                        error: function () {
                            Swal.fire({
                                icon: 'error',
                                title: 'Pembaruan Gagal',
                                text: 'Terjadi kesalahan saat mencoba mengubah status akun klien !',
                                showCloseButton: false,
                                focusConfirm: false,
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                },
                                buttonsStyling: false,
                            });
                        }
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Data tidak ditemukan',
                        text: 'Data akun klien tidak ditemukan !',
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Kesalahan',
                    text: 'Terjadi kesalahan saat mencoba mendapatkan data akun klien.',
                    showCloseButton: false,
                    focusConfirm: false,
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false,
                });
            }
        });
    }

    //GetCompanyManager
    $('#tableVendorManager').DataTable({
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
            url: '/Company/GetCompanyApproveManager',
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
            {
                data: 'statusAccount',
                render: function (data, type, row) {
                    var statusText = data;
                    var badgeClass = "bg-success";

                    if (data === "Approved") {
                        statusText = "Approved";
                    } else if (data === "Requested") {
                        statusText = "Requested";
                        badgeClass = "bg-dark";
                    } else if (data === "Rejected") {
                        statusText = "Reject";
                        badgeClass = "bg-danger";
                    } else if (data === "NonAktif") {
                        statusText = "NonAktif";
                        badgeClass = "bg-secondary";
                    }

                    return `
                        <div class="text-center">
                            <span class="badge bg-glow ${badgeClass}">${statusText}</span>
                        </div>`;
                }
            },
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
            {
                data: null,
                render: function (data, type, row) {

                    return `<div class="btn-group">
                            <button type="button" class="btn btn-primary waves-effect waves-light btn-sm">Actions</button>
                            <button type="button" class="btn btn-primary dropdown-toggle dropdown-toggle-split waves-effect waves-light" data-bs-toggle="dropdown" aria-expanded="false">
                              <span class="visually-hidden">Toggle Dropdown</span>
                            </button>
                            <ul class="dropdown-menu" style="">
                                 <a class="dropdown-item" data-guid="${data.companyGuid}" data-status="3">Approve</a>
                                 <a class="dropdown-item" data-guid="${data.companyGuid}" data-status="4">Reject</a>
                            </ul>
                          </div><br/><br/>
                          `;
                }
            },
        ]
    });
    $('.dt-buttons').removeClass('dt-buttons');
    $('#tableVendorManager').on('click', '.dropdown-item', function () {
        var guid = $(this).data('guid');
        var status = $(this).data('status');
        updateVendorStatusManager(guid, status);
    });
    function updateVendorStatusManager(guid, status) {
        $.ajax({
            url: '/Vendor/' + guid,
            type: 'GET',
            dataType: 'json',
            dataSrc: 'data',
            success: function (data) {
                if (data) {
                    var dataToUpdate = {
                        guid: data.guid,
                        bidangUsaha: data.bidangUsaha,
                        jenisPerusahaan: data.jenisPerusahaan,
                        statusVendor: status,
                    };

                    $.ajax({
                        url: '/Vendor/PutVendorByAdmin/' + guid,
                        type: 'PUT',
                        data: JSON.stringify(dataToUpdate),
                        contentType: 'application/json',

                        success: function (response) {
                            $('#tableVendorManager').DataTable().ajax.reload();
                            Swal.fire({
                                icon: 'success',
                                title: 'Pembaruan berhasil',
                                text: 'Status akun klien telah diubah !',
                                showCloseButton: false,
                                focusConfirm: false,
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                },
                                buttonsStyling: false,
                            });
                        },
                        error: function () {
                            Swal.fire({
                                icon: 'error',
                                title: 'Pembaruan Gagal',
                                text: 'Terjadi kesalahan saat mencoba mengubah status akun klien !',
                                showCloseButton: false,
                                focusConfirm: false,
                                customClass: {
                                    confirmButton: 'btn btn-primary'
                                },
                                buttonsStyling: false,
                            });
                        }
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Data tidak ditemukan',
                        text: 'Data akun klien tidak ditemukan !',
                        showCloseButton: false,
                        focusConfirm: false,
                        customClass: {
                            confirmButton: 'btn btn-primary'
                        },
                        buttonsStyling: false,
                    });
                }
            },
            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Kesalahan',
                    text: 'Terjadi kesalahan saat mencoba mendapatkan data akun klien.',
                    showCloseButton: false,
                    focusConfirm: false,
                    customClass: {
                        confirmButton: 'btn btn-primary'
                    },
                    buttonsStyling: false,
                });
            }
        });
    }
    
})