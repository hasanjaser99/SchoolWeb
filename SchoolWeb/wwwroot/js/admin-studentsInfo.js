var dataTable;

$(document).ready(function () {
    loadDataTable("", "");
});

function loadDataTable(grade, section) {
    dataTable = $("#tblData").DataTable({
        "bDestroy": true,
        "ajax": {
            "url": `/Admin/Students/GetStudents/?grade=${grade}&section=${section}`
        },
        "iDisplayLength": 10,
        "language": {
            "sProcessing": "جارٍ التحميل...",
            "sLengthMenu": "أظهر _MENU_ مدخلات",
            "sZeroRecords": "لم يعثر على أية سجلات",
            "sInfo": "إظهار _START_ إلى _END_ من أصل _TOTAL_ مدخل",
            "sInfoEmpty": "يعرض 0 إلى 0 من أصل 0 سجل",
            "sInfoFiltered": "(منتقاة من مجموع _MAX_ مُدخل)",
            "sInfoPostFix": "",
            "sSearch": "ابحث:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "الأول",
                "sPrevious": "السابق",
                "sNext": "التالي",
                "sLast": "الأخير"
            }
        },
        "columns": [
            {
                "data": "id",
                "render": function (data) {
                    return `<span class="tableActionButtons">
                        <a class="deleteButton btn p-sm-0"
                            onclick=DeleteStudent("/Admin/Students/DeleteStudent/?id=${data}")>
                            <p class="text-white font-weight-bold mt-1">حذف</p>
                        </a>
                        <a class="editButton btn p-sm-0"
                           href="/Admin/Students/UpsertStudent/?id=${data}">
                            <p class="green-font font-weight-bold mt-1">تعديل</p>
                        </a>
                    </span>`;
                }, "width": "14%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<span class="tableActionButtons">
                        <a class="editButton btn p-sm-0"
                           href="/Admin/Students/StudentDetails/?id=${data}">
                            <p class="green-font font-weight-bold mt-1">المزيد</p>
                        </a>
                    </span>`;
                }, "width": "14%"
            },
            { "data": "address", "wdith": "14%" },
            { "data": "parentPhoneNumber", "wdith": "14%" },
            {
                "data": "grade",
                "render": function (data) {
                    return GetGrade(data);
                }, "width": "14%"
            },
            { "data": "englishName", "wdith": "14%" },
            { "data": "arabicName", "wdith": "14%" },


        ],
    });
}


function PopulateSections() {
    var grade = $('#Grades').val();
    $.ajax({
        type: 'POST',
        cache: false,
        url: `/Admin/Students/PopulateSections/?grade=${grade}`,
        success: function (response) {
            $("#sectionsDiv").empty();
            $("#sectionsDiv").html(response);
        }
    });

}

function PopulateTable() {
    var section = $('#sections').val();
    var grade = $('#Grades').val();
    loadDataTable(grade, section);
}

function DeleteStudent(url) {
    swal({
        title: "هل متأكد من حذف الطالب ؟",
        text: "لن تستطيع اعادة بيانات الطالب اذا تم حذفها",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}