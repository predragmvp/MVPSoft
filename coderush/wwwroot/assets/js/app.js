$(function () {

	// Small and open nav
	$(".top-bar .hamburg-icon").on("click", function (e) {
		$(this).toggleClass('active');
		if ($(window).width() > 1024) {
			$("#wrapper").toggleClass("small-nav");
			$(".sub-menu").slideUp(300);
		}
		else {
			$(".main-header").toggleClass('active');
		}
	});

	// Open sub nav
	$(".nav-item .nav-link").on("click", function (e) {
		if ($(this).hasClass("has-submenu")) {
			$(this).next(".sub-menu").slideToggle(300);
			$(this).toggleClass('open');
			$(this).parent().siblings().find(".sub-menu").slideUp(300);
			$(this).parent().siblings().find(".has-submenu").removeClass('open');
		}
	});

	// Add Class active on keyup search input
	$(".search-area > input").on("keyup", function () {
		var inputValue = $(this).val();
		if (inputValue.length > 0) {
			$(this).addClass("active");
		}
		else {
			$(this).removeClass("active");
		}
	});

	// Show clear icon when user type something in input box
	$(".clear-icon").on("click", function () {
		$(this).siblings("input").removeClass("active").val("").attr("placeholder", "Search something special");
	});

	// Show inputbox when user click on search icon in tablet version
	$(".search-icon .mdi").on("click", function () {
		if ($(window).width() < 991) {
			$(this).parents('.search-area').toggleClass('show-input');
			$(this).parent().siblings("input").removeClass("active").val("").attr("placeholder", "Search something special");
		}
	});

	// Show three dots icon from mobile version open user specific menu like notification, message
	$(".three-dots").on("click", function () {
		$(this).toggleClass('active');
		$(this).siblings('.notifications').toggleClass('show-bar');
		$(this).children().toggleClass("mdi-close").toggleClass("mdi-dots-vertical");
	});

	// Open page settings
	$(".open-btn").on('click', function () {
		$(this).parents('.settings').addClass('settings-open')
	});

	// Close page settings
	$(".close-btn").on('click', function () {
		$(this).parents('.settings').removeClass('settings-open')
	})

	// Change navbar background color
	$(".theme-settings-inner .colors li").on('click', function () {
		$(this).addClass('active').siblings().removeClass('active');
		let dataClass = $(this).attr('class').split(" ")[0];
		$('header').removeClass().addClass("top-bar flex-row align-items-center " + dataClass);
	});

	// Change sidebar background color
	$(".theme-settings-inner .sidebar-colors li").on('click', function () {
		$(this).addClass('active').siblings().removeClass('active');
		let dataClass = $(this).attr('class').split(" ")[0];
		$('.main-header').removeClass().addClass('main-header ' + dataClass);
		if (dataClass != 'bg-white') {
			$('header > .navbar-header').removeClass().addClass('navbar-header ' + dataClass);
		}
		else {
			$('header > .navbar-header').removeClass().addClass('navbar-header');
		}
	})

	// Fixed navbar
	$(".theme-settings-layout .switcher-input").on("change", function () {
		$("body").removeClass("fixed-layout").toggleClass("fixed-header");
	});

	// Fixed and static navbar and sidebar
	$(".theme-settings-layout .dropdown-menu > a").on("click", function () {
		let attrVal = $(this).attr("val");
		$(this).parents(".btn-group").children().first().text($(this).text());
		$(this).addClass("active").siblings().removeClass("active");
		$("body").removeClass("fixed-header");
		$(".switcher .switcher-input").prop("checked", false);
		if (attrVal === 'fixed') {
			$("body").addClass("fixed-layout");
		}
		else {
			$("body").removeClass("fixed-layout");
		}
	});

	// Ripple effect
	$(".waves-effect").on("click",function(e){
		var X = e.pageX - $(this).offset().left;
		var Y = e.pageY - $(this).offset().top;
		var rippleDiv = document.createElement("div");
		rippleDiv.classList.add('ripple');
		rippleDiv.setAttribute("style","top:"+Y+"px; left:"+X+"px;");
		rippleDiv.style.background = "#ccc";
		this.appendChild(rippleDiv);
		setTimeout(function(){
			rippleDiv.parentElement.removeChild(rippleDiv);
		}, 900);
	});

}(jQuery));