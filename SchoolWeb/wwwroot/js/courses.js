function PopulateTable() {
    var grade = $('#Grades').val();
    console.log(grade);
    $.ajax({
        type: 'POST',
        cache: false,
        url: `/Admin/CoursesAndSections/PopulateCoursesTable/?grade=${grade}`,

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