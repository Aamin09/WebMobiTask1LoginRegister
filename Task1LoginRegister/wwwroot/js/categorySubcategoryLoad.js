$(document).ready(function () {
    $("#CategoryId").change(function () {
        var categoryId = $(this).val();
        if (categoryId) {
            $.getJSON("/Admin/GstTaxes/GetSubcategories", { categoryId: categoryId }, function (data) {
                var subcategoryDropdown = $("#SubcategoryId");
                subcategoryDropdown.empty();
                subcategoryDropdown.append('<option value="">Select Subcategory</option>');
                $.each(data, function (index, item) {
                    subcategoryDropdown.append('<option value="' + item.subcategoryId + '">' + item.name + '</option>');
                });
            });
        } else {
            $("#SubcategoryId").empty().append('<option value="">Select Subcategory</option>');
        }
    });
});