//only image and video
$("#FilUploader").change(function (q) {
    var fileExtension = ['jpeg', 'jpg', 'png'];
    if ($.inArray($(this).val().split('.').pop().toLowerCase(), fileExtension) == -1) {
        alert("Only formats are allowed : " + fileExtension.join(','));
        $("#FilUploader").val("");
        return false;
    }
});

