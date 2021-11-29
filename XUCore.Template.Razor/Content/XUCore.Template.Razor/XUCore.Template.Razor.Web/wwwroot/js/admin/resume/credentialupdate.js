
$(function () {

    $('#data-form').parsleyForm({
        submit: function (formData) {

            formData['resumeCredentialModel.Id'] = id;

            $.ajax({
                url: '?handler=ResumeCredentialByUpdateRow',
                type: "PUT",
                data: formData,
                success: function (result) {
                    if (result.Code == 0) {
                        show.successConfirm('操作提示', result.Message, {
                            confirm: { text: '好的', style: 'success' },
                            back: {
                                text: '关闭页面', style: 'default', action: function () {
                                    window.parent.closeTab('resume-credential-update-' + id);
                                }
                            },
                        });
                    } else {
                        show.error('操作提示', result.Message);
                    }
                }
            });
        }
    });
});