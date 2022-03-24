// Browse File
$(function () {
	$(".file-upload-browse").on("click", function () {
		var file = $(this).parents('.form-group').find('.file-upload-default');
		file.trigger('click');
	});

	$(".file-upload-default").on("change", function () {
		$(this).parent().find('.form-control').val($(this).val().replace(/C:\\fakepath\\/i, ''));
	});
});