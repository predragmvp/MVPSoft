// Smart Widget Plugin Call
$(function () {
	$('.smartwizard-example').smartWizard({
		autoAdjustHeight: false,
		backButtonSupport: false,
		useURLhash: false,
		showStepURLhash: false
	});
	$('#smartwizard-3 .sw-toolbar').appendTo($('#smartwizard-3 .sw-container'));
});