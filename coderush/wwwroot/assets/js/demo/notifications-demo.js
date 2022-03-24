// Growl Notification Calls
$(function () {
	$(".growls-btn > button").on("click", function () {
		var id = $(this).attr("id");
		var str = ` <div class="growl bg-${id}">
       <div class="growl-close">&times;</div>
       <div class="growl-title">Error!</div>
       <div class="growl-message">The kitten is attacking!</div>
   </div>`;
		$("#growl-container").append(str);
		setTimeout(function () {
			$("#growl-container .growl").each(function () {
				$(this).addClass("gone");
				var _this = this;
				setTimeout(function () {
					$(_this).remove();
				}, 500);
			});
		}, 15000);
	});

	$(document).on("click","#growl-container .growl-close", function() {
		var _this = this;
		$(this).parent().addClass("gone");
		setTimeout(function () {
			$(_this).parent().remove();
		}, 500);
	});
});