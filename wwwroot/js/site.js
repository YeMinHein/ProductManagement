// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    // Add any client-side functionality here
    console.log("Product Management app loaded");

    // Confirm before delete
    $('form[asp-action="Delete"]').submit(function (e) {
        if (!confirm('Are you sure you want to delete this product?')) {
            e.preventDefault();
        }
    });
});

