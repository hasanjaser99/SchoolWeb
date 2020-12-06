



var dataTable;

$(document).ready(function () {
    PopulateMarks();
});


function PopulateMarks() {

    // get data 
    var grade = $('#grades').val();
    var courseId = $('#courses').val();
    var sectionId = $('#sections').val();
    var semester = $('#semester').val();



    
    dataTable = $("#tblData").DataTable({
        "bDestroy": true,
        "ajax": {
            "url": `/TeacherPortal/Home/PopulateMarksTable/?courseId=${courseId}&grade=${grade}&sectionId=${sectionId}&semester=${semester}`
        },
        success: function (res) {
            console.log(res);
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
                        <a class="editButton btn p-sm-0"
                           href="/Admin/Students/UpsertMark/?id=${data}">
                            <p class="green-font font-weight-bold mt-1">تعديل</p>
                        </a>
                    </span>`;
                }, "width": "16.6%"
            },
            { "data": "finalMark", "wdith": "16.6%" },
            { "data": "assignmentsMark", "wdith": "16.6%" },
            { "data": "secondMark", "wdith": "16.6%" },
            { "data": "firstMark", "wdith": "16.6%" },
            { "data": "student.arabicName", "wdith": "16.6%" },


        ],
    });

}





function PopulateGrades() {
    // get data 
    var courseId = $('#courses').val();
    
    // disable grades List
    $("#grades").attr("disabled","true");
    
    $.ajax({
        type: 'POST',
        cache: false,
        url: `/TeacherPortal/Home/PopulateGradesList/?courseId=${courseId}`,
        success: function (response) {

            // replace grades List
            $("#grades").remove();
            $("#gradesContainer").append(response);

        }

    });

}


function PopulateSections() {
    // get data 
    var grade = $('#grades').val();
    var courseId = $('#courses').val();

    // disable grades List
    $("#sections").attr("disabled", "true");
    $.ajax({
        type: 'POST',
        cache: false,
        url: `/TeacherPortal/Home/PopulateSectionsList/?grade=${grade}&courseId=${courseId}`,
        success: function (response) {
            
            //hide spinner and show marks table
            $("#sections").remove();
            $("#sectionsContainer").append(response);

        }

    });

}