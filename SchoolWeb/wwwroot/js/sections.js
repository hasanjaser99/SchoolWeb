function PopulateTable() {
    var grade = $('#Grades').val();
    $.ajax({
        type: 'POST',
        cache: false,
        url: `/Admin/CoursesAndSections/PopulateSectionsTable/?grade=${grade}`,

        data: { grade: grade },
        success: function (response) {
            $("#table").empty();
            $("#table").html(response);
        }
    });

}

function DeleteSection(sectionId) {
    var url = `/Admin/CoursesAndSections/DeleteSection/?id=${sectionId}`;
    swal({
        title: "هل متأكد من حذف الشعبة ؟",
        text: "لن تستطيع اعادة بيانات الشعبة اذا تم حذفها",
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
                        $(`#${sectionId}`).remove();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}