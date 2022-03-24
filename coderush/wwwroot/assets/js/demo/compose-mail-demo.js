$(function () {
    $('.summernote').summernote({
        disableResizeEditor: true
    });

    $(".to > a").on("click", function () {
        $(this).children().toggleClass('mdi-chevron-down').toggleClass("mdi-chevron-up");
        $(this).parent().siblings().slideToggle();
    });
});