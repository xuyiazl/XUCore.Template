
$(function () {

    $('.col-md-3').click(function () {
        var css = $('i', this).attr('class');
        $('#Menus_Icon').val(css);
        $('#myModal').modal('hide');
    })
    $('.color').click(function () {
        var color = $(this).attr('data-color');
        $('#Menus_Icon').val($('#Menus_Icon').val() + ' ' + color);
    })

    $('#data-form').parsleyForm({
        submit: function (formData) {
            formData['Menus.Id'] = id;
            $.ajax({
                url: '?handler=MenuByUpdateRow',
                type: "PUT",
                data: formData,
                success: function (result) {
                    if (result.Code == 0) {
                        show.successConfirm('操作提示', result.Message, {
                            confirm: { text: '确认', style: 'success' },
                            back: {
                                text: '关闭页面', style: 'default', action: function () {
                                    window.parent.closeTab('admin-menu-update-' + id);
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