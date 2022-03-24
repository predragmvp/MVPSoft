// Add active class when blur on material form
$(function () {
	$(".form-material.non-static .form-control").on("blur", function () {
		if ($(this).val().length > 0) {
			$(this).addClass("focused");
		}
	});
})