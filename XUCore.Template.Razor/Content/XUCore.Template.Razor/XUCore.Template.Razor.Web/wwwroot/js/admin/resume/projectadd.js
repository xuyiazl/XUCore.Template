
$(function () {

    $('#btnPicture').bindUpload({
        data: 'cutorgin=true&orginsize=200x200',
        success: function (res) {
            $('#resumeProjectModel_ProjectLogo').val(res.Data.Url);
            $('#pictureView').show().val(res.Data.RootUrl);
        }
    });

    $('#data-form').parsleyForm({
        submit: function (formData) {

            $.ajax({
                url: '?handler=ResumeProjectByAdd',
                type: "POST",
                data: formData,
                success: function (result) {
                    if (result.Code == 0) {
                        show.successConfirm('操作提示', result.Message, {
                            confirm: { text: '好的', style: 'success' },
                            back: {
                                text: '关闭页面', style: 'default', action: function () {
                                    window.parent.closeTab('resume-project-add');
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