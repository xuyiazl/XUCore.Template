
$(function () {
    $('#Category_ParentGuid').change(function () {
        var label = $(this).find("option:selected").parent().attr('label');
        if (label)
            $('#Category_Code').val(label);
    });
    $('.color').click(function () {
        var color = $(this).attr('data-color');
        $('#Category_Css').val(color);
    })

    $('#data-form').parsleyForm({
        submit: function (formData) {
            $.ajax({
                url: '?handler=BaseCategoryByAdd',
                type: "POST",
                data: formData,
                success: function (result) {
                    if (result.Code == 0) {
                        show.successConfirm('操作提示', result.Message, {
                            confirm: { text: '确认', style: 'success' },
                            back: {
                                text: '关闭页面', style: 'default', action: function () {
                                    window.parent.closeTab('base-category-add');
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