$(document).ready(function () {
    const removeRags = ['script', 'link', 'style', 'img', 'div', 'iframe', 'form', 'input', 'select', 'textarea', 'button',
        'a', 'option', 'table', 'video', 'audio'];
    const allowButtons = [['viewHTML'], ['undo', 'redo'], ['strong', 'em', 'del'], ['superscript', 'subscript'],
        ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
        ['unorderedList', 'orderedList'], ['horizontalRule']]

    $('#descriptionEditor').trumbowyg({
        btns: allowButtons,
        tagsToRemove: removeRags,
        autogrow: true
    });
});