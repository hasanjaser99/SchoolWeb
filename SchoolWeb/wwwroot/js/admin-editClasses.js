function PopulateTeachers() {
    var courseId = $('#Courses').val();
    $.ajax({
        type: 'POST',
        cache: false,
        url: `/Admin/CoursesAndSections/PopulateTeachers/?courseId=${courseId}`,
        success: function (response) {
            $("#teachersDiv").empty();
            $("#teachersDiv").html(response);
        }
    });

}

function FillTeacherWithValue() {
    var teacherId = $('#teachers').val();
    $('#selectedTeacher').val(teacherId);
}