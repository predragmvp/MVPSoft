$(function () {
  new ClipboardJS('.btn-clipboard').on('success', function(e) {
    alert("action:"+ e.action + '    text:' + e.text)
  });

  new ClipboardJS('.btn-clipboard').on('error', function(e) {
    alert(e.text)
  });
});