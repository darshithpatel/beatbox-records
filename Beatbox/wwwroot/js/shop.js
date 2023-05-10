var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url":"/Admin/Shop/GetAll"
        },
        "columns": [
            { "data": "name", "width": "15%"},
            { "data": "streetAddress", "width": "15%"},
            { "data": "city", "width": "15%"},
            { "data": "state", "width": "15%"},
            { "data": "phoneNumber", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                             <div class="w-75 btn-group" role="group">
                            <a style="background-color:#3fb3ec" href="/Admin/Shop/Upsert?id=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i></a>
                            <a onClick=Delete('/Admin/Shop/Delete/${data}') style="background-color:#ed5f69"  class="btn btn-primary mx-3"> <i class="bi bi-trash3"></i></a>
                </div>
                            `
                },
                "width": "15%"
            },
            ]
        });
}


function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
                })
        }
    })
}