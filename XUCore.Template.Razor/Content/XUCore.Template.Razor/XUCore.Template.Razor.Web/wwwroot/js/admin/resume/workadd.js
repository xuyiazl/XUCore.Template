
$(function () {

    $('#btnPicture').bindUpload({
        data: 'cutorgin=true&orginsize=200x200',
        success: function (res) {
            $('#resumeWorkModel_CompnayLogo').val(res.Data.Url);
            $('#pictureView').show().val(res.Data.RootUrl);
        }
    });

    $('#data-form').parsleyForm({
        submit: function (formData) {

            $.ajax({
                url: '?handler=ResumeWorkByAdd',
                type: "POST",
                data: formData,
                success: function (result) {
                    if (result.Code == 0) {
                        show.successConfirm('操作提示', result.Message, {
                            confirm: { text: '好的', style: 'success' },
                            back: {
                                text: '关闭页面', style: 'default', action: function () {
                                    window.parent.closeTab('resume-work-add');
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