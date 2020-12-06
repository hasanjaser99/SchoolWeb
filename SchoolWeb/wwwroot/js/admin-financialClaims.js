var dataTable;

$(document).ready(function () {
    loadDataTable("");
});

function loadDataTable(grade) {
    dataTable = $("#tblData").DataTable({
        "bDestroy": true,
        "ajax": {
            "url": `/Admin/Students/GetFinanicalClaimStudents/?grade=${grade}`
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
                "data": "student.id",
                "render": function (data) {
                    return `<span class="tableActionButtons">
                        <a class="editButton btn p-sm-0"
                           style="width:100px;"
                           href="/Admin/Students/FinanicalDetails/?studentId=${data}">
                            <p class="green-font font-weight-bold mt-1">التفاصيل</p>
                        </a>
                    </span>`;
                }, "width": "16%"
            },

            {
                "data": "totalFees",
                "render": function (data) {
                    return `<div style="display:flex; align-items:center; justify-content:center;">
                              <span>دينار</span>
                              &nbsp;
                              <span>${data}</span>
                            </div>`;
                }, "width": "16%"
            },
            { "data": "student.parentPhoneNumber", "wdith": "16%" },
            { "data": "student.section.name", "wdith": "16%" },
            { "data": "student.arabicName", "wdith": "16%" },


        ],
    });
}

function PopulateClaims() {
    var grade = $('#Grades').val();
    loadDataTable(grade);
}
