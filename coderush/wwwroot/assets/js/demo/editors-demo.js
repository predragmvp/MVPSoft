// Bootstrap Markdown Editor
$(function () {
	$('#bs-markdown').markdown({
		iconlibrary: 'fa',
		footer: '<div id="md-character-footer"></div><small id="md-character-counter" class="text-muted">350 character left</small>',

		onChange: function (e) {
			var contentLength = e.getContent().length;

			if (contentLength > 350) {
				$('#md-character-counter')
					.removeClass('text-muted')
					.addClass('text-danger')
					.html((contentLength - 350) + ' character surplus.');
			} else {
				$('#md-character-counter')
					.removeClass('text-danger')
					.addClass('text-muted')
					.html((350 - contentLength) + ' character left.');
			}
		},
	});
});

// Quill Editor
$(function () {
	var toolbarOptions = [
		[{ 'font': [] }],
		[{ 'size': ['small', false, 'large', 'huge'] }],
		['bold', 'italic', 'underline', 'strike'],
		[{ 'color': [] }, { 'background': [] }],
		[{ 'script': 'sub' }, { 'script': 'super' }],      // toggled buttons

		[{ 'header': 1 }, { 'header': 2 }],
		['blockquote', 'code-block'],              // custom button values
		[{ 'list': 'ordered' }, { 'list': 'bullet' }],
		// superscript/subscript
		[{ 'indent': '-1' }, { 'indent': '+1' }],          // outdent/indent
		[{ 'direction': 'rtl' }],
		[{ 'align': [] }],           // text direction
		['link', 'image', 'video'],
		['clean']                                         // remove formatting button
	];

	new Quill('#editor', {
		modules: { toolbar: toolbarOptions },
		placeholder: 'Type something',
		theme: 'snow'
	});
});

// Simple MDE Editor
$(function () {
	var simplemde = new SimpleMDE({
		autofocus: true,
		autosave: {
			enabled: true,
			uniqueId: "MyUniqueID",
			delay: 1000,
		},
		placeholder: "Type something",
		element: $("#simpleMde")[0],

	});
});