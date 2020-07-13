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


function toggleIcon(e) {
    $(e.target)
        .prev('.card-header')
        .find(".more-less")
        .toggleClass('fa-plus-circle fa-minus-circle');
    }
$('.panel-group').on('hidden.bs.collapse', toggleIcon);
$('.panel-group').on('shown.bs.collapse', toggleIcon);


$(document).ready(function () {
    $('#dt-filter-select').dataTable({
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                var select = $('<select  class="browser-default custom-select form-control-sm"><option value="" selected>Search</option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );

                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });
                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        }
    });
});

$(document).ready(function () {
    $('#dt-filter-select2').dataTable({
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                var select = $('<select  class="browser-default custom-select form-control-sm"><option value="" selected>Search</option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );

                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });
                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        }
    });
});

$(document).ready(function () {
    $('#dt-filter-select3').dataTable({
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                var select = $('<select  class="browser-default custom-select form-control-sm"><option value="" selected>Search</option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );

                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });
                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        }
    });
});