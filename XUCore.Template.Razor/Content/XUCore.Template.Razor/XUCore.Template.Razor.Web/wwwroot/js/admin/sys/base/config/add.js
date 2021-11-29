
$(function () {
    $('#Config_ParentGuid').change(function () {
        var label = $(this).find("option:selected").parent().attr('label');
        if (label)
            $('#Config_Code').val(label);
    });
    $('#data-form').parsleyForm({
        submit: function (formData) {
            $.ajax({
                url: '?handler=BaseConfigByAdd',
                type: "POST",
                data: formData,
                success: function (result) {
                    if (result.Code == 0) {
                        show.successConfirm('操作提示', result.Message, {
                            confirm: { text: '确认', style: 'success' },
                            back: {
                                text: '关闭页面', style: 'default', action: function () {
                                    window.parent.closeTab('base-config-add');
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