
$(function () {

    $('#btnPicture').bindUpload({
        data: 'cutorgin=true&orginsize=400x400',
        success: function (res) {
            $('#resumeModel_Picture').val(res.Data.Url);
            $('#pictureView').show().val(res.Data.RootUrl);
        }
    });

    $('#data-form').parsleyForm({
        submit: function (formData) {

            formData['resumeModel.Id'] = id;

            $.ajax({
                url: '?handler=ResumeByUpdateRow',
                type: "PUT",
                data: formData,
                success: function (result) {
                    if (result.Code == 0) {
                        show.successConfirm('操作提示', result.Message, {
                            confirm: { text: '好的', style: 'success' },
                            back: {
                                text: '关闭页面', style: 'default', action: function () {
                                    window.parent.closeTab('resume-update-' + id);
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