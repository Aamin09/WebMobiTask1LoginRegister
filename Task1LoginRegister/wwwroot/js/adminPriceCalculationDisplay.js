﻿$(document).ready(function () {
    function calculatePricing() {
        const costPrice = parseFloat($('#CostPrice').val()) || 0;
        const profitPercentage = parseFloat($('#ProfitPercentage').val()) || 0;
        const sellingPricePercentage = parseFloat($('#SellingPricePercentage').val()) || 0;

        // Calculate Base Price
        const basePrice = costPrice * (1 + (profitPercentage / 100));
        $('#CalculatedBasePrice').val(basePrice.toFixed(2));

        // Calculate Final Selling Price
        const finalSellingPrice = basePrice * (1 - (sellingPricePercentage / 100));
        $('#FinalSellingPrice').val(finalSellingPrice.toFixed(2));

        const finalProfit = finalSellingPrice - costPrice;
        $('#FinalProfit').val(finalProfit.toFixed(2));

        // Validate Pricing
        const isValidPricing = finalSellingPrice > costPrice;
        $('#pricingValidationAlert').toggle(!isValidPricing);
        $('#submitButton').prop('disabled', !isValidPricing);
    }

    $('.pricing-input').on('input', calculatePricing);

    // Initial calculation
    calculatePricing();
});