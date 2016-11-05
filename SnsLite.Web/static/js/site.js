'use strict';

var Message = {
    alert: function (message) {
        Dialog.model({ title: '提示', width: 400, body: message });
    },
    confirm: function (message, callback) {

    },
    handle: function (data, callback, msgid) {
        if (msgid) { $('#' + msgid).html(''); }
        if ($.type(data) == 'string') {
            Dialog.showModel(data);
        } else {
            if (!data.IsValid) {
                Message.showError(msgid, data.Message);
            } else {
                if (data.Message != '') {
                    Message.showSuccess(msgid, data.Message);
                }
                if (callback) { callback(data.Data); }
            }
        }
    },
    showError: function (msgid, message) {
        if (msgid) {
            $('#' + msgid).attr('class', 'text-danger').html(message);
        } else {
            Message.alert(message);
        }
    },
    showSuccess: function (msgid, message) {
        if (msgid) {
            $('#' + msgid).attr('class', 'text-success').html(message);
        } else {
            Message.alert(message);
        }
    }
};

var Dialog = {
    model: function (option) {
        var styleWidth = option.width ? ' style="width:' + option.width + 'px;"' : '';
        var html = '<div class="modal-dialog"' + styleWidth + '>';
        html += '    <div class="modal-content">';
        html += '        <div class="modal-header">';
        html += '            <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>';
        html += '            <h4 class="modal-title">' + option.title + '</h4>';
        html += '        </div>';
        html += '        <div class="modal-body">';
        if (option.body) {
            html += '            ' + option.body;
        }
        html += '        </div>';
        html += '        <div class="modal-footer">';
        if (option.footer) {
            html += '            ' + option.footer;
        }
        html += '            <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>';
        html += '        </div>';
        html += '    </div>';
        html += '</div>';
        Dialog.showModel(html);
    },
    showModel: function (html) {
        $('#myModal').html(html);
        $('#myModal').modal('show');
    }
};