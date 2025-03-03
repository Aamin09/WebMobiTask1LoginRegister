$(document).on("click", ".dropdown-toggle-status", function (e) {
    e.preventDefault();
    var link = $(this);
    var userId = link.data("id");
    var url = link.data("url"); 
    var userStatus = $(".userstatus");
    $.ajax({
        type: "POST",
        url: url,
        data: { id: userId },
        success: function (data) {
            if (data.isActive) {
                link.removeClass("text-success").addClass("text-danger").html('<i class="fas fa-ban me-2"></i>Deactivate Account');
                userStatus.removeClass("bg-danger").addClass("bg-success").text('Active');
            } else {
                link.removeClass("text-danger").addClass("text-success").html('<i class="fas fa-check-circle me-2"></i>Activate Account');
                userStatus.removeClass("bg-success").addClass("bg-danger").text('Inactive');
            }
        },
        error: function () {
            alert("An error occurred while updating the status.");
        }
    });
});