function PopulateSections() {
    var grade = $('#Grades').val();
    $.ajax({
        type: 'POST',
        cache: false,
        url: `/Admin/Students/PopulateSections/?grade=${grade}&operation=Upsert`,

        data: { grade: grade },
        success: function (response) {
            $("#sectionsDiv").empty();
            $("#sectionsDiv").html(response);
            $('#sectionId').val(null);
        }
    });

}

function FillSectionWithValue() {
    var sectionId = $('#sections').val();
    $('#sectionId').val(sectionId);
}
