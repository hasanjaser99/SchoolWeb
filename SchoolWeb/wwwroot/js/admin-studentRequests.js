var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/Students/GetRequests"
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
                                  onclick=DeleteRequest("/Admin/Students/DeleteRequest/?id=${data}")>
                                   <p class="text-white font-weight-bold mt-1">رفض</p>
                                </a>
                                <a class="acceptStudent editButton btn p-sm-0"
                                    data-backdrop="static"
                                    data-keyboard="false"
                                    data-toggle="modal"
                                    href="#acceptStudentModal"
                                    onclick=showAcceptStudentModal("${data}")>
                                        <p class="green-font font-weight-bold mt-1">قبول</p>
                                </a>
                                </span>`;
                }, "width": "14%"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<span class="tableActionButtons">
                               <a class="editButton btn p-sm-0"
                                  href="/Admin/Students/StudentDetails/?id=${data}&from=Requests">
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




function showAcceptStudentModal(studentId) {
    $('#studentId').val(studentId);
    PopulateGrades(studentId);
};

function PopulateGrades(studentId) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: `/Admin/Students/PopulateGrades/?studentId=${studentId}`,
        success: function (response) {
            $("#gradesDiv").empty();
            $("#gradesDiv").html(response);
            PopulateSections();
        }
    });

}



function PopulateSections() {
    var grade = $('#Grades').val();
    $('#grade').val(grade);
    $.ajax({
        type: 'POST',
        cache: false,
        url: `/Admin/Students/PopulateSections/?grade=${grade}&operation=AcceptRequest`,
        success: function (response) {
            $("#sectionsDiv").empty();
            $("#sectionsDiv").html(response);
            var sectionId = $('#sections').val();
            $('#sectionId').val(sectionId);
        }
    });

}

function FillSectionWithValue() {
    var sectionId = $('#sections').val();
    $('#sectionId').val(sectionId);
}

function acceptDone() {
    $("#acceptDoneButton").submit();

    if ($("#BussFees").val().trim() != "" && $("#Discount").val().trim()) {
        setTimeout(function () {
            $("#modalContentContainer").empty();
            $("#modalContentContainer").html(`<div class="modal-content align-items-center" style = "height:300px;justify-content:center;" >
            <div class="spinner-border text-success" role="status">
                <span class="sr-only">Loading...</span>
            </div>
            <br />
            <h2 style="font-size:1.1rem;">يرجى الإنتظار</h2>
        </div>`);
        }, 3);
    }


}


function DeleteRequest(url) {
    swal({
        title: "هل متأكد من رفض طلب التسجيل ؟",
        text: "لن تستطيع اعادة بيانات الطلب اذا تم حذفها",
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
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}
