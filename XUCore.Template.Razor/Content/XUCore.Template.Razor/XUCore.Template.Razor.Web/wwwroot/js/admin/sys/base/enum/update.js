
$(function () {
    $('#Enum_ParentGuid').change(function () {
        var label = $(this).find("option:selected").parent().attr('label');
        if (label)
            $('#Enum_Code').val(label);
    });
    $('.color').click(function () {
        var color = $(this).attr('data-color');
        $('#Enum_Css').val(color);
    })
    $('#data-form').parsleyForm({
        submit: function (formData) {
            formData['Enum.Id'] = id;
            $.ajax({
                url: '?handler=BaseEnumByUpdateRow',
                type: "PUT",
                data: formData,
                success: function (result) {
                    if (result.Code == 0) {
                        show.successConfirm('操作提示', result.Message, {
                            confirm: { text: '确认', style: 'success' },
                            back: {
                                text: '关闭页面', style: 'default', action: function () {
                                    window.parent.closeTab('base-enum-update-' + id);
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