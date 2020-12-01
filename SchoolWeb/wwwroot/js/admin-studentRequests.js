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

function ShowLoading() {
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
