function PopulateMarks() {
    // get data 
    var semester = $('#Semesters').val();
    var grade = $('#grade').val();

    // check if user select poth 
    if (grade == "none" || semester == "none") {
        $("#marksTable").remove();
        return;
    }
     
    // show spinner
    $("#marksTable").remove();
    $("#tableContainer").append('<div class="spinner-border text-success" id="spinner" role="status">'+
        '<span class="sr-only">Loading...</span> </div>');
    

    $.ajax({
        type: 'POST',
        cache: false,
        url: `/StudentPortal/Home/PopulateMarksTable/?grade=${grade}&semester=${semester}`,
        success: function (response) {
            //hide spinner and show marks table
            $("#spinner").remove();
            $("#tableContainer").append(response);
            
        }

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