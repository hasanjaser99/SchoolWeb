function DeleteTeacher(teacherId) {
    console.log(teacherId);
    var url = `/Admin/Teachers/Delete/?id=${teacherId}`;
    console.log("asdasd");
    swal({
        title: "هل متأكد من حذف المعلم ؟",
        text: "لن تستطيع اعادة بيانات المعلم اذا تم حذفها",
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
                        $(`#${teacherId}`).remove();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}