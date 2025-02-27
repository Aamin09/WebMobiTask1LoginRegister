$(document).ready(function () {

    $(document).on("click", ".increment", function () {
        let input = $(this).siblings(".quantity");
        let newValue = parseInt(input.val()) + 1;
        input.val(newValue).trigger("change");
    });

    $(document).on("click", ".decrement", function () {
        let input = $(this).siblings(".quantity");
        let newValue = parseInt(input.val()) - 1;
        if (newValue > 0) {
            input.val(newValue).trigger("change");
        }
    });

    $(document).on("change", ".quantity", function () {
        var cartId = $(this).data("cartid");
        var quantity = $(this).val();

        $.ajax({
            url: "/Cart/UpdateQuantity",
            type: "POST",
            data: { cartId: cartId, quantity: quantity },
            success: function (response) {
                if (response.success) {
                    location.reload();
                } else {
                    alert("Error updating quantity!");
                }
            },
            error: function () {
                alert("Something went wrong.");
            }
        });
    });
});