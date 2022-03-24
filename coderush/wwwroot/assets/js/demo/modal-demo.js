// Modal Demo Call
$(function () {
    $(".modal-size input").on("change", function () {
        var modalClass = $(this).attr("data-class");
        var target = $(this).parents(".modal-size").find(".btn").attr("data-target");
        console.log(target);
        $(target).find(".modal-dialog").removeClass().addClass("modal-dialog " + modalClass);
    });
});