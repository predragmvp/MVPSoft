// Multiple Code Mode Call
$(function () {
	//Use this method of there are multiple codes with same properties
	if ($('.multiple-codes').length) {
    var code_type = '';
    var editorTextarea = $('.multiple-codes');
    for (var i = 0; i < editorTextarea.length; i++) {
      $(editorTextarea[i]).attr('id', 'code-' + i);
      CodeMirror.fromTextArea(document.getElementById('code-' + i), {
        mode: "javascript",
        theme: "ambiance",
        lineNumbers: true,
        readOnly: "nocursor",
        maxHighlightLength: 0,
        workDelay: 0
      });
    }
  }
});

// Shell Mode Call
$(function () {
	if ($('.shell-mode').length) {
		var code_type = '';
		var shellEditor = $('.shell-mode');
		for (var i = 0; i < shellEditor.length; i++) {
			$(shellEditor[i]).attr('id', 'code-' + i);
			CodeMirror.fromTextArea(document.getElementById('code-' + i), {
				mode: "shell",
				theme: "ambiance",
				readOnly: "nocursor",
				maxHighlightLength: 0,
				workDelay: 0
			});
		}
	}
});

// Bootstrap Tooltip Plugin Call
$(function () {
	$('[data-toggle="tooltip"]').tooltip()
	$('[data-toggle="tooltip"]').on('shown.bs.tooltip', function () {
		var attr = $(this).attr("data-state")

		if (typeof attr !== typeof undefined && attr !== false) {
			$(".tooltip").removeClass("primary danger dark info warning success secondary").addClass(attr)
		}
	});
});

$(function () {
	// The function actually applying the offset
	function offsetAnchor() {
		if (location.hash.length !== 0) {
			$("html").animate({ scrollTop: $(location.hash).offset().top - 140 }, 500);
		}
	}

	// Captures click events of all <a> elements with href starting with #
	$(".list-arrow a").on("click", function (event) {
		// Click events are captured before hashchanges. Timeout
		// causes offsetAnchor to be called after the page jump.
		window.setTimeout(function () {
			offsetAnchor();
		}, 0);
	});

	// Set the offset when entering page with hash present in the url
	window.setTimeout(offsetAnchor, 0);
});