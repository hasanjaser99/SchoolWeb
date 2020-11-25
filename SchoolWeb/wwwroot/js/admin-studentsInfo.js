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
    $.ajax({
        type: 'POST',
        cache: false,
        url: `/Admin/Students/PopulatestudentsTable/?grade=${grade}&section=${section}`,
        success: function (response) {
            $("#table").empty();
            $("#table").html(response);
        }
    });

}

function DeleteStudent(studentId) {
    var url = `/Admin/Students/DeleteStudent/?id=${studentId}`;
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
                        toastr.success(data.message);
                        $(`#${studentId}`).remove();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}