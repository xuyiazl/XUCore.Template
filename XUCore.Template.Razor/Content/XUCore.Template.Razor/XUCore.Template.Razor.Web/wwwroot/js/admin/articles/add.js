
$(function () {

    $('#btnPicture').bindUpload({
        data: 'cutorgin=true&autocutsize=600',
        success: function (res) {
            $('#ArticleModel_Picture').val(res.Data.Url);
            $('#pictureView').show().val(res.Data.RootUrl);
        }
    });

    var editor;
    if (editorMode == 'tinymce')
        tinymceInit('#ArticleModel_Contents');
    else
        editor = editormdInit('Contents');

    $('#data-form').parsleyForm({
        submit: function (formData) {

            formData['ArticleModel.Editor'] = editorMode;

            if (editorMode == 'tinymce')
                formData['ArticleModel.Contents'] = tinymce.get('ArticleModel_Contents').getContent();
            else
                formData['ArticleModel.Contents'] = editor.getMarkdown();

            $.ajax({
                url: '?handler=ArticleByAdd',
                type: "POST",
                data: formData,
                success: function (result) {
                    if (result.Code == 0) {
                        show.successConfirm('操作提示', result.Message, {
                            confirm: { text: '好的', style: 'success' },
                            back: {
                                text: '关闭页面', style: 'default', action: function () {
                                    window.parent.closeTab('article-add-' + editorMode);
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

    $('#categoryModel').on('hide.bs.modal', function () {
        $.ajax({
            url: '?handler=CategoryList',
            type: "GET",
            success: function (result) {
                if (result.Code == 0) {
                    var obj = $("#ArticleModel_CategoryId");
                    obj.get(0).length = 0;
                    for (var ndx = 0; ndx < result.Data.length; ndx++) {
                        var item = result.Data[ndx];
                        obj.append('<option value="' + item.Item1 + '">' + item.Item2 + '</option>');
                    }
                    obj.trigger("chosen:updated");
                } else {
                    show.error('操作提示', result.Message);
                }
            }
        });
    });

    $('#tagModel').on('hide.bs.modal', function () {
        $.ajax({
            url: '?handler=TagList',
            type: "GET",
            success: function (result) {
                if (result.Code == 0) {
                    var obj = $("#ArticleModel_TagIds");
                    obj.get(0).length = 0;
                    for (var ndx = 0; ndx < result.Data.length; ndx++) {
                        var item = result.Data[ndx];
                        obj.append('<option value="' + item.Item1 + '">' + item.Item2 + '</option>');
                    }
                    obj.trigger("chosen:updated");
                } else {
                    show.error('操作提示', result.Message);
                }
            }
        });
    });
});