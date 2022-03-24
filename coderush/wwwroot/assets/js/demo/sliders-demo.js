$(function () {
	var rangeTwoObj = {
		min: 100,
		max: 1000,
		from: 550
	}
	var rangeThreeObj = {
		type: "double",
		grid: true,
		min: 0,
		max: 1000,
		from: 200,
		to: 800,
		prefix: "$"
	}
	var rangeFourObj = {
		type: "double",
		min: 100,
		max: 200,
		from: 145,
		to: 155,
		prefix: "Weight: ",
		postfix: " million pounds",
		decorate_both: true
	}
	var rangeFiveObj = {
		type: "double",
		min: 1000,
		max: 2000,
		from: 1200,
		to: 1800,
		hide_min_max: true,
		hide_from_to: true,
		grid: false
	}
	var rangeSixObj = {
		type: "double",
		min: 1000,
		max: 2000,
		from: 1200,
		to: 1800,
		hide_min_max: true,
		hide_from_to: true,
		grid: true
	}
	var rangeSevenObj = {
		type: "double",
		grid: true,
		min: 0,
		max: 10000,
		from: 1000,
		prefix: "$"
	}
	var rangeEightObj = {
		type: "single",
		grid: true,
		min: -90,
		max: 90,
		from: 0,
		postfix: "Ã‚Â°"
	}
	var advanceRangeObj1 = {
		type: "double",
		min: 0,
		max: 10000,
		grid: true
	}
	var advanceRangeObj2 = {
		type: "double",
		min: 0,
		max: 10000,
		grid: true,
		grid_num: 10
	}
	var advanceRangeObj3 = {
		type: "double",
		min: 0,
		max: 10000,
		step: 500,
		grid: true,
		grid_snap: true
	}
	var advanceRangeObj4 = {
		type: "single",
		min: 0,
		max: 10,
		step: 2.34,
		grid: true,
		grid_snap: true
	}
	var advanceRangeObj5 = {
		type: "double",
		min: 0,
		max: 100,
		from: 30,
		to: 70,
		from_fixed: true
	}
	var advanceRangeObj6 = {
		min: 0,
		max: 100,
		from: 30,
		from_min: 10,
		from_max: 50
	}
	var advanceRangeObj7 = {
		min: 0,
		max: 100,
		from: 30,
		from_min: 10,
		from_max: 50,
		from_shadow: true
	}
	var advanceRangeObj8 = {
		type: "double",
		min: 0,
		max: 100,
		from: 20,
		from_min: 10,
		from_max: 30,
		from_shadow: true,
		to: 80,
		to_min: 70,
		to_max: 90,
		to_shadow: true,
		grid: true,
		grid_num: 10
	}
	var advanceRangeObj9 = {
		min: 0,
		max: 100,
		from: 30,
		disable: true
	}

	function slider(id, start, direction = null) {
		if ($("#" + id).length) {
			var startSlider = document.getElementById(id);
			noUiSlider.create(startSlider, {
				start: [start],
				connect: [true, false],
				orientation: direction ? direction : "horizontal",
				range: {
					'min': [0],
					'max': [100]
				}
			});
		}
	}

	function connected(id, start) {
		if ($("#" + id).length) {
			$(function () {
				var skipSlider = document.getElementById(id);
				noUiSlider.create(skipSlider, {
					connect: true,
					range: {
						'min': 0,
						'10%': 10,
						'20%': 20,
						'30%': 30,
						// Nope, 40 is no fun.
						'50%': 50,
						'60%': 60,
						'70%': 70,
						// I never liked 80.
						'90%': 90,
						'max': 100
					},
					snap: true,
					start: start
				});
				var skipValues = [
					document.getElementById('skip-value-lower-2'),
					document.getElementById('skip-value-upper-2')
				];
				// skipSlider.noUiSlider.on('update', function (values, handle) {
				//   skipValues[handle].innerHTML = values[handle];
				// });
			});
		}
	}

	function valueRange(id, vId, range = null) {
		if ($("#" + id).length) {
			var bigValueSlider = document.getElementById(id),
				bigValueSpan = document.getElementById(vId);

			noUiSlider.create(bigValueSlider, {
				start: 1,
				step: 0,
				range: {
					min: 0,
					max: 14
				}
			});
			var range = range ? range : [
				'0', '2', '3', '4', '5', '6', '7', '8', '9', '10', '11', '12', '13', '14', '15'
			];
			bigValueSlider.noUiSlider.on('update', function (values, handle) {
				bigValueSpan.innerHTML = range[Math.floor(values)];
			});
		}
	}

	function skipStep(id) {
		if ($("#" + id).length) {
			var skipSlider = document.getElementById(id);
			noUiSlider.create(skipSlider, {
				range: {
					'min': 0,
					'10%': 10,
					'20%': 20,
					'30%': 30,
					// Nope, 40 is no fun.
					'50%': 50,
					'60%': 60,
					'70%': 70,
					// I never liked 80.
					'90%': 90,
					'max': 100
				},
				snap: true,
				start: [20, 90]
			});
			var skipValues = [
				document.getElementById('skip-value-lower'),
				document.getElementById('skip-value-upper')
			];

			skipSlider.noUiSlider.on('update', function (values, handle) {
				skipValues[handle].innerHTML = values[handle];
			});
		}
	}
	function toolTipSlider(id, start) {
		if ($("#" + id).length) {
			var softSlider = document.getElementById(id);

			noUiSlider.create(softSlider, {
				start: start,
				tooltips: true,
				connect: true,
				range: {
					min: 0,
					max: 100
				},
				pips: {
					mode: 'values',
					values: [0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100],
					density: 10
				}
			});
		}
	}

	function rangeSlider(id, obj = {}) {
		$("#" + id).ionRangeSlider(obj ? obj : '');
	}

	function advanceRangeSlider(id, obj = {}) {
		$("#" + id).ionRangeSlider(obj ? obj : '');
	}

	// UI Range Slider Method Call
	slider('ul-slider-1', 72);
	slider('ul-slider-2', 92);
	slider('ul-slider-3', 50);
	slider('ul-slider-4', 20);
	slider('ul-slider-5', 70);
	slider('ul-slider-6', 35);
	slider('ul-slider-7', 60);
	slider('ul-slider-8', 10, "vertical");
	slider('ul-slider-9', 15, "vertical");
	slider('ul-slider-10', 50, "vertical");
	slider('ul-slider-11', 20, "vertical");
	slider('ul-slider-12', 70, "vertical");
	slider('ul-slider-13', 35, "vertical");
	slider('ul-slider-14', 60, "vertical");

	// Skip Step Method Call
	connected('skipstep-connect', [20, 90])
	connected('skipstep-connect1', [20, 90])
	skipStep('skipstep');

	// Value Range Method Call
	valueRange('value-range', 'huge-value');

	// Tooltip Slider Range Method Call
	toolTipSlider('soft-limit', [24, 50]);
	toolTipSlider('soft-limit1', [20, 60]);
	toolTipSlider('soft-limit2', [24, 82]);

	// Range Slider Method Call
	rangeSlider('range_01')
	rangeSlider('range_02', rangeTwoObj);
	rangeSlider('range_03', rangeThreeObj);
	rangeSlider('range_04', rangeFourObj);
	rangeSlider('range_05', rangeFiveObj);
	rangeSlider('range_06', rangeSixObj);
	rangeSlider('range_07', rangeSevenObj);
	rangeSlider('range_08', rangeEightObj);

	// Advance Range Slider Method Call
	advanceRangeSlider('range_09', advanceRangeObj1);
	advanceRangeSlider('range_10', advanceRangeObj2);
	advanceRangeSlider('range_11', advanceRangeObj3);
	advanceRangeSlider('range_12', advanceRangeObj4);
	advanceRangeSlider('range_13', advanceRangeObj5);
	advanceRangeSlider('range_14', advanceRangeObj6);
	advanceRangeSlider('range_15', advanceRangeObj7);
	advanceRangeSlider('range_16', advanceRangeObj8);
	advanceRangeSlider('range_17', advanceRangeObj9);

});