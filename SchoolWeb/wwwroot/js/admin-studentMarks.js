function PopulateMarks() {
    var semester = $('#Semesters').val();
    var studentId = $('#studentId').val();
    $.ajax({
        type: 'POST',
        cache: false,
        url: `/Admin/Students/PopulateMarksTable/?semester=${semester}&studentId=${studentId}`,
        success: function (response) {
            $("#marksTable").empty();
            $("#marksTable").html(response);
        }
    });

}
