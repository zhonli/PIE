angular.module('pie').factory('clipboard', ['$document', '$timeout', function ($document, $timeout) {
    function createNode(text) {
        var node = $document[0].createElement('textarea');
        node.style.position = 'relative';
        //node.style.left = '-10000px';
        node.textContent = text;
        return node;
    }
    function createMessage() {
        var message = $document[0].createElement('span');
        message.innerHTML = '<span class="lrs-clipboard-message">Copied!</span>';
        return message;
    }
    function copyNode(node) {
        var selection = $document[0].getSelection();
        selection.removeAllRanges();
        node.select();
        $document[0].execCommand('copy')
        //if(!$document[0].execCommand('copy')) {
        //    throw('failure copy');
        //}
        selection.removeAllRanges();

    }

    function copyText(text, element) {
        var node = createNode(text);
        $document[0].body.appendChild(node);
        copyNode(node);
        $document[0].body.removeChild(node);
        element[0].style.position = 'relative';
        var message = createMessage();
        element[0].appendChild(message);
        $timeout(function () {
            element[0].removeChild(message);
        }, 500)

    }

    return {
        copyText: copyText
    };
}])