// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
jQuery(document).ready(function ($) {
    $(".clickable-row").click(function () {
        window.location = $(this).data("href");
    });
});

function validateForm() {
    var x = document.forms["CommentForm"]["Comment"].value;
    if (x == "" || x == null || x == " ") {
        alert("Comment must be filled out!");
        return false;
    }
}