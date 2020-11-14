function PopulateSections() {
    var grade = $('#Grades').val();
    $.ajax({
        type: 'POST',
        cache: false,
        url: `/Admin/Students/PopulateSections/?grade=${grade}`,

        data: { grade: grade },
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

        data: { grade: grade },
        success: function (response) {
            $("#table").empty();
            $("#table").html(response);
        }
    });

}

function DeleteCourse(courseId) {
    var url = `/Admin/CoursesAndSections/DeleteCourse/?id=${courseId}`;
    swal({
        title: "هل متأكد من حذف المادة ؟",
        text: "لن تستطيع اعادة بيانات المادة اذا تم حذفها",
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
                        $(`#${courseId}`).remove();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}